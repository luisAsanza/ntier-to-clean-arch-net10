namespace CleanCRUDSolution.Application.Features.Persons.DTOs
{
    public record PersonResponse(
        Guid Id,
        string Name,
        string? Email,
        DateOnly? DateOfBirth,
        string? Gender,
        Guid? CountryId,
        string? CountryName,
        string? Address,
        bool ReceiveNewsLetters,
        double? Age
    );
}
