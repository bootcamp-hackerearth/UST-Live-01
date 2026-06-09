using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Tests
{
    public class AppointmentServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;

        private Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private Mock<IPatientRepository> _patientRepositoryMock;
        private Mock<IDoctorRepository> _doctorRepositoryMock;

        private AppointmentService _service;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock =
                new Mock<IUnitOfWork>();

            _mapperMock =
                new Mock<IMapper>();

            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _patientRepositoryMock =
                new Mock<IPatientRepository>();

            _doctorRepositoryMock =
                new Mock<IDoctorRepository>();

            _unitOfWorkMock
                .Setup(x => x.Appointments)
                .Returns(_appointmentRepositoryMock.Object);

            _unitOfWorkMock
                .Setup(x => x.Patients)
                .Returns(_patientRepositoryMock.Object);

            _unitOfWorkMock
                .Setup(x => x.Doctors)
                .Returns(_doctorRepositoryMock.Object);

            _service =
                new AppointmentService(
                    _unitOfWorkMock.Object,
                    _mapperMock.Object);
        }

        [TestMethod]
        public async Task AddAppointmentAsync_InvalidPatient_ShouldThrowPatientNotFoundException()
        {
            // Arrange

            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Patient)null);

            // Act + Assert

            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_ValidAppointment_ShouldReturnAppointmentId()
        {
            // Arrange

            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            var patient = new Patient
            {
                PatientId = 1
            };

            var doctor = new Doctor
            {
                DoctorId = 1,
                IsActive = true
            };

            var appointment = new Appointment
            {
                AppointmentId = 100
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
                        1,
                        dto.ScheduledDate,
                        dto.TimeSlot))
                .ReturnsAsync(true);

            _mapperMock
                .Setup(x =>
                    x.Map<Appointment>(dto))
                .Returns(appointment);

            // Act

            var result =
                await _service.AddAppointmentAsync(dto);

            // Assert

            Assert.AreEqual(100, result);

            _appointmentRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Appointment>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [TestMethod]
        public async Task GetAppointmentByIdAsync_ExistingId_ShouldReturnAppointment()
        {
            // Arrange

            var appointment = new Appointment
            {
                AppointmentId = 1
            };

            var dto = new AppointmentDto
            {
                AppointmentId = 1
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(dto);

            // Act

            var result =
                await _service.GetAppointmentByIdAsync(1);

            // Assert

            Assert.IsNotNull(result);

            Assert.AreEqual(
                1,
                result.AppointmentId);
        }

        [TestMethod]
        public async Task GetAppointmentByIdAsync_InvalidId_ShouldThrowAppointmentNotFoundException()
        {
            // Arrange

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Appointment)null);

            // Act + Assert

            await Assert.ThrowsExceptionAsync<AppointmentNotFoundException>(
                () => _service.GetAppointmentByIdAsync(1));
        }

      
        [TestMethod]
        public async Task AddAppointmentAsync_InvalidDoctor_ShouldThrowDoctorNotFoundException()
        {
            // Arrange

            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Doctor)null);

            // Act + Assert

            await Assert.ThrowsExceptionAsync<DoctorNotFoundException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_PastDate_ShouldThrowPastDateException()
        {
            // Arrange

            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(-1),
                TimeSlot = "10:00 AM"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(
                    new Doctor
                    {
                        IsActive = true
                    });

            // Act + Assert

            await Assert.ThrowsExceptionAsync<PastDateException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_InactiveDoctor_ShouldThrowDoctorUnavailableException()
        {
            // Arrange

            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(
                    new Doctor
                    {
                        IsActive = false
                    });

            // Act + Assert

            await Assert.ThrowsExceptionAsync<DoctorUnavailableException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [TestMethod]
        public async Task AddAppointmentAsync_Conflict_ShouldThrowAppointmentConflictException()
        {
            // Arrange

            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(
                    new Doctor
                    {
                        IsActive = true
                    });

            _appointmentRepositoryMock
                .Setup(x =>
                    x.IsSlotAvailableAsync(
                        dto.DoctorId,
                        dto.ScheduledDate,
                        dto.TimeSlot))
                .ReturnsAsync(false);

            // Act + Assert

            await Assert.ThrowsExceptionAsync<AppointmentConflictException>(
                () => _service.AddAppointmentAsync(dto));
        }


    }
}
