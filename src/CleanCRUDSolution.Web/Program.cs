using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;
using CleanCRUDSolution.Web.Extensions.StartupExtensions;
using CleanCRUDSolution.Infrastructure;
using CleanCRUDSolution.Web;
using CleanCRUDSolution.Application;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 1. Configure Host
if (builder.Environment.IsEnvironment("Testing"))
{
    builder.ConfigureSerilogForTesting();
}
else
{
    builder.ConfigureSerilog();
}

// 2. Services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices(builder.Configuration);

var app = builder.Build();

// Configure the error handling middleware using the built-in ExceptionHandler middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");

    // Use HSTS in production
    //app.UseHsts();
}

// Use Serilog custom middleware to enrich logs with Username
app.Use(async (ctx, next) =>
{
    var isAuthenticated = ctx.User.Identity?.IsAuthenticated == true;
    var userName = isAuthenticated ? ctx.User.Identity?.Name : "Anonymous";
    var diagnosticContext = ctx.RequestServices.GetRequiredService<IDiagnosticContext>();
    diagnosticContext.Set("Username", userName!);

    await next();
});

// Log every request using Serilog. app.UseHttpLogging(); can be removed so it won't generate http requests logs twice
app.UseSerilogRequestLogging();

app.Logger.LogDebug("Adding Csp configuration");

//Add csp to responses
if (app.Environment.IsDevelopment())
{
    // Option A: no CSP
    // Option B: very relaxed CSP or Report-Only
    app.UseCspReportOnly();
}
else if (app.Environment.IsStaging())
{
    // Real policy, but report-only to see violations
    app.UseCspReportOnly();
}
else if (app.Environment.IsProduction())
{
    // Real, enforced CSP
    app.UseCsp();
}

app.Logger.LogDebug("End of Csp configuration");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.UseRotativa();

app.Run();

public partial class Program { } //Make the Program class public for integration testing