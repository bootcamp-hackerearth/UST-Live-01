using System.Net.Http.Json;
using HealthApp.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace HealthApp.AdminBlazor.Services;

public class AuthApiService(
    HttpClient httpClient,
    TokenStorageService tokenStorageService,
    AuthenticationStateProvider authenticationStateProvider)
{
    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var response = await httpClient.PostAsJsonAsync("api/auth/login", dto);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

        if (authResponse is null)
        {
            return null;
        }

        if (!string.Equals(authResponse.Role, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        await tokenStorageService.SetTokensAsync(
            authResponse.AccessToken,
            authResponse.RefreshToken);

        if (authenticationStateProvider is AdminAuthenticationStateProvider provider)
        {
            provider.NotifyUserAuthentication(authResponse.AccessToken);
        }

        return authResponse;
    }

    public async Task LogoutAsync()
    {
        await tokenStorageService.ClearTokensAsync();

        if (authenticationStateProvider is AdminAuthenticationStateProvider provider)
        {
            provider.NotifyUserLogout();
        }
    }
}