using System.Globalization;
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
                // NOTE: This 30-minute sliding expiration is an idle timeout.
                // The Login/Register actions set an AbsoluteExpirationTicks of 5 hours
                // to enforce a hard maximum session lifetime. The shorter sliding timeout
                // here is intentional and complements the longer absolute expiration.
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
                options.Events.OnValidatePrincipal = context =>
                {
                    // 1. Validate Security Stamp or other user data here if needed (TODO)

                    // 2. If identity rejected the Principal
                    if(context.Principal == null)
                    {
                        return Task.CompletedTask;
                    }

                    // 3. If AbsoluteExpirationTicks is not present, reject the Principal
                    if (!context.Properties.Items.TryGetValue("AbsoluteExpirationTicks", out var absoluteExpirationTicksString))
                    {
                        context.RejectPrincipal();
                        return Task.CompletedTask;
                    }

                    // 4. If AbsoluteExpirationTicks is present but invalid, reject the Principal
                    if (!long.TryParse(absoluteExpirationTicksString, out var absoluteExpirationTicks))
                    {
                        context.RejectPrincipal();
                        return Task.CompletedTask;
                    }

                    if (absoluteExpirationTicks < DateTime.UtcNow.Ticks)
                    {
                        context.RejectPrincipal();
                        return Task.CompletedTask;
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
