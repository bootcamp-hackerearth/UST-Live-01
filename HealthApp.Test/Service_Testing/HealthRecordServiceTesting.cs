using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using HealthApp.Api.Service.Impl;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Model;
using HealthApp.Shared.Dto;
using AutoMapper;
using HealthApp.Api.Exceptions;

namespace HealthApp.Test.Service_Testing
{
    public class HealthRecordServiceTesting
    {
        private readonly Mock<IHealthRecordRepository> _repo;
        private readonly Mock<IPatientRepository> _patientRepo;
        private readonly Mock<IDoctorRepository> _doctorRepo;
        private readonly Mock<IMapper> _mapper;

        private readonly HealthRecordService _service;

        public HealthRecordServiceTesting()
        {
            _repo = new Mock<IHealthRecordRepository>();
            _patientRepo = new Mock<IPatientRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();
            _mapper = new Mock<IMapper>();

            _service = new HealthRecordService(
                _repo.Object,
                _patientRepo.Object,
                _doctorRepo.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task AddRecord_ShouldCreate()
        {
            var dto = new HealthRecordDto
            {
                Diagnosis = "Flu",
                Prescription = "Medicine",
                PatientId = 1,
                DoctorId = 1
            };

            var entity = new HealthRecord { PatientId = 1, DoctorId = 1 };

            _mapper.Setup(x => x.Map<HealthRecord>(dto)).Returns(entity);
            _repo.Setup(x => x.addAsync(entity)).ReturnsAsync(entity);

            _patientRepo.Setup(x => x.getbyidAsync(1)).ReturnsAsync(new Patient());
            _doctorRepo.Setup(x => x.getbyidAsync(1)).ReturnsAsync(new Doctor());

            _mapper.Setup(x => x.Map<HealthRecordDto>(entity)).Returns(dto);

            var result = await _service.AddRecordAsync(dto);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task AddRecord_ShouldThrow_WhenDiagnosisMissing()
        {
            var dto = new HealthRecordDto { Prescription = "Med" };

            await Assert.ThrowsAsync<HealthRecordRuleException>(() =>
                _service.AddRecordAsync(dto));
        }

        [Fact]
        public async Task GetAll_ShouldReturnList()
        {
            var list = new List<HealthRecord>();

            _repo.Setup(x => x.getallAsync()).ReturnsAsync(list);
            _mapper.Setup(x => x.Map<List<HealthRecordDto>>(list))
                .Returns(new List<HealthRecordDto>());

            var result = await _service.GetAllRecordsAsync();

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_ShouldReturnRecord()
        {
            var record = new HealthRecord();

            _repo.Setup(x => x.getbyidAsync(1)).ReturnsAsync(record);
            _mapper.Setup(x => x.Map<HealthRecordDto>(record))
                .Returns(new HealthRecordDto());

            var result = await _service.GetRecordByIdAsync(1);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_ShouldThrow_WhenInvalid()
        {
            await Assert.ThrowsAsync<HealthRecordRuleException>(() =>
                _service.GetRecordByIdAsync(0));
        }

        [Fact]
        public async Task GetById_ShouldThrow_WhenNotFound()
        {
            _repo.Setup(x => x.getbyidAsync(1))
                .ReturnsAsync((HealthRecord)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.GetRecordByIdAsync(1));
        }

        [Fact]
        public async Task GetByDoctorPatient_ShouldReturn()
        {
            var list = new List<HealthRecord>();

            _repo.Setup(x => x.GetHealthRecordsByDoctorAndPatientAsync(1, 1))
                .ReturnsAsync(list);

            _mapper.Setup(x => x.Map<List<HealthRecordDto>>(list))
                .Returns(new List<HealthRecordDto>());

            var result = await _service.GetHealthRecordsByDoctorAsync(1, 1);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByDoctorPatient_ShouldThrow_WhenDoctorInvalid()
        {
            await Assert.ThrowsAsync<HealthRecordRuleException>(() =>
                _service.GetHealthRecordsByDoctorAsync(0, 1));
        }

        [Fact]
        public async Task GetByDoctorPatient_ShouldThrow_WhenPatientInvalid()
        {
            await Assert.ThrowsAsync<HealthRecordRuleException>(() =>
                _service.GetHealthRecordsByDoctorAsync(1, 0));
        }

        [Fact]
        public async Task GetByDoctorPatient_ShouldThrow_WhenNull()
        {
            _repo.Setup(x => x.GetHealthRecordsByDoctorAndPatientAsync(1, 1))
                .ReturnsAsync((List<HealthRecord>)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.GetHealthRecordsByDoctorAsync(1, 1));
        }

        [Fact]
        public async Task GetByUser_ShouldReturn()
        {
            var patient = new Patient { PatientId = 1 };

            _patientRepo.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync(patient);

            _repo.Setup(x => x.GetByPatientIdAsync(1))
                .ReturnsAsync(new List<HealthRecord>());

            _mapper.Setup(x => x.Map<List<HealthRecordDto>>(It.IsAny<object>()))
                .Returns(new List<HealthRecordDto>());

            var result = await _service.GetRecordsByUserAsync("user1");

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByUser_ShouldThrow_WhenNotFound()
        {
            _patientRepo.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.GetRecordsByUserAsync("user1"));
        }

        [Fact]
        public async Task GetByDoctor_ShouldReturn()
        {
            var doctor = new Doctor { DoctorId = 1 };

            _doctorRepo.Setup(x => x.GetByIdentityUserIdAsync("doc1"))
                .ReturnsAsync(doctor);

            _repo.Setup(x => x.GetByDoctorIdAsync(1))
                .ReturnsAsync(new List<HealthRecord>());

            _mapper.Setup(x => x.Map<List<HealthRecordDto>>(It.IsAny<object>()))
                .Returns(new List<HealthRecordDto>());

            var result = await _service.GetRecordsByDoctorAsync("doc1");

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByDoctor_ShouldThrow_WhenNotFound()
        {
            _doctorRepo.Setup(x => x.GetByIdentityUserIdAsync("doc1"))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.GetRecordsByDoctorAsync("doc1"));
        }
    }
}