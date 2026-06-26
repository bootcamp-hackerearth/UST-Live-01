using HealthAxis.Shared.DTOs.Auth;
using HealthAxis.Shared.DTOs.User;
using HealthAxisAdminLayout.Services.Interfaces;
using System.Net.Http.Json;

namespace HealthAxisAdminLayout.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthStateService _authState;
        private readonly HttpClient _http;

        public AuthService(IAuthStateService authState, HttpClient http)
        {
            _authState = authState;
            _http = http;
        }

        public async Task<AuthResponseDTO?> LoginAsync(LoginDTO loginDto)
        {
            var response = await _http.PostAsJsonAsync(
                "api/auth/login",
                loginDto
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

            if (result != null)
            {
                await _authState.SetLoginAsync(result);
            }

            return result;
        }

        public async Task LogoutAsync()
        {
            await _authState.LogoutAsync();
        }

        public Task<string?> GetTokenAsync()
        {
            return Task.FromResult(_authState.Token);
        }

        public Task<string?> GetEmailAsync()
        {
            return Task.FromResult(_authState.Email);
        }

        public Task<string?> GetRoleAsync()
        {
            return Task.FromResult(_authState.Role);
        }

        public Task<bool> IsLoggedInAsync()
        {
            return Task.FromResult(_authState.IsLoggedIn);
        }

        public Task<bool> IsAdminAsync()
        {
            return Task.FromResult(_authState.IsAdmin);
        }
    }
}
