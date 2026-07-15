using Microsoft.JSInterop;

namespace HealthAxisCore_Admin.Services;

public class TokenService
{
    private const string LocalStorageSetItem = "localStorage.setItem";
    private const string LocalStorageGetItem = "localStorage.getItem";
    private const string LocalStorageRemoveItem = "localStorage.removeItem";

    private const string AccessTokenKey = "healthaxis_admin_access_token";
    private const string RefreshTokenKey = "healthaxis_admin_refresh_token";
    private const string UserIdKey = "healthaxis_admin_user_id";
    private const string RoleKey = "healthaxis_admin_role";
    private const string FullNameKey = "healthaxis_admin_full_name";
    private const string EmailKey = "healthaxis_admin_email";

    private readonly IJSRuntime _jsRuntime;

    public TokenService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SaveTokensAsync(
        string accessToken,
        string refreshToken,
        string userId,
        string role,
        string fullName,
        string email)
    {
        await _jsRuntime.InvokeVoidAsync(
            LocalStorageSetItem,
            AccessTokenKey,
            accessToken);

        await _jsRuntime.InvokeVoidAsync(
            LocalStorageSetItem,
            RefreshTokenKey,
            refreshToken ?? string.Empty);

        await _jsRuntime.InvokeVoidAsync(
            LocalStorageSetItem,
            UserIdKey,
            userId ?? string.Empty);

        await _jsRuntime.InvokeVoidAsync(
            LocalStorageSetItem,
            RoleKey,
            role ?? string.Empty);

        await _jsRuntime.InvokeVoidAsync(
            LocalStorageSetItem,
            FullNameKey,
            fullName ?? string.Empty);

        await _jsRuntime.InvokeVoidAsync(
            LocalStorageSetItem,
            EmailKey,
            email ?? string.Empty);
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>(
            LocalStorageGetItem,
            AccessTokenKey);
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>(
            LocalStorageGetItem,
            RefreshTokenKey);
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
            UserIdKey);

        await _jsRuntime.InvokeVoidAsync(
            LocalStorageRemoveItem,
            RoleKey);

        await _jsRuntime.InvokeVoidAsync(
            LocalStorageRemoveItem,
            FullNameKey);

        await _jsRuntime.InvokeVoidAsync(
            LocalStorageRemoveItem,
            EmailKey);
    }
}