using System.Linq;
using System.Web.Http;
using WebActivatorEx;
using HealthcareApi;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace HealthcareApi
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "HealthcareApi");

                    c.DescribeAllEnumsAsStrings();

                    c.ResolveConflictingActions(apiDescriptions =>
                        apiDescriptions.First());
                })
                .EnableSwaggerUi(c =>
                {
                    c.DocExpansion(DocExpansion.List);
                });
        }
    }
}