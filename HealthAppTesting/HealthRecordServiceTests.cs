using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HealthAppWebAPI;
using HealthAppWebAPI.Enums;
using HealthAppWebAPI.Models.Dtos;
using HealthAppWebAPI.Repositories.Interfaces;
using HealthAppWebAPI.Services.Impl;
using Moq;
using Xunit;

namespace HealthAppWebAPI.Tests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _recordRepoMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _recordRepoMock = new Mock<IHealthRecordRepository>();
            _appointmentRepoMock = new Mock<IAppointmentRepository>();

            var mapperMock = new Mock<IMapper>();

            _service = new HealthRecordService(
                _recordRepoMock.Object,
                _appointmentRepoMock.Object,
                mapperMock.Object);
        }

        private static CreateHealthRecordDto GetValidCreateHealthRecordDto()
        {
            return new CreateHealthRecordDto
            {
                AppointmentId = 1,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest"
            };
        }

        private static HealthRecord GetSampleHealthRecord()
        {
            return new HealthRecord
            {
                HealthRecordId = 1,
                AppointmentId = 10,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest",
                Appointment = new Appointment
                {
                    AppointmentId = 10,
                    PatientId = 5,
                    DoctorId = 2,
                    Patient = new Patient
                    {
                        PatientId = 5,
                        FullName = "John Patient"
                    },
                    Doctor = new Doctor
                    {
                        DoctorId = 2,
                        FullName = "Dr Smith"
                    }
                }
            };
        }

        [Fact]
        public async Task GetAllAsync_WhenRecordsExist_ReturnsHealthRecordDtos()
        {
            var records = new List<HealthRecord>
            {
                GetSampleHealthRecord()
            };

            _recordRepoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(records);

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].HealthRecordId);
            Assert.Equal(10, result[0].AppointmentId);
            Assert.Equal(5, result[0].PatientId);
            Assert.Equal("John Patient", result[0].PatientName);
            Assert.Equal("Dr Smith", result[0].DoctorName);
            Assert.Equal("Fever", result[0].Diagnosis);
            Assert.Equal("Paracetamol", result[0].Prescription);
            Assert.Equal("Take rest", result[0].Notes);

            _recordRepoMock.Verify(
                r => r.GetAllAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenRecordNotFound_ThrowsKeyNotFoundException()
        {
            int healthRecordId = 1;

            _recordRepoMock
                .Setup(r => r.GetByIdAsync(healthRecordId))
                .ReturnsAsync((HealthRecord)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.GetByIdAsync(healthRecordId));

            Assert.Equal("Health record not found.", exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_WhenRecordExists_ReturnsHealthRecordDto()
        {
            int healthRecordId = 1;

            var record = GetSampleHealthRecord();

            _recordRepoMock
                .Setup(r => r.GetByIdAsync(healthRecordId))
                .ReturnsAsync(record);

            var result = await _service.GetByIdAsync(healthRecordId);

            Assert.NotNull(result);
            Assert.Equal(record.HealthRecordId, result.HealthRecordId);
            Assert.Equal(record.AppointmentId, result.AppointmentId);
            Assert.Equal("John Patient", result.PatientName);
            Assert.Equal("Dr Smith", result.DoctorName);
            Assert.Equal(record.Diagnosis, result.Diagnosis);
            Assert.Equal(record.Prescription, result.Prescription);
            Assert.Equal(record.Notes, result.Notes);

            _recordRepoMock.Verify(
                r => r.GetByIdAsync(healthRecordId),
                Times.Once);
        }

        [Fact]
        public async Task AddAsync_WhenDtoIsNull_ThrowsArgumentException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.AddAsync(null));

            Assert.Equal("Health record data is required.", exception.Message);

            _recordRepoMock.Verify(
                r => r.AddAsync(It.IsAny<HealthRecord>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAsync_WhenAppointmentIdIsInvalid_ThrowsArgumentException()
        {
            var dto = GetValidCreateHealthRecordDto();
            dto.AppointmentId = 0;

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.AddAsync(dto));

            Assert.Equal("Valid appointment is required.", exception.Message);

            _recordRepoMock.Verify(
                r => r.AddAsync(It.IsAny<HealthRecord>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAsync_WhenDiagnosisIsEmpty_ThrowsArgumentException()
        {
            var dto = GetValidCreateHealthRecordDto();
            dto.Diagnosis = " ";

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.AddAsync(dto));

            Assert.Equal("Diagnosis is required.", exception.Message);

            _recordRepoMock.Verify(
                r => r.AddAsync(It.IsAny<HealthRecord>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAsync_WhenPrescriptionIsEmpty_ThrowsArgumentException()
        {
            var dto = GetValidCreateHealthRecordDto();
            dto.Prescription = " ";

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.AddAsync(dto));

            Assert.Equal("Prescription is required.", exception.Message);

            _recordRepoMock.Verify(
                r => r.AddAsync(It.IsAny<HealthRecord>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAsync_WhenAppointmentNotFound_ThrowsKeyNotFoundException()
        {
            var dto = GetValidCreateHealthRecordDto();

            _appointmentRepoMock
                .Setup(r => r.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync((Appointment)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.AddAsync(dto));

            Assert.Equal("Appointment not found.", exception.Message);

            _recordRepoMock.Verify(
                r => r.AddAsync(It.IsAny<HealthRecord>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAsync_WhenAppointmentIsNotConfirmed_ThrowsInvalidOperationException()
        {
            var dto = GetValidCreateHealthRecordDto();

            var appointment = new Appointment
            {
                AppointmentId = dto.AppointmentId,
                Status = AppointmentStatus.Pending.ToString()
            };

            _appointmentRepoMock
                .Setup(r => r.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.AddAsync(dto));

            Assert.Equal(
                "Health record can be added only for confirmed appointments.",
                exception.Message);

            _recordRepoMock.Verify(
                r => r.AddAsync(It.IsAny<HealthRecord>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAsync_WhenHealthRecordAlreadyExists_ThrowsInvalidOperationException()
        {
            var dto = GetValidCreateHealthRecordDto();

            var appointment = new Appointment
            {
                AppointmentId = dto.AppointmentId,
                Status = AppointmentStatus.Confirmed.ToString()
            };

            var existingRecord = new HealthRecord
            {
                HealthRecordId = 1,
                AppointmentId = dto.AppointmentId
            };

            _appointmentRepoMock
                .Setup(r => r.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _recordRepoMock
                .Setup(r => r.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(existingRecord);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.AddAsync(dto));

            Assert.Equal(
                "Health record already exists for this appointment.",
                exception.Message);

            _recordRepoMock.Verify(
                r => r.AddAsync(It.IsAny<HealthRecord>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAsync_WhenValid_AddsHealthRecordAndCompletesAppointment()
        {
            var dto = GetValidCreateHealthRecordDto();

            var appointment = new Appointment
            {
                AppointmentId = dto.AppointmentId,
                Status = AppointmentStatus.Confirmed.ToString()
            };

            HealthRecord addedRecord = null;
            Appointment updatedAppointment = null;

            _appointmentRepoMock
                .Setup(r => r.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _recordRepoMock
                .Setup(r => r.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord)null);

            _recordRepoMock
                .Setup(r => r.AddAsync(It.IsAny<HealthRecord>()))
                .Callback<HealthRecord>(record =>
                {
                    addedRecord = record;
                })
                .Returns(Task.CompletedTask);

            _appointmentRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
                .Callback<Appointment>(appointmentToUpdate =>
                {
                    updatedAppointment = appointmentToUpdate;
                })
                .Returns(Task.CompletedTask);

            await _service.AddAsync(dto);

            Assert.NotNull(addedRecord);
            Assert.Equal(dto.AppointmentId, addedRecord.AppointmentId);
            Assert.Equal(dto.Diagnosis, addedRecord.Diagnosis);
            Assert.Equal(dto.Prescription, addedRecord.Prescription);
            Assert.Equal(dto.Notes, addedRecord.Notes);

            Assert.NotNull(updatedAppointment);
            Assert.Equal(
                AppointmentStatus.Completed.ToString(),
                updatedAppointment.Status);

            _recordRepoMock.Verify(
                r => r.AddAsync(It.IsAny<HealthRecord>()),
                Times.Once);

            _appointmentRepoMock.Verify(
                r => r.UpdateAsync(appointment),
                Times.Once);
        }

        [Fact]
        public async Task GetPatientHistoryAsync_WhenPatientIdIsInvalid_ThrowsArgumentException()
        {
            int patientId = 0;

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.GetPatientHistoryAsync(patientId));

            Assert.Equal("Valid patient is required.", exception.Message);
        }

        [Fact]
        public async Task GetPatientHistoryAsync_WhenRecordsExist_ReturnsHealthRecordDtos()
        {
            int patientId = 5;

            var records = new List<HealthRecord>
            {
                GetSampleHealthRecord()
            };

            _recordRepoMock
                .Setup(r => r.GetByPatientIdAsync(patientId))
                .ReturnsAsync(records);

            var result = await _service.GetPatientHistoryAsync(patientId);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(patientId, result[0].PatientId);
            Assert.Equal("John Patient", result[0].PatientName);
            Assert.Equal("Dr Smith", result[0].DoctorName);

            _recordRepoMock.Verify(
                r => r.GetByPatientIdAsync(patientId),
                Times.Once);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenPatientIdIsInvalid_ThrowsArgumentException()
        {
            int patientId = 0;

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.GetByPatientIdAsync(patientId));

            Assert.Equal("Valid patient is required.", exception.Message);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenRecordsExist_ReturnsHealthRecordDtos()
        {
            int patientId = 5;

            var records = new List<HealthRecord>
            {
                GetSampleHealthRecord()
            };

            _recordRepoMock
                .Setup(r => r.GetByPatientIdAsync(patientId))
                .ReturnsAsync(records);

            var result = await _service.GetByPatientIdAsync(patientId);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(patientId, result[0].PatientId);
            Assert.Equal("John Patient", result[0].PatientName);
            Assert.Equal("Dr Smith", result[0].DoctorName);

            _recordRepoMock.Verify(
                r => r.GetByPatientIdAsync(patientId),
                Times.Once);
        }

        [Fact]
        public async Task ExistsByAppointmentIdAsync_WhenAppointmentIdIsInvalid_ThrowsArgumentException()
        {
            int appointmentId = 0;

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.ExistsByAppointmentIdAsync(appointmentId));

            Assert.Equal("Valid appointment is required.", exception.Message);
        }

        [Fact]
        public async Task ExistsByAppointmentIdAsync_WhenRecordDoesNotExist_ReturnsFalse()
        {
            int appointmentId = 1;

            _recordRepoMock
                .Setup(r => r.GetByAppointmentIdAsync(appointmentId))
                .ReturnsAsync((HealthRecord)null);

            var result = await _service.ExistsByAppointmentIdAsync(appointmentId);

            Assert.False(result);

            _recordRepoMock.Verify(
                r => r.GetByAppointmentIdAsync(appointmentId),
                Times.Once);
        }

        [Fact]
        public async Task ExistsByAppointmentIdAsync_WhenRecordExists_ReturnsTrue()
        {
            int appointmentId = 1;

            var record = new HealthRecord
            {
                HealthRecordId = 1,
                AppointmentId = appointmentId
            };

            _recordRepoMock
                .Setup(r => r.GetByAppointmentIdAsync(appointmentId))
                .ReturnsAsync(record);

            var result = await _service.ExistsByAppointmentIdAsync(appointmentId);

            Assert.True(result);

            _recordRepoMock.Verify(
                r => r.GetByAppointmentIdAsync(appointmentId),
                Times.Once);
        }
    }
}