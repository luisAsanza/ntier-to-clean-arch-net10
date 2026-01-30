using CleanCRUDSolution.Application.Abstractions;
using CleanCRUDSolution.Application.Common.Exceptions;
using CleanCRUDSolution.Domain.Entities;
using CleanCRUDSolution.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CleanCRUDSolution.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for managing <see cref="Country"/> entities.
    /// Uses <see cref="ApplicationDbContext"/> to perform data access operations.
    /// </summary>
    public class CountriesRepository : ICountriesRepository
    {

        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountriesRepository"/> class.
        /// </summary>
        /// <param name="db">The <see cref="ApplicationDbContext"/> used for data access.</param>
        public CountriesRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds a new <see cref="Country"/> to the database and saves changes.
        /// </summary>
        /// <param name="country">The country entity to add.</param>
        /// <returns>
        /// A task that represents the asynchronous add operation. The task result contains the added <see cref="Country"/>.
        /// </returns>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">
        /// Thrown when an error occurs while saving changes to the database.
        /// </exception>
        public async Task<Country> AddAsync(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }

        /// <summary>
        /// Retrieves a <see cref="Country"/> by its identifier.
        /// </summary>
        /// <param name="countryId">The unique identifier of the country.</param>
        /// <returns>
        /// A task that represents the asynchronous retrieval. The task result contains the matching <see cref="Country"/>,
        /// or <c>null</c> if no matching country is found.
        /// </returns>
        public Task<Country?> GetByIdAsync(Guid countryId)
        {
            return _db.Countries
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == countryId);
        }

        /// <summary>
        /// Retrieves all countries from the database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a read-only list of all <see cref="Country"/> entities.
        /// </returns>
        public async Task<IReadOnlyList<Country>> GetAllAsync()
        {
            return await _db.Countries
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a <see cref="Country"/> by its name using a case-insensitive comparison.
        /// </summary>
        /// <param name="countryName">The country name to search for.</param>
        /// <returns>
        /// A task that represents the asynchronous retrieval. The task result contains the matching <see cref="Country"/>,
        /// or <c>null</c> if no matching country is found.
        /// </returns>
        public Task<Country?> GetByCountryNameAsync(string countryName)
        {
            return _db.Countries
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Name.Equals(countryName, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Adds a range of countries to the database and saves changes.
        /// This method will attempt to add all provided countries in a single operation.
        /// </summary>
        /// <param name="countries">The list of countries to add.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the number of countries added.
        /// Returns <c>0</c> if the input list is null or empty.
        /// </returns>
        /// <exception cref="DuplicateResourceException">
        /// Thrown when a database constraint violation indicates a duplicate country (SQL error numbers 2627 or 2601).
        /// </exception>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">
        /// Thrown when an error occurs while saving changes to the database and it is not recognized as a duplicate key violation.
        /// </exception>
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

        /// <summary>
        /// Returns the subset of provided names that already exist in the database.
        /// Comparison is performed in a case-insensitive manner.
        /// </summary>
        /// <param name="normalizedNames">
        /// A list of country names (expected to be normalized). Duplicates in this list are ignored.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a case-insensitive <see cref="HashSet{String}"/>
        /// with the names from <paramref name="normalizedNames"/> that already exist in the database.
        /// </returns>
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
