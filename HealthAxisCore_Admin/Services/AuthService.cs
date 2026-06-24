using HealthAxisCore_Admin.Dtos.Auth;
using HealthAxisCore_Admin.Providers;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HealthAxisCore_Admin.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly TokenService _tokenService;
    private readonly ApiAuthenticationStateProvider _authenticationStateProvider;

    public AuthService(
        HttpClient httpClient,
        TokenService tokenService,
        ApiAuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<bool> LoginAsync(LoginRequestDto loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "api/auth/login",
            loginRequest);

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        if (loginResponse is null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(loginResponse.AccessToken))
        {
            return false;
        }

        if (!string.Equals(loginResponse.Role, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        await _tokenService.SaveTokensAsync(
            loginResponse.AccessToken,
            loginResponse.RefreshToken,
            loginResponse.UserId,
            loginResponse.Role,
            loginResponse.FullName,
            loginResponse.Email);

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginResponse.AccessToken);

        _authenticationStateProvider.NotifyUserAuthenticated();

        return true;
    }

    public async Task LogoutAsync()
    {
        await _tokenService.ClearTokensAsync();

        _httpClient.DefaultRequestHeaders.Authorization = null;

        _authenticationStateProvider.NotifyUserLoggedOut();
    }

    public async Task AddBearerTokenAsync()
    {
        var token = await _tokenService.GetAccessTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}