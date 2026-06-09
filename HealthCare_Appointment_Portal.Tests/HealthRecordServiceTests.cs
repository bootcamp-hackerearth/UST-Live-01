using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
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
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Tests
{
    [TestClass]
    public class HealthRecordServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;

        private Mock<IHealthRecordRepository> _healthRecordRepositoryMock;
        private Mock<IAppointmentRepository> _appointmentRepositoryMock;

        private HealthRecordService _service;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock =
                new Mock<IUnitOfWork>();

            _mapperMock =
                new Mock<IMapper>();

            _healthRecordRepositoryMock =
                new Mock<IHealthRecordRepository>();

            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _unitOfWorkMock
                .Setup(x => x.HealthRecords)
                .Returns(_healthRecordRepositoryMock.Object);

            _unitOfWorkMock
                .Setup(x => x.Appointments)
                .Returns(_appointmentRepositoryMock.Object);

            _service =
                new HealthRecordService(
                    _unitOfWorkMock.Object,
                    _mapperMock.Object);
        }

        [TestMethod]
        public async Task AddHealthRecordAsync_ValidRecord_ShouldReturnRecordId()
        {
            // Arrange

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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.RecordExistsAsync(1))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(x => x.Map<HealthRecord>(dto))
                .Returns(record);

            // Act

            int result =
                await _service.AddHealthRecordAsync(dto);

            // Assert

            Assert.AreEqual(10, result);

            _healthRecordRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<HealthRecord>()),
                Times.Once);
        }

        [TestMethod]
        public async Task AddHealthRecordAsync_InvalidAppointment_ShouldThrowAppointmentNotFoundException()
        {
            // Arrange

            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Appointment)null);

            // Act + Assert

            await Assert.ThrowsExceptionAsync<AppointmentNotFoundException>(
                () => _service.AddHealthRecordAsync(dto));
        }
        [TestMethod]
        public async Task AddHealthRecordAsync_NotCompletedAppointment_ShouldThrowInvalidAppointmentStatusException()
        {
            // Arrange

            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act + Assert

            await Assert.ThrowsExceptionAsync<InvalidAppointmentStatusException>(
                () => _service.AddHealthRecordAsync(dto));
        }
        [TestMethod]
        public async Task AddHealthRecordAsync_DuplicateRecord_ShouldThrowDuplicateHealthRecordException()
        {
            // Arrange

            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Completed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.RecordExistsAsync(1))
                .ReturnsAsync(true);

            // Act + Assert

            await Assert.ThrowsExceptionAsync<DuplicateHealthRecordException>(
                () => _service.AddHealthRecordAsync(dto));
        }

        [TestMethod]
        public async Task GetHealthRecordByIdAsync_InvalidId_ShouldThrowHealthRecordNotFoundException()
        {
            // Arrange

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((HealthRecord)null);

            // Act + Assert

            await Assert.ThrowsExceptionAsync<HealthRecordNotFoundException>(
                () => _service.GetHealthRecordByIdAsync(1));
        }
    }
}
