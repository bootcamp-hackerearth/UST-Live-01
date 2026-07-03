using AutoMapper;
using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using Moq;

namespace HealthAxis.API.Tests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _healthRecordRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthRecordService _healthRecordService;

        public HealthRecordServiceTests()
        {
            _healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();

            _healthRecordService = new HealthRecordService(
                _healthRecordRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenRecordsExist_ReturnsMappedHealthRecordsWithPatientAndDoctorNames()
        {
            // Arrange
            const int patientId = 1;

            List<HealthRecord> healthRecords = new()
            {
                new HealthRecord
                {
                    RecordId = 1,
                    PatientId = patientId,
                    DoctorId = 10,
                    AppointmentId = 100,
                    VisitDate = new DateTime(2026, 6, 14),
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Rest advised"
                }
            };

            List<Doctor> doctors = new()
            {
                new Doctor
                {
                    DoctorId = 10,
                    FullName = "Dr. Test",
                    Specialisation = Specialisation.Cardiology
                }
            };

            Patient patient = new()
            {
                PatientId = patientId,
                FullName = "Patient Test"
            };

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecords);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            // Act
            List<HealthRecordReadDto> result =
                await _healthRecordService.GetByPatientIdAsync(patientId);

            // Assert
            Assert.Single(result);

            HealthRecordReadDto record = result[0];

            Assert.Equal(1, record.HealthRecordId);
            Assert.Equal(patientId, record.PatientId);
            Assert.Equal("Patient Test", record.PatientName);
            Assert.Equal(10, record.DoctorId);
            Assert.Equal("Dr. Test", record.DoctorName);
            Assert.Equal(Specialisation.Cardiology, record.Specialisation);
            Assert.Equal(100, record.AppointmentId);
            Assert.Equal(new DateTime(2026, 6, 14), record.VisitDate);
            Assert.Equal("Fever", record.Diagnosis);
            Assert.Equal("Paracetamol", record.Prescription);
            Assert.Equal("Rest advised", record.Notes);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenMultipleRecordsExist_ReturnsOnlyRecordsForRequestedPatient()
        {
            // Arrange
            const int patientId = 1;

            List<HealthRecord> healthRecords = new()
            {
                new HealthRecord
                {
                    RecordId = 1,
                    PatientId = patientId,
                    DoctorId = 10,
                    AppointmentId = 100,
                    VisitDate = new DateTime(2026, 6, 14),
                    Diagnosis = "Fever",
                    Prescription = "Medicine A",
                    Notes = "Notes A"
                },
                new HealthRecord
                {
                    RecordId = 2,
                    PatientId = 2,
                    DoctorId = 11,
                    AppointmentId = 101,
                    VisitDate = new DateTime(2026, 6, 15),
                    Diagnosis = "Cold",
                    Prescription = "Medicine B",
                    Notes = "Notes B"
                },
                new HealthRecord
                {
                    RecordId = 3,
                    PatientId = patientId,
                    DoctorId = 10,
                    AppointmentId = 102,
                    VisitDate = new DateTime(2026, 6, 16),
                    Diagnosis = "Headache",
                    Prescription = "Medicine C",
                    Notes = "Notes C"
                }
            };

            List<Doctor> doctors = new()
            {
                new Doctor
                {
                    DoctorId = 10,
                    FullName = "Dr. One",
                    Specialisation = Specialisation.GeneralMedicine
                },
                new Doctor
                {
                    DoctorId = 11,
                    FullName = "Dr. Two",
                    Specialisation = Specialisation.Cardiology
                }
            };

            Patient patient = new()
            {
                PatientId = patientId,
                FullName = "Patient One"
            };

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecords);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            // Act
            List<HealthRecordReadDto> result =
                await _healthRecordService.GetByPatientIdAsync(patientId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, record => Assert.Equal(patientId, record.PatientId));

            Assert.Contains(result, record => record.HealthRecordId == 1);
            Assert.Contains(result, record => record.HealthRecordId == 3);
            Assert.DoesNotContain(result, record => record.HealthRecordId == 2);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenRecordsExist_ReturnsRecordsOrderedByVisitDateDescending()
        {
            // Arrange
            const int patientId = 1;

            DateTime oldestDate = new(2026, 6, 10);
            DateTime latestDate = new(2026, 6, 20);
            DateTime middleDate = new(2026, 6, 15);

            List<HealthRecord> healthRecords = new()
            {
                new HealthRecord
                {
                    RecordId = 1,
                    PatientId = patientId,
                    DoctorId = 10,
                    AppointmentId = 100,
                    VisitDate = oldestDate,
                    Diagnosis = "Old Diagnosis",
                    Prescription = "Old Prescription",
                    Notes = "Old Notes"
                },
                new HealthRecord
                {
                    RecordId = 2,
                    PatientId = patientId,
                    DoctorId = 10,
                    AppointmentId = 101,
                    VisitDate = latestDate,
                    Diagnosis = "Latest Diagnosis",
                    Prescription = "Latest Prescription",
                    Notes = "Latest Notes"
                },
                new HealthRecord
                {
                    RecordId = 3,
                    PatientId = patientId,
                    DoctorId = 10,
                    AppointmentId = 102,
                    VisitDate = middleDate,
                    Diagnosis = "Middle Diagnosis",
                    Prescription = "Middle Prescription",
                    Notes = "Middle Notes"
                }
            };

            List<Doctor> doctors = new()
            {
                new Doctor
                {
                    DoctorId = 10,
                    FullName = "Dr. Test",
                    Specialisation = Specialisation.GeneralMedicine
                }
            };

            Patient patient = new()
            {
                PatientId = patientId,
                FullName = "Patient Test"
            };

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecords);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            // Act
            List<HealthRecordReadDto> result =
                await _healthRecordService.GetByPatientIdAsync(patientId);

            // Assert
            Assert.Equal(3, result.Count);

            Assert.Equal(2, result[0].HealthRecordId);
            Assert.Equal(latestDate, result[0].VisitDate);

            Assert.Equal(3, result[1].HealthRecordId);
            Assert.Equal(middleDate, result[1].VisitDate);

            Assert.Equal(1, result[2].HealthRecordId);
            Assert.Equal(oldestDate, result[2].VisitDate);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenPatientDoesNotExist_ReturnsUnknownPatientName()
        {
            // Arrange
            const int patientId = 1;

            List<HealthRecord> healthRecords = new()
            {
                new HealthRecord
                {
                    RecordId = 1,
                    PatientId = patientId,
                    DoctorId = 10,
                    AppointmentId = 100,
                    VisitDate = new DateTime(2026, 6, 14),
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Rest"
                }
            };

            List<Doctor> doctors = new()
            {
                new Doctor
                {
                    DoctorId = 10,
                    FullName = "Dr. Test",
                    Specialisation = Specialisation.Cardiology
                }
            };

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecords);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            // Act
            List<HealthRecordReadDto> result =
                await _healthRecordService.GetByPatientIdAsync(patientId);

            // Assert
            Assert.Single(result);
            Assert.Equal("Unknown Patient", result[0].PatientName);
            Assert.Equal("Dr. Test", result[0].DoctorName);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenDoctorDoesNotExist_ReturnsUnknownDoctorNameAndDefaultSpecialisation()
        {
            // Arrange
            const int patientId = 1;

            List<HealthRecord> healthRecords = new()
            {
                new HealthRecord
                {
                    RecordId = 1,
                    PatientId = patientId,
                    DoctorId = 99,
                    AppointmentId = 100,
                    VisitDate = new DateTime(2026, 6, 14),
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Rest"
                }
            };

            Patient patient = new()
            {
                PatientId = patientId,
                FullName = "Patient Test"
            };

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecords);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Doctor>());

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            // Act
            List<HealthRecordReadDto> result =
                await _healthRecordService.GetByPatientIdAsync(patientId);

            // Assert
            Assert.Single(result);
            Assert.Equal("Patient Test", result[0].PatientName);
            Assert.Equal("Unknown Doctor", result[0].DoctorName);
            Assert.Equal(default, result[0].Specialisation);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenNoRecordsMatchPatient_ReturnsEmptyList()
        {
            // Arrange
            const int patientId = 1;

            List<HealthRecord> healthRecords = new()
            {
                new HealthRecord
                {
                    RecordId = 1,
                    PatientId = 2,
                    DoctorId = 10,
                    AppointmentId = 100,
                    VisitDate = new DateTime(2026, 6, 14),
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Rest"
                }
            };

            List<Doctor> doctors = new()
            {
                new Doctor
                {
                    DoctorId = 10,
                    FullName = "Dr. Test",
                    Specialisation = Specialisation.Cardiology
                }
            };

            Patient patient = new()
            {
                PatientId = patientId,
                FullName = "Patient Test"
            };

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecords);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            // Act
            List<HealthRecordReadDto> result =
                await _healthRecordService.GetByPatientIdAsync(patientId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenNoHealthRecordsExist_ReturnsEmptyList()
        {
            // Arrange
            const int patientId = 1;

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<HealthRecord>());

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Doctor>());

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Patient
                {
                    PatientId = patientId,
                    FullName = "Patient Test"
                });

            // Act
            List<HealthRecordReadDto> result =
                await _healthRecordService.GetByPatientIdAsync(patientId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenCalled_CallsRepositoriesOnce()
        {
            // Arrange
            const int patientId = 1;

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<HealthRecord>());

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Doctor>());

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Patient
                {
                    PatientId = patientId,
                    FullName = "Patient Test"
                });

            // Act
            await _healthRecordService.GetByPatientIdAsync(patientId);

            // Assert
            _healthRecordRepositoryMock.Verify(
                repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _doctorRepositoryMock.Verify(
                repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _patientRepositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenCancellationTokenProvided_PassesCancellationTokenToRepositories()
        {
            // Arrange
            const int patientId = 1;

            CancellationTokenSource cancellationTokenSource = new();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync(cancellationToken))
                .ReturnsAsync(new List<HealthRecord>());

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(cancellationToken))
                .ReturnsAsync(new List<Doctor>());

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patientId,
                    cancellationToken))
                .ReturnsAsync(new Patient
                {
                    PatientId = patientId,
                    FullName = "Patient Test"
                });

            // Act
            List<HealthRecordReadDto> result =
                await _healthRecordService.GetByPatientIdAsync(
                    patientId,
                    cancellationToken);

            // Assert
            Assert.Empty(result);

            _healthRecordRepositoryMock.Verify(
                repository => repository.GetAllAsync(cancellationToken),
                Times.Once);

            _doctorRepositoryMock.Verify(
                repository => repository.GetAllAsync(cancellationToken),
                Times.Once);

            _patientRepositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    patientId,
                    cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenDoctorHasSpecialisation_ReturnsSameSpecialisation()
        {
            // Arrange
            const int patientId = 1;

            List<HealthRecord> healthRecords = new()
            {
                new HealthRecord
                {
                    RecordId = 1,
                    PatientId = patientId,
                    DoctorId = 10,
                    AppointmentId = 100,
                    VisitDate = new DateTime(2026, 6, 14),
                    Diagnosis = "Heart Checkup",
                    Prescription = "Medicine",
                    Notes = "Follow up"
                }
            };

            List<Doctor> doctors = new()
            {
                new Doctor
                {
                    DoctorId = 10,
                    FullName = "Dr. Cardio",
                    Specialisation = Specialisation.Cardiology
                }
            };

            Patient patient = new()
            {
                PatientId = patientId,
                FullName = "Patient Test"
            };

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecords);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            // Act
            List<HealthRecordReadDto> result =
                await _healthRecordService.GetByPatientIdAsync(patientId);

            // Assert
            Assert.Single(result);
            Assert.Equal(Specialisation.Cardiology, result[0].Specialisation);
        }
    }
}
