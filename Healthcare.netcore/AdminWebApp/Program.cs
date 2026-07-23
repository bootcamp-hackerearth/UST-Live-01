using AdminWebApp;
using AdminWebApp.Auth;
using AdminWebApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder =
    WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>(
    "head::after");

/*
 * Blazor is hosted under /blazor/,
 * but the API is hosted under /api/.
 *
 * HostEnvironment.BaseAddress:
 * http://localhost:5145/blazor/
 *
 * API base address required:
 * http://localhost:5145/
 */
var applicationBaseAddress =
    new Uri(builder.HostEnvironment.BaseAddress);

var apiBaseAddress =
    new Uri(
        applicationBaseAddress,
        "/");

builder.Services.AddScoped(
    _ => new HttpClient
    {
        BaseAddress = apiBaseAddress
    });

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<AppointmentService>();

builder.Services.AddScoped<
    CustomAuthStateProvider>();

builder.Services.AddScoped<
    AuthenticationStateProvider>(
    serviceProvider =>
        serviceProvider.GetRequiredService<
            CustomAuthStateProvider>());

await builder.Build().RunAsync();