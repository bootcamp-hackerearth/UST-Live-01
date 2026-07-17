using HealthAxis_Admin.Providers;
using HealthAxis.Shared.DTO.AuthDtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace HealthAxis_Admin.Services
{
    public sealed class AuthService
    {
        private const string LoginEndpoint = "api/Auth/login";
        private const string DefaultAdminName = "Admin";

        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;
        private readonly ApiAuthenticationStateProvider
            _authenticationStateProvider;

        public AuthService(
            HttpClient httpClient,
            TokenService tokenService,
            ApiAuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
            _authenticationStateProvider =
                authenticationStateProvider;
        }

        public bool IsLoggedIn { get; private set; }

        public string AdminName { get; private set; } =
            DefaultAdminName;

        public async Task InitializeAsync()
        {
            var token =
                await _tokenService.GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(token) ||
                !ApiAuthenticationStateProvider.IsAdminToken(token))
            {
                IsLoggedIn = false;
                AdminName = DefaultAdminName;

                _httpClient.DefaultRequestHeaders.Authorization =
                    null;

                return;
            }

            IsLoggedIn = true;
            AdminName = GetAdminName(token);

            AddAuthorizationHeader(token);
        }

        public async Task<(bool Success, string Message)> LoginAsync(
            LoginDto loginDto)
        {
            try
            {
                using var response =
                    await _httpClient.PostAsJsonAsync(
                        LoginEndpoint,
                        loginDto);

                if (!response.IsSuccessStatusCode)
                {
                    var message =
                        await ReadErrorMessageAsync(response);

                    return (false, message);
                }

                var loginResponse =
                    await response.Content
                        .ReadFromJsonAsync<LoginResponse>();

                if (loginResponse is null)
                {
                    return (
                        false,
                        "API returned empty login response.");
                }

                if (string.IsNullOrWhiteSpace(
                        loginResponse.AccessToken))
                {
                    return (
                        false,
                        "Access token not found in API response.");
                }

                if (!ApiAuthenticationStateProvider.IsAdminToken(
                        loginResponse.AccessToken))
                {
                    return (
                        false,
                        "Only admin users can access this portal.");
                }

                await _tokenService.SaveTokensAsync(
                    loginResponse.AccessToken,
                    loginResponse.RefreshToken);

                AddAuthorizationHeader(
                    loginResponse.AccessToken);

                IsLoggedIn = true;
                AdminName =
                    GetAdminName(loginResponse.AccessToken);

                _authenticationStateProvider
                    .NotifyUserAuthenticated();

                return (
                    true,
                    loginResponse.Message);
            }
            catch (HttpRequestException)
            {
                return (
                    false,
                    "Could not connect to API. Check API running, URL, CORS and HTTPS certificate.");
            }
            catch (JsonException)
            {
                return (
                    false,
                    "Invalid response received from API.");
            }
        }

        public async Task<(bool Success, string Message)>
            CompleteExternalLoginAsync(
                string accessToken,
                string? refreshToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return (
                    false,
                    "Access token not found.");
            }

            if (!ApiAuthenticationStateProvider.IsAdminToken(
                    accessToken))
            {
                return (
                    false,
                    "Only admin users can access this portal.");
            }

            await _tokenService.SaveTokensAsync(
                accessToken,
                refreshToken ?? string.Empty);

            AddAuthorizationHeader(accessToken);

            IsLoggedIn = true;
            AdminName = GetAdminName(accessToken);

            _authenticationStateProvider
                .NotifyUserAuthenticated();

            return (
                true,
                "Admin login completed successfully.");
        }

        public async Task LogoutAsync()
        {
            await _tokenService.ClearTokensAsync();

            _httpClient.DefaultRequestHeaders.Authorization =
                null;

            IsLoggedIn = false;
            AdminName = DefaultAdminName;

            _authenticationStateProvider.NotifyUserLoggedOut();
        }

        private void AddAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
        }

        private static string GetAdminName(string token)
        {
            var claims =
                ApiAuthenticationStateProvider
                    .ParseClaimsFromJwt(token);

            var email = claims
                .FirstOrDefault(claim =>
                    claim.Type == ClaimTypes.Email)
                ?.Value;

            if (string.IsNullOrWhiteSpace(email))
            {
                return DefaultAdminName;
            }

            return email.Split('@')[0];
        }

        private static async Task<string> ReadErrorMessageAsync(
            HttpResponseMessage response)
        {
            const string invalidCredentialsMessage =
                "Invalid email or password.";

            var content =
                await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return invalidCredentialsMessage;
            }

            try
            {
                using var document =
                    JsonDocument.Parse(content);

                if (document.RootElement.TryGetProperty(
                        "message",
                        out var messageElement))
                {
                    return messageElement.GetString()
                        ?? invalidCredentialsMessage;
                }
            }
            catch (JsonException)
            {
                return content;
            }

            return invalidCredentialsMessage;
        }

        private sealed class LoginResponse
        {
            public string Message { get; set; } =
                string.Empty;

            public string AccessToken { get; set; } =
                string.Empty;

            public string RefreshToken { get; set; } =
                string.Empty;
        }
    }
}