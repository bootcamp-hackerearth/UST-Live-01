using AutoMapper;
using Moq;
using Xunit;

using HealthCare_Appointment_Portal.Services;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _service = new AppointmentService(
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_ReturnsAppointments()
        {
            var appointments = new List<Appointment>
            {
                new Appointment { AppointmentId = 1 }
            };

            var dtos = new List<AppointmentDto>
            {
                new AppointmentDto { AppointmentId = 1 }
            };

            _unitOfWorkMock
                .Setup(x => x.Appointments.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<AppointmentDto>>(appointments))
                .Returns(dtos);

            var result = await _service.GetAllAppointmentsAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_ValidId_ReturnsAppointment()
        {
            var appointment = new Appointment { AppointmentId = 1 };

            var dto = new AppointmentDto { AppointmentId = 1 };

            _unitOfWorkMock
                .Setup(x => x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(dto);

            var result = await _service.GetAppointmentByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.AppointmentId);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_InvalidId_ThrowsException()
        {
            _unitOfWorkMock
                .Setup(x => x.Appointments.GetByIdAsync(1))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.GetAppointmentByIdAsync(1));
        }

        [Fact]
        public async Task AddAppointmentAsync_Valid_ReturnsId()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };

            var patient = new Patient();
            var doctor = new Doctor { IsActive = true };

            var appointment = new Appointment { AppointmentId = 1 };

            _unitOfWorkMock
                .Setup(x => x.Patients.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(patient);

            _unitOfWorkMock
                .Setup(x => x.Doctors.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(doctor);

            _unitOfWorkMock
                .Setup(x => x.Appointments.IsSlotAvailableAsync(
                    dto.DoctorId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(true);

            _mapperMock
                .Setup(x => x.Map<Appointment>(dto))
                .Returns(appointment);

            var result = await _service.AddAppointmentAsync(dto);

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task AddAppointmentAsync_PatientNotFound_ThrowsException()
        {
            var dto = new CreateAppointmentDto { PatientId = 1 };

            _unitOfWorkMock
                .Setup(x => x.Patients.GetByIdAsync(1))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsAsync<PatientNotFoundException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [Fact]
        public async Task AddAppointmentAsync_DoctorNotFound_ThrowsException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1
            };

            _unitOfWorkMock
                .Setup(x => x.Patients.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _unitOfWorkMock
                .Setup(x => x.Doctors.GetByIdAsync(1))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<DoctorNotFoundException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [Fact]
        public async Task AddAppointmentAsync_PastDate_ThrowsException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(-1)
            };

            _unitOfWorkMock
                .Setup(x => x.Patients.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _unitOfWorkMock
                .Setup(x => x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(new Doctor { IsActive = true });

            await Assert.ThrowsAsync<PastDateException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [Fact]
        public async Task AddAppointmentAsync_DoctorUnavailable_ThrowsException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1)
            };

            _unitOfWorkMock
                .Setup(x => x.Patients.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _unitOfWorkMock
                .Setup(x => x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(new Doctor { IsActive = false });

            await Assert.ThrowsAsync<DoctorUnavailableException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [Fact]
        public async Task AddAppointmentAsync_SlotUnavailable_ThrowsException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };

            _unitOfWorkMock
                .Setup(x => x.Patients.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _unitOfWorkMock
                .Setup(x => x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(new Doctor { IsActive = true });

            _unitOfWorkMock
                .Setup(x => x.Appointments.IsSlotAvailableAsync(
                    dto.DoctorId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<AppointmentConflictException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [Fact]
        public async Task DeleteAppointmentAsync_Valid_DeletesSuccessfully()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Completed
            };

            _unitOfWorkMock
                .Setup(x => x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.DeleteAppointmentAsync(1);

            _unitOfWorkMock
                .Verify(x => x.Appointments.DeleteAsync(1), Times.Once);

            _unitOfWorkMock
                .Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WithPendingStatus_ThrowsException()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Pending
            };

            _unitOfWorkMock
                .Setup(x => x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<AppointmentDeletionException>(
                () => _service.DeleteAppointmentAsync(1));
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_Valid_UpdatesStatus()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Pending
            };

            _unitOfWorkMock
                .Setup(x => x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.ConfirmAppointmentAsync(1);

            _unitOfWorkMock.Verify(
                x => x.Appointments.UpdateAsync(appointment),
                Times.Once);
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_InvalidStatus_ThrowsException()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Confirmed
            };

            _unitOfWorkMock
                .Setup(x => x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.ConfirmAppointmentAsync(1));
        }

        [Fact]
        public async Task CompleteAppointmentAsync_NotConfirmed_ThrowsException()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            _unitOfWorkMock
                .Setup(x => x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.CompleteAppointmentAsync(1));
        }
    }
}
