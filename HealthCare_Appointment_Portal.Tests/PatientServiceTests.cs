using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.PatientDtos;
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
    public class PatientServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IPatientRepository> _mockPatientRepo;
        private Mock<IAppointmentRepository> _mockAppointmentRepo;
        private Mock<IUserRepository> _mockUserRepo;
        private Mock<IMapper> _mockMapper;
        private PatientService _sut;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPatientRepo = new Mock<IPatientRepository>();
            _mockAppointmentRepo = new Mock<IAppointmentRepository>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(u => u.Patients).Returns(_mockPatientRepo.Object);
            _mockUnitOfWork.Setup(u => u.Appointments).Returns(_mockAppointmentRepo.Object);
            _mockUnitOfWork.Setup(u => u.Users).Returns(_mockUserRepo.Object);

            _sut = new PatientService(
                _mockUnitOfWork.Object,
                _mockMapper.Object);
        }

        [TestMethod]
        public async Task GetAllPatientsAsync_ReturnsMappedPatients()
        {
            var patients = new List<Patient>
            {
                new Patient(),
                new Patient()
            };

            var patientDtos = new List<PatientDto>
            {
                new PatientDto(),
                new PatientDto()
            };

            _mockPatientRepo
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(patients);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<PatientDto>>(patients))
                .Returns(patientDtos);

            var result = await _sut.GetAllPatientsAsync();

            Assert.AreEqual(2, result.Count());
            _mockPatientRepo.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetPatientByIdAsync_ValidId_ReturnsPatient()
        {
            int patientId = 1;

            var patient = new Patient
            {
                PatientId = patientId
            };

            var patientDto = new PatientDto
            {
                PatientId = patientId
            };

            _mockPatientRepo
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            _mockMapper
                .Setup(m => m.Map<PatientDto>(patient))
                .Returns(patientDto);

            var result = await _sut.GetPatientByIdAsync(patientId);

            Assert.IsNotNull(result);
            Assert.AreEqual(patientId, result.PatientId);
        }

        [TestMethod]
        public async Task GetPatientByIdAsync_InvalidId_ThrowsPatientNotFoundException()
        {
            _mockPatientRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(
                () => _sut.GetPatientByIdAsync(99));
        }

        [TestMethod]
        public async Task GetPatientByEmailAsync_ValidEmail_ReturnsPatient()
        {
            string email = "patient@gmail.com";

            var patient = new Patient
            {
                PatientId = 1,
                Email = email
            };

            var patientDto = new PatientDto
            {
                PatientId = 1,
                Email = email
            };

            _mockPatientRepo
                .Setup(r => r.GetPatientByEmailAsync(email))
                .ReturnsAsync(patient);

            _mockMapper
                .Setup(m => m.Map<PatientDto>(patient))
                .Returns(patientDto);

            var result = await _sut.GetPatientByEmailAsync(email);

            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
        }

        [TestMethod]
        public async Task GetPatientByEmailAsync_InvalidEmail_ThrowsPatientNotFoundException()
        {
            _mockPatientRepo
                .Setup(r => r.GetPatientByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(
                () => _sut.GetPatientByEmailAsync("wrong@gmail.com"));
        }

        [TestMethod]
        public async Task AddPatientAsync_EmailAlreadyExists_ThrowsDuplicatePatientException()
        {
            var createDto = new CreatePatientDto
            {
                Email = "patient@gmail.com"
            };

            _mockPatientRepo
                .Setup(r => r.GetPatientByEmailAsync(createDto.Email))
                .ReturnsAsync(new Patient());

            await Assert.ThrowsExceptionAsync<DuplicatePatientException>(
                () => _sut.AddPatientAsync(createDto));
        }

        [TestMethod]
        public async Task AddPatientAsync_ValidPatient_AddsPatientUserAndCommits()
        {
            var createDto = new CreatePatientDto
            {
                Email = "patient@gmail.com"
            };

            var patient = new Patient
            {
                PatientId = 1,
                Email = "patient@gmail.com"
            };

            _mockPatientRepo
                .Setup(r => r.GetPatientByEmailAsync(createDto.Email))
                .ReturnsAsync((Patient)null);

            _mockMapper
                .Setup(m => m.Map<Patient>(createDto))
                .Returns(patient);

            var result = await _sut.AddPatientAsync(createDto);

            Assert.AreEqual(1, result);

            _mockPatientRepo.Verify(
                r => r.AddAsync(patient),
                Times.Once);

            _mockUserRepo.Verify(
                r => r.AddAsync(It.Is<User>(u =>
                    u.UserCode == "P001" &&
                    u.Email == "patient@gmail.com" &&
                    u.Role == Role.Patient &&
                    u.ReferenceId == 1)),
                Times.Once);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(),
                Times.Exactly(2));
        }

        [TestMethod]
        public async Task UpdatePatientAsync_InvalidId_ThrowsPatientNotFoundException()
        {
            _mockPatientRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(
                () => _sut.UpdatePatientAsync(1, new UpdatePatientDto()));
        }

        [TestMethod]
        public async Task UpdatePatientAsync_EmailBelongsToAnotherPatient_ThrowsDuplicatePatientException()
        {
            int patientId = 1;

            var updateDto = new UpdatePatientDto
            {
                Email = "same@gmail.com"
            };

            var currentPatient = new Patient
            {
                PatientId = patientId,
                Email = "old@gmail.com"
            };

            var existingPatient = new Patient
            {
                PatientId = 2,
                Email = "same@gmail.com"
            };

            _mockPatientRepo
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(currentPatient);

            _mockPatientRepo
                .Setup(r => r.GetPatientByEmailAsync(updateDto.Email))
                .ReturnsAsync(existingPatient);

            await Assert.ThrowsExceptionAsync<DuplicatePatientException>(
                () => _sut.UpdatePatientAsync(patientId, updateDto));
        }

        [TestMethod]
        public async Task UpdatePatientAsync_ValidPatient_UpdatesAndCommits()
        {
            int patientId = 1;

            var updateDto = new UpdatePatientDto
            {
                Email = "new@gmail.com"
            };

            var patient = new Patient
            {
                PatientId = patientId,
                Email = "old@gmail.com"
            };

            _mockPatientRepo
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            _mockPatientRepo
                .Setup(r => r.GetPatientByEmailAsync(updateDto.Email))
                .ReturnsAsync((Patient)null);

            await _sut.UpdatePatientAsync(patientId, updateDto);

            _mockMapper.Verify(
                m => m.Map(updateDto, patient),
                Times.Once);

            _mockPatientRepo.Verify(
                r => r.UpdateAsync(patient),
                Times.Once);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(),
                Times.Once);
        }

        [TestMethod]
        public async Task DeletePatientAsync_InvalidId_ThrowsPatientNotFoundException()
        {
            _mockPatientRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(
                () => _sut.DeletePatientAsync(1));
        }

        [TestMethod]
        public async Task DeletePatientAsync_HasConfirmedAppointments_ThrowsPatientDeletionException()
        {
            int patientId = 1;

            var patient = new Patient
            {
                PatientId = patientId
            };

            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    Status = AppointmentStatus.Confirmed
                }
            };

            _mockPatientRepo
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            _mockAppointmentRepo
                .Setup(r => r.GetAppointmentsByPatientAsync(patientId))
                .ReturnsAsync(appointments);

            await Assert.ThrowsExceptionAsync<PatientDeletionException>(
                () => _sut.DeletePatientAsync(patientId));

            _mockPatientRepo.Verify(
                r => r.DeleteAsync(It.IsAny<int>()),
                Times.Never);
        }

        [TestMethod]
        public async Task DeletePatientAsync_NoConfirmedAppointments_DeletesPatient()
        {
            int patientId = 1;

            var patient = new Patient
            {
                PatientId = patientId
            };

            var appointments = new List<Appointment>();

            _mockPatientRepo
                .Setup(r => r.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            _mockAppointmentRepo
                .Setup(r => r.GetAppointmentsByPatientAsync(patientId))
                .ReturnsAsync(appointments);

            await _sut.DeletePatientAsync(patientId);

            _mockPatientRepo.Verify(
                r => r.DeleteAsync(patientId),
                Times.Once);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(),
                Times.Once);
        }
    }
}