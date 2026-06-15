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

        public async Task<IEnumerable<Patient>> GetPatientsAsync(string? name, string? email, CancellationToken ct = default)
        {
            var query = _context.Set<Patient>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => p.FullName.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(p => p.Email.Contains(email));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> IsDuplicatePatient(string name, DateTime dob, string email, CancellationToken ct = default)
        {
            var dobOnly = DateOnly.FromDateTime(dob);
            return await _context.Set<Patient>()
                    .AnyAsync(p =>
                        p.FullName == name &&
                        p.DateOfBirth == dobOnly &&
                        p.Email == email);

        }
    }
}
