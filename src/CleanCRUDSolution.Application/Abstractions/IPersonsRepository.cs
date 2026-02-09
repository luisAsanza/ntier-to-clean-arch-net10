using CleanCRUDSolution.Application.Common.Enums;
using CleanCRUDSolution.Application.Features.Persons.Enums;
using CleanCRUDSolution.Domain.Entities;

namespace CleanCRUDSolution.Application.Abstractions
{
    public interface IPersonsRepository
    {
        Task<Person> AddAsync(Person person);
        Task<Person?> GetByIdAsync(Guid personId);
        Task<IReadOnlyList<Person>> GetAllAsync();
        Task<bool> DeleteAsync(Person person);
        Task UpdateAsync(Person person);
        Task<IReadOnlyList<Person>> GetFilteredPersonsAsync(PersonColumn? searchColumn, string? searchText, PersonColumn? sortColumn, SortOrder? sortOrder);
    }
}
