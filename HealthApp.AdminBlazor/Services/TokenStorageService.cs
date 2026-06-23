using Microsoft.JSInterop;

namespace HealthApp.AdminBlazor.Services;

public class TokenStorageService(IJSRuntime jsRuntime)
{
    private const string AccessTokenKey = "healthapp_admin_access_token";
    private const string RefreshTokenKey = "healthapp_admin_refresh_token";

    public async Task SetTokensAsync(string accessToken, string refreshToken)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", AccessTokenKey, accessToken);
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", RefreshTokenKey, refreshToken);
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        return await jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            AccessTokenKey);
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        return await jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            RefreshTokenKey);
    }

    public async Task ClearTokensAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", AccessTokenKey);
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", RefreshTokenKey);
    }
}