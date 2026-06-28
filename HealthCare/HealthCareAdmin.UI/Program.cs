using HealthCareAdmin.UI;
using HealthCareAdmin.UI.Services;
using HealthCareAdmin.UI.Services.Auth;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");



builder.Services.AddScoped<AuthHandler>();

var apiBase = new Uri("https://localhost:7225/");
builder.Services.AddHttpClient<AuthService>(c =>
{
    c.BaseAddress = apiBase;
});

builder.Services.AddHttpClient<AppointmentService>(c => c.BaseAddress = apiBase)
                .AddHttpMessageHandler<AuthHandler>();

builder.Services.AddHttpClient<DoctorService>(c => c.BaseAddress = apiBase)
                .AddHttpMessageHandler<AuthHandler>();

builder.Services.AddHttpClient<PatientService>(c => c.BaseAddress = apiBase)
                .AddHttpMessageHandler<AuthHandler>();

await builder.Build().RunAsync();
