using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.DTOs.DoctorDtos;
using HealthAxisHealth.API.DTOs.ReportDtos;
using HealthAxisHealth.API.DTOs.UserDtos;
using HealthAxisHealth.API.Helpers;

namespace HealthAxisHealth.API.Services.Interfaces
{
    public interface IAdminService
    {
        Task<PagedResultDto<DoctorDto>>
            GetDoctorsAsync(
                PaginationParams pagination);

        Task<int>
            CreateDoctorAsync(
                CreateDoctorDto dto);

        Task UpdateDoctorAsync(
            int doctorId,
            UpdateDoctorDto dto);

        Task<PagedResultDto<UserDto>>
            GetUsersAsync(
                PaginationParams pagination,
                string? role);

        Task<IEnumerable<AppointmentReportDto>>
            GetAppointmentReportAsync();
    }
}
