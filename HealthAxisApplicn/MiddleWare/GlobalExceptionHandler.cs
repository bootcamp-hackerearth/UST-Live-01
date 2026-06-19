using HealthAxisApplicn.Exceptions;
using HealthAxisApplicn.Dto;
using Microsoft.AspNetCore.Diagnostics;
using HealthAxisApplicn.Dto.Common;

namespace HealthAxisApplicn.MiddleWare
{
    public class GlobalExceptionHandler: IExceptionHandler
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
                "An unexpected error occurred. Path: {Path}, Method: {Method}, Message: {Message}",
                httpContext.Request.Path,
                httpContext.Request.Method,
                exception.Message);

            var (statusCode, message) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, exception.Message),

                BadRequestException => (StatusCodes.Status400BadRequest, exception.Message),

                ForbiddenException => (StatusCodes.Status403Forbidden, exception.Message),

                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, exception.Message),

                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };

            var response = new ErrorResponseDto
            {
                StatusCode = statusCode,
                Message = message,
                TimeStamp = DateTime.UtcNow,
                Path = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(
                response,
                cancellationToken);

            return true;
        }
    }
}
