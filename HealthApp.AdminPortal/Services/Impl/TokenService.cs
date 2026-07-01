using HealthApp.AdminPortal.Services.Interface;
using Microsoft.JSInterop;

namespace HealthApp.AdminPortal.Services.Impl
{
    public class TokenService : ITokenService
    {
        private const string TokenKey = "healthapp_access_token";

        private readonly IJSRuntime _js;

        public TokenService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SetToken(string token)
        {
            await _js.InvokeVoidAsync(
                "localStorage.setItem",
                TokenKey,
                token);
        }

        public async Task<string?> GetToken()
        {
            return await _js.InvokeAsync<string?>(
                "localStorage.getItem",
                TokenKey);
        }

        public async Task RemoveToken()
        {
            await _js.InvokeVoidAsync(
                "localStorage.removeItem",
                TokenKey);
        }
    }
}
