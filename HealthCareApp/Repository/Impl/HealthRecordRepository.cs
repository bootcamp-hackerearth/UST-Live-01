using HealthCareApp.Data;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Repository.Impl
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
        private readonly HealthAxisDbContext _context;

        public HealthRecordRepository(HealthAxisDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<HealthRecord>> GetByPatientIdAsync(int patientId, CancellationToken ct = default)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Patient)
                .Include(hr => hr.Doctor)
                .Include(hr => hr.Appointment)
                .Where(hr => hr.PatientId == patientId)
                .OrderByDescending(hr => hr.VisitDate)
                .ToListAsync(ct);
        }

        public async Task<List<HealthRecord>> GetByDoctorIdAsync(int doctorId, CancellationToken ct = default)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Patient)
                .Include(hr => hr.Doctor)
                .Include(hr => hr.Appointment)
                .Where(hr => hr.DoctorId == doctorId)
                .OrderByDescending(hr => hr.VisitDate)
                .ToListAsync(ct);
        }

        public async Task<List<HealthRecord>> GetByAppointmentIdAsync(int appointmentId, CancellationToken ct = default)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Patient)
                .Include(hr => hr.Doctor)
                .Include(hr => hr.Appointment)
                .Where(hr => hr.AppointmentId == appointmentId)
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsByAppointmentIdAsync(int appointmentId, CancellationToken ct = default)
        {
            return await _context.HealthRecords
                .AnyAsync(hr => hr.AppointmentId == appointmentId, ct);
        }
    }
}