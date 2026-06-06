using System.Collections.Generic;
using System.Linq;
using HealthcareMvcApp.Data;
using HealthcareMvcApp.Models;

namespace HealthcareMvcApp.Repositories.Implementations
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly HealthcareDbContext _context;

        public HealthRecordRepository(HealthcareDbContext context)
        {
            _context = context;
        }

        public void Add(HealthRecord record)
        {
            _context.HealthRecords.Add(record);
            _context.SaveChanges();
        }

        public HealthRecord GetById(int recordId)
        {
            return _context.HealthRecords
                .FirstOrDefault(r => r.HealthRecordId == recordId);
        }

        public List<HealthRecord> GetAll()
        {
            return _context.HealthRecords
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }

        public List<HealthRecord> GetByPatientId(int patientId)
        {
            return _context.HealthRecords
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }

        public List<HealthRecord> GetByDoctorId(int doctorId)
        {
            return _context.HealthRecords
                .Where(r => r.DoctorId == doctorId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }

        public List<HealthRecord> GetByAppointmentId(int appointmentId)
        {
            return _context.HealthRecords
                .Where(r => r.AppointmentId == appointmentId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }

        public bool ExistsByAppointmentId(int appointmentId)
        {
            return _context.HealthRecords.Any(r => r.AppointmentId == appointmentId);
        }

        public bool Update(HealthRecord record)
        {
            HealthRecord existingRecord = GetById(record.HealthRecordId);

            if (existingRecord == null)
            {
                return false;
            }

            existingRecord.PatientId = record.PatientId;
            existingRecord.DoctorId = record.DoctorId;
            existingRecord.AppointmentId = record.AppointmentId;
            existingRecord.VisitDate = record.VisitDate.Date;
            existingRecord.Diagnosis = record.Diagnosis;
            existingRecord.Prescription = record.Prescription;
            existingRecord.Notes = record.Notes;

            _context.SaveChanges();

            return true;
        }

        public bool Delete(int recordId)
        {
            HealthRecord existingRecord = GetById(recordId);

            if (existingRecord == null)
            {
                return false;
            }

            _context.HealthRecords.Remove(existingRecord);
            _context.SaveChanges();

            return true;
        }
    }
}