using HealthAxisCore_Admin;
using HealthAxisCore_Admin.Providers;
using HealthAxisCore_Admin.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder =
    WebAssemblyHostBuilder.CreateDefault(args);

// ============================================================
// Root components
// ============================================================

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>(
    "head::after");

// ============================================================
// API HttpClient
// ============================================================

// CHANGED:
//
// builder.HostEnvironment.BaseAddress is:
//
// Local development:
// http://localhost:5072/
//
// Elastic Beanstalk:
// http://elastic-beanstalk-host/blazor/
//
// The API itself is hosted at the site root:
//
// /api/auth/login
// /api/admin-handoff/exchange
// /api/doctors
//
// Therefore, HttpClient.BaseAddress must be the site origin rather
// than the /blazor/ application path.
//
// This also protects services that use relative URLs such as:
//
// api/admin-handoff/exchange
//
// from accidentally calling:
//
// /blazor/api/admin-handoff/exchange
builder.Services.AddScoped(_ =>
{
    var blazorBaseAddress =
        new Uri(builder.HostEnvironment.BaseAddress);

    var siteOrigin =
        new Uri(
            blazorBaseAddress.GetLeftPart(
                UriPartial.Authority) + "/");

    return new HttpClient
    {
        BaseAddress = siteOrigin
    };
});

// ============================================================
// Blazor authorization
// ============================================================

builder.Services.AddAuthorizationCore();

// ============================================================
// HealthAxis services
// ============================================================

builder.Services.AddScoped<AdminHandoffService>();

builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<DoctorAdminService>();

builder.Services.AddScoped<UserAdminService>();

builder.Services.AddScoped<AppointmentReportService>();

builder.Services.AddScoped<ThemeService>();

// ============================================================
// Authentication state provider
// ============================================================

builder.Services.AddScoped<
    ApiAuthenticationStateProvider>();

builder.Services.AddScoped<
    AuthenticationStateProvider>(
    serviceProvider =>
    {
        return serviceProvider
            .GetRequiredService<
                ApiAuthenticationStateProvider>();
    });

// ============================================================
// Build and run Blazor WebAssembly
// ============================================================

var host = builder.Build();

await host.RunAsync();