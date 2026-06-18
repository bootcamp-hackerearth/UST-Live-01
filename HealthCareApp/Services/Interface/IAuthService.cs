using HealthCareApp.Dtos;
using HealthCareApp.Models.Dtos;

namespace HealthCareApp.Services.Interface
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string UserId)> RegisterPatientAsync(PatientRegisterDto request);

        Task<(bool Success, string Message, string Token, int ExpiresIn)> Login(LoginDto request);

        Task<(bool Success, string Message)> ChangePasswordAsync(string userId, ChangePasswordDto request);
    }
}