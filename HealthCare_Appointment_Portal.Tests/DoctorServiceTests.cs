using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using HealthCare_Appointment_Portal.Utilities;
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
        private Mock<IDoctorRepository> _mockDoctorRepo;
        private Mock<IAppointmentRepository> _mockAppointmentRepo;
        private Mock<IUserRepository> _mockUserRepo;
        private Mock<IMapper> _mockMapper;
        private DoctorService _sut; // System Under Test

        [TestInitialize]
        public void Setup()
        {
            // Arrange - Common setup runs before EVERY test
            _mockDoctorRepo = new Mock<IDoctorRepository>();
            _mockAppointmentRepo = new Mock<IAppointmentRepository>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            _sut = new DoctorService(
                _mockDoctorRepo.Object,
                _mockAppointmentRepo.Object,
                _mockUserRepo.Object,
                _mockMapper.Object);
        }

        #region GetAllDoctorsAsync Tests

        [TestMethod]
        public async Task GetAllDoctorsAsync_ReturnsAllDoctors()
        {
            // Arrange
            var doctors = new List<Doctor> { new Doctor(), new Doctor() };
            var doctorDtos = new List<DoctorDto> { new DoctorDto(), new DoctorDto() };

            _mockDoctorRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(doctors);
            _mockMapper.Setup(m => m.Map<IEnumerable<DoctorDto>>(doctors)).Returns(doctorDtos);

            // Act
            var result = await _sut.GetAllDoctorsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            _mockDoctorRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        #endregion

        #region GetDoctorByIdAsync Tests

        [TestMethod]
        public async Task GetDoctorByIdAsync_DoctorExists_ReturnsDoctorDto()
        {
            // Arrange
            int doctorId = 1;
            var doctor = new Doctor { DoctorId = doctorId };
            var doctorDto = new DoctorDto { DoctorId = doctorId };

            _mockDoctorRepo.Setup(repo => repo.GetByIdAsync(doctorId)).ReturnsAsync(doctor);
            _mockMapper.Setup(m => m.Map<DoctorDto>(doctor)).Returns(doctorDto);

            // Act
            var result = await _sut.GetDoctorByIdAsync(doctorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(doctorId, result.DoctorId);
        }

        [TestMethod]
        public async Task GetDoctorByIdAsync_DoctorDoesNotExist_ThrowsDoctorNotFoundException()
        {
            // Arrange
            int doctorId = 1;
            _mockDoctorRepo.Setup(repo => repo.GetByIdAsync(doctorId)).ReturnsAsync((Doctor)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<DoctorNotFoundException>(() => _sut.GetDoctorByIdAsync(doctorId));
        }

        #endregion

        #region AddDoctorAsync Tests

        [TestMethod]
        public async Task AddDoctorAsync_ValidDoctor_AddsDoctorAndCreatesUser()
        {
            // Arrange
            // FIX: Using FullName as defined in your CreateDoctorDto
            var createDto = new CreateDoctorDto { FullName = "Dr. John Doe" };
            int expectedDoctorId = 5;

            var mappedDoctor = new Doctor { DoctorId = expectedDoctorId };

            _mockMapper.Setup(m => m.Map<Doctor>(createDto)).Returns(mappedDoctor);
            _mockDoctorRepo.Setup(repo => repo.AddAsync(mappedDoctor)).Returns(Task.CompletedTask);
            _mockUserRepo.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _sut.AddDoctorAsync(createDto);

            // Assert
            Assert.AreEqual(expectedDoctorId, result);
            _mockDoctorRepo.Verify(repo => repo.AddAsync(mappedDoctor), Times.Once);

            // Verify that the User was created with the exact string formatting expected
            _mockUserRepo.Verify(repo => repo.AddAsync(It.Is<User>(u =>
                u.UserCode == "D005" &&
                u.Email == "doctor5@hospital.com" &&
                u.Role == Role.Doctor &&
                u.ReferenceId == expectedDoctorId &&
                u.PasswordHash == string.Empty
            )), Times.Once);
        }

        #endregion

        #region UpdateDoctorAsync Tests

        [TestMethod]
        public async Task UpdateDoctorAsync_ValidData_UpdatesSuccessfully()
        {
            // Arrange
            int doctorId = 1;
            // Assuming UpdateDoctorDto might also be empty or use similar properties, 
            // an empty initialization is safest here for mapping verification.
            var updateDto = new UpdateDoctorDto();
            var existingDoctor = new Doctor { DoctorId = doctorId };

            _mockDoctorRepo.Setup(repo => repo.GetByIdAsync(doctorId)).ReturnsAsync(existingDoctor);
            _mockDoctorRepo.Setup(repo => repo.UpdateAsync(existingDoctor)).Returns(Task.CompletedTask);

            // Act
            await _sut.UpdateDoctorAsync(doctorId, updateDto);

            // Assert
            _mockMapper.Verify(m => m.Map(updateDto, existingDoctor), Times.Once);
            _mockDoctorRepo.Verify(repo => repo.UpdateAsync(existingDoctor), Times.Once);
        }

        [TestMethod]
        public async Task UpdateDoctorAsync_DoctorNotFound_ThrowsDoctorNotFoundException()
        {
            // Arrange
            int doctorId = 1;
            var updateDto = new UpdateDoctorDto();

            _mockDoctorRepo.Setup(repo => repo.GetByIdAsync(doctorId)).ReturnsAsync((Doctor)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<DoctorNotFoundException>(() => _sut.UpdateDoctorAsync(doctorId, updateDto));
        }

        #endregion

        #region DeleteDoctorAsync Tests

        [TestMethod]
        public async Task DeleteDoctorAsync_DoctorNotFound_ThrowsDoctorNotFoundException()
        {
            // Arrange
            int doctorId = 1;
            _mockDoctorRepo.Setup(repo => repo.GetByIdAsync(doctorId)).ReturnsAsync((Doctor)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<DoctorNotFoundException>(() => _sut.DeleteDoctorAsync(doctorId));
        }

        [TestMethod]
        public async Task DeleteDoctorAsync_HasConfirmedAppointments_ThrowsDoctorDeletionException()
        {
            // Arrange
            int doctorId = 1;
            var doctor = new Doctor { DoctorId = doctorId };
            var appointments = new List<Appointment>
            {
                new Appointment { Status = AppointmentStatus.Confirmed } // Triggers the exception
            };

            _mockDoctorRepo.Setup(repo => repo.GetByIdAsync(doctorId)).ReturnsAsync(doctor);
            _mockAppointmentRepo.Setup(repo => repo.GetAppointmentsByDoctorAsync(doctorId)).ReturnsAsync(appointments);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<DoctorDeletionException>(() => _sut.DeleteDoctorAsync(doctorId));

            // Ensure no pending appointments were updated and doctor wasn't deleted
            _mockAppointmentRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Appointment>()), Times.Never);
            _mockDoctorRepo.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task DeleteDoctorAsync_NoConfirmedAppointments_CancelsPendingAndDeletesDoctor()
        {
            // Arrange
            int doctorId = 1;
            var doctor = new Doctor { DoctorId = doctorId };

            var pendingAppointment = new Appointment { Status = AppointmentStatus.Pending };
            var completedAppointment = new Appointment { Status = AppointmentStatus.Completed };

            var appointments = new List<Appointment> { pendingAppointment, completedAppointment };

            _mockDoctorRepo.Setup(repo => repo.GetByIdAsync(doctorId)).ReturnsAsync(doctor);
            _mockAppointmentRepo.Setup(repo => repo.GetAppointmentsByDoctorAsync(doctorId)).ReturnsAsync(appointments);
            _mockAppointmentRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Appointment>())).Returns(Task.CompletedTask);

            // Act
            await _sut.DeleteDoctorAsync(doctorId);

            // Assert
            // Ensure UpdateAsync was only called for the 1 Pending appointment, not the Completed one
            _mockAppointmentRepo.Verify(repo => repo.UpdateAsync(pendingAppointment), Times.Once);
            _mockAppointmentRepo.Verify(repo => repo.UpdateAsync(completedAppointment), Times.Never);

            // Ensure the doctor was deleted
            _mockDoctorRepo.Verify(repo => repo.DeleteAsync(doctorId), Times.Once);
        }

        #endregion

        #region GetDoctorsBySpecialisationAsync Tests

        [TestMethod]
        public async Task GetDoctorsBySpecialisationAsync_ValidSpecialisation_ReturnsMappedDoctors()
        {
            // Arrange
            var spec = Specialisation.Cardiology;
            var doctors = new List<Doctor> { new Doctor() };
            var doctorDtos = new List<DoctorDto> { new DoctorDto() };

            _mockDoctorRepo.Setup(repo => repo.GetDoctorsBySpecialisationAsync(spec)).ReturnsAsync(doctors);
            _mockMapper.Setup(m => m.Map<IEnumerable<DoctorDto>>(doctors)).Returns(doctorDtos);

            // Act
            var result = await _sut.GetDoctorsBySpecialisationAsync(spec);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            _mockDoctorRepo.Verify(repo => repo.GetDoctorsBySpecialisationAsync(spec), Times.Once);
        }

        #endregion

        #region GetDoctorsByNameAsync Tests

        [TestMethod]
        public async Task GetDoctorsByNameAsync_ValidSearchTerm_ReturnsMappedDoctors()
        {
            // Arrange
            string searchTerm = "Smith";
            var doctors = new List<Doctor> { new Doctor() };
            var doctorDtos = new List<DoctorDto> { new DoctorDto() };

            _mockDoctorRepo.Setup(repo => repo.GetDoctorsByNameAsync(searchTerm)).ReturnsAsync(doctors);
            _mockMapper.Setup(m => m.Map<IEnumerable<DoctorDto>>(doctors)).Returns(doctorDtos);

            // Act
            var result = await _sut.GetDoctorsByNameAsync(searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            _mockDoctorRepo.Verify(repo => repo.GetDoctorsByNameAsync(searchTerm), Times.Once);
        }

        #endregion
    }
}