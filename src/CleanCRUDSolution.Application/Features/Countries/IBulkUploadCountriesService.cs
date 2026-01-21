using CleanCRUDSolution.Application.Common.Results;
using CleanCRUDSolution.Application.Features.Countries.DTOs;

namespace CleanCRUDSolution.Application.Features.Countries
{
    public interface IBulkUploadCountriesService
    {
        Task<Result<CountryImportReport>> UploadCountriesAsync(Stream stream, CancellationToken ct);
    }
}
