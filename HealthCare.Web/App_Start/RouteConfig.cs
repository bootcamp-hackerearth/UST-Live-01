using System.Web.Mvc;
using System.Web.Routing;

namespace HealthCare.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Ignore system routes (required)
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // ✅ DEFAULT ROUTE
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Home",          // ✅ Home page first
                    action = "Index",             // ✅ Dashboard page
                    id = UrlParameter.Optional
                }
            );
        }
    }
}