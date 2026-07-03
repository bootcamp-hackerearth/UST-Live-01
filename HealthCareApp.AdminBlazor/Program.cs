using HealthCareApp.AdminBlazor;
using HealthCareApp.AdminBlazor.Auth;
using HealthCareApp.AdminBlazor.Services.Impl;
using HealthCareApp.AdminBlazor.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];

if (string.IsNullOrWhiteSpace(apiBaseUrl))
{
    throw new InvalidOperationException("ApiSettings:BaseUrl is missing from appsettings.json.");
}

builder.Services.AddScoped(_ =>
    new HttpClient
    {
        BaseAddress = new Uri(apiBaseUrl)
    });

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(serviceProvider =>
    serviceProvider.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IDoctorAdminService, DoctorAdminService>();
builder.Services.AddScoped<IPatientAdminService, PatientAdminService>();
builder.Services.AddScoped<IAppointmentAdminService, AppointmentAdminService>();
builder.Services.AddScoped<IToastService, ToastService>();

await builder.Build().RunAsync();