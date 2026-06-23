
using HealthApp.Shared.DTOs;

namespace HealthApp.API.Service.Interface;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);

    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto);

    Task ChangePasswordAsync(ChangePasswordDto dto);
}