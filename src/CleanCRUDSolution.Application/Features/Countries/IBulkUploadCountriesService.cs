using CleanCRUDSolution.Application.Common.Results;
using CleanCRUDSolution.Application.Features.Countries.DTOs;

namespace CleanCRUDSolution.Application.Features.Countries
{
    /// <summary>
    /// Service abstraction for bulk uploading countries from a stream.
    /// </summary>
    public interface IBulkUploadCountriesService
    {
        Task<Result<CountryImportReport>> UploadCountriesAsync(Stream stream, CancellationToken ct);
    }
}
