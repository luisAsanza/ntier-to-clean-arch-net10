using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanCRUDSolution.Application.Features.Countries;
using CleanCRUDSolution.Application.Abstractions;
using CleanCRUDSolution.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Xunit;

namespace CleanCRUDSolution.UnitTests.Application.Features.Countries
{
    public class CountriesServiceTests
    {
        [Fact]
        public async Task GetAllCountriesAsync_WhenCountriesExist_ReturnsMappedResponses()
        {
            var repo = Substitute.For<ICountriesRepository>();
            var logger = Substitute.For<ILogger<CountriesService>>();

            var countries = new List<Country>
            {
                new Country("Aland"),
                new Country("Belgium")
            };

            repo.GetAllAsync().Returns(Task.FromResult((IReadOnlyList<Country>)countries));

            var sut = new CountriesService(repo, logger);

            var result = await sut.GetAllCountriesAsync();

            result.Should().HaveCount(2);
            result.Select(r => r.Name).Should().Contain(new[] { "Aland", "Belgium" });
        }

        [Fact]
        public async Task GetAllCountriesAsync_WhenNoCountries_ReturnsEmptyList()
        {
            var repo = Substitute.For<ICountriesRepository>();
            var logger = Substitute.For<ILogger<CountriesService>>();

            repo.GetAllAsync().Returns(Task.FromResult((IReadOnlyList<Country>)new List<Country>()));

            var sut = new CountriesService(repo, logger);

            var result = await sut.GetAllCountriesAsync();

            result.Should().BeEmpty();
        }
    }
}
