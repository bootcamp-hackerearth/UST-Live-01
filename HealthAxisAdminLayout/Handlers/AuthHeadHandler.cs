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
            if (!string.IsNullOrWhiteSpace(_authState.Token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _authState.Token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}