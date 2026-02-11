using System.Collections.Generic;
using CleanCRUDSolution.Application.Features.Countries;
using CleanCRUDSolution.Application.Features.Countries.DTOs;
using CleanCRUDSolution.Application.Features.Countries.Enums;
using FluentAssertions;
using Xunit;

namespace CleanCRUDSolution.UnitTests.Application.Features.Countries
{
    public class AbortIfAnyExistsPolicyTests
    {
        [Fact]
        public void Evaluate_WhenNoExisting_ReturnsProceed()
        {
            var report = new CountryImportReport(new List<CountryRow>());
            report.AddValidCountryImportItem(1, "Aland", "Aland");
            report.AddValidCountryImportItem(2, "Belgium", "Belgium");

            var policy = new AbortIfAnyExistsPolicy();

            var decision = policy.Evaluate(report, new HashSet<string>());

            decision.Should().Be(ImportPolicyDecision.ProceedToPersist);
            report.Outcome.Should().Be(ImportOutcome.NotAttempted);
        }

        [Fact]
        public void Evaluate_WhenSomeExist_BlocksAndMarksItems()
        {
            var report = new CountryImportReport(new List<CountryRow>());
            report.AddValidCountryImportItem(1, "Aland", "Aland");
            report.AddValidCountryImportItem(2, "Belgium", "Belgium");

            var policy = new AbortIfAnyExistsPolicy();

            var existing = new HashSet<string>(new[] { "Belgium" });

            var decision = policy.Evaluate(report, existing);

            decision.Should().Be(ImportPolicyDecision.BlockedByPolicy);
            report.Outcome.Should().Be(ImportOutcome.BlockedByPolicy);
            // one item should be DuplicateInDb and the other BlockedByPolicy
            report.Items.Should().Contain(i => i.NormalizedCountryName == "Belgium" && i.Status == CountryImportItemStatus.DuplicateInDb);
            report.Items.Should().Contain(i => i.NormalizedCountryName == "Aland" && i.Status == CountryImportItemStatus.BlockedByPolicy);
        }
    }
}
