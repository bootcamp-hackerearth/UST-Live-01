using HealthAxis.Shared.DTOs.Auth;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string UserId)> Register(RegisterDto request);

        Task<(bool Success, string Message, string AccessToken, string RefreshToken, int ExpiresIn, bool RequiresPasswordChange)> Login(LoginDto request);

        Task<(bool Success, string Message)> ChangePassword(ChangePasswordDto request);

        Task<(bool Success, string Message, string AccessToken, string RefreshToken, int ExpiresIn)> RefreshToken(RefreshTokenRequestDto request);
    }
}