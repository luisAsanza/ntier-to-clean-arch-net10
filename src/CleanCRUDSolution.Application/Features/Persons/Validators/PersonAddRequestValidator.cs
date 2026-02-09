using CleanCRUDSolution.Application.Features.Persons.DTOs;
using CleanCRUDSolution.Domain.Common.Time;
using FluentValidation;

namespace CleanCRUDSolution.Application.Features.Persons.Validators
{
    public class PersonAddRequestValidator : AbstractValidator<PersonAddRequest>
    {

        public PersonAddRequestValidator(IClock clock)
        {
            RuleFor(p => p)
                .NotNull().WithMessage("Person add request cannot be null").WithErrorCode("person.request.null");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Person name cannot be blank").WithErrorCode("person.name.required")
                .Length(2, 80).WithMessage("Person name must be between 2 and 80 characters").WithErrorCode("person.name.length");

            RuleFor(p => p.Email)
                .EmailAddress()
                .When(p => !string.IsNullOrWhiteSpace(p.Email))
                .WithMessage("Email format is incorrect.")
                .WithErrorCode("person.email.invalid");

            RuleFor(p => p.Email)
                .MaximumLength(80)
                .When(p => !string.IsNullOrWhiteSpace(p.Email))
                .WithMessage("Email cannot exceed 80 characters.")
                .WithErrorCode("person.email.length");

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

            RuleFor(p => p.Address)
                .MaximumLength(400)
                .When(p => !string.IsNullOrWhiteSpace(p.Address))
                .WithMessage("Address cannot exceed 400 characters.")
                .WithErrorCode("person.address.length");
        }
    }
}
