using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace HealthcareWeb.Filters
{
    public class MvcExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            string message = "An unexpected error occurred. Please try again.";

            if (!string.IsNullOrWhiteSpace(filterContext.Exception.Message))
            {
                message = filterContext.Exception.Message;
            }

            filterContext.Controller.TempData["ErrorMessage"] = message;

            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "Index" }
                });

            filterContext.ExceptionHandled = true;
        }
    }
}