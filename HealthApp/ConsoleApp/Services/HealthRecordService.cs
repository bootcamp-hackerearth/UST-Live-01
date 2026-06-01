using HealthApp.ConsoleApp.Services;
using HealthApp.ConsoleApp.Repositories;
using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Exceptions;
namespace HealthApp.ConsoleApp.Services
{
    public class HealthRecordService : IHealthRecordService
    {
        //Injecting HealthRecord, Doctor and Patient dependencies
        private readonly IHealthRecordRepository _healthRecordRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public HealthRecordService(IHealthRecordRepository healthRecordRepository,
                                    IDoctorRepository doctorRepository,
                                    IPatientRepository patientRepository)
        {
            _healthRecordRepository = healthRecordRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        // Add a health record to the database
        public string AddHealthRecord(HealthRecord record)
        {
            List<HealthRecord> records = _healthRecordRepository.GetAllRecords();
            record.RecordId = RecordIdGenerator(records);

            return _healthRecordRepository.AddHealthRecord(record);
        }

        // Get a health record by patient id and return in descending order of visit date
        public List<HealthRecord> GetByPatientIdOrderByVisitDateDesc(int id)
        {
            var patient = _patientRepository.GetPatientById(id);

            if (patient == null)
            {
                throw new PatientNotFoundException("Patient of this id has not been found.");
            }

            var records = _healthRecordRepository
                .GetByPatientIdOrderByVisitDateDesc(id);

            if (records == null || records.Count == 0)
            {
                throw new HealthRecordNotFoundException("No health records found for this patient ID.");
            }

            return records;
        }

        // Get a health record by doctor id and return in descending order of visit date
        public List<HealthRecord> GetByDoctorIdOrderByVisitDateDesc(int id)
        {
            var doctor = _doctorRepository.GetDoctorById(id);

            if (doctor == null)
            {
                throw new DoctorNotFoundException("Doctor of this id has not been found.");
            }

            var records = _healthRecordRepository
                .GetByDoctorIdOrderByVisitDateDesc(id);

            if (records == null || records.Count == 0)
            {
                throw new HealthRecordNotFoundException("No health records found for this doctor ID.");
            }

            return records;
        }

        // Update health record with new details
        public HealthRecord UpdateHealthRecord(HealthRecord record)
        {
            HealthRecord? existingHealthRecord = GetRecordById(record.RecordId);

            if (existingHealthRecord is null)
            {
                throw new HealthRecordNotFoundException($"Health Record of ID {record.RecordId} does not exist");
            }
            return _healthRecordRepository.UpdateHealthRecord(existingHealthRecord, record);
        }

        // Get a health record by its id
        public HealthRecord GetRecordById(int recordId)
        {
            HealthRecord? record = _healthRecordRepository.GetRecordById(recordId);
            if (record is null)
            {
                throw new HealthRecordNotFoundException($"Health Record of ID {recordId} does not exist");
            }
            return record;
        }

        // Assign health record id based on latest record id
        public static int RecordIdGenerator(List<HealthRecord> records)
        {
            return records.Count > 0
                ? records.Max(r => r.RecordId) + 1
                : 101;
        }
    }
}