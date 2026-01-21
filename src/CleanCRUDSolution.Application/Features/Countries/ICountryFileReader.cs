using CleanCRUDSolution.Application.Features.Countries.DTOs;

namespace CleanCRUDSolution.Application.Features.Countries
{
    public interface ICountryFileReader
    {
        Task<IReadOnlyList<CountryRow>> ReadCountries(Stream fileStream);
    }
}
