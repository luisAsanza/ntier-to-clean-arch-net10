using CleanCRUDSolution.Application.Features.Persons.DTOs;
using CleanCRUDSolution.Domain.Common.Time;
using FluentValidation;
using System.Threading;

namespace CleanCRUDSolution.Application.Features.Persons.Validators
{
    public class PersonUpdateRequestValidator : AbstractValidator<PersonUpdateRequest>
    {
        public PersonUpdateRequestValidator(IClock clock)
        {
            RuleFor(p => p)
                .NotNull().WithMessage("Person update request cannot be null").WithErrorCode("person.request.null");

            RuleFor(p => p.Email)
                .EmailAddress()
                .When(p => !string.IsNullOrWhiteSpace(p.Email))
                .WithMessage("Email format is incorrect.")
                .WithErrorCode("person.email.invalid");

            RuleFor(p => p.DateOfBirth)
                .LessThanOrEqualTo(clock.GetToday(TimeZoneInfo.Local))
                .When(p => p.DateOfBirth is not null)
                .WithMessage("Date of Birth cannot be in the future")
                .WithErrorCode("person.dateofbirth.future");

            RuleFor(p => p.TIN)
                .Matches(@"^\d{5,15}$")
                .When(p => !string.IsNullOrWhiteSpace(p.TIN))
                .WithMessage("Tax Identification Number must numeric and between 5 and 15 digits.")
                .WithErrorCode("person.tin.format");
        }
    }
}
