using HealthAxisAdminLayout;
using HealthAxisAdminLayout.Auth;
using HealthAxisAdminLayout.Handlers;
using HealthAxisAdminLayout.Services.Implementations;
using HealthAxisAdminLayout.Services.Interfaces;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder =
    WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>(
    "head::after");

builder.Services.AddScoped<
    IAuthStateService,
    AuthStateService>();

builder.Services.AddScoped<
    AuthHeaderHandler>();

var applicationUri =
    new Uri(
        builder.HostEnvironment.BaseAddress);

var apiBaseUri =
    new Uri(
        applicationUri.GetLeftPart(
            UriPartial.Authority) +
        "/api/");

Console.WriteLine(
    $"BaseAddress={builder.HostEnvironment.BaseAddress}");

Console.WriteLine(
    $"ResolvedApiBaseUri={apiBaseUri}");

builder.Services.AddHttpClient(
        "HealthAxisApi",
        client =>
        {
            client.BaseAddress =
                apiBaseUri;
        })
    .AddHttpMessageHandler<
        AuthHeaderHandler>();


builder.Services.AddScoped(
    serviceProvider =>
        serviceProvider
            .GetRequiredService<
                IHttpClientFactory>()
            .CreateClient(
                "HealthAxisApi"));

builder.Services.AddScoped<
    IAuthService,
    AuthService>();

builder.Services.AddScoped<
    IDoctorApiService,
    DoctorApiService>();

builder.Services.AddScoped<
    IPatientApiService,
    PatientApiService>();

builder.Services.AddScoped<
    IAppointmentApiService,
    AppointmentApiService>();

builder.Services.AddScoped<
    IHealthRecordApiService,
    HealthRecordApiService>();

builder.Services.AddScoped<
    IDashboardService,
    DashboardService>();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<
    AuthenticationStateProvider,
    CustomAuthenticationStateProvider>();

await builder.Build().RunAsync();




static Uri EnsureTrailingSlash(
    Uri uri)
{
    var uriValue =
        uri.ToString();

    if (uriValue.EndsWith(
        "/",
        StringComparison.Ordinal))
    {
        return uri;
    }

    return new Uri(
        uriValue + "/");
}

