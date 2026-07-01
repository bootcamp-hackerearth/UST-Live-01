using HealthAxisCore_Admin;
using HealthAxisCore_Admin.Providers;
using HealthAxisCore_Admin.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

if (string.IsNullOrWhiteSpace(apiBaseUrl))
{
    apiBaseUrl = "https://localhost:7056/";
}

builder.Services.AddScoped(sp =>
{
    return new HttpClient
    {
        BaseAddress = new Uri(apiBaseUrl)
    };
});

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AdminHandoffService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DoctorAdminService>();
builder.Services.AddScoped<UserAdminService>();
builder.Services.AddScoped<AppointmentReportService>();
builder.Services.AddScoped<ThemeService>();

builder.Services.AddScoped<ApiAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(serviceProvider =>
{
    return serviceProvider.GetRequiredService<ApiAuthenticationStateProvider>();
});
await Task.Delay(2000);
await builder.Build().RunAsync();