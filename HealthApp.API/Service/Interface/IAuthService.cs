using HealthApp.API.Models.DTOs;

namespace HealthApp.API.Service.Interface
{
    public interface IAuthService
    {
        Task<(bool Success, string Error, string UserId)> Register(RegisterDto request);
        Task<(bool Success, string Error, string Token, int ExpiresIn)> Login(LoginDto request);
    }
}
