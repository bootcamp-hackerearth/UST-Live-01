using AutoMapper;
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
        private readonly Mock<IAppointmentRepository> _repoMock;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _repoMock = new Mock<IAppointmentRepository>();

            var mapperMock = new Mock<IMapper>();

            _service = new AppointmentService(
                _repoMock.Object,
                mapperMock.Object);
        }

        private static string ValidSlot
        {
            get
            {
                return TimeSlots.Slots.First();
            }
        }

        private static CreateAppointmentDto GetValidCreateAppointmentDto()
        {
            return new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = ValidSlot
            };
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenDtoIsNull_ThrowsArgumentException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.BookAppointmentAsync(null));

            Assert.Equal("Appointment data is required.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenPatientIdIsInvalid_ThrowsArgumentException()
        {
            var dto = GetValidCreateAppointmentDto();
            dto.PatientId = 0;

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.BookAppointmentAsync(dto));

            Assert.Equal("Please select a valid patient.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenDoctorIdIsInvalid_ThrowsArgumentException()
        {
            var dto = GetValidCreateAppointmentDto();
            dto.DoctorId = 0;

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.BookAppointmentAsync(dto));

            Assert.Equal("Please select a valid doctor.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenScheduledDateIsDefault_ThrowsArgumentException()
        {
            var dto = GetValidCreateAppointmentDto();
            dto.ScheduledDate = default(DateTime);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.BookAppointmentAsync(dto));

            Assert.Equal("Please select appointment date.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenTimeSlotIsEmpty_ThrowsArgumentException()
        {
            var dto = GetValidCreateAppointmentDto();
            dto.TimeSlot = " ";

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.BookAppointmentAsync(dto));

            Assert.Equal("Please select time slot.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenScheduledDateIsPast_ThrowsInvalidOperationException()
        {
            var dto = GetValidCreateAppointmentDto();
            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.BookAppointmentAsync(dto));

            Assert.Equal("Past date not allowed.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenTimeSlotIsInvalid_ThrowsInvalidOperationException()
        {
            var dto = GetValidCreateAppointmentDto();
            dto.TimeSlot = "Invalid Slot";

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.BookAppointmentAsync(dto));

            Assert.Equal("Invalid time slot selected.", exception.Message);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenDoctorSlotAlreadyBooked_ThrowsInvalidOperationException()
        {
            var dto = GetValidCreateAppointmentDto();

            _repoMock
                .Setup(r => r.IsDoctorSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.BookAppointmentAsync(dto));

            Assert.Equal(
                "Doctor is already booked for this time slot.",
                exception.Message);

            _repoMock.Verify(
                r => r.IsDoctorSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot),
                Times.Once);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenPatientHasSlotConflict_ThrowsInvalidOperationException()
        {
            var dto = GetValidCreateAppointmentDto();

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

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.BookAppointmentAsync(dto));

            Assert.Equal(
                "Patient already has an appointment for this time slot.",
                exception.Message);

            _repoMock.Verify(
                r => r.HasPatientSlotConflictAsync(
                    dto.PatientId,
                    dto.ScheduledDate,
                    dto.TimeSlot),
                Times.Once);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenPatientAlreadyHasAppointmentWithSameDoctorOnSameDay_ThrowsInvalidOperationException()
        {
            var dto = GetValidCreateAppointmentDto();

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

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.BookAppointmentAsync(dto));

            Assert.Equal(
                "Patient already has an appointment with this doctor on the selected date.",
                exception.Message);

            _repoMock.Verify(
                r => r.HasAppointmentWithDoctorOnSameDayAsync(
                    dto.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate),
                Times.Once);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenValid_AddsAppointmentWithPendingStatus()
        {
            var dto = GetValidCreateAppointmentDto();

            Appointment addedAppointment = null;

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
                .Callback<Appointment>(appointment =>
                {
                    addedAppointment = appointment;
                })
                .Returns(Task.CompletedTask);

            await _service.BookAppointmentAsync(dto);

            Assert.NotNull(addedAppointment);
            Assert.Equal(dto.PatientId, addedAppointment.PatientId.Value);
            Assert.Equal(dto.DoctorId, addedAppointment.DoctorId.Value);
            Assert.Equal(dto.ScheduledDate, addedAppointment.ScheduledDate);
            Assert.Equal(dto.TimeSlot, addedAppointment.TimeSlot);
            Assert.Equal(
                AppointmentStatus.Pending.ToString(),
                addedAppointment.Status);

            _repoMock.Verify(
                r => r.AddAsync(It.IsAny<Appointment>()),
                Times.Once);
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenAppointmentNotFound_ThrowsKeyNotFoundException()
        {
            int appointmentId = 1;

            _repoMock
                .Setup(r => r.GetByIdAsync(appointmentId))
                .ReturnsAsync((Appointment)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.ConfirmAppointmentAsync(appointmentId));

            Assert.Equal("Appointment not found.", exception.Message);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenAppointmentExists_UpdatesStatusToConfirmed()
        {
            int appointmentId = 1;

            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                Status = AppointmentStatus.Pending.ToString()
            };

            Appointment updatedAppointment = null;

            _repoMock
                .Setup(r => r.GetByIdAsync(appointmentId))
                .ReturnsAsync(appointment);

            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
                .Callback<Appointment>(a =>
                {
                    updatedAppointment = a;
                })
                .Returns(Task.CompletedTask);

            await _service.ConfirmAppointmentAsync(appointmentId);

            Assert.NotNull(updatedAppointment);
            Assert.Equal(
                AppointmentStatus.Confirmed.ToString(),
                updatedAppointment.Status);

            _repoMock.Verify(
                r => r.UpdateAsync(appointment),
                Times.Once);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenAppointmentNotFound_ThrowsKeyNotFoundException()
        {
            int appointmentId = 1;

            _repoMock
                .Setup(r => r.GetByIdAsync(appointmentId))
                .ReturnsAsync((Appointment)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.CancelAppointmentAsync(
                    appointmentId,
                    "Patient unavailable"));

            Assert.Equal("Appointment not found.", exception.Message);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenAppointmentCompleted_ThrowsInvalidOperationException()
        {
            int appointmentId = 1;

            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                Status = AppointmentStatus.Completed.ToString()
            };

            _repoMock
                .Setup(r => r.GetByIdAsync(appointmentId))
                .ReturnsAsync(appointment);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CancelAppointmentAsync(
                    appointmentId,
                    "Patient unavailable"));

            Assert.Equal(
                "Cannot cancel completed appointment.",
                exception.Message);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenReasonIsEmpty_ThrowsArgumentException()
        {
            int appointmentId = 1;

            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                Status = AppointmentStatus.Pending.ToString()
            };

            _repoMock
                .Setup(r => r.GetByIdAsync(appointmentId))
                .ReturnsAsync(appointment);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.CancelAppointmentAsync(appointmentId, " "));

            Assert.Equal(
                "Cancellation reason is required.",
                exception.Message);

            _repoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenValid_UpdatesStatusAndCancellationReason()
        {
            int appointmentId = 1;
            string reason = "Patient unavailable";

            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                Status = AppointmentStatus.Pending.ToString()
            };

            Appointment updatedAppointment = null;

            _repoMock
                .Setup(r => r.GetByIdAsync(appointmentId))
                .ReturnsAsync(appointment);

            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
                .Callback<Appointment>(a =>
                {
                    updatedAppointment = a;
                })
                .Returns(Task.CompletedTask);

            await _service.CancelAppointmentAsync(appointmentId, reason);

            Assert.NotNull(updatedAppointment);
            Assert.Equal(
                AppointmentStatus.Cancelled.ToString(),
                updatedAppointment.Status);

            Assert.Equal(reason, updatedAppointment.CancellationReason);

            _repoMock.Verify(
                r => r.UpdateAsync(appointment),
                Times.Once);
        }
    }
}
