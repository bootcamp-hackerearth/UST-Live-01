using HealthCareApp.Shared.Dtos.Auth;
using HealthCareApp.Shared.Dtos.Patients;


namespace HealthCareApp.Services.Interface
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, int PatientId)> RegisterPatientAsync(PatientRegisterDto request);
        
        Task<(bool Success, string Message, string Token, int ExpiresIn,string Role,bool MustChangePassword)> Login(LoginDto request);

        Task<(bool Success, string Message)> ChangePasswordAsync(string userId, ChangePasswordDto request);
    }
}