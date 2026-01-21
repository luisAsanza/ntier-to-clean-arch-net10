using CleanCRUDSolution.Application.Abstractions;
using CleanCRUDSolution.Application.Common.Configuration;
using CleanCRUDSolution.Application.Common.Events;
using CleanCRUDSolution.Application.Common.Exceptions;
using CleanCRUDSolution.Application.Common.Mappers;
using CleanCRUDSolution.Application.Common.Results;
using CleanCRUDSolution.Application.Features.Countries.DTOs;
using CleanCRUDSolution.Application.Features.Countries.Enums;
using CleanCRUDSolution.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace CleanCRUDSolution.Application.Features.Countries
{
    public class BulkUploadCountriesService : IBulkUploadCountriesService
    {
        private readonly ICountryFileReader _countryFileReader;
        private readonly ICountriesRepository _countriesRepository;
        private readonly ICountryImportPolicy _countryImportPolicy;
        private readonly IValidator<CountryRow> _countryRowValidator;
        private readonly ILogger<BulkUploadCountriesService> _logger;
        private readonly IEventPublisher _eventPublisher;
        private readonly IOptionsSnapshot<ExcelSettings> _options;

        public BulkUploadCountriesService(ICountryFileReader countryFileReader, 
            ICountriesRepository countriesRepository,
            ICountryImportPolicy countryImportPolicy,
            IValidator<CountryRow> countryRowValidator,
            ILogger<BulkUploadCountriesService> logger,
            IEventPublisher eventPublisher,
            IOptionsSnapshot<ExcelSettings> options)
        {
            _countryFileReader = countryFileReader;
            _countriesRepository = countriesRepository;
            _countryImportPolicy = countryImportPolicy;
            _countryRowValidator = countryRowValidator;
            _logger = logger;
            _eventPublisher = eventPublisher;
            _options = options;
        }

        public async Task<Result<CountryImportReport>> UploadCountriesAsync(Stream stream, CancellationToken ct)
        {
            if(stream is null || stream.Length == 0)
            {
                return Result<CountryImportReport>.Failure(CountryErrors.StreamIsEmpty);
            }

            if (stream.Length > _options.Value.MaxSizeInBytes)
            {
                return Result<CountryImportReport>.Failure(CountryErrors.StreamExcelTooLarge(_options.Value.MaxFileSizeInMB));
            }

            IReadOnlyList<CountryRow> rowCountries;

            // 1. Validate the input stream
            try
            {
                rowCountries = await _countryFileReader.ReadCountries(stream);
            }
            catch (InvalidDataException)
            {
                return Result<CountryImportReport>.Failure(CountryErrors.FileInvalid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error.");
                return Result<CountryImportReport>.Failure(CountryErrors.FileUnexpected);
            }

            // 2. Check for empty file
            if (rowCountries is null || !rowCountries.Any())
            {
                var emptyReport = new CountryImportReport([]);
                return Result<CountryImportReport>.Success(emptyReport);
            }

            // 3. Process country import items and generate report
            var countryImportReport = new CountryImportReport(rowCountries);
            await ProcessCountryRow(rowCountries, countryImportReport, ct);

            if(countryImportReport.Items.All(item => item.Status != CountryImportItemStatus.Valid))
            {
                return Result<CountryImportReport>.Success(countryImportReport);
            }

            // 4. Map to country entities to persist
            var validItems = countryImportReport.Items
                .Where(i => i.Status == CountryImportItemStatus.Valid)
                .ToList();

            var namesToMatch = validItems.Select(i => i.NormalizedCountryName).ToList();
            var existingNames = await _countriesRepository.GetExistingNamesAsync(namesToMatch);
            var decision = _countryImportPolicy.Evaluate(countryImportReport, existingNames);

            if (decision == ImportPolicyDecision.BlockedByPolicy)
            {
                return Result<CountryImportReport>.Success(countryImportReport);
            }

            // 5. Attempt to save countries to the repository
            var countries = validItems
                .Select(i => new Country(i.NormalizedCountryName))
                .ToList();

            try
            {
                await _countriesRepository.AddRangeAsync(countries);
                countryImportReport.SetValidItemsAsPersisted();
                await _eventPublisher.PublishAsync(new CountriesChangedEvent(), ct);
            }
            catch (DuplicateResourceException)
            {
                countryImportReport.SetValidItemsAsFailedToPersist();
            }

            return Result<CountryImportReport>.Success(countryImportReport);
        }

        private static string NormalizeCountryName(string countryRawName)
        {
            var trimmedName = countryRawName.Trim();
            //var collapsed = string.Join(" ", trimmedName.Split(" ", StringSplitOptions.RemoveEmptyEntries));
            var collapsed = Regex.Replace(trimmedName, @"\s+", " ");
            return collapsed;
        }

        private async Task ProcessCountryRow(IReadOnlyList<CountryRow> countryRowList, 
            CountryImportReport importReports,
            CancellationToken ct)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var countryRow in countryRowList)
            {
                var validationResult = await _countryRowValidator.ValidateAsync(countryRow, ct);

                if (!validationResult.IsValid)
                {
                    importReports.AddInvalidCountryImportItem(
                        countryRow.RowNumber, 
                        countryRow.AttemptedCountryName, 
                        validationResult.ToErrorList());

                    continue;
                }

                var normalizedCountryName = NormalizeCountryName(countryRow.AttemptedCountryName!);

                if (seen.Add(normalizedCountryName))
                {
                    importReports.AddValidCountryImportItem(
                        countryRow.RowNumber, 
                        countryRow.AttemptedCountryName,
                        normalizedCountryName);
                }
                else
                {
                    importReports.AddDuplicateInFileCountryImportItem(
                        countryRow.RowNumber,
                        countryRow.AttemptedCountryName,
                        CountryErrors.CountryAlreadyExists);
                }
            }
        }
    }
}
