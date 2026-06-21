using HealthCareApp.AdminBlazor.Dtos.Doctors;

namespace HealthCareApp.AdminBlazor.Services.Interfaces
{
    public interface IDoctorAdminService
    {
        Task<List<DoctorDto>> GetAllDoctorsAsync();

        Task<DoctorDto?> GetDoctorByIdAsync(int doctorId);

        Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto request);

        Task<DoctorDto?> UpdateDoctorAsync(int doctorId, UpdateDoctorDto request);

        Task<bool> DeleteDoctorAsync(int doctorId);

        Task<DoctorDto?> ToggleDoctorStatusAsync(int doctorId);
    }
}