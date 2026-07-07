using HealthCareApp.Shared.Dtos.Doctors;
using HealthCareApp.Shared.Dtos.Pagination;

namespace HealthCareApp.AdminBlazor.Services.Interfaces
{
    public interface IDoctorAdminService
    {
        Task<List<DoctorDto>> GetAllDoctorsAsync();

        Task<PagedResponse<DoctorDto>> GetDoctorsPagedAsync(DoctorPaginationQueryDto query);

        Task<DoctorDto?> GetDoctorByIdAsync(int doctorId);

        Task<DoctorCreatedResponseDto> CreateDoctorAsync(CreateDoctorDto doctorDto);

        Task<DoctorDto?> UpdateDoctorAsync(int doctorId, UpdateDoctorDto doctorDto);

        Task<DoctorDto?> ToggleDoctorStatusAsync(int doctorId);

    }
}