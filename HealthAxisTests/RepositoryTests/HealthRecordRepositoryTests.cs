using HealthAxis.Data;
using HealthAxis.Models;
using HealthAxis.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxisTests.RepositoryTests
{
    public class HealthRecordRepositoryTests
    {
        private readonly Database _db;
        private readonly HealthRecordRepository _repository;

        public HealthRecordRepositoryTests()
        {
            _db = new Database();
            _repository = new HealthRecordRepository(_db);
        }
        [Fact]
        public void AddRecord_ShouldAddHealthRecord()
        {
            var record = new HealthRecord
            {
                RecordId = 1,
                Patient = new Patient { PatientId = 1, PatientName = "Test Patient" },
                Doctor = new Doctor { DoctorId = 1, DoctorName = "Test Doctor" },
                Diagnosis = "Test Diagnosis",
                VisitedDate = DateTime.Now
            };
            _repository.AddRecord(record);
            var records = _repository.GetRecordsByPatient(1);
            Assert.Single(records);
            Assert.Equal("Test Diagnosis", records[0].Diagnosis);
        }
        [Fact]
        public void GetRecordsByPatient_ShouldReturnHealthRecordsForPatient()
        {
            var record1 = new HealthRecord
            {
                RecordId = 1,
                Patient = new Patient { PatientId = 1, PatientName = "Test Patient" },
                Doctor = new Doctor { DoctorId = 1, DoctorName = "Test Doctor" },
                Diagnosis = "Test Diagnosis 1",
                VisitedDate = DateTime.Now
            };
            var record2 = new HealthRecord
            {
                RecordId = 2,
                Patient = new Patient { PatientId = 1, PatientName = "Test Patient" },
                Doctor = new Doctor { DoctorId = 2, DoctorName = "Another Doctor" },
                Diagnosis = "Test Diagnosis 2",
                VisitedDate = DateTime.Now
            };
            _repository.AddRecord(record1);
            _repository.AddRecord(record2);
            var records = _repository.GetRecordsByPatient(1);
            Assert.Equal(2, records.Count);
        }
        [Fact]
        public void GetRecordsByPatient_ShouldReturnEmptyListIfNoRecords()
        {
            var records = _repository.GetRecordsByPatient(999);
            Assert.Empty(records);
        }
        [Fact]
        public void GetRecordsByDoctor_ShouldReturnHealthRecordsForDoctor()
        {
            var record1 = new HealthRecord
            {
                RecordId = 1,
                Patient = new Patient { PatientId = 1, PatientName = "Test Patient" },
                Doctor = new Doctor { DoctorId = 1, DoctorName = "Test Doctor" },
                Diagnosis = "Test Diagnosis 1",
                VisitedDate = DateTime.Now
            };
            var record2 = new HealthRecord
            {
                RecordId = 2,
                Patient = new Patient { PatientId = 2, PatientName = "Another Patient" },
                Doctor = new Doctor { DoctorId = 1, DoctorName = "Test Doctor" },
                Diagnosis = "Test Diagnosis 2",
                VisitedDate = DateTime.Now
            };
            _repository.AddRecord(record1);
            _repository.AddRecord(record2);
            var records = _repository.GetRecordsByDoctor(1);
            Assert.Equal(2, records.Count);
        }
    }
}
