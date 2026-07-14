using HealthAxisAdminPortal.Services.FrontEndMemory;
using Microsoft.JSInterop;

namespace HealthAxisAdminPortal.Services.Helpers
{
    public class AuthService
    {
        private readonly IJSRuntime js;

        public AuthService(IJSRuntime js)
        {
            this.js = js;
        }

        public async Task EnsureTokenLoaded()
        {
            if (string.IsNullOrEmpty(TokenStore.AccessToken))
            {
                TokenStore.AccessToken =
                    await js.InvokeAsync<string>(
                        "authHelper.getToken");
            }
        }
    }
}
