using HealthAxisCore_Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace HealthAxisCore_Api.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        { _logger = logger; }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unexpected error occurred: {Message}", exception.Message);
            var (statusCode, message) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                InvalidException => (StatusCodes.Status400BadRequest, exception.Message),
                UnauthorizedException => (StatusCodes.Status401Unauthorized, exception.Message),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(new { statusCode, message, timeStamp = DateTime.UtcNow, path = httpContext.Request.Path }, cancellationToken);
            return true;
        }
    }
}