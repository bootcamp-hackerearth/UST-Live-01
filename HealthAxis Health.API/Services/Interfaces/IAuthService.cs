using HealthAxis_Health.API.DTOs.AuthDtos;
using HealthAxisHealth.API.DTOs.AuthDtos;

namespace HealthAxisHealth.API.Services.Interfaces
{
    public interface IAuthService
    {
        #region Methods

        Task<RegisterResponseDto>
            RegisterAsync(
                RegisterDto dto);

        Task<LoginResponseDto>
            LoginAsync(
                LoginDto dto);

        Task<LoginResponseDto>
            RefreshTokenAsync(
                RefreshTokenDto dto);

         Task LogoutAsync(string refreshToken);

        #endregion
    }
}