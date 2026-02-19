
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
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

            services.AddAuthentication(options =>
                {
                    // The default scheme for [Authorize] checks
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    // The default scheme for sign-in operations
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = "CleanCRUDAuthCookie";
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            // Register Fluent Validation for Web layer
            services.AddValidatorsFromAssemblyContaining<Program>();

            // Register Automapper profiles
            services.AddAutoMapper(cfg => { }, typeof(Program));

            return services;
        }
    }
}
