using CleanCRUDSolution.Application.Features.Countries;
using CleanCRUDSolution.Application.Features.Countries.DTOs;
using OfficeOpenXml;

namespace CleanCRUDSolution.Infrastructure.FileReader
{
    public class CountryExcelFileReader : ICountryFileReader
    {
        private readonly IXlsxSafetyValidator _validator;
        public CountryExcelFileReader(IXlsxSafetyValidator validator)
        {
            _validator = validator;
        }
        public async Task<IReadOnlyList<CountryRow>> ReadCountries(Stream fileStream)
        {
            // Validate input
            await _validator.ValidateXlsxContainerAsync(fileStream);

            if (fileStream.CanSeek) fileStream.Position = 0;
            var result = new List<CountryRow>();

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                await excelPackage.LoadAsync(fileStream);
                var worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null || worksheet.Dimension == null) return result;
                var rowCount = worksheet.Dimension.Rows;
                const int headerRow = 1;

                for (var row = headerRow + 1; row <= rowCount; row++)
                {
                    var cellValue = worksheet.Cells[row, 1].Value?.ToString();

                    //Add validated country to the list
                    if (!string.IsNullOrWhiteSpace(cellValue))
                    {
                        result.Add(new CountryRow(row, cellValue));
                    }
                }
            }

            return result;
        }
    }
}
