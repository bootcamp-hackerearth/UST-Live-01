using Microsoft.AspNetCore.Diagnostics;
using HealthApp.Api.Exceptions;

namespace HealthApp.Api.Middleware
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
                "An unexpected Error Occured : {Message}",
                exception.Message);

            var (statusCode, message) = exception switch
            {
                EntityNotFoundException ex =>
                    (StatusCodes.Status404NotFound, ex.Message),

                ConflictException ex =>
                    (StatusCodes.Status409Conflict, ex.Message),

                AppointmentRuleException ex =>
                    (StatusCodes.Status400BadRequest, ex.Message),

                HealthRecordRuleException ex =>
                    (StatusCodes.Status400BadRequest, ex.Message),

                BusinessRuleException ex =>
                    (StatusCodes.Status400BadRequest, ex.Message),

                HealthCareAppException ex =>
                    (StatusCodes.Status400BadRequest, ex.Message),

                _ =>
                    (StatusCodes.Status500InternalServerError,
                        "Something went wrong. Please try again later.")
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = statusCode,
                Message = message,
                Path = httpContext.Request.Path.ToString(),
                TimeStamp = DateTime.UtcNow
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}