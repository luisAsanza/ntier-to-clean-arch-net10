using CleanCRUDSolution.Application.Common.Results;
using CleanCRUDSolution.Application.Features.Countries.Enums;

namespace CleanCRUDSolution.Application.Features.Countries
{
    /// <summary>
    /// Service abstraction for country related operations.
    /// </summary>
    public interface ICountriesService
    {
        Task<IReadOnlyList<CountryResponse>> GetAllCountriesAsync();
    }
}
