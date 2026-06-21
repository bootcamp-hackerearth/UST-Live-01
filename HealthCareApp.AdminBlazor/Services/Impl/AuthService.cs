using HealthCareApp.AdminBlazor.Dtos.Auth;
using HealthCareApp.AdminBlazor.Services.Interfaces;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public class AuthService : IAuthService
    {
        private bool _isAuthenticated;

        private string? _currentUserEmail;

        private string? _currentUserRole;

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

                return Task.FromResult(new AuthResponseDto
                {
                    IsSuccess = true,
                    Message = "Login successful.",
                    AccessToken = "fake-admin-token",
                    Role = "Admin",
                    Email = AdminEmail
                });
            }

            _isAuthenticated = false;
            _currentUserEmail = null;
            _currentUserRole = null;

            return Task.FromResult(new AuthResponseDto
            {
                IsSuccess = false,
                Message = "Invalid email or password.",
                AccessToken = string.Empty,
                Role = string.Empty,
                Email = string.Empty
            });
        }

        public Task LogoutAsync()
        {
            _isAuthenticated = false;
            _currentUserEmail = null;
            _currentUserRole = null;

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