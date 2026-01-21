using CleanCRUDSolution.Application.Common.Results;

namespace CleanCRUDSolution.Application.Features.Countries
{
    public static class CountryErrors
    {
        public static Error FileInvalid => new (
                "file.invalid",
                "The uploaded file is not a valid .xlsx Excel file.",
                ErrorType.Validation);

        public static Error FileUnexpected => new (
                "file.unexpected",
                "An unexpected error occurred.",
                ErrorType.Unexpected);

        public static Error CountryAlreadyExists => new (
            "country.duplicate",
            "Duplicate country name in the upload file.",
            ErrorType.Validation);

        public static Error StreamIsEmpty => new(
            "stream.empty",
            "Stream cannot be empty",
            ErrorType.Validation);

        public static Error StreamExcelTooLarge(int sizeInMb) => new(
            "excel.toolarge",
            $"Excel File cannot exceed {sizeInMb} MB."
            );
    }
}
