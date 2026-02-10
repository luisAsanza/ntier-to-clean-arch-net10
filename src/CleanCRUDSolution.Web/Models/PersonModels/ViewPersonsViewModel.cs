using CleanCRUDSolution.Application.Features.Persons.DTOs;
using CleanCRUDSolution.Application.Features.Persons.Enums;
using CleanCRUDSolution.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleanCRUDSolution.Web.Models.PersonModels
{
    public class ViewPersonsViewModel
    {
        public IReadOnlyList<SelectListItem> SearchOptions => EnumExtensions.ToSelectListItem<PersonColumn>(x => x.ToLabel());
        public IReadOnlyList<PersonResponse> Persons { get; set; } = [];
        public GetPersonRequest SearchRequest{ get; set; } = new();
    }
}
