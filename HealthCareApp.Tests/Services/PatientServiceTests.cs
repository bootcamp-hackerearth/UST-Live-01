using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FluentAssertions;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services;
using HealthCareApp.Shared.Dtos.Patients;
using HealthCareApp.Tests.Helpers;
using Moq;
using Xunit;

namespace HealthCareApp.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _repository;

        private readonly Mock<IMapper> _mapper;

        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _repository = new Mock<IPatientRepository>();

            _mapper = new Mock<IMapper>();

            _service = new PatientService(
                _repository.Object,
                _mapper.Object);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ShouldReturnPatients()
        {
            // Arrange

            var patients = new List<Patient>
    {
        PatientTestData.Patient
    };

            var patientDtos = new List<PatientDto>
    {
        PatientTestData.PatientDto
    };

            _repository
                .Setup(x => x.GetAllAsync(default))
                .ReturnsAsync(patients);

            _mapper
                .Setup(x => x.Map<List<PatientDto>>(patients))
                .Returns(patientDtos);

            // Act

            var result = await _service.GetAllPatientsAsync();

            // Assert

            result.Should().NotBeNull();

            result.Should().HaveCount(1);

            result[0].PatientId.Should().Be(1);

            _repository.Verify(
                x => x.GetAllAsync(default),
                Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_IdZero_ShouldThrowBusinessRuleException()
        {
            Func<Task> action =
                () => _service.GetPatientByIdAsync(0);

            await action.Should()
                .ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task GetPatientByIdAsync_PatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            _repository
                .Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync((Patient?)null);

            Func<Task> action =
                () => _service.GetPatientByIdAsync(1);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetPatientByIdAsync_ShouldReturnPatient()
        {
            var patient = PatientTestData.Patient;

            var dto = PatientTestData.PatientDto;

            _repository
                .Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(patient);

            _mapper
                .Setup(x => x.Map<PatientDto>(patient))
                .Returns(dto);

            var result =
                await _service.GetPatientByIdAsync(1);

            result.PatientId.Should().Be(1);

            result.FullName.Should().Be("John Smith");

            _repository.Verify(
                x => x.GetByIdAsync(1, default),
                Times.Once);
        }


        [Fact]
        public async Task RegisterPatientAsync_DuplicatePatient_ShouldThrowConflictException()
        {
            var dto = PatientTestData.CreatePatientDto;

            _repository
                .Setup(x => x.IsDuplicatePatientAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    null,
                    default))
                .ReturnsAsync(true);

            Func<Task> action =
                () => _service.RegisterPatientAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>();
        }


        [Fact]
        public async Task RegisterPatientAsync_ShouldCreatePatient()
        {
            var dto = PatientTestData.CreatePatientDto;

            var patient = PatientTestData.Patient;

            var saved = PatientTestData.Patient;

            var patientDto = PatientTestData.PatientDto;

            _repository
                .Setup(x => x.IsDuplicatePatientAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    null,
                    default))
                .ReturnsAsync(false);

            _mapper
                .Setup(x => x.Map<Patient>(dto))
                .Returns(patient);

            _repository
                .Setup(x => x.CreateAsync(
                    It.IsAny<Patient>(),
                    default))
                .ReturnsAsync(saved);

            _mapper
                .Setup(x => x.Map<PatientDto>(saved))
                .Returns(patientDto);

            var result =
                await _service.RegisterPatientAsync(dto);

            result.PatientId.Should().Be(1);

            _repository.Verify(
                x => x.CreateAsync(
                    It.IsAny<Patient>(),
                    default),
                Times.Once);
        }


        [Fact]
        public async Task UpdatePatientAsync_PatientNotFound_ShouldThrowEntityNotFoundException()
        {
            _repository
                .Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync((Patient?)null);

            Func<Task> action =
                () => _service.UpdatePatientAsync(1, PatientTestData.UpdatePatientDto);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetMyProfileAsync_ShouldReturnProfile()
        {
            var patient = PatientTestData.Patient;

            var dto = PatientTestData.PatientDto;

            patient.IdentityUserId = "abc123";

            _repository
                .Setup(x => x.GetByIdentityUserIdAsync("abc123", default))
                .ReturnsAsync(patient);

            _mapper
                .Setup(x => x.Map<PatientDto>(patient))
                .Returns(dto);

            var result =
                await _service.GetMyProfileAsync("abc123");

            result.PatientId.Should().Be(1);
        }

        [Fact]
        public async Task GetMyProfileAsync_InvalidIdentity_ShouldThrowBusinessRuleException()
        {
            Func<Task> action =
                () => _service.GetMyProfileAsync("");

            await action.Should()
                .ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task UpdateMyProfileAsync_ShouldUpdatePatient()
        {
            var patient = PatientTestData.Patient;

            patient.IdentityUserId = "abc123";

            var dto = PatientTestData.UpdatePatientDto;

            _repository
                .Setup(x => x.GetByIdentityUserIdAsync("abc123", default))
                .ReturnsAsync(patient);

            _repository
                .Setup(x => x.UpdateAsync(
                    patient.PatientId,
                    It.IsAny<Patient>(),
                    default))
                .ReturnsAsync(patient);

            _mapper
                .Setup(x => x.Map<Patient>(dto))
                .Returns(patient);

            _mapper
                .Setup(x => x.Map<PatientDto>(patient))
                .Returns(PatientTestData.PatientDto);

            var result =
                await _service.UpdateMyProfileAsync("abc123", dto);

            result.PatientId.Should().Be(1);

            _repository.Verify(
                x => x.UpdateAsync(
                    patient.PatientId,
                    It.IsAny<Patient>(),
                    default),
                Times.Once);
        }
    }
}
