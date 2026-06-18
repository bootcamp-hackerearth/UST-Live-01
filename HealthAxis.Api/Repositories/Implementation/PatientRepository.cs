using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace HealthAxisCore_Api.Repositories.Implementation
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(AppDbContext context) : base(context) { }
        public async Task<Patient?> GetByEmailAsync(string email, CancellationToken ct = default) => await _context.Patients.FirstOrDefaultAsync(p => p.Email == email, ct);
        public async Task<List<HealthRecord>> GetHealthRecordsAsync(int patientId, CancellationToken ct = default) => await _context.HealthRecords.Include(h => h.Patient).Include(h => h.Doctor).Include(h => h.Appointment).Where(h => h.PatientId == patientId).OrderByDescending(h => h.VisitDate).ToListAsync(ct);
    }
}
