using Microsoft.EntityFrameworkCore;
using HealthAxis.API.Data;
using S3_HealthAxisApi.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;

namespace S3_HealthAxisApi.Repository.Implementation
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthAxisDbContext _context;

        public DoctorRepository(HealthAxisDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync(string? sortBy, int? specialisation)
        {
            IQueryable<Doctor> query = _context.Doctors;

            if (specialisation.HasValue)
            {
                if (!Enum.IsDefined(typeof(DoctorSpecialisation), specialisation.Value))
                    throw new ArgumentException("Invalid doctor specialisation.");

                var doctorSpecialisation = (DoctorSpecialisation)specialisation.Value;
                query = query.Where(d => d.Specialisation == doctorSpecialisation);
            }

            query = sortBy?.ToLower() switch
            {
                "name" => query.OrderBy(d => d.FullName),
                "name_desc" => query.OrderByDescending(d => d.FullName),
                "id" => query.OrderBy(d => d.DoctorId),
                _ => query.OrderBy(d => d.DoctorId)
            };

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetActiveBySpecialisationAsync(int specialisation)
        {
            if (!Enum.IsDefined(typeof(DoctorSpecialisation), specialisation))
                throw new ArgumentException("Invalid doctor specialisation.");

            var doctorSpecialisation = (DoctorSpecialisation)specialisation;

            return await _context.Doctors
                .Where(d => d.IsActive && d.Specialisation == doctorSpecialisation)
                .OrderBy(d => d.FullName)
                .ToListAsync();
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors.FindAsync(id);
        }

        public async Task AddAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
        }

        public Task UpdateAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<int>> GetBookedSlotsAsync(int doctorId,DateOnly date)
        {
            return await _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate == date &&
                    a.Status != AppointmentStatus.Cancelled)
                .Select(a => (int)a.TimeSlot)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Doctors.AnyAsync(d => d.DoctorId == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}