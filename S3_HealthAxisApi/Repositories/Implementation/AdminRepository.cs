using HealthAxis.API.Data;
using Microsoft.EntityFrameworkCore;
using S3_HealthAxis.Shared.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using System.Diagnostics.CodeAnalysis;

namespace S3_HealthAxisApi.Repository.Implementation
{
    [ExcludeFromCodeCoverage]
    public class AdminRepository : IAdminRepository
    {
        private readonly HealthAxisDbContext _context;

        public AdminRepository(HealthAxisDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountPatientsAsync()
        {
            return await _context.Patients.CountAsync();
        }

        public async Task<int> CountActivePatientsAsync()
        {
            return await _context.Patients.CountAsync(p => p.IsActive);
        }

        public async Task<int> CountDoctorsAsync()
        {
            return await _context.Doctors.CountAsync();
        }

        public async Task<int> CountActiveDoctorsAsync()
        {
            return await _context.Doctors.CountAsync(d => d.IsActive);
        }

        public async Task<int> CountTodayAppointmentsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            return await _context.Appointments.CountAsync(a => a.ScheduledDate == today);
        }

        public async Task<int> CountPendingAppointmentsAsync()
        {
            return await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Pending);
        }

        public async Task<int> CountCompletedAppointmentsAsync()
        {
            return await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Completed);
        }

        public async Task<int> CountHealthRecordsAsync()
        {
            return await _context.HealthRecords.CountAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.AppUsers.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<bool> ResolveUserActiveStatusAsync(string email, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(role))
                return false;

            switch (role.Trim().ToLowerInvariant())
            {
                case "admin":
                    return true;

                case "doctor":
                    return await _context.Doctors
                        .AsNoTracking()
                        .AnyAsync(d => d.Email == email && d.IsActive);

                case "patient":
                    return await _context.Patients
                        .AsNoTracking()
                        .AnyAsync(p => p.Email == email && p.IsActive);

                default:
                    return false;
            }
        }
    }
}
