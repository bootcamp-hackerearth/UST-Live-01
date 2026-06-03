using HealthAxis.Data;
using HealthAxis.Functions;
using HealthAxis.Menus;
using HealthAxis.Repository;
using HealthAxis.Repository.Implementation;
using HealthAxis.Service;
using HealthAxis.Service.Implementation;
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

            services.AddSingleton<Database>();

            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();

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