using System;
using System.Web.Mvc;
using HealthcareMvcApp.Exceptions;

namespace HealthcareMvcApp.Filters
{
    public class MvcExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            if (filterContext.Exception is HealthcareAppException)
            {
                filterContext.Controller.TempData["ErrorMessage"] =
                    filterContext.Exception.Message;

                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Home" },
                        { "action", "Index" }
                    });

                filterContext.ExceptionHandled = true;
                return;
            }

            filterContext.Controller.TempData["ErrorMessage"] =
                "An unexpected error occurred. Please try again.";

            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "Index" }
                });

            filterContext.ExceptionHandled = true;
        }
    }
}