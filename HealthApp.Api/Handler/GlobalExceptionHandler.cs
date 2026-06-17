using HealthApp.Api.Dtos;
using HealthApp.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace HealthApp.Api.Handler
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
                "An error occurred while processing the request. Message: {Message}",
                exception.Message);

            var (statusCode, message) = exception switch
            {
                InvalidRequestException => (
                    StatusCodes.Status400BadRequest,
                    exception.Message),

                EntityNotFoundException => (
                    StatusCodes.Status404NotFound,
                    exception.Message),

                DuplicateEntityException => (
                    StatusCodes.Status409Conflict,
                    exception.Message),

                BusinessRuleViolationException => (
                    StatusCodes.Status409Conflict,
                    exception.Message),

                UnauthorizedAccessAppException => (
                    StatusCodes.Status401Unauthorized,
                    exception.Message),

                ForbiddenAccessException => (
                    StatusCodes.Status403Forbidden,
                    exception.Message),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.")
            };

            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                Details = null,
                TimeStamp = DateTime.UtcNow,
                Path = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(
                response,
                cancellationToken);

            return true;
        }
    }
}