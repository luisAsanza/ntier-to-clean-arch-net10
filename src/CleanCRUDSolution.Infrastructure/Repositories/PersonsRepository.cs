using CleanCRUDSolution.Application.Abstractions;
using CleanCRUDSolution.Application.Common.Enums;
using CleanCRUDSolution.Application.Features.Persons.Enums;
using CleanCRUDSolution.Domain.Common.Time;
using CleanCRUDSolution.Domain.Entities;
using CleanCRUDSolution.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanCRUDSolution.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for Person entities using EF Core and the application DbContext.
    /// </summary>
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IClock _clock;
        private readonly ILogger<PersonsRepository> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="PersonsRepository"/>.
        /// </summary>
        public PersonsRepository(ApplicationDbContext db, IClock clock, ILogger<PersonsRepository> logger)
        {
            _db = db;
            _clock = clock;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new person to the database and returns the saved entity.
        /// </summary>
        public async Task<Person> AddAsync(Person person)
        {
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Person with Person Id {PersonId} added", person.Id);
            return person;
        }

        /// <summary>
        /// Deletes the provided person entity from the database.
        /// </summary>
        public async Task<bool> DeleteAsync(Person person)
        {
            _logger.LogInformation("Deleting Person with Person Id {PersonId}", person.Id);
            _db.Persons.Remove(person);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves a person by identifier including the related country navigation property.
        /// </summary>
        public async Task<Person?> GetByIdAsync(Guid personId)
        {
            _logger.LogInformation("Getting Person with Person Id {PersonId}", personId);
            return await _db.Persons.Include(t => t.Country).FirstOrDefaultAsync(p => p.Id == personId);
        }

        /// <summary>
        /// Retrieves all persons from the database.
        /// </summary>
        public async Task<IReadOnlyList<Person>> GetAllAsync()
        {
            _logger.LogInformation("Getting all Persons");
            return await _db.Persons.Include(p => p.Country).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Returns persons filtered and sorted according to the supplied parameters.
        /// </summary>
        public async Task<IReadOnlyList<Person>> GetFilteredPersonsAsync(PersonColumn? searchColumn, string? searchText, PersonColumn? sortColumn, SortOrder? sortOrder)
        {
            _logger.LogInformation("Getting filtered Persons");
            var personsQuery = _db.Persons.ApplySearch(searchColumn, searchText, _clock.GetToday(TimeZoneInfo.Local)).ApplySorting(sortColumn, sortOrder);
            return await personsQuery.Include(p => p.Country).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Ensures the provided person entity is tracked and persisted to the database.
        /// </summary>
        public async Task UpdateAsync(Person person)
        {
            // In case EF Core is not tracking the entity, start tracking it and mark it as Modified.
            _db.Persons.Update(person);
            await _db.SaveChangesAsync();
        }
    }
}
