using HealthCare.Api.DTOs.Authentication;
using HealthCare.Api.DTOs.Doctor;
using HealthCare.Api.DTOs.Patient;

namespace HealthCare.Api.Services.Interfaces
{
    public interface IAuthService
    {

        Task RegisterPatientAsync(CreatePatientDto dto);

        Task RegisterDoctorAsync(CreateDoctorDto dto);

        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
