using CleanCRUDSolution.Application.Common.Results;
using FluentValidation.Results;

namespace CleanCRUDSolution.Application.Common.Mappers
{
    public static class FluentValidationMappingExtensions
    {
        public static Error ToError(this ValidationFailure failure)
        {
            return new Error(
                failure.ToFullErrorCode(), 
                failure.ErrorMessage,
                ErrorType.Validation,
                failure.PropertyName);
        }

        // Map from ValidationFailure List to Error list
        public static List<Error> ToErrorList(this ValidationResult? validationResult)
        {
            if (validationResult == null || validationResult.IsValid)
            {
                return [];
            }

            var errors = new List<Error>();
            foreach (var failure in validationResult.Errors)
            {
                if(failure == null) continue;

                errors.Add(
                    new Error(
                        failure.ToFullErrorCode(), 
                        failure.ErrorMessage,
                        ErrorType.Validation,
                        failure.PropertyName)
                    );
            }
            return errors;
        }

        public static string ToFullErrorCode(this ValidationFailure failure)
        {
            return $"validation.{failure.ErrorCode}";
        }
    }
}
