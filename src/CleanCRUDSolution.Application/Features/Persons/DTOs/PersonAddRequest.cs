using CleanCRUDSolution.Domain.Enums;

namespace CleanCRUDSolution.Application.Features.Persons.DTOs
{
    public record PersonAddRequest(
        string Name,
        string? Email,
        DateOnly? DateOfBirth,
        GenderOptions? Gender,
        Guid? CountryId,
        string? Address,
        bool ReceiveNewsLetters,
        string? TIN
    );
}
