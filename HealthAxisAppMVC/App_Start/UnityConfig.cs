using HealthAxisAppMVC.Services;
using HealthAxisAppMVC.Services.Interfaces;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace HealthAxisAppMVC
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IPatientMvcService, PatientMvcService>();
            container.RegisterType<IDoctorMvcService, DoctorMvcService>();
            container.RegisterType<IAppointmentMvcService, AppointmentMvcService>();
            container.RegisterType<IHealthRecordMvcService, HealthRecordMvcService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}