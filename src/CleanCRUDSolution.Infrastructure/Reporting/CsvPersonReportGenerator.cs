using CleanCRUDSolution.Application.Features.Persons.DTOs;
using CleanCRUDSolution.Application.Features.Reporting;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace CleanCRUDSolution.Infrastructure.Reporting
{
    public class CsvPersonReportGenerator : IPersonReportGenerator
    {
        public async Task<MemoryStream> GenerateAllPersonsDetailedReportAsync(IEnumerable<PersonResponse> persons)
        {
            CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
                NewLine = Environment.NewLine
            };
            MemoryStream memoryStream = new MemoryStream();
            using StreamWriter streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
            using CsvWriter csvWriter = new CsvWriter(streamWriter, configuration);
            {
                //csvWriter.WriteHeader<PersonResponse>();
                //PersonName, Email, DateOfBirth, Country
                csvWriter.WriteField(nameof(PersonResponse.Name));
                csvWriter.WriteField(nameof(PersonResponse.Email));
                csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
                csvWriter.WriteField(nameof(PersonResponse.CountryName));

                await csvWriter.NextRecordAsync();

                foreach (var person in persons)
                {
                    csvWriter.WriteField(person.Name);
                    csvWriter.WriteField(person.Email);
                    csvWriter.WriteField(person.DateOfBirth.HasValue ? person.DateOfBirth.Value.ToString("dd MM yyyy") : string.Empty);
                    csvWriter.WriteField(person.Name);
                    await csvWriter.NextRecordAsync();
                }

                //await csvWriter.WriteRecordsAsync(persons);

                await streamWriter.FlushAsync();
            }

            memoryStream.Position = 0;

            return memoryStream;
        }

        public async Task<MemoryStream> GenerateAllPersonsReportAsync(IEnumerable<PersonResponse> persons)
        {
            MemoryStream memoryStream = new MemoryStream();
            using StreamWriter streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
            using CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            {
                csvWriter.WriteHeader<PersonResponse>();
                await csvWriter.NextRecordAsync();

                await csvWriter.WriteRecordsAsync(persons);

                await streamWriter.FlushAsync();
            }

            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
