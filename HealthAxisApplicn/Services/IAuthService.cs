using HealthAxisApplicn.Dto;

namespace HealthAxisApplicn.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string UserId)> RegisterAsync(RegisterDto request);
        Task<(bool success, string message, string token, int ExpiresIn)> LoginAsync(LoginDto request);

    }
}
