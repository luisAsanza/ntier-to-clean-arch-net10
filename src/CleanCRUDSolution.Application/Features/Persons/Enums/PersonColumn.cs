using System.ComponentModel.DataAnnotations;

namespace CleanCRUDSolution.Application.Features.Persons.Enums
{
    public enum PersonColumn
    {
        [Display(Name = "Person Name")]
        Name,
        [Display(Name = "Email")]
        Email,
        [Display(Name = "Date Of Birth")]
        DateOfBirth,
        [Display(Name = "Age")]
        Age,
        [Display(Name = "Gender")]
        Gender,
        [Display(Name = "Address")]
        Address,
        [Display(Name = "Receive NewsLetter")]
        ReceiveNewsletter
    };
}
