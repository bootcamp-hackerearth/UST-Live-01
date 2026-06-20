using HealthAxis.API.DTO.AuthDtos;
using HealthAxis.API.DTO.DoctorDtos;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IAuthService
    {
            Task<(bool Success, string Message, string UserId, int StatusCode)> Register(RegisterDto request);
            Task<(bool Success, string Message, string Token, int ExpiresIn)> Login(LoginDto request);
            Task<(bool Success, string Message, int StatusCode)> ChangePassword( string userId, ChangePasswordDto request);


    }
}

