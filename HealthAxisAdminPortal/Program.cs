using HealthAxisAdminPortal;
using HealthAxisAdminPortal.Services.FrontEndMemory;
using HealthAxisAdminPortal.Services.Implementation;
using HealthAxisAdminPortal.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IDoctorApiService, DoctorApiService>();
builder.Services.AddScoped<IAuthApiService, AuthApiService>();
builder.Services.AddScoped<IPatientApiService, PatientApiService>();

builder.Services.AddScoped(sp =>
{
    var client = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7038/")
    };

    if (!string.IsNullOrEmpty(TokenStore.AccessToken))
    {
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                TokenStore.AccessToken
            );
    }

    return client;
});
await builder.Build().RunAsync();
