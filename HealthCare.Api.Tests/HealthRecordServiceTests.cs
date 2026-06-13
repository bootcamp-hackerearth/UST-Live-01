using HealthCare.Shared;
using HealthCareApi.Repositories.Interfaces;
using HealthCareApi.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthCareWebApi;

namespace HealthCare.Tests
{
    [TestClass]
    public class HealthRecordServiceTests
    {
        private Mock<IHealthRecordRepository> _healthRepoMock;
        private Mock<IAppointmentRepository> _apptRepoMock;
        private Mock<HealthAppDbContext> _contextMock;

        private HealthRecordService _service;

        [TestInitialize]
        public void Setup()
        {
            _healthRepoMock = new Mock<IHealthRecordRepository>();
            _apptRepoMock = new Mock<IAppointmentRepository>();
            _contextMock = new Mock<HealthAppDbContext>();

            _service = new HealthRecordService(
                _healthRepoMock.Object,
                _apptRepoMock.Object,
                _contextMock.Object
            );
        }

        // ✅ ADD SUCCESS
        [TestMethod]
        public async Task AddHealthRecord_ShouldSucceed_WhenValid()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                ScheduledDate = DateTime.Today
            };

            var record = new HealthRecord
            {
                AppointmentId = 1
            };

            _apptRepoMock.Setup(r => r.GetByIdAsync(1))
                         .ReturnsAsync(appointment);

            _healthRepoMock.Setup(r => r.HealthRecordExistsAsync(1))
                           .ReturnsAsync(false);

            _healthRepoMock.Setup(r => r.AddAsync(It.IsAny<HealthRecord>()))
                           .Returns(Task.CompletedTask);

            var result = await _service.AddHealthRecordAsync(record);

            Assert.IsNotNull(result, "Health record should be created");
            Assert.AreEqual(DateTime.Today, result.VisitDate, "VisitDate should match appointment date");
        }

        // ✅ INVALID APPOINTMENT
        [TestMethod]
        public async Task AddHealthRecord_ShouldThrow_WhenAppointmentInvalid()
        {
            var record = new HealthRecord
            {
                AppointmentId = 1
            };

            _apptRepoMock.Setup(r => r.GetByIdAsync(1))
                         .ReturnsAsync((Appointment)null);

            await Assert.ThrowsExceptionAsync<Exception>(() =>
                _service.AddHealthRecordAsync(record));
        }

        // ✅ ALREADY EXISTS
        [TestMethod]
        public async Task AddHealthRecord_ShouldThrow_WhenRecordAlreadyExists()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                ScheduledDate = DateTime.Today
            };

            var record = new HealthRecord
            {
                AppointmentId = 1
            };

            _apptRepoMock.Setup(r => r.GetByIdAsync(1))
                         .ReturnsAsync(appointment);

            _healthRepoMock.Setup(r => r.HealthRecordExistsAsync(1))
                           .ReturnsAsync(true);

            await Assert.ThrowsExceptionAsync<Exception>(() =>
                _service.AddHealthRecordAsync(record));
        }

        // ✅ GET BY APPOINTMENT ID
        [TestMethod]
        public async Task GetByAppointmentId_ShouldReturnRecord()
        {
            var record = new HealthRecord
            {
                AppointmentId = 1
            };

            _healthRepoMock.Setup(r => r.GetByAppointmentIdAsync(1))
                           .ReturnsAsync(record);

            var result = await _service.GetByAppointmentIdAsync(1);

            Assert.IsNotNull(result, "Record should not be null");
            Assert.AreEqual(1, result.AppointmentId, "AppointmentId should match");
        }

        // ✅ GET BY ID
        [TestMethod]
        public async Task GetById_ShouldReturnRecord()
        {
            var record = new HealthRecord
            {
                RecordId = 5
            };

            _healthRepoMock.Setup(r => r.GetByIdAsync(5))
                           .ReturnsAsync(record);

            var result = await _service.GetByIdAsync(5);

            Assert.IsNotNull(result, "Record should not be null");
            Assert.AreEqual(5, result.RecordId, "RecordId should match");
        }

        // ✅ GET PATIENT HISTORY
        [TestMethod]
        public async Task GetPatientHistory_ShouldReturnPagedResult()
        {
            var data = new PagedResult<vw_PatientHealthHistory>
            {
                Items = new List<vw_PatientHealthHistory>(),
                TotalCount = 0
            };

            _healthRepoMock.Setup(r => r.GetPatientHealthHistoryAsync(1, 1, 10))
                           .ReturnsAsync(data);

            var result = await _service.GetPatientHealthHistoryAsync(1);

            Assert.IsNotNull(result, "Paged result should not be null");
            Assert.AreEqual(0, result.TotalCount, "TotalCount should be zero");
        }
    }
}
