using HealthAxis.API.Data;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Repositories.Implementations
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
       

        public HealthRecordRepository(HealthAxisDbContext context) : base(context)
        {
            
        }

        // ✅ Get records by PatientId
        public async Task<IEnumerable<HealthRecord>> GetByPatientIdAsync(int patientId)
        {
            return await _context.HealthRecords
                .Where(r => r.PatientId == patientId)
                .ToListAsync();
        }

        // ✅ Get records by DoctorId
        public async Task<IEnumerable<HealthRecord>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.HealthRecords
                .Where(r => r.DoctorId == doctorId)
                .ToListAsync();
        }

        // ✅ Get records by AppointmentId
        public async Task<IEnumerable<HealthRecord>> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.HealthRecords
                .Where(r => r.AppointmentId == appointmentId)
                .ToListAsync();
        }
    }
}