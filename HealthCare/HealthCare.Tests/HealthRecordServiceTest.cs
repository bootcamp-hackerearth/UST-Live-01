using AutoMapper;
using Healthcare.Shared.DTOs;
using Healthcare.Shared.DTOs.HealthRecord;
using HealthCare.Api.Data;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using Xunit;

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
                _context);
        }

        // Add

        [Fact]
        public async Task AddAsync_ShouldAddHealthRecord()
        {
            _context.Appointments.Add(new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                TimeSlot = "10:00 AM"
            });

            await _context.SaveChangesAsync();

            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1,
                PatientId = 1,
                VisitDate = DateTime.Now
            };

            var record = new HealthRecord
            {
                Diagnosis = "Fever",
                Prescription = "Paracetamol"
            };

            _mapperMock
                .Setup(m => m.Map<HealthRecord>(dto))
                .Returns(record);

            await _service.AddAsync(dto, 10);

            Assert.Single(_context.HealthRecords);
            Assert.Equal(10, record.DoctorId);
            Assert.Equal(1, record.PatientId);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenAppointmentNotFound()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 99,
                PatientId = 1,
                VisitDate = DateTime.Now
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _service.AddAsync(dto, 10));
        }

        // Update

        [Fact]
        public async Task UpdateAsync_ShouldUpdateRecord()
        {
            var record = new HealthRecord();

            _repoMock.Setup(r => r.GetProfileAsync(1))
                .ReturnsAsync(record);

            await _service.UpdateAsync(1, new UpdateHealthRecordDto());

            _repoMock.Verify(r => r.UpdateAsync(record), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenRecordNotFound()
        {
            _repoMock.Setup(r => r.GetProfileAsync(1))
                .ReturnsAsync((HealthRecord?)null);

            await Assert.ThrowsAsync<HealthRecordNotFoundException>(() =>
                _service.UpdateAsync(1, new UpdateHealthRecordDto()));
        }

        // Delete

        [Fact]
        public async Task DeleteAsync_ShouldDeleteRecord()
        {
            var record = new HealthRecord();

            _repoMock.Setup(r => r.GetProfileAsync(1))
                .ReturnsAsync(record);

            await _service.DeleteAsync(1);

            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenRecordNotFound()
        {
            _repoMock.Setup(r => r.GetProfileAsync(1))
                .ReturnsAsync((HealthRecord?)null);

            await Assert.ThrowsAsync<HealthRecordNotFoundException>(() =>
                _service.DeleteAsync(1));
        }

        // GetById

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRecord()
        {
            var record = new HealthRecord();
            var dto = new HealthRecordListDto();

            _repoMock.Setup(r => r.GetProfileAsync(1))
                .ReturnsAsync(record);

            _mapperMock.Setup(m => m.Map<HealthRecordListDto?>(record))
                .Returns(dto);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenRecordNotFound()
        {
            _repoMock.Setup(r => r.GetProfileAsync(1))
                .ReturnsAsync((HealthRecord?)null);

            var result = await _service.GetByIdAsync(1);

            Assert.Null(result);
        }

        // GetAll

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedRecords()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord()
            };

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
                    It.IsAny<Func<IQueryable<HealthRecord>,
                    IOrderedQueryable<HealthRecord>>>()))
                .ReturnsAsync(paged);

            _mapperMock.Setup(m =>
                    m.Map<IEnumerable<HealthRecordListDto>>(records))
                .Returns(new List<HealthRecordListDto>
                {
                    new HealthRecordListDto()
                });

            var result = await _service.GetAllAsync(new HealthRecordFilter());

            Assert.NotNull(result);
            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task GetAllAsync_ShouldApplyVisitDateFilter()
        {
            var filter = new HealthRecordFilter
            {
                VisitDate = DateOnly.FromDateTime(DateTime.Today)
            };

            var paged = new PagedResult<HealthRecord>
            {
                Items = new List<HealthRecord>(),
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 0
            };

            _repoMock.Setup(r => r.GetAllAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Expression<Func<HealthRecord, bool>>>(),
                    It.IsAny<Func<IQueryable<HealthRecord>,
                    IOrderedQueryable<HealthRecord>>>()))
                .ReturnsAsync(paged);

            var result = await _service.GetAllAsync(filter);

            Assert.NotNull(result);

            _repoMock.Verify(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<HealthRecord, bool>>>(),
                It.IsAny<Func<IQueryable<HealthRecord>,
                IOrderedQueryable<HealthRecord>>>()),
                Times.Once);
        }

        // GetHealthRecordByPatient

        [Fact]
        public async Task GetHealthRecordByPatient_ShouldReturnRecords()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord()
            };

            var dto = new List<HealthRecordListDto>
            {
                new HealthRecordListDto()
            };

            _repoMock.Setup(r => r.GetHealthRecordByPatient(1))
                .ReturnsAsync(records);

            _mapperMock.Setup(m =>
                m.Map<List<HealthRecordListDto>>(records))
                .Returns(dto);

            var result = await _service.GetHealthRecordByPatient(1);

            Assert.Single(result);
        }

        // GetHealthRecordByAppointment

        [Fact]
        public async Task GetHealthRecordByAppointment_ShouldReturnRecords()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord()
            };

            var dto = new List<HealthRecordListDto>
            {
                new HealthRecordListDto()
            };

            _repoMock.Setup(r => r.GetHealthRecordByAppointment(1))
                .ReturnsAsync(records);

            _mapperMock.Setup(m =>
                m.Map<List<HealthRecordListDto>>(records))
                .Returns(dto);

            var result = await _service.GetHealthRecordByAppointment(1);

            Assert.Single(result);
        }
    }
}