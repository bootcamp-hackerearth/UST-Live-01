using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository>
            _appointmentRepositoryMock;

        private readonly Mock<IDoctorRepository>
            _doctorRepositoryMock;

        private readonly Mock<IPatientRepository>
            _patientRepositoryMock;

        private readonly Mock<IMapper>
            _mapperMock;

        private readonly AppointmentService
            _service;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _doctorRepositoryMock =
                new Mock<IDoctorRepository>();

            _patientRepositoryMock =
                new Mock<IPatientRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _service =
                new AppointmentService(
                    _appointmentRepositoryMock.Object,
                    _doctorRepositoryMock.Object,
                    _patientRepositoryMock.Object,
                    _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_ReturnsAppointments()
        {
            var appointments =
                new List<Appointment>
                {
                    new Appointment(),
                    new Appointment()
                };

            var appointmentDtos =
                new List<AppointmentDto>
                {
                    new AppointmentDto(),
                    new AppointmentDto()
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<AppointmentDto>>(
                        appointments))
                .Returns(appointmentDtos);

            var result =
                await _service.GetAllAppointmentsAsync();

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_ReturnsAppointment()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1
                };

            var appointmentDto =
                new AppointmentDto();

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(x =>
                    x.Map<AppointmentDto>(
                        appointment))
                .Returns(appointmentDto);

            var result =
                await _service.GetAppointmentByIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenNotFound_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.GetAppointmentByIdAsync(1));
        }

        [Fact]
        public async Task AddAppointmentAsync_ReturnsAppointmentId()
        {
            var dto =
                new CreateAppointmentDto
                {
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = DateTime.Today.AddDays(1),
                    TimeSlot = "10:00 AM"
                };

            var patient =
                new Patient
                {
                    PatientId = 1
                };

            var doctor =
                new Doctor
                {
                    DoctorId = 1,
                    IsActive = true
                };

            var appointment =
                new Appointment
                {
                    AppointmentId = 1
                };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.IsSlotAvailableAsync(
                        dto.DoctorId,
                        dto.ScheduledDate,
                        dto.TimeSlot))
                .ReturnsAsync(true);

            _mapperMock
                .Setup(x =>
                    x.Map<Appointment>(dto))
                .Returns(appointment);

            var result =
                await _service.AddAppointmentAsync(dto);

            Assert.Equal(
                1,
                result);

            Assert.Equal(
                AppointmentStatus.Pending,
                appointment.Status);

            _appointmentRepositoryMock.Verify(
                x => x.AddAsync(
                    It.IsAny<Appointment>()),
                Times.Once);
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenPatientNotFound_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsAsync<PatientNotFoundException>(
                () => _service.AddAppointmentAsync(
                    new CreateAppointmentDto()));
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenDoctorNotFound_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<DoctorNotFoundException>(
                () => _service.AddAppointmentAsync(
                    new CreateAppointmentDto()));
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenScheduledDateIsInPast_ThrowsException()
        {
            var dto =
                new CreateAppointmentDto
                {
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = DateTime.Today.AddDays(-1),
                    TimeSlot = "10:00 AM"
                };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(
                    new Doctor
                    {
                        IsActive = true
                    });

            await Assert.ThrowsAsync<PastDateException>(
                () => _service.AddAppointmentAsync(dto));

            _appointmentRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenScheduledDateBeyondSixMonths_ThrowsException()
        {
            var dto =
                new CreateAppointmentDto
                {
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = DateTime.Today.AddMonths(6).AddDays(1),
                    TimeSlot = "10:00 AM"
                };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(
                    new Doctor
                    {
                        IsActive = true
                    });

            await Assert.ThrowsAsync<AdvanceBookingLimitException>(
                () => _service.AddAppointmentAsync(dto));

            _appointmentRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenDoctorInactive_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(
                    new Doctor
                    {
                        IsActive = false
                    });

            await Assert.ThrowsAsync<DoctorUnavailableException>(
                () => _service.AddAppointmentAsync(
                    new CreateAppointmentDto
                    {
                        ScheduledDate =
                            DateTime.Today.AddDays(1)
                    }));
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenSlotUnavailable_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(
                    new Doctor
                    {
                        IsActive = true
                    });

            _appointmentRepositoryMock
                .Setup(x =>
                    x.IsSlotAvailableAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<string>()))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<AppointmentConflictException>(
                () => _service.AddAppointmentAsync(
                    new CreateAppointmentDto
                    {
                        ScheduledDate =
                            DateTime.Today.AddDays(1)
                    }));
        }

        [Fact]
        public async Task UpdateAppointmentAsync_UpdatesSuccessfully()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Pending
                };

            var dto =
                new UpdateAppointmentDto
                {
                    DoctorId = 2,
                    ScheduledDate = DateTime.Today.AddDays(2),
                    TimeSlot = "11:00 AM"
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.IsSlotAvailableForUpdateAsync(
                        1,
                        dto.DoctorId,
                        dto.ScheduledDate,
                        dto.TimeSlot))
                .ReturnsAsync(true);

            await _service.UpdateAppointmentAsync(
                1,
                dto);

            Assert.Equal(
                dto.DoctorId,
                appointment.DoctorId);

            Assert.Equal(
                dto.ScheduledDate,
                appointment.ScheduledDate);

            Assert.Equal(
                dto.TimeSlot,
                appointment.TimeSlot);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(appointment),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.UpdateAppointmentAsync(
                    1,
                    new UpdateAppointmentDto()));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenSlotUnavailable_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Pending
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.IsSlotAvailableForUpdateAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<string>()))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<AppointmentConflictException>(
                () => _service.UpdateAppointmentAsync(
                    1,
                    new UpdateAppointmentDto()));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenAppointmentCompleted_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Completed
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.IsSlotAvailableForUpdateAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<string>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.UpdateAppointmentAsync(
                    1,
                    new UpdateAppointmentDto()));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenAppointmentCancelled_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Cancelled
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.IsSlotAvailableForUpdateAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<string>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.UpdateAppointmentAsync(
                    1,
                    new UpdateAppointmentDto()));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_DeletesSuccessfully()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Completed
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.DeleteAppointmentAsync(1);

            _appointmentRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.DeleteAppointmentAsync(1));

            _appointmentRepositoryMock.Verify(
                x => x.DeleteAsync(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WhenAppointmentPending_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Pending
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<AppointmentDeletionException>(
                () => _service.DeleteAppointmentAsync(1));

            _appointmentRepositoryMock.Verify(
                x => x.DeleteAsync(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WhenAppointmentConfirmed_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Confirmed
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<AppointmentDeletionException>(
                () => _service.DeleteAppointmentAsync(1));

            _appointmentRepositoryMock.Verify(
                x => x.DeleteAsync(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_UpdatesStatus()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Pending
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.ConfirmAppointmentAsync(1);

            Assert.Equal(
                AppointmentStatus.Confirmed,
                appointment.Status);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(appointment),
                Times.Once);
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.ConfirmAppointmentAsync(1));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenAppointmentNotPending_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Confirmed
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.ConfirmAppointmentAsync(1));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task CancelAppointmentAsync_UpdatesStatus()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Pending
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.CancelAppointmentAsync(
                1,
                "Patient request");

            Assert.Equal(
                AppointmentStatus.Cancelled,
                appointment.Status);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(appointment),
                Times.Once);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenConfirmed_UpdatesStatus()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Confirmed
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.CancelAppointmentAsync(
                1,
                "Doctor unavailable");

            Assert.Equal(
                AppointmentStatus.Cancelled,
                appointment.Status);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(appointment),
                Times.Once);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.CancelAppointmentAsync(
                    1,
                    "Reason"));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenStatusCompleted_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Completed
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.CancelAppointmentAsync(
                    1,
                    "Reason"));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenStatusCancelled_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Cancelled
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.CancelAppointmentAsync(
                    1,
                    "Reason"));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task CompleteAppointmentAsync_UpdatesStatus()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Confirmed
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.CompleteAppointmentAsync(1);

            Assert.Equal(
                AppointmentStatus.Completed,
                appointment.Status);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(appointment),
                Times.Once);
        }

        [Fact]
        public async Task CompleteAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.CompleteAppointmentAsync(1));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task CompleteAppointmentAsync_WhenStatusPending_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Pending
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.CompleteAppointmentAsync(1));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task CompleteAppointmentAsync_WhenStatusCancelled_ThrowsException()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Cancelled
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.CompleteAppointmentAsync(1));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAppointmentsByPatientAsync_ReturnsAppointments()
        {
            var appointments =
                new List<Appointment>
                {
                    new Appointment()
                };

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<AppointmentDto>>(
                        appointments))
                .Returns(
                    new List<AppointmentDto>
                    {
                        new AppointmentDto()
                    });

            var result =
                await _service
                    .GetAppointmentsByPatientAsync(1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAppointmentsByDoctorAsync_ReturnsAppointments()
        {
            var appointments =
                new List<Appointment>
                {
                    new Appointment()
                };

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<AppointmentDto>>(
                        appointments))
                .Returns(
                    new List<AppointmentDto>
                    {
                        new AppointmentDto()
                    });

            var result =
                await _service
                    .GetAppointmentsByDoctorAsync(1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetTodayScheduleAsync_ReturnsAppointments()
        {
            var appointments =
                new List<Appointment>
                {
                    new Appointment(),
                    new Appointment()
                };

            var appointmentDtos =
                new List<AppointmentDto>
                {
                    new AppointmentDto(),
                    new AppointmentDto()
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetTodayScheduleAsync(1))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<AppointmentDto>>(
                        appointments))
                .Returns(appointmentDtos);

            var result =
                await _service.GetTodayScheduleAsync(1);

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task GetWeeklyScheduleAsync_ReturnsAppointments()
        {
            var appointments =
                new List<Appointment>
                {
                    new Appointment(),
                    new Appointment(),
                    new Appointment()
                };

            var appointmentDtos =
                new List<AppointmentDto>
                {
                    new AppointmentDto(),
                    new AppointmentDto(),
                    new AppointmentDto()
                };

            _appointmentRepositoryMock
                .Setup(x => x.GetWeeklyScheduleAsync(1))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<AppointmentDto>>(
                        appointments))
                .Returns(appointmentDtos);

            var result =
                await _service.GetWeeklyScheduleAsync(1);

            Assert.Equal(
                3,
                result.Count());
        }

        [Fact]
        public async Task GetNextAppointmentByPatientAsync_ReturnsAppointment()
        {
            var appointment =
                new Appointment();

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetNextAppointmentByPatientAsync(1))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(x =>
                    x.Map<AppointmentDto>(
                        appointment))
                .Returns(
                    new AppointmentDto());

            var result =
                await _service
                    .GetNextAppointmentByPatientAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetNextAppointmentByPatientAsync_WhenNoAppointment_ReturnsNull()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetNextAppointmentByPatientAsync(1))
                .ReturnsAsync((Appointment)null);

            var result =
                await _service.GetNextAppointmentByPatientAsync(1);

            Assert.Null(result);

            _mapperMock.Verify(
                x => x.Map<AppointmentDto>(
                    It.IsAny<Appointment>()),
                Times.Never);
        }
    }
}
