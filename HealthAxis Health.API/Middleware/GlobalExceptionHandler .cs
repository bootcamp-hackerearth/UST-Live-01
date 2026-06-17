using System.Net;
using System.Text.Json;
using HealthAxisHealth.API.Exceptions;
using System.Diagnostics.CodeAnalysis;
using HealthAxisHealth.Shared.DTOs.CommonDtos;
using Microsoft.AspNetCore.Diagnostics;

namespace HealthAxisHealth.API.Handlers
{
    [ExcludeFromCodeCoverage]
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
            _logger.LogError(
                exception,
                "An unhandled exception occurred: {ErrorMessage}",
                exception.Message);

            HttpStatusCode statusCode = exception switch
            {
                BadRequestException => HttpStatusCode.BadRequest,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                ForbiddenException => HttpStatusCode.Forbidden,
                NotFoundException => HttpStatusCode.NotFound,
                ConflictException => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };

            var response = new ErrorResponseDto
            {
                StatusCode = (int)statusCode,
                Message = exception.InnerException?.Message
                          ?? exception.Message
            };

            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(
                JsonSerializer.Serialize(response),
                cancellationToken);

            return true;
        }
    }
}
