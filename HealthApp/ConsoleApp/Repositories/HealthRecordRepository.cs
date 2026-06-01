using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Exceptions;
using HealthApp.ConsoleApp.Databases;
using HealthApp.ConsoleApp.Interfaces;

namespace HealthApp.ConsoleApp.Repositories
{
    // Repository class to manage health records in the healthcare system
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly HealthRecordDb _healthRecordDb;

        public HealthRecordRepository(HealthRecordDb healthRecordDb)
        {
            _healthRecordDb = healthRecordDb;
        }

        // Method to add a new health record to the database
        public string AddHealthRecord(HealthRecord record)
        {
            _healthRecordDb.Records.Add(record);
            return $"Record ID {record.RecordId} added successfully!";
        }
        // Method to Get All health records from the database
        public List<HealthRecord> GetAllRecords()
        {
            return _healthRecordDb.Records.ToList();
        }
        // Method to get health records by patient ID from the database, ordered by visit date in descending order
        public List<HealthRecord> GetByPatientIdOrderByVisitDateDesc(int id)
        {
            return _healthRecordDb.Records
                    .Where(r => r.Patient != null && r.Patient.PatientId == id)
                    .OrderByDescending(r => r.VisitDate)
                    .ToList();
        }
        // Method to get health records by doctor ID from the database, ordered by visit date in descending order

        public List<HealthRecord> GetByDoctorIdOrderByVisitDateDesc(int id)
        {
            return _healthRecordDb.Records
                    .Where(r => r.Doctor != null && r.Doctor.DoctorId == id)
                    .OrderByDescending(r => r.VisitDate)
                    .ToList();
        }
        // Method to get a health record by ID from the database
        public HealthRecord? GetRecordById(int id)
        {
            return _healthRecordDb.Records.FirstOrDefault(r => r.RecordId == id);
        }
        // Method to update an existing health record in the database
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