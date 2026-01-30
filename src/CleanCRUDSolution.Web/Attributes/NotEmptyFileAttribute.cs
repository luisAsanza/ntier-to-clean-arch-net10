using System.ComponentModel.DataAnnotations;

namespace CleanCRUDSolution.Web.Attributes
{
    /// <summary>
    /// Validation attribute that ensures an uploaded IFormFile is not empty.
    /// </summary>
    public class NotEmptyFileAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validates that the provided value is an <see cref="IFormFile"/> with content.
        /// </summary>
        override public bool IsValid(object? value)
        {
            if(value is not IFormFile file)
            {
                return false;
            }

            if (file == null)
            {
                return false; // No file provided
            }

            return file.Length > 0; // Check if file is not empty
        }
    }
}
