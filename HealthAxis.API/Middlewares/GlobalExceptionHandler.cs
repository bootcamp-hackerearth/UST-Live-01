using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace HealthAxis.API.Middlewares
{
    public sealed class GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var statusCode = GetStatusCode(exception);
            var message = GetMessage(exception);

            logger.LogError(
                exception,
                "Exception occurred. StatusCode: {StatusCode}, Path: {Path}",
                statusCode,
                httpContext.Request.Path);

            var response = new ApiErrorResponse
            {
                StatusCode = statusCode,
                Message = message
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType =
                "application/json";

            await httpContext.Response.WriteAsJsonAsync(
                response,
                cancellationToken);

            return true;
        }

        private static int GetStatusCode(
            Exception exception)
        {
            return exception switch
            {
                NotFoundException =>
                    StatusCodes.Status404NotFound,

                ValidationException =>
                    StatusCodes.Status400BadRequest,

                AppointmentConflictException =>
                    StatusCodes.Status409Conflict,

                BusinessRuleException =>
                    StatusCodes.Status400BadRequest,

                UnauthorizedAccessException =>
                    StatusCodes.Status401Unauthorized,

                _ =>
                    StatusCodes.Status500InternalServerError
            };
        }

        private static string GetMessage(
            Exception exception)
        {
            return exception switch
            {
                NotFoundException =>
                    exception.Message,

                ValidationException =>
                    exception.Message,

                AppointmentConflictException =>
                    exception.Message,

                BusinessRuleException =>
                    exception.Message,

                UnauthorizedAccessException =>
                    "Unauthorized access",

                _ =>
                    "An unexpected error occurred. Please try again later."
            };
        }
    }
}