using HealthAppWebApi.App_Data;
using HealthAppWebApi.Repositories.Impl;
using HealthAppWebApi.Repositories.Interface;
using HealthAppWebApi.Services.Impl;
using HealthAppWebApi.Services.Interface;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace HealthAppWebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<AppDbContext>(
               new HierarchicalLifetimeManager());

            container.RegisterType<IPatientRepository,
                                   PatientRepository>();

            container.RegisterType<IPatientService,
                                   PatientService>();

            container.RegisterType<IDoctorRepository,
                       DoctorRepository>();

            container.RegisterType<IDoctorService,DoctorService>();

            container.RegisterType<IAppointmentRepository,AppointmentRepository>();

            container.RegisterType<IAppointmentService,AppointmentService>();

            container.RegisterType<IHealthRecordRepository,HealthRecordRepository>();

            container.RegisterType<IHealthRecordService,HealthRecordService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}