using System;
using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using SharedClasses.Dtos;
using SharedClasses.Enums;
using HealthcareApi.Exceptions;
using HealthcareApi.Models;
using HealthcareApi.Repositories;
using HealthcareApi.Services.Implementations;

namespace HealthcareMvcApiTests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _sut;

        public PatientServiceTests()
        {
            _patientRepoMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();

            _sut = new PatientService(_patientRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void GetPatientById_ValidId_ReturnsPatientDto()
        {
            // Arrange
            int patientId = 1;

            var patientEntity = new Patient
            {
                PatientId = patientId,
                FullName = "John Doe"
            };

            var patientDto = new PatientDto
            {
                PatientId = patientId,
                FullName = "John Doe"
            };

            _patientRepoMock.Setup(r => r.GetById(patientId)).Returns(patientEntity);
            _mapperMock.Setup(m => m.Map<PatientDto>(patientEntity)).Returns(patientDto);

            // Act
            PatientDto result = _sut.GetPatientById(patientId);

            // Assert
            result.Should().NotBeNull();
            result.PatientId.Should().Be(patientId);
            result.FullName.Should().Be("John Doe");

            _patientRepoMock.Verify(r => r.GetById(patientId), Times.Once);
        }

        [Fact]
        public void GetPatientById_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            int patientId = 99;

            _patientRepoMock.Setup(r => r.GetById(patientId)).Returns((Patient)null);

            // Act
            Action act = () => _sut.GetPatientById(patientId);

            // Assert
            act.Should().Throw<EntityNotFoundException>();

            _patientRepoMock.Verify(r => r.GetById(patientId), Times.Once);
        }

        [Fact]
        public void GetPatientById_ZeroId_ThrowsBusinessRuleException()
        {
            // Arrange
            int patientId = 0;

            // Act
            Action act = () => _sut.GetPatientById(patientId);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Valid Patient ID is required.");

            _patientRepoMock.Verify(r => r.GetById(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void GetPatientById_NegativeId_ThrowsBusinessRuleException()
        {
            // Arrange
            int patientId = -1;

            // Act
            Action act = () => _sut.GetPatientById(patientId);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Valid Patient ID is required.");

            _patientRepoMock.Verify(r => r.GetById(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_ValidDetails_ReturnsPatientDto()
        {
            // Arrange
            var createDto = new CreatePatientDto
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            var mappedPatient = new Patient
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            var savedPatient = new Patient
            {
                PatientId = 1,
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123",
                CreatedDate = DateTime.Today
            };

            var resultDto = new PatientDto
            {
                PatientId = 1,
                FullName = "Jane Doe",
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            _mapperMock.Setup(m => m.Map<Patient>(createDto)).Returns(mappedPatient);
            _patientRepoMock.Setup(r => r.Add(It.IsAny<Patient>())).Returns(savedPatient);
            _mapperMock.Setup(m => m.Map<PatientDto>(savedPatient)).Returns(resultDto);

            // Act
            PatientDto result = _sut.RegisterPatient(createDto);

            // Assert
            result.Should().NotBeNull();
            result.PatientId.Should().Be(1);
            result.FullName.Should().Be("Jane Doe");

            _patientRepoMock.Verify(
                r => r.Add(It.Is<Patient>(p =>
                    p.FullName == "Jane Doe" &&
                    p.PhoneNumber == "1234567890" &&
                    p.Email == "jane@example.com" &&
                    p.InsuranceId == "INS123" &&
                    p.CreatedDate == DateTime.Today)),
                Times.Once);
        }

        [Fact]
        public void RegisterPatient_NullDto_ThrowsBusinessRuleException()
        {
            // Act
            Action act = () => _sut.RegisterPatient(null);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Patient details are required.");

            _patientRepoMock.Verify(r => r.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_TrimsAndNormalizesInputBeforeSaving()
        {
            // Arrange
            var createDto = new CreatePatientDto
            {
                FullName = "  Jane Doe  ",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = " 1234567890 ",
                Email = " jane@example.com ",
                InsuranceId = " ins123 "
            };

            var mappedPatient = new Patient
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            var savedPatient = new Patient
            {
                PatientId = 1,
                FullName = "Jane Doe",
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            var resultDto = new PatientDto
            {
                PatientId = 1,
                FullName = "Jane Doe",
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            _mapperMock.Setup(m => m.Map<Patient>(createDto)).Returns(mappedPatient);
            _patientRepoMock.Setup(r => r.Add(It.IsAny<Patient>())).Returns(savedPatient);
            _mapperMock.Setup(m => m.Map<PatientDto>(savedPatient)).Returns(resultDto);

            // Act
            PatientDto result = _sut.RegisterPatient(createDto);

            // Assert
            result.Should().NotBeNull();

            createDto.FullName.Should().Be("Jane Doe");
            createDto.PhoneNumber.Should().Be("1234567890");
            createDto.Email.Should().Be("jane@example.com");
            createDto.InsuranceId.Should().Be("INS123");

            _patientRepoMock.Verify(r => r.Add(It.IsAny<Patient>()), Times.Once);
        }

        [Fact]
        public void RegisterPatient_InvalidFullName_ThrowsBusinessRuleException()
        {
            // Arrange
            var createDto = new CreatePatientDto
            {
                FullName = "Jane123",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            var mappedPatient = new Patient
            {
                FullName = "Jane123",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            _mapperMock.Setup(m => m.Map<Patient>(createDto)).Returns(mappedPatient);

            // Act
            Action act = () => _sut.RegisterPatient(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Full name can contain only letters and spaces.");

            _patientRepoMock.Verify(r => r.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_MissingFullName_ThrowsBusinessRuleException()
        {
            // Arrange
            var createDto = new CreatePatientDto
            {
                FullName = "",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            var mappedPatient = new Patient
            {
                FullName = "",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            _mapperMock.Setup(m => m.Map<Patient>(createDto)).Returns(mappedPatient);

            // Act
            Action act = () => _sut.RegisterPatient(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Patient full name is required.");

            _patientRepoMock.Verify(r => r.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_DateOfBirthBefore1900_ThrowsBusinessRuleException()
        {
            // Arrange
            var createDto = new CreatePatientDto
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1899, 12, 31),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            var mappedPatient = new Patient
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1899, 12, 31),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            _mapperMock.Setup(m => m.Map<Patient>(createDto)).Returns(mappedPatient);

            // Act
            Action act = () => _sut.RegisterPatient(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Date of birth cannot be before 01 Jan 1900.");

            _patientRepoMock.Verify(r => r.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_FutureDateOfBirth_ThrowsBusinessRuleException()
        {
            // Arrange
            var createDto = new CreatePatientDto
            {
                FullName = "Jane Doe",
                DateOfBirth = DateTime.Today.AddDays(1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            var mappedPatient = new Patient
            {
                FullName = "Jane Doe",
                DateOfBirth = DateTime.Today.AddDays(1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            _mapperMock.Setup(m => m.Map<Patient>(createDto)).Returns(mappedPatient);

            // Act
            Action act = () => _sut.RegisterPatient(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Date of birth cannot be in the future.");

            _patientRepoMock.Verify(r => r.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("123456789")]
        [InlineData("12345678901")]
        [InlineData("12345abc90")]
        [InlineData("abcdefghij")]
        public void RegisterPatient_InvalidPhoneNumber_ThrowsBusinessRuleException(string phoneNumber)
        {
            // Arrange
            var createDto = new CreatePatientDto
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = phoneNumber,
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            var mappedPatient = new Patient
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = phoneNumber,
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            _mapperMock.Setup(m => m.Map<Patient>(createDto)).Returns(mappedPatient);

            // Act
            Action act = () => _sut.RegisterPatient(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Phone number must be exactly 10 digits.");

            _patientRepoMock.Verify(r => r.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_MissingPhoneNumber_ThrowsBusinessRuleException()
        {
            // Arrange
            var createDto = new CreatePatientDto
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            var mappedPatient = new Patient
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            _mapperMock.Setup(m => m.Map<Patient>(createDto)).Returns(mappedPatient);

            // Act
            Action act = () => _sut.RegisterPatient(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Phone number is required.");

            _patientRepoMock.Verify(r => r.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_MissingEmail_ThrowsBusinessRuleException()
        {
            // Arrange
            var createDto = new CreatePatientDto
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "",
                InsuranceId = "INS123"
            };

            var mappedPatient = new Patient
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "",
                InsuranceId = "INS123"
            };

            _mapperMock.Setup(m => m.Map<Patient>(createDto)).Returns(mappedPatient);

            // Act
            Action act = () => _sut.RegisterPatient(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Email is required.");

            _patientRepoMock.Verify(r => r.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Theory]
        [InlineData("ABC123")]
        [InlineData("INSABC")]
        [InlineData("123INS")]
        [InlineData("INS")]
        [InlineData("IN123")]
        public void RegisterPatient_InvalidInsuranceId_ThrowsBusinessRuleException(string insuranceId)
        {
            // Arrange
            var createDto = new CreatePatientDto
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = insuranceId
            };

            var mappedPatient = new Patient
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = insuranceId
            };

            _mapperMock.Setup(m => m.Map<Patient>(createDto)).Returns(mappedPatient);

            // Act
            Action act = () => _sut.RegisterPatient(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Insurance ID must start with INS followed by digits only. Example: INS12345");

            _patientRepoMock.Verify(r => r.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_MissingInsuranceId_ThrowsBusinessRuleException()
        {
            // Arrange
            var createDto = new CreatePatientDto
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = ""
            };

            var mappedPatient = new Patient
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = ""
            };

            _mapperMock.Setup(m => m.Map<Patient>(createDto)).Returns(mappedPatient);

            // Act
            Action act = () => _sut.RegisterPatient(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Insurance ID is required.");

            _patientRepoMock.Verify(r => r.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void UpdatePatient_ValidDetails_ReturnsUpdatedPatientDto()
        {
            // Arrange
            int patientId = 1;

            var updateDto = new UpdatePatientDto
            {
                FullName = "Jane Updated",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "updated@example.com",
                InsuranceId = "INS456"
            };

            var existingPatient = new Patient
            {
                PatientId = patientId,
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1111111111",
                Email = "old@example.com",
                InsuranceId = "INS123"
            };

            var updatedPatient = new Patient
            {
                PatientId = patientId,
                FullName = "Jane Updated",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "updated@example.com",
                InsuranceId = "INS456"
            };

            var resultDto = new PatientDto
            {
                PatientId = patientId,
                FullName = "Jane Updated",
                PhoneNumber = "1234567890",
                Email = "updated@example.com",
                InsuranceId = "INS456"
            };

            _patientRepoMock.Setup(r => r.GetById(patientId)).Returns(existingPatient);
            _patientRepoMock.Setup(r => r.Update(patientId, It.IsAny<Patient>())).Returns(updatedPatient);
            _mapperMock.Setup(m => m.Map<PatientDto>(updatedPatient)).Returns(resultDto);

            // Act
            PatientDto result = _sut.UpdatePatient(patientId, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.FullName.Should().Be("Jane Updated");

            _patientRepoMock.Verify(r => r.Update(
                patientId,
                It.Is<Patient>(p =>
                    p.FullName == "Jane Updated" &&
                    p.PhoneNumber == "1234567890" &&
                    p.Email == "updated@example.com" &&
                    p.InsuranceId == "INS456")),
                Times.Once);
        }

        [Fact]
        public void UpdatePatient_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            int patientId = 99;

            var updateDto = new UpdatePatientDto
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "jane@example.com",
                InsuranceId = "INS123"
            };

            _patientRepoMock.Setup(r => r.GetById(patientId)).Returns((Patient)null);

            // Act
            Action act = () => _sut.UpdatePatient(patientId, updateDto);

            // Assert
            act.Should().Throw<EntityNotFoundException>();

            _patientRepoMock.Verify(r => r.Update(It.IsAny<int>(), It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void UpdatePatient_MissingEmail_ThrowsBusinessRuleException()
        {
            // Arrange
            int patientId = 1;

            var updateDto = new UpdatePatientDto
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "1234567890",
                Email = "",
                InsuranceId = "INS123"
            };

            var existingPatient = new Patient
            {
                PatientId = patientId,
                FullName = "Old Name"
            };

            _patientRepoMock.Setup(r => r.GetById(patientId)).Returns(existingPatient);

            // Act
            Action act = () => _sut.UpdatePatient(patientId, updateDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Email is required.");

            _patientRepoMock.Verify(r => r.Update(It.IsAny<int>(), It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void DeletePatient_ValidId_ReturnsDeletedPatientDto()
        {
            // Arrange
            int patientId = 1;

            var deletedPatient = new Patient
            {
                PatientId = patientId,
                FullName = "Jane Doe"
            };

            var resultDto = new PatientDto
            {
                PatientId = patientId,
                FullName = "Jane Doe"
            };

            _patientRepoMock.Setup(r => r.Delete(patientId)).Returns(deletedPatient);
            _mapperMock.Setup(m => m.Map<PatientDto>(deletedPatient)).Returns(resultDto);

            // Act
            PatientDto result = _sut.DeletePatient(patientId);

            // Assert
            result.Should().NotBeNull();
            result.PatientId.Should().Be(patientId);

            _patientRepoMock.Verify(r => r.Delete(patientId), Times.Once);
        }

        [Fact]
        public void DeletePatient_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            int patientId = 99;

            _patientRepoMock.Setup(r => r.Delete(patientId)).Returns((Patient)null);

            // Act
            Action act = () => _sut.DeletePatient(patientId);

            // Assert
            act.Should().Throw<EntityNotFoundException>();

            _patientRepoMock.Verify(r => r.Delete(patientId), Times.Once);
        }
    }
}