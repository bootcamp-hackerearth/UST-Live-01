using HealthcareApi.Data;
using HealthcareApi.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HealthcareApi.Repositories.Implementations
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly HealthcareDbContext _context;

        public HealthRecordRepository(HealthcareDbContext context)
        {
            _context = context;
        }

        public List<HealthRecord> GetAll()
        {
            return _context.HealthRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }

        public HealthRecord GetById(int healthRecordId)
        {
            return _context.HealthRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .FirstOrDefault(r => r.HealthRecordId == healthRecordId);
        }

        public List<HealthRecord> GetByPatientId(int patientId)
        {
            return _context.HealthRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }

        public List<HealthRecord> GetByDoctorId(int doctorId)
        {
            return _context.HealthRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .Where(r => r.DoctorId == doctorId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }

        public List<HealthRecord> GetByAppointmentId(int appointmentId)
        {
            return _context.HealthRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .Where(r => r.AppointmentId == appointmentId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }

        public bool ExistsByAppointmentId(int appointmentId)
        {
            return _context.HealthRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .Any(r => r.AppointmentId == appointmentId);
        }

        public List<HealthRecord> SearchHealthRecords(string query)
        {
            IQueryable<HealthRecord> records = _context.HealthRecords
                .Include(r => r.Doctor)
                .Include(r => r.Patient);
            
            records = ApplyHealthRecordSearch(records, query);

            return records
                .OrderBy(r => r.HealthRecordId)
                .ToList();
        }

        public List<HealthRecord> SearchHealthRecordsByPatientId(
            int patientId,
            string query)
        {
            IQueryable<HealthRecord> records = _context.HealthRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .Where(r => r.PatientId == patientId);

            records = ApplyHealthRecordSearch(records, query);

            return records
                .OrderBy(r => r.HealthRecordId)
                .ToList();
        }

        public List<HealthRecord> SearchHealthRecordsByDoctorId(
            int doctorId,
            string query)
        {
            IQueryable<HealthRecord> records = _context.HealthRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .Where(r => r.DoctorId == doctorId);

            records = ApplyHealthRecordSearch(records, query);

            return records
                .OrderBy(r => r.HealthRecordId)
                .ToList();
        }


        public HealthRecord Add(HealthRecord record)
        {
            _context.HealthRecords.Add(record);
            _context.SaveChanges();

            return GetById(record.HealthRecordId);
        }

        public HealthRecord Update(int healthRecordId, HealthRecord record)
        {
            HealthRecord existingRecord = GetById(healthRecordId);

            if (existingRecord == null)
            {
                return null;
            }

            existingRecord.PatientId = record.PatientId;
            existingRecord.DoctorId = record.DoctorId;
            existingRecord.AppointmentId = record.AppointmentId;
            existingRecord.VisitDate = record.VisitDate.Date;
            existingRecord.Diagnosis = record.Diagnosis;
            existingRecord.Prescription = record.Prescription;
            existingRecord.Notes = record.Notes;

            _context.SaveChanges();

            return GetById(healthRecordId);
        }

        public HealthRecord Delete(int healthRecordId)
        {
            HealthRecord existingRecord = GetById(healthRecordId);

            if (existingRecord == null)
            {
                return null;
            }

            _context.HealthRecords.Remove(existingRecord);
            _context.SaveChanges();

            return existingRecord;
        }
        private IQueryable<HealthRecord> ApplyHealthRecordSearch(IQueryable<HealthRecord> records, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return records;
            }

            string searchTerm = query.Trim().ToLower();

            return records.Where(r =>
                (r.Patient != null &&
                    r.Patient.FullName.ToLower().Contains(searchTerm)) ||
                (r.Doctor != null &&
                    r.Doctor.FullName.ToLower().Contains(searchTerm)));
        }
    }
}
