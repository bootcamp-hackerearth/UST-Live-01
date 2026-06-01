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
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _mockRepo;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _mockRepo = new Mock<IPatientRepository>();
            _service = new PatientService(_mockRepo.Object);
        }

        private static Patient GetSamplePatient(int id)
        {
            return new Patient
            {
                PatientId = id,
                FullName = "Patient " + id,
                PhoneNumber = "9999999999",
                Email = "patient@test.com",
                InsuranceId = "sdkjfh234"
            };
        }

        // RegisterPatient
        [Fact]
        public void RegisterPatient_ShouldAssignIdAndAddPatient()
        {
            // Arrange
            var patients = new List<Patient>
            {
                GetSamplePatient(101),
                GetSamplePatient(102)
            };

            var newPatient = GetSamplePatient(0);

            _mockRepo.Setup(r => r.GetAllPatients()).Returns(patients);
            _mockRepo.Setup(r => r.RegisterPatient(It.IsAny<Patient>()))
                     .Returns("Patient added successfully");

            // Act
            var result = _service.RegisterPatient(newPatient);

            // Assert
            Assert.Equal(103, newPatient.PatientId);
            Assert.Contains("successfully", result);
        }

        // GetPatientById
        [Fact]
        public void GetPatientById_ShouldReturnPatient()
        {
            var patient = GetSamplePatient(101);

            _mockRepo.Setup(r => r.GetPatientById(101)).Returns(patient);

            var result = _service.GetPatientById(101);

            Assert.NotNull(result);
            Assert.Equal(101, result.PatientId);
        }

        // GetPatientById - Exception
        [Fact]
        public void GetPatientById_ShouldThrowException_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetPatientById(999)).Returns((Patient?)null);

            Assert.Throws<PatientNotFoundException>(() => _service.GetPatientById(999));
        }

        [Fact]
        public void GetAllPatients_ShouldReturnPatients_WhenDataExists()
        {
            // Arrange
            var patients = new List<Patient>
            {
                GetSamplePatient(101),
                GetSamplePatient(102)
            };

            _mockRepo.Setup(r => r.GetAllPatients()).Returns(patients);

            // Act
            var result = _service.GetAllPatients();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetPatientByName_ShouldReturnMatchingPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient { PatientId = 1, FullName = "John Doe", PhoneNumber = "9999999999",
                Email = "patient@test.com",
                InsuranceId = "sdkjfh234" },
                new Patient { PatientId = 2, FullName = "Jane Doe", PhoneNumber = "9999999999",
                Email = "patient@test.com",
                InsuranceId = "sdkjfh234" }
            };

            _mockRepo
                .Setup(r => r.GetPatientByName("doe"))
                .Returns(patients);

            // Act
            var result = _service.GetPatientByName("doe");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetAllPatients_ShouldThrowException_WhenNoPatients()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllPatients())
                    .Returns(new List<Patient>());

            // Act & Assert
            Assert.Throws<PatientNotFoundException>(() => _service.GetAllPatients());
        }

        [Fact]
        public void GetPatientByName_ShouldThrowException_WhenNoMatchFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetPatientByName("unknown"))
                    .Returns(new List<Patient>());

            // Act & Assert
            Assert.Throws<PatientNotFoundException>(() => 
                _service.GetPatientByName("unknown"));
        }


        // UpdatePatient
        [Fact]
        public void UpdatePatient_ShouldUpdateExistingPatient()
        {
            var existing = GetSamplePatient(101);
            var updated = GetSamplePatient(101);
            updated.FullName = "Updated Name";

            _mockRepo.Setup(r => r.GetPatientById(101)).Returns(existing);
            _mockRepo.Setup(r => r.UpdatePatient(existing, updated)).Returns(updated);

            var result = _service.UpdatePatient(updated);

            Assert.Equal("Updated Name", result.FullName);
        }

        // UpdatePatient - Exception
        [Fact]
        public void UpdatePatient_ShouldThrowException_WhenPatientNotFound()
        {
            var patient = GetSamplePatient(999);

            _mockRepo.Setup(r => r.GetPatientById(999)).Returns((Patient?)null);

            Assert.Throws<PatientNotFoundException>(() => _service.UpdatePatient(patient));
        }

        // PatientIdGenerator
        [Fact]
        public void PatientIdGenerator_ShouldReturnNextId()
        {
            var patients = new List<Patient>
            {
                GetSamplePatient(101),
                GetSamplePatient(102)
            };

            var result = PatientService.PatientIdGenerator(patients);

            Assert.Equal(103, result);
        }

        // PatientIdGenerator - Empty List
        [Fact]
        public void PatientIdGenerator_ShouldReturn101_WhenEmpty()
        {
            var patients = new List<Patient>();

            var result = PatientService.PatientIdGenerator(patients);

            Assert.Equal(101, result);
        }
    }
}