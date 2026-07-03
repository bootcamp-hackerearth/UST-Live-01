using Microsoft.JSInterop;

namespace HealthAxis_Admin.Services
{
    public sealed class TokenService
    {
        private const string AccessTokenKey = "healthaxis_admin_access_token";
        private const string RefreshTokenKey = "healthaxis_admin_refresh_token";
        private const string RoleKey = "healthaxis_admin_role";
        private const string EmailKey = "healthaxis_admin_email";
        private const string ExpiresAtKey = "healthaxis_admin_expires_at";
        private const string ExpiresInKey = "healthaxis_admin_expires_in_minutes";

        private readonly IJSRuntime _jsRuntime;

        public TokenService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SaveTokensAsync(string accessToken, string refreshToken)
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                AccessTokenKey,
                accessToken);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                RefreshTokenKey,
                refreshToken);
        }

        public async Task SaveAdminSessionAsync(
            string accessToken,
            string refreshToken,
            string role,
            string email,
            string expiresAt,
            string expiresInMinutes)
        {
            await SaveTokensAsync(accessToken, refreshToken);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                RoleKey,
                role);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                EmailKey,
                email);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                ExpiresAtKey,
                expiresAt);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                ExpiresInKey,
                expiresInMinutes);
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                AccessTokenKey);
        }

        public async Task<string?> GetRefreshTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                RefreshTokenKey);
        }

        public async Task<string?> GetRoleAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                RoleKey);
        }

        public async Task<string?> GetEmailAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                EmailKey);
        }

        public async Task<string?> GetExpiresAtAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                ExpiresAtKey);
        }

        public async Task<string?> GetExpiresInMinutesAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                ExpiresInKey);
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
                RoleKey);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                EmailKey);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                ExpiresAtKey);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                ExpiresInKey);
        }
    }
}