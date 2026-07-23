using HealthAxis_Admin;
using HealthAxis_Admin.Providers;
using HealthAxis_Admin.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddTransient<AuthTokenHandler>();

builder.Services
    .AddHttpClient("HealthAxisApi", client =>
    {
        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    })
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddScoped(serviceProvider =>
    serviceProvider
        .GetRequiredService<IHttpClientFactory>()
        .CreateClient("HealthAxisApi"));

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<ApiAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(serviceProvider =>
    serviceProvider.GetRequiredService<ApiAuthenticationStateProvider>());

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PatientAdminService>();
builder.Services.AddScoped<DoctorAdminService>();
builder.Services.AddScoped<UserAdminService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<AppointmentReportAdminService>();
builder.Services.AddScoped<AdminProfileService>();
await builder.Build().RunAsync();