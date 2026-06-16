using Microsoft.EntityFrameworkCore;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Enums;

namespace HealthAxisCore_Api.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly HealthAppDbContext _context;

        public DoctorRepository(HealthAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetDoctors(string? name, SpecialisationType? specialization, bool? isActive)
        {
            return await _context.Doctors
                .Where(d =>
                    (string.IsNullOrEmpty(name) || d.DoctorName.Contains(name)) &&
                    (!specialization.HasValue || d.Specialisation == specialization) &&
                    (!isActive.HasValue || d.IsActive == isActive))
                .ToListAsync();
        }

        public async Task SetStatus(int doctorId, bool status)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor != null)
            {
                doctor.IsActive = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsDoctorAvailable(int doctorId, DateTime date)
        {
            return !await _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId && a.ScheduledDate.Date == date.Date);
        }
    }
}
