using HealthCare.Api.DTOs.Authentication;


namespace HealthCare.Api.Services.Interfaces
{
    public interface IAuthService
    {

        Task RegisterPatientAsync(PatientRegisterDto dto);

        Task RegisterDoctorAsync(DoctorRegisterDto dto);

        Task<AuthResponseDto> LoginAsync(LoginDto dto);

        Task ChangePasswordAsync(string userId, ChangePasswordDto dto);
    }
}
