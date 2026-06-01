using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Exceptions;
using HealthApp.ConsoleApp.Databases;
using HealthApp.ConsoleApp.Interfaces;

namespace HealthApp.ConsoleApp.Repositories
{

    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly HealthRecordDB _healthRecordDb;

        public HealthRecordRepository(HealthRecordDB healthRecordDB)
        {
            _healthRecordDb = healthRecordDB;
        }

        public string AddHealthRecord(HealthRecord record)
        {
            _healthRecordDb.Records.Add(record);
            return $"Record ID {record.RecordId} added successfully!";
        }

        public List<HealthRecord> GetAllRecords()
        {
            return _healthRecordDb.Records.ToList();
        }

        public List<HealthRecord> GetByPatientIdOrderByVisitDateDesc(int id)
        {
            return _healthRecordDb.Records
                    .Where(r => r.Patient != null && r.Patient.PatientId == id)
                    .OrderByDescending(r => r.VisitDate)
                    .ToList();
        }

        public List<HealthRecord> GetByDoctorIdOrderByVisitDateDesc(int id)
        {
            return _healthRecordDb.Records
                    .Where(r => r.Doctor != null && r.Doctor.DoctorId == id)
                    .OrderByDescending(r => r.VisitDate)
                    .ToList();
        }

        public HealthRecord? GetRecordById(int id)
        {
            return _healthRecordDb.Records.FirstOrDefault(r => r.RecordId == id);
        }

        public HealthRecord UpdateHealthRecord(HealthRecord existingHealthRecord, HealthRecord record)
        {
            existingHealthRecord.RecordId = record.RecordId;
            existingHealthRecord.Patient = record.Patient;
            existingHealthRecord.Doctor = record.Doctor;
            existingHealthRecord.VisitDate = record.VisitDate;
            existingHealthRecord.Diagnosis = record.Diagnosis;
            existingHealthRecord.Prescription = record.Prescription;
            existingHealthRecord.DoctorNotes = record.DoctorNotes;

            return existingHealthRecord;
        }
    }
}