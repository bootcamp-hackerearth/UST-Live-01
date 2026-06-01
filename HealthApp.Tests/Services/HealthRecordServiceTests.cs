using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using HealthApp.ConsoleApp.Services;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Exceptions;

namespace HealthApp.Tests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _healthRepoMock;
        private readonly Mock<IDoctorRepository> _doctorRepoMock;
        private readonly Mock<IPatientRepository> _patientRepoMock;

        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _healthRepoMock = new Mock<IHealthRecordRepository>();
            _doctorRepoMock = new Mock<IDoctorRepository>();
            _patientRepoMock = new Mock<IPatientRepository>();

            _service = new HealthRecordService(
                _healthRepoMock.Object,
                _doctorRepoMock.Object,
                _patientRepoMock.Object
            );
        }

        private static Patient GetSamplePatient(int id)
        {
            return new Patient
            {
                PatientId = id,
                FullName = "Patient " + id,
                PhoneNumber = "9999999999",
                Email = "patient@test.com",
                InsuranceId = "23sdfs"
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

        private static HealthRecord GetSampleRecord(int id)
        {
            return new HealthRecord
            {
                RecordId = id,
                VisitDate = DateTime.Now,
                Patient = GetSamplePatient(1),
                Doctor = GetSampleDoctor(1),
                Diagnosis = "Flu",
                Prescription = "Medicine",
                DoctorNotes = "Take rest"
            };
        }

        // AddHealthRecord
        [Fact]
        public void AddHealthRecord_ShouldAssignIdAndAddRecord()
        {
            var records = new List<HealthRecord>
            {
                GetSampleRecord(101),
                GetSampleRecord(102)
            };

            var newRecord = GetSampleRecord(0);

            _healthRepoMock.Setup(r => r.GetAllRecords()).Returns(records);
            _healthRepoMock.Setup(r => r.AddHealthRecord(It.IsAny<HealthRecord>()))
                           .Returns("Record added successfully");

            var result = _service.AddHealthRecord(newRecord);

            Assert.Equal(103, newRecord.RecordId);
            Assert.Contains("successfully", result);
        }

        // GetByPatientIdOrderByVisitDateDesc
        [Fact]
        public void GetByPatientId_ShouldReturnRecords()
        {
            var patient = GetSamplePatient(1);
            var records = new List<HealthRecord> { GetSampleRecord(1) };

            _patientRepoMock.Setup(p => p.GetPatientById(1)).Returns(patient);
            _healthRepoMock.Setup(r => r.GetByPatientIdOrderByVisitDateDesc(1))
                           .Returns(records);

            var result = _service.GetByPatientIdOrderByVisitDateDesc(1);

            Assert.Single(result);
        }

        // GetByPatientId - Patient Not Found
        [Fact]
        public void GetByPatientId_ShouldThrow_WhenPatientNotFound()
        {
            _patientRepoMock.Setup(p => p.GetPatientById(1)).Returns((Patient?)null);

            Assert.Throws<PatientNotFoundException>(() =>
                _service.GetByPatientIdOrderByVisitDateDesc(1));
        }

        // GetByPatientId - Records Not Found
        [Fact]
        public void GetByPatientId_ShouldThrow_WhenNoRecords()
        {
            _patientRepoMock.Setup(p => p.GetPatientById(1))
                            .Returns(GetSamplePatient(1));

            _healthRepoMock.Setup(r => r.GetByPatientIdOrderByVisitDateDesc(1))
                           .Returns(new List<HealthRecord>());

            Assert.Throws<HealthRecordNotFoundException>(() =>
                _service.GetByPatientIdOrderByVisitDateDesc(1));
        }

        // GetByDoctorIdOrderByVisitDateDesc
        [Fact]
        public void GetByDoctorId_ShouldReturnRecords()
        {
            var doctor = GetSampleDoctor(1);
            var records = new List<HealthRecord> { GetSampleRecord(1) };

            _doctorRepoMock.Setup(d => d.GetDoctorById(1)).Returns(doctor);
            _healthRepoMock.Setup(r => r.GetByDoctorIdOrderByVisitDateDesc(1))
                           .Returns(records);

            var result = _service.GetByDoctorIdOrderByVisitDateDesc(1);

            Assert.Single(result);
        }

        // GetByDoctorId - Doctor Not Found
        [Fact]
        public void GetByDoctorId_ShouldThrow_WhenDoctorNotFound()
        {
            _doctorRepoMock.Setup(d => d.GetDoctorById(1)).Returns((Doctor?)null);

            Assert.Throws<DoctorNotFoundException>(() =>
                _service.GetByDoctorIdOrderByVisitDateDesc(1));
        }

        // GetByDoctorId - Records Not Found
        [Fact]
        public void GetByDoctorId_ShouldThrow_WhenNoRecords()
        {
            _doctorRepoMock.Setup(d => d.GetDoctorById(1))
                           .Returns(GetSampleDoctor(1));

            _healthRepoMock.Setup(r => r.GetByDoctorIdOrderByVisitDateDesc(1))
                           .Returns(new List<HealthRecord>());

            Assert.Throws<HealthRecordNotFoundException>(() =>
                _service.GetByDoctorIdOrderByVisitDateDesc(1));
        }

        // GetRecordById
        [Fact]
        public void GetRecordById_ShouldReturnRecord()
        {
            var record = GetSampleRecord(1);

            _healthRepoMock.Setup(r => r.GetRecordById(1))
                           .Returns(record);

            var result = _service.GetRecordById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.RecordId);
        }

        // GetRecordById - Not Found
        [Fact]
        public void GetRecordById_ShouldThrow_WhenNotFound()
        {
            _healthRepoMock.Setup(r => r.GetRecordById(1)).Returns((HealthRecord?)null);

            Assert.Throws<HealthRecordNotFoundException>(() =>
                _service.GetRecordById(1));
        }

        // UpdateHealthRecord
        [Fact]
        public void UpdateHealthRecord_ShouldUpdateRecord()
        {
            var existing = GetSampleRecord(1);
            var updated = GetSampleRecord(1);
            updated.Diagnosis = "Updated Diagnosis";

            _healthRepoMock.Setup(r => r.GetRecordById(1)).Returns(existing);
            _healthRepoMock.Setup(r => r.UpdateHealthRecord(existing, updated))
                           .Returns(updated);

            var result = _service.UpdateHealthRecord(updated);

            Assert.Equal("Updated Diagnosis", result.Diagnosis);
        }

        // UpdateHealthRecord - Not Found
        [Fact]
        public void UpdateHealthRecord_ShouldThrow_WhenNotFound()
        {
            var record = GetSampleRecord(999);

            _healthRepoMock.Setup(r => r.GetRecordById(999))
                           .Returns((HealthRecord?)null);

            Assert.Throws<HealthRecordNotFoundException>(() =>
                _service.UpdateHealthRecord(record));
        }

        // RecordIdGenerator
        [Fact]
        public void RecordIdGenerator_ShouldReturnNextId()
        {
            var records = new List<HealthRecord>
            {
                GetSampleRecord(101),
                GetSampleRecord(102)
            };

            var result = HealthRecordService.RecordIdGenerator(records);

            Assert.Equal(103, result);
        }

        // RecordIdGenerator - Empty
        [Fact]
        public void RecordIdGenerator_ShouldReturn101_WhenEmpty()
        {
            var records = new List<HealthRecord>();

            var result = HealthRecordService.RecordIdGenerator(records);

            Assert.Equal(101, result);
        }
    }
}