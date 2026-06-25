using HealthApp.AdminPortal.Services.Interface;
using Microsoft.JSInterop;

namespace HealthApp.AdminPortal.Services.Impl
{

    public class TokenService : ITokenService
    {
        private readonly IJSRuntime _js;

        public TokenService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SetToken(string token)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        }

        public async Task<string?> GetToken()
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
        }

        public async Task RemoveToken()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
        }
    }

}
