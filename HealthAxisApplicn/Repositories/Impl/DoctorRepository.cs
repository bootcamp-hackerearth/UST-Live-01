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
        public async Task<List<Doctor>> GetAvailableDoctorsAsync(CancellationToken ct = default)
        {
            var availableDoctors = await _context.Set<Doctor>().Where(d => d.IsActive == true).ToListAsync(ct);
            return availableDoctors;
        }

        public async Task<List<Doctor>> SearchBySpecialisationAsync(string specialisation, CancellationToken ct = default)
        {
            var existing = await _context.Set<Doctor>().Where(d => d.Specialisation == specialisation).ToListAsync(ct);
            return existing;
        }

        public async Task<List<Doctor>> SearchDoctorByNameAsync(string name, CancellationToken ct = default)
        {
            var existing = await _context.Set<Doctor>().Where(d => d.DoctorName == name).ToListAsync(ct);
            return existing;
        }
    }
}
