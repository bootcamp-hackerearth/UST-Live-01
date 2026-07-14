using HealthApp.Api.Data;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repositories.Impl
{
    public class AdminRepository : IAdminRepository
    {
        private readonly HealthAppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminRepository(
            HealthAppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<AdminUserDto>> GetUsersAsync(
            string? role)
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<AdminUserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (!string.IsNullOrWhiteSpace(role) &&
                    !roles.Contains(
                        role,
                        StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                result.Add(new AdminUserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    PatientId = user.PatientId,
                    DoctorId = user.DoctorId,
                    Roles = roles
                });
            }

            return result;
        }

        public async Task<IEnumerable<AppointmentReportDto>>
            GetAppointmentReportsAsync()
        {
            var reports = await _context.Appointments
                .GroupBy(appointment => appointment.ScheduledDate)
                .Select(group => new AppointmentReportDto
                {
                    Date = group.Key,
                    Confirmed = group.Count(appointment =>
                        appointment.Status == AppointmentStatus.Confirmed),
                    Cancelled = group.Count(appointment =>
                        appointment.Status == AppointmentStatus.Cancelled),
                    Completed = group.Count(appointment =>
                        appointment.Status == AppointmentStatus.Completed),
                    Pending = group.Count(appointment =>
                        appointment.Status == AppointmentStatus.Pending),
                    Total = group.Count()
                })
                .OrderBy(report => report.Date)
                .ToListAsync();

            return reports;
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
            var today = DateOnly.FromDateTime(DateTime.Today);

            var query = _context.DoctorLeaves
                .AsNoTracking()
                .Include(leave => leave.Doctor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(leave =>
                    leave.Doctor != null &&
                    leave.Doctor.FullName != null &&
                    leave.Doctor.FullName.Contains(search));
            }

            if (fromDate.HasValue)
            {
                query = query.Where(leave =>
                    leave.EndDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(leave =>
                    leave.StartDate <= toDate.Value);
            }

            query = status switch
            {
                "Current" => query.Where(leave =>
                    leave.StartDate <= today &&
                    leave.EndDate >= today),

                "Upcoming" => query.Where(leave =>
                    leave.StartDate > today),

                "Past" => query.Where(leave =>
                    leave.EndDate < today),

                _ => query
            };

            return await query
                .OrderByDescending(leave => leave.StartDate)
                .ThenByDescending(leave => leave.CreatedAtUtc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(leave => new AdminDoctorLeaveDto
                {
                    DoctorLeaveId = leave.DoctorLeaveId,
                    DoctorId = leave.DoctorId,
                    DoctorName = leave.Doctor != null
                        ? leave.Doctor.FullName ?? "Doctor"
                        : "Doctor",
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                    Reason = leave.Reason,
                    CreatedAtUtc = leave.CreatedAtUtc,
                    Status = today < leave.StartDate
                        ? "Upcoming"
                        : today > leave.EndDate
                            ? "Past"
                            : "Current",
                    IsSingleDayLeave =
                        leave.StartDate == leave.EndDate
                })
                .ToListAsync(ct);
        }
    }
}