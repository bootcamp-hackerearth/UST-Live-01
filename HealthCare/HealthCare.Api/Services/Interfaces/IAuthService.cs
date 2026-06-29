using Healthcare.Shared.DTOs.Authentication;
using Healthcare.Shared.DTOs.Patient;



namespace HealthCare.Api.Services.Interfaces
{
    public interface IAuthService
    {

        Task RegisterPatientAsync(CreatePatientDto dto);

        Task RegisterDoctorAsync(DoctorRegisterDto dto);

        Task<AuthorResponseDto> LoginAsync(LoginDto dto);

        Task ChangePasswordAsync(string userId, ChangePasswordDto dto);

        Task<bool> EmailExistsAsync(string email);
    }
}
