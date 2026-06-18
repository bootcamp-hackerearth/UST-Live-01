using HealthAxis.API.Data;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Repositories.Implementations
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly HealthAxisDbContext _context;

        public PatientRepository(HealthAxisDbContext context) : base(context)
        {
            _context = context;
        }

        // ✅ Get by Email
        public async Task<Patient?> GetByEmailAsync(string email)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.Email == email);
        }

        // ✅ Get by Phone
        public async Task<Patient?> GetByPhoneAsync(string phone)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.PhoneNumber == phone);
        }

        // ✅ Search by Name
        public async Task<IEnumerable<Patient>> SearchByNameAsync(string name)
        {
            return await _context.Patients
                .Where(p => p.FullName.Contains(name))
                .ToListAsync();
        }
    }
}
