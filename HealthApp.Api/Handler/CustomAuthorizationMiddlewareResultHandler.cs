using HealthApp.Api.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace HealthApp.Api.Handler
{
    public class CustomAuthorizationMiddlewareResultHandler
        : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {

            if (authorizeResult.Challenged)
            {
                var hasAuthorizationHeader =
                    context.Request.Headers.ContainsKey("Authorization");

                if (!hasAuthorizationHeader)
                {
                    throw new UnauthorizedAccessAppException(
                        "Please login to continue.");
                }

                throw new UnauthorizedAccessAppException(
                    "Your session has expired. Please login again.");
            }

            if (authorizeResult.Forbidden)
            {
                throw new ForbiddenAccessException(
                    "You do not have access to this page.");
            }


            await _defaultHandler.HandleAsync(
                next,
                context,
                policy,
                authorizeResult);
        }
    }
}