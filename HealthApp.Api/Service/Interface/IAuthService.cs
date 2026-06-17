using HealthApp.Api.Dto;

namespace HealthApp.Api.Service.Interface
{
    public interface IAuthService
    {
        Task<(bool success, string message, string token,int ExpiresIn)> Login(LoginDto register);
        Task<(bool success, string message, string userId)> Register(RegisterDto register);
    }
}
