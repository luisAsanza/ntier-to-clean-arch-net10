using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace CleanCRUDSolution.Infrastructure.FileReader
{
    public class XlsxSafetyValidator : IXlsxSafetyValidator
    {
        // Tune these based on your needs. Since you already limit upload size,
        // these are mostly "zip bomb / corruption" guards.
        private const int MaxEntries = 5_000;
        private const long MaxTotalUncompressedBytes = 50L * 1024 * 1024; // 50 MB
        private const double MaxCompressionRatio = 200.0; // suspiciously high => possible zip bomb
        private readonly ILogger<XlsxSafetyValidator> _logger;

        public XlsxSafetyValidator(ILogger<XlsxSafetyValidator> logger)
        {
            _logger = logger;
        }

        public async Task ValidateXlsxContainerAsync(Stream stream, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(stream);
            if (!stream.CanRead) throw new InvalidDataException("Stream is not readable.");

            // Reset to start if possible
            if (stream.CanSeek) stream.Position = 0;

            // 1) Verify ZIP signature: xlsx is a ZIP file (PK..)
            var header = new byte[4];
            var read = await stream.ReadAsync(header.AsMemory(0, 4), ct);

            if (read < 4 || header[0] != (byte)'P' || header[1] != (byte)'K')
                throw new InvalidDataException("File is not a valid .xlsx container (missing ZIP signature).");

            // Reset for ZipArchive
            if (stream.CanSeek) stream.Position = 0;

            // 2) Open ZIP safely
            ZipArchive archive;
            try
            {
                archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true);
            }
            catch (InvalidDataException ex)
            {
                _logger.LogWarning(ex, "Failed to open ZIP archive from stream (corrupt or wrong format).");
                throw new InvalidDataException("File is not a valid ZIP container (corrupt or wrong format).", ex);
            }

            using (archive)
            {
                // 3) Basic ZIP sanity
                if (archive.Entries.Count == 0)
                    throw new InvalidDataException("ZIP container has no entries.");

                if (archive.Entries.Count > MaxEntries)
                    throw new InvalidDataException($"ZIP has too many entries ({archive.Entries.Count}).");

                // 4) Required OpenXML parts for XLSX
                // These are strongly indicative of an XLSX package.
                bool hasContentTypes = archive.GetEntry("[Content_Types].xml") is not null;
                bool hasWorkbook = archive.GetEntry("xl/workbook.xml") is not null;

                if (!hasContentTypes || !hasWorkbook)
                    throw new InvalidDataException("File does not appear to be a valid .xlsx (missing required OpenXML parts).");

                // 5) Zip bomb / resource abuse guard
                // ZipArchiveEntry.Length is the uncompressed size.
                // ZipArchiveEntry.CompressedLength is compressed size.
                long totalUncompressed = 0;

                foreach (var entry in archive.Entries)
                {
                    ct.ThrowIfCancellationRequested();

                    // Some zip entries may have 0 sizes; still count them.
                    // Prevent runaway totals.
                    totalUncompressed += entry.Length;
                    if (totalUncompressed > MaxTotalUncompressedBytes)
                    {
                        _logger.LogWarning("Decompressed size of ZIP entries exceeds limit: {TotalUncompressed} bytes.",
                            totalUncompressed);
                        throw new InvalidDataException("File expands too large when decompressed (possible zip bomb).");
                    }

                    // Prevent suspicious compression ratios on individual entries
                    if (entry.CompressedLength > 0)
                    {
                        double ratio = (double)entry.Length / entry.CompressedLength;
                        if (ratio > MaxCompressionRatio && entry.Length > 1024 * 1024) // ratio high and sizable entry
                        {
                            _logger.LogWarning("Suspicious compression ratio detected in entry {EntryName}: {Ratio:F2}.",
                                entry.FullName, ratio);
                            throw new InvalidDataException("Suspicious compression ratio detected (possible zip bomb).");
                        }
                    }
                }
            }

            // Reset for the actual reader that will parse the file
            if (stream.CanSeek) stream.Position = 0;
        }
    }

}
