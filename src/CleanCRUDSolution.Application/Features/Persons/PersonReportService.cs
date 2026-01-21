using CleanCRUDSolution.Application.Common.Enums;
using CleanCRUDSolution.Application.Common.Results;
using CleanCRUDSolution.Application.Features.Reporting;

namespace CleanCRUDSolution.Application.Features.Persons
{
    public class PersonReportService : IPersonReportService
    {
        private readonly IPersonService _personService;
        private readonly IPersonReportGeneratorFactory _personReportGeneratorFactory;

        public PersonReportService(IPersonService personService, IPersonReportGeneratorFactory personReportGeneratorFactory)
        {
            _personService = personService;
            _personReportGeneratorFactory = personReportGeneratorFactory;
        }

        public async Task<Result<MemoryStream>> GetPersonsReportAsync(ReportOptions option)
        {
            // 1. Get the data to be displayed in the report
            var persons = await _personService.GetAllPersonsAsync();

            if (persons == null || persons.Count == 0)
            {
                var error = new Error(
                    Code: "NoData",
                    Message: "No persons available to generate the report.",
                    Type: ErrorType.NotFound
                );

                return Result<MemoryStream>.Failure(error);
            }

            // 2. Get the appropriate report generator based on the option
            var generator = _personReportGeneratorFactory.GetGenerator(option);

            // 3. Generate and return the report
            var report = await generator.GenerateAllPersonsReportAsync(persons);

            return Result<MemoryStream>.Success(report);
        }
    }
}
