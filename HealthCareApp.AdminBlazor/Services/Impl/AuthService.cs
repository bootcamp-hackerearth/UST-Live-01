using HealthCareApp.AdminBlazor.Dtos.Auth;
using HealthCareApp.AdminBlazor.Services.Interfaces;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public class AuthService : IAuthService
    {
        private bool _isAuthenticated;

        private string? _currentUserEmail;

        private string? _currentUserRole;

        private string? _accessToken;

        private const string AdminEmail = "admin@healthcare.com";

        private const string AdminPassword = "Admin@123";

        public Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            if (loginDto.Email.Equals(AdminEmail, StringComparison.OrdinalIgnoreCase) &&
                loginDto.Password == AdminPassword)
            {
                _isAuthenticated = true;
                _currentUserEmail = AdminEmail;
                _currentUserRole = "Admin";
                _accessToken = "fake-admin-token";

                return Task.FromResult(new AuthResponseDto
                {
                    AccessToken = _accessToken,
                    Message = "Login successful.",
                    ExpiresIn = 15
                });
            }

            _isAuthenticated = false;
            _currentUserEmail = null;
            _currentUserRole = null;
            _accessToken = null;

            return Task.FromResult(new AuthResponseDto
            {
                AccessToken = string.Empty,
                Message = "Invalid email or password.",
                ExpiresIn = 0
            });
        }

        public Task LogoutAsync()
        {
            _isAuthenticated = false;
            _currentUserEmail = null;
            _currentUserRole = null;
            _accessToken = null;

            return Task.CompletedTask;
        }

        public Task<bool> IsAuthenticatedAsync()
        {
            return Task.FromResult(_isAuthenticated);
        }

        public Task<string?> GetCurrentUserEmailAsync()
        {
            return Task.FromResult(_currentUserEmail);
        }

        public Task<string?> GetCurrentUserRoleAsync()
        {
            return Task.FromResult(_currentUserRole);
        }
    }
}