using HealthAxisApplicn.Data;
using HealthAxisApplicn.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisApplicn.Repositories.Impl
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly AppDbContext _context;
        public PatientRepository(AppDbContext context): base(context)
        {
            _context = context;
        }
        public async Task<Patient?> SearchByEmailAsync(string email, CancellationToken ct = default)
        {
            var existing = await _context.Set<Patient>().FirstOrDefaultAsync(p => p.Email == email, ct);
            return existing;
        }

        public async Task<Patient?> SearchByPatientNameAsync(string name, CancellationToken ct = default)
        {
            var existing = await _context.Set<Patient>().FirstOrDefaultAsync(p => p.PatientName == name, ct);
            return existing;
        }

        public async Task<Patient?> SearchByPhoneNumberAsync(string phoneNumber, CancellationToken ct = default)
        {
            var existing = await _context.Set<Patient>().FirstOrDefaultAsync(p => p.PhoneNo == phoneNumber, ct);
            return existing;
        }
    }
}
