using CleanCRUDSolution.Application.Common.Results;
using CleanCRUDSolution.Application.Features.Persons.DTOs;

namespace CleanCRUDSolution.Application.Features.Persons
{
    public interface IPersonService
    {
        Task<Result<PersonResponse>> AddPersonAsync(PersonAddRequest request);
        Task<Result> UpdatePersonAsync(PersonUpdateRequest request);
        Task<IReadOnlyList<PersonResponse>> GetAllPersonsAsync();
        Task<Result<PersonResponse>> GetPersonAsync(Guid personId);
        Task<Result<IReadOnlyList<PersonResponse>>> GetFilteredPersonsAsync(GetPersonRequest request);
        Task<Result> DeletePersonAsync(Guid personId);
    }
}
