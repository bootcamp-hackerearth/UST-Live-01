using System;
using System.Collections.Generic;
using Xunit;
using HealthApp.ConsoleApp.Repositories;
using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Databases;

namespace HealthApp.Tests.Repositories
{
    public class HealthRecordRepositoryTests
    {
        private readonly HealthRecordDB _healthRecordDb;
        private readonly HealthRecordRepository _repository;

        public HealthRecordRepositoryTests()
        {
            _healthRecordDb = new HealthRecordDB
            {
                Records = new List<HealthRecord>()
            };

            _repository = new HealthRecordRepository(_healthRecordDb);
        }
        private static Patient GetSamplePatient(int id)
        {
            return new Patient
            {
                PatientId = id,
                FullName = "Patient " + id,
                PhoneNumber = "9999999999",
                Email = "patient@test.com",
                InsuranceId = "sadf23423"
            };
        }

        private static Doctor GetSampleDoctor(int id)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = "Doctor " + id,
                Specialisation = "General"
            };
        }

        // AddHealthRecord
        [Fact]
        public void AddHealthRecord_ShouldAddRecord()
        {
            var record = new HealthRecord
            {
                RecordId = 1,
                VisitDate = DateTime.Now,
                Patient = GetSamplePatient(1),
                Doctor = GetSampleDoctor(1),
                Diagnosis = "Flu",
                Prescription = "Paracetamol",
                DoctorNotes = "Rest well"
            };

            var result = _repository.AddHealthRecord(record);

            Assert.Single(_healthRecordDb.Records);
            Assert.Contains("added successfully", result);
        }

        // GetAllRecords
        [Fact]
        public void GetAllRecords_ShouldReturnAllRecords()
        {
            _healthRecordDb.Records.Add(new HealthRecord
            {
                RecordId = 1,
                VisitDate = DateTime.Now,
                Patient = GetSamplePatient(1),
                Doctor = GetSampleDoctor(1),
                Diagnosis = "Cold",
                Prescription = "Tablet",
                DoctorNotes = "OK"
            });

            _healthRecordDb.Records.Add(new HealthRecord
            {
                RecordId = 2,
                VisitDate = DateTime.Now,
                Patient = GetSamplePatient(2),
                Doctor = GetSampleDoctor(2),
                Diagnosis = "Fever",
                Prescription = "Medicine",
                DoctorNotes = "Observe"
            });

            var result = _repository.GetAllRecords();

            Assert.Equal(2, result.Count);
        }

        // GetByPatientIdOrderByVisitDateDesc
        [Fact]
        public void GetByPatientIdOrderByVisitDateDesc_ShouldReturnSortedRecords()
        {
            var patient = GetSamplePatient(1);

            _healthRecordDb.Records.Add(new HealthRecord
            {
                RecordId = 1,
                Patient = patient,
                Doctor = GetSampleDoctor(1),
                VisitDate = DateTime.Now.AddDays(-1),
                Diagnosis = "Cold",
                Prescription = "Tab",
                DoctorNotes = "Note1"
            });

            _healthRecordDb.Records.Add(new HealthRecord
            {
                RecordId = 2,
                Patient = patient,
                Doctor = GetSampleDoctor(2),
                VisitDate = DateTime.Now,
                Diagnosis = "Fever",
                Prescription = "Med",
                DoctorNotes = "Note2"
            });

            var result = _repository.GetByPatientIdOrderByVisitDateDesc(1);

            Assert.Equal(2, result.Count);
            Assert.True(result[0].VisitDate >= result[1].VisitDate);
        }

        // GetByDoctorIdOrderByVisitDateDesc
        [Fact]
        public void GetByDoctorIdOrderByVisitDateDesc_ShouldReturnSortedRecords()
        {
            var doctor = GetSampleDoctor(1);

            _healthRecordDb.Records.Add(new HealthRecord
            {
                RecordId = 1,
                Doctor = doctor,
                Patient = GetSamplePatient(1),
                VisitDate = DateTime.Now.AddDays(-2),
                Diagnosis = "Injury",
                Prescription = "Rest",
                DoctorNotes = "Note1"
            });

            _healthRecordDb.Records.Add(new HealthRecord
            {
                RecordId = 2,
                Doctor = doctor,
                Patient = GetSamplePatient(2),
                VisitDate = DateTime.Now,
                Diagnosis = "Pain",
                Prescription = "Tablet",
                DoctorNotes = "Note2"
            });

            var result = _repository.GetByDoctorIdOrderByVisitDateDesc(1);

            Assert.Equal(2, result.Count);
            Assert.True(result[0].VisitDate >= result[1].VisitDate);
        }

        // GetRecordById
        [Fact]
        public void GetRecordById_ShouldReturnRecord()
        {
            _healthRecordDb.Records.Add(new HealthRecord
            {
                RecordId = 1,
                VisitDate = DateTime.Now,
                Patient = GetSamplePatient(1),
                Doctor = GetSampleDoctor(1),
                Diagnosis = "Cold",
                Prescription = "Tablet",
                DoctorNotes = "OK"
            });

            var result = _repository.GetRecordById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.RecordId);
        }

        // UpdateHealthRecord
        [Fact]
        public void UpdateHealthRecord_ShouldUpdateFields()
        {
            var existing = new HealthRecord
            {
                RecordId = 1,
                VisitDate = DateTime.Now.AddDays(-1),
                Patient = GetSamplePatient(1),
                Doctor = GetSampleDoctor(1),
                Diagnosis = "Old",
                Prescription = "OldMed",
                DoctorNotes = "OldNote"
            };

            var updated = new HealthRecord
            {
                RecordId = 2,
                VisitDate = DateTime.Now,
                Patient = GetSamplePatient(2),
                Doctor = GetSampleDoctor(2),
                Diagnosis = "New",
                Prescription = "NewMed",
                DoctorNotes = "NewNote"
            };

            var result = _repository.UpdateHealthRecord(existing, updated);

            Assert.Equal(2, result.RecordId);
            Assert.Equal("New", result.Diagnosis);
            Assert.Equal("NewMed", result.Prescription);
            Assert.Equal("NewNote", result.DoctorNotes);
        }
    }
}
