using CleanCRUDSolution.Application.Features.Countries.DTOs;
using CleanCRUDSolution.Application.Features.Countries.Enums;
using CleanCRUDSolution.Domain.Entities;

namespace CleanCRUDSolution.Application.Features.Countries.Mappers
{
    public static class CountryMappingExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse(
                country.Id,
                country.Name);
        }

        public static Country ToCountry(this CountryAddRequest request)
        {
            return new Country(request.Name);
        }
    }
}
