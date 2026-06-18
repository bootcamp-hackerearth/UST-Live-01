using HealthAxisCore_Api.DTOs.User;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string AccessToken, int ExpiryInSeconds)>
            RegisterAsync(RegisterDTO request);

        Task<(bool Success, string Message, string AccessToken, int ExpiryInSeconds)>
            LoginAsync(LoginDTO request);
    }
}
