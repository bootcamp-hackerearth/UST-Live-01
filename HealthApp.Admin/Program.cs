using HealthApp.Admin;
using HealthApp.Admin.Services;
using HealthApp.Admin.Services.Impl;
using HealthApp.Admin.Services.Interface;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Blazor is hosted at:
// https://localhost:7037/blazor/
//
// API is hosted at:
// https://localhost:7037/api/...
//
// So this converts BaseAddress from /blazor/ to site root /
builder.Services.AddScoped(sp =>
{
    var uri = new Uri(builder.HostEnvironment.BaseAddress);

    var siteRoot = $"{uri.Scheme}://{uri.Authority}/";

    return new HttpClient
    {
        BaseAddress = new Uri(siteRoot)
    };
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDoctorLeaveService, DoctorLeaveService>();

await builder.Build().RunAsync();