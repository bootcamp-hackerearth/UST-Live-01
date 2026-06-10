using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Repositories
{
    public class HealthRecordRepository
        : IHealthRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public HealthRecordRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HealthRecord>>
            GetAllAsync()
        {
            return await _context.HealthRecords
                .Include(r => r.Appointment)
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .ToListAsync();
        }

        public async Task<HealthRecord>
            GetByIdAsync(
                int recordId)
        {
            return await _context.HealthRecords
                .FirstOrDefaultAsync(r =>
                    r.RecordId ==
                    recordId);
        }

        public async Task<IEnumerable<HealthRecord>>
            GetByPatientAsync(
                int patientId)
        {
            return await _context.HealthRecords
                .Where(r =>
                    r.PatientId ==
                    patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<HealthRecord>>
            GetByDoctorAsync(
                int doctorId)
        {
            return await _context.HealthRecords
                .Where(r =>
                    r.DoctorId ==
                    doctorId)
                .ToListAsync();
        }

        public async Task<bool>
            ExistsByAppointmentAsync(
                int appointmentId)
        {
            return await _context.HealthRecords
                .AnyAsync(r =>
                    r.AppointmentId ==
                    appointmentId);
        }

        public async Task
            AddAsync(
                HealthRecord record)
        {
            _context.HealthRecords
                .Add(record);

            await _context
                .SaveChangesAsync();
        }

        public async Task
            UpdateAsync(
                HealthRecord record)
        {
            _context.Entry(record)
                .State =
                EntityState.Modified;

            await _context
                .SaveChangesAsync();
        }

        public async Task
            DeleteAsync(
                int recordId)
        {
            var record =
                await GetByIdAsync(
                    recordId);

            if (record != null)
            {
                _context.HealthRecords
                    .Remove(record);

                await _context
                    .SaveChangesAsync();
            }
        }
    }
}