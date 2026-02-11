using System.Collections.Generic;
using CleanCRUDSolution.Application.Features.Countries.DTOs;
using CleanCRUDSolution.Application.Features.Countries.Validators;
using FluentAssertions;

using Xunit;

namespace CleanCRUDSolution.UnitTests.Application.Features.Countries.Validators
{
    public class CountryAddRequestListValidatorTests
    {
        [Fact]
        public void Validate_EmptyList_Fails()
        {
            var itemValidator = new CountryAddRequestValidator();
            var validator = new CountryAddRequestListValidator(itemValidator);
            var list = new List<CountryAddRequest>();

            var result = validator.Validate(list);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_ListWithInvalidItem_Fails()
        {
            var itemValidator = new CountryAddRequestValidator();
            var validator = new CountryAddRequestListValidator(itemValidator);
            var list = new List<CountryAddRequest> { new CountryAddRequest("") };

            var result = validator.Validate(list);
            result.IsValid.Should().BeFalse();
        }
    }
}
