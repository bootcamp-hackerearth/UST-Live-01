using AutoMapper;
using FluentAssertions;
using HealthAxisCore_Api.DTOs.Patient;
using HealthAxisCore_Api.Enums;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Implementations;
using HealthAxisCore_Api.Tests.Helpers;
using Moq;

namespace HealthAxisCore_Api.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly IMapper _mapper;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _mapper = MapperHelper.GetMapper();

            _patientService = new PatientService(
                _patientRepositoryMock.Object,
                _mapper
            );
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    PatientName = "Ayushi",
                    DateOfBirth = new DateTime(2000, 1, 1),
                    Gender = GenderType.Female,
                    Email = "ayushi@test.com",
                    PhoneNumber = "9876543210",
                    InsuranceID = "INS001"
                },
                new Patient
                {
                    PatientId = 2,
                    PatientName = "Rahul",
                    DateOfBirth = new DateTime(1998, 5, 10),
                    Gender = GenderType.Male,
                    Email = "rahul@test.com",
                    PhoneNumber = "9876543211",
                    InsuranceID = "INS002"
                }
            };

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patients);

            // Act
            var result = await _patientService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().PatientName.Should().Be("Ayushi");
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientExists_ShouldReturnPatient()
        {
            // Arrange
            var patient = new Patient
            {
                PatientId = 1,
                PatientName = "Ayushi",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = GenderType.Female,
                Email = "ayushi@test.com",
                PhoneNumber = "9876543210",
                InsuranceID = "INS001"
            };

            _patientRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(patient);

            // Act
            var result = await _patientService.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.PatientId.Should().Be(1);
            result.PatientName.Should().Be("Ayushi");
            result.Email.Should().Be("ayushi@test.com");
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            // Act
            var act = async () => await _patientService.GetByIdAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Patient not found");
        }

        [Fact]
        public async Task CreateAsync_WhenPatientIsDuplicate_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var dto = new CreatePatientDTO
            {
                PatientName = "Ayushi",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = GenderType.Female,
                Email = "ayushi@test.com",
                PhoneNumber = "9876543210",
                InsuranceID = "INS001"
            };

            _patientRepositoryMock
                .Setup(repo => repo.IsDuplicate(dto.Email, dto.PhoneNumber))
                .ReturnsAsync(true);

            // Act
            var act = async () => await _patientService.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Patient already exists with same email or phone number");

            _patientRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Patient>()),
                Times.Never
            );
        }

        [Fact]
        public async Task CreateAsync_WhenValidPatient_ShouldCreatePatient()
        {
            // Arrange
            var dto = new CreatePatientDTO
            {
                PatientName = "Ayushi",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = GenderType.Female,
                Email = "ayushi@test.com",
                PhoneNumber = "9876543210",
                InsuranceID = "INS001"
            };

            _patientRepositoryMock
                .Setup(repo => repo.IsDuplicate(dto.Email, dto.PhoneNumber))
                .ReturnsAsync(false);

            _patientRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask)
                .Callback<Patient>(patient =>
                {
                    patient.PatientId = 1;
                });

            // Act
            var result = await _patientService.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.PatientId.Should().Be(1);
            result.PatientName.Should().Be(dto.PatientName);
            result.Email.Should().Be(dto.Email);
            result.PhoneNumber.Should().Be(dto.PhoneNumber);

            _patientRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Patient>()),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateAsync_WhenPatientExists_ShouldUpdatePatientAndReturnTrue()
        {
            // Arrange
            var existingPatient = new Patient
            {
                PatientId = 1,
                PatientName = "Old Name",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = GenderType.Female,
                Email = "old@test.com",
                PhoneNumber = "9876543210",
                InsuranceID = "INS001"
            };

            var updateDto = new UpdatePatientDTO
            {
                PatientName = "New Name",
                DateOfBirth = new DateTime(2001, 2, 2),
                Gender = GenderType.Female,
                Email = "new@test.com",
                PhoneNumber = "9876543211",
                InsuranceID = "INS002"
            };

            _patientRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingPatient);

            _patientRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _patientService.UpdateAsync(1, updateDto);

            // Assert
            result.Should().BeTrue();

            existingPatient.PatientName.Should().Be("New Name");
            existingPatient.Email.Should().Be("new@test.com");
            existingPatient.PhoneNumber.Should().Be("9876543211");
            existingPatient.InsuranceID.Should().Be("INS002");

            _patientRepositoryMock.Verify(
                repo => repo.UpdateAsync(existingPatient),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateAsync_WhenPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            var updateDto = new UpdatePatientDTO
            {
                PatientName = "New Name",
                DateOfBirth = new DateTime(2001, 2, 2),
                Gender = GenderType.Female,
                Email = "new@test.com",
                PhoneNumber = "9876543211",
                InsuranceID = "INS002"
            };

            _patientRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            // Act
            var act = async () => await _patientService.UpdateAsync(1, updateDto);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Patient not found");

            _patientRepositoryMock.Verify(
                repo => repo.UpdateAsync(It.IsAny<Patient>()),
                Times.Never
            );
        }

        [Fact]
        public async Task DeleteAsync_WhenPatientExists_ShouldDeletePatientAndReturnTrue()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(true);

            _patientRepositoryMock
                .Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _patientService.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();

            _patientRepositoryMock.Verify(
                repo => repo.DeleteAsync(1),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteAsync_WhenPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(false);

            // Act
            var act = async () => await _patientService.DeleteAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Patient not found");

            _patientRepositoryMock.Verify(
                repo => repo.DeleteAsync(It.IsAny<int>()),
                Times.Never
            );
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    PatientName = "Ayushi",
                    DateOfBirth = new DateTime(2000, 1, 1),
                    Gender = GenderType.Female,
                    Email = "ayushi@test.com",
                    PhoneNumber = "9876543210",
                    InsuranceID = "INS001"
                }
            };

            _patientRepositoryMock
                .Setup(repo => repo.SearchPatients("Ayushi", "ayushi@test.com"))
                .ReturnsAsync(patients);

            // Act
            var result = await _patientService.SearchAsync("Ayushi", "ayushi@test.com");

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().PatientName.Should().Be("Ayushi");

            _patientRepositoryMock.Verify(
                repo => repo.SearchPatients("Ayushi", "ayushi@test.com"),
                Times.Once
            );
        }
    }
}