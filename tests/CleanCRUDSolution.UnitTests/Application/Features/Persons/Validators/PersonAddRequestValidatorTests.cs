using System;
using CleanCRUDSolution.Application.Features.Persons.DTOs;
using CleanCRUDSolution.Application.Features.Persons.Validators;
using CleanCRUDSolution.Domain.Common.Time;
using FluentAssertions;
using Xunit;

namespace CleanCRUDSolution.UnitTests.Application.Features.Persons.Validators
{
    public class PersonAddRequestValidatorTests
    {
        private class FakeClock : IClock
        {
            public DateTime UtcNow => new DateTime(2025,1,1);
            public DateOnly GetToday(TimeZoneInfo timeZone) => new DateOnly(2025,1,1);
        }

        [Fact]
        public void Validate_ValidRequest_Passes()
        {
            var validator = new PersonAddRequestValidator(new FakeClock());
            var req = new PersonAddRequest("Alice", "a@b.com", new DateOnly(1990,1,1), null, null, null, false, "12345");
            var result = validator.Validate(req);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_NameTooShort_Fails()
        {
            var validator = new PersonAddRequestValidator(new FakeClock());
            var req = new PersonAddRequest("A", null, null, null, null, null, false, null);
            var result = validator.Validate(req);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_FutureDateOfBirth_Fails()
        {
            var validator = new PersonAddRequestValidator(new FakeClock());
            var req = new PersonAddRequest("Bob", null, new DateOnly(2026,1,1), null, null, null, false, null);
            var result = validator.Validate(req);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_InvalidTin_Fails()
        {
            var validator = new PersonAddRequestValidator(new FakeClock());
            var req = new PersonAddRequest("Carol", null, null, null, null, null, false, "12ab");
            var result = validator.Validate(req);
            result.IsValid.Should().BeFalse();
        }
    }
}
