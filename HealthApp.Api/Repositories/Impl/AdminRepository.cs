using HealthApp.Api.Data;
using HealthApp.Api.Dtos;
using HealthApp.Api.Enums;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
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

        public async Task<IEnumerable<AdminUserDto>> GetUsersAsync(string? role)
        {
            var users = await _userManager.Users.ToListAsync();

            var result = new List<AdminUserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (!string.IsNullOrWhiteSpace(role) &&
                    !roles.Contains(role, StringComparer.OrdinalIgnoreCase))
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

        public async Task<IEnumerable<AppointmentReportDto>> GetAppointmentReportsAsync()
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
    }
}