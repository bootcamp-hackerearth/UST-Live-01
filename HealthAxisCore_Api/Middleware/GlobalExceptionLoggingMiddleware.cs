using System.Net;
using System.Text.Json;

namespace HealthAxisCore_Api.Middleware
{
    public class GlobalExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionLoggingMiddleware> _logger;

        public GlobalExceptionLoggingMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred while processing {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "An unexpected error occurred.",
                    statusCode = context.Response.StatusCode
                };

                var jsonResponse = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}