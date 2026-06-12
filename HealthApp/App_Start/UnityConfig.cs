using HealthApp.Service.Impl;
using HealthApp.Service.Interface;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace HealthApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<IPatientApiService, PatientApiService>();
            container.RegisterType<IDoctorApiService, DoctorApiService>();
            container.RegisterType<IHealthRecordApiService, HealthRecordApiService>();
            container.RegisterType<IAppointmentApiService, AppointmentApiService>();


            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}