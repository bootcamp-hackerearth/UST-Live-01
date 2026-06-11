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
        private Mock<IPatientRepository> _mockPatientRepo;
        private Mock<IAppointmentRepository> _mockAppointmentRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<IUserRepository> _mockUserRepo;
        private PatientService _sut; // System Under Test

        [TestInitialize]
        public void Setup()
        {
            // Arrange - Common setup runs before EVERY test in MSTest
            _mockPatientRepo = new Mock<IPatientRepository>();
            _mockAppointmentRepo = new Mock<IAppointmentRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockUserRepo = new Mock<IUserRepository>();

            _sut = new PatientService(
                _mockPatientRepo.Object,
                _mockUserRepo.Object,
                _mockAppointmentRepo.Object,
                _mockMapper.Object);
        }

        #region GetAllPatientsAsync Tests

        [TestMethod]
        public async Task GetAllPatientsAsync_NoSearchTerm_ReturnsAllPatients()
        {
            // Arrange
            var patients = new List<Patient> { new Patient(), new Patient() };
            var patientDtos = new List<PatientDto> { new PatientDto(), new PatientDto() };

            _mockPatientRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(patients);
            _mockMapper.Setup(m => m.Map<IEnumerable<PatientDto>>(patients)).Returns(patientDtos);

            // Act
            var result = await _sut.GetAllPatientsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            _mockPatientRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockPatientRepo.Verify(repo => repo.SearchPatientsAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetAllPatientsAsync_WithSearchTerm_ReturnsFilteredPatients()
        {
            // Arrange
            string searchTerm = "John";
            var patients = new List<Patient> { new Patient() };
            var patientDtos = new List<PatientDto> { new PatientDto() };

            _mockPatientRepo.Setup(repo => repo.SearchPatientsAsync(searchTerm)).ReturnsAsync(patients);
            _mockMapper.Setup(m => m.Map<IEnumerable<PatientDto>>(patients)).Returns(patientDtos);

            // Act
            var result = await _sut.GetAllPatientsAsync(searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            _mockPatientRepo.Verify(repo => repo.SearchPatientsAsync(searchTerm), Times.Once);
            _mockPatientRepo.Verify(repo => repo.GetAllAsync(), Times.Never);
        }

        #endregion

        #region GetPatientByIdAsync Tests

        [TestMethod]
        public async Task GetPatientByIdAsync_PatientExists_ReturnsPatientDto()
        {
            // Arrange
            int patientId = 1;
            var patient = new Patient { PatientId = patientId };
            var patientDto = new PatientDto { PatientId = patientId };

            _mockPatientRepo.Setup(repo => repo.GetByIdAsync(patientId)).ReturnsAsync(patient);
            _mockMapper.Setup(m => m.Map<PatientDto>(patient)).Returns(patientDto);

            // Act
            var result = await _sut.GetPatientByIdAsync(patientId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(patientId, result.PatientId);
        }

        [TestMethod]
        public async Task GetPatientByIdAsync_PatientDoesNotExist_ThrowsPatientNotFoundException()
        {
            // Arrange
            int patientId = 1;
            _mockPatientRepo.Setup(repo => repo.GetByIdAsync(patientId)).ReturnsAsync((Patient)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(() => _sut.GetPatientByIdAsync(patientId));
        }

        #endregion

        #region GetPatientByEmailAsync Tests

        [TestMethod]
        public async Task GetPatientByEmailAsync_PatientExists_ReturnsPatientDto()
        {
            // Arrange
            string email = "test@test.com";
            var patient = new Patient { Email = email };
            var patientDto = new PatientDto { Email = email };

            _mockPatientRepo.Setup(repo => repo.GetPatientByEmailAsync(email)).ReturnsAsync(patient);
            _mockMapper.Setup(m => m.Map<PatientDto>(patient)).Returns(patientDto);

            // Act
            var result = await _sut.GetPatientByEmailAsync(email);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
        }

        [TestMethod]
        public async Task GetPatientByEmailAsync_PatientDoesNotExist_ThrowsPatientNotFoundException()
        {
            // Arrange
            string email = "test@test.com";
            _mockPatientRepo.Setup(repo => repo.GetPatientByEmailAsync(email)).ReturnsAsync((Patient)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(() => _sut.GetPatientByEmailAsync(email));
        }

        #endregion

        #region AddPatientAsync Tests

        [TestMethod]
        public async Task AddPatientAsync_ValidPatient_AddsAndReturnsId()
        {
            // Arrange
            var createDto = new CreatePatientDto { Email = "new@test.com" };
            var mappedPatient = new Patient { PatientId = 1, Email = "new@test.com" };

            _mockPatientRepo.Setup(repo => repo.GetPatientByEmailAsync(createDto.Email)).ReturnsAsync((Patient)null);
            _mockMapper.Setup(m => m.Map<Patient>(createDto)).Returns(mappedPatient);
            _mockPatientRepo.Setup(repo => repo.AddAsync(mappedPatient)).Returns(Task.CompletedTask);

            // Act
            var result = await _sut.AddPatientAsync(createDto);

            // Assert
            Assert.AreEqual(1, result);
            _mockPatientRepo.Verify(repo => repo.AddAsync(mappedPatient), Times.Once);
        }

        [TestMethod]
        public async Task AddPatientAsync_DuplicateEmail_ThrowsDuplicatePatientException()
        {
            // Arrange
            var createDto = new CreatePatientDto { Email = "existing@test.com" };
            var existingPatient = new Patient { Email = "existing@test.com" };

            _mockPatientRepo.Setup(repo => repo.GetPatientByEmailAsync(createDto.Email)).ReturnsAsync(existingPatient);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<DuplicatePatientException>(() => _sut.AddPatientAsync(createDto));
            _mockPatientRepo.Verify(repo => repo.AddAsync(It.IsAny<Patient>()), Times.Never);
        }

        #endregion

        #region UpdatePatientAsync Tests

        [TestMethod]
        public async Task UpdatePatientAsync_ValidData_UpdatesSuccessfully()
        {
            // Arrange
            int patientId = 1;
            var updateDto = new UpdatePatientDto { Email = "update@test.com" };
            var existingPatient = new Patient { PatientId = patientId, Email = "old@test.com" };

            _mockPatientRepo.Setup(repo => repo.GetByIdAsync(patientId)).ReturnsAsync(existingPatient);
            _mockPatientRepo.Setup(repo => repo.GetPatientByEmailAsync(updateDto.Email)).ReturnsAsync((Patient)null);

            // Act
            await _sut.UpdatePatientAsync(patientId, updateDto);

            // Assert
            _mockMapper.Verify(m => m.Map(updateDto, existingPatient), Times.Once);
            _mockPatientRepo.Verify(repo => repo.UpdateAsync(existingPatient), Times.Once);
        }

        [TestMethod]
        public async Task UpdatePatientAsync_PatientNotFound_ThrowsPatientNotFoundException()
        {
            // Arrange
            int patientId = 1;
            var updateDto = new UpdatePatientDto { Email = "update@test.com" };

            _mockPatientRepo.Setup(repo => repo.GetByIdAsync(patientId)).ReturnsAsync((Patient)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(() => _sut.UpdatePatientAsync(patientId, updateDto));
        }

        [TestMethod]
        public async Task UpdatePatientAsync_EmailBelongsToAnotherPatient_ThrowsDuplicatePatientException()
        {
            // Arrange
            int patientId = 1;
            var updateDto = new UpdatePatientDto { Email = "taken@test.com" };
            var currentPatient = new Patient { PatientId = patientId, Email = "old@test.com" };
            var otherPatient = new Patient { PatientId = 2, Email = "taken@test.com" };

            _mockPatientRepo.Setup(repo => repo.GetByIdAsync(patientId)).ReturnsAsync(currentPatient);
            _mockPatientRepo.Setup(repo => repo.GetPatientByEmailAsync(updateDto.Email)).ReturnsAsync(otherPatient);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<DuplicatePatientException>(() => _sut.UpdatePatientAsync(patientId, updateDto));
            _mockPatientRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Patient>()), Times.Never);
        }

        #endregion

        #region DeletePatientAsync Tests

        [TestMethod]
        public async Task DeletePatientAsync_NoConfirmedAppointments_DeletesSuccessfully()
        {
            // Arrange
            int patientId = 1;
            var patient = new Patient { PatientId = patientId };
            var appointments = new List<Appointment>
            {
                new Appointment { Status = AppointmentStatus.Cancelled },
                new Appointment { Status = AppointmentStatus.Completed }
            };

            _mockPatientRepo.Setup(repo => repo.GetByIdAsync(patientId)).ReturnsAsync(patient);
            _mockAppointmentRepo.Setup(repo => repo.GetAppointmentsByPatientAsync(patientId)).ReturnsAsync(appointments);

            // Act
            await _sut.DeletePatientAsync(patientId);

            // Assert
            _mockPatientRepo.Verify(repo => repo.DeleteAsync(patientId), Times.Once);
        }

        [TestMethod]
        public async Task DeletePatientAsync_PatientNotFound_ThrowsPatientNotFoundException()
        {
            // Arrange
            int patientId = 1;
            _mockPatientRepo.Setup(repo => repo.GetByIdAsync(patientId)).ReturnsAsync((Patient)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(() => _sut.DeletePatientAsync(patientId));
        }

        [TestMethod]
        public async Task DeletePatientAsync_HasConfirmedAppointments_ThrowsPatientDeletionException()
        {
            // Arrange
            int patientId = 1;
            var patient = new Patient { PatientId = patientId };
            var appointments = new List<Appointment>
            {
                new Appointment { Status = AppointmentStatus.Confirmed }
            };

            _mockPatientRepo.Setup(repo => repo.GetByIdAsync(patientId)).ReturnsAsync(patient);
            _mockAppointmentRepo.Setup(repo => repo.GetAppointmentsByPatientAsync(patientId)).ReturnsAsync(appointments);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<PatientDeletionException>(() => _sut.DeletePatientAsync(patientId));
            _mockPatientRepo.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        #endregion
    }
}