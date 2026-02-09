using AutoMapper;
using CleanCRUDSolution.Application.Features.Countries.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleanCRUDSolution.Web.Mapping
{
    public class CountryProfile: Profile
    {
        public CountryProfile()
        {
            // Application Data > UI Model
            CreateMap<CountryResponse, SelectListItem>()
                .ConstructUsing(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
        }
    }
}
