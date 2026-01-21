using System.ComponentModel.DataAnnotations;

namespace CleanCRUDSolution.Web.Attributes
{
    public class NotEmptyFileAttribute : ValidationAttribute
    {
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
