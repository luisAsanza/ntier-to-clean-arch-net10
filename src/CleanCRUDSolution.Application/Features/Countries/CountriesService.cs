using CleanCRUDSolution.Application.Features.Countries.Mappers;
using Microsoft.Extensions.Logging;
using CleanCRUDSolution.Application.Abstractions;
using CleanCRUDSolution.Application.Features.Countries.Enums;

namespace CleanCRUDSolution.Application.Features.Countries
{
    /// <summary>
    /// Service providing operations related to countries, such as retrieving all countries.
    /// </summary>
    public partial class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository _countriesRepository;
        private readonly ILogger<CountriesService> _logger;

        public CountriesService(ICountriesRepository countriesRepository, ILogger<CountriesService> logger)
        {
            _countriesRepository = countriesRepository;
            _logger = logger;
        }

        public async Task<IReadOnlyList<CountryResponse>> GetAllCountriesAsync()
        {
            _logger.LogInformation("GetAllCountries of CountriesService");
            var countries = await _countriesRepository.GetAllAsync();
            var countryResponseList = countries.Select(c => c.ToCountryResponse()).ToList();
            return countryResponseList;
        }
    }
}
