using ASP.C_.Users._287802.source.repos.Sprint4_HealthAxis.HealthAxisAdminLayout;
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

var configuredApiBaseUrl =
    builder.Configuration[
        "ApiSettings:BaseUrl"];

if (string.IsNullOrWhiteSpace(
    configuredApiBaseUrl))
{
    throw new InvalidOperationException(
        "The HealthAxis API base URL is missing.");
}

var apiBaseUri =
    ResolveApiBaseUri(
        builder.HostEnvironment.BaseAddress,
        configuredApiBaseUrl);

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

static Uri ResolveApiBaseUri(
    string applicationBaseAddress,
    string configuredApiBaseUrl)
{
    if (
        Uri.TryCreate(
            configuredApiBaseUrl,
            UriKind.Absolute,
            out var absoluteApiUri)
    )
    {
        return EnsureTrailingSlash(
            absoluteApiUri);
    }

    var applicationUri =
        new Uri(applicationBaseAddress);

    var applicationOrigin =
        new Uri(
            applicationUri.GetLeftPart(
                UriPartial.Authority) + "/");

    var relativeApiPath =
        configuredApiBaseUrl
            .TrimStart('/');

    var resolvedApiUri =
        new Uri(
            applicationOrigin,
            relativeApiPath);

    return EnsureTrailingSlash(
        resolvedApiUri);
}

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