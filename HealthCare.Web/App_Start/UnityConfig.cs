using System.Web.Http;
using HealthCare.Web.Services;
using HealthCare.Web.Services.Interfaces;
using System.Web.Mvc;
using Unity;
using Unity.WebApi;

namespace HealthCare.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IDoctorService, DoctorService>();
            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IAppointmentService, AppointmentService>();
            container.RegisterType<IHealthRecordService, HealthRecordService>();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(
            new Unity.Mvc5.UnityDependencyResolver(container)

        );
        }
    }
}