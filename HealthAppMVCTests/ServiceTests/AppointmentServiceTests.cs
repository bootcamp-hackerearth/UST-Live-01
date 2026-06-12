using FluentAssertions;
using HealthAppWebApi.Constants;
using HealthAppWebApi.Models;
using HealthAppWebApi.Repositories.Interface;
using HealthAppWebApi.Services.Impl;
using Moq;
using SharedDto.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthAppWebApi.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _repoMock;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _repoMock = new Mock<IAppointmentRepository>();
            _service = new AppointmentService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_ShouldReturnAppointmentDtos()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 2,
                    Patient = new Patient
                    {
                        PatientId = 1,
                        FullName = "John Patient"
                    },
                    Doctor = new Doctor
                    {
                        DoctorId = 2,
                        FullName = "Dr Smith"
                    },
                    ScheduledDate = new DateTime(2026, 1, 10),
                    TimeSlot = "10:00 AM",
                    Status = (int)AppointmentStatus.Pending,
                    CancellationReason = null
                }
            };

            _repoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(appointments);

            // Act
            var result = await _service.GetAllAppointmentsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            result[0].AppointmentId.Should().Be(1);
            result[0].PatientName.Should().Be("John Patient");
            result[0].DoctorName.Should().Be("Dr Smith");
            result[0].ScheduledDate.Should().Be(new DateTime(2026, 1, 10));
            result[0].TimeSlot.Should().Be("10:00 AM");

            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenAppointmentExists_ShouldReturnAppointmentDto()
        {
            // Arrange
            var appointment = CreateAppointment();

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act
            var result = await _service.GetAppointmentByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.AppointmentId.Should().Be(1);
            result.PatientName.Should().Be("John Patient");
            result.DoctorName.Should().Be("Dr Smith");

            // Updated assertion
            result.TimeSlot.Should().Be(TimeSlots.Slots.First());

            _repoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenAppointmentDoesNotExist_ShouldThrowException()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Appointment)null);

            // Act
            Func<Task> act = async () =>
                await _service.GetAppointmentByIdAsync(99);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Appointment not found.");

            _repoMock.Verify(r => r.GetByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenScheduledDateIsPast_ShouldThrowException()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(-1),
                TimeSlot = TimeSlots.Slots.First()
            };

            // Act
            Func<Task> act = async () =>
                await _service.BookAppointmentAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Past dates are not allowed.");

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenTimeSlotIsInvalid_ShouldThrowException()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "Invalid Slot"
            };

            // Act
            Func<Task> act = async () =>
                await _service.BookAppointmentAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Invalid time slot.");

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenDoctorSlotAlreadyBooked_ShouldThrowException()
        {
            // Arrange
            var dto = CreateValidAppointmentDto();

            _repoMock
                .Setup(r => r.IsDoctorSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () =>
                await _service.BookAppointmentAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Selected slot is already booked.");

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenPatientHasSlotConflict_ShouldThrowException()
        {
            // Arrange
            var dto = CreateValidAppointmentDto();

            _repoMock
                .Setup(r => r.IsDoctorSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(false);

            _repoMock
                .Setup(r => r.HasPatientSlotConflictAsync(
                    dto.PatientId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () =>
                await _service.BookAppointmentAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Patient already has another appointment during this slot.");

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenPatientAlreadyHasAppointmentWithDoctorSameDay_ShouldThrowException()
        {
            // Arrange
            var dto = CreateValidAppointmentDto();

            _repoMock
                .Setup(r => r.IsDoctorSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(false);

            _repoMock
                .Setup(r => r.HasPatientSlotConflictAsync(
                    dto.PatientId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(false);

            _repoMock
                .Setup(r => r.HasAppointmentWithDoctorOnSameDayAsync(
                    dto.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () =>
                await _service.BookAppointmentAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Patient already has an appointment with this doctor on this date.");

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenValid_ShouldAddAppointment()
        {
            // Arrange
            var dto = CreateValidAppointmentDto();

            _repoMock
                .Setup(r => r.IsDoctorSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(false);

            _repoMock
                .Setup(r => r.HasPatientSlotConflictAsync(
                    dto.PatientId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(false);

            _repoMock
                .Setup(r => r.HasAppointmentWithDoctorOnSameDayAsync(
                    dto.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate))
                .ReturnsAsync(false);

            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.BookAppointmentAsync(dto);

            // Assert
            _repoMock.Verify(r => r.AddAsync(It.Is<Appointment>(a =>
                a.PatientId == dto.PatientId &&
                a.DoctorId == dto.DoctorId &&
                a.ScheduledDate == dto.ScheduledDate &&
                a.TimeSlot == dto.TimeSlot &&
                a.Status == (int)AppointmentStatus.Pending
            )), Times.Once);
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenAppointmentDoesNotExist_ShouldThrowException()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Appointment)null);

            // Act
            Func<Task> act = async () =>
                await _service.ConfirmAppointmentAsync(99);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Appointment not found.");

            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenAppointmentExists_ShouldUpdateStatusToConfirmed()
        {
            // Arrange
            var appointment = CreateAppointment();
            appointment.Status = (int)AppointmentStatus.Pending;

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.ConfirmAppointmentAsync(1);

            // Assert
            _repoMock.Verify(r => r.UpdateAsync(It.Is<Appointment>(a =>
                a.AppointmentId == 1 &&
                a.Status == (int)AppointmentStatus.Confirmed
            )), Times.Once);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenAppointmentDoesNotExist_ShouldThrowException()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Appointment)null);

            // Act
            Func<Task> act = async () =>
                await _service.CancelAppointmentAsync(99, "Patient unavailable");

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Appointment not found.");

            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenAppointmentIsCompleted_ShouldThrowException()
        {
            // Arrange
            var appointment = CreateAppointment();
            appointment.Status = (int)AppointmentStatus.Completed;

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () =>
                await _service.CancelAppointmentAsync(1, "Patient unavailable");

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Completed appointments cannot be cancelled.");

            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CancelAppointmentAsync_WhenReasonIsEmpty_ShouldThrowException(string reason)
        {
            // Arrange
            var appointment = CreateAppointment();
            appointment.Status = (int)AppointmentStatus.Pending;

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () =>
                await _service.CancelAppointmentAsync(1, reason);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Cancellation reason is required.");

            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenValid_ShouldUpdateStatusAndReason()
        {
            // Arrange
            var appointment = CreateAppointment();
            appointment.Status = (int)AppointmentStatus.Pending;

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.CancelAppointmentAsync(1, "Patient unavailable");

            // Assert
            _repoMock.Verify(r => r.UpdateAsync(It.Is<Appointment>(a =>
                a.AppointmentId == 1 &&
                a.Status == (int)AppointmentStatus.Cancelled &&
                a.CancellationReason == "Patient unavailable"
            )), Times.Once);
        }

        [Fact]
        public async Task GetAppointmentsForPatientAsync_ShouldReturnAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                CreateAppointment()
            };

            _repoMock
                .Setup(r => r.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(appointments);

            // Act
            var result = await _service.GetAppointmentsForPatientAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].PatientName.Should().Be("John Patient");

            _repoMock.Verify(r => r.GetAppointmentsByPatientAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetUpcomingAppointmentsAsync_ShouldReturnUpcomingAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                CreateAppointment()
            };

            _repoMock
                .Setup(r => r.GetUpcomingAppointmentsAsync())
                .ReturnsAsync(appointments);

            // Act
            var result = await _service.GetUpcomingAppointmentsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].DoctorName.Should().Be("Dr Smith");

            _repoMock.Verify(r => r.GetUpcomingAppointmentsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetUpcomingAppointmentsByDoctorAsync_ShouldReturnDoctorAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                CreateAppointment()
            };

            _repoMock
                .Setup(r => r.GetUpcomingAppointmentsByDoctorAsync("Dr Smith"))
                .ReturnsAsync(appointments);

            // Act
            var result = await _service.GetUpcomingAppointmentsByDoctorAsync("Dr Smith");

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].DoctorName.Should().Be("Dr Smith");

            _repoMock.Verify(r => r.GetUpcomingAppointmentsByDoctorAsync("Dr Smith"), Times.Once);
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_ShouldReturnAvailableSlots()
        {
            // Arrange
            var date = DateTime.Today.AddDays(1);

            var slots = new List<string>
            {
                "10:00 AM",
                "11:00 AM"
            };

            _repoMock
                .Setup(r => r.GetAvailableSlotsAsync(2, date))
                .ReturnsAsync(slots);

            // Act
            var result = await _service.GetAvailableSlotsAsync(2, date);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain("10:00 AM");

            _repoMock.Verify(r => r.GetAvailableSlotsAsync(2, date), Times.Once);
        }

        [Fact]
        public async Task GetAppointmentsByPatientNameAsync_ShouldReturnAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                CreateAppointment()
            };

            _repoMock
                .Setup(r => r.GetAppointmentsByPatientNameAsync("John"))
                .ReturnsAsync(appointments);

            // Act
            var result = await _service.GetAppointmentsByPatientNameAsync("John");

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].PatientName.Should().Be("John Patient");

            _repoMock.Verify(r => r.GetAppointmentsByPatientNameAsync("John"), Times.Once);
        }

        [Fact]
        public async Task HealthRecordExistsAsync_WhenRecordExists_ShouldReturnTrue()
        {
            // Arrange
            _repoMock
                .Setup(r => r.HealthRecordExistsAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _service.HealthRecordExistsAsync(1);

            // Assert
            result.Should().BeTrue();

            _repoMock.Verify(r => r.HealthRecordExistsAsync(1), Times.Once);
        }

        [Fact]
        public async Task HealthRecordExistsAsync_WhenRecordDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            _repoMock
                .Setup(r => r.HealthRecordExistsAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _service.HealthRecordExistsAsync(1);

            // Assert
            result.Should().BeFalse();

            _repoMock.Verify(r => r.HealthRecordExistsAsync(1), Times.Once);
        }

        private static Appointment CreateAppointment()
        {
            return new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 2,
                Patient = new Patient
                {
                    PatientId = 1,
                    FullName = "John Patient"
                },
                Doctor = new Doctor
                {
                    DoctorId = 2,
                    FullName = "Dr Smith"
                },
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = TimeSlots.Slots.First(),
                Status = (int)AppointmentStatus.Pending,
                CancellationReason = null
            };
        }

        private static CreateAppointmentDto CreateValidAppointmentDto()
        {
            return new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = TimeSlots.Slots.First()
            };
        }
    }
}
