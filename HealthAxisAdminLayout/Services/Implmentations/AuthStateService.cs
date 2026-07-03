using HealthAxis.Shared.DTOs.Auth;
using HealthAxis.Shared.DTOs.User;
using HealthAxisAdminLayout.Services.Interfaces;
using Microsoft.JSInterop;

namespace HealthAxisAdminLayout.Services.Implementations
{
    public class AuthStateService : IAuthStateService
    {
        private readonly IJSRuntime _js;
        private bool _isLoaded = false;

        public AuthStateService(IJSRuntime js)
        {
            _js = js;
        }

        public string? Token { get; private set; }
        public string? Email { get; private set; }
        public string? Role { get; private set; }
        public int? ReferenceId { get; private set; }
        public bool IsFirstLogin { get; private set; }

        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);

        public bool IsAdmin =>
            Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;

        public async Task SetLoginAsync(AuthResponseDto response)
        {
            Token = response.Token;
            Email = response.Email;
            Role = response.Role;
            ReferenceId = response.ReferenceId;
            IsFirstLogin = response.IsFirstLogin;

            try
            {
                await _js.InvokeVoidAsync("authLocalStorage.setItem", "token", Token ?? "");
                await _js.InvokeVoidAsync("authLocalStorage.setItem", "email", Email ?? "");
                await _js.InvokeVoidAsync("authLocalStorage.setItem", "role", Role ?? "");
                await _js.InvokeVoidAsync("authLocalStorage.setItem", "referenceId", ReferenceId?.ToString() ?? "");
                await _js.InvokeVoidAsync("authLocalStorage.setItem", "isFirstLogin", IsFirstLogin.ToString());
            }
            catch
            {
            }
        }

        public async Task LoadFromStorageAsync()
        {
            if (_isLoaded) return;

            try
            {
                Token = await _js.InvokeAsync<string>("authLocalStorage.getItem", "token");
                Email = await _js.InvokeAsync<string>("authLocalStorage.getItem", "email");
                Role = await _js.InvokeAsync<string>("authLocalStorage.getItem", "role");

                var refId = await _js.InvokeAsync<string>("authLocalStorage.getItem", "referenceId");
                if (int.TryParse(refId, out var parsedId))
                {
                    ReferenceId = parsedId;
                }

                var firstLogin = await _js.InvokeAsync<string>("authLocalStorage.getItem", "isFirstLogin");
                if (bool.TryParse(firstLogin, out var parsedBool))
                {
                    IsFirstLogin = parsedBool;
                }
            }
            catch
            {
            }

            _isLoaded = true;
        }

        public async Task LogoutAsync()
        {
            Token = null;
            Email = null;
            Role = null;
            ReferenceId = null;
            IsFirstLogin = false;

            try
            {
                await _js.InvokeVoidAsync("authLocalStorage.removeItem", "token");
                await _js.InvokeVoidAsync("authLocalStorage.removeItem", "email");
                await _js.InvokeVoidAsync("authLocalStorage.removeItem", "role");
                await _js.InvokeVoidAsync("authLocalStorage.removeItem", "referenceId");
                await _js.InvokeVoidAsync("authLocalStorage.removeItem", "isFirstLogin");
            }
            catch
            {
            }
        }
    }
}
