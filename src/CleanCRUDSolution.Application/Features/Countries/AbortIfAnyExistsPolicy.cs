using CleanCRUDSolution.Application.Common.Results;
using CleanCRUDSolution.Application.Features.Countries.DTOs;
using CleanCRUDSolution.Application.Features.Countries.Enums;

namespace CleanCRUDSolution.Application.Features.Countries
{
    public class AbortIfAnyExistsPolicy : ICountryImportPolicy
    {
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
