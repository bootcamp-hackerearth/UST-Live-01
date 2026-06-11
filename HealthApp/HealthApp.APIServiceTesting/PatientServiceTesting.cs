using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace HealthApp.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _repoMock;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _repoMock = new Mock<IPatientRepository>();
            _service = new PatientService(_repoMock.Object);
        }

        // ✅ RegisterPatient Tests

        [Fact]
        public void RegisterPatient_ShouldThrow_WhenPatientIsNull()
        {
            Assert.Throws<Exception>(() => _service.RegisterPatient(null));
        }

        [Fact]
        public void RegisterPatient_ShouldThrow_WhenNameIsEmpty()
        {
            var patient = new Patient { FullName = "", Email = "test@test.com" };

            Assert.Throws<Exception>(() => _service.RegisterPatient(patient));
        }

        [Fact]
        public void RegisterPatient_ShouldThrow_WhenEmailIsEmpty()
        {
            var patient = new Patient { FullName = "Test", Email = "" };

            Assert.Throws<Exception>(() => _service.RegisterPatient(patient));
        }

        [Fact]
        public void RegisterPatient_ShouldThrow_WhenEmailAlreadyExists()
        {
            var existingPatients = new List<Patient>
            {
                new Patient { Email = "test@test.com" }
            };

            _repoMock.Setup(r => r.GetAll()).Returns(existingPatients);

            var newPatient = new Patient
            {
                FullName = "John",
                Email = "test@test.com"
            };

            Assert.Throws<Exception>(() => _service.RegisterPatient(newPatient));
        }

        [Fact]
        public void RegisterPatient_ShouldCallAdd_WhenValid()
        {
            _repoMock.Setup(r => r.GetAll()).Returns(new List<Patient>());

            var patient = new Patient
            {
                FullName = "John",
                Email = "john@test.com"
            };

            _service.RegisterPatient(patient);

            _repoMock.Verify(r => r.Add(patient), Times.Once);
        }

        // ✅ GetAll Tests

        [Fact]
        public void GetAll_ShouldReturnPatients()
        {
            var patients = new List<Patient>
            {
                new Patient { FullName = "John" },
                new Patient { FullName = "Jane" }
            };

            _repoMock.Setup(r => r.GetAll()).Returns(patients);

            var result = _service.GetAll();

            Assert.Equal(2, result.Count);
        }

        // ✅ GetPatientById Tests

        [Fact]
        public void GetPatientById_ShouldReturnPatient_WhenExists()
        {
            var patient = new Patient { PatientId = 1, FullName = "John" };

            _repoMock.Setup(r => r.GetById(1)).Returns(patient);

            var result = _service.GetPatientById(1);

            Assert.Equal("John", result.FullName);
        }

        [Fact]
        public void GetPatientById_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((Patient)null);

            Assert.Throws<Exception>(() => _service.GetPatientById(1));
        }

        // ✅ UpdatePatientById Tests

        [Fact]
        public void UpdatePatientById_ShouldThrow_WhenPatientNotFound()
        {
            _repoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((Patient)null);

            var patient = new Patient();

            Assert.Throws<Exception>(() => _service.UpdatePatientById(1, patient));
        }

        [Fact]
        public void UpdatePatientById_ShouldCallUpdate_WhenValid()
        {
            var existingPatient = new Patient { PatientId = 1 };

            _repoMock.Setup(r => r.GetById(1)).Returns(existingPatient);

            var updatedPatient = new Patient
            {
                FullName = "Updated Name",
                Email = "updated@test.com"
            };

            _service.UpdatePatientById(1, updatedPatient);

            _repoMock.Verify(r => r.UpdatePatient(1, updatedPatient), Times.Once);
            Assert.Equal(1, updatedPatient.PatientId);
        }
    }
}