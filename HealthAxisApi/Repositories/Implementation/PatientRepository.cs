using Microsoft.EntityFrameworkCore;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;

namespace HealthAxisCore_Api.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly HealthAppDbContext _context;

        public PatientRepository(HealthAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> SearchPatients(string? name, string? email)
        {
            return await _context.Patients
                .Where(p =>
                    (string.IsNullOrEmpty(name) || p.PatientName.Contains(name)) &&
                    (string.IsNullOrEmpty(email) || p.Email.Contains(email)))
                .ToListAsync();
        }

        public async Task<bool> IsDuplicate(string email, string phoneNumber)
        {
            return await _context.Patients
                .AnyAsync(p => p.Email == email || p.PhoneNumber == phoneNumber);
        }
    }
}