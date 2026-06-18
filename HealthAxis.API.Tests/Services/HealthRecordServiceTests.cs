using AutoMapper;
using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using Moq;
using Xunit;

namespace HealthAxis.API.Tests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _healthRecordRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthRecordService _healthRecordService;

        public HealthRecordServiceTests()
        {
            _healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();
            _mapperMock = new Mock<IMapper>();

            _healthRecordService = new HealthRecordService(
                _healthRecordRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenRecordsExist_ReturnsMappedHealthRecords()
        {
            // Arrange
            const int patientId = 1;

            List<HealthRecord> healthRecords = new()
            {
                new HealthRecord
                {
                    RecordId = 1,
                    AppointmentId = 1,
                    PatientId = patientId,
                    DoctorId = 1,
                    VisitDate = new DateTime(2026, 6, 14),
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Take rest"
                },
                new HealthRecord
                {
                    RecordId = 2,
                    AppointmentId = 2,
                    PatientId = patientId,
                    DoctorId = 2,
                    VisitDate = new DateTime(2026, 6, 15),
                    Diagnosis = "Cough",
                    Prescription = "Cough syrup",
                    Notes = "Drink warm water"
                }
            };

            List<HealthRecordReadDto> expectedDtos = new()
            {
                new HealthRecordReadDto
                {
                    RecordId = 1,
                    AppointmentId = 1,
                    PatientId = patientId,
                    DoctorId = 1,
                    VisitDate = new DateTime(2026, 6, 14),
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Take rest"
                },
                new HealthRecordReadDto
                {
                    RecordId = 2,
                    AppointmentId = 2,
                    PatientId = patientId,
                    DoctorId = 2,
                    VisitDate = new DateTime(2026, 6, 15),
                    Diagnosis = "Cough",
                    Prescription = "Cough syrup",
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
                await _healthRecordService.GetByPatientIdAsync(patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(expectedDtos[0].RecordId, result[0].RecordId);
            Assert.Equal(expectedDtos[0].Diagnosis, result[0].Diagnosis);

            Assert.Equal(expectedDtos[1].RecordId, result[1].RecordId);
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
        public async Task GetByPatientIdAsync_WhenNoRecordsExist_ReturnsEmptyList()
        {
            // Arrange
            const int patientId = 10;

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
                await _healthRecordService.GetByPatientIdAsync(patientId);

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
        public async Task GetByPatientIdAsync_WhenCancellationTokenProvided_PassesTokenToRepository()
        {
            // Arrange
            const int patientId = 3;

            using CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource();

            CancellationToken cancellationToken =
                cancellationTokenSource.Token;

            List<HealthRecord> healthRecords = new()
            {
                new HealthRecord
                {
                    RecordId = 1,
                    AppointmentId = 1,
                    PatientId = patientId,
                    DoctorId = 1,
                    VisitDate = new DateTime(2026, 7, 1),
                    Diagnosis = "Headache",
                    Prescription = "Painkiller",
                    Notes = "Follow up if pain continues"
                }
            };

            List<HealthRecordReadDto> expectedDtos = new()
            {
                new HealthRecordReadDto
                {
                    RecordId = 1,
                    AppointmentId = 1,
                    PatientId = patientId,
                    DoctorId = 1,
                    VisitDate = new DateTime(2026, 7, 1),
                    Diagnosis = "Headache",
                    Prescription = "Painkiller",
                    Notes = "Follow up if pain continues"
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
                await _healthRecordService.GetByPatientIdAsync(
                    patientId,
                    cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(patientId, result[0].PatientId);

            _healthRecordRepositoryMock.Verify(
                repository => repository.GetByPatientIdAsync(
                    patientId,
                    cancellationToken),
                Times.Once);

            _mapperMock.Verify(
                mapper => mapper.Map<List<HealthRecordReadDto>>(healthRecords),
                Times.Once);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenRepositoryReturnsMultiplePatientRecords_MapsAllRecords()
        {
            // Arrange
            const int patientId = 5;

            List<HealthRecord> healthRecords = new()
            {
                CreateHealthRecord(1, patientId, "Diagnosis One"),
                CreateHealthRecord(2, patientId, "Diagnosis Two"),
                CreateHealthRecord(3, patientId, "Diagnosis Three")
            };

            List<HealthRecordReadDto> expectedDtos = new()
            {
                CreateHealthRecordReadDto(1, patientId, "Diagnosis One"),
                CreateHealthRecordReadDto(2, patientId, "Diagnosis Two"),
                CreateHealthRecordReadDto(3, patientId, "Diagnosis Three")
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
                await _healthRecordService.GetByPatientIdAsync(patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            Assert.All(
                result,
                record => Assert.Equal(patientId, record.PatientId));

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
        public async Task GetByPatientIdAsync_WhenRepositoryReturnsSingleRecord_MapsSingleRecord()
        {
            // Arrange
            const int patientId = 7;

            List<HealthRecord> healthRecords = new()
            {
                CreateHealthRecord(1, patientId, "Single Diagnosis")
            };

            List<HealthRecordReadDto> expectedDtos = new()
            {
                CreateHealthRecordReadDto(1, patientId, "Single Diagnosis")
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
                await _healthRecordService.GetByPatientIdAsync(patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Single Diagnosis", result[0].Diagnosis);

            _healthRecordRepositoryMock.Verify(
                repository => repository.GetByPatientIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private static HealthRecord CreateHealthRecord(
            int recordId,
            int patientId,
            string diagnosis)
        {
            return new HealthRecord
            {
                RecordId = recordId,
                AppointmentId = recordId,
                PatientId = patientId,
                DoctorId = 1,
                VisitDate = new DateTime(2026, 8, recordId),
                Diagnosis = diagnosis,
                Prescription = "Prescription",
                Notes = "Notes"
            };
        }

        private static HealthRecordReadDto CreateHealthRecordReadDto(
            int recordId,
            int patientId,
            string diagnosis)
        {
            return new HealthRecordReadDto
            {
                RecordId = recordId,
                AppointmentId = recordId,
                PatientId = patientId,
                DoctorId = 1,
                VisitDate = new DateTime(2026, 8, recordId),
                Diagnosis = diagnosis,
                Prescription = "Prescription",
                Notes = "Notes"
            };
        }
    }
}
