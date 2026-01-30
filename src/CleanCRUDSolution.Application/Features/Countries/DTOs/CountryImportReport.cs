using CleanCRUDSolution.Application.Common.Results;
using CleanCRUDSolution.Application.Features.Countries.Enums;

namespace CleanCRUDSolution.Application.Features.Countries.DTOs
{
    /// <summary>
    /// Represents the result of attempting to import a list of countries from an external source.
    /// Contains per-row items, overall outcome and any associated reasons.
    /// </summary>
    public class CountryImportReport
    {
        public IReadOnlyList<CountryRow> CountryRowList { get; }

        private readonly List<CountryImportItem> _items = [];
        public IReadOnlyList<CountryImportItem> Items => _items;

        public ImportOutcome Outcome { get; private set; }
        public string? OutcomeReason { get; private set; }

        public int PersistedItemsCount => _items.Count(i => i.Status == CountryImportItemStatus.Persisted);

        public CountryImportReport(IReadOnlyList<CountryRow> countryRows)
        {
            CountryRowList = countryRows;
            Outcome = ImportOutcome.NotAttempted;
        }

        public void AddValidCountryImportItem(int rowNumber, string? attemptedCountryName, string normalizedCountryName)
        {
            var countryImportedItem = CountryImportItem.Valid(rowNumber, attemptedCountryName, normalizedCountryName);
            _items.Add(countryImportedItem);
        }

        public void AddInvalidCountryImportItem(int rowNumber, string? attemptedCountryName, List<Error> errors)
        {
            var countryImportedItem = CountryImportItem.Invalid(rowNumber, attemptedCountryName, errors);
            _items.Add(countryImportedItem);
        }

        public void AddDuplicateInFileCountryImportItem(int rowNumber, string? attemptedCountryName, Error error)
        {
            var countryImportedItem = CountryImportItem.DuplicateInFile(rowNumber, attemptedCountryName, error);
            _items.Add(countryImportedItem);
        }

        public void SetValidItemsAsPersisted()
        {
            foreach (var item in _items.Where(i => i.Status == CountryImportItemStatus.Valid))
            {
                item.SetStatus(CountryImportItemStatus.Persisted);
            }

            Outcome = ImportOutcome.Persisted;
        }

        public void SetValidItemsAsFailedToPersist()
        {
            foreach (var item in _items.Where(i => i.Status == CountryImportItemStatus.Valid))
            {
                item.SetStatus(CountryImportItemStatus.FailedToPersist);
            }

            Outcome = ImportOutcome.FailedToPersist;
            OutcomeReason = "One or more countries failed to persist due to a db failure.";
        }

        public void SetOutcome(ImportOutcome outcome, string? reason = null)
        {
            Outcome = outcome;
            OutcomeReason = reason;
        }

        public void AddItemErrorAndStatus(
            CountryImportItem item,
            CountryImportItemStatus status,
            Error error)
        {
            item.AddErrorAndSetStatus(status, error);
        }
    }
}
