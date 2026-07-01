using HealthAxisCore_Api.Models.Dtos;
using System.Security.Claims;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterPatientAsync(
            RegisterPatientDto request,
            CancellationToken ct = default);

        Task<AuthResponseDto> LoginAsync(
            LoginDto request,
            CancellationToken ct = default);

        Task<AuthResponseDto> RefreshTokenAsync(
            RefreshTokenRequestDto request,
            CancellationToken ct = default);

        Task<ForgotPasswordResponseDto> ForgotPasswordAsync(
            ForgotPasswordDto request);

        Task<string> ResetPasswordAsync(
            ResetPasswordDto request);
        Task<string> ChangeFirstLoginPasswordAsync(
    ChangeFirstLoginPasswordDto request,
    ClaimsPrincipal user,
    CancellationToken ct = default);

        Task<string> ChangePasswordAsync(
    ChangePasswordDto request,
    ClaimsPrincipal user,
    CancellationToken ct = default);
    }
}