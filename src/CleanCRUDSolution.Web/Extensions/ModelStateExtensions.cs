using CleanCRUDSolution.Application.Common.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CleanCRUDSolution.Web.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddApplicationErrors(this ModelStateDictionary modelState, 
            IReadOnlyList<Error> errors,
            string? prefix = null)
        {
            foreach (var error in errors)
            {
                if (string.IsNullOrWhiteSpace(error.Field))
                {
                    modelState.AddModelError(string.Empty, error.Message);
                }
                else
                {
                    if(string.IsNullOrWhiteSpace(prefix)) prefix = string.Empty;
                    var key = $"{prefix}.{error.Field}";
                    modelState.AddModelError(key, error.Message);
                }
            }
        }
    }
}
