using System.ComponentModel.DataAnnotations;

namespace CleanCRUDSolution.Web.Models.PersonModels.Data
{
    public class UpdatePersonDataVM : PersonDataVM
    {
        [Required]
        public Guid? Id { get; set; }
    }
}
