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
        private AppointmentService _sut; // System Under Test

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAppointmentRepo = new Mock<IAppointmentRepository>();
            _mockPatientRepo = new Mock<IPatientRepository>();
            _mockDoctorRepo = new Mock<IDoctorRepository>();
            _mockMapper = new Mock<IMapper>();

            // Wire up the Unit Of Work to return our mocked repositories
            _mockUnitOfWork.Setup(u => u.Appointments).Returns(_mockAppointmentRepo.Object);
            _mockUnitOfWork.Setup(u => u.Patients).Returns(_mockPatientRepo.Object);
            _mockUnitOfWork.Setup(u => u.Doctors).Returns(_mockDoctorRepo.Object);

            _sut = new AppointmentService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        #region Retrieval Tests (GetAll, GetById, etc.)

        [TestMethod]
        public async Task GetAllAppointmentsAsync_ReturnsMappedAppointments()
        {
            // Arrange
            var appointments = new List<Appointment> { new Appointment(), new Appointment() };
            var dtos = new List<AppointmentDto> { new AppointmentDto(), new AppointmentDto() };

            _mockAppointmentRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(appointments);
            _mockMapper.Setup(m => m.Map<IEnumerable<AppointmentDto>>(appointments)).Returns(dtos);

            // Act
            var result = await _sut.GetAllAppointmentsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
            _mockAppointmentRepo.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAppointmentByIdAsync_ValidId_ReturnsAppointment()
        {
            // Arrange
            int id = 1;
            var appointment = new Appointment { AppointmentId = id };
            var dto = new AppointmentDto { AppointmentId = id };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);
            _mockMapper.Setup(m => m.Map<AppointmentDto>(appointment)).Returns(dto);

            // Act
            var result = await _sut.GetAppointmentByIdAsync(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.AppointmentId);
        }

        [TestMethod]
        public async Task GetAppointmentByIdAsync_InvalidId_ThrowsAppointmentNotFoundException()
        {
            // Arrange
            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Appointment)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<AppointmentNotFoundException>(() => _sut.GetAppointmentByIdAsync(99));
        }

        [TestMethod]
        public async Task GetNextAppointmentByPatientAsync_HasAppointment_ReturnsDto()
        {
            // Arrange
            int patientId = 1;
            var appointment = new Appointment { AppointmentId = 10 };
            var dto = new AppointmentDto { AppointmentId = 10 };

            _mockAppointmentRepo.Setup(r => r.GetNextAppointmentByPatientAsync(patientId)).ReturnsAsync(appointment);
            _mockMapper.Setup(m => m.Map<AppointmentDto>(appointment)).Returns(dto);

            // Act
            var result = await _sut.GetNextAppointmentByPatientAsync(patientId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.AppointmentId);
        }

        [TestMethod]
        public async Task GetNextAppointmentByPatientAsync_NoAppointment_ReturnsNull()
        {
            // Arrange
            int patientId = 1;
            _mockAppointmentRepo.Setup(r => r.GetNextAppointmentByPatientAsync(patientId)).ReturnsAsync((Appointment)null);

            // Act
            var result = await _sut.GetNextAppointmentByPatientAsync(patientId);

            // Assert
            Assert.IsNull(result);
        }

        #endregion

        #region AddAppointmentAsync Tests

        [TestMethod]
        public async Task AddAppointmentAsync_PatientNotFound_ThrowsException()
        {
            // Arrange
            var dto = new CreateAppointmentDto { PatientId = 1 };
            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync((Patient)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(() => _sut.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_DoctorNotFound_ThrowsException()
        {
            // Arrange
            var dto = new CreateAppointmentDto { PatientId = 1, DoctorId = 2 };
            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync(new Patient());
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(dto.DoctorId)).ReturnsAsync((Doctor)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<DoctorNotFoundException>(() => _sut.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_PastDate_ThrowsException()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(-1) // Past Date
            };
            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync(new Patient());
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(dto.DoctorId)).ReturnsAsync(new Doctor());

            // Act & Assert
            await Assert.ThrowsExceptionAsync<PastDateException>(() => _sut.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_ExceedsSixMonths_ThrowsException()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddMonths(7) // > 6 months
            };
            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync(new Patient());
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(dto.DoctorId)).ReturnsAsync(new Doctor());

            // Act & Assert
            await Assert.ThrowsExceptionAsync<AdvanceBookingLimitException>(() => _sut.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_PatientAlreadyBooked_ThrowsException()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1)
            };
            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync(new Patient());
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(dto.DoctorId)).ReturnsAsync(new Doctor());
            _mockAppointmentRepo.Setup(r => r.HasAppointmentForPatientOnDateAsync(dto.PatientId, dto.ScheduledDate))
                .ReturnsAsync(true); // Already booked

            // Act & Assert
            await Assert.ThrowsExceptionAsync<DuplicatePatientAppointmentException>(() => _sut.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_DoctorInactive_ThrowsException()
        {
            // Arrange
            var dto = new CreateAppointmentDto { PatientId = 1, DoctorId = 2, ScheduledDate = DateTime.Today.AddDays(1) };
            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync(new Patient());
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(dto.DoctorId)).ReturnsAsync(new Doctor { IsActive = false });
            _mockAppointmentRepo.Setup(r => r.HasAppointmentForPatientOnDateAsync(dto.PatientId, dto.ScheduledDate)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<DoctorUnavailableException>(() => _sut.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_SlotUnavailable_ThrowsException()
        {
            // Arrange
            // FIX: Changed TimeSlot to a string
            var dto = new CreateAppointmentDto { PatientId = 1, DoctorId = 2, ScheduledDate = DateTime.Today.AddDays(1), TimeSlot = "10:00" };
            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync(new Patient());
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(dto.DoctorId)).ReturnsAsync(new Doctor { IsActive = true });
            _mockAppointmentRepo.Setup(r => r.HasAppointmentForPatientOnDateAsync(dto.PatientId, dto.ScheduledDate)).ReturnsAsync(false);
            _mockAppointmentRepo.Setup(r => r.IsSlotAvailableAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot)).ReturnsAsync(false); // Slot taken

            // Act & Assert
            await Assert.ThrowsExceptionAsync<AppointmentConflictException>(() => _sut.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_ValidData_AddsAndCommits()
        {
            // Arrange
            // FIX: Changed TimeSlot to a string
            var dto = new CreateAppointmentDto { PatientId = 1, DoctorId = 2, ScheduledDate = DateTime.Today.AddDays(1), TimeSlot = "10:00" };
            var mappedAppointment = new Appointment { AppointmentId = 5 };

            _mockPatientRepo.Setup(r => r.GetByIdAsync(dto.PatientId)).ReturnsAsync(new Patient());
            _mockDoctorRepo.Setup(r => r.GetByIdAsync(dto.DoctorId)).ReturnsAsync(new Doctor { IsActive = true });
            _mockAppointmentRepo.Setup(r => r.HasAppointmentForPatientOnDateAsync(dto.PatientId, dto.ScheduledDate)).ReturnsAsync(false);
            _mockAppointmentRepo.Setup(r => r.IsSlotAvailableAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot)).ReturnsAsync(true);
            _mockMapper.Setup(m => m.Map<Appointment>(dto)).Returns(mappedAppointment);

            // Act
            var result = await _sut.AddAppointmentAsync(dto);

            // Assert
            Assert.AreEqual(5, result);
            Assert.AreEqual(AppointmentStatus.Pending, mappedAppointment.Status);
            _mockAppointmentRepo.Verify(r => r.AddAsync(mappedAppointment), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        #endregion

        #region UpdateAppointmentAsync Tests

        [TestMethod]
        public async Task UpdateAppointmentAsync_CompletedStatus_ThrowsException()
        {
            // Arrange
            int id = 1;
            // FIX: Changed TimeSlot to a string
            var dto = new UpdateAppointmentDto { DoctorId = 2, ScheduledDate = DateTime.Today.AddDays(1), TimeSlot = "10:00" };
            var existingAppt = new Appointment { AppointmentId = id, Status = AppointmentStatus.Completed };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingAppt);
            _mockAppointmentRepo.Setup(r => r.IsSlotAvailableAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidAppointmentStatusException>(() => _sut.UpdateAppointmentAsync(id, dto));
        }

        [TestMethod]
        public async Task UpdateAppointmentAsync_ValidUpdate_UpdatesAndCommits()
        {
            // Arrange
            int id = 1;
            // FIX: Changed TimeSlot to a string
            var dto = new UpdateAppointmentDto { DoctorId = 3, ScheduledDate = DateTime.Today.AddDays(2), TimeSlot = "11:00" };
            var existingAppt = new Appointment { AppointmentId = id, Status = AppointmentStatus.Pending };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingAppt);
            _mockAppointmentRepo.Setup(r => r.IsSlotAvailableAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot)).ReturnsAsync(true);

            // Act
            await _sut.UpdateAppointmentAsync(id, dto);

            // Assert
            Assert.AreEqual(dto.DoctorId, existingAppt.DoctorId);
            Assert.AreEqual(dto.ScheduledDate, existingAppt.ScheduledDate);
            Assert.AreEqual(dto.TimeSlot, existingAppt.TimeSlot);
            _mockAppointmentRepo.Verify(r => r.UpdateAsync(existingAppt), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        #endregion

        #region Status Change & Delete Tests (Delete, Confirm, Cancel, Complete)

        [TestMethod]
        public async Task DeleteAppointmentAsync_PendingOrConfirmed_ThrowsException()
        {
            // Arrange
            int id = 1;
            var appointment = new Appointment { AppointmentId = id, Status = AppointmentStatus.Pending };
            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<AppointmentDeletionException>(() => _sut.DeleteAppointmentAsync(id));
        }

        [TestMethod]
        public async Task DeleteAppointmentAsync_CompletedOrCancelled_DeletesSuccessfully()
        {
            // Arrange
            int id = 1;
            var appointment = new Appointment { AppointmentId = id, Status = AppointmentStatus.Completed };
            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);

            // Act
            await _sut.DeleteAppointmentAsync(id);

            // Assert
            _mockAppointmentRepo.Verify(r => r.DeleteAsync(id), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public async Task ConfirmAppointmentAsync_NotPending_ThrowsException()
        {
            // Arrange
            int id = 1;
            var appointment = new Appointment { AppointmentId = id, Status = AppointmentStatus.Confirmed };
            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidAppointmentStatusException>(() => _sut.ConfirmAppointmentAsync(id));
        }

        [TestMethod]
        public async Task CancelAppointmentAsync_NotPendingOrConfirmed_ThrowsException()
        {
            // Arrange
            int id = 1;
            var appointment = new Appointment { AppointmentId = id, Status = AppointmentStatus.Completed };
            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidAppointmentStatusException>(() => _sut.CancelAppointmentAsync(id, "No Show"));
        }

        [TestMethod]
        public async Task CancelAppointmentAsync_ValidStatus_UpdatesAndCommits()
        {
            // Arrange
            int id = 1;
            string cancelReason = "Patient requested";

            // Use a REAL instance of your entity, not a mock
            var appointment = new Appointment
            {
                AppointmentId = id,
                Status = AppointmentStatus.Pending
            };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);

            // Act
            await _sut.CancelAppointmentAsync(id, cancelReason);

            // Assert
            // Verify the state of the real object changed. 
            // The Cancel() method inside your entity should have set the status to Cancelled.
            Assert.AreEqual(AppointmentStatus.Cancelled, appointment.Status);

            // Note: If your Appointment model has a "CancellationReason" property, uncomment this:
            // Assert.AreEqual(cancelReason, appointment.CancellationReason);

            _mockAppointmentRepo.Verify(r => r.UpdateAsync(appointment), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestMethod]
        public async Task CompleteAppointmentAsync_NotConfirmed_ThrowsException()
        {
            // Arrange
            int id = 1;
            var appointment = new Appointment { AppointmentId = id, Status = AppointmentStatus.Pending };
            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(appointment);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidAppointmentStatusException>(() => _sut.CompleteAppointmentAsync(id));
        }

        #endregion

        #region List Retrieval Passthrough Tests

        [TestMethod]
        public async Task GetAppointmentsByPatientAsync_ReturnsMappedCollection()
        {
            // Arrange
            int patientId = 1;
            _mockAppointmentRepo.Setup(r => r.GetAppointmentsByPatientAsync(patientId)).ReturnsAsync(new List<Appointment>());
            _mockMapper.Setup(m => m.Map<IEnumerable<AppointmentDto>>(It.IsAny<IEnumerable<Appointment>>())).Returns(new List<AppointmentDto>());

            // Act
            var result = await _sut.GetAppointmentsByPatientAsync(patientId);

            // Assert
            Assert.IsNotNull(result);
            _mockAppointmentRepo.Verify(r => r.GetAppointmentsByPatientAsync(patientId), Times.Once);
        }

        [TestMethod]
        public async Task GetTodayScheduleAsync_ReturnsMappedCollection()
        {
            // Arrange
            int doctorId = 1;
            _mockAppointmentRepo.Setup(r => r.GetTodayScheduleAsync(doctorId)).ReturnsAsync(new List<Appointment>());
            _mockMapper.Setup(m => m.Map<IEnumerable<AppointmentDto>>(It.IsAny<IEnumerable<Appointment>>())).Returns(new List<AppointmentDto>());

            // Act
            var result = await _sut.GetTodayScheduleAsync(doctorId);

            // Assert
            Assert.IsNotNull(result);
            _mockAppointmentRepo.Verify(r => r.GetTodayScheduleAsync(doctorId), Times.Once);
        }

        #endregion
    }
}