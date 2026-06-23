using Microsoft.JSInterop;

namespace HealthAxis.Admin.Services
{
    public class TokenService
    {
        private readonly IJSRuntime _jsRuntime;

        private const string AccessTokenKey = "healthaxis_access_token";
        private const string RefreshTokenKey = "healthaxis_refresh_token";
        private const string UserRoleKey = "healthaxis_user_role";
        private const string UserEmailKey = "healthaxis_user_email";
        private const string UserIdKey = "healthaxis_user_id";

        public TokenService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SaveTokensAsync(
            string accessToken,
            string refreshToken,
            string role,
            string email,
            string userId)
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                AccessTokenKey,
                accessToken);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                RefreshTokenKey,
                refreshToken);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                UserRoleKey,
                role);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                UserEmailKey,
                email);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                UserIdKey,
                userId);
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                AccessTokenKey);
        }

        public async Task<string?> GetUserRoleAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                UserRoleKey);
        }

        public async Task ClearTokensAsync()
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                AccessTokenKey);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                RefreshTokenKey);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                UserRoleKey);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                UserEmailKey);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                UserIdKey);
        }
    }
}
