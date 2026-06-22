using S3_HealthAxis.Shared.DTOs.Admin;

namespace S3_HealthAxisApi.Services.Interface
{
    public interface IAdminService
    {
        Task<AdminDashboardDto> GetDashboardAsync();

        Task<AdminStatisticsDto> GetStatisticsAsync();

        Task<IEnumerable<UserManagementDto>> GetUsersAsync();

        Task<UserManagementDto?> GetUserByIdAsync(int id);
    }
}