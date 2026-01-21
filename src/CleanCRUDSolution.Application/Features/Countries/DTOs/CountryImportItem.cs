using CleanCRUDSolution.Application.Common.Results;
using CleanCRUDSolution.Application.Features.Countries.Enums;

namespace CleanCRUDSolution.Application.Features.Countries.DTOs
{
    public class CountryImportItem
    {
        public int RowNumber { get; }
        public string? AttemptedCountryName { get; }

        private readonly List<Error> _errors;
        public IReadOnlyList<Error> Errors => _errors;

        private CountryImportItemStatus _status;
        public CountryImportItemStatus Status => _status;

        public string NormalizedCountryName { get; private set; } = string.Empty;

        private CountryImportItem(int rowNumber, string? attemptedCountryName, string normalizedCountryName)
        {
            RowNumber = rowNumber;
            AttemptedCountryName = attemptedCountryName;
            _errors = [];
            NormalizedCountryName = normalizedCountryName;
            _status = CountryImportItemStatus.Valid;
        }

        private CountryImportItem(int rowNumber, string? attemptedCountryName, List<Error> errors, CountryImportItemStatus status)
        {
            RowNumber = rowNumber;
            AttemptedCountryName = attemptedCountryName;
            _errors = errors;
            _status = status;
        }

        public static CountryImportItem Valid(int rowNumber, string? attemptedCountryName, string normalizedCountryName)
            => new (rowNumber, attemptedCountryName, normalizedCountryName);
        public static CountryImportItem Invalid(int rowNumber, string? attemptedCountryName, List<Error> errors)
            => new (rowNumber, attemptedCountryName, errors, CountryImportItemStatus.Invalid);
        public static CountryImportItem DuplicateInFile(int rowNumber, string? attemptedCountryName, Error error)
            => new(rowNumber, attemptedCountryName, [error], CountryImportItemStatus.DuplicateInFile);

        public void SetStatus(CountryImportItemStatus status)
        {
            if (_status != CountryImportItemStatus.Valid)
                throw new InvalidOperationException("Only valid items can transition due to persistence policy.");

            _status = status;
        }

        public void AddErrorAndSetStatus(CountryImportItemStatus status, Error error)
        {
            if(_status != CountryImportItemStatus.Valid) 
                throw new InvalidOperationException("Only valid items can transition due to persistence policy.");

            _errors.Add(error);
            _status = status;
        }
    }
}
