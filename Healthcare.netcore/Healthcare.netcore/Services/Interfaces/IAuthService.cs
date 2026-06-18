using HealthAxis.API.Models.Auth;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string UserId)> Register(RegisterDto request);

        Task<(bool Success, string Message, string Token, int ExpiresIn, bool RequiresPasswordChange)> Login(LoginDto request);

        Task<(bool Success, string Message)> ChangePassword(ChangePasswordDto request);
    }
}
