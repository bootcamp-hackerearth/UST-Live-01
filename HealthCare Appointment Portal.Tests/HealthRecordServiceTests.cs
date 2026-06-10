using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using HealthCare_Appointment_Portal.Utilities;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository>
            _healthRecordRepositoryMock;

        private readonly Mock<IAppointmentRepository>
            _appointmentRepositoryMock;

        private readonly Mock<IMapper>
            _mapperMock;

        private readonly HealthRecordService
            _service;

        public HealthRecordServiceTests()
        {
            _healthRecordRepositoryMock =
                new Mock<IHealthRecordRepository>();

            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _service =
                new HealthRecordService(
                    _healthRecordRepositoryMock.Object,
                    _appointmentRepositoryMock.Object,
                    _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllHealthRecordsAsync_ReturnsRecords()
        {
            var records =
                new List<HealthRecord>
                {
                    new HealthRecord(),
                    new HealthRecord()
                };

            var recordDtos =
                new List<HealthRecordDto>
                {
                    new HealthRecordDto(),
                    new HealthRecordDto()
                };

            _healthRecordRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(records);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(recordDtos);

            var result =
                await _service.GetAllHealthRecordsAsync();

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_ReturnsRecord()
        {
            var record =
                new HealthRecord
                {
                    RecordId = 1
                };

            var recordDto =
                new HealthRecordDto();

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(record);

            _mapperMock
                .Setup(x =>
                    x.Map<HealthRecordDto>(record))
                .Returns(recordDto);

            var result =
                await _service.GetHealthRecordByIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_WhenNotFound_ThrowsException()
        {
            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((HealthRecord)null!);

            await Assert.ThrowsAsync<
                HealthRecordNotFoundException>(
                () => _service.GetHealthRecordByIdAsync(1));
        }

        [Fact]
        public async Task GetRecordsByPatientAsync_ReturnsRecords()
        {
            var records =
                new List<HealthRecord>
                {
                    new HealthRecord()
                };

            var recordDtos =
                new List<HealthRecordDto>
                {
                    new HealthRecordDto()
                };

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetByPatientAsync(1))
                .ReturnsAsync(records);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(recordDtos);

            var result =
                await _service.GetRecordsByPatientAsync(1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetRecordsByDoctorAsync_ReturnsRecords()
        {
            var records =
                new List<HealthRecord>
                {
                    new HealthRecord()
                };

            var recordDtos =
                new List<HealthRecordDto>
                {
                    new HealthRecordDto()
                };

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetByDoctorAsync(1))
                .ReturnsAsync(records);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(recordDtos);

            var result =
                await _service.GetRecordsByDoctorAsync(1);

            Assert.Single(result);
        }

        [Fact]
        public async Task AddHealthRecordAsync_ReturnsRecordId()
        {
            var dto =
                new CreateHealthRecordDto
                {
                    AppointmentId = 1
                };

            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Completed
                };

            var record =
                new HealthRecord
                {
                    RecordId = 1
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.ExistsByAppointmentAsync(1))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(x =>
                    x.Map<HealthRecord>(dto))
                .Returns(record);

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.AddAsync(record))
                .Returns(Task.CompletedTask);

            var result =
                await _service.AddHealthRecordAsync(dto);

            Assert.Equal(
                1,
                result);

            _healthRecordRepositoryMock.Verify(
                x => x.AddAsync(record),
                Times.Once);
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Appointment)null!);

            await Assert.ThrowsAsync<
                AppointmentNotFoundException>(
                () => _service.AddHealthRecordAsync(
                    new CreateHealthRecordDto()));
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentNotCompleted_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Pending
                };

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<
                InvalidAppointmentStatusException>(
                () => _service.AddHealthRecordAsync(
                    new CreateHealthRecordDto
                    {
                        AppointmentId = 1
                    }));
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenDuplicateExists_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Completed
                };

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.ExistsByAppointmentAsync(1))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<
                DuplicateHealthRecordException>(
                () => _service.AddHealthRecordAsync(
                    new CreateHealthRecordDto
                    {
                        AppointmentId = 1
                    }));
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_UpdatesSuccessfully()
        {
            var record =
                new HealthRecord
                {
                    RecordId = 1
                };

            var dto =
                new UpdateHealthRecordDto();

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(record);

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.UpdateAsync(record))
                .Returns(Task.CompletedTask);

            await _service.UpdateHealthRecordAsync(
                1,
                dto);

            _mapperMock.Verify(
                x => x.Map(
                    dto,
                    record),
                Times.Once);

            _healthRecordRepositoryMock.Verify(
                x => x.UpdateAsync(record),
                Times.Once);
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenNotFound_ThrowsException()
        {
            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync((HealthRecord)null!);

            await Assert.ThrowsAsync<
                HealthRecordNotFoundException>(
                () => _service.UpdateHealthRecordAsync(
                    1,
                    new UpdateHealthRecordDto()));
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_DeletesSuccessfully()
        {
            var record =
                new HealthRecord
                {
                    RecordId = 1
                };

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(record);

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeleteHealthRecordAsync(1);

            _healthRecordRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_WhenNotFound_ThrowsException()
        {
            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync((HealthRecord)null!);

            await Assert.ThrowsAsync<
                HealthRecordNotFoundException>(
                () => _service.DeleteHealthRecordAsync(1));
        }
    }
}