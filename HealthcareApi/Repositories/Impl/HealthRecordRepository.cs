using HealthcareApi.Data;
using HealthcareApi.Models;
using System.Collections.Generic;
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
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }

        public HealthRecord GetById(int healthRecordId)
        {
            return _context.HealthRecords
                .FirstOrDefault(r => r.HealthRecordId == healthRecordId);
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
            return _context.HealthRecords
                .Any(r => r.AppointmentId == appointmentId);
        }

        public HealthRecord Add(HealthRecord record)
        {
            _context.HealthRecords.Add(record);
            _context.SaveChanges();

            return record;
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

            return existingRecord;
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
    }
}