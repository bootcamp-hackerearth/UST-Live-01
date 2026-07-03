using HealthCareApp.AdminBlazor.Auth;
using HealthCareApp.AdminBlazor.Services.Interfaces;
using HealthCareApp.Shared.Dtos.Auth;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public class AuthService : IAuthService
    {
        private const string TokenStorageKey = "token";
        private const string AdminRoleName = "Admin";
        private const string LoginEndpoint = "api/Auth/login";

        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly CustomAuthenticationStateProvider _authStateProvider;

        public AuthService(
            HttpClient httpClient,
            IJSRuntime jsRuntime,
            CustomAuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _authStateProvider = authStateProvider;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync(LoginEndpoint, loginDto);

            if (!response.IsSuccessStatusCode)
            {
                return CreateFailedResponse("Invalid email or password.");
            }

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (authResponse is null || string.IsNullOrWhiteSpace(authResponse.AccessToken))
            {
                return CreateFailedResponse("Invalid login response from server.");
            }

            var role = GetRoleFromToken(authResponse.AccessToken);

            if (!string.Equals(role, AdminRoleName, StringComparison.OrdinalIgnoreCase))
            {
                await LogoutAsync();

                return CreateFailedResponse("Only Admin users can access this portal.");
            }

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                TokenStorageKey,
                authResponse.AccessToken);

            _authStateProvider.NotifyUserLoggedIn(authResponse.AccessToken);

            return authResponse;
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                TokenStorageKey);

            _authStateProvider.NotifyUserLoggedOut();
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();

            return !string.IsNullOrWhiteSpace(token);
        }

        public async Task<string?> GetCurrentUserEmailAsync()
        {
            var token = await GetTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            return GetEmailFromToken(token);
        }

        public async Task<string?> GetCurrentUserRoleAsync()
        {
            var token = await GetTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            return GetRoleFromToken(token);
        }

        private async Task<string?> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                TokenStorageKey);
        }

        private static AuthResponseDto CreateFailedResponse(string message)
        {
            return new AuthResponseDto
            {
                AccessToken = string.Empty,
                Message = message,
                ExpiresIn = 0
            };
        }

        private static string GetEmailFromToken(string token)
        {
            var payload = GetJwtPayload(token);

            if (payload.ValueKind != JsonValueKind.Object)
            {
                return string.Empty;
            }

            if (payload.TryGetProperty("email", out var emailClaim))
            {
                return emailClaim.GetString() ?? string.Empty;
            }

            if (payload.TryGetProperty(ClaimTypes.Email, out var claimTypesEmail))
            {
                return claimTypesEmail.GetString() ?? string.Empty;
            }

            return string.Empty;
        }

        private static string GetRoleFromToken(string token)
        {
            var payload = GetJwtPayload(token);

            if (payload.ValueKind != JsonValueKind.Object)
            {
                return string.Empty;
            }

            if (payload.TryGetProperty(ClaimTypes.Role, out var claimTypesRole))
            {
                return GetJsonElementStringValue(claimTypesRole);
            }

            if (payload.TryGetProperty("role", out var simpleRoleClaim))
            {
                return GetJsonElementStringValue(simpleRoleClaim);
            }

            if (payload.TryGetProperty("roles", out var rolesClaim))
            {
                return GetJsonElementStringValue(rolesClaim);
            }

            return string.Empty;
        }

        private static string GetJsonElementStringValue(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Array)
            {
                var firstRole = element.EnumerateArray().FirstOrDefault();

                return firstRole.GetString() ?? string.Empty;
            }

            return element.GetString() ?? string.Empty;
        }

        private static JsonElement GetJwtPayload(string token)
        {
            var tokenParts = token.Split('.');

            if (tokenParts.Length < 2)
            {
                return default;
            }

            try
            {
                var payload = tokenParts[1]
                    .Replace('-', '+')
                    .Replace('_', '/');

                payload = AddBase64Padding(payload);

                var jsonBytes = Convert.FromBase64String(payload);

                var json = Encoding.UTF8.GetString(jsonBytes);

                return JsonSerializer.Deserialize<JsonElement>(json);
            }
            catch (FormatException)
            {
                return default;
            }
            catch (JsonException)
            {
                return default;
            }
        }

        private static string AddBase64Padding(string base64)
        {
            int remainder = base64.Length % 4;

            if (remainder == 2)
            {
                return base64 + "==";
            }

            if (remainder == 3)
            {
                return base64 + "=";
            }

            return base64;
        }
    }
}