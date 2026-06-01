using System;
using System.Collections.Generic;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;
using HAP_Pod4_ConsoleApp_au.Services.Impl;
using Xunit;

namespace HealthAxisTests.ServiceTests
{
    public class HealthRecordServiceTests
    {
        private readonly HealthRepository _repository;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _repository = new HealthRepository();
            _service = new HealthRecordService(_repository);
        }

        [Fact]
        public void AddRecord_WhenValid_ShouldAddRecord()
        {
            HealthRecord record = new HealthRecord
            {
                RecordId = 1,
                Patient = new Patient
                {
                    PatientId = 1,
                    FullName = "Arun Kumar"
                },
                Doctor = new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr. Priya Sharma"
                },
                VisitDate = DateTime.Now,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest"
            };

            HealthRecord result = _service.AddRecord(record);

            Assert.NotNull(result);
            Assert.Equal("Fever", result.Diagnosis);
        }

        [Fact]
        public void AddRecord_WhenRecordIsNull_ShouldThrowException()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _service.AddRecord(null!);
            });

            // Assert against the parameter name since we removed the custom message
            Assert.Equal("record", ex.ParamName);
        }

        [Fact]
        public void AddRecord_WhenDiagnosisIsEmpty_ShouldThrowException()
        {
            HealthRecord record = new HealthRecord
            {
                RecordId = 1,
                Patient = new Patient { PatientId = 1 },
                Doctor = new Doctor { DoctorId = 1 },
                VisitDate = DateTime.Now,
                Diagnosis = string.Empty,
                Prescription = "Medicine",
                Notes = "Notes"
            };

            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                _service.AddRecord(record);
            });

            // Assert against the specific exception message and the corrected parameter name
            Assert.Contains("Diagnosis cannot be empty.", ex.Message);
            Assert.Equal("record", ex.ParamName);
        }

        [Fact]
        public void GetRecordsByPatient_ShouldReturnPatientRecords()
        {
            HealthRecord record1 = new HealthRecord
            {
                RecordId = 1,
                Patient = new Patient { PatientId = 1 },
                Doctor = new Doctor { DoctorId = 1 },
                VisitDate = DateTime.Now,
                Diagnosis = "Cold"
            };

            HealthRecord record2 = new HealthRecord
            {
                RecordId = 2,
                Patient = new Patient { PatientId = 2 },
                Doctor = new Doctor { DoctorId = 1 },
                VisitDate = DateTime.Now,
                Diagnosis = "Fever"
            };

            _service.AddRecord(record1);
            _service.AddRecord(record2);

            List<HealthRecord> result = _service.GetRecordsByPatient(1);

            Assert.Single(result);
            Assert.Equal(1, result[0].Patient!.PatientId);
        }

        [Fact]
        public void GetRecordsByDoctor_ShouldReturnDoctorRecords()
        {
            HealthRecord record1 = new HealthRecord
            {
                RecordId = 1,
                Patient = new Patient { PatientId = 1 },
                Doctor = new Doctor { DoctorId = 1 },
                VisitDate = DateTime.Now,
                Diagnosis = "Headache"
            };

            HealthRecord record2 = new HealthRecord
            {
                RecordId = 2,
                Patient = new Patient { PatientId = 2 },
                Doctor = new Doctor { DoctorId = 2 },
                VisitDate = DateTime.Now,
                Diagnosis = "Fever"
            };

            _service.AddRecord(record1);
            _service.AddRecord(record2);

            List<HealthRecord> result = _service.GetRecordsByDoctor(1);

            Assert.Single(result);
            Assert.Equal(1, result[0].Doctor!.DoctorId);
        }
    }
}