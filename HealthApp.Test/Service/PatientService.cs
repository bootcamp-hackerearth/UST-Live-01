using AutoMapper;
using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.DTOs;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HealthApp.Test.Service
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
            _service = new PatientService(_repoMock.Object, _mapperMock.Object);
        }

        // ✅ REGISTER SUCCESS
        [Fact]
        public async Task RegisterPatient_Should_Add_When_Valid()
        {
            var dto = new PatientDto
            {
                FullName = "John",
                Email = "john@test.com"
            };

            _repoMock.Setup(x => x.GetAllAsync())
                     .ReturnsAsync(new List<Patient>());

            _mapperMock.Setup(m => m.Map<Patient>(dto))
                       .Returns(new Patient { Email = dto.Email });

            await _service.RegisterPatient(dto);

            _repoMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Once);
        }

        // ✅ REGISTER NULL
        [Fact]
        public async Task RegisterPatient_Should_Throw_When_Null()
        {
            await Assert.ThrowsAsync<Exception>(() => _service.RegisterPatient(null));
        }

        // ✅ REGISTER EMPTY NAME
        [Fact]
        public async Task RegisterPatient_Should_Throw_When_Name_Empty()
        {
            var dto = new PatientDto { FullName = "", Email = "a@test.com" };

            await Assert.ThrowsAsync<Exception>(() => _service.RegisterPatient(dto));
        }

        // ✅ REGISTER EMPTY EMAIL
        [Fact]
        public async Task RegisterPatient_Should_Throw_When_Email_Empty()
        {
            var dto = new PatientDto { FullName = "John", Email = "" };

            await Assert.ThrowsAsync<Exception>(() => _service.RegisterPatient(dto));
        }

        // ✅ REGISTER DUPLICATE EMAIL
        [Fact]
        public async Task RegisterPatient_Should_Throw_When_Email_Exists()
        {
            var dto = new PatientDto
            {
                FullName = "John",
                Email = "john@test.com"
            };

            _repoMock.Setup(x => x.GetAllAsync())
                     .ReturnsAsync(new List<Patient>
                     {
                         new Patient { Email = "john@test.com" }
                     });

            await Assert.ThrowsAsync<Exception>(() => _service.RegisterPatient(dto));
        }

        // ✅ GET ALL
        [Fact]
        public async Task GetAll_Should_Return_List()
        {
            var patients = new List<Patient>
            {
                new Patient { PatientId = 1, Email = "a@test.com" }
            };

            var dtoList = new List<PatientDto>
            {
                new PatientDto { Email = "a@test.com" }
            };

            _repoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(patients);
            _mapperMock.Setup(m => m.Map<List<PatientDto>>(patients))
                       .Returns(dtoList);

            var result = await _service.GetAll();

            Assert.Single(result);
        }

        // ✅ GET BY ID SUCCESS
        [Fact]
        public async Task GetPatientById_Should_Return_When_Exists()
        {
            var patient = new Patient { PatientId = 1, Email = "a@test.com" };
            var dto = new PatientDto { Email = "a@test.com" };

            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(patient);
            _mapperMock.Setup(m => m.Map<PatientDto>(patient)).Returns(dto);

            var result = await _service.GetPatientById(1);

            Assert.Equal("a@test.com", result.Email);
        }

        // ✅ GET BY ID NOT FOUND
        [Fact]
        public async Task GetPatientById_Should_Throw_When_NotFound()
        {
            _repoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Patient)null);

            await Assert.ThrowsAsync<Exception>(() => _service.GetPatientById(1));
        }

        // ✅ UPDATE SUCCESS
        [Fact]
        public async Task UpdatePatient_Should_Call_Update_When_Exists()
        {
            var dto = new PatientDto { Email = "test@test.com" };

            _repoMock.Setup(x => x.GetByIdAsync(1))
                     .ReturnsAsync(new Patient());

            _mapperMock.Setup(m => m.Map<Patient>(dto))
                       .Returns(new Patient());

            await _service.UpdatePatientById(1, dto);

            _repoMock.Verify(x => x.UpdatePatientAsync(1, It.IsAny<Patient>()),
                             Times.Once);
        }

        // ✅ UPDATE NOT FOUND
        [Fact]
        public async Task UpdatePatient_Should_Throw_When_NotFound()
        {
            var dto = new PatientDto();

            _repoMock.Setup(x => x.GetByIdAsync(1))
                     .ReturnsAsync((Patient)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.UpdatePatientById(1, dto));
        }
    }
}