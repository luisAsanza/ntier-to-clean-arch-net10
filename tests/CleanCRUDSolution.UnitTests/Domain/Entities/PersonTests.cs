using System;
using CleanCRUDSolution.Domain.Entities;
using CleanCRUDSolution.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace CleanCRUDSolution.UnitTests.Domain.Entities
{
    public class PersonTests
    {
        [Fact]
        public void Ctor_WithValidName_SetsNameAndDefaults()
        {
            var person = new Person(" Alice ");

            person.Name.Should().Be("Alice");
            person.ReceiveNewsLetters.Should().BeFalse();
            person.Email.Should().BeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Ctor_WithNullOrWhitespace_ThrowsArgumentException(string? invalid)
        {
            Action act = () => new Person(invalid!);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void UpdateDateOfBirth_WithFutureDate_ThrowsArgumentOutOfRangeException()
        {
            var person = new Person("Bob");
            var today = new DateOnly(2025, 1, 1);
            var future = today.AddDays(1);

            Action act = () => person.UpdateDateOfBirth(future, today);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void MoveTo_WithEmptyGuid_ThrowsArgumentException()
        {
            var person = new Person("Cindy");
            Action act = () => person.MoveTo(Guid.Empty, "123 Main St");

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void UpdateTin_WithTooShort_ThrowsInvalidTinFormatException()
        {
            var person = new Person("David");

            Action act = () => person.UpdateTin("123");

            act.Should().Throw<InvalidTinFormatException>();
        }

        [Fact]
        public void UpdateTin_WithNull_SetsNull()
        {
            var person = new Person("Eve");
            person.UpdateTin(null);
            person.TIN.Should().BeNull();
        }

        [Fact]
        public void UpdateTin_WithEmptyString_ThrowsArgumentException()
        {
            var person = new Person("Frank");
            Action act = () => person.UpdateTin("");
            act.Should().Throw<ArgumentException>();
        }
    }
}
