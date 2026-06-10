using System.Web.Http;
using WebActivatorEx;
using HealthCare_Appointment_Portal;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace HealthCare_Appointment_Portal
{
    public static class SwaggerConfig
    {
        public static void Register()
        { 
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "HealthCare_Appointment_Portal");
                    })
                .EnableSwaggerUi(c =>
                    {
                    });
        }
    }
}
