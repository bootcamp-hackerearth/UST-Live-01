using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Services.Impl
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<IEnumerable<AdminUserDto>> GetUsersAsync(string? role)
        {
            return await _adminRepository.GetUsersAsync(role);
        }

        public async Task<IEnumerable<AppointmentReportDto>> GetAppointmentReportsAsync()
        {
            return await _adminRepository.GetAppointmentReportsAsync();
        }
    }
}