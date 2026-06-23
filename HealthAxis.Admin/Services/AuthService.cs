using HealthAxis.API.DTOs.Auth;
using HealthAxis.API.DTOs.CommonDtos;
using HealthAxis.Shared.DTOs.Auth;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthAxis.Admin.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AuthService(
            HttpClient httpClient,
            TokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<(bool Success, string Message)> LoginAsync(
            LoginDto loginDto)
        {
            HttpResponseMessage response =
                await _httpClient.PostAsJsonAsync(
                    "api/auth/login",
                    loginDto);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage =
                    await GetErrorMessageAsync(response);

                return (false, errorMessage);
            }

            AuthResponseDto? authResponse =
                await response.Content.ReadFromJsonAsync<AuthResponseDto>(
                    _jsonOptions);

            if (authResponse == null ||
                string.IsNullOrWhiteSpace(authResponse.AccessToken))
            {
                return (false, "Invalid login response received from server.");
            }

            if (!string.Equals(
                    authResponse.Role,
                    "Admin",
                    StringComparison.OrdinalIgnoreCase))
            {
                return (false, "Only Admin users can access this portal.");
            }

            await _tokenService.SaveTokensAsync(
                authResponse.AccessToken,
                authResponse.RefreshToken,
                authResponse.Role,
                authResponse.Email,
                authResponse.UserId);

            return (true, authResponse.Message);
        }

        public async Task LogoutAsync()
        {
            await _tokenService.ClearTokensAsync();
        }

        private async Task<string> GetErrorMessageAsync(
            HttpResponseMessage response)
        {
            try
            {
                ErrorResponseDto? error =
                    await response.Content.ReadFromJsonAsync<ErrorResponseDto>(
                        _jsonOptions);

                if (!string.IsNullOrWhiteSpace(error?.Message))
                {
                    return error.Message;
                }
            }
            catch
            {
                // Ignore parsing error and return generic message below.
            }

            return $"Request failed with status code {(int)response.StatusCode}.";
        }
    }
}
