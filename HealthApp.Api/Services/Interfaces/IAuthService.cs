using HealthApp.Api.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool success, string message, string userId)> RegisterPatient(
            RegisterPatientDto request);

        Task<(bool success, string message, string userId, string temporaryPassword)> RegisterDoctor(
            RegisterDoctorDto request);

        Task<(bool success, string message, string token, int expiresIn)> Login(
            LoginDto request);

        Task ChangePasswordAsync(string userId, ChangePasswordDto request);
    }
}