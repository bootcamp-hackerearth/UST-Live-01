using S3_HealthAxis.Shared.DTOs.Auth;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace S3_HealthAxis.Blazor.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(
            HttpClient httpClient,
            IJSRuntime jsRuntime,
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> LoginAsync(LoginDto request)
        {
            var response =
                await _httpClient.PostAsJsonAsync(
                    "api/auth/login",
                    request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var authResponse =
                await response.Content
                    .ReadFromJsonAsync<AuthResponseDto>();

            if (authResponse == null ||
                string.IsNullOrWhiteSpace(authResponse.AccessToken))
            {
                return false;
            }

            await SaveAuthDataAsync(authResponse);

            ((CustomAuthStateProvider)_authStateProvider)
                .NotifyUserAuthentication(authResponse.AccessToken);

            return true;
        }

        public async Task LogoutAsync()
        {
            await ClearAuthDataAsync();

            ((CustomAuthStateProvider)_authStateProvider)
                .NotifyUserLogout();
        }

        private async Task SaveAuthDataAsync(AuthResponseDto authResponse)
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                "authToken",
                authResponse.AccessToken);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                "refreshToken",
                authResponse.RefreshToken);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                "authEmail",
                authResponse.Email);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                "authRole",
                authResponse.Role);

            if (authResponse.ReferenceId.HasValue)
            {
                await _jsRuntime.InvokeVoidAsync(
                    "localStorage.setItem",
                    "authReferenceId",
                    authResponse.ReferenceId.Value.ToString());
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync(
                    "localStorage.removeItem",
                    "authReferenceId");
            }
        }

        private async Task ClearAuthDataAsync()
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                "authToken");

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                "refreshToken");

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                "authEmail");

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                "authRole");

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                "authReferenceId");
        }
    }
}

