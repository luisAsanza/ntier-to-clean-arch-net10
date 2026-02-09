using CleanCRUDSolution.Application.Common.Configuration;
using CleanCRUDSolution.Application.Features.Countries;
using CleanCRUDSolution.Application.Features.Persons;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanCRUDSolution.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configure Options pattern for FileSettings
            services.AddOptions<ExcelSettings>()
                .Bind(configuration.GetSection(ExcelSettings.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            // Use cases service registrations go here            
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IBulkUploadCountriesService, BulkUploadCountriesService>();
            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IPersonReportService, PersonReportService>();
            services.AddScoped<ICountryImportPolicy, AbortIfAnyExistsPolicy>();

            // Register Fluent Validation for Application layer
            services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyMarker>();

            return services;
        }
    }
}
