using HealthCare.Shared;
using HealthCareApi;
using HealthCareApi.Repositories.Interfaces;
using HealthCareApi.Services.Implementations;
using HealthCareApi.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Tests
{
    [TestClass]
    public class HealthRecordServiceTests
    {
        private HealthRecordService _service;

        private Mock<IHealthRecordRepository> _healthRepoMock;
        private Mock<IAppointmentRepository> _apptRepoMock;
        private Mock<HealthAppDbContext> _contextMock;

        [TestInitialize]
        public void Setup()
        {
            _healthRepoMock = new Mock<IHealthRecordRepository>();
            _apptRepoMock = new Mock<IAppointmentRepository>();
            

            _service = new HealthRecordService(
                _healthRepoMock.Object,
                _apptRepoMock.Object
                
            );
        }

        //  ADD SUCCESS
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
                .Returns(Task.FromResult(appointment));

            _healthRepoMock.Setup(r => r.HealthRecordExistsAsync(1))
                .Returns(Task.FromResult(false));

            _healthRepoMock.Setup(r => r.AddAsync(It.IsAny<HealthRecord>()))
                .Returns(Task.CompletedTask);

            var result = await _service.AddHealthRecordAsync(record);

            Assert.IsNotNull(result);
            Assert.AreEqual(DateTime.Today, result.VisitDate);
        }

        //  INVALID APPOINTMENT
        [TestMethod]
        public async Task AddHealthRecord_ShouldThrow_WhenAppointmentInvalid()
        {
            var record = new HealthRecord
            {
                AppointmentId = 1
            };

            _apptRepoMock.Setup(r => r.GetByIdAsync(1))
                .Returns(Task.FromResult<Appointment>(null));

            await Assert.ThrowsExceptionAsync<Exception>(() =>
                _service.AddHealthRecordAsync(record));
        }

        //  ALREADY EXISTS
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
                .Returns(Task.FromResult(appointment));

            _healthRepoMock.Setup(r => r.HealthRecordExistsAsync(1))
                .Returns(Task.FromResult(true));

            await Assert.ThrowsExceptionAsync<Exception>(() =>
                _service.AddHealthRecordAsync(record));
        }

        //  GET BY APPOINTMENT ID
        [TestMethod]
        public async Task GetByAppointmentId_ShouldReturnRecord()
        {
            var record = new HealthRecord
            {
                AppointmentId = 1
            };

            _healthRepoMock.Setup(r => r.GetByAppointmentIdAsync(1))
                .Returns(Task.FromResult(record)); 

            var result = await _service.GetByAppointmentIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.AppointmentId);
        }

        //  GET BY ID
        [TestMethod]
        public async Task GetById_ShouldReturnRecord()
        {
            var record = new HealthRecord
            {
                RecordId = 5
            };

            _healthRepoMock.Setup(r => r.GetByIdAsync(5))
                .Returns(Task.FromResult(record)); 

            var result = await _service.GetByIdAsync(5);

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.RecordId);
        }

        //  GET PATIENT HISTORY
        [TestMethod]
        public async Task GetPatientHistory_ShouldReturnPagedResult()
        {
            var data = new PagedResult<vw_PatientHealthHistory>
            {
                Items = new List<vw_PatientHealthHistory>(),
                TotalCount = 0
            };

            _healthRepoMock.Setup(r => r.GetPatientHealthHistoryAsync(1, 1, 10))
                .Returns(Task.FromResult(data));

            var result = await _service.GetPatientHealthHistoryAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalCount);
        }
    }
}