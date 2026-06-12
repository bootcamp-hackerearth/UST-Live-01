using HealthAppWebApi.Models;
using HealthAppWebApi.Repositories.Interface;
using HealthAppWebApi.Services.Impl;
using Moq;
using SharedDto.HealthRecordDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HealthAppWebApi.Tests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _recordRepoMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _recordRepoMock =
                new Mock<IHealthRecordRepository>();

            _appointmentRepoMock =
                new Mock<IAppointmentRepository>();

            _service =
                new HealthRecordService(
                    _recordRepoMock.Object,
                    _appointmentRepoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsRecords()
        {
            // Arrange
            var records = new List<HealthRecord>
            {
                new HealthRecord
                {
                    HealthRecordId = 1,
                    VisitDate = DateTime.Today,
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Take rest",
                    Appointment = new Appointment
                    {
                        Patient = new Patient
                        {
                            FullName = "John Doe"
                        },
                        Doctor = new Doctor
                        {
                            FullName = "Dr Smith"
                        }
                    }
                }
            };

            _recordRepoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(records);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("John Doe", result[0].PatientName);
            Assert.Equal("Dr Smith", result[0].DoctorName);
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsRecord()
        {
            // Arrange
            var record = new HealthRecord
            {
                HealthRecordId = 1,
                AppointmentId = 10,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Medicine",
                Notes = "Notes",
                Appointment = new Appointment
                {
                    PatientId = 5,
                    Patient = new Patient
                    {
                        FullName = "John Doe"
                    },
                    Doctor = new Doctor
                    {
                        FullName = "Dr Smith"
                    }
                }
            };

            _recordRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(record);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.HealthRecordId);
            Assert.Equal(10, result.AppointmentId);
            Assert.Equal(5, result.PatientId);
            Assert.Equal("John Doe", result.PatientName);
            Assert.Equal("Dr Smith", result.DoctorName);
        }

        [Fact]
        public async Task GetByIdAsync_RecordNotFound_ThrowsException()
        {
            // Arrange
            _recordRepoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((HealthRecord)null);

            // Act & Assert
            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () => _service.GetByIdAsync(99));

            Assert.Equal(
                "Health record not found.",
                ex.Message);
        }

        [Fact]
        public async Task GetPatientHistoryAsync_ReturnsHistory()
        {
            // Arrange
            var records = new List<HealthRecord>
            {
                new HealthRecord
                {
                    HealthRecordId = 1,
                    AppointmentId = 10,
                    VisitDate = DateTime.Today,
                    Diagnosis = "Fever",
                    Prescription = "Medicine",
                    Notes = "Notes",
                    Appointment = new Appointment
                    {
                        PatientId = 1,
                        Patient = new Patient
                        {
                            FullName = "John Doe"
                        },
                        Doctor = new Doctor
                        {
                            FullName = "Dr Smith"
                        }
                    }
                }
            };

            _recordRepoMock
                .Setup(r => r.GetByPatientIdAsync(1))
                .ReturnsAsync(records);

            // Act
            var result =
                await _service.GetPatientHistoryAsync(1);

            // Assert
            Assert.Single(result);
            Assert.Equal("John Doe",
                result[0].PatientName);
        }

        [Fact]
        public async Task AddAsync_AppointmentNotFound_ThrowsException()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1,
                Diagnosis = "Fever",
                Prescription = "Medicine"
            };

            _appointmentRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Appointment)null);

            // Act & Assert
            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () => _service.AddAsync(dto));

            Assert.Equal(
                "Appointment not found.",
                ex.Message);
        }

        [Fact]
        public async Task AddAsync_AppointmentNotConfirmed_ThrowsException()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1,
                Diagnosis = "Fever",
                Prescription = "Medicine"
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Pending
            };

            _appointmentRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            // Act & Assert
            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () => _service.AddAsync(dto));

            Assert.Equal(
                "Health record can only be added for confirmed appointments.",
                ex.Message);
        }

        [Fact]
        public async Task AddAsync_RecordAlreadyExists_ThrowsException()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1,
                Diagnosis = "Fever",
                Prescription = "Medicine"
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Confirmed
            };

            var existingRecord = new HealthRecord
            {
                HealthRecordId = 1,
                AppointmentId = 1
            };

            _appointmentRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _recordRepoMock
                .Setup(r => r.GetByAppointmentIdAsync(1))
                .ReturnsAsync(existingRecord);

            // Act & Assert
            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () => _service.AddAsync(dto));

            Assert.Equal(
                "Health record already exists for this appointment.",
                ex.Message);
        }

        [Fact]
        public async Task AddAsync_ValidRecord_AddsRecordAndCompletesAppointment()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Rest for 3 days"
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _recordRepoMock
                .Setup(r => r.GetByAppointmentIdAsync(1))
                .ReturnsAsync((HealthRecord)null);

            // Act
            await _service.AddAsync(dto);

            // Assert
            _recordRepoMock.Verify(
                r => r.AddAsync(
                    It.Is<HealthRecord>(h =>
                        h.AppointmentId == 1 &&
                        h.Diagnosis == "Fever" &&
                        h.Prescription == "Paracetamol")),
                Times.Once);

            _appointmentRepoMock.Verify(
                r => r.UpdateAsync(
                    It.Is<Appointment>(a =>
                        a.Status ==
                        AppointmentStatus.Completed)),
                Times.Once);
        }

        [Fact]
        public async Task AddAsync_ValidRecord_ChangesAppointmentStatusToCompleted()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 5,
                Diagnosis = "Cold",
                Prescription = "Tablet"
            };

            var appointment = new Appointment
            {
                AppointmentId = 5,
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepoMock
                .Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(appointment);

            _recordRepoMock
                .Setup(r => r.GetByAppointmentIdAsync(5))
                .ReturnsAsync((HealthRecord)null);

            // Act
            await _service.AddAsync(dto);

            // Assert
            Assert.Equal(
                AppointmentStatus.Completed,
                appointment.Status);
        }
    }
}