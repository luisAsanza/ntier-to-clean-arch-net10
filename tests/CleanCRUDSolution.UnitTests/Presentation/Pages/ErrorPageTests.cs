using System;
using CleanCRUDSolution.Web.Pages;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace CleanCRUDSolution.UnitTests.Presentation.Pages
{
    public class ErrorPageTests
    {
        [Fact]
        public void OnGet_WithCode_SetsRequestIdAndStatusCode_NoExceptionFeature()
        {
            var logger = Substitute.For<ILogger<ErrorModel>>();
            var model = new ErrorModel(logger);

            var context = new DefaultHttpContext();
            context.TraceIdentifier = "trace-123";
            model.PageContext = new PageContext { HttpContext = context };

            model.OnGet(404);

            model.RequestId.Should().Be("trace-123");
            model.ErrorStatusCode.Should().Be(404);
            model.ShowRequestId.Should().BeTrue();
        }

        [Fact]
        public void OnGet_WithExceptionFeature_SetsRequestIdAndStatusCode()
        {
            var logger = Substitute.For<ILogger<ErrorModel>>();
            var model = new ErrorModel(logger);

            var context = new DefaultHttpContext();
            context.TraceIdentifier = "trace-xyz";
            var feature = new TestExceptionFeature { Error = new Exception("boom"), Path = "/x" };
            context.Features.Set<IExceptionHandlerPathFeature>(feature);

            model.PageContext = new PageContext { HttpContext = context };

            model.OnGet(null);

            model.RequestId.Should().Be("trace-xyz");
            model.ErrorStatusCode.Should().Be(500);
            model.ShowRequestId.Should().BeTrue();
        }
    }
}

    internal class TestExceptionFeature : IExceptionHandlerPathFeature
    {
        public Exception Error { get; set; } = default!;
        public string Path { get; set; } = string.Empty;
    }
