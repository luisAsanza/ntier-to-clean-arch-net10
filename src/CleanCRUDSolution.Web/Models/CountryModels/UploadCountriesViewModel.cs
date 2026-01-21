using CleanCRUDSolution.Web.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CleanCRUDSolution.Web.Models.CountryModels
{
    public class UploadCountriesViewModel
    {
        public int? PersistedItemsCount { get; set; }
        public string? ReportOutcome { get; set; }
        public string? ReportOutcomeReason { get; set; }

        [Required]
        [NotEmptyFile(ErrorMessage = "File cannot be empty")]
        public IFormFile? FormFile { get; set; }

    }
}
