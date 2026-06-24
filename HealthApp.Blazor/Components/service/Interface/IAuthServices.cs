using HealthApp.Shared.Dto;

namespace HealthApp.Blazor.Components.service.Interface
{
    public interface IAuthService
    {
        bool IsLoggedIn { get; }
        string? Token { get; }
        string? Role { get; }

        Task InitializeAsync();
        Task<LoginResponseDto?> LoginAsync(LoginDto request);
        Task LogoutAsync();
    }
}