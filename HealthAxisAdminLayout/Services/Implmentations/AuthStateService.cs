using HealthAxis.Shared.DTOs.Auth;
using HealthAxis.Shared.DTOs.User;
using HealthAxisAdminLayout.Services.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace HealthAxisAdminLayout.Services.Implementations
{
    public sealed class AuthStateService : IAuthStateService
    {
        private const string SetItemFunction =
            "authLocalStorage.setItem";

        private const string GetItemFunction =
            "authLocalStorage.getItem";

        private const string RemoveItemFunction =
            "authLocalStorage.removeItem";

        private const string TokenKey =
            "token";

        private const string EmailKey =
            "email";

        private const string RoleKey =
            "role";

        private const string ReferenceIdKey =
            "referenceId";

        private const string IsFirstLoginKey =
            "isFirstLogin";

        private readonly IJSRuntime _js;

        private readonly ILogger<AuthStateService> _logger;

        private bool _isLoaded;

        public AuthStateService(
            IJSRuntime js,
            ILogger<AuthStateService> logger)
        {
            _js = js;
            _logger = logger;
        }

        public string? Token { get; private set; }

        public string? Email { get; private set; }

        public string? Role { get; private set; }

        public int? ReferenceId { get; private set; }

        public bool IsFirstLogin { get; private set; }

        public bool IsLoggedIn =>
            !string.IsNullOrWhiteSpace(Token);

        public bool IsAdmin =>
            Role?.Equals(
                "Admin",
                StringComparison.OrdinalIgnoreCase) == true;

        public async Task SetLoginAsync(
            AuthResponseDto response)
        {
            ArgumentNullException.ThrowIfNull(response);

            SetAuthenticationState(response);

            try
            {
                await SaveAuthenticationStateAsync();
                _isLoaded = true;
            }
            catch (JSException exception)
            {
                LogStorageWriteFailure(exception);
            }
            catch (InvalidOperationException exception)
            {
                LogInteropUnavailable(exception);
            }
        }

        public async Task LoadFromStorageAsync()
        {
            if (_isLoaded)
            {
                return;
            }

            try
            {
                await LoadAuthenticationStateAsync();

                _isLoaded = true;
            }
            catch (JSException exception)
            {
                LogStorageReadFailure(exception);
            }
            catch (InvalidOperationException exception)
            {
                LogInteropUnavailable(exception);
            }
        }

        public async Task LogoutAsync()
        {
            ClearAuthenticationState();

            try
            {
                await RemoveAuthenticationStateAsync();

                _isLoaded = true;
            }
            catch (JSException exception)
            {
                LogStorageRemovalFailure(exception);
            }
            catch (InvalidOperationException exception)
            {
                LogInteropUnavailable(exception);
            }
        }

        private void SetAuthenticationState(
            AuthResponseDto response)
        {
            Token = response.Token;
            Email = response.Email;
            Role = response.Role;
            ReferenceId = response.ReferenceId;
            IsFirstLogin = response.IsFirstLogin;
        }

        private async Task SaveAuthenticationStateAsync()
        {
            await SetStorageItemAsync(
                TokenKey,
                Token);

            await SetStorageItemAsync(
                EmailKey,
                Email);

            await SetStorageItemAsync(
                RoleKey,
                Role);

            await SetStorageItemAsync(
                ReferenceIdKey,
                ReferenceId?.ToString());

            await SetStorageItemAsync(
                IsFirstLoginKey,
                IsFirstLogin.ToString());
        }

        private async Task LoadAuthenticationStateAsync()
        {
            Token = NormalizeStorageValue(
                await GetStorageItemAsync(TokenKey));

            Email = NormalizeStorageValue(
                await GetStorageItemAsync(EmailKey));

            Role = NormalizeStorageValue(
                await GetStorageItemAsync(RoleKey));

            var referenceIdValue =
                await GetStorageItemAsync(
                    ReferenceIdKey);

            ReferenceId =
                ParseReferenceId(referenceIdValue);

            var firstLoginValue =
                await GetStorageItemAsync(
                    IsFirstLoginKey);

            IsFirstLogin =
                ParseFirstLogin(firstLoginValue);
        }

        private async Task RemoveAuthenticationStateAsync()
        {
            await RemoveStorageItemAsync(TokenKey);
            await RemoveStorageItemAsync(EmailKey);
            await RemoveStorageItemAsync(RoleKey);
            await RemoveStorageItemAsync(ReferenceIdKey);
            await RemoveStorageItemAsync(IsFirstLoginKey);
        }

        private async Task SetStorageItemAsync(
            string key,
            string? value)
        {
            await _js.InvokeVoidAsync(
                SetItemFunction,
                key,
                value ?? string.Empty);
        }

        private async Task<string?>
            GetStorageItemAsync(string key)
        {
            return await _js.InvokeAsync<string?>(
                GetItemFunction,
                key);
        }

        private async Task RemoveStorageItemAsync(
            string key)
        {
            await _js.InvokeVoidAsync(
                RemoveItemFunction,
                key);
        }

        private void ClearAuthenticationState()
        {
            Token = null;
            Email = null;
            Role = null;
            ReferenceId = null;
            IsFirstLogin = false;
        }

        private static string? NormalizeStorageValue(
            string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value;
        }

        private static int? ParseReferenceId(
            string? referenceIdValue)
        {
            return int.TryParse(
                referenceIdValue,
                out var parsedReferenceId)
                    ? parsedReferenceId
                    : null;
        }

        private static bool ParseFirstLogin(
            string? firstLoginValue)
        {
            return bool.TryParse(
                firstLoginValue,
                out var parsedFirstLogin) &&
                parsedFirstLogin;
        }

        private void LogStorageWriteFailure(
            JSException exception)
        {
            if (!_logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            _logger.LogWarning(
                exception,
                "Unable to save authentication state " +
                "to browser local storage.");
        }

        private void LogStorageReadFailure(
            JSException exception)
        {
            if (!_logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            _logger.LogWarning(
                exception,
                "Unable to load authentication state " +
                "from browser local storage.");
        }

        private void LogStorageRemovalFailure(
            JSException exception)
        {
            if (!_logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            _logger.LogWarning(
                exception,
                "Unable to remove authentication state " +
                "from browser local storage.");
        }

        private void LogInteropUnavailable(
            InvalidOperationException exception)
        {
            if (!_logger.IsEnabled(LogLevel.Debug))
            {
                return;
            }

            _logger.LogDebug(
                exception,
                "Browser local storage is currently unavailable. " +
                "This can occur during Blazor prerendering.");
        }
    }
}

