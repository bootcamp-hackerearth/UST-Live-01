using HealthCareApp.AdminBlazor.Dtos.Auth;

namespace HealthCareApp.AdminBlazor.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        Task LogoutAsync();

        Task<bool> IsAuthenticatedAsync();

        Task<string?> GetCurrentUserEmailAsync();

        Task<string?> GetCurrentUserRoleAsync();
    }
}
