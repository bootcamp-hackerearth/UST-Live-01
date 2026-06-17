using HealthApp.Api.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool success, string message, string userId)> Register(RegisterDto request);
        Task<(bool success, string message, string token, int expiresIn)> Login(LoginDto request);
    }
}
