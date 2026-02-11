
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanCRUDSolution.Application.Common;
using CleanCRUDSolution.Application.Features.Countries;
using CleanCRUDSolution.Application.Features.Countries.Enums;
using CleanCRUDSolution.Infrastructure.Caching;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CleanCRUDSolution.UnitTests.Infrastructure.Caching
{
    public class CountriesCachedServiceTests
    {
        [Fact]
        public async Task GetAllCountriesAsync_WhenCached_ReturnsFromCacheAndDoesNotCallInner()
        {
            var inner = Substitute.For<ICountriesService>();
            var cache = Substitute.For<ICacheService>();

            var expected = new List<CountryResponse> { new CountryResponse(System.Guid.NewGuid(), "X") };
            cache.GetOrCreateAsync<IReadOnlyList<CountryResponse>>(Arg.Any<string>(), Arg.Any<Func<Task<IReadOnlyList<CountryResponse>>>>(), Arg.Any<TimeSpan?>())
                .Returns(Task.FromResult((IReadOnlyList<CountryResponse>)expected));

            var sut = new CountriesCachedService(inner, cache);

            var result = await sut.GetAllCountriesAsync();

            result.Should().HaveCount(1);
            await inner.DidNotReceiveWithAnyArgs().GetAllCountriesAsync();
        }
    }
}
