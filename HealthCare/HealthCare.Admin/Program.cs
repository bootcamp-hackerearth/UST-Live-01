using HealthCare.Admin.Handlers;
using HealthCare.Admin.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace HealthCare.Admin
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddSingleton<SessionExpirationService>();
            builder.Services.AddTransient<JwtAuthorizationHandler>();

            builder.Services.AddScoped<HttpClient>(serviceProvider =>
            {
                var jwtHandler = serviceProvider
                    .GetRequiredService<JwtAuthorizationHandler>();

                jwtHandler.InnerHandler = new HttpClientHandler();

                var blazorBaseAddress =
                    new Uri(builder.HostEnvironment.BaseAddress);

                var applicationRoot = new Uri(
                    $"{blazorBaseAddress.Scheme}://{blazorBaseAddress.Authority}/");

                return new HttpClient(jwtHandler)
                {
                    BaseAddress = applicationRoot
                };
            });

            builder.Services.AddScoped<PatientService>();
            builder.Services.AddScoped<DoctorService>();
            builder.Services.AddScoped<AppointmentService>();

            await builder.Build().RunAsync();
        }
    }
}
