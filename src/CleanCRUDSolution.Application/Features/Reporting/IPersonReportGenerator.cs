using CleanCRUDSolution.Application.Features.Persons.DTOs;
namespace CleanCRUDSolution.Application.Features.Reporting
{
    public interface IPersonReportGenerator
    {
        Task<MemoryStream> GenerateAllPersonsReportAsync(IEnumerable<PersonResponse> persons);
        Task<MemoryStream> GenerateAllPersonsDetailedReportAsync(IEnumerable<PersonResponse> persons);
    }
}
