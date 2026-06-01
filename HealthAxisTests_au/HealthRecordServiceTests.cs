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
            // Arrange
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

            // Act
            var result = _service.AddRecord(record);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Fever", result.Diagnosis);
        }

        [Fact]
        public void AddRecord_WhenRecordIsNull_ShouldThrowException()
        {
            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() =>
            {
                _service.AddRecord(null!);
            });

            Assert.Equal(
                "Health record cannot be null.",
                ex.Message);
        }

        [Fact]
        public void AddRecord_WhenDiagnosisIsEmpty_ShouldThrowException()
        {
            // Arrange
            HealthRecord record = new HealthRecord
            {
                RecordId = 1,
                Patient = new Patient
                {
                    PatientId = 1
                },
                Doctor = new Doctor
                {
                    DoctorId = 1
                },
                VisitDate = DateTime.Now,
                Diagnosis = "",
                Prescription = "Medicine",
                Notes = "Notes"
            };

            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() =>
            {
                _service.AddRecord(record);
            });

            Assert.Equal(
                "Diagnosis cannot be empty.",
                ex.Message);
        }

        [Fact]
        public void GetRecordsByPatient_ShouldReturnPatientRecords()
        {
            // Arrange
            HealthRecord record1 = new HealthRecord
            {
                RecordId = 1,
                Patient = new Patient
                {
                    PatientId = 1
                },
                Doctor = new Doctor
                {
                    DoctorId = 1
                },
                VisitDate = DateTime.Now,
                Diagnosis = "Cold"
            };

            HealthRecord record2 = new HealthRecord
            {
                RecordId = 2,
                Patient = new Patient
                {
                    PatientId = 2
                },
                Doctor = new Doctor
                {
                    DoctorId = 1
                },
                VisitDate = DateTime.Now,
                Diagnosis = "Fever"
            };

            _service.AddRecord(record1);
            _service.AddRecord(record2);

            // Act
            var result = _service.GetRecordsByPatient(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].Patient.PatientId);
        }

        [Fact]
        public void GetRecordsByDoctor_ShouldReturnDoctorRecords()
        {
            // Arrange
            HealthRecord record1 = new HealthRecord
            {
                RecordId = 1,
                Patient = new Patient
                {
                    PatientId = 1
                },
                Doctor = new Doctor
                {
                    DoctorId = 1
                },
                VisitDate = DateTime.Now,
                Diagnosis = "Headache"
            };

            HealthRecord record2 = new HealthRecord
            {
                RecordId = 2,
                Patient = new Patient
                {
                    PatientId = 2
                },
                Doctor = new Doctor
                {
                    DoctorId = 2
                },
                VisitDate = DateTime.Now,
                Diagnosis = "Fever"
            };

            _service.AddRecord(record1);
            _service.AddRecord(record2);

            // Act
            var result = _service.GetRecordsByDoctor(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].Doctor.DoctorId);
        }
    }
}