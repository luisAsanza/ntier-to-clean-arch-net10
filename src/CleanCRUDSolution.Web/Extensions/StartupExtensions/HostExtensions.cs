using Serilog;

namespace CleanCRUDSolution.Web.Extensions.StartupExtensions
{
    public static class HostExtensions
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, sp, loggerConfiguration) => {
                loggerConfiguration
                .ReadFrom.Services(sp)
                .Enrich.WithMachineName();
            });
        }

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
