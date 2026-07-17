using Microsoft.JSInterop;

namespace HealthAxis_Admin.Services
{
    public sealed class TokenService
    {
        private const string AccessTokenKey =
            "healthaxis_admin_access_token";

        private const string RefreshTokenKey =
            "healthaxis_admin_refresh_token";

        private const string RoleKey =
            "healthaxis_admin_role";

        private const string EmailKey =
            "healthaxis_admin_email";

        private const string ExpiresAtKey =
            "healthaxis_admin_expires_at";

        private const string ExpiresInKey =
            "healthaxis_admin_expires_in_minutes";

        private const string SetStorageItemMethod =
            "localStorage.setItem";

        private const string GetStorageItemMethod =
            "localStorage.getItem";

        private const string RemoveStorageItemMethod =
            "localStorage.removeItem";

        private readonly IJSRuntime _jsRuntime;

        public TokenService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SaveTokensAsync(
            string accessToken,
            string refreshToken)
        {
            await SetStorageItemAsync(
                AccessTokenKey,
                accessToken);

            await SetStorageItemAsync(
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
            await SaveTokensAsync(
                accessToken,
                refreshToken);

            await SetStorageItemAsync(
                RoleKey,
                role);

            await SetStorageItemAsync(
                EmailKey,
                email);

            await SetStorageItemAsync(
                ExpiresAtKey,
                expiresAt);

            await SetStorageItemAsync(
                ExpiresInKey,
                expiresInMinutes);
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            return await GetStorageItemAsync(
                AccessTokenKey);
        }

        public async Task<string?> GetRefreshTokenAsync()
        {
            return await GetStorageItemAsync(
                RefreshTokenKey);
        }

        public async Task<string?> GetRoleAsync()
        {
            return await GetStorageItemAsync(
                RoleKey);
        }

        public async Task<string?> GetEmailAsync()
        {
            return await GetStorageItemAsync(
                EmailKey);
        }

        public async Task<string?> GetExpiresAtAsync()
        {
            return await GetStorageItemAsync(
                ExpiresAtKey);
        }

        public async Task<string?> GetExpiresInMinutesAsync()
        {
            return await GetStorageItemAsync(
                ExpiresInKey);
        }

        public async Task ClearTokensAsync()
        {
            await RemoveStorageItemAsync(
                AccessTokenKey);

            await RemoveStorageItemAsync(
                RefreshTokenKey);

            await RemoveStorageItemAsync(
                RoleKey);

            await RemoveStorageItemAsync(
                EmailKey);

            await RemoveStorageItemAsync(
                ExpiresAtKey);

            await RemoveStorageItemAsync(
                ExpiresInKey);
        }

        private ValueTask SetStorageItemAsync(
            string key,
            string value)
        {
            return _jsRuntime.InvokeVoidAsync(
                SetStorageItemMethod,
                key,
                value);
        }

        private ValueTask<string?> GetStorageItemAsync(
            string key)
        {
            return _jsRuntime.InvokeAsync<string?>(
                GetStorageItemMethod,
                key);
        }

        private ValueTask RemoveStorageItemAsync(
            string key)
        {
            return _jsRuntime.InvokeVoidAsync(
                RemoveStorageItemMethod,
                key);
        }
    }
}