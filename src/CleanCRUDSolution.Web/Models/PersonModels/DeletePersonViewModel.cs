using CleanCRUDSolution.Application.Common.Enums;
using CleanCRUDSolution.Domain.Enums;
using CleanCRUDSolution.Web.Extensions;
using CleanCRUDSolution.Web.Models.PersonModels.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleanCRUDSolution.Web.Models.PersonModels
{
    public class DeletePersonViewModel
    {
        public UpdatePersonDataVM PersonData { get; set; } = new();
        public IReadOnlyList<SelectListItem> CountryOptions { get; set; } = [];
        public IReadOnlyList<SelectListItem> GenderOptions { get; } = EnumExtensions.ToSelectListItem<GenderOptions>();
    }
}
