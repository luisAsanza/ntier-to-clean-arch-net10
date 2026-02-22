using Microsoft.Extensions.Caching.Memory;
using AsyncKeyedLock;

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
        private static readonly AsyncKeyedLocker<string> _keyedLocker = new(o =>
        {
                o.PoolSize = 50;
                o.PoolInitialFill = 1;
        });

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (_memoryCache.TryGetValue(key, out T? result)) return result!;

            using (await _keyedLocker.LockAsync(key))
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
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
