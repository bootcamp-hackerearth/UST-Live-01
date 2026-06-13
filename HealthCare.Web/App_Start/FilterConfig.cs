using HealthCare.Web;
using HealthCare.Web.Filters;
using System.Web;
using System.Web.Mvc;

namespace HealthCare.Web
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new GlobalExceptionFilterAttribute());
        }
    }
}
