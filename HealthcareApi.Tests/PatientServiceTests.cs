using HealthcareApi.Exceptions;
using HealthcareApi.Models;
using HealthcareApi.Repositories;
using HealthcareApi.Services.Implementations;
using Moq;
using SharedClasses.Dtos;
using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using Xunit;

namespace HealthcareApi.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _patientService = new PatientService(_patientRepositoryMock.Object);
        }

        [Fact]
        public void GetAllPatients_ShouldReturnPatientDtoList_WhenPatientsExist()
        {
            // Arrange
            List<Patient> patients = new List<Patient>
            {
                GetSamplePatient(1),
                GetSamplePatient(2)
            };

            _patientRepositoryMock
                .Setup(repo => repo.GetAll())
                .Returns(patients);

            // Act
            List<PatientDto> result = _patientService.GetAllPatients();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].PatientId);
            Assert.Equal(2, result[1].PatientId);

            _patientRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        }



        [Fact]
        public void GetAllPatients_ShouldReturnEmptyList_WhenRepositoryReturnsNull()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(repo => repo.GetAll())
                .Returns((List<Patient>)null);

            // Act
            List<PatientDto> result = _patientService.GetAllPatients();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _patientRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetPatientById_ShouldReturnPatientDto_WhenPatientExists()
        {
            // Arrange
            int patientId = 1;
            Patient patient = GetSamplePatient(patientId);

            _patientRepositoryMock
                .Setup(repo => repo.GetById(patientId))
                .Returns(patient);

            // Act
            PatientDto result = _patientService.GetPatientById(patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patientId, result.PatientId);
            Assert.Equal(patient.FullName, result.FullName);
            Assert.Equal(patient.Email, result.Email);

            _patientRepositoryMock.Verify(repo => repo.GetById(patientId), Times.Once);
        }

        [Fact]
        public void GetPatientById_ShouldThrowBusinessRuleException_WhenPatientIdIsInvalid()
        {
            // Arrange
            int invalidPatientId = 0;

            // Act
            BusinessRuleException exception = Assert.Throws<BusinessRuleException>(
                () => _patientService.GetPatientById(invalidPatientId));

            // Assert
            Assert.Equal("Please provide a valid patient reference.", exception.Message);

            _patientRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void GetPatientById_ShouldThrowEntityNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            int patientId = 99;

            _patientRepositoryMock
                .Setup(repo => repo.GetById(patientId))
                .Returns((Patient)null);

            // Act
            EntityNotFoundException exception = Assert.Throws<EntityNotFoundException>(
                () => _patientService.GetPatientById(patientId));

            // Assert
            Assert.NotNull(exception);

            _patientRepositoryMock.Verify(repo => repo.GetById(patientId), Times.Once);
        }

        [Fact]
        public void RegisterPatient_WhenSameNameAndPhone_ShouldThrowBusinessRuleException()
        {
            // Arrange
            CreatePatientDto dto = new CreatePatientDto
            {
                FullName = "Rishab Gorla",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "rishab@test.com",
                InsuranceId = "INS123"
            };

            _patientRepositoryMock
                .Setup(r => r.IsDuplicatePatient(
                    dto.FullName.Trim().ToLower(),
                    dto.PhoneNumber.Trim(),
                    dto.Email.Trim().ToLower(),
                    dto.DateOfBirth.Date))
                .Returns(true); // ✅ simulate duplicate found

            // Act & Assert
            BusinessRuleException exception = Assert.Throws<BusinessRuleException>(() =>
                _patientService.RegisterPatient(dto));

            Assert.Equal(
                "A patient with similar details already exists.",
                exception.Message);
        }




        [Fact]
        public void RegisterPatient_ShouldReturnPatientDto_WhenValidDtoIsProvided()
        {
            // Arrange
            CreatePatientDto dto = GetValidCreatePatientDto();

            Patient savedPatient = new Patient
            {
                PatientId = 1,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth.Date,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                InsuranceId = dto.InsuranceId,
                CreatedDate = DateTime.Today
            };

            _patientRepositoryMock
                .Setup(repo => repo.Add(It.IsAny<Patient>()))
                .Returns(savedPatient);

            // Act
            PatientDto result = _patientService.RegisterPatient(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.PatientId);
            Assert.Equal(dto.FullName, result.FullName);
            Assert.Equal(dto.DateOfBirth.Date, result.DateOfBirth);
            Assert.Equal(dto.Email, result.Email);
            Assert.Equal(DateTime.Today, result.CreatedDate);

            _patientRepositoryMock.Verify(repo => repo.Add(It.Is<Patient>(p =>
                p.FullName == dto.FullName &&
                p.DateOfBirth == dto.DateOfBirth.Date &&
                p.Gender == dto.Gender &&
                p.PhoneNumber == dto.PhoneNumber &&
                p.Email == dto.Email &&
                p.InsuranceId == dto.InsuranceId &&
                p.CreatedDate == DateTime.Today
            )), Times.Once);
        }

        [Fact]
        public void RegisterPatient_ShouldThrowBusinessRuleException_WhenDtoIsNull()
        {
            // Arrange
            CreatePatientDto dto = null;

            // Act
            BusinessRuleException exception = Assert.Throws<BusinessRuleException>(
                () => _patientService.RegisterPatient(dto));

            // Assert
            Assert.Equal("Patient details are required.", exception.Message);

            _patientRepositoryMock.Verify(repo => repo.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_ShouldThrowBusinessRuleException_WhenFullNameIsEmpty()
        {
            // Arrange
            CreatePatientDto dto = GetValidCreatePatientDto();
            dto.FullName = "";

            // Act
            BusinessRuleException exception = Assert.Throws<BusinessRuleException>(
                () => _patientService.RegisterPatient(dto));

            // Assert
            Assert.Equal("Patient full name is required.", exception.Message);

            _patientRepositoryMock.Verify(repo => repo.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_ShouldThrowBusinessRuleException_WhenDateOfBirthIsBefore1900()
        {
            // Arrange
            CreatePatientDto dto = GetValidCreatePatientDto();
            dto.DateOfBirth = new DateTime(1899, 12, 31);

            // Act
            BusinessRuleException exception = Assert.Throws<BusinessRuleException>(
                () => _patientService.RegisterPatient(dto));

            // Assert
            Assert.Equal("Date of birth cannot be before 01 Jan 1900.", exception.Message);

            _patientRepositoryMock.Verify(repo => repo.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_ShouldThrowBusinessRuleException_WhenDateOfBirthIsFutureDate()
        {
            // Arrange
            CreatePatientDto dto = GetValidCreatePatientDto();
            dto.DateOfBirth = DateTime.Today.AddDays(1);

            // Act
            BusinessRuleException exception = Assert.Throws<BusinessRuleException>(
                () => _patientService.RegisterPatient(dto));

            // Assert
            Assert.Equal("Date of birth cannot be a future date.", exception.Message);

            _patientRepositoryMock.Verify(repo => repo.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_ShouldThrowBusinessRuleException_WhenPhoneNumberIsEmpty()
        {
            // Arrange
            CreatePatientDto dto = GetValidCreatePatientDto();
            dto.PhoneNumber = "";

            // Act
            BusinessRuleException exception = Assert.Throws<BusinessRuleException>(
                () => _patientService.RegisterPatient(dto));

            // Assert
            Assert.Equal("Phone number is required.", exception.Message);

            _patientRepositoryMock.Verify(repo => repo.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_ShouldThrowBusinessRuleException_WhenEmailIsEmpty()
        {
            // Arrange
            CreatePatientDto dto = GetValidCreatePatientDto();
            dto.Email = "";

            // Act
            BusinessRuleException exception = Assert.Throws<BusinessRuleException>(
                () => _patientService.RegisterPatient(dto));

            // Assert
            Assert.Equal("Email address is required.", exception.Message);

            _patientRepositoryMock.Verify(repo => repo.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void RegisterPatient_ShouldThrowBusinessRuleException_WhenInsuranceIdIsEmpty()
        {
            // Arrange
            CreatePatientDto dto = GetValidCreatePatientDto();
            dto.InsuranceId = "";

            // Act
            BusinessRuleException exception = Assert.Throws<BusinessRuleException>(
                () => _patientService.RegisterPatient(dto));

            // Assert
            Assert.Equal("Insurance ID is required.", exception.Message);

            _patientRepositoryMock.Verify(repo => repo.Add(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void UpdatePatient_ShouldReturnUpdatedPatientDto_WhenValidDataIsProvided()
        {
            // Arrange
            int patientId = 1;

            Patient existingPatient = GetSamplePatient(patientId);
            UpdatePatientDto dto = GetValidUpdatePatientDto();

            _patientRepositoryMock
                .Setup(repo => repo.GetById(patientId))
                .Returns(existingPatient);

            _patientRepositoryMock
                .Setup(repo => repo.Update(patientId, It.IsAny<Patient>()))
                .Returns((int id, Patient patient) => patient);

            // Act
            PatientDto result = _patientService.UpdatePatient(patientId, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patientId, result.PatientId);
            Assert.Equal(dto.FullName, result.FullName);
            Assert.Equal(dto.DateOfBirth.Date, result.DateOfBirth);
            Assert.Equal(dto.Gender, result.Gender);
            Assert.Equal(dto.PhoneNumber, result.PhoneNumber);
            Assert.Equal(dto.Email, result.Email);
            Assert.Equal(dto.InsuranceId, result.InsuranceId);

            _patientRepositoryMock.Verify(repo => repo.GetById(patientId), Times.Once);

            _patientRepositoryMock.Verify(repo => repo.Update(patientId, It.Is<Patient>(p =>
                p.FullName == dto.FullName &&
                p.DateOfBirth == dto.DateOfBirth.Date &&
                p.Gender == dto.Gender &&
                p.PhoneNumber == dto.PhoneNumber &&
                p.Email == dto.Email &&
                p.InsuranceId == dto.InsuranceId
            )), Times.Once);
        }

        [Fact]
        public void UpdatePatient_ShouldThrowBusinessRuleException_WhenPatientIdIsInvalid()
        {
            // Arrange
            int invalidPatientId = -1;
            UpdatePatientDto dto = GetValidUpdatePatientDto();

            // Act
            BusinessRuleException exception = Assert.Throws<BusinessRuleException>(
                () => _patientService.UpdatePatient(invalidPatientId, dto));

            // Assert
            Assert.Equal("Please provide a valid patient reference.", exception.Message);

            _patientRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Never);
            _patientRepositoryMock.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void UpdatePatient_ShouldThrowBusinessRuleException_WhenDtoIsNull()
        {
            // Arrange
            int patientId = 1;
            UpdatePatientDto dto = null;

            // Act
            BusinessRuleException exception = Assert.Throws<BusinessRuleException>(
                () => _patientService.UpdatePatient(patientId, dto));

            // Assert
            Assert.Equal("Patient details are required.", exception.Message);

            _patientRepositoryMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Never);
            _patientRepositoryMock.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void UpdatePatient_ShouldThrowEntityNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            int patientId = 99;
            UpdatePatientDto dto = GetValidUpdatePatientDto();

            _patientRepositoryMock
                .Setup(repo => repo.GetById(patientId))
                .Returns((Patient)null);

            // Act
            EntityNotFoundException exception = Assert.Throws<EntityNotFoundException>(
                () => _patientService.UpdatePatient(patientId, dto));

            // Assert
            Assert.NotNull(exception);

            _patientRepositoryMock.Verify(repo => repo.GetById(patientId), Times.Once);
            _patientRepositoryMock.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void UpdatePatient_ShouldThrowBusinessRuleException_WhenUpdatedDateOfBirthIsFutureDate()
        {
            // Arrange
            int patientId = 1;

            Patient existingPatient = GetSamplePatient(patientId);
            UpdatePatientDto dto = GetValidUpdatePatientDto();
            dto.DateOfBirth = DateTime.Today.AddDays(1);

            _patientRepositoryMock
                .Setup(repo => repo.GetById(patientId))
                .Returns(existingPatient);

            // Act
            BusinessRuleException exception = Assert.Throws<BusinessRuleException>(
                () => _patientService.UpdatePatient(patientId, dto));

            // Assert
            Assert.Equal("Date of birth cannot be a future date.", exception.Message);

            _patientRepositoryMock.Verify(repo => repo.GetById(patientId), Times.Once);
            _patientRepositoryMock.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void UpdatePatient_ShouldThrowEntityNotFoundException_WhenRepositoryUpdateReturnsNull()
        {
            // Arrange
            int patientId = 1;

            Patient existingPatient = GetSamplePatient(patientId);
            UpdatePatientDto dto = GetValidUpdatePatientDto();

            _patientRepositoryMock
                .Setup(repo => repo.GetById(patientId))
                .Returns(existingPatient);

            _patientRepositoryMock
                .Setup(repo => repo.Update(patientId, It.IsAny<Patient>()))
                .Returns((Patient)null);

            // Act
            EntityNotFoundException exception = Assert.Throws<EntityNotFoundException>(
                () => _patientService.UpdatePatient(patientId, dto));

            // Assert
            Assert.NotNull(exception);

            _patientRepositoryMock.Verify(repo => repo.GetById(patientId), Times.Once);
            _patientRepositoryMock.Verify(repo => repo.Update(patientId, It.IsAny<Patient>()), Times.Once);
        }

        [Fact]
        public void DeletePatient_ShouldReturnDeletedPatientDto_WhenPatientExists()
        {
            // Arrange
            int patientId = 1;
            Patient deletedPatient = GetSamplePatient(patientId);

            _patientRepositoryMock
                .Setup(repo => repo.Delete(patientId))
                .Returns(deletedPatient);

            // Act
            PatientDto result = _patientService.DeletePatient(patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patientId, result.PatientId);
            Assert.Equal(deletedPatient.FullName, result.FullName);

            _patientRepositoryMock.Verify(repo => repo.Delete(patientId), Times.Once);
        }

        [Fact]
        public void DeletePatient_ShouldThrowBusinessRuleException_WhenPatientIdIsInvalid()
        {
            // Arrange
            int invalidPatientId = 0;

            // Act
            BusinessRuleException exception = Assert.Throws<BusinessRuleException>(
                () => _patientService.DeletePatient(invalidPatientId));

            // Assert
            Assert.Equal("Please provide a valid patient reference.", exception.Message);

            _patientRepositoryMock.Verify(repo => repo.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void DeletePatient_ShouldThrowEntityNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            int patientId = 99;

            _patientRepositoryMock
                .Setup(repo => repo.Delete(patientId))
                .Returns((Patient)null);

            // Act
            EntityNotFoundException exception = Assert.Throws<EntityNotFoundException>(
                () => _patientService.DeletePatient(patientId));

            // Assert
            Assert.NotNull(exception);

            _patientRepositoryMock.Verify(repo => repo.Delete(patientId), Times.Once);
        }

        private Patient GetSamplePatient(int patientId)
        {
            return new Patient
            {
                PatientId = patientId,
                FullName = "John Doe",
                DateOfBirth = new DateTime(1995, 5, 10),
                Gender = Gender.Male,          
                PhoneNumber = "9876543210",
                Email = "john@example.com",
                InsuranceId = "INS123",
                CreatedDate = DateTime.Today
            };
        }

        private CreatePatientDto GetValidCreatePatientDto()
        {
            return new CreatePatientDto
            {
                FullName = "John Doe",
                DateOfBirth = new DateTime(1995, 5, 10),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "john@example.com",
                InsuranceId = "INS123"
            };
        }

        private UpdatePatientDto GetValidUpdatePatientDto()
        {
            return new UpdatePatientDto
            {
                FullName = "Jane Doe",
                DateOfBirth = new DateTime(1998, 8, 15),
                Gender = Gender.Female,
                PhoneNumber = "9123456780",
                Email = "jane@example.com",
                InsuranceId = "INS456"
            };
        }
    }
}