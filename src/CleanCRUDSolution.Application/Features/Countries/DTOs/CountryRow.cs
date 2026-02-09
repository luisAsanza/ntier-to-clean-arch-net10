namespace CleanCRUDSolution.Application.Features.Countries.DTOs
{
    /// <summary>
    /// Represents a single row from an input file containing an attempted country name and row number.
    /// </summary>
    public record CountryRow(int RowNumber, string? AttemptedCountryName);
}
