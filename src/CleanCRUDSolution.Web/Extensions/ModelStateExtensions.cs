using CleanCRUDSolution.Application.Common.Results;
using FluentValidation.Results;
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
                    var key = string.Empty;

                    if(string.IsNullOrWhiteSpace(prefix))
                    {
                        key = $"{error.Field}";
                    }
                    else
                    {
                        key = $"{prefix}.{error.Field}";
                    }
                    
                    modelState.AddModelError(key, error.Message);
                }
            }
        }

        public static void AddFluentValidationErrors(this ModelStateDictionary modelState, 
            ValidationResult validationResult,
            string? prefix = null)
        {
            foreach (var error in validationResult.Errors)
            {
                if (string.IsNullOrWhiteSpace(error.PropertyName))
                {
                    modelState.AddModelError(string.Empty, error.ErrorMessage);
                }
                else
                {
                    var key = string.Empty;

                    if(string.IsNullOrWhiteSpace(prefix))
                    {
                        key = $"{error.PropertyName}";
                    }
                    else
                    {
                        key = $"{prefix}.{error.PropertyName}";
                    }
                    modelState.AddModelError(key, error.ErrorMessage);
                }
            }
        }
    }
}
