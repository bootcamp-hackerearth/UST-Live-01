using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace HealthCareAdmin.UI.Services.Auth
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly IJSRuntime _js;

        public AuthHandler(IJSRuntime js)
        {
            _js = js;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}