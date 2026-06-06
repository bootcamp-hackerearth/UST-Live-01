using System.Web.Mvc;
using HealthcareMvcApp.Data;
using HealthcareMvcApp.Repositories;
using HealthcareMvcApp.Repositories.Implementations;
using HealthcareMvcApp.Services;
using HealthcareMvcApp.Services.Implementations;
using Unity;
using Unity.Lifetime;
using Unity.Mvc5;

namespace HealthcareMvcApp.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<HealthcareDbContext>();

            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IDoctorRepository, DoctorRepository>();
            container.RegisterType<IAppointmentRepository, AppointmentRepository>();
            container.RegisterType<IHealthRecordRepository, HealthRecordRepository>();

            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IDoctorService, DoctorService>();
            container.RegisterType<IAppointmentService, AppointmentService>();
            container.RegisterType<IHealthRecordService, HealthRecordService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}