using HealthAxisCore_Admin.Models;

namespace HealthAxisCore_Admin.Services
{
    public class AuthService
    {
        private readonly TokenService _tokenService;

        public AuthService(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto request)
        {
            /*
             * ============================================================
             * TEMPORARY DISCONNECTED VERSION
             * ============================================================
             * This version does not call the API.
             * Replace this file later with the connected API version.
             * ============================================================
             */

            if (request.Email != "admin@healthcare.com" ||
                request.Password != "Admin@123")
            {
                throw new Exception("Invalid email or password");
            }

            var result = new AuthResponseDto
            {
                UserId = "admin-user-1",
                PatientId = null,
                DoctorId = null,
                FullName = "System Admin",
                Email = "admin@healthcare.com",
                Role = "Admin",
                AccessToken = "hardcoded-admin-token",
                RefreshToken = "hardcoded-refresh-token",
                ExpiresIn = 3600
            };

            await _tokenService.SetTokenAsync(
                result.AccessToken,
                result.Role);

            return result;
        }

        public async Task LogoutAsync()
        {
            await _tokenService.ClearAsync();
        }
    }
}