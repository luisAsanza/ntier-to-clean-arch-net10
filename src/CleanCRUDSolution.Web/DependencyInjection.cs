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
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "CleanCRUDAuthCookie";
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.Events.OnValidatePrincipal = context =>
                {
                    // Validate AuthenticationProperties Items AbsoluteExpiration
                    if (context.Properties.Items.TryGetValue("AbsoluteExpiration", out var absoluteExpirationString) &&
                        DateTime.TryParse(absoluteExpirationString, null, System.Globalization.DateTimeStyles.RoundtripKind, out var absoluteExpiration))
                    {
                        if (DateTime.UtcNow > absoluteExpiration)
                        {
                            context.RejectPrincipal();
                            return Task.CompletedTask;
                        }
                    }
                    
                    return Task.CompletedTask;
                };
            });

            // Register Fluent Validation for Web layer
            services.AddValidatorsFromAssemblyContaining<Program>();

            // Register Automapper profiles
            services.AddAutoMapper(cfg => { }, typeof(Program));

            return services;
        }
    }
}
