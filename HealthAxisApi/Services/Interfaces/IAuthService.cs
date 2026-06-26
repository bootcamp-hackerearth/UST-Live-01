using HealthAxis.Shared.DTOs.Auth;
using HealthAxis.Shared.DTOs.User;
using HealthAxisCore_Api.DTOs.User;


namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IAuthService
    {
        // ✅ Existing
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO request);

        Task<AuthResponseDTO> LoginAsync(LoginDTO request);

        Task ChangePasswordAsync(ChangePasswordDTO request);

        // ✅ NEW (Refresh Token Support)
        Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken);

        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
