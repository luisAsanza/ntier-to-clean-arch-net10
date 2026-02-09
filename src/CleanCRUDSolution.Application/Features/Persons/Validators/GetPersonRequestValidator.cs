using CleanCRUDSolution.Application.Features.Persons.DTOs;
using FluentValidation;

namespace CleanCRUDSolution.Application.Features.Persons.Validators
{
    public sealed class GetPersonRequestValidator : AbstractValidator<GetPersonRequest>
    {
        public GetPersonRequestValidator()
        {
            RuleFor(g => g)
                .NotNull().WithMessage("GetPersonRequest cannot be null.").WithErrorCode("getpersonrequest.null");

            RuleFor(g => g.SearchTerm)
                .MaximumLength(100)
                .WithMessage("Search term must be less than 100 characters.")
                .WithErrorCode("getpersonrequest.length");
        }
    }
}
