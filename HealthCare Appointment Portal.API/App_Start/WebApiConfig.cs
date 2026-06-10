using HealthCare_Appointment_Portal.Filters;
using Newtonsoft.Json;
using System.Web.Http;

namespace HealthCare_Appointment_Portal
{
    public static class WebApiConfig
    {
        public static void Register(
            HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter
                .SerializerSettings
                .ReferenceLoopHandling =
                    ReferenceLoopHandling.Ignore;

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate:
                    "api/{controller}/{id}",
                defaults:
                    new
                    {
                        id =
                            RouteParameter.Optional
                    });

            config.Filters.Add(
                new GlobalExceptionFilterAttribute());
        }
    }
}