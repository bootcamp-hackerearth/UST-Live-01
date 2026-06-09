using AutoMapper;
using Moq;
using Xunit;

using HealthCare_Appointment_Portal.Services;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;

using System.Collections.Generic;
using System.Threading.Tasks;

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
                new HealthRecord { RecordId = 1 }
            };

            var dtos = new List<HealthRecordDto>
            {
                new HealthRecordDto { RecordId = 1 }
            };

            _unitOfWorkMock
                .Setup(x => x.HealthRecords.GetAllRecordsWithDetailsAsync())
                .ReturnsAsync(records);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(dtos);

            var result = await _service.GetAllHealthRecordsAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_ValidId_ReturnsRecord()
        {
            var record = new HealthRecord { RecordId = 1 };

            var dto = new HealthRecordDto { RecordId = 1 };

            _unitOfWorkMock
                .Setup(x => x.HealthRecords.GetByIdAsync(1))
                .ReturnsAsync(record);

            _mapperMock
                .Setup(x => x.Map<HealthRecordDto>(record))
                .Returns(dto);

            var result = await _service.GetHealthRecordByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.RecordId);
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_InvalidId_ThrowsException()
        {
            _unitOfWorkMock
                .Setup(x => x.HealthRecords.GetByIdAsync(1))
                .ReturnsAsync((HealthRecord)null);

            await Assert.ThrowsAsync<HealthRecordNotFoundException>(
                () => _service.GetHealthRecordByIdAsync(1));
        }

        [Fact]
        public async Task GetRecordsByPatientAsync_ReturnsRecords()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord { RecordId = 1 }
            };

            _unitOfWorkMock
                .Setup(x => x.HealthRecords.GetRecordsByPatientAsync(1))
                .ReturnsAsync(records);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(new List<HealthRecordDto>());

            var result = await _service.GetRecordsByPatientAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetRecordsByDoctorAsync_ReturnsRecords()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord { RecordId = 1 }
            };

            _unitOfWorkMock
                .Setup(x => x.HealthRecords.GetRecordsByDoctorAsync(1))
                .ReturnsAsync(records);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(new List<HealthRecordDto>());

            var result = await _service.GetRecordsByDoctorAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddHealthRecordAsync_Valid_ReturnsId()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Completed
            };

            var record = new HealthRecord { RecordId = 1 };

            _unitOfWorkMock
                .Setup(x => x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _unitOfWorkMock
                .Setup(x => x.HealthRecords.RecordExistsAsync(1))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(x => x.Map<HealthRecord>(dto))
                .Returns(record);

            var result = await _service.AddHealthRecordAsync(dto);

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task AddHealthRecordAsync_AppointmentNotFound_ThrowsException()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            _unitOfWorkMock
                .Setup(x => x.Appointments.GetByIdAsync(1))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.AddHealthRecordAsync(dto));
        }

        [Fact]
        public async Task AddHealthRecordAsync_InvalidAppointmentStatus_ThrowsException()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            _unitOfWorkMock
                .Setup(x => x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.AddHealthRecordAsync(dto));
        }

        [Fact]
        public async Task AddHealthRecordAsync_DuplicateRecord_ThrowsException()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            var appointment = new Appointment
            {
                Status = AppointmentStatus.Completed
            };

            _unitOfWorkMock
                .Setup(x => x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _unitOfWorkMock
                .Setup(x => x.HealthRecords.RecordExistsAsync(1))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<DuplicateHealthRecordException>(
                () => _service.AddHealthRecordAsync(dto));
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_Valid_UpdatesSuccessfully()
        {
            var record = new HealthRecord { RecordId = 1 };

            _unitOfWorkMock
                .Setup(x => x.HealthRecords.GetByIdAsync(1))
                .ReturnsAsync(record);

            await _service.UpdateHealthRecordAsync(1, new UpdateHealthRecordDto());

            _unitOfWorkMock.Verify(
                x => x.HealthRecords.UpdateAsync(record),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_NotFound_ThrowsException()
        {
            _unitOfWorkMock
                .Setup(x => x.HealthRecords.GetByIdAsync(1))
                .ReturnsAsync((HealthRecord)null);

            await Assert.ThrowsAsync<HealthRecordNotFoundException>(
                () => _service.UpdateHealthRecordAsync(1, new UpdateHealthRecordDto()));
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_Valid_DeletesSuccessfully()
        {
            var record = new HealthRecord { RecordId = 1 };

            _unitOfWorkMock
                .Setup(x => x.HealthRecords.GetByIdAsync(1))
                .ReturnsAsync(record);

            await _service.DeleteHealthRecordAsync(1);

            _unitOfWorkMock.Verify(
                x => x.HealthRecords.DeleteAsync(1),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_NotFound_ThrowsException()
        {
            _unitOfWorkMock
                .Setup(x => x.HealthRecords.GetByIdAsync(1))
                .ReturnsAsync((HealthRecord)null);

            await Assert.ThrowsAsync<HealthRecordNotFoundException>(
                () => _service.DeleteHealthRecordAsync(1));
        }
    }
}