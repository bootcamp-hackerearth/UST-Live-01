using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using HealthCare_Appointment_Portal_MVC.Services;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;

namespace HealthCare_Appointment_Portal
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container =
                new UnityContainer();

            // API Services

            container.RegisterType<
                IAppointmentApiService,
                AppointmentApiService>();

            container.RegisterType<
                IDoctorApiService,
                DoctorApiService>();

            container.RegisterType<
                IPatientApiService,
                PatientApiService>();

            container.RegisterType<
                IHealthRecordApiService,
                HealthRecordApiService>();

            container.RegisterType<
                IUserApiService,
                UserApiService>();

            DependencyResolver.SetResolver(
                new UnityDependencyResolver(
                    container));
        }
    }
}