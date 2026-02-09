using CleanCRUDSolution.Application.Common;
using CleanCRUDSolution.Application.Features.Countries;
using CleanCRUDSolution.Application.Features.Countries.Enums;

namespace CleanCRUDSolution.Infrastructure.Caching
{
    public class CountriesCachedService : ICountriesService
    {
        private readonly ICountriesService _innerService;
        private readonly ICacheService _cacheService;

        public CountriesCachedService(ICountriesService innerService, ICacheService cacheService)
        {
            _innerService = innerService;
            _cacheService = cacheService;
        }

        public Task<IReadOnlyList<CountryResponse>> GetAllCountriesAsync()
        {
            return _cacheService.GetOrCreateAsync(CountriesCacheKeys.GetAllCountries, _innerService.GetAllCountriesAsync);
        }
    }
}
