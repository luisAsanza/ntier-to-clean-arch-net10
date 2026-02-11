using System;
using System.Threading.Tasks;
using CleanCRUDSolution.Infrastructure.Caching;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace CleanCRUDSolution.UnitTests.Infrastructure
{
    public class MemoryCacheServiceTests
    {
        [Fact]
        public async Task GetOrCreateAsync_KeyNotExists_FactoryCalledAndResultCached()
        {
            using var memory = new MemoryCache(new MemoryCacheOptions());
            var sut = new MemoryCacheService(memory);

            var calls = 0;
            Func<Task<string>> factory = async () =>
            {
                await Task.Yield();
                calls++;
                return "value";
            };

            var first = await sut.GetOrCreateAsync<string>("k1", factory);
            var second = await sut.GetOrCreateAsync<string>("k1", () => throw new InvalidOperationException());

            first.Should().Be("value");
            second.Should().Be("value");
            calls.Should().Be(1);
        }

        [Fact]
        public async Task Remove_RemovesCachedValue()
        {
            var memory = new MemoryCache(new MemoryCacheOptions());
            var sut = new MemoryCacheService(memory);

            await sut.GetOrCreateAsync("k2", () => Task.FromResult("v"));
            sut.Remove("k2");

            var found = memory.TryGetValue("k2", out string? _);
            found.Should().BeFalse();
        }
    }
}
