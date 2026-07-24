using System.Net.Http.Headers;
using HealthAxisAdminLayout.Services.Interfaces;

namespace HealthAxisAdminLayout.Handlers
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IAuthStateService _authState;

        public AuthHeaderHandler(IAuthStateService authState)
        {
            _authState = authState;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // ? LOAD TOKEN FROM LOCAL STORAGE (CRITICAL FIX)
            await _authState.LoadFromStorageAsync();

            var token = _authState.Token;

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}


