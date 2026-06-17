using HealthApp.Api.Dtos;
using Microsoft.AspNetCore.Diagnostics;
using HealthApp.Api.Exceptions;

namespace HealthApp.Api.Handler
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An Unexpected Error Occurred : {Message}", exception.Message);

            var (statusCode, message) = exception switch
            {

                EntityNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                HealthAppException => (StatusCodes.Status400BadRequest, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, exception.Message)

            };

            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                TimeStamp = DateTime.UtcNow,
                Path = httpContext.Request.Path

            };
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(response);

            return true;
        }
    }
}
