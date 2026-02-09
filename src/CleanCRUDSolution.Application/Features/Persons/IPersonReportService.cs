using CleanCRUDSolution.Application.Common.Enums;
using CleanCRUDSolution.Application.Common.Results;

namespace CleanCRUDSolution.Application.Features.Persons
{
    public interface IPersonReportService
    {
        Task<Result<MemoryStream>> GetPersonsReportAsync(ReportOptions option);
    }
}
