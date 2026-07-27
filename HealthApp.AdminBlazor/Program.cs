using HealthApp.AdminBlazor;
using HealthApp.AdminBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder =
    WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>(
    "head::after");

var applicationBaseAddress =
    new Uri(builder.HostEnvironment.BaseAddress);

var applicationOrigin =
    applicationBaseAddress.GetLeftPart(
        UriPartial.Authority);

var apiBaseAddress =
    new Uri($"{applicationOrigin}/");

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<
    TokenStorageService>();

builder.Services.AddScoped<
    AdminAuthenticationStateProvider>();

builder.Services.AddScoped<
    AuthenticationStateProvider>(
        serviceProvider =>
            serviceProvider.GetRequiredService<
                AdminAuthenticationStateProvider>());

builder.Services.AddScoped<
    AdminAuthorizationMessageHandler>();

builder.Services.AddScoped(
    serviceProvider =>
    {
        var tokenStorageService =
            serviceProvider.GetRequiredService<
                TokenStorageService>();

        var authorizationHandler =
            new AdminAuthorizationMessageHandler(
                tokenStorageService)
            {
                InnerHandler =
                    new HttpClientHandler()
            };

        return new HttpClient(
            authorizationHandler)
        {
            BaseAddress =
                apiBaseAddress
        };
    });

builder.Services.AddScoped<
    AuthApiService>();

builder.Services.AddScoped<
    AdminApiService>();

await builder.Build().RunAsync();