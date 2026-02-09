using System.ComponentModel.DataAnnotations;

namespace CleanCRUDSolution.Application.Common.Configuration
{
    public class ExcelSettings
    {
        public const string SectionName = "Features:ExcelUpload";

        [Required]
        [Range(0, 100)]
        public int MaxFileSizeInMB { get; set; }

        [Required]
        public string[] AllowedExtensions { get; set; } = [];

        public long MaxSizeInBytes => MaxFileSizeInMB * 1024 * 1024;
    }
}
