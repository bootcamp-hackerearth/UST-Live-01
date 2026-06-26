using HealthAxis.Shared.DTOs.Auth;
using HealthAxis.Shared.DTOs.User;

namespace HealthAxisAdminLayout.Services.Interfaces
{
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