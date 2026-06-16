using HealthCare.Api.DTOs.Authentication;

namespace HealthCare.Api.Services.Interfaces
{
    public interface IAuthService
    {

        Task<(bool Success, string message, string UserId)> Register(RegisterDto request);

        Task<(bool Success, string message,string token,int expiresIn)>Login(LoginDto request);
    }
}
