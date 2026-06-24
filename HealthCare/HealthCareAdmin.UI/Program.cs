using HealthCareAdmin.UI;
using HealthCareAdmin.UI.Services;
using HealthCareAdmin.UI.Services.Auth;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<AuthHandler>();
//builder.Services.AddScoped<PatientService>();


builder.Services.AddScoped(sp =>
{
    var http = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7225/")
    };

    return http;
});





await builder.Build().RunAsync();
