using System.Web.Mvc;
using System.Web.Routing;

namespace HealthcareWeb.Filters
{
    public class RequirePositiveIntParametersAttribute : ActionFilterAttribute
    {
        private readonly string[] _parameterNames;

        public string RedirectController { get; set; }

        public string RedirectAction { get; set; }

        public string ErrorMessage { get; set; }

        public RequirePositiveIntParametersAttribute(params string[] parameterNames)
        {
            _parameterNames = parameterNames;

            RedirectController = "Home";
            RedirectAction = "Index";
            ErrorMessage = "Please use a valid link to access this page.";
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            foreach (string parameterName in _parameterNames)
            {
                object value;

                bool parameterExists =
                    filterContext.ActionParameters.TryGetValue(parameterName, out value);

                if (!parameterExists || value == null)
                {
                    Redirect(filterContext);
                    return;
                }

                int parsedValue;

                bool isValidInteger =
                    int.TryParse(value.ToString(), out parsedValue);

                if (!isValidInteger || parsedValue <= 0)
                {
                    Redirect(filterContext);
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }

        private void Redirect(ActionExecutingContext filterContext)
        {
            filterContext.Controller.TempData["ErrorMessage"] = ErrorMessage;

            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", RedirectController },
                    { "action", RedirectAction }
                });
        }
    }
}