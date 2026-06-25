using Microsoft.EntityFrameworkCore;
using HealthAxis.API.Data;
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

        public AdminRepository(
            HealthAxisDbContext context)
        {
            _context = context;
        }

        public Task<int> CountPatientsAsync()
            => _context.Patients.CountAsync();

        public Task<int> CountActivePatientsAsync()
            => _context.Patients
                .CountAsync(p => p.IsActive);

        public Task<int> CountDoctorsAsync()
            => _context.Doctors.CountAsync();

        public Task<int> CountActiveDoctorsAsync()
            => _context.Doctors
                .CountAsync(d => d.IsActive);

        public Task<int> CountTodayAppointmentsAsync()
            => _context.Appointments
                .CountAsync(a =>
                    a.ScheduledDate ==
                    DateOnly.FromDateTime(DateTime.Today));

        public Task<int> CountPendingAppointmentsAsync()
            => _context.Appointments
                .CountAsync(a =>
                    a.Status == AppointmentStatus.Pending);

        public Task<int> CountCompletedAppointmentsAsync()
            => _context.Appointments
                .CountAsync(a =>
                    a.Status == AppointmentStatus.Completed);

        public Task<int> CountHealthRecordsAsync()
            => _context.HealthRecords.CountAsync();

        public async Task<IEnumerable<User>>
            GetUsersAsync()
        {
            return await _context.Users
                .OrderBy(u => u.UserId)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    u => u.UserId == id);
        }
        public async Task<bool> ResolveUserActiveStatusAsync(string email, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(role))
                return false;

            switch (role)
            {
                case "Admin":
                    return true;

                case "Doctor":
                    return await _context.Doctors
                        .AnyAsync(d => d.Email == email && d.IsActive);

                case "Patient":
                    return await _context.Patients
                        .AnyAsync(p => p.Email == email && p.IsActive);

                default:
                    return false;
            }
        }

    }
}