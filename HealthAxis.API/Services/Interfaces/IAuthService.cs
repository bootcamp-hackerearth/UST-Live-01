using HealthAxis.Shared.DTO.AuthDtos;
using HealthAxis.Shared.DTO.DoctorDtos;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string UserId, int StatusCode)> Register(
            RegisterDto request);

        Task<(bool Success, string Message, string AccessToken, string RefreshToken, int ExpiresIn)> Login(
            LoginDto request);

        Task<(bool Success, string Message, string AccessToken, string RefreshToken, int ExpiresIn, int StatusCode)> RefreshToken(
            RefreshTokenDto request);

        Task<(bool Success, string Message, int StatusCode)> ChangePassword(
            string userId,
            ChangePasswordDto request);

        Task<(bool Success, string Message, int StatusCode)> AdminResetPassword(
            AdminResetPasswordDto request);
    }
}