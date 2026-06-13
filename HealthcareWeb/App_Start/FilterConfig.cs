using System.Web.Mvc;
using HealthcareWeb.Filters;

namespace HealthcareWeb
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