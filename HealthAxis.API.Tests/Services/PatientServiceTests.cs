using AutoMapper;
using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.DTOs.Patients;
using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using Moq;
using Xunit;

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
        public async Task GetHealthRecordsAsync_WhenRecordsExist_ReturnsMappedHealthRecords()
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
                await _patientService.GetHealthRecordsAsync(patientId);

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
        public async Task GetHealthRecordsAsync_WhenNoRecordsExist_ReturnsEmptyList()
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
        public async Task GetHealthRecordsAsync_WhenCancellationTokenProvided_PassesTokenToRepository()
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
                await _patientService.GetHealthRecordsAsync(
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
        public async Task GetHealthRecordsAsync_WhenRepositoryReturnsMultipleRecords_MapsAllRecords()
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
                await _patientService.GetHealthRecordsAsync(patientId);

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
        public async Task GetHealthRecordsAsync_WhenRepositoryReturnsSingleRecord_ReturnsSingleMappedRecord()
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
                await _patientService.GetHealthRecordsAsync(patientId);

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

        [Fact]
        public async Task GetAllAsync_WhenPatientsExist_ReturnsMappedPatients()
        {
            // Arrange
            List<Patient> patients = new()
            {
                new Patient
                {
                    PatientId = 1,
                    FullName = "Patient One",
                    DateOfBirth = new DateTime(2000, 1, 1),
                    Gender = Gender.Male,
                    PhoneNumber = "9876543210",
                    Email = "patient1@test.com",
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Patient
                {
                    PatientId = 2,
                    FullName = "Patient Two",
                    DateOfBirth = new DateTime(2001, 1, 1),
                    Gender = Gender.Female,
                    PhoneNumber = "9876543211",
                    Email = "patient2@test.com",
                    CreatedDate = new DateTime(2026, 1, 2)
                }
            };

            List<PatientReadDto> expectedDtos = new()
            {
                new PatientReadDto
                {
                    PatientId = 1,
                    FullName = "Patient One",
                    DateOfBirth = new DateTime(2000, 1, 1),
                    Gender = Gender.Male,
                    PhoneNumber = "9876543210",
                    Email = "patient1@test.com",
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new PatientReadDto
                {
                    PatientId = 2,
                    FullName = "Patient Two",
                    DateOfBirth = new DateTime(2001, 1, 1),
                    Gender = Gender.Female,
                    PhoneNumber = "9876543211",
                    Email = "patient2@test.com",
                    CreatedDate = new DateTime(2026, 1, 2)
                }
            };

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(mapper => mapper.Map<List<PatientReadDto>>(patients))
                .Returns(expectedDtos);

            // Act
            List<PatientReadDto> result =
                await _patientService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(expectedDtos[0].PatientId, result[0].PatientId);
            Assert.Equal(expectedDtos[1].PatientId, result[1].PatientId);

            _patientRepositoryMock.Verify(
                repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientExists_ReturnsMappedPatient()
        {
            // Arrange
            const int patientId = 1;

            Patient patient = new()
            {
                PatientId = patientId,
                FullName = "Patient One",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "patient1@test.com",
                CreatedDate = new DateTime(2026, 1, 1)
            };

            PatientReadDto expectedDto = new()
            {
                PatientId = patientId,
                FullName = "Patient One",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "patient1@test.com",
                CreatedDate = new DateTime(2026, 1, 1)
            };

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(mapper => mapper.Map<PatientReadDto>(patient))
                .Returns(expectedDto);

            // Act
            PatientReadDto? result =
                await _patientService.GetByIdAsync(patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.PatientId, result.PatientId);
            Assert.Equal(expectedDto.FullName, result.FullName);

            _patientRepositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientDoesNotExist_ReturnsNull()
        {
            // Arrange
            const int patientId = 404;

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            // Act
            PatientReadDto? result =
                await _patientService.GetByIdAsync(patientId);

            // Assert
            Assert.Null(result);

            _patientRepositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenValidDto_ReturnsCreatedPatient()
        {
            // Arrange
            PatientCreateDto createDto = new()
            {
                FullName = "New Patient",
                DateOfBirth = new DateTime(2002, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "newpatient@test.com"
            };

            Patient patientEntity = new()
            {
                FullName = createDto.FullName,
                DateOfBirth = createDto.DateOfBirth,
                Gender = createDto.Gender,
                PhoneNumber = createDto.PhoneNumber,
                Email = createDto.Email
            };

            Patient createdPatient = new()
            {
                PatientId = 10,
                FullName = createDto.FullName,
                DateOfBirth = createDto.DateOfBirth,
                Gender = createDto.Gender,
                PhoneNumber = createDto.PhoneNumber,
                Email = createDto.Email,
                CreatedDate = new DateTime(2026, 1, 1)
            };

            PatientReadDto expectedDto = new()
            {
                PatientId = 10,
                FullName = createDto.FullName,
                DateOfBirth = createDto.DateOfBirth,
                Gender = createDto.Gender,
                PhoneNumber = createDto.PhoneNumber,
                Email = createDto.Email,
                CreatedDate = new DateTime(2026, 1, 1)
            };

            _mapperMock
                .Setup(mapper => mapper.Map<Patient>(createDto))
                .Returns(patientEntity);

            _patientRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    patientEntity,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdPatient);

            _mapperMock
                .Setup(mapper => mapper.Map<PatientReadDto>(createdPatient))
                .Returns(expectedDto);

            // Act
            PatientReadDto result =
                await _patientService.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.PatientId, result.PatientId);
            Assert.Equal(expectedDto.Email, result.Email);

            _patientRepositoryMock.Verify(
                repository => repository.CreateAsync(
                    patientEntity,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenPatientExists_ReturnsDeletedPatient()
        {
            // Arrange
            const int patientId = 1;

            Patient deletedPatient = new()
            {
                PatientId = patientId,
                FullName = "Deleted Patient",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "deleted@test.com",
                CreatedDate = new DateTime(2026, 1, 1)
            };

            PatientReadDto expectedDto = new()
            {
                PatientId = patientId,
                FullName = "Deleted Patient",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "deleted@test.com",
                CreatedDate = new DateTime(2026, 1, 1)
            };

            _patientRepositoryMock
                .Setup(repository => repository.DeleteAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(deletedPatient);

            _mapperMock
                .Setup(mapper => mapper.Map<PatientReadDto>(deletedPatient))
                .Returns(expectedDto);

            // Act
            PatientReadDto? result =
                await _patientService.DeleteAsync(patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.PatientId, result.PatientId);

            _patientRepositoryMock.Verify(
                repository => repository.DeleteAsync(
                    patientId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenPatientDoesNotExist_ReturnsNull()
        {
            // Arrange
            const int patientId = 404;

            _patientRepositoryMock
                .Setup(repository => repository.DeleteAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            // Act
            PatientReadDto? result =
                await _patientService.DeleteAsync(patientId);

            // Assert
            Assert.Null(result);

            _patientRepositoryMock.Verify(
                repository => repository.DeleteAsync(
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
