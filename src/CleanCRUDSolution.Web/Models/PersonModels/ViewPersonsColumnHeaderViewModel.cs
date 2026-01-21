using CleanCRUDSolution.Application.Features.Persons.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleanCRUDSolution.Web.Models.PersonModels
{
    public class ViewPersonsColumnHeaderViewModel
    {
        public SelectListItem Column { get; set; } = new();
        public GetPersonRequest SearchRequest { get; set; } = new();
    }
}
