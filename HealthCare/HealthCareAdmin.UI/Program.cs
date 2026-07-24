using HealthCareAdmin.UI.Services;
using HealthCareAdmin.UI.Services.Auth;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace HealthCareAdmin.UI
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped<AuthHandler>();

            var blazorBaseUri =
                new Uri(builder.HostEnvironment.BaseAddress);

            var apiBaseUri =
                new Uri(
                    $"{blazorBaseUri.Scheme}://{blazorBaseUri.Authority}/");

            builder.Services.AddHttpClient<AuthService>(client =>
            {
                client.BaseAddress = apiBaseUri;
            });

            builder.Services
                .AddHttpClient<AppointmentService>(client =>
                {
                    client.BaseAddress = apiBaseUri;
                })
                .AddHttpMessageHandler<AuthHandler>();

            builder.Services
                .AddHttpClient<DoctorService>(client =>
                {
                    client.BaseAddress = apiBaseUri;
                })
                .AddHttpMessageHandler<AuthHandler>();

            builder.Services
                .AddHttpClient<PatientService>(client =>
                {
                    client.BaseAddress = apiBaseUri;
                })
                .AddHttpMessageHandler<AuthHandler>();

            await builder.Build().RunAsync();
        }
    }
}