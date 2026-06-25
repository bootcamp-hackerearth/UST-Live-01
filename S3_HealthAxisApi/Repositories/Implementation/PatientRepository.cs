using HealthAxis.API.Data;
using Microsoft.EntityFrameworkCore;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using System.Diagnostics.CodeAnalysis;

namespace S3_HealthAxisApi.Repository.Implementation
{
    [ExcludeFromCodeCoverage]
    public class PatientRepository : IPatientRepository
    {
        private readonly HealthAxisDbContext _context;

        public PatientRepository(HealthAxisDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .OrderBy(p => p.PatientId)
                .ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == id);
        }

        public async Task<IEnumerable<Patient>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<Patient>();

            name = name.Trim();

            return await _context.Patients
                .Where(p => p.IsActive && p.FullName.Contains(name))
                .OrderBy(p => p.FullName)
                .ToListAsync();
        }

        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
        }

        public Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Patients.AnyAsync(p => p.PatientId == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}