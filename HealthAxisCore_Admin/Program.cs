using HealthAxisCore_Admin;
using HealthAxisCore_Admin.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    return new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7056/")
    };
});

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DoctorAdminService>();
builder.Services.AddScoped<UserAdminService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<ThemeService>();
await Task.Delay(1000);
await builder.Build().RunAsync();