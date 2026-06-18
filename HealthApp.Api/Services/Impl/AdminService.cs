using HealthApp.Api.Data;
using HealthApp.Api.Dtos;
using HealthApp.Api.Enums;
using HealthApp.Api.Models;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Services.Impl
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HealthAppDbContext _context;

        public AdminService(
            UserManager<ApplicationUser> userManager,
            HealthAppDbContext context)
        {
            _userManager = userManager;
            _context = context;
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
                .GroupBy(a => a.ScheduledDate)
                .Select(g => new AppointmentReportDto
                {
                    Date = g.Key,
                    Confirmed = g.Count(a => a.Status == AppointmentStatus.Confirmed),
                    Cancelled = g.Count(a => a.Status == AppointmentStatus.Cancelled),
                    Completed = g.Count(a => a.Status == AppointmentStatus.Completed),
                    Pending = g.Count(a => a.Status == AppointmentStatus.Pending),
                    Total = g.Count()
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

            return reports;
        }
    }
}