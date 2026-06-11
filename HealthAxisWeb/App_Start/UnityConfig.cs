using System.Net.Http;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using System;
using HealthAxis.Web.Services;

namespace HealthAxisWeb
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44315/api/")

            };
            container.RegisterInstance<HttpClient>(httpClient); //httpclient is like a singleton
            container.RegisterType<IDoctorApiService, DoctorApiService>();
            container.RegisterType<IPatientApiService, PatientApiService>();
            container.RegisterType<IAppointmentApiService, AppointmentApiService>();
            container.RegisterType<IHealthRecordApiService, HealthRecordApiService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}