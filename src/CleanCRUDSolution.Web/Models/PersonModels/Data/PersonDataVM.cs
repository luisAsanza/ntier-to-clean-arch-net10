using System.ComponentModel.DataAnnotations;

namespace CleanCRUDSolution.Web.Models.PersonModels.Data
{
    public class PersonDataVM
    {
        [Required(ErrorMessage = "Persons name can't be empty")]
        [MaxLength(80)]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email value should be a valid email")]
        [MaxLength(80)]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public Guid? CountryId { get; set; }

        public string? CountryName { get; set; }

        [MaxLength(400)]
        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }
    }
}
