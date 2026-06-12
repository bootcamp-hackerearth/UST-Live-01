using System.Web.Http;
using WebActivatorEx;
using HealthCareApi;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace HealthCareApi
{
    public static class SwaggerConfig
    {
        public static void Register()
        {

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        
                        c.SingleApiVersion("v1", "HealthCareApi");

                        
                    })
                .EnableSwaggerUi(c =>
                    {
                        
                        
                    });
        }
    }
}
