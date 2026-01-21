using CleanCRUDSolution.Application.Features.Countries.DTOs;
using FluentValidation;

namespace CleanCRUDSolution.Application.Features.Countries.Validators
{
    public class CountryAddRequestListValidator : AbstractValidator<IEnumerable<CountryAddRequest>>
    {
        public CountryAddRequestListValidator(IValidator<CountryAddRequest> itemValidator)
        {
            RuleFor<IEnumerable<CountryAddRequest>>(l => l)
                .Must(x => x != null && x.Any())
                .WithMessage("Country list cannot be empty.")
                .WithErrorCode("countrylist.empty");

            RuleForEach(x => x).SetValidator(itemValidator);
        }
    }
}
