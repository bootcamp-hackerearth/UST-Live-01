using System;
using System.Net;
using System.Web.Mvc;

public class GlobalExceptionFilter : HandleErrorAttribute
{
    public override void OnException(ExceptionContext filterContext)
    {
        if (filterContext.ExceptionHandled)
            return;

        var exception = filterContext.Exception;

        // ✅ Default values
        int statusCode = (int)HttpStatusCode.InternalServerError;
        string message = "Something went wrong.";

        // ✅ Custom handling
        if (exception.Message.Contains("not found"))
        {
            statusCode = (int)HttpStatusCode.NotFound;
            message = "The requested resource was not found.";
        }
        else if (exception.Message.Contains("already booked"))
        {
            message = exception.Message; // keep API message
        }

        // ✅ Set response
        filterContext.Result = new ViewResult
        {
            ViewName = statusCode == 404 ? "NotFound" : "Error",
            ViewData = new ViewDataDictionary
            {
                { "ErrorMessage", message }
            }
        };

        filterContext.HttpContext.Response.StatusCode = statusCode;
        filterContext.ExceptionHandled = true;
    }
}