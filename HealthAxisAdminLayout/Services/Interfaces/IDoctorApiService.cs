using HealthAxisAdminLayout.DTOs.Doctor;

namespace HealthAxisAdminLayout.Services.Interfaces
{
    public interface IDoctorApiService
    {
        Task<List<DoctorResponseDTO>> GetDoctorsAsync();

        Task<DoctorResponseDTO?> GetDoctorByIdAsync(int id);

        Task<bool> CreateDoctorAsync(CreateDoctorDTO dto);

        Task<bool> DeleteDoctorAsync(int id);

        Task<bool> SetDoctorStatusAsync(int doctorId, bool status);
    }
}