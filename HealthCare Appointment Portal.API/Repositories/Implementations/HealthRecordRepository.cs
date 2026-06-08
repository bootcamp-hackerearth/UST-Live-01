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
        : Repository<HealthRecord>,
          IHealthRecordRepository
    {
        public HealthRecordRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<bool>
            RecordExistsAsync(
                int appointmentId)
        {
            return await _dbSet.AnyAsync(
                hr => hr.AppointmentId
                    == appointmentId);
        }

        public async Task<
            IEnumerable<HealthRecord>>
            GetRecordsByPatientAsync(
                int patientId)
        {
            return await _dbSet
                .Include(hr => hr.Patient)
                .Include(hr => hr.Doctor)
                .Include(hr => hr.Appointment)
                .Where(hr =>
                    hr.PatientId
                    == patientId)
                .OrderByDescending(
                    hr => hr.VisitDate)
                .ToListAsync();
        }

        public async Task<
            IEnumerable<HealthRecord>>
            GetRecordsByDoctorAsync(
                int doctorId)
        {
            return await _dbSet
                .Include(hr => hr.Patient)
                .Include(hr => hr.Doctor)
                .Include(hr => hr.Appointment)
                .Where(hr =>
                    hr.DoctorId
                    == doctorId)
                .OrderByDescending(
                    hr => hr.VisitDate)
                .ToListAsync();
        }

        public async Task<
            IEnumerable<int>>
            GetRecordedAppointmentIdsAsync()
        {
            return await _dbSet
                .Select(hr =>
                    hr.AppointmentId)
                .ToListAsync();
        }

        public async Task<HealthRecord>
            GetByAppointmentIdAsync(
                int appointmentId)
        {
            return await _dbSet
                .Include(hr => hr.Patient)
                .Include(hr => hr.Doctor)
                .Include(hr => hr.Appointment)
                .FirstOrDefaultAsync(hr =>
                    hr.AppointmentId
                    == appointmentId);
        }

        public async Task<
            IEnumerable<HealthRecord>>
            GetAllRecordsWithDetailsAsync()
        {
            return await _dbSet
                .Include(hr => hr.Patient)
                .Include(hr => hr.Doctor)
                .Include(hr => hr.Appointment)
                .OrderByDescending(
                    hr => hr.VisitDate)
                .ToListAsync();
        }
    }
}