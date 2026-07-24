using HealthApp.AdminPortal;
using HealthApp.AdminPortal.Auth;
using HealthApp.AdminPortal.Services;
using HealthApp.AdminPortal.Services.Impl;
using HealthApp.AdminPortal.Services.Interface;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// The admin portal is hosted at /admin/, while the API is hosted at /api
// on the same origin. Resolve the site root so requests do not become
// /admin/api/... when the application is deployed.
var adminBaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
var siteRootAddress = new Uri(adminBaseAddress, "/");

builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = siteRootAddress
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAdminDoctorLeaveService, AdminDoctorLeaveService>();

builder.Services.AddScoped<AuthRedirectState>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>());

await builder.Build().RunAsync();