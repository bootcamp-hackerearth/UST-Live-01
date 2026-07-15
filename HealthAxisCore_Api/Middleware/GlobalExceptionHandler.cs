using HealthAxisCore_Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace HealthAxisCore_Api.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (statusCode, message) = exception switch
            {
                NotFoundException => (
                    StatusCodes.Status404NotFound,
                    exception.Message),

                InvalidException => (
                    StatusCodes.Status400BadRequest,
                    exception.Message),

                UnauthorizedException => (
                    StatusCodes.Status401Unauthorized,
                    exception.Message),

                UnauthorizedAccessException => (
                    StatusCodes.Status401Unauthorized,
                    exception.Message),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred")
            };

            if (statusCode >= 500)
            {
                _logger.LogError(
                    exception,
                    "[EXCEPTION] Request failed | Method={Method} | Path={Path} | StatusCode={StatusCode} | Error={ErrorMessage}",
                    httpContext.Request.Method,
                    httpContext.Request.Path,
                    statusCode,
                    exception.Message);
            }
            else
            {
                _logger.LogWarning(
                    "[REQUEST-BLOCKED] Request rejected | Method={Method} | Path={Path} | StatusCode={StatusCode} | Reason={ErrorMessage}",
                    httpContext.Request.Method,
                    httpContext.Request.Path,
                    statusCode,
                    exception.Message);
            }

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(
                new
                {
                    statusCode,
                    message,
                    timeStamp = DateTime.UtcNow,
                    path = httpContext.Request.Path
                },
                cancellationToken);

            return true;
        }
    }
}