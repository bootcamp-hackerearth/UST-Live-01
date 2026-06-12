using HealthAxisApp.Data;
using HealthAxisApp.Repositories;
using HealthAxisApp.Repositories.Impl;
using HealthAxisApp.Repositories.Impl.HealthAxisApp.Repositories.Impl;
using HealthAxisApp.Services;
using HealthAxisApp.Services.Impl;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.WebApi;

namespace HealthAxisApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<HealthAxisEntities2>();
            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IDoctorRepository, DoctorRepository>();
            container.RegisterType<IAppointmentRepository, AppointmentRepository>();
            container.RegisterType<IHealthRecordRepository, HealthRecordRepository>();
            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IDoctorService, DoctorService>();
            container.RegisterType<IAppointmentService, AppointmentService>();
            container.RegisterType<IHealthRecordService, HealthRecordService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}