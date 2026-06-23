
using HealthApp.Shared.DTOs;
using Microsoft.AspNetCore.Diagnostics;

namespace HealthApp.API.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "Unhandled exception at {Path}: {Message}",
            httpContext.Request.Path,
            exception.Message);

        var (statusCode, message) = exception switch
        {
            EntityNotFoundException ex =>
                (StatusCodes.Status404NotFound, ex.Message),

            ConflictException ex =>
                (StatusCodes.Status409Conflict, ex.Message),

            ForbiddenAccessException ex =>
                    (StatusCodes.Status403Forbidden, ex.Message),

            PasswordChangeRequiredException ex =>
                (StatusCodes.Status403Forbidden, ex.Message),

            BusinessRuleException ex =>
                (StatusCodes.Status400BadRequest, ex.Message),

            HealthcareAppException ex =>
                (StatusCodes.Status400BadRequest, ex.Message),

            _ =>
                (StatusCodes.Status500InternalServerError,
                    "Something went wrong. Please try again later.")
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(
            new ErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                TimeStamp = DateTime.UtcNow,
                Path = httpContext.Request.Path
            },
            cancellationToken);

        return true;
    }
}
