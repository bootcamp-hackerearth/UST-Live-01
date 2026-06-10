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

        public async Task<HealthRecord>
            GetByIdAsync(
                int id)
        {
            return await _context.HealthRecords
                .FindAsync(id);
        }

        public async Task<IEnumerable<HealthRecord>>
            GetAllAsync()
        {
            return await _context.HealthRecords
                .ToListAsync();
        }

        public async Task AddAsync(
            HealthRecord healthRecord)
        {
            _context.HealthRecords
                .Add(healthRecord);

            await _context
                .SaveChangesAsync();
        }

        public async Task UpdateAsync(
            HealthRecord healthRecord)
        {
            _context.Entry(healthRecord)
                .State = EntityState.Modified;

            await _context
                .SaveChangesAsync();
        }

        public async Task DeleteAsync(
            int id)
        {
            HealthRecord healthRecord =
                await _context.HealthRecords
                    .FindAsync(id);

            if (healthRecord != null)
            {
                _context.HealthRecords
                    .Remove(healthRecord);

                await _context
                    .SaveChangesAsync();
            }
        }

        public async Task<bool>
            RecordExistsAsync(
                int appointmentId)
        {
            return await _context.HealthRecords
                .AnyAsync(hr =>
                    hr.AppointmentId ==
                    appointmentId);
        }

        public async Task<IEnumerable<HealthRecord>>
            GetRecordsByPatientAsync(
                int patientId)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Patient)
                .Include(hr => hr.Doctor)
                .Include(hr => hr.Appointment)
                .Where(hr =>
                    hr.PatientId ==
                    patientId)
                .OrderByDescending(hr =>
                    hr.VisitDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<HealthRecord>>
            GetRecordsByDoctorAsync(
                int doctorId)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Patient)
                .Include(hr => hr.Doctor)
                .Include(hr => hr.Appointment)
                .Where(hr =>
                    hr.DoctorId ==
                    doctorId)
                .OrderByDescending(hr =>
                    hr.VisitDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<int>>
            GetRecordedAppointmentIdsAsync()
        {
            return await _context.HealthRecords
                .Select(hr =>
                    hr.AppointmentId)
                .ToListAsync();
        }

        public async Task<HealthRecord>
            GetByAppointmentIdAsync(
                int appointmentId)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Patient)
                .Include(hr => hr.Doctor)
                .Include(hr => hr.Appointment)
                .FirstOrDefaultAsync(hr =>
                    hr.AppointmentId ==
                    appointmentId);
        }

        public async Task<IEnumerable<HealthRecord>>
            GetAllRecordsWithDetailsAsync()
        {
            return await _context.HealthRecords
                .Include(hr => hr.Patient)
                .Include(hr => hr.Doctor)
                .Include(hr => hr.Appointment)
                .OrderByDescending(hr =>
                    hr.VisitDate)
                .ToListAsync();
        }
    }
}