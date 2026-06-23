using HealthApp.Api.Dto;

namespace HealthApp.Api.Service.Interface
{
    public interface IAuthService
    {
        Task<(bool success, string message, string token,int ExpiresIn)> Login(LoginDto register);
        Task<(bool success, string message, string userId)> Register(RegisterDto register);



        Task<(bool success, string message, string userId)> 
            RegisterPatientAsync(PatientRegisterDto request);

        Task<(bool success, string message, string userId, 
            string temporaryPassword)> RegisterDoctorByAdminAsync(DoctorRegisterDto request);
    }
}
