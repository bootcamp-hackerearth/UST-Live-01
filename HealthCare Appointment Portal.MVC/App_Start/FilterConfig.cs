using System.Web;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
