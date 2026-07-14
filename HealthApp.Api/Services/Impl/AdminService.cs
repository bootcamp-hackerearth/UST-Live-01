using HealthApp.Api.Exceptions;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Services.Impl
{
    public class AdminService : IAdminService
    {
        private static readonly string[] ValidLeaveStatuses =
        {
            "Current",
            "Upcoming",
            "Past"
        };

        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<IEnumerable<AdminUserDto>> GetUsersAsync(
            string? role)
        {
            return await _adminRepository.GetUsersAsync(role);
        }

        public async Task<IEnumerable<AppointmentReportDto>>
            GetAppointmentReportsAsync()
        {
            return await _adminRepository.GetAppointmentReportsAsync();
        }

        public async Task<IEnumerable<AdminDoctorLeaveDto>>
            GetDoctorLeavesAsync(
                string? search,
                string? status,
                DateOnly? fromDate,
                DateOnly? toDate,
                int pageNumber,
                int pageSize,
                CancellationToken ct = default)
        {
            if (pageNumber <= 0)
            {
                throw new InvalidRequestException(
                    "Page number must be greater than zero.");
            }

            if (pageSize <= 0 || pageSize > 100)
            {
                throw new InvalidRequestException(
                    "Page size must be between 1 and 100.");
            }

            if (fromDate.HasValue &&
                toDate.HasValue &&
                toDate.Value < fromDate.Value)
            {
                throw new InvalidRequestException(
                    "To date cannot be before from date.");
            }

            var normalizedStatus = string.IsNullOrWhiteSpace(status)
                ? null
                : ValidLeaveStatuses.FirstOrDefault(item =>
                    item.Equals(
                        status.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(status) &&
                normalizedStatus == null)
            {
                throw new InvalidRequestException(
                    "Leave status must be Current, Upcoming, or Past.");
            }

            return await _adminRepository.GetDoctorLeavesAsync(
                search?.Trim(),
                normalizedStatus,
                fromDate,
                toDate,
                pageNumber,
                pageSize,
                ct);
        }
    }
}
