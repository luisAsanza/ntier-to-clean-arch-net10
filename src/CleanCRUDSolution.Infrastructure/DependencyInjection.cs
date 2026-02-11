using CleanCRUDSolution.Application.Abstractions;
using CleanCRUDSolution.Application.Common;
using CleanCRUDSolution.Application.Common.Enums;
using CleanCRUDSolution.Application.Common.Events;
using CleanCRUDSolution.Application.Features.Countries;
using CleanCRUDSolution.Application.Features.Reporting;
using CleanCRUDSolution.Domain.Common.Time;
using CleanCRUDSolution.Infrastructure.Caching;
using CleanCRUDSolution.Infrastructure.Events;
using CleanCRUDSolution.Infrastructure.FileReader;
using CleanCRUDSolution.Infrastructure.Persistence;
using CleanCRUDSolution.Infrastructure.Reporting;
using CleanCRUDSolution.Infrastructure.Repositories;
using CleanCRUDSolution.Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanCRUDSolution.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            // Configure DbContext with SQL Server            
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
            }
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, c =>
                {
                    c.MigrationsAssembly(typeof(InfrastructureAssemblyMarker).Assembly.GetName().Name);
                });
            });

            // Add memory cache
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, MemoryCacheService>();

            // Register repositories
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();

            //Register Reporting Service
            services.AddScoped<IPersonReportGeneratorFactory, PersonReportGeneratorFactory>();
            services.AddKeyedScoped<IPersonReportGenerator, CsvPersonReportGenerator>(ReportOptions.CSV);
            services.AddKeyedScoped<IPersonReportGenerator, ExcelPersonReportGenerator>(ReportOptions.Excel);

            // Register Countries Service with Caching Decorator
            if(services.Any(s => s.ServiceType == typeof(ICountriesService)))
            {
                services.Decorate<ICountriesService, CountriesCachedService>();
            }
            else
            {
                throw new InvalidOperationException("CRITICAL: CountriesService must be registered before adding caching decorator.");
            }
            
            // Register File Reader Service
            services.AddScoped<ICountryFileReader, CountryExcelFileReader>();
            services.AddScoped<IXlsxSafetyValidator, XlsxSafetyValidator>();

            //Register Event services
            services.AddScoped<IEventPublisher, InProcessEventPublisher>();
            services.AddScoped<IEventHandler<CountriesChangedEvent>, CountriesChangedCacheInvalidationHandler>();

            //Register clock service
            services.AddSingleton<IClock, SystemClock>();

            return services;
        }
    }
}
