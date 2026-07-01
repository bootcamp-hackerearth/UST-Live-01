using HealthApp.Shared.Dto;

namespace HealthApp.Api.Service.Interface
{
    public interface IAuthService
    {
        Task<(bool success, string message, string userId)> 
            RegisterPatientAsync(PatientRegisterDto request);

        Task<(bool success, string message, string userId, string temporaryPassword)> RegisterDoctorByAdminAsync(DoctorRegisterDto request);

        Task<(bool success, string message, string userId)> 
            Register(RegisterDto register);

        Task<(bool success, string message, string accessToken, int expiresIn, string role)> Login(LoginDto login);

        Task<(bool success, string message)> ChangePasswordAsync(string identityUserId, ChangePasswordDto dto);
    }
}