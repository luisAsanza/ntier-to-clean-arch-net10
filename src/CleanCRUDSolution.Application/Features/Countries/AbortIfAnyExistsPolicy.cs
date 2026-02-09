using CleanCRUDSolution.Application.Common.Results;
using CleanCRUDSolution.Application.Features.Countries.DTOs;
using CleanCRUDSolution.Application.Features.Countries.Enums;

namespace CleanCRUDSolution.Application.Features.Countries
{
    /// <summary>
    /// An import policy that aborts the entire import when any valid country to be imported
    /// already exists in the database.
    /// </summary>
    /// <remarks>
    /// Behavior:
    /// - Evaluates only items in <see cref="CountryImportReport.Items"/> whose status is <see cref="CountryImportItemStatus.Valid"/>.
    /// - If none of the valid items exist in the provided <paramref name="existingNames"/>, returns <see cref="ImportPolicyDecision.ProceedToPersist"/>.
    /// - If one or more valid items already exist in <paramref name="existingNames"/>:
    ///   - Marks those items as <see cref="CountryImportItemStatus.DuplicateInDb"/> and attaches an error with code "country.duplicateindb".
    ///   - Marks the remaining valid items as <see cref="CountryImportItemStatus.BlockedByPolicy"/> and attaches an error with code "country.blockedbypolicy".
    ///   - Sets the report outcome to <see cref="ImportOutcome.BlockedByPolicy"/> with an explanatory message.
    ///   - Returns <see cref="ImportPolicyDecision.BlockedByPolicy"/>.
    /// </remarks>
    public class AbortIfAnyExistsPolicy : ICountryImportPolicy
    {
        /// <summary>
        /// Evaluates the import report against the policy using a set of normalized country names
        /// that already exist in the database.
        /// </summary>
        /// <param name="report">The import report containing items to evaluate and to which errors/status updates will be applied.</param>
        /// <param name="existingNames">
        /// A set of normalized country names representing countries already present in the database.
        /// Comparison is performed against each valid item's <see cref="CountryImportItem.NormalizedCountryName"/>.
        /// </param>
        /// <returns>
        /// <see cref="ImportPolicyDecision.ProceedToPersist"/> when no valid items conflict with existing names;
        /// otherwise <see cref="ImportPolicyDecision.BlockedByPolicy"/>.
        /// </returns>
        public ImportPolicyDecision Evaluate(CountryImportReport report, HashSet<string> existingNames)
        {
            // Policy: If any of the valid countries to be imported already exist in the database, block the entire import.
            var validItems = report.Items.Where(i => i.Status == CountryImportItemStatus.Valid).ToList();
            var anyExist = validItems.Any(item => existingNames.Contains(item.NormalizedCountryName));
            if (!anyExist)
            {
                return ImportPolicyDecision.ProceedToPersist;
            }

            // Apply changes
            foreach (var item in validItems)
            {
                if (existingNames.Contains(item.NormalizedCountryName))
                {
                    var error = new Error("country.duplicateindb", 
                        $"Country '{item.NormalizedCountryName}' already exists in the database.", 
                        ErrorType.Validation);
                    report.AddItemErrorAndStatus(item, CountryImportItemStatus.DuplicateInDb, error);
                }
                else
                {
                    var error = new Error("country.blockedbypolicy", 
                        $"Country '{item.NormalizedCountryName}' was not persisted due to policy.", 
                        ErrorType.Validation);
                    report.AddItemErrorAndStatus(item, CountryImportItemStatus.BlockedByPolicy, error);
                }
            }

            report.SetOutcome(ImportOutcome.BlockedByPolicy,
                "One or more countries already exist; no rows were persisted due to policy");

            return ImportPolicyDecision.BlockedByPolicy;
        }
    }
}
