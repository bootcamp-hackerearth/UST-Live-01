using Microsoft.JSInterop;

namespace HealthAxisCore_Admin.Services
{
    public class TokenService
    {
        private readonly IJSRuntime _jsRuntime;

        private const string AccessTokenKey = "admin_access_token";

        private const string RoleKey = "admin_role";

        public TokenService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetTokenAsync(string token, string role)
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                AccessTokenKey,
                token);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                RoleKey,
                role);
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                AccessTokenKey);
        }

        public async Task<string?> GetRoleAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                RoleKey);
        }

        public async Task ClearAsync()
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                AccessTokenKey);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                RoleKey);
        }
    }
}