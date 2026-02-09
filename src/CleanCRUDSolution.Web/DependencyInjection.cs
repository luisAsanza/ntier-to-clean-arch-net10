using FluentValidation;
using Microsoft.AspNetCore.Http.Features;

namespace CleanCRUDSolution.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            //Configure file upload limit
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50MB
            });

            // MVC with client-side validation enabled
            services.AddControllersWithViews()
                .AddViewOptions(vo => vo.HtmlHelperOptions.ClientValidationEnabled = true);

            // Error page is a Razor Page
            services.AddRazorPages();

            // Add routing with lowercase URLs
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            // Register Fluent Validation for Web layer
            services.AddValidatorsFromAssemblyContaining<Program>();

            // Register Automapper profiles
            services.AddAutoMapper(cfg => { }, typeof(Program));

            return services;
        }
    }
}
