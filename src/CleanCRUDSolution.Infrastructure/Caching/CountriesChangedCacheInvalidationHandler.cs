using CleanCRUDSolution.Application.Common;
using CleanCRUDSolution.Application.Common.Events;
using CleanCRUDSolution.Infrastructure.Events;

namespace CleanCRUDSolution.Infrastructure.Caching
{
    public sealed class CountriesChangedCacheInvalidationHandler : IEventHandler<CountriesChangedEvent>
    {
        private readonly ICacheService _cache;

        public CountriesChangedCacheInvalidationHandler(ICacheService cache)
        {
            _cache = cache;
        }

        public Task HandleAsync(CountriesChangedEvent appEvent, CancellationToken ct)
        {
            _cache.Remove(CountriesCacheKeys.GetAllCountries);
            return Task.CompletedTask;
        }
    }
}
