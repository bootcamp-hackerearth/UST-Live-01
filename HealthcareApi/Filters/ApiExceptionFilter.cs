using HealthcareApi;
using HealthcareApi.Exceptions;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace HealthcareApi.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is EntityNotFoundException)
            {
                context.Response = context.Request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    context.Exception.Message);

                return;
            }

            if (context.Exception is HealthcareAppException)
            {
                context.Response = context.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    context.Exception.Message);

                return;
            }

            context.Response = context.Request.CreateErrorResponse(
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred.");
        }
    }
}

