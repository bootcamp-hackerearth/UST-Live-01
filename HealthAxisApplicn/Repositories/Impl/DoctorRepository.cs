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
            specialisation = specialisation.Trim();

            var existing = await _context.Set<Doctor>()
                .Where(d => d.Specialisation == specialisation)
                .ToListAsync(ct);
            return existing;
        }


        public async Task<List<Doctor>> SearchByNameAsync(string name, CancellationToken ct = default)
        {
            return await _context.Doctors
                .Where(d => EF.Functions.Like(d.DoctorName, $"%{name}%"))
                .ToListAsync(ct);
        }



        public async Task<List<Doctor>> SearchAsync(string query, CancellationToken ct = default)
        {
            return await _context.Doctors
                .Where(d =>
                    EF.Functions.Like(d.DoctorName, $"%{query}%") ||
                    EF.Functions.Like(d.Specialisation, $"%{query}%"))
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

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(d =>
                    EF.Functions.Like(d.DoctorName, $"%{name}%"));
            }

            if (!string.IsNullOrWhiteSpace(specialization))
            {
                query = query.Where(d =>
                    EF.Functions.Like(d.Specialisation, $"%{specialization}%"));
            }

            return await query.ToListAsync();

        }
    }
}
