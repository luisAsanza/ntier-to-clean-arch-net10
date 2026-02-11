using System.IO;
using CleanCRUDSolution.Web.Attributes;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
// FormFile is available from Microsoft.AspNetCore.Http
using Xunit;

namespace CleanCRUDSolution.UnitTests.Presentation.Attributes
{
    public class NotEmptyFileAttributeTests
    {
        [Fact]
        public void IsValid_WithNull_ReturnsFalse()
        {
            var attr = new NotEmptyFileAttribute();
            attr.IsValid(null).Should().BeFalse();
        }

        [Fact]
        public void IsValid_WithEmptyFile_ReturnsFalse()
        {
            using var ms = new MemoryStream();
            var file = new FormFile(ms, 0, 0, "f", "f.txt");
            var attr = new NotEmptyFileAttribute();
            attr.IsValid(file).Should().BeFalse();
        }

        [Fact]
        public void IsValid_WithContent_ReturnsTrue()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("x");
            var ms = new MemoryStream(bytes);
            var file = new FormFile(ms, 0, bytes.Length, "f", "f.txt");
            var attr = new NotEmptyFileAttribute();
            attr.IsValid(file).Should().BeTrue();
        }
    }
}
