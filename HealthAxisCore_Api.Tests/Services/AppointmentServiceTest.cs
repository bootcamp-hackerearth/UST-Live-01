using AutoMapper;
using FluentAssertions;
using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Implementations;
using HealthAxisCore_Api.Tests.Helpers;
using Moq;

namespace HealthAxisCore_Api.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly IMapper _mapper;
        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _mapper = MapperHelper.GetMapper();

            _appointmentService = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _mapper
            );
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 2,
                    ScheduledDate = DateTime.Today.AddDays(1),
                    TimeSlot = "10:00 AM",
                    Status = AppointmentStatus.Pending,
                    CancellationReason = null
                },
                new Appointment
                {
                    AppointmentId = 2,
                    PatientId = 2,
                    DoctorId = 3,
                    ScheduledDate = DateTime.Today.AddDays(2),
                    TimeSlot = "11:00 AM",
                    Status = AppointmentStatus.Confirmed,
                    CancellationReason = null
                }
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(appointments);

            // Act
            var result = await _appointmentService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().AppointmentId.Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppointmentExists_ShouldReturnAppointment()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Pending,
                CancellationReason = null
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act
            var result = await _appointmentService.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.AppointmentId.Should().Be(1);
            result.Status.Should().Be(AppointmentStatus.Pending);
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppointmentDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            // Act
            var act = async () => await _appointmentService.GetByIdAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Appointment not found");
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorAvailable_ShouldCreateAppointment()
        {
            // Arrange
            var dto = new CreateAppointmentDTO
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _doctorRepositoryMock
                .Setup(repo => repo.IsDoctorAvailable(dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(true);

            _appointmentRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask)
                .Callback<Appointment>(appointment =>
                {
                    appointment.AppointmentId = 1;
                });

            // Act
            var result = await _appointmentService.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.AppointmentId.Should().Be(1);
            result.PatientId.Should().Be(dto.PatientId);
            result.DoctorId.Should().Be(dto.DoctorId);
            result.Status.Should().Be(AppointmentStatus.Pending);


            _appointmentRepositoryMock.Verify(
                repo => repo.AddAsync(It.Is<Appointment>(a =>
                    a.PatientId == dto.PatientId &&
                    a.DoctorId == dto.DoctorId &&
                    a.ScheduledDate.Date == dto.ScheduledDate.Date &&
                    a.Status == AppointmentStatus.Pending)),
                Times.Once
            );

        }

        [Fact]
        public async Task CreateAsync_WhenDoctorNotAvailable_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            var dto = new CreateAppointmentDTO
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _doctorRepositoryMock
                .Setup(repo => repo.IsDoctorAvailable(dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(false);

            // Act
            var act = async () => await _appointmentService.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Doctor not available for the selected date");

            _appointmentRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Appointment>()),
                Times.Never
            );
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentExists_ShouldDeleteAppointmentAndReturnTrue()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(true);

            _appointmentRepositoryMock
                .Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _appointmentService.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();

            _appointmentRepositoryMock.Verify(
                repo => repo.DeleteAsync(1),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(false);

            // Act
            var act = async () => await _appointmentService.DeleteAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Appointment not found");

            _appointmentRepositoryMock.Verify(
                repo => repo.DeleteAsync(It.IsAny<int>()),
                Times.Never
            );
        }

        [Fact]
        public async Task GetByDoctorAsync_WhenAppointmentsExist_ShouldReturnAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 2,
                    ScheduledDate = DateTime.Today.AddDays(1),
                    TimeSlot = "10:00 AM",
                    Status = AppointmentStatus.Pending
                }
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByDoctor(2))
                .ReturnsAsync(appointments);

            // Act
            var result = await _appointmentService.GetByDoctorAsync(2);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().DoctorId.Should().Be(2);
        }

        [Fact]
        public async Task GetByDoctorAsync_WhenNoAppointmentsExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByDoctor(2))
                .ReturnsAsync(new List<Appointment>());

            // Act
            var act = async () => await _appointmentService.GetByDoctorAsync(2);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("No appointments found for this doctor");
        }

        [Fact]
        public async Task GetByPatientAsync_WhenAppointmentsExist_ShouldReturnAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 2,
                    ScheduledDate = DateTime.Today.AddDays(1),
                    TimeSlot = "10:00 AM",
                    Status = AppointmentStatus.Pending
                }
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByPatient(1))
                .ReturnsAsync(appointments);

            // Act
            var result = await _appointmentService.GetByPatientAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().PatientId.Should().Be(1);
        }

        [Fact]
        public async Task GetByPatientAsync_WhenNoAppointmentsExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByPatient(1))
                .ReturnsAsync(new List<Appointment>());

            // Act
            var act = async () => await _appointmentService.GetByPatientAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("No appointments found for this patient");
        }

        [Fact]
        public async Task FilterAsync_WhenAppointmentsExist_ShouldReturnAppointments()
        {
            // Arrange
            var status = AppointmentStatus.Pending;
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(5);

            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 2,
                    ScheduledDate = DateTime.Today.AddDays(1),
                    TimeSlot = "10:00 AM",
                    Status = AppointmentStatus.Pending
                }
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.FilterAppointments(status, startDate, endDate))
                .ReturnsAsync(appointments);

            // Act
            var result = await _appointmentService.FilterAsync(status, startDate, endDate);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Status.Should().Be(AppointmentStatus.Pending);
        }

        [Fact]
        public async Task FilterAsync_WhenNoAppointmentsExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            var status = AppointmentStatus.Pending;
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(5);

            _appointmentRepositoryMock
                .Setup(repo => repo.FilterAppointments(status, startDate, endDate))
                .ReturnsAsync(new List<Appointment>());

            // Act
            var act = async () => await _appointmentService.FilterAsync(status, startDate, endDate);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("No appointments found for given criteria");
        }

        [Fact]
        public async Task CancelAsync_WhenAppointmentExists_ShouldCancelAppointmentAndReturnTrue()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Pending
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(repo => repo.CancelAppointment(1, "Patient unavailable"))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _appointmentService.CancelAsync(1, "Patient unavailable");

            // Assert
            result.Should().BeTrue();

            _appointmentRepositoryMock.Verify(
                repo => repo.CancelAppointment(1, "Patient unavailable"),
                Times.Once
            );
        }

        [Fact]
        public async Task CancelAsync_WhenAppointmentDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            // Act
            var act = async () => await _appointmentService.CancelAsync(1, "Reason");

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Appointment not found");

            _appointmentRepositoryMock.Verify(
                repo => repo.CancelAppointment(It.IsAny<int>(), It.IsAny<string>()),
                Times.Never
            );
        }

        [Fact]
        public async Task CancelAsync_WhenAppointmentAlreadyCancelled_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Cancelled
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act
            var act = async () => await _appointmentService.CancelAsync(1, "Reason");

            // Assert
            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointment already cancelled");

            _appointmentRepositoryMock.Verify(
                repo => repo.CancelAppointment(It.IsAny<int>(), It.IsAny<string>()),
                Times.Never
            );
        }

        [Fact]
        public async Task CancelAsync_WhenAppointmentCompleted_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Completed
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act
            var act = async () => await _appointmentService.CancelAsync(1, "Reason");

            // Assert
            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Cannot cancel a completed appointment");

            _appointmentRepositoryMock.Verify(
                repo => repo.CancelAppointment(It.IsAny<int>(), It.IsAny<string>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ConfirmAsync_WhenAppointmentExists_ShouldConfirmAppointmentAndReturnTrue()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Pending
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(repo => repo.ConfirmAppointment(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _appointmentService.ConfirmAsync(1);

            // Assert
            result.Should().BeTrue();

            _appointmentRepositoryMock.Verify(
                repo => repo.ConfirmAppointment(1),
                Times.Once
            );
        }

        [Fact]
        public async Task ConfirmAsync_WhenAppointmentDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            // Act
            var act = async () => await _appointmentService.ConfirmAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Appointment not found");

            _appointmentRepositoryMock.Verify(
                repo => repo.ConfirmAppointment(It.IsAny<int>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ConfirmAsync_WhenAppointmentCompleted_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Completed
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act
            var act = async () => await _appointmentService.ConfirmAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Cannot confirm a completed appointment");

            _appointmentRepositoryMock.Verify(
                repo => repo.ConfirmAppointment(It.IsAny<int>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ConfirmAsync_WhenAppointmentCancelled_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Cancelled
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act
            var act = async () => await _appointmentService.ConfirmAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Cannot confirm a cancelled appointment");

            _appointmentRepositoryMock.Verify(
                repo => repo.ConfirmAppointment(It.IsAny<int>()),
                Times.Never
            );
        }
    }
}