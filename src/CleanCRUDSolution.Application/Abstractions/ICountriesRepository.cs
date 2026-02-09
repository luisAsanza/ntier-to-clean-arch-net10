using CleanCRUDSolution.Domain.Entities;

namespace CleanCRUDSolution.Application.Abstractions
{
    public interface ICountriesRepository
    {
        Task<Country> AddAsync(Country country);
        Task<Country?> GetByIdAsync(Guid countryId);
        Task<Country?> GetByCountryNameAsync(string countryName);
        Task<IReadOnlyList<Country>> GetAllAsync();
        Task<int> AddRangeAsync(IReadOnlyList<Country> countries);
        Task<HashSet<string>> GetExistingNamesAsync(IReadOnlyList<string> normalizedNames);
    }
}
