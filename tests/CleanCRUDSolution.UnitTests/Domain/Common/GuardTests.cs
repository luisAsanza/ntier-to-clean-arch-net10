using System;
using CleanCRUDSolution.Domain.Common;
using FluentAssertions;
using Xunit;

namespace CleanCRUDSolution.UnitTests.Domain.Common
{
    public class GuardTests
    {
        [Fact]
        public void NotNullOrWhiteSpace_WithValid_ReturnsTrimmed()
        {
            var result = Guard.NotNullOrWhiteSpace("  abc  ", "p");
            result.Should().Be("abc");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void NotNullOrWhiteSpace_Invalid_Throws(string? input)
        {
            Action act = () => Guard.NotNullOrWhiteSpace(input, "p");
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void NullOrNotWhiteSpace_Null_ReturnsNull()
        {
            var result = Guard.NullOrNotWhiteSpace(null, "x");
            result.Should().BeNull();
        }

        [Fact]
        public void NullOrNotWhiteSpace_Empty_Throws()
        {
            Action act = () => Guard.NullOrNotWhiteSpace("", "x");
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void NullOrNotFutureDate_Future_Throws()
        {
            var today = new DateOnly(2025,1,1);
            Action act = () => Guard.NullOrNotFutureDate(new DateOnly(2025,1,2), today, "d");
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void NotEmpty_WithEmptyGuid_Throws()
        {
            Action act = () => Guard.NotEmpty(Guid.Empty, "g");
            act.Should().Throw<ArgumentException>();
        }
    }
}
