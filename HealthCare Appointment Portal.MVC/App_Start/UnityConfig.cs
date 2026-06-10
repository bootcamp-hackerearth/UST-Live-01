using System.Web.Mvc;
using Unity;
using Unity.Injection;
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

            container.RegisterType<
                IAppointmentApiService,
                AppointmentApiService>(
                    new InjectionConstructor());

            container.RegisterType<
                IDoctorApiService,
                DoctorApiService>(
                    new InjectionConstructor());

            container.RegisterType<
                IPatientApiService,
                PatientApiService>(
                    new InjectionConstructor());

            container.RegisterType<
                IHealthRecordApiService,
                HealthRecordApiService>(
                    new InjectionConstructor());

            DependencyResolver.SetResolver(
                new UnityDependencyResolver(
                    container));
        }
    }
}