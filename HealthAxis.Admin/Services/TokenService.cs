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

        private const string LocalStorageSetItem = "localStorage.setItem";
        private const string LocalStorageGetItem = "localStorage.getItem";
        private const string LocalStorageRemoveItem = "localStorage.removeItem";

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
                LocalStorageSetItem,
                AccessTokenKey,
                accessToken);

            await _jsRuntime.InvokeVoidAsync(
                LocalStorageSetItem,
                RefreshTokenKey,
                refreshToken);

            await _jsRuntime.InvokeVoidAsync(
                LocalStorageSetItem,
                UserRoleKey,
                role);

            await _jsRuntime.InvokeVoidAsync(
                LocalStorageSetItem,
                UserEmailKey,
                email);

            await _jsRuntime.InvokeVoidAsync(
                LocalStorageSetItem,
                UserIdKey,
                userId);
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                LocalStorageGetItem,
                AccessTokenKey);
        }

        public async Task<string?> GetUserRoleAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                LocalStorageGetItem,
                UserRoleKey);
        }

        public async Task ClearTokensAsync()
        {
            await _jsRuntime.InvokeVoidAsync(
                LocalStorageRemoveItem,
                AccessTokenKey);

            await _jsRuntime.InvokeVoidAsync(
                LocalStorageRemoveItem,
                RefreshTokenKey);

            await _jsRuntime.InvokeVoidAsync(
                LocalStorageRemoveItem,
                UserRoleKey);

            await _jsRuntime.InvokeVoidAsync(
                LocalStorageRemoveItem,
                UserEmailKey);

            await _jsRuntime.InvokeVoidAsync(
                LocalStorageRemoveItem,
                UserIdKey);
        }
    }
}
