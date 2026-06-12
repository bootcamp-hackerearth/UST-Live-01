using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
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
    public class HealthRecordServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IHealthRecordRepository> _mockHealthRecordRepo;
        private Mock<IAppointmentRepository> _mockAppointmentRepo;
        private Mock<IMapper> _mockMapper;
        private HealthRecordService _sut;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockHealthRecordRepo = new Mock<IHealthRecordRepository>();
            _mockAppointmentRepo = new Mock<IAppointmentRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(u => u.HealthRecords).Returns(_mockHealthRecordRepo.Object);
            _mockUnitOfWork.Setup(u => u.Appointments).Returns(_mockAppointmentRepo.Object);

            _sut = new HealthRecordService(
                _mockUnitOfWork.Object,
                _mockMapper.Object);
        }

        [TestMethod]
        public async Task GetAllHealthRecordsAsync_ReturnsMappedRecords()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord(),
                new HealthRecord()
            };

            var dtos = new List<HealthRecordDto>
            {
                new HealthRecordDto(),
                new HealthRecordDto()
            };

            _mockHealthRecordRepo
                .Setup(r => r.GetAllRecordsWithDetailsAsync())
                .ReturnsAsync(records);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(dtos);

            var result = await _sut.GetAllHealthRecordsAsync();

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetHealthRecordByIdAsync_ValidId_ReturnsRecord()
        {
            int recordId = 1;

            var record = new HealthRecord
            {
                RecordId = recordId
            };

            var dto = new HealthRecordDto
            {
                RecordId = recordId
            };

            _mockHealthRecordRepo
                .Setup(r => r.GetByIdAsync(recordId))
                .ReturnsAsync(record);

            _mockMapper
                .Setup(m => m.Map<HealthRecordDto>(record))
                .Returns(dto);

            var result = await _sut.GetHealthRecordByIdAsync(recordId);

            Assert.IsNotNull(result);
            Assert.AreEqual(recordId, result.RecordId);
        }

        [TestMethod]
        public async Task GetHealthRecordByIdAsync_InvalidId_ThrowsHealthRecordNotFoundException()
        {
            _mockHealthRecordRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((HealthRecord)null);

            await Assert.ThrowsExceptionAsync<HealthRecordNotFoundException>(
                () => _sut.GetHealthRecordByIdAsync(99));
        }

        [TestMethod]
        public async Task GetRecordsByPatientAsync_ReturnsMappedRecords()
        {
            int patientId = 1;

            var records = new List<HealthRecord>
            {
                new HealthRecord()
            };

            var dtos = new List<HealthRecordDto>
            {
                new HealthRecordDto()
            };

            _mockHealthRecordRepo
                .Setup(r => r.GetRecordsByPatientAsync(patientId))
                .ReturnsAsync(records);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(dtos);

            var result = await _sut.GetRecordsByPatientAsync(patientId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetRecordsByDoctorAsync_ReturnsMappedRecords()
        {
            int doctorId = 1;

            var records = new List<HealthRecord>
            {
                new HealthRecord()
            };

            var dtos = new List<HealthRecordDto>
            {
                new HealthRecordDto()
            };

            _mockHealthRecordRepo
                .Setup(r => r.GetRecordsByDoctorAsync(doctorId))
                .ReturnsAsync(records);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(dtos);

            var result = await _sut.GetRecordsByDoctorAsync(doctorId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task AddHealthRecordAsync_AppointmentNotFound_ThrowsAppointmentNotFoundException()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            _mockAppointmentRepo
                .Setup(r => r.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsExceptionAsync<AppointmentNotFoundException>(
                () => _sut.AddHealthRecordAsync(dto));
        }

        [TestMethod]
        public async Task AddHealthRecordAsync_AppointmentNotCompleted_ThrowsInvalidAppointmentStatusException()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Pending
            };

            _mockAppointmentRepo
                .Setup(r => r.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            await Assert.ThrowsExceptionAsync<InvalidAppointmentStatusException>(
                () => _sut.AddHealthRecordAsync(dto));
        }

        [TestMethod]
        public async Task AddHealthRecordAsync_RecordAlreadyExists_ThrowsDuplicateHealthRecordException()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Completed
            };

            _mockAppointmentRepo
                .Setup(r => r.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _mockHealthRecordRepo
                .Setup(r => r.RecordExistsAsync(dto.AppointmentId))
                .ReturnsAsync(true);

            await Assert.ThrowsExceptionAsync<DuplicateHealthRecordException>(
                () => _sut.AddHealthRecordAsync(dto));
        }

        [TestMethod]
        public async Task AddHealthRecordAsync_ValidRecord_AddsAndCommits()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Completed
            };

            var record = new HealthRecord
            {
                RecordId = 10,
                AppointmentId = 1
            };

            _mockAppointmentRepo
                .Setup(r => r.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _mockHealthRecordRepo
                .Setup(r => r.RecordExistsAsync(dto.AppointmentId))
                .ReturnsAsync(false);

            _mockMapper
                .Setup(m => m.Map<HealthRecord>(dto))
                .Returns(record);

            var result = await _sut.AddHealthRecordAsync(dto);

            Assert.AreEqual(10, result);

            _mockHealthRecordRepo.Verify(
                r => r.AddAsync(record),
                Times.Once);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(),
                Times.Once);
        }

        [TestMethod]
        public async Task UpdateHealthRecordAsync_InvalidId_ThrowsHealthRecordNotFoundException()
        {
            _mockHealthRecordRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((HealthRecord)null);

            await Assert.ThrowsExceptionAsync<HealthRecordNotFoundException>(
                () => _sut.UpdateHealthRecordAsync(1, new UpdateHealthRecordDto()));
        }

        [TestMethod]
        public async Task UpdateHealthRecordAsync_ValidRecord_UpdatesAndCommits()
        {
            int recordId = 1;

            var dto = new UpdateHealthRecordDto();

            var record = new HealthRecord
            {
                RecordId = recordId
            };

            _mockHealthRecordRepo
                .Setup(r => r.GetByIdAsync(recordId))
                .ReturnsAsync(record);

            await _sut.UpdateHealthRecordAsync(recordId, dto);

            _mockMapper.Verify(
                m => m.Map(dto, record),
                Times.Once);

            _mockHealthRecordRepo.Verify(
                r => r.UpdateAsync(record),
                Times.Once);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(),
                Times.Once);
        }

        [TestMethod]
        public async Task DeleteHealthRecordAsync_InvalidId_ThrowsHealthRecordNotFoundException()
        {
            _mockHealthRecordRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((HealthRecord)null);

            await Assert.ThrowsExceptionAsync<HealthRecordNotFoundException>(
                () => _sut.DeleteHealthRecordAsync(1));
        }

        [TestMethod]
        public async Task DeleteHealthRecordAsync_ValidId_DeletesAndCommits()
        {
            int recordId = 1;

            var record = new HealthRecord
            {
                RecordId = recordId
            };

            _mockHealthRecordRepo
                .Setup(r => r.GetByIdAsync(recordId))
                .ReturnsAsync(record);

            await _sut.DeleteHealthRecordAsync(recordId);

            _mockHealthRecordRepo.Verify(
                r => r.DeleteAsync(recordId),
                Times.Once);

            _mockUnitOfWork.Verify(
                u => u.CommitAsync(),
                Times.Once);
        }
    }
}