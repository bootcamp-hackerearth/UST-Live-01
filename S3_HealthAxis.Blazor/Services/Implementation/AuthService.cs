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

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> LoginAsync(LoginDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (authResponse == null || string.IsNullOrEmpty(authResponse.AccessToken))
            {
                return false;
            }

            // Save token using native JS Interop
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", authResponse.AccessToken);

            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(authResponse.AccessToken);

            return true;
        }

        public async Task LogoutAsync()
        {
            // Remove token using native JS Interop
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }
    }
}