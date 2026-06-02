using HealthAxis.Data;
using HealthAxis.Menus;
using HealthAxis.Functions;
using HealthAxis.Repositories;
using HealthAxis.Repositories.Impl;
using HealthAxis.Services;
using HealthAxis.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxis
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddSingleton<AppDbContext>();

            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IHealthRepository, HealthRepository>();

            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IHealthRecordService, HealthRecordService>();

            services.AddScoped<Function>();

            var provider = services.BuildServiceProvider();

            var menu = new MainMenu(
                provider.GetRequiredService<Function>());

            menu.Show();
        }
    }
}