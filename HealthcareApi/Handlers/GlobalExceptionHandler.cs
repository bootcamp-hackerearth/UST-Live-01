using HealthcareApi.Exceptions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace HealthcareApi.Handlers
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred while processing the request.";

            if (context.Exception is EntityNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = context.Exception.Message;
            }
            else if (context.Exception is HealthcareAppException ||
                     context.Exception is BusinessRuleException ||
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

            context.Result = new ErrorResult
            {
                Request = context.Request,
                StatusCode = statusCode,
                Message = message
            };
        }

        private class ErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public HttpStatusCode StatusCode { get; set; }

            public string Message { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(
                CancellationToken cancellationToken)
            {
                HttpResponseMessage response =
                    Request.CreateResponse(
                        StatusCode,
                        new
                        {
                            message = Message
                        });

                return Task.FromResult(response);
            }
        }
    }
}