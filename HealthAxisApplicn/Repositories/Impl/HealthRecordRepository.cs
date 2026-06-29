using HealthAxisApplicn.Data;
using HealthAxisApplicn.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisApplicn.Repositories.Impl
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
        private readonly AppDbContext _context;
        public HealthRecordRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<HealthRecord>> GetRecordsByPatientIdAsync(int patientId, CancellationToken ct = default)
        {
            var recordsByPatientID = await _context.Set<HealthRecord>().Where(h => h.PatientId == patientId).ToListAsync(ct);
            return recordsByPatientID;
        }

        public async Task<List<HealthRecord>> GetRecordsByDoctorIdAsync(int doctorId, CancellationToken ct = default)
        {
            var recordsByDoctorID = await _context.Set<HealthRecord>().Where(h => h.DoctorId == doctorId).ToListAsync(ct);
            return recordsByDoctorID;
        }

        public async Task<List<HealthRecord>> GetRecordsByDoctorNameAsync(string doctorName, CancellationToken ct = default)
        {
            return await _context.Set<HealthRecord>()
                .Include(h => h.Doctor)
                .Where(h => h.Doctor.DoctorName == doctorName)
                .ToListAsync(ct);
        }

        public async Task<List<HealthRecord>> GetRecordsByPatientNameAsync(string patientName, CancellationToken ct = default)
        {
            return await _context.Set<HealthRecord>()
                .Include(h => h.Patient)
                .Where(h => h.Patient.PatientName == patientName).ToListAsync(ct);
        }

        public async Task<HealthRecord?> GetByAppointmentIdAsync(int appointmentId, CancellationToken ct = default)
        {
            return await _context.Set<HealthRecord>()
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.AppointmentId == appointmentId, ct);
        }

        public async Task<bool> ExistsForAppointmentAsync(int appointmentId, CancellationToken ct = default)
        {
            return await _context.Set<HealthRecord>()
                .AnyAsync(h => h.AppointmentId == appointmentId, ct);
        }
    }
}
