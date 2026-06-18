using S3_HealthAxisApi.DTOs.Auth;

namespace S3_HealthAxisApi.Services.Interface
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, AuthResponseDto? Data)> RegisterAsync(RegisterDto request);
        Task<(bool Success, string Message, AuthResponseDto? Data)> RegisterPatientAsync(RegisterPatientDto request);

        Task<(bool Success, string Message, AuthResponseDto? Data)> LoginAsync(LoginDto request);

        Task<(bool Success, string Message, AuthResponseDto? Data)> RefreshTokenAsync(RefreshTokenDto request);
    }
}