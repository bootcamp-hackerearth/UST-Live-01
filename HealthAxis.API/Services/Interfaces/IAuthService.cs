using HealthAxis.API.DTO.AuthDtos;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IAuthService
    {
            Task<(bool Success, string Message, string UserId, int StatusCode)> Register(RegisterDto request);
            Task<(bool Success, string Message, string Token, int ExpiresIn)> Login(LoginDto request);


    }
}

