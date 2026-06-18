using HealthAxis.API.DTOs.Auth;
using HealthAxis.Shared.DTOs.Auth;

namespace HealthAxis.API.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string UserId, int PatientId)> RegisterPatientAsync(
            RegisterPatientDto request,
            CancellationToken ct = default);

        Task<(bool Success, string Message, string AccessToken, string RefreshToken, int ExpiresIn)> LoginAsync(
            LoginDto request,
            CancellationToken ct = default);

        Task<(bool Success, string Message, string AccessToken, string RefreshToken, int ExpiresIn)> RefreshTokenAsync(
            RefreshTokenDto request,
            CancellationToken ct = default);
    }
}

