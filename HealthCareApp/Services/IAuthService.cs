using HealthCareApp.Models.Dtos;

namespace HealthCareApp.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message,string UserId)> Register(RegisterDto request);
        Task<(bool Success, string Message, string Token,int ExpiresIn)> Login(LoginDto request);


    }
}
