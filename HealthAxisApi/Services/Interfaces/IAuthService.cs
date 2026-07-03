using HealthAxis.Shared.DTOs.Auth;
using HealthAxis.Shared.DTOs.User;
using HealthAxisCore_Api.DTOs.User;


namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IAuthService
    {
        // ✅ Existing
        Task<AuthResponseDto> RegisterAsync(RegisterDto request);

        Task<AuthResponseDto> LoginAsync(LoginDto request);

        Task ChangePasswordAsync(ChangePasswordDto request);

        // ✅ NEW (Refresh Token Support)
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);

        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
