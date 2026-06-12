using HealthAppWebApi.Models;
using HealthAppWebApi.Repositories.Interface;
using HealthAppWebApi.Services.Impl;
using Moq;
using SharedDto.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HealthAppWebApi.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository>
            _appointmentRepositoryMock;

        private readonly AppointmentService
            _service;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _service =
                new AppointmentService(
                    _appointmentRepositoryMock.Object);
        }

        #region GetAppointmentByIdAsync

        [Fact]
        public async Task
            GetAppointmentByIdAsync_ValidId_ReturnsDto()
        {
            // Arrange

            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Patient = new Patient
                    {
                        FullName = "John"
                    },
                    Doctor = new Doctor
                    {
                        FullName = "Dr Smith"
                    },
                    ScheduledDate =
                        DateTime.Today,
                    TimeSlot = "09:00 AM",
                    Status =
                        AppointmentStatus.Pending
                };

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act

            var result =
                await _service
                    .GetAppointmentByIdAsync(1);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                "John",
                result.PatientName);

            Assert.Equal(
                "Dr Smith",
                result.DoctorName);
        }

        [Fact]
        public async Task
            GetAppointmentByIdAsync_InvalidId_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Appointment)null);

            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .GetAppointmentByIdAsync(1));

            Assert.Equal(
                "Appointment not found.",
                ex.Message);
        }

        #endregion

        #region GetAllAppointmentsAsync

        [Fact]
        public async Task
            GetAllAppointmentsAsync_ReturnsAppointments()
        {
            var appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        AppointmentId = 1,
                        Patient =
                            new Patient
                            {
                                FullName = "John"
                            },
                        Doctor =
                            new Doctor
                            {
                                FullName = "Dr A"
                            },
                        Status =
                            AppointmentStatus.Pending
                    }
                };

            _appointmentRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(appointments);

            var result =
                await _service
                    .GetAllAppointmentsAsync();

            Assert.Single(result);
        }

        #endregion

        #region BookAppointmentAsync

        [Fact]
        public async Task
            BookAppointmentAsync_PastDate_ThrowsException()
        {
            var dto =
                new CreateAppointmentDto
                {
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate =
                        DateTime.Today.AddDays(-1),
                    TimeSlot = "09:00 AM"
                };

            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .BookAppointmentAsync(dto));

            Assert.Equal(
                "Past dates are not allowed.",
                ex.Message);
        }

        [Fact]
        public async Task
            BookAppointmentAsync_InvalidSlot_ThrowsException()
        {
            var dto =
                new CreateAppointmentDto
                {
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate =
                        DateTime.Today.AddDays(1),
                    TimeSlot = "ABC"
                };

            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .BookAppointmentAsync(dto));

            Assert.Equal(
                "Invalid time slot.",
                ex.Message);
        }

        [Fact]
        public async Task
            BookAppointmentAsync_DoctorSlotBooked_ThrowsException()
        {
            var dto =
                new CreateAppointmentDto
                {
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate =
                        DateTime.Today.AddDays(1),
                    TimeSlot = "09:00 AM"
                };

            _appointmentRepositoryMock
                .Setup(r =>
                    r.IsDoctorSlotBookedAsync(
                        dto.DoctorId,
                        dto.ScheduledDate,
                        dto.TimeSlot))
                .ReturnsAsync(true);

            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .BookAppointmentAsync(dto));

            Assert.Equal(
                "Selected slot is already booked.",
                ex.Message);
        }

        [Fact]
        public async Task
            BookAppointmentAsync_PatientConflict_ThrowsException()
        {
            var dto =
                new CreateAppointmentDto
                {
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate =
                        DateTime.Today.AddDays(1),
                    TimeSlot = "09:00 AM"
                };

            _appointmentRepositoryMock
                .Setup(r =>
                    r.IsDoctorSlotBookedAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<string>()))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(r =>
                    r.HasPatientSlotConflictAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<string>()))
                .ReturnsAsync(true);

            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .BookAppointmentAsync(dto));

            Assert.Equal(
                "Patient already has another appointment during this slot.",
                ex.Message);
        }

        [Fact]
        public async Task
            BookAppointmentAsync_AlreadyHasDoctorAppointment_ThrowsException()
        {
            var dto =
                new CreateAppointmentDto
                {
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate =
                        DateTime.Today.AddDays(1),
                    TimeSlot = "09:00 AM"
                };

            _appointmentRepositoryMock
                .Setup(r =>
                    r.IsDoctorSlotBookedAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<string>()))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(r =>
                    r.HasPatientSlotConflictAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<string>()))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(r =>
                    r.HasAppointmentWithDoctorOnSameDayAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<DateTime>()))
                .ReturnsAsync(true);

            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .BookAppointmentAsync(dto));

            Assert.Equal(
                "Patient already has an appointment with this doctor on this date.",
                ex.Message);
        }

        [Fact]
        public async Task
            BookAppointmentAsync_ValidAppointment_AddsAppointment()
        {
            var dto =
                new CreateAppointmentDto
                {
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate =
                        DateTime.Today.AddDays(1),
                    TimeSlot = "09:00 AM"
                };

            _appointmentRepositoryMock
                .Setup(r =>
                    r.IsDoctorSlotBookedAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<string>()))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(r =>
                    r.HasPatientSlotConflictAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<string>()))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(r =>
                    r.HasAppointmentWithDoctorOnSameDayAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<DateTime>()))
                .ReturnsAsync(false);

            await _service
                .BookAppointmentAsync(dto);

            _appointmentRepositoryMock
                .Verify(
                    r => r.AddAsync(
                        It.IsAny<Appointment>()),
                    Times.Once);
        }

        #endregion

        #region ConfirmAppointmentAsync

        [Fact]
        public async Task
            ConfirmAppointmentAsync_InvalidId_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<Exception>(
                () =>
                    _service
                        .ConfirmAppointmentAsync(1));
        }

        [Fact]
        public async Task
            ConfirmAppointmentAsync_ValidId_UpdatesAppointment()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Pending
                };

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service
                .ConfirmAppointmentAsync(1);

            Assert.Equal(
                AppointmentStatus.Confirmed,
                appointment.Status);

            _appointmentRepositoryMock
                .Verify(
                    r => r.UpdateAsync(
                        appointment),
                    Times.Once);
        }

        #endregion

        #region CancelAppointmentAsync

        [Fact]
        public async Task
            CancelAppointmentAsync_InvalidId_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<Exception>(
                () =>
                    _service
                        .CancelAppointmentAsync(
                            1,
                            "Reason"));
        }

        [Fact]
        public async Task
            CancelAppointmentAsync_CompletedAppointment_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Completed
                };

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<Exception>(
                () =>
                    _service
                        .CancelAppointmentAsync(
                            1,
                            "Reason"));
        }

        [Fact]
        public async Task
            CancelAppointmentAsync_EmptyReason_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Pending
                };

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<Exception>(
                () =>
                    _service
                        .CancelAppointmentAsync(
                            1,
                            ""));
        }

        [Fact]
        public async Task
            CancelAppointmentAsync_ValidRequest_CancelsAppointment()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Pending
                };

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service
                .CancelAppointmentAsync(
                    1,
                    "Patient request");

            Assert.Equal(
                AppointmentStatus.Cancelled,
                appointment.Status);

            Assert.Equal(
                "Patient request",
                appointment.CancellationReason);

            _appointmentRepositoryMock
                .Verify(
                    r => r.UpdateAsync(
                        appointment),
                    Times.Once);
        }

        #endregion

        #region GetAppointmentsForPatientAsync

        [Fact]
        public async Task
            GetAppointmentsForPatientAsync_ReturnsAppointments()
        {
            var appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        AppointmentId = 1,
                        Patient =
                            new Patient
                            {
                                FullName = "John"
                            },
                        Doctor =
                            new Doctor
                            {
                                FullName = "Dr A"
                            }
                    }
                };

            _appointmentRepositoryMock
                .Setup(r =>
                    r.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(appointments);

            var result =
                await _service
                    .GetAppointmentsForPatientAsync(1);

            Assert.Single(result);
        }

        #endregion

        #region GetUpcomingAppointmentsAsync

        [Fact]
        public async Task
            GetUpcomingAppointmentsAsync_ReturnsData()
        {
            _appointmentRepositoryMock
                .Setup(r =>
                    r.GetUpcomingAppointmentsAsync())
                .ReturnsAsync(
                    new List<Appointment>());

            var result =
                await _service
                    .GetUpcomingAppointmentsAsync();

            Assert.NotNull(result);
        }

        #endregion

        #region GetUpcomingAppointmentsByDoctorAsync

        [Fact]
        public async Task
            GetUpcomingAppointmentsByDoctorAsync_ReturnsData()
        {
            _appointmentRepositoryMock
                .Setup(r =>
                    r.GetUpcomingAppointmentsByDoctorAsync(
                        "Dr"))
                .ReturnsAsync(
                    new List<Appointment>());

            var result =
                await _service
                    .GetUpcomingAppointmentsByDoctorAsync(
                        "Dr");

            Assert.NotNull(result);
        }

        #endregion

        #region GetAvailableSlotsAsync

        [Fact]
        public async Task
            GetAvailableSlotsAsync_ReturnsSlots()
        {
            var slots =
                new List<string>
                {
                    "09:00 AM",
                    "10:00 AM"
                };

            _appointmentRepositoryMock
                .Setup(r =>
                    r.GetAvailableSlotsAsync(
                        1,
                        It.IsAny<DateTime>()))
                .ReturnsAsync(slots);

            var result =
                await _service
                    .GetAvailableSlotsAsync(
                        1,
                        DateTime.Today);

            Assert.Equal(
                2,
                result.Count);
        }

        #endregion

        #region GetAppointmentsByPatientNameAsync

        [Fact]
        public async Task
            GetAppointmentsByPatientNameAsync_ReturnsAppointments()
        {
            _appointmentRepositoryMock
                .Setup(r =>
                    r.GetAppointmentsByPatientNameAsync(
                        "John"))
                .ReturnsAsync(
                    new List<Appointment>());

            var result =
                await _service
                    .GetAppointmentsByPatientNameAsync(
                        "John");

            Assert.NotNull(result);
        }

        #endregion

        #region HealthRecordExistsAsync

        [Fact]
        public async Task
            HealthRecordExistsAsync_ReturnsTrue()
        {
            _appointmentRepositoryMock
                .Setup(r =>
                    r.HealthRecordExistsAsync(1))
                .ReturnsAsync(true);

            bool result =
                await _service
                    .HealthRecordExistsAsync(1);

            Assert.True(result);
        }

        #endregion
    }
}