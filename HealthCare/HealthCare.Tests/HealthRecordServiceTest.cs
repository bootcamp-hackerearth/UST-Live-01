using Xunit;
using Moq;
using AutoMapper;
using HealthCare.Api.Models;
using HealthCare.Api.Data;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Repositories.Interfaces;
using Healthcare.Shared.DTOs;
using Healthcare.Shared.DTOs.HealthRecord;
using HealthCare.Api.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HealthCare.Api.Tests
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthCareDbContext _context;
        private readonly HealthRecordService _service;
        public HealthRecordServiceTests()
        {
            _repoMock = new Mock<IHealthRecordRepository>();
            _mapperMock = new Mock<IMapper>();

            var options = new DbContextOptionsBuilder<HealthCareDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new HealthCareDbContext(options);

            _service = new HealthRecordService(
                _repoMock.Object,
                _mapperMock.Object,
                _context
            );
        }

        //  Add
        //[Fact]
        //public async Task AddAsync_ShouldAddHealthRecord()
        //{
        //    var dto = new CreateHealthRecordDto();
        //    var record = new HealthRecord();

        //    _mapperMock.Setup(m => m.Map<HealthRecord>(dto)).Returns(record);

        //    await _service.AddAsync(dto);

        //    _repoMock.Verify(r => r.AddAsync(record), Times.Once);
        //}

        //  Update
        [Fact]
        public async Task UpdateAsync_ShouldUpdateRecord()
        {
            var record = new HealthRecord();

            _repoMock.Setup(r => r.GetProfileAsync(1)).ReturnsAsync(record);

            await _service.UpdateAsync(1, new UpdateHealthRecordDto());

            _repoMock.Verify(r => r.UpdateAsync(record), Times.Once);
        }

        //  Update Not Found
        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetProfileAsync(1))
                .ReturnsAsync((HealthRecord)null);

            await Assert.ThrowsAsync<HealthRecordNotFoundException>(() =>
                _service.UpdateAsync(1, new UpdateHealthRecordDto()));
        }

        //  Delete
        [Fact]
        public async Task DeleteAsync_ShouldDeleteRecord()
        {
            var record = new HealthRecord();

            _repoMock.Setup(r => r.GetProfileAsync(1)).ReturnsAsync(record);

            await _service.DeleteAsync(1);

            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        //  Delete Not Found
        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetProfileAsync(1))
                .ReturnsAsync((HealthRecord)null);

            await Assert.ThrowsAsync<HealthRecordNotFoundException>(() =>
                _service.DeleteAsync(1));
        }

        //  GetById
        [Fact]
        public async Task GetByIdAsync_ShouldReturnRecord()
        {
            var record = new HealthRecord();
            var dto = new HealthRecordListDto();

            _repoMock.Setup(r => r.GetProfileAsync(1)).ReturnsAsync(record);
            _mapperMock.Setup(m => m.Map<HealthRecordListDto>(record)).Returns(dto);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
        }

        //  GetAll
        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedRecords()
        {
            var records = new List<HealthRecord> { new HealthRecord() };

            var paged = new PagedResult<HealthRecord>
            {
                Items = records,
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 1
            };

            _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<HealthRecord, bool>>>(),
                It.IsAny<Func<IQueryable<HealthRecord>, IOrderedQueryable<HealthRecord>>>()))
                .ReturnsAsync(paged);

            _mapperMock.Setup(m => m.Map<IEnumerable<HealthRecordListDto>>(records))
                .Returns(new List<HealthRecordListDto> { new HealthRecordListDto() });

            var result = await _service.GetAllAsync(new HealthRecordFilter());

            Assert.NotNull(result);
            Assert.Equal(1, result.TotalCount);
        }

        //  GetHealthRecordByPatient
        [Fact]
        public async Task GetHealthRecordByPatient_ShouldReturnRecords()
        {
            var records = new List<HealthRecord> { new HealthRecord() };

            _repoMock.Setup(r => r.GetHealthRecordByPatient(1))
                .ReturnsAsync(records);

            _mapperMock.Setup(m => m.Map<List<HealthRecordListDto>>(records))
                .Returns(new List<HealthRecordListDto> { new HealthRecordListDto() });

            var result = await _service.GetHealthRecordByPatient(1);

            Assert.Single(result);
        }

        //  GetHealthRecordByAppointment
        [Fact]
        public async Task GetHealthRecordByAppointment_ShouldReturnRecords()
        {
            var records = new List<HealthRecord> { new HealthRecord() };

            _repoMock.Setup(r => r.GetHealthRecordByAppointment(1))
                .ReturnsAsync(records);

            _mapperMock.Setup(m => m.Map<List<HealthRecordListDto>>(records))
                .Returns(new List<HealthRecordListDto> { new HealthRecordListDto() });

            var result = await _service.GetHealthRecordByAppointment(1);

            Assert.Single(result);
        }
    }
}