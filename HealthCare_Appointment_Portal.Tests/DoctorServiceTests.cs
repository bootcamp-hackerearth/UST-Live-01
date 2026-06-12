using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    [TestClass]
    public class DoctorServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IDoctorRepository> _mockDoctorRepo;
        private Mock<IAppointmentRepository> _mockAppointmentRepo;
        private Mock<IUserRepository> _mockUserRepo;
        private Mock<IMapper> _mockMapper;
        private DoctorService _sut;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockDoctorRepo = new Mock<IDoctorRepository>();
            _mockAppointmentRepo = new Mock<IAppointmentRepository>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(u => u.Doctors).Returns(_mockDoctorRepo.Object);
            _mockUnitOfWork.Setup(u => u.Appointments).Returns(_mockAppointmentRepo.Object);
            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUserRepo.Object);

            _sut = new DoctorService(
                _mockUnitOfWork.Object,
                _mockMapper.Object);
        }

        [TestMethod]
        public async Task GetAllDoctorsAsync_ReturnsMappedDoctors()
        {
            var doctors = new List<Doctor>
            {
                new Doctor(),
                new Doctor()
            };

            var doctorDtos = new List<DoctorDto>
            {
                new DoctorDto(),
                new DoctorDto()
            };

            _mockDoctorRepo
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(doctors);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result = await _sut.GetAllDoctorsAsync();

            Assert.AreEqual(2, result.Count());
            _mockDoctorRepo.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetDoctorByIdAsync_ValidId_ReturnsDoctor()
        {
            int doctorId = 1;

            var doctor = new Doctor
            {
                DoctorId = doctorId
            };

            var doctorDto = new DoctorDto
            {
                DoctorId = doctorId
            };

            _mockDoctorRepo
                .Setup(r => r.GetByIdAsync(doctorId))
                .ReturnsAsync(doctor);

            _mockMapper
                .Setup(m => m.Map<DoctorDto>(doctor))
                .Returns(doctorDto);

            var result = await _sut.GetDoctorByIdAsync(doctorId);

            Assert.IsNotNull(result);
            Assert.AreEqual(doctorId, result.DoctorId);
        }

        [TestMethod]
        public async Task GetDoctorByIdAsync_InvalidId_ThrowsDoctorNotFoundException()
        {
            _mockDoctorRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsExceptionAsync<DoctorNotFoundException>(
                () => _sut.GetDoctorByIdAsync(99));
        }

        [TestMethod]
        public async Task AddDoctorAsync_ValidDoctor_AddsDoctorUserAndCommits()
        {
            var createDto = new CreateDoctorDto();

            var doctor = new Doctor
            {
                DoctorId = 1
            };

            _mockMapper
                .Setup(m => m.Map<Doctor>(createDto))
                .Returns(doctor);

            var result = await _sut.AddDoctorAsync(createDto);

            Assert.AreEqual(1, result);

            _mockDoctorRepo.Verify(
                r => r.AddAsync(doctor),
                Times.Once);

            _mockUserRepo.Verify(
                r => r.AddAsync(It.Is<User>(u =>
                    u.UserCode == "D001" &&
                    u.Email == "doctor1@hospital.com" &&
                    u.Role == Role.Doctor &&
                    u.ReferenceId == 1)),
                Times.Once);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(),
                Times.Exactly(2));
        }

        [TestMethod]
        public async Task UpdateDoctorAsync_InvalidId_ThrowsDoctorNotFoundException()
        {
            _mockDoctorRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsExceptionAsync<DoctorNotFoundException>(
                () => _sut.UpdateDoctorAsync(1, new UpdateDoctorDto()));
        }

        [TestMethod]
        public async Task UpdateDoctorAsync_ValidDoctor_UpdatesAndCommits()
        {
            int doctorId = 1;

            var updateDto = new UpdateDoctorDto();

            var doctor = new Doctor
            {
                DoctorId = doctorId
            };

            _mockDoctorRepo
                .Setup(r => r.GetByIdAsync(doctorId))
                .ReturnsAsync(doctor);

            await _sut.UpdateDoctorAsync(doctorId, updateDto);

            _mockMapper.Verify(
                m => m.Map(updateDto, doctor),
                Times.Once);

            _mockDoctorRepo.Verify(
                r => r.UpdateAsync(doctor),
                Times.Once);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(),
                Times.Once);
        }

        [TestMethod]
        public async Task DeleteDoctorAsync_InvalidId_ThrowsDoctorNotFoundException()
        {
            _mockDoctorRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsExceptionAsync<DoctorNotFoundException>(
                () => _sut.DeleteDoctorAsync(1));
        }

        [TestMethod]
        public async Task DeleteDoctorAsync_HasConfirmedAppointments_ThrowsDoctorDeletionException()
        {
            int doctorId = 1;

            var doctor = new Doctor
            {
                DoctorId = doctorId
            };

            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    Status = AppointmentStatus.Confirmed
                }
            };

            _mockDoctorRepo
                .Setup(r => r.GetByIdAsync(doctorId))
                .ReturnsAsync(doctor);

            _mockAppointmentRepo
                .Setup(r => r.GetAppointmentsByDoctorAsync(doctorId))
                .ReturnsAsync(appointments);

            await Assert.ThrowsExceptionAsync<DoctorDeletionException>(
                () => _sut.DeleteDoctorAsync(doctorId));

            _mockDoctorRepo.Verify(
                r => r.DeleteAsync(It.IsAny<int>()),
                Times.Never);
        }

        [TestMethod]
        public async Task DeleteDoctorAsync_HasPendingAppointments_CancelsPendingAndDeletesDoctor()
        {
            int doctorId = 1;

            var doctor = new Doctor
            {
                DoctorId = doctorId
            };

            var pendingAppointment = new Appointment
            {
                AppointmentId = 10,
                Status = AppointmentStatus.Pending
            };

            var appointments = new List<Appointment>
            {
                pendingAppointment
            };

            _mockDoctorRepo
                .Setup(r => r.GetByIdAsync(doctorId))
                .ReturnsAsync(doctor);

            _mockAppointmentRepo
                .Setup(r => r.GetAppointmentsByDoctorAsync(doctorId))
                .ReturnsAsync(appointments);

            await _sut.DeleteDoctorAsync(doctorId);

            Assert.AreEqual(
                AppointmentStatus.Cancelled,
                pendingAppointment.Status);

            _mockAppointmentRepo.Verify(
                r => r.UpdateAsync(pendingAppointment),
                Times.Once);

            _mockDoctorRepo.Verify(
                r => r.DeleteAsync(doctorId),
                Times.Once);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(),
                Times.Once);
        }

        [TestMethod]
        public async Task DeleteDoctorAsync_NoConfirmedAppointments_DeletesDoctor()
        {
            int doctorId = 1;

            var doctor = new Doctor
            {
                DoctorId = doctorId
            };

            var appointments = new List<Appointment>();

            _mockDoctorRepo
                .Setup(r => r.GetByIdAsync(doctorId))
                .ReturnsAsync(doctor);

            _mockAppointmentRepo
                .Setup(r => r.GetAppointmentsByDoctorAsync(doctorId))
                .ReturnsAsync(appointments);

            await _sut.DeleteDoctorAsync(doctorId);

            _mockDoctorRepo.Verify(
                r => r.DeleteAsync(doctorId),
                Times.Once);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(),
                Times.Once);
        }

        [TestMethod]
        public async Task GetDoctorsBySpecialisationAsync_ReturnsMappedDoctors()
        {
            var specialisation = Specialisation.Cardiologist;

            var doctors = new List<Doctor>
            {
                new Doctor(),
                new Doctor()
            };

            var doctorDtos = new List<DoctorDto>
            {
                new DoctorDto(),
                new DoctorDto()
            };

            _mockDoctorRepo
                .Setup(r => r.GetDoctorsBySpecialisationAsync(specialisation))
                .ReturnsAsync(doctors);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result =
                await _sut.GetDoctorsBySpecialisationAsync(specialisation);

            Assert.AreEqual(2, result.Count());

            _mockDoctorRepo.Verify(
                r => r.GetDoctorsBySpecialisationAsync(specialisation),
                Times.Once);
        }
    }
}