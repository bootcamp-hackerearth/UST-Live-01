using HealthCare.Web.Services;
using HealthCare.Web.Services.Interfaces;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace HealthCare.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

           
            container.RegisterType<IDoctorService, DoctorService>();
            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IAppointmentService, AppointmentService>();
            container.RegisterType<IHealthRecordService, HealthRecordService>();

 

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}