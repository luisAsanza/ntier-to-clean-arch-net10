using System;
using CleanCRUDSolution.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CleanCRUDSolution.UnitTests.Domain.Entities
{
    public class CountryTests
    {
        [Fact]
        public void Ctor_WithValidName_SetsName()
        {
            var country = new Country("Spain");
            country.Name.Should().Be("Spain");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Ctor_WithInvalidName_ThrowsArgumentException(string? invalid)
        {
            Action act = () => new Country(invalid!);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Rename_WithValidName_ChangesName()
        {
            var country = new Country("Old");
            country.Rename("New");
            country.Name.Should().Be("New");
        }
    }
}
