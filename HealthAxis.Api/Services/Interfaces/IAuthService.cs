using HealthAxisCore_Api.Models.DTOs;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success,string Error,string UserId)> Register(RegisterDto request);
        Task<(bool Success,string Error,string Token,int ExpiresIn)> Login(LoginDto request);
    }
}
