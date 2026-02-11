using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CleanCRUDSolution.Application.Common.Configuration;
using CleanCRUDSolution.Application.Common.Events;
using CleanCRUDSolution.Application.Features.Countries;
using CleanCRUDSolution.Application.Abstractions;
using CleanCRUDSolution.Application.Features.Countries.DTOs;
using CleanCRUDSolution.Application.Features.Countries.Validators;
using CleanCRUDSolution.Infrastructure.FileReader;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace CleanCRUDSolution.UnitTests.Application.Features.Countries
{
    public class BulkUploadCountriesServiceTests
    {
        private IOptionsSnapshot<ExcelSettings> Options(int mb)
        {
            var options = Substitute.For<IOptionsSnapshot<ExcelSettings>>();
            options.Value.Returns(new ExcelSettings { MaxFileSizeInMB = mb });
            return options;
        }

        [Fact]
        public async Task UploadCountriesAsync_EmptyStream_ReturnsFailure()
        {
            var reader = Substitute.For<ICountryFileReader>();
            var repo = Substitute.For<ICountriesRepository>();
            var policy = Substitute.For<ICountryImportPolicy>();
            var validator = Substitute.For<FluentValidation.IValidator<CountryRow>>();
            var logger = Substitute.For<ILogger<BulkUploadCountriesService>>();
            var events = Substitute.For<IEventPublisher>();
            var options = Options(1);

            var sut = new BulkUploadCountriesService(reader, repo, policy, validator, logger, events, options);

            using var ms = new MemoryStream();
            var result = await sut.UploadCountriesAsync(ms, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Code == "stream.empty");
        }

        [Fact]
        public async Task UploadCountriesAsync_InvalidFileFromReader_ReturnsFileInvalid()
        {
            var reader = Substitute.For<ICountryFileReader>();
            reader.ReadCountries(Arg.Any<Stream>()).Returns<Task<IReadOnlyList<CountryRow>>>(_ => throw new InvalidDataException());

            var repo = Substitute.For<ICountriesRepository>();
            var policy = Substitute.For<ICountryImportPolicy>();
            var validator = Substitute.For<FluentValidation.IValidator<CountryRow>>();
            var logger = Substitute.For<ILogger<BulkUploadCountriesService>>();
            var events = Substitute.For<IEventPublisher>();
            var options = Options(10);

            var sut = new BulkUploadCountriesService(reader, repo, policy, validator, logger, events, options);

            using var ms = new MemoryStream(new byte[10]);
            var result = await sut.UploadCountriesAsync(ms, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Code == "file.invalid");
        }

        [Fact]
        public async Task UploadCountriesAsync_EmptyRows_ReturnsEmptyReportSuccess()
        {
            var reader = Substitute.For<ICountryFileReader>();
            reader.ReadCountries(Arg.Any<Stream>()).Returns(Task.FromResult((IReadOnlyList<CountryRow>)new List<CountryRow>()));

            var repo = Substitute.For<ICountriesRepository>();
            var policy = Substitute.For<ICountryImportPolicy>();
            var validator = Substitute.For<FluentValidation.IValidator<CountryRow>>();
            var logger = Substitute.For<ILogger<BulkUploadCountriesService>>();
            var events = Substitute.For<IEventPublisher>();
            var options = Options(10);

            var sut = new BulkUploadCountriesService(reader, repo, policy, validator, logger, events, options);

            using var ms = new MemoryStream(new byte[1]);
            var result = await sut.UploadCountriesAsync(ms, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value!.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task UploadCountriesAsync_ValidRows_PersistsAndPublishesEvent()
        {
            var rows = new List<CountryRow> { new CountryRow(1, "Aland") };

            var reader = Substitute.For<ICountryFileReader>();
            reader.ReadCountries(Arg.Any<Stream>()).Returns(Task.FromResult((IReadOnlyList<CountryRow>)rows));

            var repo = Substitute.For<ICountriesRepository>();
            repo.AddRangeAsync(Arg.Any<IReadOnlyList<CleanCRUDSolution.Domain.Entities.Country>>()).Returns(Task.FromResult(1));

            var policy = new AbortIfAnyExistsPolicy();
            var validator = new CountryRowValidator();
            repo.GetExistingNamesAsync(Arg.Any<IReadOnlyList<string>>()).Returns(Task.FromResult(new HashSet<string>()));
            var logger = Substitute.For<ILogger<BulkUploadCountriesService>>();
            var events = Substitute.For<IEventPublisher>();
            var options = Options(10);

            var sut = new BulkUploadCountriesService(reader, repo, policy, validator, logger, events, options);

            using var ms = new MemoryStream(new byte[1]);
            var result = await sut.UploadCountriesAsync(ms, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value!.PersistedItemsCount.Should().Be(1);
            await events.Received(1).PublishAsync(Arg.Any<CleanCRUDSolution.Application.Common.Events.CountriesChangedEvent>(), Arg.Any<CancellationToken>());
        }
    }
}
