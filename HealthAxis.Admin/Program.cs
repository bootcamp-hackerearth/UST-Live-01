using HealthAxis.Admin;
using HealthAxis.Admin.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder =
    WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>(
    "head::after");

// Blazor is hosted at /blazor/.
// Resolve the HttpClient base address to the website root
// so API calls go to /api instead of /blazor/api.
Uri applicationOrigin =
    new(
        new Uri(builder.HostEnvironment.BaseAddress),
        "/");

builder.Services.AddScoped(
    _ => new HttpClient
    {
        BaseAddress = applicationOrigin
    });

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AdminApiService>();

await builder.Build().RunAsync();