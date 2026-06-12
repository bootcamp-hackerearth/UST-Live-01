using FluentAssertions;
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
            _recordRepoMock = new Mock<IHealthRecordRepository>();
            _appointmentRepoMock = new Mock<IAppointmentRepository>();

            _service = new HealthRecordService(
                _recordRepoMock.Object,
                _appointmentRepoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnHealthRecordDtos()
        {
            // Arrange
            var records = new List<HealthRecord>
            {
                CreateHealthRecord()
            };

            _recordRepoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(records);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            result[0].HealthRecordId.Should().Be(1);
            result[0].VisitDate.Should().Be(new DateTime(2026, 1, 10));
            result[0].PatientName.Should().Be("John Patient");
            result[0].DoctorName.Should().Be("Dr Smith");
            result[0].Diagnosis.Should().Be("Fever");
            result[0].Prescription.Should().Be("Paracetamol");
            result[0].Notes.Should().Be("Take rest");

            _recordRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenRecordExists_ShouldReturnHealthRecordDto()
        {
            // Arrange
            var record = CreateHealthRecord();

            _recordRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(record);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();

            result.HealthRecordId.Should().Be(1);
            result.AppointmentId.Should().Be(10);
            result.PatientId.Should().Be(100);
            result.VisitDate.Should().Be(new DateTime(2026, 1, 10));
            result.PatientName.Should().Be("John Patient");
            result.DoctorName.Should().Be("Dr Smith");
            result.Diagnosis.Should().Be("Fever");
            result.Prescription.Should().Be("Paracetamol");
            result.Notes.Should().Be("Take rest");

            _recordRepoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenRecordDoesNotExist_ShouldThrowException()
        {
            // Arrange
            _recordRepoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((HealthRecord)null);

            // Act
            Func<Task> act = async () =>
                await _service.GetByIdAsync(99);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Health record not found.");

            _recordRepoMock.Verify(r => r.GetByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppointmentIsNull_ShouldReturnUnknownPatientAndDoctor()
        {
            // Arrange
            var record = new HealthRecord
            {
                HealthRecordId = 1,
                AppointmentId = 10,
                Appointment = null,
                VisitDate = new DateTime(2026, 1, 10),
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest"
            };

            _recordRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(record);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();

            result.HealthRecordId.Should().Be(1);
            result.AppointmentId.Should().Be(10);
            result.PatientId.Should().Be(0);
            result.PatientName.Should().Be("Unknown Patient");
            result.DoctorName.Should().Be("Unknown Doctor");

            _recordRepoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientAndDoctorAreNull_ShouldReturnUnknownPatientAndDoctor()
        {
            // Arrange
            var record = CreateHealthRecord();
            record.Appointment.Patient = null;
            record.Appointment.Doctor = null;

            _recordRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(record);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();

            result.PatientId.Should().Be(100);
            result.PatientName.Should().Be("Unknown Patient");
            result.DoctorName.Should().Be("Unknown Doctor");

            _recordRepoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetPatientHistoryAsync_ShouldReturnPatientHealthRecords()
        {
            // Arrange
            var records = new List<HealthRecord>
            {
                CreateHealthRecord()
            };

            _recordRepoMock
                .Setup(r => r.GetByPatientIdAsync(100))
                .ReturnsAsync(records);

            // Act
            var result = await _service.GetPatientHistoryAsync(100);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            result[0].HealthRecordId.Should().Be(1);
            result[0].AppointmentId.Should().Be(10);
            result[0].PatientId.Should().Be(100);
            result[0].PatientName.Should().Be("John Patient");
            result[0].DoctorName.Should().Be("Dr Smith");
            result[0].Diagnosis.Should().Be("Fever");

            _recordRepoMock.Verify(r => r.GetByPatientIdAsync(100), Times.Once);
        }

        [Fact]
        public async Task GetPatientHistoryAsync_WhenAppointmentIsNull_ShouldReturnUnknownPatientAndDoctor()
        {
            // Arrange
            var records = new List<HealthRecord>
            {
                new HealthRecord
                {
                    HealthRecordId = 1,
                    AppointmentId = 10,
                    Appointment = null,
                    VisitDate = new DateTime(2026, 1, 10),
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Take rest"
                }
            };

            _recordRepoMock
                .Setup(r => r.GetByPatientIdAsync(100))
                .ReturnsAsync(records);

            // Act
            var result = await _service.GetPatientHistoryAsync(100);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            result[0].PatientId.Should().Be(0);
            result[0].PatientName.Should().Be("Unknown Patient");
            result[0].DoctorName.Should().Be("Unknown Doctor");

            _recordRepoMock.Verify(r => r.GetByPatientIdAsync(100), Times.Once);
        }

        [Fact]
        public async Task AddAsync_WhenAppointmentDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var dto = CreateHealthRecordDto();

            _appointmentRepoMock
                .Setup(r => r.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync((Appointment)null);

            // Act
            Func<Task> act = async () =>
                await _service.AddAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Appointment not found.");

            _appointmentRepoMock.Verify(
                r => r.GetByIdAsync(dto.AppointmentId),
                Times.Once);

            _recordRepoMock.Verify(
                r => r.GetByAppointmentIdAsync(It.IsAny<int>()),
                Times.Never);

            _recordRepoMock.Verify(
                r => r.AddAsync(It.IsAny<HealthRecord>()),
                Times.Never);

            _appointmentRepoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAsync_WhenAppointmentIsNotConfirmed_ShouldThrowException()
        {
            // Arrange
            var dto = CreateHealthRecordDto();

            var appointment = CreateAppointment();
            appointment.Status = (int)AppointmentStatus.Pending;

            _appointmentRepoMock
                .Setup(r => r.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () =>
                await _service.AddAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Health record can only be added for confirmed appointments.");

            _appointmentRepoMock.Verify(
                r => r.GetByIdAsync(dto.AppointmentId),
                Times.Once);

            _recordRepoMock.Verify(
                r => r.GetByAppointmentIdAsync(It.IsAny<int>()),
                Times.Never);

            _recordRepoMock.Verify(
                r => r.AddAsync(It.IsAny<HealthRecord>()),
                Times.Never);

            _appointmentRepoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAsync_WhenHealthRecordAlreadyExists_ShouldThrowException()
        {
            // Arrange
            var dto = CreateHealthRecordDto();

            var appointment = CreateAppointment();
            appointment.Status = (int)AppointmentStatus.Confirmed;

            var existingRecord = CreateHealthRecord();

            _appointmentRepoMock
                .Setup(r => r.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _recordRepoMock
                .Setup(r => r.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(existingRecord);

            // Act
            Func<Task> act = async () =>
                await _service.AddAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Health record already exists for this appointment.");

            _appointmentRepoMock.Verify(
                r => r.GetByIdAsync(dto.AppointmentId),
                Times.Once);

            _recordRepoMock.Verify(
                r => r.GetByAppointmentIdAsync(dto.AppointmentId),
                Times.Once);

            _recordRepoMock.Verify(
                r => r.AddAsync(It.IsAny<HealthRecord>()),
                Times.Never);

            _appointmentRepoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAsync_WhenValid_ShouldAddHealthRecordAndCompleteAppointment()
        {
            // Arrange
            var dto = CreateHealthRecordDto();

            var appointment = CreateAppointment();
            appointment.Status = (int)AppointmentStatus.Confirmed;

            _appointmentRepoMock
                .Setup(r => r.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _recordRepoMock
                .Setup(r => r.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord)null);

            _recordRepoMock
                .Setup(r => r.AddAsync(It.IsAny<HealthRecord>()))
                .Returns(Task.CompletedTask);

            _appointmentRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddAsync(dto);

            // Assert
            _recordRepoMock.Verify(r => r.AddAsync(It.Is<HealthRecord>(h =>
                h.AppointmentId == dto.AppointmentId &&
                h.Diagnosis == dto.Diagnosis &&
                h.Prescription == dto.Prescription &&
                h.Notes == dto.Notes &&
                h.VisitDate != default
            )), Times.Once);

            _appointmentRepoMock.Verify(r => r.UpdateAsync(It.Is<Appointment>(a =>
                a.AppointmentId == dto.AppointmentId &&
                a.Status == (int)AppointmentStatus.Completed
            )), Times.Once);
        }

        private static HealthRecord CreateHealthRecord()
        {
            return new HealthRecord
            {
                HealthRecordId = 1,
                AppointmentId = 10,
                Appointment = CreateAppointment(),
                VisitDate = new DateTime(2026, 1, 10),
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest"
            };
        }

        private static Appointment CreateAppointment()
        {
            return new Appointment
            {
                AppointmentId = 10,
                PatientId = 100,
                DoctorId = 200,
                Patient = new Patient
                {
                    PatientId = 100,
                    FullName = "John Patient"
                },
                Doctor = new Doctor
                {
                    DoctorId = 200,
                    FullName = "Dr Smith"
                },
                ScheduledDate = new DateTime(2026, 1, 10),
                TimeSlot = "10:00 AM",
                Status = (int)AppointmentStatus.Confirmed,
                CancellationReason = null
            };
        }

        private static CreateHealthRecordDto CreateHealthRecordDto()
        {
            return new CreateHealthRecordDto
            {
                AppointmentId = 10,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest"
            };
        }
    }
}
