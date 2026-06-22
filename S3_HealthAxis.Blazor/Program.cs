using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using S3_HealthAxis.Blazor;
using S3_HealthAxis.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddTransient<TokenAuthenticationHandler>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7258/")
});

builder.Services.AddHttpClient("HealthAxisAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7258/");
}).AddHttpMessageHandler<TokenAuthenticationHandler>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IAdminService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var client = httpClientFactory.CreateClient("HealthAxisAPI");
    return new AdminService(client);
});

builder.Services.AddScoped<IDoctorService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var client = httpClientFactory.CreateClient("HealthAxisAPI");
    return new DoctorService(client);
});

await builder.Build().RunAsync();