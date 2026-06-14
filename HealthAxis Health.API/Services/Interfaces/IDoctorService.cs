using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.DTOs.DoctorDtos;
using HealthAxisHealth.API.Helpers;

namespace HealthAxisHealth.API.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<PagedResultDto<DoctorDto>>
            GetPagedAsync(
                PaginationParams pagination);

        Task<DoctorDto?>
            GetByIdAsync(
                int doctorId);

        Task<DoctorDto?>
            GetByUserIdAsync(
                int userId);

        Task<IEnumerable<DoctorDto>>
            GetBySpecialisationAsync(
                string specialisation);

        Task<IEnumerable<DoctorAvailabilityDto>>
            GetAvailabilityAsync(
                int doctorId);

        Task UpdateAsync(
            int doctorId,
            UpdateDoctorDto updateDoctorDto);
    }
}
