using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace HealthAppWebAPI.Handlers
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var exception = context.Exception;

            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An internal server error has occurred.";

            if (exception is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
            }
            else if (exception is InvalidOperationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
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
                var response = Request.CreateResponse(
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