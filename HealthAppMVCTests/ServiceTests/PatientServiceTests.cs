using FluentAssertions;
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
        private readonly Mock<IPatientRepository> _repoMock;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _repoMock = new Mock<IPatientRepository>();
            _service = new PatientService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ShouldReturnListOfPatientDtos()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    FullName = "John Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = 1,
                    Email = "john@example.com",
                    PhoneNumber = "9876543210",
                    InsuranceId = "INS001",
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new Patient
                {
                    PatientId = 2,
                    FullName = "Jane Smith",
                    DateOfBirth = new DateTime(1995, 5, 10),
                    Gender = 2,
                    Email = "jane@example.com",
                    PhoneNumber = "9876543211",
                    InsuranceId = "INS002",
                    CreatedDate = new DateTime(2024, 2, 1)
                }
            };

            _repoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(patients);

            // Act
            var result = await _service.GetAllPatientsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            result[0].PatientId.Should().Be(1);
            result[0].FullName.Should().Be("John Doe");
            result[0].Email.Should().Be("john@example.com");
            result[0].PhoneNumber.Should().Be("9876543210");
            result[0].InsuranceId.Should().Be("INS001");

            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenPatientExists_ShouldReturnPatientDto()
        {
            // Arrange
            var patient = new Patient
            {
                PatientId = 1,
                FullName = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = 1,
                Email = "john@example.com",
                PhoneNumber = "9876543210",
                InsuranceId = "INS001",
                CreatedDate = new DateTime(2024, 1, 1)
            };

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(patient);

            // Act
            var result = await _service.GetPatientByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.PatientId.Should().Be(1);
            result.FullName.Should().Be("John Doe");
            result.Email.Should().Be("john@example.com");
            result.DateOfBirth.Should().Be(new DateTime(1990, 1, 1));

            _repoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenPatientDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Patient)null);

            // Act
            var result = await _service.GetPatientByIdAsync(99);

            // Assert
            result.Should().BeNull();

            _repoMock.Verify(r => r.GetByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenEmailAlreadyExists_ShouldThrowException()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                Email = "john@example.com",
                PhoneNumber = "9876543210",
                InsuranceId = "INS001"
            };

            _repoMock
                .Setup(r => r.EmailExistsAsync(dto.Email))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _service.RegisterPatientAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Email already exists.");

            _repoMock.Verify(r => r.EmailExistsAsync(dto.Email), Times.Once);
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDateOfBirthIsFuture_ShouldThrowException()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "Future Patient",
                DateOfBirth = DateTime.Today.AddDays(1),
                Gender = "Male",
                Email = "future@example.com",
                PhoneNumber = "9876543210",
                InsuranceId = "INS001"
            };

            _repoMock
                .Setup(r => r.EmailExistsAsync(dto.Email))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _service.RegisterPatientAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Future date not allowed.");

            _repoMock.Verify(r => r.EmailExistsAsync(dto.Email), Times.Once);
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenValidData_ShouldAddPatient()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                Email = "john@example.com",
                PhoneNumber = "9876543210",
                InsuranceId = "INS001"
            };

            _repoMock
                .Setup(r => r.EmailExistsAsync(dto.Email))
                .ReturnsAsync(false);

            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.RegisterPatientAsync(dto);

            // Assert
            _repoMock.Verify(r => r.AddAsync(It.Is<Patient>(p =>
                p.FullName == dto.FullName &&
                p.DateOfBirth == dto.DateOfBirth &&
                p.Email == dto.Email &&
                p.PhoneNumber == dto.PhoneNumber &&
                p.InsuranceId == dto.InsuranceId &&
                p.CreatedDate != default
            )), Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenPatientDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "Updated Name",
                DateOfBirth = new DateTime(1992, 2, 2),
                Gender = "Female",
                Email = "updated@example.com",
                PhoneNumber = "9876543210",
                InsuranceId = "INS009"
            };

            _repoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Patient)null);

            // Act
            Func<Task> act = async () => await _service.UpdatePatientAsync(99, dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Patient not found.");

            _repoMock.Verify(r => r.GetByIdAsync(99), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenPatientExists_ShouldUpdatePatient()
        {
            // Arrange
            var existingPatient = new Patient
            {
                PatientId = 1,
                FullName = "Old Name",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = 1,
                Email = "old@example.com",
                PhoneNumber = "1111111111",
                InsuranceId = "OLD001",
                CreatedDate = new DateTime(2024, 1, 1)
            };

            var dto = new CreatePatientDto
            {
                FullName = "Updated Name",
                DateOfBirth = new DateTime(1992, 2, 2),
                Gender = "Female",
                Email = "updated@example.com",
                PhoneNumber = "9999999999",
                InsuranceId = "NEW001"
            };

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingPatient);

            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdatePatientAsync(1, dto);

            // Assert
            _repoMock.Verify(r => r.UpdateAsync(It.Is<Patient>(p =>
                p.PatientId == 1 &&
                p.FullName == dto.FullName &&
                p.DateOfBirth == dto.DateOfBirth &&
                p.Email == dto.Email &&
                p.PhoneNumber == dto.PhoneNumber &&
                p.InsuranceId == dto.InsuranceId
            )), Times.Once);
        }

        [Fact]
        public async Task SearchByNameAsync_ShouldReturnMatchingPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    FullName = "John Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = 1,
                    Email = "john@example.com",
                    PhoneNumber = "9876543210",
                    InsuranceId = "INS001",
                    CreatedDate = new DateTime(2024, 1, 1)
                }
            };

            _repoMock
                .Setup(r => r.SearchByNameAsync("John"))
                .ReturnsAsync(patients);

            // Act
            var result = await _service.SearchByNameAsync("John");

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].FullName.Should().Be("John Doe");
            result[0].Email.Should().Be("john@example.com");

            _repoMock.Verify(r => r.SearchByNameAsync("John"), Times.Once);
        }

        [Fact]
        public async Task GetAppointmentCountAsync_ShouldReturnAppointmentCount()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetAppointmentCountAsync(1))
                .ReturnsAsync(5);

            // Act
            var result = await _service.GetAppointmentCountAsync(1);

            // Assert
            result.Should().Be(5);

            _repoMock.Verify(r => r.GetAppointmentCountAsync(1), Times.Once);
        }
    }
}