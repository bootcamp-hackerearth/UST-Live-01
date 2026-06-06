using System.Web.Mvc;
using HealthcareMvcApp.Filters;

namespace HealthcareMvcApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MvcExceptionFilterAttribute());
        }
    }
}