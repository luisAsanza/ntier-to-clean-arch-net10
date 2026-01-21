using CleanCRUDSolution.Application.Abstractions;
using CleanCRUDSolution.Application.Common.Exceptions;
using CleanCRUDSolution.Domain.Entities;
using CleanCRUDSolution.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CleanCRUDSolution.Infrastructure.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {

        private readonly ApplicationDbContext _db;

        public CountriesRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Country> AddAsync(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }

        public Task<Country?> GetByIdAsync(Guid countryId)
        {
            return _db.Countries
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == countryId);
        }

        public async Task<IReadOnlyList<Country>> GetAllAsync()
        {
            return await _db.Countries
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<Country?> GetByCountryNameAsync(string countryName)
        {
            return _db.Countries
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Name.Equals(countryName, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<int> AddRangeAsync(IReadOnlyList<Country> countries)
        {
            if (countries == null || countries.Count == 0) return 0;

            try
            {
                await _db.Countries.AddRangeAsync(countries);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
                {
                    throw new DuplicateResourceException("A duplicate Country exists.");
                }

                throw;
            }

            return countries.Count;
        }

        public async Task<HashSet<string>> GetExistingNamesAsync(IReadOnlyList<string> normalizedNames)
        {
            var names = normalizedNames.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            var existing = await _db.Countries
                .AsNoTracking()
                .Where(c => names.Contains(c.Name))
                .Select(c => c.Name)
                .ToListAsync();

            return existing.ToHashSet(StringComparer.OrdinalIgnoreCase);
        }
    }
}
