using HealthCareApp.AdminBlazor;
using HealthCareApp.AdminBlazor.Services.Impl;
using HealthCareApp.AdminBlazor.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    });

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IDoctorAdminService, DoctorAdminService>();
builder.Services.AddScoped<IPatientAdminService, PatientAdminService>();
builder.Services.AddScoped<IAppointmentAdminService, AppointmentAdminService>();
await builder.Build().RunAsync();