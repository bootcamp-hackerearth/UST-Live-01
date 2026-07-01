using HealthAxisCore_Admin.Dtos.Auth;
using HealthAxisCore_Admin.Providers;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HealthAxisCore_Admin.Services;

public class AdminHandoffService
{
    private readonly HttpClient _httpClient;

    private readonly TokenService _tokenService;

    private readonly ApiAuthenticationStateProvider _authenticationStateProvider;

    public AdminHandoffService(
        HttpClient httpClient,
        TokenService tokenService,
        ApiAuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<AdminHandoffExchangeResult> ExchangeAndStoreAsync(string code)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/admin-handoff/exchange",
                new ExchangeAdminHandoffRequestDto
                {
                    Code = code
                });

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();

                return AdminHandoffExchangeResult.Fail(
                    $"Admin handoff exchange failed. Status: {(int)response.StatusCode}. Response: {errorBody}");
            }

            var loginResponse =
                await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            if (loginResponse is null)
            {
                return AdminHandoffExchangeResult.Fail(
                    "Admin handoff exchange returned an empty response.");
            }

            if (string.IsNullOrWhiteSpace(loginResponse.AccessToken))
            {
                return AdminHandoffExchangeResult.Fail(
                    "Access token was empty after handoff exchange.");
            }

            if (!string.Equals(loginResponse.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return AdminHandoffExchangeResult.Fail(
                    $"Invalid role received from handoff exchange: {loginResponse.Role}");
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

            return AdminHandoffExchangeResult.Success();
        }
        catch (Exception exception)
        {
            return AdminHandoffExchangeResult.Fail(
                $"Admin handoff failed: {exception.Message}");
        }
    }
}

public class AdminHandoffExchangeResult
{
    public bool IsSuccess { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    public static AdminHandoffExchangeResult Success()
    {
        return new AdminHandoffExchangeResult
        {
            IsSuccess = true
        };
    }

    public static AdminHandoffExchangeResult Fail(string errorMessage)
    {
        return new AdminHandoffExchangeResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}