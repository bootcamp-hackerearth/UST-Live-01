using HealthAppWebAPI.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAppWebAPI.Repositories.Impl
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HealthAppDbContext _context;

        public PatientRepository(HealthAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .ToListAsync();
        }

        public async Task<Patient> GetByIdAsync(int id)
        {
            return await _context.Patients
                .FindAsync(id);
        }

        public async Task AddAsync(Patient patient)
        {
            _context.Patients.Add(patient);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            _context.Entry(patient).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            string normalizedEmail =
                email.Trim().ToLower();

            return await _context.Patients
                .AnyAsync(p =>
                    p.Email.ToLower() == normalizedEmail);
        }

        public async Task<bool> EmailExistsForOtherPatientAsync(
            int patientId,
            string email)
        {
            string normalizedEmail =
                email.Trim().ToLower();

            return await _context.Patients
                .AnyAsync(p =>
                    p.PatientId != patientId &&
                    p.Email.ToLower() == normalizedEmail);
        }
    }
}