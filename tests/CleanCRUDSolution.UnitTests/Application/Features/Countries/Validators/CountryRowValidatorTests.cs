using CleanCRUDSolution.Application.Features.Countries.DTOs;
using CleanCRUDSolution.Application.Features.Countries.Validators;
using FluentAssertions;
using Xunit;

namespace CleanCRUDSolution.UnitTests.Application.Features.Countries.Validators
{
    public class CountryRowValidatorTests
    {
        [Fact]
        public void Validate_ValidName_Passes()
        {
            var validator = new CountryRowValidator();
            var row = new CountryRow(1, "Spain");
            var result = validator.Validate(row);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_EmptyName_Fails()
        {
            var validator = new CountryRowValidator();
            var row = new CountryRow(1, "");
            var result = validator.Validate(row);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_TooLong_Fails()
        {
            var validator = new CountryRowValidator();
            var longName = new string('x', 101);
            var row = new CountryRow(1, longName);
            var result = validator.Validate(row);
            result.IsValid.Should().BeFalse();
        }
    }
}
