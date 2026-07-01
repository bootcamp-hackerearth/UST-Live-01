using Microsoft.JSInterop;

namespace HealthAxisCore_Admin.Services;

public class TokenService
{
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
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AccessTokenKey, accessToken);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", RefreshTokenKey, refreshToken ?? string.Empty);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserIdKey, userId ?? string.Empty);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", RoleKey, role ?? string.Empty);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", FullNameKey, fullName ?? string.Empty);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", EmailKey, email ?? string.Empty);
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

    public async Task ClearTokensAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", AccessTokenKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", RefreshTokenKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", UserIdKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", RoleKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", FullNameKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", EmailKey);
    }
}