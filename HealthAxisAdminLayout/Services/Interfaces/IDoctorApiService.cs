using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Doctor;


namespace HealthAxisAdminLayout.Services.Interfaces
{
    public interface IDoctorApiService
    {
        Task<List<DoctorResponseDTO>> GetDoctorsAsync();

        Task<DoctorResponseDTO?> GetDoctorByIdAsync(int id);

        Task<PagedResponseDTO<DoctorResponseDTO>> GetDoctorsPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            string? specialisation,
            string? status
        );

        Task<CreateDoctorResultDTO?> CreateDoctorAsync(CreateDoctorDTO dto);

        Task<bool> UpdateDoctorAsync(int id, CreateDoctorDTO dto);

        Task<bool> DeleteDoctorAsync(int id);

        Task<bool> SetDoctorStatusAsync(int doctorId, bool status);
    }
}