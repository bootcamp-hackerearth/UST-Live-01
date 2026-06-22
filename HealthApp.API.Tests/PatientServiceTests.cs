using AutoMapper;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using HealthApp.API.Models;
using HealthApp.API.Models.DTOs;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.API.Exceptions;

namespace HealthApp.API.Tests
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

        [Fact]
        public async Task GetAllPatientsAsync_ShouldReturnMappedPatients()
        {
            var patients = new List<Patient>
        {
            new Patient { PatientId = 1 },
            new Patient { PatientId = 2 }
        };

            var dtos = new List<PatientDto>
        {
            new PatientDto { PatientId = 1 },
            new PatientDto { PatientId = 2 }
        };

            _repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(patients);

            _mapperMock.Setup(m => m.Map<List<PatientDto>>(patients))
                .Returns(dtos);

            var result = await _service.GetAllPatientsAsync();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ShouldThrow_WhenInvalidId()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.GetPatientByIdAsync(0));
        }

        [Fact]
        public async Task GetPatientByIdAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.GetPatientByIdAsync(1));
        }

        [Fact]
        public async Task GetPatientByIdAsync_ShouldReturnPatientDto_WhenValid()
        {
            var patient = new Patient { PatientId = 1 };
            var dto = new PatientDto { PatientId = 1 };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _mapperMock.Setup(m => m.Map<PatientDto>(patient))
                .Returns(dto);

            var result = await _service.GetPatientByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.PatientId);
        }

        [Fact]
        public async Task RegisterPatientAsync_ShouldThrow_WhenNameEmpty()
        {
            var dto = new CreatePatientDto
            {
                FullName = "",
                DateOfBirth = DateTime.Today.AddYears(-20),
                Email = "test@test.com",
                PhoneNumber = "1234567890"
            };

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.RegisterPatientAsync(dto));
        }

        [Fact]
        public async Task RegisterPatientAsync_ShouldThrow_WhenInvalidDob()
        {
            var dto = new CreatePatientDto
            {
                FullName = "John",
                DateOfBirth = DateTime.Today.AddDays(1),
                Email = "test@test.com",
                PhoneNumber = "1234567890"
            };

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.RegisterPatientAsync(dto));
        }

        [Fact]
        public async Task RegisterPatientAsync_ShouldThrow_WhenEmailEmpty()
        {
            var dto = new CreatePatientDto
            {
                FullName = "John",
                DateOfBirth = DateTime.Today.AddYears(-20),
                Email = "",
                PhoneNumber = "1234567890"
            };

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.RegisterPatientAsync(dto));
        }

        [Fact]
        public async Task RegisterPatientAsync_ShouldThrow_WhenPhoneEmpty()
        {
            var dto = new CreatePatientDto
            {
                FullName = "John",
                DateOfBirth = DateTime.Today.AddYears(-20),
                Email = "test@test.com",
                PhoneNumber = ""
            };

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.RegisterPatientAsync(dto));
        }

        [Fact]
        public async Task RegisterPatientAsync_ShouldCreatePatient_WhenValid()
        {
            var dto = new CreatePatientDto
            {
                FullName = "John",
                DateOfBirth = DateTime.Today.AddYears(-20),
                Email = "test@test.com",
                PhoneNumber = "1234567890"
            };

            var patient = new Patient();
            var savedPatient = new Patient { PatientId = 1 };
            var resultDto = new PatientDto { PatientId = 1 };

            _mapperMock.Setup(m => m.Map<Patient>(dto))
                .Returns(patient);

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Patient>()))
                .ReturnsAsync(savedPatient);

            _mapperMock.Setup(m => m.Map<PatientDto>(savedPatient))
                .Returns(resultDto);

            var result = await _service.RegisterPatientAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(1, result.PatientId);
        }


        [Fact]
        public async Task UpdatePatientAsync_ShouldThrow_WhenInvalidId()
        {
            var dto = new UpdatePatientDto();

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.UpdatePatientAsync(0, dto));
        }

        [Fact]
        public async Task UpdatePatientAsync_ShouldThrow_WhenPatientNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.UpdatePatientAsync(1, new UpdatePatientDto()));
        }

        [Fact]
        public async Task UpdatePatientAsync_ShouldThrow_WhenInvalidData()
        {
            var existing = new Patient { PatientId = 1 };

            var dto = new UpdatePatientDto
            {
                FullName = "",
                DateOfBirth = DateTime.Today,
                Email = "a@a.com",
                PhoneNumber = "123"
            };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.UpdatePatientAsync(1, dto));
        }

        [Fact]
        public async Task UpdatePatientAsync_ShouldUpdatePatient_WhenValid()
        {
            var existing = new Patient
            {
                PatientId = 1,
                CreatedDate = DateTime.Now.AddYears(-1)
            };

            var dto = new UpdatePatientDto
            {
                FullName = "Updated",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Email = "updated@test.com",
                PhoneNumber = "9999999999"
            };

            var mappedPatient = new Patient();
            var updatedPatient = new Patient { PatientId = 1 };
            var resultDto = new PatientDto { PatientId = 1 };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);

            _mapperMock.Setup(m => m.Map<Patient>(dto))
                .Returns(mappedPatient);

            _repoMock.Setup(r => r.UpdateAsync(1, It.IsAny<Patient>()))
                .ReturnsAsync(updatedPatient);

            _mapperMock.Setup(m => m.Map<PatientDto>(updatedPatient))
                .Returns(resultDto);

            var result = await _service.UpdatePatientAsync(1, dto);

            Assert.NotNull(result);
            Assert.Equal(1, result.PatientId);
        }

        [Fact]
        public async Task UpdatePatientAsync_ShouldThrow_WhenUpdateFails()
        {
            var existing = new Patient { PatientId = 1 };

            var dto = new UpdatePatientDto
            {
                FullName = "Valid",
                DateOfBirth = DateTime.Today.AddYears(-20),
                Email = "valid@test.com",
                PhoneNumber = "1234567890"
            };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);

            _mapperMock.Setup(m => m.Map<Patient>(dto))
                .Returns(new Patient());

            _repoMock.Setup(r => r.UpdateAsync(1, It.IsAny<Patient>()))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.UpdatePatientAsync(1, dto));
        }
    }
}