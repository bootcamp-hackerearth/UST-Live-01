using S3_HealthAxis.Shared.DTOs.Auth; // Points to your new shared project

namespace S3_HealthAxis.Blazor.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginDto request);
        Task LogoutAsync();
    }
}