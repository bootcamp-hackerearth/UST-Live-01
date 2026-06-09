using System.Net.Http;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using System;
using HealthAxisWeb.Services;

namespace HealthAxisWeb
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44315/")

            };
            container.RegisterInstance<HttpClient>(httpClient); //httpclient is like a singleton
            container.RegisterType<IDoctorApiService, DoctorApiService>();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}