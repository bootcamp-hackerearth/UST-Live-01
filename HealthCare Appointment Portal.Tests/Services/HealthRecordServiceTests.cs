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
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _service = new HealthRecordService(
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllHealthRecordsAsync_ReturnsRecords()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord(),
                new HealthRecord()
            };

            var recordDtos = new List<HealthRecordDto>
            {
                new HealthRecordDto(),
                new HealthRecordDto()
            };

            _unitOfWorkMock.Setup(x =>
                    x.HealthRecords.GetAllRecordsWithDetailsAsync())
                .ReturnsAsync(records);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(recordDtos);

            var result =
                await _service.GetAllHealthRecordsAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_ReturnsRecord()
        {
            var record = new HealthRecord();

            var dto = new HealthRecordDto();

            _unitOfWorkMock.Setup(x =>
                    x.HealthRecords.GetByIdAsync(1))
                .ReturnsAsync(record);

            _mapperMock.Setup(x =>
                    x.Map<HealthRecordDto>(record))
                .Returns(dto);

            var result =
                await _service.GetHealthRecordByIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_WhenNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.HealthRecords.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((HealthRecord)null!);

            await Assert.ThrowsAsync<HealthRecordNotFoundException>(
                () => _service.GetHealthRecordByIdAsync(1));
        }

        [Fact]
        public async Task GetRecordsByPatientAsync_ReturnsRecords()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord()
            };

            var recordDtos = new List<HealthRecordDto>
            {
                new HealthRecordDto()
            };

            _unitOfWorkMock.Setup(x =>
                    x.HealthRecords.GetRecordsByPatientAsync(1))
                .ReturnsAsync(records);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(recordDtos);

            var result =
                await _service.GetRecordsByPatientAsync(1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetRecordsByDoctorAsync_ReturnsRecords()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord()
            };

            var recordDtos = new List<HealthRecordDto>
            {
                new HealthRecordDto()
            };

            _unitOfWorkMock.Setup(x =>
                    x.HealthRecords.GetRecordsByDoctorAsync(1))
                .ReturnsAsync(records);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(recordDtos);

            var result =
                await _service.GetRecordsByDoctorAsync(1);

            Assert.Single(result);
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentNotFound_ThrowsException()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync((Appointment)null!);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.AddHealthRecordAsync(dto));
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentNotCompleted_ThrowsException()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    Status = AppointmentStatus.Confirmed
                });

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.AddHealthRecordAsync(dto));
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenRecordAlreadyExists_ThrowsException()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    Status = AppointmentStatus.Completed
                });

            _unitOfWorkMock.Setup(x =>
                    x.HealthRecords.RecordExistsAsync(1))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<DuplicateHealthRecordException>(
                () => _service.AddHealthRecordAsync(dto));
        }

        [Fact]
        public async Task AddHealthRecordAsync_ReturnsRecordId()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            var record = new HealthRecord
            {
                RecordId = 1
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    Status = AppointmentStatus.Completed
                });

            _unitOfWorkMock.Setup(x =>
                    x.HealthRecords.RecordExistsAsync(1))
                .ReturnsAsync(false);

            _mapperMock.Setup(x =>
                    x.Map<HealthRecord>(dto))
                .Returns(record);

            var result =
                await _service.AddHealthRecordAsync(dto);

            Assert.Equal(1, result);

            _unitOfWorkMock.Verify(
                x => x.HealthRecords.AddAsync(record),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenRecordNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.HealthRecords.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((HealthRecord)null!);

            await Assert.ThrowsAsync<HealthRecordNotFoundException>(
                () => _service.UpdateHealthRecordAsync(
                    1,
                    new UpdateHealthRecordDto()));
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_UpdatesSuccessfully()
        {
            var record = new HealthRecord();

            _unitOfWorkMock.Setup(x =>
                    x.HealthRecords.GetByIdAsync(1))
                .ReturnsAsync(record);

            await _service.UpdateHealthRecordAsync(
                1,
                new UpdateHealthRecordDto());

            _unitOfWorkMock.Verify(
                x => x.HealthRecords.UpdateAsync(record),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_WhenRecordNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.HealthRecords.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((HealthRecord)null!);

            await Assert.ThrowsAsync<HealthRecordNotFoundException>(
                () => _service.DeleteHealthRecordAsync(1));
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_DeletesSuccessfully()
        {
            var record = new HealthRecord();

            _unitOfWorkMock.Setup(x =>
                    x.HealthRecords.GetByIdAsync(1))
                .ReturnsAsync(record);

            await _service.DeleteHealthRecordAsync(1);

            _unitOfWorkMock.Verify(
                x => x.HealthRecords.DeleteAsync(1),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }
    }
}