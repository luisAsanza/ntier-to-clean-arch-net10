using OfficeOpenXml;
using CleanCRUDSolution.Application.Features.Reporting;
using CleanCRUDSolution.Application.Features.Persons.DTOs;

namespace CleanCRUDSolution.Infrastructure.Reporting
{
    public class ExcelPersonReportGenerator : IPersonReportGenerator
    {
        public Task<MemoryStream> GenerateAllPersonsDetailedReportAsync(IEnumerable<PersonResponse> persons)
        {
            throw new NotImplementedException();
        }

        public async Task<MemoryStream> GenerateAllPersonsReportAsync(IEnumerable<PersonResponse> persons)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("Persons");
                workSheet.Cells["A1"].Value = "Person Name";
                workSheet.Cells["B1"].Value = "Email";
                workSheet.Cells["C1"].Value = "Date Of Birth";
                workSheet.Cells["D1"].Value = "Country";

                int row = 2;

                foreach (var person in persons)
                {
                    workSheet.Cells[row, 1].Value = person.Name;
                    workSheet.Cells[row, 2].Value = person.Email;
                    workSheet.Cells[row, 3].Value = person.DateOfBirth.HasValue ? person.DateOfBirth.Value.ToString("dd MM yyyy") : string.Empty;
                    workSheet.Cells[row, 4].Value = person.CountryName;

                    row++;
                }

                workSheet.Cells.AutoFitColumns();
                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
