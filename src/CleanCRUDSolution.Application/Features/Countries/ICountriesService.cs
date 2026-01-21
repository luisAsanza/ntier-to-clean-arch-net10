using CleanCRUDSolution.Application.Common.Results;
using CleanCRUDSolution.Application.Features.Countries.Enums;

namespace CleanCRUDSolution.Application.Features.Countries
{
    public interface ICountriesService
    {
        Task<IReadOnlyList<CountryResponse>> GetAllCountriesAsync();
    }
}
