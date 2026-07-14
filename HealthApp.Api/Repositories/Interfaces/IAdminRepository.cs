using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Repositories.Interfaces
{
    public interface IAdminRepository
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
