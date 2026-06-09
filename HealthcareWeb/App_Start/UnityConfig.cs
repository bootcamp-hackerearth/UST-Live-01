using HealthcareWeb.Services;
using System;
using System.Net.Http;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace HealthcareWeb
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44364/api/")
            };


            // Register service interfaces with their concrete implementations
            container.RegisterType<IDoctorApiService, DoctorApiService>();
            container.RegisterType<IPatientApiService, PatientApiService>();
            container.RegisterType<IAppointmentApiService, AppointmentApiService>();
            container.RegisterType<IHealthRecordApiService, HealthRecordApiService>();

            container.RegisterInstance<HttpClient>(httpClient);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
