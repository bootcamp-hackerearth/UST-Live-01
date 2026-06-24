using HealthApp.Blazor.Components;
using HealthApp.Blazor.Components.service.Impl;
using HealthApp.Blazor.Components.service.Interface;
using HealthApp.Blazor.Components.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<AppointmentService>();

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();





builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7066/")
});

builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();