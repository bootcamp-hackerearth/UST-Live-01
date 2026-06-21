using HealthApp.Api.Dtos;

namespace HealthApp.Api.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<AdminUserDto>> GetUsersAsync(string? role);

        Task<IEnumerable<AppointmentReportDto>> GetAppointmentReportsAsync();
    }
}