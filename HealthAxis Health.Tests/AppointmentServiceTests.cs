using AutoMapper;
using HealthAxisHealth.API.DTOs.AppointmentDtos;
using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.Enums;
using HealthAxisHealth.API.Exceptions;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Repositories.Interfaces;
using HealthAxisHealth.API.Services.Implementations;
using HealthAxisHealth.API.UnitOfWork;
using Moq;
using Xunit;

namespace HealthAxisHealth.Tests.Services
{
    public class AppointmentServiceTests
    {
        #region Fields

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;

        private readonly AppointmentService _service;

        #endregion

        #region Constructor

        public AppointmentServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _patientRepositoryMock =
                new Mock<IPatientRepository>();

            _doctorRepositoryMock =
                new Mock<IDoctorRepository>();

            _unitOfWorkMock
                .Setup(x => x.Appointments)
                .Returns(_appointmentRepositoryMock.Object);

            _unitOfWorkMock
                .Setup(x => x.Patients)
                .Returns(_patientRepositoryMock.Object);

            _unitOfWorkMock
                .Setup(x => x.Doctors)
                .Returns(_doctorRepositoryMock.Object);

            _service = new AppointmentService(
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        #endregion

        #region GetPagedAsync

        [Fact]
        public async Task GetPagedAsync_ShouldReturnPagedResult()
        {
            // Arrange
            var pagination = new PaginationParams();

            var pagedResult =
                new PagedResultDto<Appointment>
                {
                    Items = new List<Appointment>
                    {
                        new Appointment
                        {
                            AppointmentId = 1
                        }
                    },
                    PageNumber = 1,
                    PageSize = 10,
                    TotalRecords = 1
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetPagedAsync(pagination))
                .ReturnsAsync(pagedResult);

            _mapperMock
                .Setup(x => x.Map<List<AppointmentDto>>(
                    pagedResult.Items))
                .Returns(new List<AppointmentDto>
                {
                    new AppointmentDto
                    {
                        AppointmentId = 1
                    }
                });

            // Act
            var result =
                await _service.GetPagedAsync(
                    pagination);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalRecords);
        }

        #endregion

        #region GetByPatientIdAsync

        [Fact]
        public async Task GetByPatientIdAsync_ShouldReturnAppointments()
        {
            // Arrange
            var appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        AppointmentId = 1
                    }
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(1))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(x => x.Map<List<AppointmentDto>>(
                    appointments))
                .Returns(new List<AppointmentDto>
                {
                    new AppointmentDto
                    {
                        AppointmentId = 1
                    }
                });

            // Act
            var result =
                await _service.GetByPatientIdAsync(1);

