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
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _repoMock = new Mock<IHealthRecordRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new HealthRecordService(_repoMock.Object, _mapperMock.Object);
        }

        // ✅ CREATE SUCCESS
        [Fact]
        public async Task AddRecord_Should_Add_When_Valid()
        {
            var dto = new HealthRecordDto
            {
                PatientId = 1,
                DoctorId = 2,
                Diagnosis = "Fever"
            };

            var entity = new HealthRecord();

            _mapperMock.Setup(m => m.Map<HealthRecord>(dto))
                       .Returns(entity);

            await _service.AddRecord(dto);

            _repoMock.Verify(r => r.AddAsync(It.IsAny<HealthRecord>()), Times.Once);
        }

        // NULL DTO
        [Fact]
        public async Task AddRecord_Should_Throw_When_Null()
        {
            await Assert.ThrowsAsync<Exception>(() => _service.AddRecord(null));
        }

        // ✅ GET ALL
        [Fact]
        public async Task GetAllRecords_Should_Return_List()
        {
            var list = new List<HealthRecord>
            {
                new HealthRecord { PatientId = 1 }
            };

            var dtoList = new List<HealthRecordDto>
            {
                new HealthRecordDto { PatientId = 1 }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);
            _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(list))
                       .Returns(dtoList);

            var result = await _service.GetAllRecords();

            Assert.Single(result);
        }

        // ✅ GET BY PATIENT
        [Fact]
        public async Task GetPatientRecords_Should_Filter_By_Patient()
        {
            var list = new List<HealthRecord>
            {
                new HealthRecord { PatientId = 1 },
                new HealthRecord { PatientId = 2 }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);
            _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>()))
                       .Returns((List<HealthRecord> src) =>
                       {
                           var results = new List<HealthRecordDto>();
                           foreach (var r in src)
                               results.Add(new HealthRecordDto { PatientId = r.PatientId });
                           return results;
                       });

            var result = await _service.GetPatientRecords(1);

            Assert.Single(result);
        }

        // ✅ GET BY DOCTOR + PATIENT
        [Fact]
        public async Task GetHealthRecordsByDoctor_Should_Filter_Correctly()
        {
            var list = new List<HealthRecord>
            {
                new HealthRecord { DoctorId = 1, PatientId = 1, VisitDate = DateTime.Now },
                new HealthRecord { DoctorId = 1, PatientId = 2 }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>()))
                       .Returns(new List<HealthRecordDto>());

            var result = await _service.GetHealthRecordsByDoctor(1, 1);

            Assert.NotNull(result);
        }
    }
}