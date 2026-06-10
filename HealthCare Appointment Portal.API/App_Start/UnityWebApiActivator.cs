using System.Web.Http;

using Unity.AspNet.WebApi;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(HealthCare_Appointment_Portal.UnityWebApiActivator), nameof(HealthCare_Appointment_Portal.UnityWebApiActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(HealthCare_Appointment_Portal.UnityWebApiActivator), nameof(HealthCare_Appointment_Portal.UnityWebApiActivator.Shutdown))]

namespace HealthCare_Appointment_Portal
{
    public static class UnityWebApiActivator
    {
        public static void Start() 
        {
            var resolver = new UnityDependencyResolver(UnityConfig.Container);

            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
        public static void Shutdown()
        {
            UnityConfig.Container.Dispose();
        }
    }
}