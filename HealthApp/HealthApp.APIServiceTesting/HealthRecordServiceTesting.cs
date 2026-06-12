using Xunit;
using Moq;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthApp.API.Service.Impl;
using HealthApp.API.Repository.Interface;
using HealthApp.Shared.DTOs;
using HealthApp.API.Data;

namespace HealthApp.APIServiceTesting
{
    public class HealthRecordServiceTesting
    {
        private readonly Mock<IHealthRecordRepository> _repo;
        private readonly Mock<IMapper> _mapper;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTesting()
        {
            _repo = new Mock<IHealthRecordRepository>();
            _mapper = new Mock<IMapper>();

            _service = new HealthRecordService(_repo.Object, _mapper.Object);
        }

        // -------------------- ADD RECORD --------------------

        [Fact]
        public async Task AddRecord_ShouldThrow_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<Exception>(() =>
                _service.AddRecord(null));
        }

        [Fact]
        public async Task AddRecord_ShouldAdd_WhenValid()
        {
            var dto = new HealthRecordDto();

            var record = new HealthRecord();

            _mapper.Setup(x => x.Map<HealthRecord>(dto))
                   .Returns(record);

            await _service.AddRecord(dto);

            _repo.Verify(x => x.AddAsync(It.IsAny<HealthRecord>()), Times.Once);
        }

        // -------------------- GET ALL --------------------

        [Fact]
        public async Task GetAllRecords_ShouldReturnList()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord(),
                new HealthRecord()
            };

            var dtoList = new List<HealthRecordDto>
            {
                new HealthRecordDto(),
                new HealthRecordDto()
            };

            _repo.Setup(x => x.GetAllAsync())
                 .ReturnsAsync(records);

            _mapper.Setup(x => x.Map<List<HealthRecordDto>>(records))
                   .Returns(dtoList);

            var result = await _service.GetAllRecords();

            Assert.Equal(2, result.Count);
        }

        // -------------------- GET PATIENT RECORDS --------------------

        [Fact]
        public async Task GetPatientRecords_ShouldReturnFilteredRecords()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord { PatientId = 1 },
                new HealthRecord { PatientId = 2 }
            };

            _repo.Setup(x => x.GetAllAsync())
                 .ReturnsAsync(records);

            _mapper.Setup(x => x.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>()))
                   .Returns(new List<HealthRecordDto> { new HealthRecordDto() });

            var result = await _service.GetPatientRecords(1);

            Assert.Single(result);
        }

        // -------------------- GET BY DOCTOR + PATIENT FILTER --------------------
               
        [Fact]
        public async Task GetHealthRecordsByDoctor_ShouldFilterByDoctor()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord { DoctorId = 1, VisitDate = DateTime.Today },
                new HealthRecord { DoctorId = 2, VisitDate = DateTime.Today }
            };

            _repo.Setup(x => x.GetAllAsync())
                 .ReturnsAsync(records);

            _mapper.Setup(x => x.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>()))
                   .Returns(new List<HealthRecordDto> { new HealthRecordDto() });

            var result = await _service.GetHealthRecordsByDoctor(1, null);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetHealthRecordsByDoctor_ShouldFilterByPatient()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord { PatientId = 1, VisitDate = DateTime.Today },
                new HealthRecord { PatientId = 2, VisitDate = DateTime.Today }
            };

            _repo.Setup(x => x.GetAllAsync())
                 .ReturnsAsync(records);

            _mapper.Setup(x => x.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>()))
                   .Returns(new List<HealthRecordDto> { new HealthRecordDto() });

            var result = await _service.GetHealthRecordsByDoctor(null, 1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetHealthRecordsByDoctor_ShouldFilterByDoctorAndPatient()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord { DoctorId = 1, PatientId = 1, VisitDate = DateTime.Today },
                new HealthRecord { DoctorId = 1, PatientId = 2, VisitDate = DateTime.Today }
            };

            _repo.Setup(x => x.GetAllAsync())
                 .ReturnsAsync(records);

            _mapper.Setup(x => x.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>()))
                   .Returns(new List<HealthRecordDto> { new HealthRecordDto() });

            var result = await _service.GetHealthRecordsByDoctor(1, 1);

            Assert.Single(result);
        }
    }
}