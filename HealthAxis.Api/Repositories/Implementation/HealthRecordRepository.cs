using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Repositories.Implementation
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
        public HealthRecordRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<HealthRecord>> GetByPatientIdAsync(
            int patientId, CancellationToken ct = default) => await _context.HealthRecords.Include(h => h.Patient).Include(h => h.Doctor).Include(h => h.Appointment).Where(h => h.PatientId == patientId).OrderByDescending(h => h.VisitDate).ToListAsync(ct);

        public async Task<HealthRecord?> GetDetailsAsync(
            int healthRecordId, CancellationToken ct = default) => await _context.HealthRecords.Include(h => h.Patient).Include(h => h.Doctor).Include(h => h.Appointment).FirstOrDefaultAsync(h => h.HealthRecordId == healthRecordId, ct);

        public async Task<bool> ExistsForAppointmentAsync(
    int appointmentId,
    CancellationToken ct = default)
        {
            return await _context.HealthRecords.AnyAsync(
                record => record.AppointmentId == appointmentId,
                ct);
        }
    }
}