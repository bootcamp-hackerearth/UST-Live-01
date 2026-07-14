using HealthCareApp.Shared.Dtos.DoctorDto;
using HealthCareApp.Shared.Dtos.Doctors;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Enums;

namespace HealthCareApp.Services
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllDoctorsAsync();

        Task<List<DoctorDto>> GetAllActiveDoctorsAsync();

        Task<DoctorDto> GetDoctorByIdAsync(int doctorId);

        Task<List<DoctorDto>> GetDoctorsBySpecialisationAsync(SpecialisationType specialisation);

        Task<List<DoctorDto>> GetActiveDoctorsBySpecialisationAsync(SpecialisationType specialisation);

        Task<DoctorCreatedResponseDto> CreateDoctorByAdminAsync(CreateDoctorDto dto);

        Task<DoctorDto> UpdateDoctorAsync(int doctorId, UpdateDoctorDto dto);

        Task<DoctorDto> DeleteDoctorAsync(int doctorId);

        Task<DoctorAvailabilityResponseDto> GetDoctorAvailabilityAsync(int doctorId,DateTime? date);

        Task<DoctorDto> GetMyProfileAsync(string identityUserId);

        Task<PagedResponse<DoctorDto>> GetAllDoctorsPagedAsync(DoctorPaginationQueryDto query);
    }
}