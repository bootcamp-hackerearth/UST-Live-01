using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace S3_HealthAxis.Blazor.Services
{
    public class TokenAuthenticationHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;

        public TokenAuthenticationHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Native JS call to retrieve the token just before sending the request
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}