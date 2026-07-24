using HealthAxis.Shared.DTOs.Auth;
using HealthAxis.Shared.DTOs.User;
using HealthAxis.Shared;
    
namespace HealthAxisAdminLayout.Services.Interfaces
{
    public interface IAuthStateService
{
    string? Token { get; }
    string? Email { get; }
    string? Role { get; }
    int? ReferenceId { get; }
    bool IsFirstLogin { get; }

    bool IsLoggedIn { get; }
    bool IsAdmin { get; }

    Task SetLoginAsync(AuthResponseDto response);
    Task LoadFromStorageAsync();
    Task LogoutAsync();
}
}




