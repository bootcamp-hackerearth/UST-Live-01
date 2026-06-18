using HealthApp.Api.Dtos;

namespace HealthApp.Api.Services.Interfaces
{

    public interface IAdminService
    {
        Task<IEnumerable<AdminUserDto>> GetUsersAsync(string? role);

        Task<IEnumerable<AppointmentReportDto>> GetAppointmentReportsAsync();
    }

}
