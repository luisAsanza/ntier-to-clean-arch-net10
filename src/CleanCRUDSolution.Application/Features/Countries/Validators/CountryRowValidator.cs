using CleanCRUDSolution.Application.Features.Countries.DTOs;
using FluentValidation;

namespace CleanCRUDSolution.Application.Features.Countries.Validators
{
    /// <summary>
    /// Validates a `CountryRow` instance for required fields and length constraints.
    /// </summary>
    public class CountryRowValidator : AbstractValidator<CountryRow>
    {
        public CountryRowValidator()
        {
            RuleFor(c => c.AttemptedCountryName)
                .NotEmpty()
                .WithMessage("Country name must not be empty.")
                .WithErrorCode("countryrow.name.empty");

            RuleFor(c => c.AttemptedCountryName)
                .MaximumLength(100)
                .WithMessage("Country name must not exceed 100 characters.")
                .WithErrorCode("countryrow.name.length");
        }
    }
}
