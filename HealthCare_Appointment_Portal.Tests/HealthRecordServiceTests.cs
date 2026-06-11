using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
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
    public class HealthRecordServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IHealthRecordRepository> _mockHealthRecordRepo;
        private Mock<IAppointmentRepository> _mockAppointmentRepo;
        private Mock<IMapper> _mockMapper;
        private HealthRecordService _sut; // System Under Test

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockHealthRecordRepo = new Mock<IHealthRecordRepository>();
            _mockAppointmentRepo = new Mock<IAppointmentRepository>();
            _mockMapper = new Mock<IMapper>();

            // Wire up the Unit Of Work to return our mocked repositories
            _mockUnitOfWork.Setup(u => u.HealthRecords).Returns(_mockHealthRecordRepo.Object);
            _mockUnitOfWork.Setup(u => u.Appointments).Returns(_mockAppointmentRepo.Object);

            _sut = new HealthRecordService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        #region Retrieval Tests

        [TestMethod]
        public async Task GetAllHealthRecordsAsync_ReturnsMappedRecords()
        {
            // Arrange
            var records = new List<HealthRecord> { new HealthRecord(), new HealthRecord() };
            var dtos = new List<HealthRecordDto> { new HealthRecordDto(), new HealthRecordDto() };

            _mockHealthRecordRepo.Setup(r => r.GetAllRecordsWithDetailsAsync()).ReturnsAsync(records);
            _mockMapper.Setup(m => m.Map<IEnumerable<HealthRecordDto>>(records)).Returns(dtos);

            // Act
            var result = await _sut.GetAllHealthRecordsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            _mockHealthRecordRepo.Verify(r => r.GetAllRecordsWithDetailsAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetHealthRecordByIdAsync_RecordExists_ReturnsDto()
        {
            // Arrange
            int recordId = 1;
            var record = new HealthRecord { RecordId = recordId };
            var dto = new HealthRecordDto { RecordId = recordId };

            _mockHealthRecordRepo.Setup(r => r.GetByIdAsync(recordId)).ReturnsAsync(record);
            _mockMapper.Setup(m => m.Map<HealthRecordDto>(record)).Returns(dto);

            // Act
            var result = await _sut.GetHealthRecordByIdAsync(recordId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(recordId, result.RecordId);
        }

        [TestMethod]
        public async Task GetHealthRecordByIdAsync_RecordDoesNotExist_ThrowsException()
        {
            // Arrange
            int recordId = 1;
            _mockHealthRecordRepo.Setup(r => r.GetByIdAsync(recordId)).ReturnsAsync((HealthRecord)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<HealthRecordNotFoundException>(() => _sut.GetHealthRecordByIdAsync(recordId));
        }

        [TestMethod]
        public async Task GetRecordsByPatientAsync_ReturnsMappedRecords()
        {
            // Arrange
            int patientId = 1;
            var records = new List<HealthRecord> { new HealthRecord() };
            var dtos = new List<HealthRecordDto> { new HealthRecordDto() };

            _mockHealthRecordRepo.Setup(r => r.GetRecordsByPatientAsync(patientId)).ReturnsAsync(records);
            _mockMapper.Setup(m => m.Map<IEnumerable<HealthRecordDto>>(records)).Returns(dtos);

            // Act
            var result = await _sut.GetRecordsByPatientAsync(patientId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            _mockHealthRecordRepo.Verify(r => r.GetRecordsByPatientAsync(patientId), Times.Once);
        }

        [TestMethod]
        public async Task GetRecordsByDoctorAsync_ReturnsMappedRecords()
        {
            // Arrange
            int doctorId = 1;
            var records = new List<HealthRecord> { new HealthRecord() };
            var dtos = new List<HealthRecordDto> { new HealthRecordDto() };

            _mockHealthRecordRepo.Setup(r => r.GetRecordsByDoctorAsync(doctorId)).ReturnsAsync(records);
            _mockMapper.Setup(m => m.Map<IEnumerable<HealthRecordDto>>(records)).Returns(dtos);

            // Act
            var result = await _sut.GetRecordsByDoctorAsync(doctorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            _mockHealthRecordRepo.Verify(r => r.GetRecordsByDoctorAsync(doctorId), Times.Once);
        }

        #endregion

        #region AddHealthRecordAsync Tests

        [TestMethod]
        public async Task AddHealthRecordAsync_AppointmentNotFound_ThrowsException()
        {
            // Arrange
            var dto = new CreateHealthRecordDto { AppointmentId = 1 };
            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(dto.AppointmentId)).ReturnsAsync((Appointment)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<AppointmentNotFoundException>(() => _sut.AddHealthRecordAsync(dto));
        }

        [TestMethod]
        public async Task AddHealthRecordAsync_AppointmentNotCompleted_ThrowsException()
        {
            // Arrange
            var dto = new CreateHealthRecordDto { AppointmentId = 1 };
            var appointment = new Appointment { AppointmentId = 1, Status = AppointmentStatus.Pending }; // Not Completed

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(dto.AppointmentId)).ReturnsAsync(appointment);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidAppointmentStatusException>(() => _sut.AddHealthRecordAsync(dto));
        }

        [TestMethod]
        public async Task AddHealthRecordAsync_RecordAlreadyExists_ThrowsException()
        {
            // Arrange
            var dto = new CreateHealthRecordDto { AppointmentId = 1 };
            var appointment = new Appointment { AppointmentId = 1, Status = AppointmentStatus.Completed };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(dto.AppointmentId)).ReturnsAsync(appointment);
            _mockHealthRecordRepo.Setup(r => r.RecordExistsAsync(dto.AppointmentId)).ReturnsAsync(true); // Exists

            // Act & Assert
            await Assert.ThrowsExceptionAsync<DuplicateHealthRecordException>(() => _sut.AddHealthRecordAsync(dto));
        }

        [TestMethod]
        public async Task AddHealthRecordAsync_ValidData_AddsAndCommits()
        {
            // Arrange
            var dto = new CreateHealthRecordDto { AppointmentId = 1 };
            var appointment = new Appointment { AppointmentId = 1, Status = AppointmentStatus.Completed };
            var mappedRecord = new HealthRecord { RecordId = 5 };

            _mockAppointmentRepo.Setup(r => r.GetByIdAsync(dto.AppointmentId)).ReturnsAsync(appointment);
            _mockHealthRecordRepo.Setup(r => r.RecordExistsAsync(dto.AppointmentId)).ReturnsAsync(false);
            _mockMapper.Setup(m => m.Map<HealthRecord>(dto)).Returns(mappedRecord);

            // Act
            var result = await _sut.AddHealthRecordAsync(dto);

            // Assert
            Assert.AreEqual(5, result);
            _mockHealthRecordRepo.Verify(r => r.AddAsync(mappedRecord), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        #endregion

        #region UpdateHealthRecordAsync Tests

        [TestMethod]
        public async Task UpdateHealthRecordAsync_RecordNotFound_ThrowsException()
        {
            // Arrange
            int recordId = 1;
            var dto = new UpdateHealthRecordDto();
            _mockHealthRecordRepo.Setup(r => r.GetByIdAsync(recordId)).ReturnsAsync((HealthRecord)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<HealthRecordNotFoundException>(() => _sut.UpdateHealthRecordAsync(recordId, dto));
        }

        [TestMethod]
        public async Task UpdateHealthRecordAsync_ValidData_UpdatesAndCommits()
        {
            // Arrange
            int recordId = 1;
            var dto = new UpdateHealthRecordDto();
            var existingRecord = new HealthRecord { RecordId = recordId };

            _mockHealthRecordRepo.Setup(r => r.GetByIdAsync(recordId)).ReturnsAsync(existingRecord);

            // Act
            await _sut.UpdateHealthRecordAsync(recordId, dto);

            // Assert
            _mockMapper.Verify(m => m.Map(dto, existingRecord), Times.Once);
            _mockHealthRecordRepo.Verify(r => r.UpdateAsync(existingRecord), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        #endregion

        #region DeleteHealthRecordAsync Tests

        [TestMethod]
        public async Task DeleteHealthRecordAsync_RecordNotFound_ThrowsException()
        {
            // Arrange
            int recordId = 1;
            _mockHealthRecordRepo.Setup(r => r.GetByIdAsync(recordId)).ReturnsAsync((HealthRecord)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<HealthRecordNotFoundException>(() => _sut.DeleteHealthRecordAsync(recordId));
        }

        [TestMethod]
        public async Task DeleteHealthRecordAsync_RecordExists_DeletesAndCommits()
        {
            // Arrange
            int recordId = 1;
            var existingRecord = new HealthRecord { RecordId = recordId };

            _mockHealthRecordRepo.Setup(r => r.GetByIdAsync(recordId)).ReturnsAsync(existingRecord);

            // Act
            await _sut.DeleteHealthRecordAsync(recordId);

            // Assert
            _mockHealthRecordRepo.Verify(r => r.DeleteAsync(recordId), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        #endregion
    }
}