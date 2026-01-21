using CleanCRUDSolution.Application.Features.Countries.DTOs;
using FluentValidation;

namespace CleanCRUDSolution.Application.Features.Countries.Validators
{
    public class CountryAddRequestValidator : AbstractValidator<CountryAddRequest>
    {
        public CountryAddRequestValidator()
        {
            RuleFor(c => c)
                .NotNull()
                .WithMessage("Country add request cannot be null.")
                .WithErrorCode("country.null");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Country name is required.")
                .MaximumLength(100).WithMessage("Country name must not exceed 100 characters.");
        }
    }
}
