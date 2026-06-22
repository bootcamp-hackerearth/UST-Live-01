using HealthAxisAdminLayout.DTOs.Auth;
using HealthAxisAdminLayout.Services.Interfaces;

namespace HealthAxisAdminLayout.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthStateService _authState;

        public AuthService(IAuthStateService authState)
        {
            _authState = authState;
        }

        public Task<AuthResponseDTO?> LoginAsync(LoginDTO loginDto)
        {
            // Mock admin login for frontend-only development
            if (loginDto.Email == "admin@healthaxis.com" &&
                loginDto.Password == "Admin@123")
            {
                var result = new AuthResponseDTO
                {
                    Token = "mock-admin-jwt-token",
                    Email = loginDto.Email,
                    Role = "Admin",
                    ReferenceId = null,
                    IsFirstLogin = false
                };

                _authState.SetLogin(result);

                return Task.FromResult<AuthResponseDTO?>(result);
            }

            return Task.FromResult<AuthResponseDTO?>(null);
        }

        public Task LogoutAsync()
        {
            _authState.Logout();
            return Task.CompletedTask;
        }

        public Task<string?> GetTokenAsync()
        {
            return Task.FromResult(_authState.Token);
        }

        public Task<string?> GetEmailAsync()
        {
            return Task.FromResult(_authState.Email);
        }

        public Task<string?> GetRoleAsync()
        {
            return Task.FromResult(_authState.Role);
        }

        public Task<bool> IsLoggedInAsync()
        {
            return Task.FromResult(_authState.IsLoggedIn);
        }

        public Task<bool> IsAdminAsync()
        {
            return Task.FromResult(_authState.IsAdmin);
        }
    }
}