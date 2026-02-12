
using System.Data;
using CleanCRUDSolution.Web.Models.PersonModels.Data;
using FluentValidation;

namespace CleanCRUDSolution.Web.Validators
{
    public class PersonDataVMValidator : AbstractValidator<PersonDataVM>
    {
        public PersonDataVMValidator()
        {
            RuleFor(x => x.CountryId)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Address))
            .WithMessage("Country is required when address is provided.");

            RuleFor(x => x.Address)
            .NotEmpty()
            .When(x => x.CountryId.HasValue)
            .WithMessage("Address is required when country is provided.");

            RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Date of birth must be in the past.");
        }
    }
}