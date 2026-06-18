using AutoMapper;
using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using Moq;

namespace HealthAxis.API.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();

            _appointmentService = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenAppointmentDoesNotExist_ReturnsNull()
        {
            // Arrange
            const int appointmentId = 100;

            AppointmentStatusUpdateDto statusUpdateDto = new()
            {
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            // Act
            AppointmentReadDto? result =
                await _appointmentService.UpdateStatusAsync(
                    appointmentId,
                    statusUpdateDto);

            // Assert
            Assert.Null(result);

            _appointmentRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenStatusIsConfirmed_ConfirmsAppointment()
        {
            // Arrange
            const int appointmentId = 1;

            Appointment appointment = CreateAppointment(
                appointmentId,
                AppointmentStatus.Scheduled);

            AppointmentStatusUpdateDto statusUpdateDto = new()
            {
                Status = AppointmentStatus.Confirmed
            };

            AppointmentReadDto expectedDto = new()
            {
                AppointmentId = appointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.TimeSlot,
                Status = AppointmentStatus.Confirmed,
                CancellationReason = string.Empty
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentReadDto>(appointment))
                .Returns(expectedDto);

            // Act
            AppointmentReadDto? result =
                await _appointmentService.UpdateStatusAsync(
                    appointmentId,
                    statusUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(AppointmentStatus.Confirmed, appointment.Status);
            Assert.Equal(AppointmentStatus.Confirmed, result.Status);

            _appointmentRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenStatusIsCancelled_CancelsAppointmentWithReason()
        {
            // Arrange
            const int appointmentId = 2;
            const string cancellationReason = "Patient requested cancellation.";

            Appointment appointment = CreateAppointment(
                appointmentId,
                AppointmentStatus.Scheduled);

            AppointmentStatusUpdateDto statusUpdateDto = new()
            {
                Status = AppointmentStatus.Cancelled,
                CancellationReason = cancellationReason
            };

            AppointmentReadDto expectedDto = new()
            {
                AppointmentId = appointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.TimeSlot,
                Status = AppointmentStatus.Cancelled,
                CancellationReason = cancellationReason
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentReadDto>(appointment))
                .Returns(expectedDto);

            // Act
            AppointmentReadDto? result =
                await _appointmentService.UpdateStatusAsync(
                    appointmentId,
                    statusUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(AppointmentStatus.Cancelled, appointment.Status);
            Assert.Equal(cancellationReason, appointment.CancellationReason);
            Assert.Equal(AppointmentStatus.Cancelled, result.Status);
            Assert.Equal(cancellationReason, result.CancellationReason);

            _appointmentRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenStatusIsCompleted_CompletesAppointment()
        {
            // Arrange
            const int appointmentId = 3;

            Appointment appointment = CreateAppointment(
                appointmentId,
                AppointmentStatus.Confirmed);

            AppointmentStatusUpdateDto statusUpdateDto = new()
            {
                Status = AppointmentStatus.Completed
            };

            AppointmentReadDto expectedDto = new()
            {
                AppointmentId = appointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.TimeSlot,
                Status = AppointmentStatus.Completed,
                CancellationReason = string.Empty
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentReadDto>(appointment))
                .Returns(expectedDto);

            // Act
            AppointmentReadDto? result =
                await _appointmentService.UpdateStatusAsync(
                    appointmentId,
                    statusUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(AppointmentStatus.Completed, appointment.Status);
            Assert.Equal(AppointmentStatus.Completed, result.Status);

            _appointmentRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenStatusIsScheduled_SetsStatusDirectly()
        {
            // Arrange
            const int appointmentId = 4;

            Appointment appointment = CreateAppointment(
                appointmentId,
                AppointmentStatus.Cancelled);

            appointment.CancellationReason = "Old reason";

            AppointmentStatusUpdateDto statusUpdateDto = new()
            {
                Status = AppointmentStatus.Scheduled
            };

            AppointmentReadDto expectedDto = new()
            {
                AppointmentId = appointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.TimeSlot,
                Status = AppointmentStatus.Scheduled,
                CancellationReason = appointment.CancellationReason
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentReadDto>(appointment))
                .Returns(expectedDto);

            // Act
            AppointmentReadDto? result =
                await _appointmentService.UpdateStatusAsync(
                    appointmentId,
                    statusUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(AppointmentStatus.Scheduled, appointment.Status);
            Assert.Equal(AppointmentStatus.Scheduled, result.Status);

            _appointmentRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAppointmentReportAsync_WhenAppointmentsExist_ReturnsGroupedReportByDate()
        {
            // Arrange
            DateTime firstDate = new(2026, 6, 14);
            DateTime secondDate = new(2026, 6, 15);

            List<Appointment> appointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = firstDate,
                    TimeSlot = "09:00-09:30",
                    Status = AppointmentStatus.Scheduled
                },
                new Appointment
                {
                    AppointmentId = 2,
                    PatientId = 2,
                    DoctorId = 1,
                    ScheduledDate = firstDate,
                    TimeSlot = "10:00-10:30",
                    Status = AppointmentStatus.Confirmed
                },
                new Appointment
                {
                    AppointmentId = 3,
                    PatientId = 3,
                    DoctorId = 2,
                    ScheduledDate = firstDate,
                    TimeSlot = "11:00-11:30",
                    Status = AppointmentStatus.Completed
                },
                new Appointment
                {
                    AppointmentId = 4,
                    PatientId = 4,
                    DoctorId = 2,
                    ScheduledDate = secondDate,
                    TimeSlot = "12:00-12:30",
                    Status = AppointmentStatus.Cancelled
                }
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            // Act
            List<AppointmentReportDto> result =
                await _appointmentService.GetAppointmentReportAsync();

            // Assert
            Assert.Equal(2, result.Count);

            AppointmentReportDto firstReport =
                result.Single(report => report.Date == firstDate.Date);

            Assert.Equal(3, firstReport.TotalCount);
            Assert.Equal(1, firstReport.ScheduledCount);
            Assert.Equal(1, firstReport.ConfirmedCount);
            Assert.Equal(0, firstReport.CancelledCount);
            Assert.Equal(1, firstReport.CompletedCount);

            AppointmentReportDto secondReport =
                result.Single(report => report.Date == secondDate.Date);

            Assert.Equal(1, secondReport.TotalCount);
            Assert.Equal(0, secondReport.ScheduledCount);
            Assert.Equal(0, secondReport.ConfirmedCount);
            Assert.Equal(1, secondReport.CancelledCount);
            Assert.Equal(0, secondReport.CompletedCount);

            _appointmentRepositoryMock.Verify(
                repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAppointmentReportAsync_WhenNoAppointmentsExist_ReturnsEmptyList()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Appointment>());

            // Act
            List<AppointmentReportDto> result =
                await _appointmentService.GetAppointmentReportAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _appointmentRepositoryMock.Verify(
                repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private static Appointment CreateAppointment(
            int appointmentId,
            AppointmentStatus status)
        {
            return new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = new DateTime(2026, 6, 14),
                TimeSlot = "09:00-09:30",
                Status = status,
                CancellationReason = string.Empty
            };
        }
    }
}
