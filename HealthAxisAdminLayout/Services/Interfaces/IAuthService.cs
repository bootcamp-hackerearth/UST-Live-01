using HealthAxisAdminLayout.DTOs.Auth;

namespace HealthAxisAdminLayout.Services.Interfaces
{
    using HealthAxisAdminLayout.DTOs.Auth;
        public interface IAuthService
        {
            Task<AuthResponseDTO?> LoginAsync(LoginDTO loginDto);

            Task LogoutAsync();

            Task<string?> GetTokenAsync();

            Task<string?> GetEmailAsync();

            Task<string?> GetRoleAsync();

            Task<bool> IsLoggedInAsync();

            Task<bool> IsAdminAsync();
        }
    }