using HealthAppWebAPI.Constants;
using HealthAppWebAPI.Enums;
using HealthAppWebAPI.Models.Dtos;
using HealthAppWebAPI.Repositories.Interfaces;
using HealthAppWebAPI.Services.Impl;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthAppWebAPI.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly AppointmentService _appointmentService;

        [Fact]
        public async Task BookAppointmentAsync_WhenDtoIsNull_ShouldThrowArgumentException()
        {
            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    _appointmentService.BookAppointmentAsync(null));

            // Assert
            Assert.Equal("Appointment data is required.", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task BookAppointmentAsync_WhenPatientIdIsInvalid_ShouldThrowArgumentException(
            int patientId)
        {
            // Arrange
            var dto = CreateValidAppointmentDto();
            dto.PatientId = patientId;

            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    _appointmentService.BookAppointmentAsync(dto));

            // Assert
            Assert.Equal("Please select a valid patient.", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task BookAppointmentAsync_WhenDoctorIdIsInvalid_ShouldThrowArgumentException(
            int doctorId)
        {
            // Arrange
            var dto = CreateValidAppointmentDto();
            dto.DoctorId = doctorId;

            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    _appointmentService.BookAppointmentAsync(dto));

            // Assert
            Assert.Equal("Please select a valid doctor.", exception.Message);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenScheduledDateIsDefault_ShouldThrowArgumentException()
        {
            // Arrange
            var dto = CreateValidAppointmentDto();
            dto.ScheduledDate = default(DateTime);

            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    _appointmentService.BookAppointmentAsync(dto));

            // Assert
            Assert.Equal("Please select appointment date.", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task BookAppointmentAsync_WhenTimeSlotIsMissing_ShouldThrowArgumentException(
            string timeSlot)
        {
            // Arrange
            var dto = CreateValidAppointmentDto();
            dto.TimeSlot = timeSlot;

            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    _appointmentService.BookAppointmentAsync(dto));

            // Assert
            Assert.Equal("Please select time slot.", exception.Message);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenDateIsPast_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var dto = CreateValidAppointmentDto();
            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            // Act
            var exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.BookAppointmentAsync(dto));

            // Assert
            Assert.Equal("Past date not allowed.", exception.Message);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenTimeSlotIsInvalid_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var dto = CreateValidAppointmentDto();
            dto.TimeSlot = "08:30 AM";

            // Act
            var exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.BookAppointmentAsync(dto));

            // Assert
            Assert.Equal("Invalid time slot selected.", exception.Message);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenDoctorSlotAlreadyBooked_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var dto = CreateValidAppointmentDto();

            _appointmentRepositoryMock
                .Setup(repo => repo.IsDoctorSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(true);

            // Act
            var exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.BookAppointmentAsync(dto));

            // Assert
            Assert.Equal(
                "Doctor is already booked for this time slot.",
                exception.Message);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenPatientHasSlotConflict_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var dto = CreateValidAppointmentDto();

            _appointmentRepositoryMock
                .Setup(repo => repo.IsDoctorSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(repo => repo.HasPatientSlotConflictAsync(
                    dto.PatientId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(true);

            // Act
            var exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.BookAppointmentAsync(dto));

            // Assert
            Assert.Equal(
                "Patient already has an appointment for this time slot.",
                exception.Message);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenPatientAlreadyHasAppointmentWithDoctorOnSameDay_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var dto = CreateValidAppointmentDto();

            _appointmentRepositoryMock
                .Setup(repo => repo.IsDoctorSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(repo => repo.HasPatientSlotConflictAsync(
                    dto.PatientId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(repo => repo.HasAppointmentWithDoctorOnSameDayAsync(
                    dto.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate))
                .ReturnsAsync(true);

            // Act
            var exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.BookAppointmentAsync(dto));

            // Assert
            Assert.Equal(
                "Patient already has an appointment with this doctor on the selected date.",
                exception.Message);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenValid_ShouldAddAppointment()
        {
            // Arrange
            var dto = CreateValidAppointmentDto();

            _appointmentRepositoryMock
                .Setup(repo => repo.IsDoctorSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(repo => repo.HasPatientSlotConflictAsync(
                    dto.PatientId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(repo => repo.HasAppointmentWithDoctorOnSameDayAsync(
                    dto.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate))
                .ReturnsAsync(false);

            Appointment addedAppointment = null;

            _appointmentRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Appointment>()))
                .Callback<Appointment>(appointment =>
                    addedAppointment = appointment)
                .Returns(Task.CompletedTask);

            // Act
            await _appointmentService.BookAppointmentAsync(dto);

            // Assert
            Assert.NotNull(addedAppointment);
            Assert.Equal(dto.PatientId, addedAppointment.PatientId);
            Assert.Equal(dto.DoctorId, addedAppointment.DoctorId);
            Assert.Equal(dto.ScheduledDate, addedAppointment.ScheduledDate);
            Assert.Equal(dto.TimeSlot, addedAppointment.TimeSlot);
            Assert.Equal(
                AppointmentStatus.Pending.ToString(),
                addedAppointment.Status);

            _appointmentRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Appointment>()),
                Times.Once);
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenAppointmentNotFound_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            int appointmentId = 1;

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(appointmentId))
                .ReturnsAsync((Appointment)null);

            // Act
            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                    _appointmentService.ConfirmAppointmentAsync(appointmentId));

            // Assert
            Assert.Equal("Appointment not found.", exception.Message);
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenAppointmentExists_ShouldUpdateAppointmentStatus()
        {
            // Arrange
            int appointmentId = 1;

            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                Status = AppointmentStatus.Pending.ToString()
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(appointmentId))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(repo => repo.UpdateAsync(appointment))
                .Returns(Task.CompletedTask);

            // Act
            await _appointmentService.ConfirmAppointmentAsync(appointmentId);

            // Assert
            Assert.Equal(
                AppointmentStatus.Confirmed.ToString(),
                appointment.Status);

            _appointmentRepositoryMock.Verify(
                repo => repo.UpdateAsync(appointment),
                Times.Once);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenAppointmentNotFound_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            int appointmentId = 1;

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(appointmentId))
                .ReturnsAsync((Appointment)null);

            // Act
            var exception =
                await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                    _appointmentService.CancelAppointmentAsync(
                        appointmentId,
                        "Reason"));

            // Assert
            Assert.Equal("Appointment not found.", exception.Message);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenAppointmentIsCompleted_ShouldThrowInvalidOperationException()
        {
            // Arrange
            int appointmentId = 1;

            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                Status = AppointmentStatus.Completed.ToString()
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(appointmentId))
                .ReturnsAsync(appointment);

            // Act
            var exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.CancelAppointmentAsync(
                        appointmentId,
                        "Reason"));

            // Assert
            Assert.Equal(
                "Cannot cancel completed appointment.",
                exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CancelAppointmentAsync_WhenReasonIsMissing_ShouldThrowArgumentException(
            string reason)
        {
            // Arrange
            int appointmentId = 1;

            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                Status = AppointmentStatus.Pending.ToString()
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(appointmentId))
                .ReturnsAsync(appointment);

            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    _appointmentService.CancelAppointmentAsync(
                        appointmentId,
                        reason));

            // Assert
            Assert.Equal(
                "Cancellation reason is required.",
                exception.Message);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenValid_ShouldUpdateStatusAndReason()
        {
            // Arrange
            int appointmentId = 1;
            string cancellationReason = "Patient requested cancellation.";

            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                Status = AppointmentStatus.Pending.ToString()
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(appointmentId))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(repo => repo.UpdateAsync(appointment))
                .Returns(Task.CompletedTask);

            // Act
            await _appointmentService.CancelAppointmentAsync(
                appointmentId,
                cancellationReason);

            // Assert
            Assert.Equal(
                AppointmentStatus.Cancelled.ToString(),
                appointment.Status);

            Assert.Equal(
                cancellationReason,
                appointment.CancellationReason);

            _appointmentRepositoryMock.Verify(
                repo => repo.UpdateAsync(appointment),
                Times.Once);
        }

        private static CreateAppointmentDto CreateValidAppointmentDto()
        {
            return new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = TimeSlots.Slots.First()
            };
        }
    }
}