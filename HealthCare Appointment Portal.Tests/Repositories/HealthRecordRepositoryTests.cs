using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Repositories;
using HealthCare_Appointment_Portal.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Repositories
{
    public class HealthRecordRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _contextMock;
        private readonly Mock<DbSet<HealthRecord>> _healthRecordDbSetMock;
        private readonly HealthRecordRepository _repository;

        public HealthRecordRepositoryTests()
        {
            _contextMock =
                new Mock<ApplicationDbContext>();

            _healthRecordDbSetMock =
                new Mock<DbSet<HealthRecord>>();

            _contextMock
                .Setup(c => c.HealthRecords)
                .Returns(_healthRecordDbSetMock.Object);

            _repository =
                new HealthRecordRepository(
                    _contextMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsRecord()
        {
            var record =
                new HealthRecord
                {
                    RecordId = 1
                };

            _healthRecordDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync(record);

            var result =
                await _repository.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.RecordId);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull()
        {
            _healthRecordDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync((HealthRecord)null!);

            var result =
                await _repository.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsRecords()
        {
            var data =
                new List<HealthRecord>
                {
                    new HealthRecord
                    {
                        RecordId = 1
                    },
                    new HealthRecord
                    {
                        RecordId = 2
                    }
                };

            var mockSet =
                DbSetMockHelper
                    .CreateMockDbSet(data);

            _contextMock
                .Setup(c => c.HealthRecords)
                .Returns(mockSet.Object);

            var repository =
                new HealthRecordRepository(
                    _contextMock.Object);

            var result =
                await repository.GetAllAsync();

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsRecord()
        {
            var record =
                new HealthRecord();

            await _repository.AddAsync(
                record);

            _healthRecordDbSetMock.Verify(
                d => d.Add(record),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_RemovesRecord()
        {
            var record =
                new HealthRecord
                {
                    RecordId = 1
                };

            _healthRecordDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync(record);

            await _repository.DeleteAsync(1);

            _healthRecordDbSetMock.Verify(
                d => d.Remove(record),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenNotFound_DoesNothing()
        {
            _healthRecordDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync((HealthRecord)null!);

            await _repository.DeleteAsync(1);

            _healthRecordDbSetMock.Verify(
                d => d.Remove(
                    It.IsAny<HealthRecord>()),
                Times.Never);
        }

        [Fact]
        public async Task RecordExistsAsync_ReturnsTrue()
        {
            var data =
                new List<HealthRecord>
                {
                    new HealthRecord
                    {
                        AppointmentId = 10
                    }
                };

            var mockSet =
                DbSetMockHelper
                    .CreateMockDbSet(data);

            _contextMock
                .Setup(c => c.HealthRecords)
                .Returns(mockSet.Object);

            var repository =
                new HealthRecordRepository(
                    _contextMock.Object);

            var result =
                await repository
                    .RecordExistsAsync(10);

            Assert.True(result);
        }

        [Fact]
        public async Task RecordExistsAsync_ReturnsFalse()
        {
            var data =
                new List<HealthRecord>();

            var mockSet =
                DbSetMockHelper
                    .CreateMockDbSet(data);

            _contextMock
                .Setup(c => c.HealthRecords)
                .Returns(mockSet.Object);

            var repository =
                new HealthRecordRepository(
                    _contextMock.Object);

            var result =
                await repository
                    .RecordExistsAsync(10);

            Assert.False(result);
        }

        [Fact]
        public async Task GetRecordedAppointmentIdsAsync_ReturnsIds()
        {
            var data =
                new List<HealthRecord>
                {
                    new HealthRecord
                    {
                        AppointmentId = 10
                    },
                    new HealthRecord
                    {
                        AppointmentId = 20
                    }
                };

            var mockSet =
                DbSetMockHelper
                    .CreateMockDbSet(data);

            _contextMock
                .Setup(c => c.HealthRecords)
                .Returns(mockSet.Object);

            var repository =
                new HealthRecordRepository(
                    _contextMock.Object);

            var result =
                await repository
                    .GetRecordedAppointmentIdsAsync();

            Assert.Equal(
                2,
                result.Count());
        }
    }
}