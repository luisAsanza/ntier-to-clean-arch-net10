using CleanCRUDSolution.Application.Features.Countries.DTOs;
using CleanCRUDSolution.Application.Features.Countries.Enums;

namespace CleanCRUDSolution.Application.Features.Countries
{
    public interface ICountryImportPolicy
    {
        ImportPolicyDecision Evaluate(CountryImportReport report, HashSet<string> existingNames);
    }
}
