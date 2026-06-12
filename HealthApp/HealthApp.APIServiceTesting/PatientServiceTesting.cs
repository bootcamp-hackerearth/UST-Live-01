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

namespace HealthApp.APIServiceTesting
{
    public class PatientServiceTesting
    {
        private readonly Mock<IPatientRepository> _repo;
        private readonly Mock<IMapper> _mapper;
        private readonly PatientService _service;

        public PatientServiceTesting()
        {
            _repo = new Mock<IPatientRepository>();
            _mapper = new Mock<IMapper>();

            _service = new PatientService(_repo.Object, _mapper.Object);
        }

        // -------------------- REGISTER --------------------

        [Fact]
        public async Task RegisterPatient_ShouldThrow_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<Exception>(() =>
                _service.RegisterPatient(null));
        }

        [Fact]
        public async Task RegisterPatient_ShouldThrow_WhenNameMissing()
        {
            var dto = new PatientDto { Email = "test@mail.com" };

            await Assert.ThrowsAsync<Exception>(() =>
                _service.RegisterPatient(dto));
        }

        [Fact]
        public async Task RegisterPatient_ShouldThrow_WhenEmailMissing()
        {
            var dto = new PatientDto { FullName = "John" };

            await Assert.ThrowsAsync<Exception>(() =>
                _service.RegisterPatient(dto));
        }

        [Fact]
        public async Task RegisterPatient_ShouldThrow_WhenEmailExists()
        {
            var dto = new PatientDto
            {
                FullName = "John",
                Email = "test@mail.com"
            };

            var existingPatients = new List<Patient>
            {
                new Patient { Email = "test@mail.com" }
            };

            _repo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(existingPatients);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.RegisterPatient(dto));
        }

        [Fact]
        public async Task RegisterPatient_ShouldAdd_WhenValid()
        {
            var dto = new PatientDto
            {
                FullName = "John",
                Email = "john@mail.com"
            };

            _repo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            var mappedPatient = new Patient();

            _mapper.Setup(x => x.Map<Patient>(dto))
                .Returns(mappedPatient);

            await _service.RegisterPatient(dto);

            _repo.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Once);
            Assert.NotEqual(default, mappedPatient.CreatedDate);
        }

        // -------------------- GET ALL --------------------

        [Fact]
        public async Task GetAll_ShouldReturnPatientList()
        {
            var patients = new List<Patient>
            {
                new Patient(),
                new Patient()
            };

            var dtoList = new List<PatientDto>
            {
                new PatientDto(),
                new PatientDto()
            };

            _repo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(patients);

            _mapper.Setup(x => x.Map<List<PatientDto>>(patients))
                .Returns(dtoList);

            var result = await _service.GetAll();

            Assert.Equal(2, result.Count);
        }

        // -------------------- GET BY ID --------------------

        [Fact]
        public async Task GetPatientById_ShouldThrow_WhenNotFound()
        {
            _repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.GetPatientById(1));
        }

        [Fact]
        public async Task GetPatientById_ShouldReturnDto_WhenFound()
        {
            var patient = new Patient();
            var dto = new PatientDto();

            _repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _mapper.Setup(x => x.Map<PatientDto>(patient))
                .Returns(dto);

            var result = await _service.GetPatientById(1);

            Assert.NotNull(result);
        }

        // -------------------- UPDATE --------------------

        [Fact]
        public async Task UpdatePatientById_ShouldThrow_WhenNotFound()
        {
            _repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.UpdatePatientById(1, new PatientDto()));
        }

        [Fact]
        public async Task UpdatePatientById_ShouldCallUpdate_WhenValid()
        {
            var dto = new PatientDto { FullName = "John" };

            var existingPatient = new Patient { PatientId = 1 };

            _repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(existingPatient);

            var mappedPatient = new Patient();

            _mapper.Setup(x => x.Map<Patient>(dto))
                .Returns(mappedPatient);

            await _service.UpdatePatientById(1, dto);

            _repo.Verify(x => x.UpdatePatientAsync(1, It.IsAny<Patient>()), Times.Once);
            Assert.Equal(1, mappedPatient.PatientId);
        }
    }
}

