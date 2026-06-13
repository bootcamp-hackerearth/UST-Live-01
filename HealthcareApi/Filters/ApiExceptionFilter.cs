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
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred while processing the request.";

            if (context.Exception is EntityNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = context.Exception.Message;
            }
            else if (context.Exception is BusinessRuleException ||
                     context.Exception is AppointmentRuleException ||
                     context.Exception is HealthRecordRuleException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = context.Exception.Message;
            }
            else
            {
                message = context.Exception.Message;
            }

            context.Response = context.Request.CreateResponse(
                statusCode,
                new
                {
                    Message = message
                });
        }
    }
}