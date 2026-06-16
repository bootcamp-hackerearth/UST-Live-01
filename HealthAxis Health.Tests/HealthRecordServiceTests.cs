using AutoMapper;
using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.Shared.DTOs.HealthRecordDtos;
using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.API.Exceptions;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Repositories.Interfaces;
using HealthAxisHealth.API.Services.Implementations;
using HealthAxisHealth.API.UnitOfWork;
using Moq;
using Xunit;

namespace HealthAxisHealth.API.Tests.Services
{
    public class HealthRecordServiceTests
    {
        #region Fields

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IHealthRecordRepository> _healthRecordRepositoryMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly HealthRecordService _service;

        #endregion

        #region Constructor

        public HealthRecordServiceTests()
        {
            _unitOfWorkMock =
                new Mock<IUnitOfWork>();

            _healthRecordRepositoryMock =
                new Mock<IHealthRecordRepository>();

            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _unitOfWorkMock
                .Setup(x => x.HealthRecords)
                .Returns(_healthRecordRepositoryMock.Object);

            _unitOfWorkMock
                .Setup(x => x.Appointments)
                .Returns(_appointmentRepositoryMock.Object);

            _service =
                new HealthRecordService(
                    _unitOfWorkMock.Object,
                    _mapperMock.Object);
        }

        #endregion

        #region GetPagedAsync

        [Fact]
        public async Task GetPagedAsync_ShouldReturnPagedHealthRecords()
        {
            // Arrange
            var pagination = new PaginationParams();

            var records = new List<HealthRecord>
            {
                new HealthRecord
                {
                    RecordId = 1
                }
            };

            var dtoList = new List<HealthRecordDto>
            {
                new HealthRecordDto
                {
                    RecordId = 1
                }
            };

            var pagedResult =
                new PagedResultDto<HealthRecord>
                {
                    Items = records,
                    PageNumber = 1,
                    PageSize = 10,
                    TotalRecords = 1
                };

            _healthRecordRepositoryMock
                .Setup(x => x.GetPagedAsync(pagination))
                .ReturnsAsync(pagedResult);

            _mapperMock
                .Setup(x => x.Map<List<HealthRecordDto>>(records))
                .Returns(dtoList);

            // Act
            var result =
                await _service.GetPagedAsync(
                    pagination);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalRecords);
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenRecordNotFound()
        {
            // Arrange
            _healthRecordRepositoryMock
                .Setup(x => x.GetHealthRecordWithDetailsAsync(1))
                .ReturnsAsync((HealthRecord?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetByIdAsync(1));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRecord()
        {
            // Arrange
            var record = new HealthRecord
            {
                RecordId = 1
            };

            var dto = new HealthRecordDto
            {
                RecordId = 1
            };

            _healthRecordRepositoryMock
                .Setup(x => x.GetHealthRecordWithDetailsAsync(1))
                .ReturnsAsync(record);

            _mapperMock
                .Setup(x => x.Map<HealthRecordDto>(record))
                .Returns(dto);

            // Act
            var result =
                await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result!.RecordId);
        }

        #endregion

        #region GetByPatientIdAsync

        [Fact]
        public async Task GetByPatientIdAsync_ShouldReturnRecords()
        {
            // Arrange
            var records = new List<HealthRecord>
            {
                new HealthRecord { RecordId = 1 }
            };

            var dtoList = new List<HealthRecordDto>
            {
                new HealthRecordDto { RecordId = 1 }
            };

            _healthRecordRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(1))
                .ReturnsAsync(records);

            _mapperMock
                .Setup(x => x.Map<List<HealthRecordDto>>(records))
                .Returns(dtoList);

            // Act
            var result =
                await _service.GetByPatientIdAsync(1);

            // Assert
            Assert.Single(result);
        }

        #endregion

        #region GetByDoctorIdAsync

        [Fact]
        public async Task GetByDoctorIdAsync_ShouldReturnRecords()
        {
            // Arrange
            var records = new List<HealthRecord>
            {
                new HealthRecord { RecordId = 1 }
            };

            var dtoList = new List<HealthRecordDto>
            {
                new HealthRecordDto { RecordId = 1 }
            };

            _healthRecordRepositoryMock
                .Setup(x => x.GetByDoctorIdAsync(1))
                .ReturnsAsync(records);

            _mapperMock
                .Setup(x => x.Map<List<HealthRecordDto>>(records))
                .Returns(dtoList);

            // Act
            var result =
                await _service.GetByDoctorIdAsync(1);

            // Assert
            Assert.Single(result);
        }

        #endregion
        #region CreateAsync

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenAppointmentNotFound()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync((Appointment?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenHealthRecordAlreadyExists()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Confirmed
                });

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(new HealthRecord());

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenAppointmentCancelled()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Cancelled
                });

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenAppointmentNotConfirmed()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Pending
                });

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenAppointmentDateIsFuture()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Confirmed,
                    ScheduledDate = DateTime.Today.AddDays(1)
                });

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateHealthRecordSuccessfully()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest"
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 100,
                DoctorId = 200,
                Status = AppointmentStatus.Confirmed,
                ScheduledDate = DateTime.Today
            };

            var healthRecord = new HealthRecord
            {
                RecordId = 10
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord?)null);

            _mapperMock
                .Setup(x => x.Map<HealthRecord>(dto))
                .Returns(healthRecord);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            Assert.Equal(10, result);

            _healthRecordRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<HealthRecord>()),
                Times.Once);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Appointment>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        #endregion
    }
}
