using Serilog;

namespace CleanCRUDSolution.Web.Extensions.StartupExtensions
{
    /// <summary>
    /// Host-related extension methods used during application startup to configure logging and related services.
    /// </summary>
    public static class HostExtensions
    {
        /// <summary>
        /// Configures Serilog for the application using service provider enrichment.
        /// </summary>
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, sp, loggerConfiguration) => {
                loggerConfiguration
                .ReadFrom.Services(sp)
                .Enrich.WithMachineName()
                .WriteTo.Console();
            });
        }

        /// <summary>
        /// Configures Serilog for automated testing, reading configuration from the host configuration.
        /// </summary>
        public static void ConfigureSerilogForTesting(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, sp, loggerConfiguration) => {
                loggerConfiguration
                .ReadFrom.Services(sp)
                .Enrich.WithMachineName()
                .ReadFrom.Configuration(context.Configuration);
            });
        }
    }
}
