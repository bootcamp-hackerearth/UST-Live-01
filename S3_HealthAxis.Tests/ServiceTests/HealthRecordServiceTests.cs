using FluentAssertions;
using Moq;
using S3_HealthAxis.Shared.DTOs.HealthRecord;
using S3_HealthAxis.Shared.Enums;
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
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((HealthRecord?)null);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().BeNull();

            _healthRecordRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenRecordExists()
        {
            // Arrange
            var record = BuildHealthRecord(
                healthRecordId: 1,
                appointmentId: 100,
                patientId: 10,
                doctorId: 20);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(record);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.HealthRecordId.Should().Be(1);
            result.AppointmentId.Should().Be(100);
            result.PatientId.Should().Be(10);
            result.DoctorId.Should().Be(20);
            result.DoctorName.Should().Be("Doctor 20");
            result.DoctorSpecialisation.Should().Be((int)DoctorSpecialisation.Cardiologist);
            result.Diagnosis.Should().Be("Fever");
            result.Prescription.Should().Be("Paracetamol");
            result.Notes.Should().Be("Rest for 2 days");

            _healthRecordRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldMapEmptyDoctorDetails_WhenDoctorNavigationIsNull()
        {
            // Arrange
            var record = BuildHealthRecord(
                healthRecordId: 1,
                appointmentId: 100,
                patientId: 10,
                doctorId: 20);

            record.Doctor = null!;

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(record);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.DoctorName.Should().BeEmpty();
            result.DoctorSpecialisation.Should().Be(0);
        }

        [Fact]
        public async Task GetByAppointmentIdAsync_ShouldReturnNull_WhenRecordDoesNotExist()
        {
            // Arrange
            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(100))
                .ReturnsAsync((HealthRecord?)null);

            // Act
            var result = await _service.GetByAppointmentIdAsync(100);

            // Assert
            result.Should().BeNull();

            _healthRecordRepositoryMock.Verify(x => x.GetByAppointmentIdAsync(100), Times.Once);
        }

        [Fact]
        public async Task GetByAppointmentIdAsync_ShouldReturnMappedDto_WhenRecordExists()
        {
            // Arrange
            var record = BuildHealthRecord(
                healthRecordId: 1,
                appointmentId: 100,
                patientId: 10,
                doctorId: 20);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(100))
                .ReturnsAsync(record);

            // Act
            var result = await _service.GetByAppointmentIdAsync(100);

            // Assert
            result.Should().NotBeNull();
            result!.HealthRecordId.Should().Be(1);
            result.AppointmentId.Should().Be(100);
            result.DoctorName.Should().Be("Doctor 20");

            _healthRecordRepositoryMock.Verify(x => x.GetByAppointmentIdAsync(100), Times.Once);
        }

        [Fact]
        public async Task GetByPatientIdAsync_ShouldThrowKeyNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync((Patient?)null);

            // Act
            var act = async () => await _service.GetByPatientIdAsync(10);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Patient with Id 10 not found.");

            _healthRecordRepositoryMock.Verify(x => x.GetByPatientIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetByPatientIdAsync_ShouldReturnMappedRecords_WhenPatientExists()
        {
            // Arrange
            var patient = BuildPatient(10);

            var records = new List<HealthRecord>
            {
                BuildHealthRecord(1, 100, 10, 20),
                BuildHealthRecord(2, 101, 10, 21)
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(patient);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(10))
                .ReturnsAsync(records);

            // Act
            var result = (await _service.GetByPatientIdAsync(10)).ToList();

            // Assert
            result.Should().HaveCount(2);

            result[0].HealthRecordId.Should().Be(1);
            result[0].DoctorName.Should().Be("Doctor 20");

            result[1].HealthRecordId.Should().Be(2);
            result[1].DoctorName.Should().Be("Doctor 21");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
            _healthRecordRepositoryMock.Verify(x => x.GetByPatientIdAsync(10), Times.Once);
        }

        [Theory]
        [InlineData(0, 10, 20, "Fever", "Medicine", "AppointmentId is required.")]
        [InlineData(-1, 10, 20, "Fever", "Medicine", "AppointmentId is required.")]
        [InlineData(100, 0, 20, "Fever", "Medicine", "PatientId is required.")]
        [InlineData(100, -1, 20, "Fever", "Medicine", "PatientId is required.")]
        [InlineData(100, 10, 0, "Fever", "Medicine", "DoctorId is required.")]
        [InlineData(100, 10, -1, "Fever", "Medicine", "DoctorId is required.")]
        [InlineData(100, 10, 20, "", "Medicine", "Diagnosis is required.")]
        [InlineData(100, 10, 20, "   ", "Medicine", "Diagnosis is required.")]
        [InlineData(100, 10, 20, null, "Medicine", "Diagnosis is required.")]
        [InlineData(100, 10, 20, "Fever", "", "Prescription is required.")]
        [InlineData(100, 10, 20, "Fever", "   ", "Prescription is required.")]
        [InlineData(100, 10, 20, "Fever", null, "Prescription is required.")]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenCreateDtoValidationFails(
            int appointmentId,
            int patientId,
            int doctorId,
            string? diagnosis,
            string? prescription,
            string expectedMessage)
        {
            // Arrange
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                DoctorId = doctorId,
                Diagnosis = diagnosis,
                Prescription = prescription,
                Notes = "Notes"
            };

            // Act
            var act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage(expectedMessage);

            _appointmentRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _healthRecordRepositoryMock.Verify(x => x.AddAsync(It.IsAny<HealthRecord>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowKeyNotFoundException_WhenAppointmentDoesNotExist()
        {
            // Arrange
            var dto = BuildValidCreateDto();

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync((Appointment?)null);

            // Act
            var act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Appointment not found.");

            _healthRecordRepositoryMock.Verify(x => x.GetByAppointmentIdAsync(It.IsAny<int>()), Times.Never);
            _healthRecordRepositoryMock.Verify(x => x.AddAsync(It.IsAny<HealthRecord>()), Times.Never);
        }

        [Theory]
        [InlineData(AppointmentStatus.Pending)]
        [InlineData(AppointmentStatus.Confirmed)]
        [InlineData(AppointmentStatus.Cancelled)]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenAppointmentIsNotCompleted(
            AppointmentStatus status)
        {
            // Arrange
            var dto = BuildValidCreateDto();

            var appointment = BuildAppointment(
                dto.AppointmentId,
                dto.PatientId,
                dto.DoctorId,
                status);

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            // Act
            var act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Health record can only be created for completed appointments.");

            _healthRecordRepositoryMock.Verify(x => x.GetByAppointmentIdAsync(It.IsAny<int>()), Times.Never);
            _healthRecordRepositoryMock.Verify(x => x.AddAsync(It.IsAny<HealthRecord>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenRecordAlreadyExistsForAppointment()
        {
            // Arrange
            var dto = BuildValidCreateDto();

            var appointment = BuildAppointment(
                dto.AppointmentId,
                dto.PatientId,
                dto.DoctorId,
                AppointmentStatus.Completed);

            var existingRecord = BuildHealthRecord(
                1,
                dto.AppointmentId,
                dto.PatientId,
                dto.DoctorId);

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(existingRecord);

            // Act
            var act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("A health record already exists for this appointment.");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _healthRecordRepositoryMock.Verify(x => x.AddAsync(It.IsAny<HealthRecord>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowKeyNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            var dto = BuildValidCreateDto();

            var appointment = BuildAppointment(
                dto.AppointmentId,
                dto.PatientId,
                dto.DoctorId,
                AppointmentStatus.Completed);

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
            var act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Patient not found.");

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _healthRecordRepositoryMock.Verify(x => x.AddAsync(It.IsAny<HealthRecord>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowKeyNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            var dto = BuildValidCreateDto();

            var appointment = BuildAppointment(
                dto.AppointmentId,
                dto.PatientId,
                dto.DoctorId,
                AppointmentStatus.Completed);

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord?)null);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(BuildPatient(dto.PatientId));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync((Doctor?)null);

            // Act
            var act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Doctor not found.");

            _healthRecordRepositoryMock.Verify(x => x.AddAsync(It.IsAny<HealthRecord>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenPatientDoesNotMatchAppointment()
        {
            // Arrange
            var dto = BuildValidCreateDto();

            var appointment = BuildAppointment(
                dto.AppointmentId,
                patientId: 999,
                doctorId: dto.DoctorId,
                AppointmentStatus.Completed);

            SetupCreateDependencies(dto, appointment);

            // Act
            var act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Patient does not match appointment.");

            _healthRecordRepositoryMock.Verify(x => x.AddAsync(It.IsAny<HealthRecord>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenDoctorDoesNotMatchAppointment()
        {
            // Arrange
            var dto = BuildValidCreateDto();

            var appointment = BuildAppointment(
                dto.AppointmentId,
                patientId: dto.PatientId,
                doctorId: 999,
                AppointmentStatus.Completed);

            SetupCreateDependencies(dto, appointment);

            // Act
            var act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Doctor does not match appointment.");

            _healthRecordRepositoryMock.Verify(x => x.AddAsync(It.IsAny<HealthRecord>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateHealthRecord_WhenRequestIsValid_AndCreatedRecordIsReturned()
        {
            // Arrange
            var dto = BuildValidCreateDto();
            dto.Diagnosis = " Fever ";
            dto.Prescription = " Paracetamol ";
            dto.Notes = " Take rest ";

            var appointment = BuildAppointment(
                dto.AppointmentId,
                dto.PatientId,
                dto.DoctorId,
                AppointmentStatus.Completed);

            SetupCreateDependencies(dto, appointment);

            HealthRecord? capturedRecord = null;

            _healthRecordRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<HealthRecord>()))
                .Callback<HealthRecord>(record =>
                {
                    capturedRecord = record;
                    record.HealthRecordId = 777;
                })
                .Returns(Task.CompletedTask);

            _healthRecordRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(777))
                .ReturnsAsync(() =>
                {
                    capturedRecord!.Doctor = BuildDoctor(dto.DoctorId);
                    return capturedRecord;
                });

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            capturedRecord.Should().NotBeNull();
            capturedRecord!.AppointmentId.Should().Be(dto.AppointmentId);
            capturedRecord.PatientId.Should().Be(dto.PatientId);
            capturedRecord.DoctorId.Should().Be(dto.DoctorId);
            capturedRecord.Diagnosis.Should().Be("Fever");
            capturedRecord.Prescription.Should().Be("Paracetamol");
            capturedRecord.Notes.Should().Be("Take rest");
            capturedRecord.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));

            result.HealthRecordId.Should().Be(777);
            result.AppointmentId.Should().Be(dto.AppointmentId);
            result.PatientId.Should().Be(dto.PatientId);
            result.DoctorId.Should().Be(dto.DoctorId);
            result.DoctorName.Should().Be("Doctor 20");
            result.DoctorSpecialisation.Should().Be((int)DoctorSpecialisation.Cardiologist);
            result.Diagnosis.Should().Be("Fever");
            result.Prescription.Should().Be("Paracetamol");
            result.Notes.Should().Be("Take rest");

            _healthRecordRepositoryMock.Verify(x => x.AddAsync(It.IsAny<HealthRecord>()), Times.Once);
            _healthRecordRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            _healthRecordRepositoryMock.Verify(x => x.GetByIdAsync(777), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateHealthRecord_WithNullNotes()
        {
            // Arrange
            var dto = BuildValidCreateDto();
            dto.Notes = null;

            var appointment = BuildAppointment(
                dto.AppointmentId,
                dto.PatientId,
                dto.DoctorId,
                AppointmentStatus.Completed);

            SetupCreateDependencies(dto, appointment);

            HealthRecord? capturedRecord = null;

            _healthRecordRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<HealthRecord>()))
                .Callback<HealthRecord>(record =>
                {
                    capturedRecord = record;
                    record.HealthRecordId = 778;
                })
                .Returns(Task.CompletedTask);

            _healthRecordRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(778))
                .ReturnsAsync(() => capturedRecord);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            capturedRecord.Should().NotBeNull();
            capturedRecord!.Notes.Should().BeNull();
            result.Notes.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnFallbackRecord_WhenRepositoryDoesNotReturnCreatedRecord()
        {
            // Arrange
            var dto = BuildValidCreateDto();

            var appointment = BuildAppointment(
                dto.AppointmentId,
                dto.PatientId,
                dto.DoctorId,
                AppointmentStatus.Completed);

            SetupCreateDependencies(dto, appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<HealthRecord>()))
                .Callback<HealthRecord>(record => record.HealthRecordId = 888)
                .Returns(Task.CompletedTask);

            _healthRecordRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(888))
                .ReturnsAsync((HealthRecord?)null);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.HealthRecordId.Should().Be(888);
            result.DoctorName.Should().BeEmpty();
            result.DoctorSpecialisation.Should().Be(0);
            result.Diagnosis.Should().Be(dto.Diagnosis);
            result.Prescription.Should().Be(dto.Prescription);
        }

        [Theory]
        [InlineData("", "Prescription", "Diagnosis is required.")]
        [InlineData("   ", "Prescription", "Diagnosis is required.")]
        [InlineData(null, "Prescription", "Diagnosis is required.")]
        [InlineData("Diagnosis", "", "Prescription is required.")]
        [InlineData("Diagnosis", "   ", "Prescription is required.")]
        [InlineData("Diagnosis", null, "Prescription is required.")]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenUpdateDtoValidationFails(
            string? diagnosis,
            string? prescription,
            string expectedMessage)
        {
            // Arrange
            var dto = new UpdateHealthRecordDto
            {
                Diagnosis = diagnosis,
                Prescription = prescription,
                Notes = "Notes"
            };

            // Act
            var act = async () => await _service.UpdateAsync(1, dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage(expectedMessage);

            _healthRecordRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenRecordDoesNotExist()
        {
            // Arrange
            var dto = BuildValidUpdateDto();

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((HealthRecord?)null);

            // Act
            var act = async () => await _service.UpdateAsync(1, dto);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Health record 1 not found.");

            _healthRecordRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<HealthRecord>()), Times.Never);
            _healthRecordRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateRecord_WhenRequestIsValid()
        {
            // Arrange
            var record = BuildHealthRecord(1, 100, 10, 20);

            var dto = new UpdateHealthRecordDto
            {
                Diagnosis = " Updated Diagnosis ",
                Prescription = " Updated Prescription ",
                Notes = " Updated Notes "
            };

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(record);

            _healthRecordRepositoryMock
                .Setup(x => x.UpdateAsync(record))
                .Returns(Task.CompletedTask);

            _healthRecordRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(1, dto);

            // Assert
            record.Diagnosis.Should().Be("Updated Diagnosis");
            record.Prescription.Should().Be("Updated Prescription");
            record.Notes.Should().Be("Updated Notes");

            _healthRecordRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
            _healthRecordRepositoryMock.Verify(x => x.UpdateAsync(record), Times.Once);
            _healthRecordRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldSetNotesToNull_WhenNotesIsNull()
        {
            // Arrange
            var record = BuildHealthRecord(1, 100, 10, 20);

            var dto = new UpdateHealthRecordDto
            {
                Diagnosis = "Updated Diagnosis",
                Prescription = "Updated Prescription",
                Notes = null
            };

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(record);

            _healthRecordRepositoryMock
                .Setup(x => x.UpdateAsync(record))
                .Returns(Task.CompletedTask);

            _healthRecordRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(1, dto);

            // Assert
            record.Notes.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldTrimNotes_WhenNotesHasWhitespace()
        {
            // Arrange
            var record = BuildHealthRecord(1, 100, 10, 20);

            var dto = new UpdateHealthRecordDto
            {
                Diagnosis = "Updated Diagnosis",
                Prescription = "Updated Prescription",
                Notes = "  Clean notes  "
            };

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(record);

            _healthRecordRepositoryMock
                .Setup(x => x.UpdateAsync(record))
                .Returns(Task.CompletedTask);

            _healthRecordRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(1, dto);

            // Assert
            record.Notes.Should().Be("Clean notes");
        }

        private void SetupCreateDependencies(
            CreateHealthRecordDto dto,
            Appointment appointment)
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync((HealthRecord?)null);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(BuildPatient(dto.PatientId));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(BuildDoctor(dto.DoctorId));
        }

        private static CreateHealthRecordDto BuildValidCreateDto()
        {
            return new CreateHealthRecordDto
            {
                AppointmentId = 100,
                PatientId = 10,
                DoctorId = 20,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Rest for 2 days"
            };
        }

        private static UpdateHealthRecordDto BuildValidUpdateDto()
        {
            return new UpdateHealthRecordDto
            {
                Diagnosis = "Updated Diagnosis",
                Prescription = "Updated Prescription",
                Notes = "Updated Notes"
            };
        }

        private static Patient BuildPatient(int id)
        {
            return new Patient
            {
                PatientId = id,
                FullName = $"Patient {id}",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = $"patient{id}@test.com",
                InsuranceNumber = $"INS{id}",
                IsActive = true
            };
        }

        private static Doctor BuildDoctor(int id)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = $"Doctor {id}",
                Email = $"doctor{id}@test.com",
                Specialisation = DoctorSpecialisation.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };
        }

        private static Appointment BuildAppointment(
            int appointmentId,
            int patientId,
            int doctorId,
            AppointmentStatus status)
        {
            var patient = BuildPatient(patientId);
            var doctor = BuildDoctor(doctorId);

            return new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                Patient = patient,
                DoctorId = doctorId,
                Doctor = doctor,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today),
                TimeSlot = AppointmentTimeSlot.TenAM,
                Status = status
            };
        }

        private static HealthRecord BuildHealthRecord(
            int healthRecordId,
            int appointmentId,
            int patientId,
            int doctorId)
        {
            return new HealthRecord
            {
                HealthRecordId = healthRecordId,
                AppointmentId = appointmentId,
                PatientId = patientId,
                DoctorId = doctorId,
                Doctor = BuildDoctor(doctorId),
                Patient = BuildPatient(patientId),
                Appointment = BuildAppointment(
                    appointmentId,
                    patientId,
                    doctorId,
                    AppointmentStatus.Completed),
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Rest for 2 days",
                CreatedOn = DateTime.UtcNow
            };
        }
    }
}

