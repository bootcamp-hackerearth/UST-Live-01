using HealthAxisAdminLayout;
using HealthAxisAdminLayout.Handlers;
using HealthAxisAdminLayout.Services.Implementations;
using HealthAxisAdminLayout.Services.Implmentations;
using HealthAxisAdminLayout.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ✅ In-memory authentication state
builder.Services.AddScoped<IAuthStateService, AuthStateService>();

// ✅ JWT handler - ready for future API connection
builder.Services.AddScoped<AuthHeaderHandler>();

// ✅ HttpClient configured for HealthAxis API
builder.Services.AddHttpClient("HealthAxisApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7054/");
})
.AddHttpMessageHandler<AuthHeaderHandler>();

// ✅ Default HttpClient used by frontend services
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("HealthAxisApi"));

// ✅ Register frontend services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDoctorApiService, DoctorApiService>();
builder.Services.AddScoped<IPatientApiService, PatientApiService>();
builder.Services.AddScoped<IAppointmentApiService, AppointmentApiService>();
builder.Services.AddScoped<IHealthRecordApiService, HealthRecordApiService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

await builder.Build().RunAsync();