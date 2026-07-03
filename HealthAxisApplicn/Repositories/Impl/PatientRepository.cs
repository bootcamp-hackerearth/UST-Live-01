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
            var cleanEmail = email.Trim().ToLower();

            return await _context.Patients
                .FirstOrDefaultAsync(p => p.Email.ToLower() == cleanEmail, ct);

        }

        public async Task<List<Patient>> SearchByNameAsync(string name, CancellationToken ct = default)
        {
            var cleanName = name.Trim().ToLower();

            return await _context.Patients
                .Where(p => p.PatientName.ToLower().Contains(cleanName))
                .ToListAsync(ct);
        }

        public async Task<Patient?> SearchByPhoneNumberAsync(string phoneNumber, CancellationToken ct = default)
        {
            var existing = await _context.Set<Patient>().FirstOrDefaultAsync(p => p.PhoneNo.Contains(phoneNumber), ct);
            return existing;    
        }

        public async Task<List<Patient>> SearchAsync(string? name, string? phone, CancellationToken ct = default)
        {
            var query = _context.Patients.AsQueryable();

            // ✅ Name filter
            if (!string.IsNullOrWhiteSpace(name))
            {
                var lowerName = name.Trim().ToLower();

                query = query.Where(p =>
                    p.PatientName.ToLower().Contains(lowerName));
            }

            // ✅ Phone filter
            if (!string.IsNullOrWhiteSpace(phone))
            {
                var cleanPhone = phone.Trim();

                query = query.Where(p =>
                    p.PhoneNo.Contains(cleanPhone));
            }

            return await query.ToListAsync(ct);
        }

        public async Task<Patient?> GetByUserIdAsync(string userId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

    }
}
