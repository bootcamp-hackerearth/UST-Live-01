using AutoMapper;
using HealthAppMVC.Enums;
using HealthAppWebAPI.Models.Dtos;
using HealthAppWebAPI.Repositories.Interfaces;
using HealthAppWebAPI.Services.Impl;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HealthAppWebAPI.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _repoMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new PatientService(
                _repoMock.Object,
                _mapperMock.Object);
        }

        private static CreatePatientDto GetValidCreatePatientDto()
        {
            return new CreatePatientDto
            {
                FullName = "John Patient",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Gender = GenderType.Male.ToString(),
                Email = " patient@test.com ",
                PhoneNumber = "9876543210",
                InsuranceId = "INS001"
            };
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenPatientNotFound_ThrowsKeyNotFoundException()
        {
            int patientId = 1;

            _repoMock
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync((Patient)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.GetPatientByIdAsync(patientId));

            Assert.Equal("Patient not found.", exception.Message);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDtoIsNull_ThrowsArgumentException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.RegisterPatientAsync(null));

            Assert.Equal("Patient data is required.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenFullNameIsEmpty_ThrowsArgumentException()
        {
            var dto = GetValidCreatePatientDto();
            dto.FullName = " ";

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.RegisterPatientAsync(dto));

            Assert.Equal("Patient name is required.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDateOfBirthIsDefault_ThrowsArgumentException()
        {
            var dto = GetValidCreatePatientDto();
            dto.DateOfBirth = default(DateTime);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.RegisterPatientAsync(dto));

            Assert.Equal("Date of birth is required.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDateOfBirthIsFuture_ThrowsInvalidOperationException()
        {
            var dto = GetValidCreatePatientDto();
            dto.DateOfBirth = DateTime.Today.AddDays(1);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.RegisterPatientAsync(dto));

            Assert.Equal("Future date is not allowed.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenGenderIsEmpty_ThrowsArgumentException()
        {
            var dto = GetValidCreatePatientDto();
            dto.Gender = " ";

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.RegisterPatientAsync(dto));

            Assert.Equal("Gender is required.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenGenderIsInvalid_ThrowsArgumentException()
        {
            var dto = GetValidCreatePatientDto();
            dto.Gender = "InvalidGender";

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.RegisterPatientAsync(dto));

            Assert.Equal("Invalid gender.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenEmailIsEmpty_ThrowsArgumentException()
        {
            var dto = GetValidCreatePatientDto();
            dto.Email = " ";

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.RegisterPatientAsync(dto));

            Assert.Equal("Email is required.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
        {
            var dto = GetValidCreatePatientDto();

            _repoMock
                .Setup(r => r.EmailExistsAsync(dto.Email.Trim()))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.RegisterPatientAsync(dto));

            Assert.Equal(
                "A patient with this email already exists.",
                exception.Message);

            _repoMock.Verify(
                r => r.EmailExistsAsync(dto.Email.Trim()),
                Times.Once);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenValid_AddsPatient()
        {
            var dto = GetValidCreatePatientDto();

            Patient addedPatient = null;

            _repoMock
                .Setup(r => r.EmailExistsAsync(dto.Email.Trim()))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(m => m.Map<Patient>(dto))
                .Returns(new Patient());

            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<Patient>()))
                .Callback<Patient>(patient =>
                {
                    addedPatient = patient;
                })
                .Returns(Task.CompletedTask);

            await _service.RegisterPatientAsync(dto);

            Assert.NotNull(addedPatient);
            Assert.Equal(dto.Email.Trim(), addedPatient.Email);
            Assert.Equal(GenderType.Male.ToString(), addedPatient.Gender);
            Assert.True(addedPatient.CreatedDate.HasValue);

            _repoMock.Verify(
                r => r.EmailExistsAsync(dto.Email.Trim()),
                Times.Once);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Patient>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenPatientNotFound_ThrowsKeyNotFoundException()
        {
            int patientId = 1;
            var dto = GetValidCreatePatientDto();

            _repoMock
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync((Patient)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.UpdatePatientAsync(patientId, dto));

            Assert.Equal("Patient not found.", exception.Message);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenDtoIsNull_ThrowsArgumentException()
        {
            int patientId = 1;

            var patient = new Patient
            {
                PatientId = patientId,
                FullName = "John Patient"
            };

            _repoMock
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.UpdatePatientAsync(patientId, null));

            Assert.Equal("Patient data is required.", exception.Message);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenFullNameIsEmpty_ThrowsArgumentException()
        {
            int patientId = 1;

            var patient = new Patient
            {
                PatientId = patientId,
                FullName = "John Patient"
            };

            var dto = GetValidCreatePatientDto();
            dto.FullName = " ";

            _repoMock
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.UpdatePatientAsync(patientId, dto));

            Assert.Equal("Patient name is required.", exception.Message);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenDateOfBirthIsDefault_ThrowsArgumentException()
        {
            int patientId = 1;

            var patient = new Patient
            {
                PatientId = patientId,
                FullName = "John Patient"
            };

            var dto = GetValidCreatePatientDto();
            dto.DateOfBirth = default(DateTime);

            _repoMock
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.UpdatePatientAsync(patientId, dto));

            Assert.Equal("Date of birth is required.", exception.Message);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenDateOfBirthIsFuture_ThrowsInvalidOperationException()
        {
            int patientId = 1;

            var patient = new Patient
            {
                PatientId = patientId,
                FullName = "John Patient"
            };

            var dto = GetValidCreatePatientDto();
            dto.DateOfBirth = DateTime.Today.AddDays(1);

            _repoMock
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.UpdatePatientAsync(patientId, dto));

            Assert.Equal("Future date is not allowed.", exception.Message);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenGenderIsEmpty_ThrowsArgumentException()
        {
            int patientId = 1;

            var patient = new Patient
            {
                PatientId = patientId,
                FullName = "John Patient"
            };

            var dto = GetValidCreatePatientDto();
            dto.Gender = " ";

            _repoMock
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.UpdatePatientAsync(patientId, dto));

            Assert.Equal("Gender is required.", exception.Message);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenGenderIsInvalid_ThrowsArgumentException()
        {
            int patientId = 1;

            var patient = new Patient
            {
                PatientId = patientId,
                FullName = "John Patient"
            };

            var dto = GetValidCreatePatientDto();
            dto.Gender = "InvalidGender";

            _repoMock
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.UpdatePatientAsync(patientId, dto));

            Assert.Equal("Invalid gender.", exception.Message);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenEmailIsEmpty_ThrowsArgumentException()
        {
            int patientId = 1;

            var patient = new Patient
            {
                PatientId = patientId,
                FullName = "John Patient"
            };

            var dto = GetValidCreatePatientDto();
            dto.Email = " ";

            _repoMock
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.UpdatePatientAsync(patientId, dto));

            Assert.Equal("Email is required.", exception.Message);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenEmailUsedByAnotherPatient_ThrowsInvalidOperationException()
        {
            int patientId = 1;

            var patient = new Patient
            {
                PatientId = patientId,
                FullName = "John Patient"
            };

            var dto = GetValidCreatePatientDto();

            _repoMock
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            _repoMock
                .Setup(r => r.EmailExistsForOtherPatientAsync(
                    patientId,
                    dto.Email.Trim()))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.UpdatePatientAsync(patientId, dto));

            Assert.Equal(
                "Another patient already uses this email.",
                exception.Message);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenValid_UpdatesPatient()
        {
            int patientId = 1;

            var patient = new Patient
            {
                PatientId = patientId,
                FullName = "Old Name",
                Email = "old@test.com",
                Gender = GenderType.Female.ToString()
            };

            var dto = GetValidCreatePatientDto();

            _repoMock
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            _repoMock
                .Setup(r => r.EmailExistsForOtherPatientAsync(
                    patientId,
                    dto.Email.Trim()))
                .ReturnsAsync(false);

            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask);

            await _service.UpdatePatientAsync(patientId, dto);

            Assert.Equal(dto.Email.Trim(), patient.Email);
            Assert.Equal(GenderType.Male.ToString(), patient.Gender);

            _repoMock.Verify(
                r => r.UpdateAsync(patient),
                Times.Once);
        }
    }
}
