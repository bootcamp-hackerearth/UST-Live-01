using AutoMapper;
using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.DTOs.Patients;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using Moq;

namespace HealthAxis.API.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IHealthRecordRepository> _healthRecordRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();
            _mapperMock = new Mock<IMapper>();

            _patientService = new PatientService(
                _patientRepositoryMock.Object,
                _healthRecordRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenHealthRecordsExist_ReturnsMappedHealthRecordDtos()
        {
            // Arrange
            const int patientId = 1;

            List<HealthRecord> healthRecords = new()
            {
                new HealthRecord
                {
                    RecordId = 1,
                    AppointmentId = 10,
                    PatientId = patientId,
                    DoctorId = 5,
                    VisitDate = new DateTime(2026, 6, 14),
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Rest advised"
                },
                new HealthRecord
                {
                    RecordId = 2,
                    AppointmentId = 11,
                    PatientId = patientId,
                    DoctorId = 6,
                    VisitDate = new DateTime(2026, 6, 20),
                    Diagnosis = "Cold",
                    Prescription = "Antihistamine",
                    Notes = "Drink warm water"
                }
            };

            List<HealthRecordReadDto> expectedDtos = new()
            {
                new HealthRecordReadDto
                {
                    HealthRecordId = 1,
                    AppointmentId = 10,
                    PatientId = patientId,
                    DoctorId = 5,
                    VisitDate = new DateTime(2026, 6, 14),
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Rest advised"
                },
                new HealthRecordReadDto
                {
                    HealthRecordId = 2,
                    AppointmentId = 11,
                    PatientId = patientId,
                    DoctorId = 6,
                    VisitDate = new DateTime(2026, 6, 20),
                    Diagnosis = "Cold",
                    Prescription = "Antihistamine",
                    Notes = "Drink warm water"
                }
            };

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecords);

            _mapperMock
                .Setup(mapper => mapper.Map<List<HealthRecordReadDto>>(healthRecords))
                .Returns(expectedDtos);

            // Act
            List<HealthRecordReadDto> result =
                await _patientService.GetHealthRecordsAsync(patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(expectedDtos[0].HealthRecordId, result[0].HealthRecordId);
            Assert.Equal(expectedDtos[0].PatientId, result[0].PatientId);
            Assert.Equal(expectedDtos[0].Diagnosis, result[0].Diagnosis);

            Assert.Equal(expectedDtos[1].HealthRecordId, result[1].HealthRecordId);
            Assert.Equal(expectedDtos[1].PatientId, result[1].PatientId);
            Assert.Equal(expectedDtos[1].Diagnosis, result[1].Diagnosis);

            _healthRecordRepositoryMock.Verify(
                repository => repository.GetByPatientIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _mapperMock.Verify(
                mapper => mapper.Map<List<HealthRecordReadDto>>(healthRecords),
                Times.Once);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenNoHealthRecordsExist_ReturnsEmptyList()
        {
            // Arrange
            const int patientId = 1;

            List<HealthRecord> healthRecords = new();

            List<HealthRecordReadDto> expectedDtos = new();

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecords);

            _mapperMock
                .Setup(mapper => mapper.Map<List<HealthRecordReadDto>>(healthRecords))
                .Returns(expectedDtos);

            // Act
            List<HealthRecordReadDto> result =
                await _patientService.GetHealthRecordsAsync(patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _healthRecordRepositoryMock.Verify(
                repository => repository.GetByPatientIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _mapperMock.Verify(
                mapper => mapper.Map<List<HealthRecordReadDto>>(healthRecords),
                Times.Once);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenCalled_PassesCorrectPatientIdToRepository()
        {
            // Arrange
            const int patientId = 25;

            List<HealthRecord> healthRecords = new()
            {
                new HealthRecord
                {
                    RecordId = 1,
                    PatientId = patientId,
                    DoctorId = 3,
                    AppointmentId = 8,
                    VisitDate = DateTime.Today,
                    Diagnosis = "Test Diagnosis",
                    Prescription = "Test Prescription",
                    Notes = "Test Notes"
                }
            };

            List<HealthRecordReadDto> expectedDtos = new()
            {
                new HealthRecordReadDto
                {
                    HealthRecordId = 1,
                    PatientId = patientId,
                    DoctorId = 3,
                    AppointmentId = 8,
                    VisitDate = DateTime.Today,
                    Diagnosis = "Test Diagnosis",
                    Prescription = "Test Prescription",
                    Notes = "Test Notes"
                }
            };

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecords);

            _mapperMock
                .Setup(mapper => mapper.Map<List<HealthRecordReadDto>>(healthRecords))
                .Returns(expectedDtos);

            // Act
            List<HealthRecordReadDto> result =
                await _patientService.GetHealthRecordsAsync(patientId);

            // Assert
            Assert.Single(result);
            Assert.Equal(patientId, result[0].PatientId);

            _healthRecordRepositoryMock.Verify(
                repository => repository.GetByPatientIdAsync(
                    It.Is<int>(id => id == patientId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenCancellationTokenProvided_PassesTokenToRepository()
        {
            // Arrange
            const int patientId = 1;

            CancellationTokenSource cancellationTokenSource = new();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            List<HealthRecord> healthRecords = new()
            {
                new HealthRecord
                {
                    RecordId = 1,
                    PatientId = patientId,
                    DoctorId = 2,
                    AppointmentId = 3,
                    VisitDate = DateTime.Today,
                    Diagnosis = "Diagnosis",
                    Prescription = "Prescription",
                    Notes = "Notes"
                }
            };

            List<HealthRecordReadDto> expectedDtos = new()
            {
                new HealthRecordReadDto
                {
                    HealthRecordId = 1,
                    PatientId = patientId,
                    DoctorId = 2,
                    AppointmentId = 3,
                    VisitDate = DateTime.Today,
                    Diagnosis = "Diagnosis",
                    Prescription = "Prescription",
                    Notes = "Notes"
                }
            };

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patientId,
                    cancellationToken))
                .ReturnsAsync(healthRecords);

            _mapperMock
                .Setup(mapper => mapper.Map<List<HealthRecordReadDto>>(healthRecords))
                .Returns(expectedDtos);

            // Act
            List<HealthRecordReadDto> result =
                await _patientService.GetHealthRecordsAsync(
                    patientId,
                    cancellationToken);

            // Assert
            Assert.Single(result);

            _healthRecordRepositoryMock.Verify(
                repository => repository.GetByPatientIdAsync(
                    patientId,
                    cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenRepositoryReturnsRecords_MapperReceivesSameRecords()
        {
            // Arrange
            const int patientId = 5;

            List<HealthRecord> healthRecords = new()
            {
                new HealthRecord
                {
                    RecordId = 101,
                    PatientId = patientId,
                    DoctorId = 10,
                    AppointmentId = 50,
                    VisitDate = new DateTime(2026, 7, 1),
                    Diagnosis = "Migraine",
                    Prescription = "Pain relief medicine",
                    Notes = "Follow up after one week"
                }
            };

            List<HealthRecordReadDto> expectedDtos = new()
            {
                new HealthRecordReadDto
                {
                    HealthRecordId = 101,
                    PatientId = patientId,
                    DoctorId = 10,
                    AppointmentId = 50,
                    VisitDate = new DateTime(2026, 7, 1),
                    Diagnosis = "Migraine",
                    Prescription = "Pain relief medicine",
                    Notes = "Follow up after one week"
                }
            };

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecords);

            _mapperMock
                .Setup(mapper => mapper.Map<List<HealthRecordReadDto>>(
                    It.Is<List<HealthRecord>>(records =>
                        records.Count == 1 &&
                        records[0].RecordId == 101 &&
                        records[0].PatientId == patientId)))
                .Returns(expectedDtos);

            // Act
            List<HealthRecordReadDto> result =
                await _patientService.GetHealthRecordsAsync(patientId);

            // Assert
            Assert.Single(result);
            Assert.Equal(101, result[0].HealthRecordId);
            Assert.Equal("Migraine", result[0].Diagnosis);

            _mapperMock.Verify(
                mapper => mapper.Map<List<HealthRecordReadDto>>(
                    It.Is<List<HealthRecord>>(records =>
                        records.Count == 1 &&
                        records[0].RecordId == 101 &&
                        records[0].PatientId == patientId)),
                Times.Once);
        }
    }
}