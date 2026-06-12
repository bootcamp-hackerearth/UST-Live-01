using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    [TestClass]
    public class AppointmentServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IAppointmentRepository> _mockAppointmentRepo;
        private Mock<IPatientRepository> _mockPatientRepo;
        private Mock<IDoctorRepository> _mockDoctorRepo;
        private Mock<IMapper> _mockMapper;
        private AppointmentService _sut;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAppointmentRepo = new Mock<IAppointmentRepository>();
            _mockPatientRepo = new Mock<IPatientRepository>();
            _mockDoctorRepo = new Mock<IDoctorRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(u => u.Appointments).Returns(_mockAppointmentRepo.Object);
            _mockUnitOfWork.Setup(u => u.Patients).Returns(_mockPatientRepo.Object);
            _mockUnitOfWork.Setup(u => u.Doctors).Returns(_mockDoctorRepo.Object);

            _sut = new AppointmentService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task GetAllAppointmentsAsync_ReturnsMappedAppointments()
        {
            var appointments = new List<Appointment>
            {
                new Appointment(),
                new Appointment()
            };

            var dtos = new List<AppointmentDto>
            {
                new AppointmentDto(),
                new AppointmentDto()
            };

            _mockAppointmentRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(appointments);
            _mockMapper.Setup(m => m.Map<IEnumerable<AppointmentDto>>(appointments)).Returns(dtos);

            var result = await _sut.GetAllAppointmentsAsync();

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetAppointmentByIdAsync_ValidId_ReturnsAppointment()
        {
            int id = 1;

            var appointment = new Appointment { AppointmentId = id };
            var dto = new AppointmentDto { AppointmentId = id };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);
            _mockMapper.Setup(m => m.Map<AppointmentDto>(appointment)).Returns(dto);

            var result = await _sut.GetAppointmentByIdAsync(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.AppointmentId);
        }

        [TestMethod]
        public async Task GetAppointmentByIdAsync_InvalidId_ThrowsAppointmentNotFoundException()
        {
            _mockAppointmentRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsExceptionAsync<AppointmentNotFoundException>(
                () => _sut.GetAppointmentByIdAsync(99));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_PatientNotFound_ThrowsPatientNotFoundException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1
            };

            _mockPatientRepo
                .Setup(r => r.GetByIdAsync(dto.PatientId))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(
                () => _sut.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_DoctorNotFound_ThrowsDoctorNotFoundException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2
            };

            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync(new Patient());
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(dto.DoctorId)).ReturnsAsync((Doctor)null);

            await Assert.ThrowsExceptionAsync<DoctorNotFoundException>(
                () => _sut.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_PastDate_ThrowsPastDateException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(-1)
            };

            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync(new Patient());
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(dto.DoctorId)).ReturnsAsync(new Doctor { IsActive = true });

            await Assert.ThrowsExceptionAsync<PastDateException>(
                () => _sut.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_MoreThanSixMonths_ThrowsAdvanceBookingLimitException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddMonths(7)
            };

            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync(new Patient());
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(dto.DoctorId)).ReturnsAsync(new Doctor { IsActive = true });

            await Assert.ThrowsExceptionAsync<AdvanceBookingLimitException>(
                () => _sut.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_DoctorInactive_ThrowsDoctorUnavailableException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1)
            };

            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync(new Patient());
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(dto.DoctorId)).ReturnsAsync(new Doctor { IsActive = false });

            await Assert.ThrowsExceptionAsync<DoctorUnavailableException>(
                () => _sut.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_SlotUnavailable_ThrowsAppointmentConflictException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };

            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync(new Patient());
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(dto.DoctorId)).ReturnsAsync(new Doctor { IsActive = true });
            _mockAppointmentRepo.Setup(r => r.IsSlotAvailableAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot)).ReturnsAsync(false);

            await Assert.ThrowsExceptionAsync<AppointmentConflictException>(
                () => _sut.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_ValidData_AddsAppointmentAndCommits()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };

            var appointment = new Appointment
            {
                AppointmentId = 5
            };

            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync(new Patient());
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(dto.DoctorId)).ReturnsAsync(new Doctor { IsActive = true });
            _mockAppointmentRepo.Setup(r => r.IsSlotAvailableAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot)).ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<Appointment>(dto)).Returns(appointment);

            var result = await _sut.AddAppointmentAsync(dto);

            Assert.AreEqual(5, result);
            Assert.AreEqual(AppointmentStatus.Pending, appointment.Status);

            _mockAppointmentRepo.Verify(r => r.AddAsync(appointment), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAppointmentAsync_InvalidId_ThrowsAppointmentNotFoundException()
        {
            _mockAppointmentRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsExceptionAsync<AppointmentNotFoundException>(
                () => _sut.UpdateAppointmentAsync(1, new UpdateAppointmentDto()));
        }

        [TestMethod]
        public async Task UpdateAppointmentAsync_SlotUnavailable_ThrowsAppointmentConflictException()
        {
            int id = 1;

            var dto = new UpdateAppointmentDto
            {
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };

            var appointment = new Appointment
            {
                AppointmentId = id,
                Status = AppointmentStatus.Pending
            };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);
            _mockAppointmentRepo.Setup(r => r.IsSlotAvailableAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot)).ReturnsAsync(false);

            await Assert.ThrowsExceptionAsync<AppointmentConflictException>(
                () => _sut.UpdateAppointmentAsync(id, dto));
        }

        [TestMethod]
        public async Task UpdateAppointmentAsync_CompletedStatus_ThrowsInvalidAppointmentStatusException()
        {
            int id = 1;

            var dto = new UpdateAppointmentDto
            {
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };

            var appointment = new Appointment
            {
                AppointmentId = id,
                Status = AppointmentStatus.Completed
            };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);
            _mockAppointmentRepo.Setup(r => r.IsSlotAvailableAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot)).ReturnsAsync(true);

            await Assert.ThrowsExceptionAsync<InvalidAppointmentStatusException>(
                () => _sut.UpdateAppointmentAsync(id, dto));
        }

        [TestMethod]
        public async Task UpdateAppointmentAsync_CancelledStatus_ThrowsInvalidAppointmentStatusException()
        {
            int id = 1;

            var dto = new UpdateAppointmentDto
            {
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };

            var appointment = new Appointment
            {
                AppointmentId = id,
                Status = AppointmentStatus.Cancelled
            };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);
            _mockAppointmentRepo.Setup(r => r.IsSlotAvailableAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot)).ReturnsAsync(true);

            await Assert.ThrowsExceptionAsync<InvalidAppointmentStatusException>(
                () => _sut.UpdateAppointmentAsync(id, dto));
        }

        [TestMethod]
        public async Task UpdateAppointmentAsync_ValidData_UpdatesAndCommits()
        {
            int id = 1;

            var dto = new UpdateAppointmentDto
            {
                DoctorId = 3,
                ScheduledDate = DateTime.Today.AddDays(2),
                TimeSlot = "11:00"
            };

            var appointment = new Appointment
            {
                AppointmentId = id,
                Status = AppointmentStatus.Pending
            };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);
            _mockAppointmentRepo.Setup(r => r.IsSlotAvailableAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot)).ReturnsAsync(true);

            await _sut.UpdateAppointmentAsync(id, dto);

            Assert.AreEqual(dto.DoctorId, appointment.DoctorId);
            Assert.AreEqual(dto.ScheduledDate, appointment.ScheduledDate);
            Assert.AreEqual(dto.TimeSlot, appointment.TimeSlot);

            _mockAppointmentRepo.Verify(r => r.UpdateAsync(appointment), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAppointmentAsync_InvalidId_ThrowsAppointmentNotFoundException()
        {
            _mockAppointmentRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsExceptionAsync<AppointmentNotFoundException>(
                () => _sut.DeleteAppointmentAsync(1));
        }

        [TestMethod]
        public async Task DeleteAppointmentAsync_PendingStatus_ThrowsAppointmentDeletionException()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Pending
            };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);

            await Assert.ThrowsExceptionAsync<AppointmentDeletionException>(
                () => _sut.DeleteAppointmentAsync(1));
        }

        [TestMethod]
        public async Task DeleteAppointmentAsync_CompletedStatus_DeletesAndCommits()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Completed
            };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);

            await _sut.DeleteAppointmentAsync(1);

            _mockAppointmentRepo.Verify(r => r.DeleteAsync(1), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public async Task ConfirmAppointmentAsync_PendingStatus_ConfirmsAndCommits()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Pending
            };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);

            await _sut.ConfirmAppointmentAsync(1);

            Assert.AreEqual(AppointmentStatus.Confirmed, appointment.Status);
            _mockAppointmentRepo.Verify(r => r.UpdateAsync(appointment), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public async Task ConfirmAppointmentAsync_NotPending_ThrowsInvalidAppointmentStatusException()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Completed
            };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);

            await Assert.ThrowsExceptionAsync<InvalidAppointmentStatusException>(
                () => _sut.ConfirmAppointmentAsync(1));
        }

        [TestMethod]
        public async Task CancelAppointmentAsync_ValidStatus_CancelsAndCommits()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Pending
            };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);

            await _sut.CancelAppointmentAsync(1, "Patient requested");

            Assert.AreEqual(AppointmentStatus.Cancelled, appointment.Status);
            _mockAppointmentRepo.Verify(r => r.UpdateAsync(appointment), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public async Task CompleteAppointmentAsync_ConfirmedStatus_CompletesAndCommits()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Confirmed
            };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);

            await _sut.CompleteAppointmentAsync(1);

            Assert.AreEqual(AppointmentStatus.Completed, appointment.Status);
            _mockAppointmentRepo.Verify(r => r.UpdateAsync(appointment), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAppointmentsByPatientAsync_ReturnsMappedAppointments()
        {
            int patientId = 1;

            var appointments = new List<Appointment> { new Appointment() };
            var dtos = new List<AppointmentDto> { new AppointmentDto() };

            _mockAppointmentRepo.Setup(r => r.GetAppointmentsByPatientAsync(patientId)).ReturnsAsync(appointments);
            _mockMapper.Setup(m => m.Map<IEnumerable<AppointmentDto>>(appointments)).Returns(dtos);

            var result = await _sut.GetAppointmentsByPatientAsync(patientId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetAppointmentsByDoctorAsync_ReturnsMappedAppointments()
        {
            int doctorId = 1;

            var appointments = new List<Appointment> { new Appointment() };
            var dtos = new List<AppointmentDto> { new AppointmentDto() };

            _mockAppointmentRepo.Setup(r => r.GetAppointmentsByDoctorAsync(doctorId)).ReturnsAsync(appointments);
            _mockMapper.Setup(m => m.Map<IEnumerable<AppointmentDto>>(appointments)).Returns(dtos);

            var result = await _sut.GetAppointmentsByDoctorAsync(doctorId);

            Assert.IsNotNull(result);
        }
    }
}