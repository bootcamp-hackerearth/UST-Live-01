using HealthApp.Api.Data;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repositories.Impl
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly HealthAppDbContext _context;
        public PatientRepository(HealthAppDbContext context) : base(context)
        {
            _context=context;
        }

        public async Task<IEnumerable<Patient>> GetPatientsAsync(string? Name, string? email, CancellationToken ct = default)
        {
            var query = _context.Set<Patient>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(Name))
            {
                query = query.Where(p => p.FullName != null && p.FullName.Contains(Name));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(p => p.Email != null && p.Email.Contains(email));
            }

            return await query.ToListAsync(ct);
        }

        public async Task<bool> IsDuplicatePatient(string name, DateTime dob, string email, CancellationToken ct = default)
        {
            var dobOnly = DateOnly.FromDateTime(dob);
            return await _context.Set<Patient>()
                    .AnyAsync(p =>
                        p.FullName == name &&
                        p.DateOfBirth == dobOnly &&
                        p.Email == email, ct);
        }
    }
}
