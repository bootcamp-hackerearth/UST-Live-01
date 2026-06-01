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
    // Test class for PatientService to validate patient management functionalities
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _mockRepo;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _mockRepo = new Mock<IPatientRepository>();
            _service = new PatientService(_mockRepo.Object);
        }
        // Helper methods to create sample patients for testing
        private static Patient GetSamplePatient(int id)
        {
            return new Patient
            {
                PatientId = id,
                Name = "Patient " + id,
                PhoneNumber = "9999999999",
                Email = "patient@test.com"
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

        // UpdatePatient
        [Fact]
        public void UpdatePatient_ShouldUpdateExistingPatient()
        {
            var existing = GetSamplePatient(101);
            var updated = GetSamplePatient(101);
            updated.Name = "Updated Name";

            _mockRepo.Setup(r => r.GetPatientById(101)).Returns(existing);
            _mockRepo.Setup(r => r.UpdatePatient(existing, updated)).Returns(updated);

            var result = _service.UpdatePatient(updated);

            Assert.Equal("Updated Name", result.Name);
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