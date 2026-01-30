using CleanCRUDSolution.Application.Features.Countries.DTOs;
using CleanCRUDSolution.Application.Features.Countries.Enums;

namespace CleanCRUDSolution.Application.Features.Countries
{
    /// <summary>
    /// Policy for determining whether an import should proceed or be blocked based on
    /// existing country names in the database.
    /// </summary>
    public interface ICountryImportPolicy
    {
        /// <summary>
        /// Evaluates the provided import report against the policy using a set of existing normalized names.
        /// </summary>
        ImportPolicyDecision Evaluate(CountryImportReport report, HashSet<string> existingNames);
    }
}
