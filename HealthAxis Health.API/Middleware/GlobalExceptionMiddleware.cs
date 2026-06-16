using System.Net;
using System.Text.Json;
using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.API.Exceptions;

namespace HealthAxisHealth.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;

        private readonly ILogger<
            GlobalExceptionMiddleware>
            _logger;

        #endregion

        #region Constructor

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;

            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    exception.Message);

                await HandleExceptionAsync(
                    context,
                    exception);
            }
        }

        private static async Task
            HandleExceptionAsync(
                HttpContext context,
                Exception exception)
        {
            HttpStatusCode statusCode =
                HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case BadRequestException:
                    statusCode =
                        HttpStatusCode.BadRequest;
                    break;

                case UnauthorizedException:
                    statusCode =
                        HttpStatusCode.Unauthorized;
                    break;

                case ForbiddenException:
                    statusCode =
                        HttpStatusCode.Forbidden;
                    break;

                case NotFoundException:
                    statusCode =
                        HttpStatusCode.NotFound;
                    break;

                case ConflictException:
                    statusCode =
                        HttpStatusCode.Conflict;
                    break;
            }

            ErrorResponseDto response =
                new ErrorResponseDto
                {
                    StatusCode =
                        (int)statusCode,

                    Message =
                        exception.InnerException?.Message
                            ?? exception.Message
                };

            context.Response.ContentType =
                "application/json";

            context.Response.StatusCode =
                (int)statusCode;

            string jsonResponse =
                JsonSerializer.Serialize(
                    response);

            await context.Response.WriteAsync(
                jsonResponse);
        }

        #endregion
    }
}
