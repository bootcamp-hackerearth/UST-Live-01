using FluentAssertions;
using Moq;
using S3_HealthAxisApi.DTOs.HealthRecord;
using S3_HealthAxisApi.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Implementation;
using Xunit;

namespace S3_HealthAxis.Tests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _healthRecordRepositoryMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();

            _service = new HealthRecordService(
                _healthRecordRepositoryMock.Object,
                _appointmentRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _doctorRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenRecordDoesNotExist()
        {
            // Arrange
            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(100))
                .ReturnsAsync((HealthRecord?)null);

            // Act
            var result = await _service.GetByIdAsync(100);

            // Assert
            result.Should().BeNull();

            _healthRecordRepositoryMock.Verify(x => x.GetByIdAsync(100), Times.Once);
            _healthRecordRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenRecordExists()
        {
            // Arrange
            var record = CreateHealthRecord(
                healthRecordId: 1,
                appointmentId: 10,
                patientId: 20,
                doctorId: 30,
                diagnosis: "Fever",
                prescription: "Paracetamol",
                notes: "Drink water");

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(record);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.HealthRecordId.Should().Be(1);
            result.AppointmentId.Should().Be(10);
            result.PatientId.Should().Be(20);
            result.DoctorId.Should().Be(30);
            result.Diagnosis.Should().Be("Fever");
            result.Prescription.Should().Be("Paracetamol");
            result.Notes.Should().Be("Drink water");

            _healthRecordRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
            _healthRecordRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByAppointmentIdAsync_ShouldReturnNull_WhenRecordDoesNotExist()
        {
            // Arrange
            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(999))
                .ReturnsAsync((HealthRecord?)null);

            // Act
            var result = await _service.GetByAppointmentIdAsync(999);

            // Assert
            result.Should().BeNull();

            _healthRecordRepositoryMock.Verify(x => x.GetByAppointmentIdAsync(999), Times.Once);
            _healthRecordRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByAppointmentIdAsync_ShouldReturnMappedDto_WhenRecordExists()
        {
            // Arrange
            var record = CreateHealthRecord(
                healthRecordId: 2,
                appointmentId: 50,
                patientId: 60,
                doctorId: 70,
                diagnosis: "Cold",
                prescription: "Syrup",
                notes: "Rest well");

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(50))
                .ReturnsAsync(record);

            // Act
            var result = await _service.GetByAppointmentIdAsync(50);

            // Assert
            result.Should().NotBeNull();
            result!.HealthRecordId.Should().Be(2);
            result.AppointmentId.Should().Be(50);
            result.PatientId.Should().Be(60);
            result.DoctorId.Should().Be(70);
            result.Diagnosis.Should().Be("Cold");
            result.Prescription.Should().Be("Syrup");
            result.Notes.Should().Be("Rest well");

            _healthRecordRepositoryMock.Verify(x => x.GetByAppointmentIdAsync(50), Times.Once);
            _healthRecordRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenAppointmentIdIsInvalid()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 0,
                PatientId = 1,
                DoctorId = 1,
                Diagnosis = "Fever",
                Prescription = "Medicine"
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*AppointmentId is required*");

            _appointmentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenPatientIdIsInvalid()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1,
                PatientId = 0,
                DoctorId = 1,
                Diagnosis = "Fever",
                Prescription = "Medicine"
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*PatientId is required*");

            _appointmentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenDoctorIdIsInvalid()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 0,
                Diagnosis = "Fever",
                Prescription = "Medicine"
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*DoctorId is required*");

            _appointmentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenDiagnosisIsMissing()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1,
                Diagnosis = "   ",
                Prescription = "Medicine"
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Diagnosis is required*");

            _appointmentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenPrescriptionIsMissing()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1,
                Diagnosis = "Fever",
                Prescription = "   "
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Prescription is required*");

            _appointmentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowKeyNotFoundException_WhenAppointmentDoesNotExist()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 10,
                PatientId = 20,
                DoctorId = 30,
                Diagnosis = "Fever",
                Prescription = "Medicine"
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync((Appointment?)null);

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Appointment not found*");

            _appointmentRepositoryMock.Verify(x => x.GetByIdAsync(dto.AppointmentId), Times.Once);
            _healthRecordRepositoryMock.Verify(x => x.GetByAppointmentIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenAppointmentIsNotCompleted()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 10,
                PatientId = 20,
                DoctorId = 30,
                Diagnosis = "Fever",
                Prescription = "Medicine"
            };

            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 20,
                doctorId: 30,
                status: AppointmentStatus.Confirmed);

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*only be created for completed appointments*");

            _healthRecordRepositoryMock.Verify(x => x.GetByAppointmentIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenRecordAlreadyExistsForAppointment()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 10,
                PatientId = 20,
                DoctorId = 30,
                Diagnosis = "Fever",
                Prescription = "Medicine"
            };

            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 20,
                doctorId: 30,
                status: AppointmentStatus.Completed);

            var existingRecord = CreateHealthRecord(
                healthRecordId: 99,
                appointmentId: 10,
                patientId: 20,
                doctorId: 30,
                diagnosis: "Old",
                prescription: "Old Rx");

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(existingRecord);

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*already exists for this appointment*");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowKeyNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 10,
                PatientId = 20,
                DoctorId = 30,
                Diagnosis = "Fever",
                Prescription = "Medicine"
            };

            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 20,
                doctorId: 30,
                status: AppointmentStatus.Completed);

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord?)null);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync((Patient?)null);

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Patient not found*");

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowKeyNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 10,
                PatientId = 20,
                DoctorId = 30,
                Diagnosis = "Fever",
                Prescription = "Medicine"
            };

            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 20,
                doctorId: 30,
                status: AppointmentStatus.Completed);

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord?)null);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(CreatePatient(dto.PatientId));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync((Doctor?)null);

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Doctor not found*");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenPatientDoesNotMatchAppointment()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 10,
                PatientId = 999,
                DoctorId = 30,
                Diagnosis = "Fever",
                Prescription = "Medicine"
            };

            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 20,
                doctorId: 30,
                status: AppointmentStatus.Completed);

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord?)null);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(CreatePatient(dto.PatientId));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(CreateDoctor(dto.DoctorId));

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Patient does not match appointment*");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenDoctorDoesNotMatchAppointment()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 10,
                PatientId = 20,
                DoctorId = 999,
                Diagnosis = "Fever",
                Prescription = "Medicine"
            };

            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 20,
                doctorId: 30,
                status: AppointmentStatus.Completed);

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord?)null);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(CreatePatient(dto.PatientId));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(CreateDoctor(dto.DoctorId));

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Doctor does not match appointment*");
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateHealthRecord_WhenRequestIsValid()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 10,
                PatientId = 20,
                DoctorId = 30,
                Diagnosis = "  Viral Fever  ",
                Prescription = "  Paracetamol  ",
                Notes = "  Drink more fluids  "
            };

            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 20,
                doctorId: 30,
                status: AppointmentStatus.Completed);

            HealthRecord? capturedRecord = null;

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord?)null);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(CreatePatient(dto.PatientId));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(CreateDoctor(dto.DoctorId));

            _healthRecordRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<HealthRecord>()))
                .Callback<HealthRecord>(record =>
                {
                    capturedRecord = record;
                    record.HealthRecordId = 500; // simulate DB id
                })
                .Returns(Task.CompletedTask);

            _healthRecordRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            capturedRecord.Should().NotBeNull();
            capturedRecord!.AppointmentId.Should().Be(10);
            capturedRecord.PatientId.Should().Be(20);
            capturedRecord.DoctorId.Should().Be(30);
            capturedRecord.Diagnosis.Should().Be("Viral Fever");
            capturedRecord.Prescription.Should().Be("Paracetamol");
            capturedRecord.Notes.Should().Be("Drink more fluids");
            capturedRecord.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));

            result.Should().NotBeNull();
            result.HealthRecordId.Should().Be(500);
            result.AppointmentId.Should().Be(10);
            result.PatientId.Should().Be(20);
            result.DoctorId.Should().Be(30);
            result.Diagnosis.Should().Be("Viral Fever");
            result.Prescription.Should().Be("Paracetamol");
            result.Notes.Should().Be("Drink more fluids");

            _healthRecordRepositoryMock.Verify(x => x.AddAsync(It.IsAny<HealthRecord>()), Times.Once);
            _healthRecordRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldAllowNullNotes_WhenRequestIsValid()
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 11,
                PatientId = 21,
                DoctorId = 31,
                Diagnosis = "Cough",
                Prescription = "Syrup",
                Notes = null
            };

            var appointment = CreateAppointment(
                appointmentId: 11,
                patientId: 21,
                doctorId: 31,
                status: AppointmentStatus.Completed);

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord?)null);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(CreatePatient(dto.PatientId));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(CreateDoctor(dto.DoctorId));

            _healthRecordRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<HealthRecord>()))
                .Callback<HealthRecord>(record => record.HealthRecordId = 501)
                .Returns(Task.CompletedTask);

            _healthRecordRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.HealthRecordId.Should().Be(501);
            result.Notes.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenDiagnosisIsMissing()
        {
            // Arrange
            var dto = new UpdateHealthRecordDto
            {
                Diagnosis = "   ",
                Prescription = "Medicine"
            };

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Diagnosis is required*");

            _healthRecordRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenPrescriptionIsMissing()
        {
            // Arrange
            var dto = new UpdateHealthRecordDto
            {
                Diagnosis = "Fever",
                Prescription = "   "
            };

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Prescription is required*");

            _healthRecordRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenRecordDoesNotExist()
        {
            // Arrange
            var dto = new UpdateHealthRecordDto
            {
                Diagnosis = "Updated Diagnosis",
                Prescription = "Updated Prescription",
                Notes = "Updated Notes"
            };

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(404))
                .ReturnsAsync((HealthRecord?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(404, dto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Health record 404 not found*");

            _healthRecordRepositoryMock.Verify(x => x.GetByIdAsync(404), Times.Once);
            _healthRecordRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<HealthRecord>()), Times.Never);
            _healthRecordRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateHealthRecord_WhenRequestIsValid()
        {
            // Arrange
            var existingRecord = CreateHealthRecord(
                healthRecordId: 55,
                appointmentId: 10,
                patientId: 20,
                doctorId: 30,
                diagnosis: "Old Diagnosis",
                prescription: "Old Prescription",
                notes: "Old Notes");

            var dto = new UpdateHealthRecordDto
            {
                Diagnosis = "  New Diagnosis  ",
                Prescription = "  New Prescription  ",
                Notes = "  New Notes  "
            };

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(55))
                .ReturnsAsync(existingRecord);

            _healthRecordRepositoryMock
                .Setup(x => x.UpdateAsync(existingRecord))
                .Returns(Task.CompletedTask);

            _healthRecordRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(55, dto);

            // Assert
            existingRecord.Diagnosis.Should().Be("New Diagnosis");
            existingRecord.Prescription.Should().Be("New Prescription");
            existingRecord.Notes.Should().Be("New Notes");

            _healthRecordRepositoryMock.Verify(x => x.GetByIdAsync(55), Times.Once);
            _healthRecordRepositoryMock.Verify(x => x.UpdateAsync(existingRecord), Times.Once);
            _healthRecordRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldAllowNullNotes_WhenRequestIsValid()
        {
            // Arrange
            var existingRecord = CreateHealthRecord(
                healthRecordId: 56,
                appointmentId: 10,
                patientId: 20,
                doctorId: 30,
                diagnosis: "Old Diagnosis",
                prescription: "Old Prescription",
                notes: "Old Notes");

            var dto = new UpdateHealthRecordDto
            {
                Diagnosis = "Updated Diagnosis",
                Prescription = "Updated Prescription",
                Notes = null
            };

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(56))
                .ReturnsAsync(existingRecord);

            _healthRecordRepositoryMock
                .Setup(x => x.UpdateAsync(existingRecord))
                .Returns(Task.CompletedTask);

            _healthRecordRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(56, dto);

            // Assert
            existingRecord.Diagnosis.Should().Be("Updated Diagnosis");
            existingRecord.Prescription.Should().Be("Updated Prescription");
            existingRecord.Notes.Should().BeNull();

            _healthRecordRepositoryMock.Verify(x => x.GetByIdAsync(56), Times.Once);
            _healthRecordRepositoryMock.Verify(x => x.UpdateAsync(existingRecord), Times.Once);
            _healthRecordRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        private static HealthRecord CreateHealthRecord(
            int healthRecordId,
            int appointmentId,
            int patientId,
            int doctorId,
            string diagnosis,
            string prescription,
            string? notes = null)
        {
            return new HealthRecord
            {
                HealthRecordId = healthRecordId,
                AppointmentId = appointmentId,
                PatientId = patientId,
                DoctorId = doctorId,
                Diagnosis = diagnosis,
                Prescription = prescription,
                Notes = notes,
                CreatedOn = DateTime.UtcNow
            };
        }

        private static Appointment CreateAppointment(
            int appointmentId,
            int patientId,
            int doctorId,
            AppointmentStatus status)
        {
            return new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                DoctorId = doctorId,
                Status = status
            };
        }

        private static Patient CreatePatient(int patientId)
        {
            return new Patient
            {
                PatientId = patientId,
                FullName = "Test Patient",
                IsActive = true
            };
        }

        private static Doctor CreateDoctor(int doctorId)
        {
            return new Doctor
            {
                DoctorId = doctorId,
                FullName = "Test Doctor",
                IsActive = true
            };
        }
    }
}