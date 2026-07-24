using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Doctor;


namespace HealthAxisAdminLayout.Services.Interfaces
{
    public interface IDoctorApiService
    {
        Task<List<DoctorResponseDto>> GetDoctorsAsync();

        Task<DoctorResponseDto?> GetDoctorByIdAsync(int id);

        Task<PagedResponseDto<DoctorResponseDto>> GetDoctorsPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            string? specialisation,
            string? status
        );

        Task<CreateDoctorResultDto?> CreateDoctorAsync(CreateDoctorDto dto);

        Task<bool> UpdateDoctorAsync(int id, CreateDoctorDto dto);

        Task<bool> DeleteDoctorAsync(int id);

        Task<bool> SetDoctorStatusAsync(int doctorId, bool status);
    }
}

