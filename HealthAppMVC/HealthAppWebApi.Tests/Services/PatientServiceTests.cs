using HealthAppWebApi.Models;
using HealthAppWebApi.Repositories.Interface;
using HealthAppWebApi.Services.Impl;
using Moq;
using SharedDto.PatientDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HealthAppWebApi.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _service = new PatientService(_patientRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ReturnsPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    FullName = "John Doe",
                    DateOfBirth = new DateTime(1995, 1, 1),
                    Gender = GenderType.Male,
                    Email = "john@test.com",
                    PhoneNumber = "9876543210",
                    InsuranceId = "INS001",
                    CreatedDate = DateTime.UtcNow
                }
            };

            _patientRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(patients);

            // Act
            var result = await _service.GetAllPatientsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("John Doe", result[0].FullName);
            Assert.Equal("Male", result[0].Gender);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ValidId_ReturnsPatient()
        {
            // Arrange
            var patient = new Patient
            {
                PatientId = 1,
                FullName = "John Doe",
                DateOfBirth = new DateTime(1995, 1, 1),
                Gender = GenderType.Male,
                Email = "john@test.com",
                PhoneNumber = "9876543210",
                InsuranceId = "INS001"
            };

            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(patient);

            // Act
            var result = await _service.GetPatientByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.PatientId);
            Assert.Equal("John Doe", result.FullName);
        }

        [Fact]
        public async Task GetPatientByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Patient)null);

            // Act
            var result = await _service.GetPatientByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RegisterPatientAsync_EmailAlreadyExists_ThrowsException()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "John Doe",
                DateOfBirth = new DateTime(1995, 1, 1),
                Gender = "Male",
                Email = "john@test.com",
                PhoneNumber = "9876543210"
            };

            _patientRepositoryMock
                .Setup(r => r.EmailExistsAsync(dto.Email))
                .ReturnsAsync(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(
                () => _service.RegisterPatientAsync(dto));

            Assert.Equal("Email already exists.", ex.Message);
        }

        [Fact]
        public async Task RegisterPatientAsync_FutureDateOfBirth_ThrowsException()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "John Doe",
                DateOfBirth = DateTime.Today.AddDays(1),
                Gender = "Male",
                Email = "john@test.com",
                PhoneNumber = "9876543210"
            };

            _patientRepositoryMock
                .Setup(r => r.EmailExistsAsync(dto.Email))
                .ReturnsAsync(false);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(
                () => _service.RegisterPatientAsync(dto));

            Assert.Equal("Future date not allowed.", ex.Message);
        }

        [Fact]
        public async Task RegisterPatientAsync_ValidPatient_AddsPatient()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "John Doe",
                DateOfBirth = new DateTime(1995, 1, 1),
                Gender = "Male",
                Email = "john@test.com",
                PhoneNumber = "9876543210",
                InsuranceId = "INS001"
            };

            _patientRepositoryMock
                .Setup(r => r.EmailExistsAsync(dto.Email))
                .ReturnsAsync(false);

            // Act
            await _service.RegisterPatientAsync(dto);

            // Assert
            _patientRepositoryMock.Verify(
                r => r.AddAsync(It.Is<Patient>(p =>
                    p.FullName == dto.FullName &&
                    p.Email == dto.Email &&
                    p.Gender == GenderType.Male)),
                Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_PatientNotFound_ThrowsException()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Patient)null);

            var dto = new CreatePatientDto
            {
                FullName = "Updated Name",
                DateOfBirth = new DateTime(1995, 1, 1),
                Gender = "Male",
                Email = "updated@test.com",
                PhoneNumber = "9999999999"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(
                () => _service.UpdatePatientAsync(1, dto));

            Assert.Equal("Patient not found.", ex.Message);
        }

        [Fact]
        public async Task UpdatePatientAsync_ValidPatient_UpdatesPatient()
        {
            // Arrange
            var patient = new Patient
            {
                PatientId = 1,
                FullName = "Old Name",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = GenderType.Male,
                Email = "old@test.com",
                PhoneNumber = "1234567890"
            };

            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(patient);

            var dto = new CreatePatientDto
            {
                FullName = "New Name",
                DateOfBirth = new DateTime(1995, 1, 1),
                Gender = "Female",
                Email = "new@test.com",
                PhoneNumber = "9999999999",
                InsuranceId = "INS100"
            };

            // Act
            await _service.UpdatePatientAsync(1, dto);

            // Assert
            _patientRepositoryMock.Verify(
                r => r.UpdateAsync(It.Is<Patient>(p =>
                    p.PatientId == 1 &&
                    p.FullName == "New Name" &&
                    p.Email == "new@test.com" &&
                    p.Gender == GenderType.Female)),
                Times.Once);
        }

        [Fact]
        public async Task SearchByNameAsync_ReturnsPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    FullName = "John Doe",
                    DateOfBirth = new DateTime(1995,1,1),
                    Gender = GenderType.Male,
                    Email = "john@test.com",
                    PhoneNumber = "9876543210"
                }
            };

            _patientRepositoryMock
                .Setup(r => r.SearchByNameAsync("John"))
                .ReturnsAsync(patients);

            // Act
            var result = await _service.SearchByNameAsync("John");

            // Assert
            Assert.Single(result);
            Assert.Equal("John Doe", result[0].FullName);
        }

        [Fact]
        public async Task SearchByNameAsync_NoResults_ReturnsEmptyList()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(r => r.SearchByNameAsync("XYZ"))
                .ReturnsAsync(new List<Patient>());

            // Act
            var result = await _service.SearchByNameAsync("XYZ");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAppointmentCountAsync_ReturnsCount()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(r => r.GetAppointmentCountAsync(1))
                .ReturnsAsync(5);

            // Act
            var result = await _service.GetAppointmentCountAsync(1);

            // Assert
            Assert.Equal(5, result);
        }
    }
}