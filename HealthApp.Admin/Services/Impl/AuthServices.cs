using HealthApp.Admin.Services.Interface;
using HealthApp.Shared.Dto;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthApp.Admin.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public bool IsLoggedIn { get; private set; }

        public string? Token { get; private set; }

        public string? Role { get; private set; }

        public AuthService(
            HttpClient httpClient,
            IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            Token = await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                "token");

            Role = await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                "role");

            IsLoggedIn =
                !string.IsNullOrWhiteSpace(Token) &&
                string.Equals(
                    Role,
                    "Admin",
                    StringComparison.OrdinalIgnoreCase);
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    "api/auth/login",
                    request);

                var text = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"RAW LOGIN RESPONSE: {text}");

                if (string.IsNullOrWhiteSpace(text))
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Empty response from server"
                    };
                }

                var result = JsonSerializer.Deserialize<LoginResponseDto>(
                    text,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (result == null)
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Invalid JSON response"
                    };
                }

                if (!result.Success)
                {
                    return result;
                }

                if (!string.Equals(
                        result.Role,
                        "Admin",
                        StringComparison.OrdinalIgnoreCase))
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Only Admin can access this portal"
                    };
                }

                Token = result.AccessToken;
                Role = result.Role;
                IsLoggedIn = true;

                await _jsRuntime.InvokeVoidAsync(
                    "localStorage.setItem",
                    "token",
                    result.AccessToken);

                await _jsRuntime.InvokeVoidAsync(
                    "localStorage.setItem",
                    "role",
                    result.Role);

                return result;
            }
            catch (Exception ex)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task LogoutAsync()
        {
            Token = null;
            Role = null;
            IsLoggedIn = false;

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                "token");

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                "role");
        }

        public async Task<string?> GetTokenAsync()
        {
            if (!string.IsNullOrWhiteSpace(Token))
                return Token;

            Token = await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                "token");

            return Token;
        }

    }
}