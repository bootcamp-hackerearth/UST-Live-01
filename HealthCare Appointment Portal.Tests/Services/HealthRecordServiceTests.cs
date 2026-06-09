using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
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
                    null!,
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
                .Setup(x =>
                    x.GetAllRecordsWithDetailsAsync())
                .ReturnsAsync(records);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<HealthRecordDto>>(
                        records))
                .Returns(recordDtos);

            var result =
                await _service
                    .GetAllHealthRecordsAsync();

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_ReturnsRecord()
        {
            var record =
                new HealthRecord();

            var dto =
                new HealthRecordDto();

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(record);

            _mapperMock
                .Setup(x =>
                    x.Map<HealthRecordDto>(
                        record))
                .Returns(dto);

            var result =
                await _service
                    .GetHealthRecordByIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_WhenNotFound_ThrowsException()
        {
            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((HealthRecord)null!);

            await Assert.ThrowsAsync<
                HealthRecordNotFoundException>(
                () => _service
                    .GetHealthRecordByIdAsync(1));
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
                    x.GetRecordsByPatientAsync(1))
                .ReturnsAsync(records);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<HealthRecordDto>>(
                        records))
                .Returns(recordDtos);

            var result =
                await _service
                    .GetRecordsByPatientAsync(1);

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
                    x.GetRecordsByDoctorAsync(1))
                .ReturnsAsync(records);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<HealthRecordDto>>(
                        records))
                .Returns(recordDtos);

            var result =
                await _service
                    .GetRecordsByDoctorAsync(1);

            Assert.Single(result);
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentNotFound_ThrowsException()
        {
            var dto =
                new CreateHealthRecordDto
                {
                    AppointmentId = 1
                };

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync((Appointment)null!);

            await Assert.ThrowsAsync<
                AppointmentNotFoundException>(
                () => _service
                    .AddHealthRecordAsync(dto));
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentNotCompleted_ThrowsException()
        {
            var dto =
                new CreateHealthRecordDto
                {
                    AppointmentId = 1
                };

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(
                    new Appointment
                    {
                        Status =
                            AppointmentStatus.Confirmed
                    });

            await Assert.ThrowsAsync<
                InvalidAppointmentStatusException>(
                () => _service
                    .AddHealthRecordAsync(dto));
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenRecordAlreadyExists_ThrowsException()
        {
            var dto =
                new CreateHealthRecordDto
                {
                    AppointmentId = 1
                };

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(
                    new Appointment
                    {
                        Status =
                            AppointmentStatus.Completed
                    });

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.RecordExistsAsync(1))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<
                DuplicateHealthRecordException>(
                () => _service
                    .AddHealthRecordAsync(dto));
        }

        [Fact]
        public async Task AddHealthRecordAsync_ReturnsRecordId()
        {
            var dto =
                new CreateHealthRecordDto
                {
                    AppointmentId = 1
                };

            var record =
                new HealthRecord
                {
                    RecordId = 1
                };

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(
                    new Appointment
                    {
                        Status =
                            AppointmentStatus.Completed
                    });

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.RecordExistsAsync(1))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(x =>
                    x.Map<HealthRecord>(dto))
                .Returns(record);

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.AddAsync(record))
                .Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<
                NullReferenceException>(
                () => _service
                    .AddHealthRecordAsync(dto));

            _healthRecordRepositoryMock.Verify(
                x => x.AddAsync(record),
                Times.Once);
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenRecordNotFound_ThrowsException()
        {
            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((HealthRecord)null!);

            await Assert.ThrowsAsync<
                HealthRecordNotFoundException>(
                () => _service
                    .UpdateHealthRecordAsync(
                        1,
                        new UpdateHealthRecordDto()));
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_UpdatesSuccessfully()
        {
            var record =
                new HealthRecord();

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(record);

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.UpdateAsync(record))
                .Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<
                NullReferenceException>(
                () => _service
                    .UpdateHealthRecordAsync(
                        1,
                        new UpdateHealthRecordDto()));

            _healthRecordRepositoryMock.Verify(
                x => x.UpdateAsync(record),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map(
                    It.IsAny<UpdateHealthRecordDto>(),
                    record),
                Times.Once);
        }
        [Fact]
        public async Task DeleteHealthRecordAsync_WhenRecordNotFound_ThrowsException()
        {
            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((HealthRecord)null!);

            await Assert.ThrowsAsync<
                HealthRecordNotFoundException>(
                () => _service
                    .DeleteHealthRecordAsync(1));
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_DeletesSuccessfully()
        {
            var record =
                new HealthRecord();

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(record);

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<
                NullReferenceException>(
                () => _service
                    .DeleteHealthRecordAsync(1));

            _healthRecordRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }
    }
}