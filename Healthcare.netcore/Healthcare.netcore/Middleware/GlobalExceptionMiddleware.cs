
using HealthAxis.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace HealthAxis.API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception,
                "An error occurred while processing request: {Message}",
                exception.Message);

            var (statusCode, message) = exception switch
            {
                NotFoundException =>
                    (StatusCodes.Status404NotFound, exception.Message),

                ValidationException =>
                    (StatusCodes.Status400BadRequest, exception.Message),

                BusinessRuleException =>
                    (StatusCodes.Status409Conflict, exception.Message),

                AppException =>
                    (StatusCodes.Status400BadRequest, exception.Message),

                UnauthorizedAccessException =>
                    (StatusCodes.Status401Unauthorized, exception.Message),

                _ =>
                    (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                TimeStamp = DateTime.UtcNow,
                Path = httpContext.Request.Path
            };

            await httpContext.Response.WriteAsJsonAsync(
                response,
                cancellationToken);

            return true;
        }
    }
}