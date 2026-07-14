using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<AdminUserDto>> GetUsersAsync(
            string? role);

        Task<IEnumerable<AppointmentReportDto>>
            GetAppointmentReportsAsync();

        Task<IEnumerable<AdminDoctorLeaveDto>>
            GetDoctorLeavesAsync(
                string? search,
                string? status,
                DateOnly? fromDate,
                DateOnly? toDate,
                int pageNumber,
                int pageSize,
                CancellationToken ct = default);
    }
}
