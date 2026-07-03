using HealthAxisApplicn.Data;
using HealthAxisApplicn.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisApplicn.Repositories.Impl
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        private readonly AppDbContext _context;
        public DoctorRepository(AppDbContext context): base(context)
        {
            _context = context;
        }
        public async Task<List<Doctor>> GetActiveDoctorsAsync(CancellationToken ct = default)
        {
            var availableDoctors = await _context.Set<Doctor>().Where(d => d.IsActive).ToListAsync(ct);
            return availableDoctors;
        }

        public async Task<List<Doctor>> SearchBySpecialisationAsync(string specialisation, CancellationToken ct = default)
        {
            specialisation = specialisation.Trim().ToLower();
            var existing = await _context.Set<Doctor>().Where(d => d.Specialisation.ToLower() == specialisation).ToListAsync(ct);
            return existing;
        }

        public async Task<List<Doctor>> SearchByNameAsync(string name, CancellationToken ct = default)
        {
            var existing = await _context.Set<Doctor>().Where(d => d.DoctorName.ToLower().Contains(name.ToLower())).ToListAsync(ct);
            return existing;
        }

        public async Task<List<Doctor>> SearchAsync(string query, CancellationToken ct = default)
        {
            query = query.ToLower();

            return await _context.Doctors
                .Where(d => d.DoctorName.ToLower().Contains(query)
                         || d.Specialisation.ToLower().Contains(query))
                .ToListAsync(ct);
        }

        public async Task<Doctor?> GetByUserIdAsync(string userId)
        {
            return await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task<List<Doctor>> FilterAsync(string? name, string? specialization)
        {
            var query = _context.Doctors.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(d => d.DoctorName.ToLower().Contains(name.ToLower()));
            }

            if (!string.IsNullOrEmpty(specialization))
            {
                query = query.Where(d => d.Specialisation.ToLower().Contains(specialization.ToLower()));
            }

            return await query.ToListAsync();
        }
    }
}
