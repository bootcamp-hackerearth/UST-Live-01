using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using Healthaxis2.Web.Services.Interfaces;
using Healthaxis2.Web.Services;

namespace Healthaxis2.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // ✅ Register MVC services
            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IDoctorService, DoctorService>();
            container.RegisterType<IAppointmentService, AppointmentService>();
            container.RegisterType<IHealthRecordService, HealthRecordService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
