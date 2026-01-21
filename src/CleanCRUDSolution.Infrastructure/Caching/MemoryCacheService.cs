using Microsoft.Extensions.Caching.Memory;
using CleanCRUDSolution.Application.Common;
using System.Collections.Concurrent;

namespace CleanCRUDSolution.Infrastructure.Caching
{
    /// <summary>
    /// Implements in-memory caching service using IMemoryCache
    /// </summary>
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _absoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
        private readonly TimeSpan _slidingExpiration = TimeSpan.FromMinutes(10);

        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (_memoryCache.TryGetValue(key, out T? result)) return result!;

            var semaphore = _locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync();
            try
            {
                if(_memoryCache.TryGetValue(key, out result)) return result!;

                result = await factory();

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? _absoluteExpirationRelativeToNow,
                    SlidingExpiration = _slidingExpiration
                };

                _memoryCache.Set(key, result, cacheOptions);

                return result;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
            _locks.TryRemove(key, out _);
        }
    }
}