            // Assert
            Assert.Single(result);
        }

        #endregion

        #region GetByDoctorIdAsync

        [Fact]
        public async Task GetByDoctorIdAsync_ShouldReturnAppointments()
        {
            // Arrange
            var appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        AppointmentId = 10
                    }
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByDoctorIdAsync(2))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(x => x.Map<List<AppointmentDto>>(
                    appointments))
                .Returns(new List<AppointmentDto>
                {
                    new AppointmentDto
                    {
                        AppointmentId = 10
                    }
                });

            // Act
            var result =
                await _service.GetByDoctorIdAsync(2);

            // Assert
            Assert.Single(result);
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAppointment()
        {
            // Arrange
            var appointment =
                new Appointment
                {
                    AppointmentId = 5
                };

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentWithDetailsAsync(5))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(x =>
                    x.Map<AppointmentDto>(appointment))
                .Returns(new AppointmentDto
                {
                    AppointmentId = 5
                });

            // Act
            var result =
                await _service.GetByIdAsync(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result!.AppointmentId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentWithDetailsAsync(99))
                .ReturnsAsync((Appointment?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetByIdAsync(99));
        }

        #endregion

        #region GetDoctorAppointmentsByDateAsync

        [Fact]
        public async Task GetDoctorAppointmentsByDateAsync_ShouldReturnAppointments()
        {
            // Arrange
            var date = DateTime.Today;

            var appointments =
                new List<Appointment>
                {
                    new Appointment()
                };

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetDoctorAppointmentsByDateAsync(
                        1,
                        date))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(x =>
                    x.Map<List<AppointmentDto>>(
                        appointments))
                .Returns(new List<AppointmentDto>
                {
                    new AppointmentDto()
                });

            // Act
            var result =
                await _service
                    .GetDoctorAppointmentsByDateAsync(
                        1,
                        date);

            // Assert
            Assert.Single(result);
        }

        #endregion
        #region CreateAsync

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenPatientNotFound()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync(1))
                .ReturnsAsync((Patient?)null);

            var dto = new CreateAppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "09:00-09:30"
            };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CreateAsync(1, dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenDoctorNotFound()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync(1))
                .ReturnsAsync(new Patient
                {
                    PatientId = 10
                });

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            var dto = new CreateAppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "09:00-09:30"
            };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CreateAsync(1, dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenDoctorInactive()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync(1))
                .ReturnsAsync(new Patient
                {
                    PatientId = 10
                });

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Doctor
                {
                    DoctorId = 1,
                    IsActive = false
                });

            var dto = new CreateAppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "09:00-09:30"
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.CreateAsync(1, dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenDateIsPast()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(-1),
                TimeSlot = "09:00-09:30"
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.CreateAsync(1, dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenDateBeyondSixMonths()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddMonths(7),
                TimeSlot = "09:00-09:30"
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.CreateAsync(1, dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenTimeSlotInvalid()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = ""
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.CreateAsync(1, dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenSlotUnavailable()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync(1))
                .ReturnsAsync(new Patient
                {
                    PatientId = 10
                });

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Doctor
                {
                    DoctorId = 1,
                    IsActive = true
                });

            _appointmentRepositoryMock
                .Setup(x => x.IsSlotAvailableAsync(
                    1,
                    It.IsAny<DateTime>(),
                    "09:00-09:30"))
                .ReturnsAsync(false);

            var dto = new CreateAppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "09:00-09:30"
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.CreateAsync(1, dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAppointmentSuccessfully()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync(1))
                .ReturnsAsync(new Patient
                {
                    PatientId = 10
                });

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Doctor
                {
                    DoctorId = 1,
                    IsActive = true
                });

            _appointmentRepositoryMock
                .Setup(x => x.IsSlotAvailableAsync(
                    1,
                    It.IsAny<DateTime>(),
                    "09:00-09:30"))
                .ReturnsAsync(true);

            _appointmentRepositoryMock
                .Setup(x => x.AddAsync(
                    It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var dto = new CreateAppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "09:00-09:30"
            };

            // Act
            await _service.CreateAsync(1, dto);

            // Assert
            _appointmentRepositoryMock.Verify(
                x => x.AddAsync(
                    It.IsAny<Appointment>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        #endregion
        #region UpdateStatusAsync

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrow_WhenAppointmentNotFound()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateStatusAsync(
                    1,
                    new UpdateAppointmentStatusDto()));
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrow_WhenAlreadyConfirmed()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Confirmed
            };

            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.UpdateStatusAsync(1, dto));
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrow_WhenAlreadyCancelled()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Cancelled
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Cancelled
            };

            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.UpdateStatusAsync(1, dto));
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrow_WhenAlreadyCompleted()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Completed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Completed
            };

            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.UpdateStatusAsync(1, dto));
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrow_WhenConfirmCancelled()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Cancelled
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Confirmed
            };

            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.UpdateStatusAsync(1, dto));
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrow_WhenCompleteCancelled()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Cancelled
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Completed
            };

            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.UpdateStatusAsync(1, dto));
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrow_WhenCompletePending()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Completed
            };

            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.UpdateStatusAsync(1, dto));
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrow_WhenCancellationReasonMissing()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Cancelled,
                CancellationReason = ""
            };

            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.UpdateStatusAsync(1, dto));
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldConfirmAppointment()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Confirmed
                });

            _appointmentRepositoryMock.Verify(
                x => x.Update(It.IsAny<Appointment>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldCancelAppointment()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = "Patient Request"
                });

            _appointmentRepositoryMock.Verify(
                x => x.Update(It.IsAny<Appointment>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        #endregion
        #region UpdateStatusAsync - Remaining Success Cases

        [Fact]
        public async Task UpdateStatusAsync_ShouldCompleteAppointment()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Completed
            };

            // Act
            await _service.UpdateStatusAsync(1, dto);

            // Assert
            _appointmentRepositoryMock.Verify(
                x => x.Update(It.IsAny<Appointment>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldUpdateDefaultStatus()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Pending
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Pending
            };

            // Act
            await _service.UpdateStatusAsync(1, dto);

            // Assert
            Assert.Equal(
                AppointmentStatus.Pending,
                appointment.Status);

            _appointmentRepositoryMock.Verify(
                x => x.Update(It.IsAny<Appointment>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenAppointmentNotFound()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.DeleteAsync(1));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenAppointmentCompleted()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Completed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.DeleteAsync(1));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenAppointmentConfirmed()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.DeleteAsync(1));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeletePendingAppointment()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Pending
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act
            await _service.DeleteAsync(1);

            // Assert
            _appointmentRepositoryMock.Verify(
                x => x.Delete(appointment),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCancelledAppointment()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Cancelled
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act
            await _service.DeleteAsync(1);

            // Assert
            _appointmentRepositoryMock.Verify(
                x => x.Delete(appointment),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        #endregion
    }
}
