using System.Net;
using System.Text.Json;

namespace S3_HealthAxisApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(
                    context,
                    ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType =
                "application/json";

            var response = new ErrorResponse
            {
                Message = exception.Message
            };

            switch (exception)
            {
                case KeyNotFoundException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.NotFound;
                    break;

                case ArgumentException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.BadRequest;
                    break;

                case InvalidOperationException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.BadRequest;
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.Unauthorized;
                    break;

                default:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.InternalServerError;

                    response.Message =
                        "An unexpected error occurred.";
                    break;
            }

            var json =
                JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
            = string.Empty;
    }
}