using AutoMapper;
using FluentAssertions;
using HealthAxisCore_Api.DTOs.HealthRecord;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Implementations;
using HealthAxisCore_Api.Tests.Helpers;
using Moq;

namespace HealthAxisCore_Api.Tests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _healthRecordRepositoryMock;
        private readonly IMapper _mapper;
        private readonly HealthRecordService _healthRecordService;

        public HealthRecordServiceTests()
        {
            _healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();
            _mapper = MapperHelper.GetMapper();

            _healthRecordService = new HealthRecordService(
                _healthRecordRepositoryMock.Object,
                _mapper
            );
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllHealthRecords()
        {
            // Arrange
            var records = new List<HealthRecord>
            {
                new HealthRecord
                {
                    HealthRecordId = 1,
                    PatientId = 1,
                    DoctorId = 2,
                    AppointmentId = 1,
                    VisitDate = DateTime.Today,
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Drink fluids"
                },
                new HealthRecord
                {
                    HealthRecordId = 2,
                    PatientId = 2,
                    DoctorId = 3,
                    AppointmentId = 2,
                    VisitDate = DateTime.Today.AddDays(-1),
                    Diagnosis = "Cold",
                    Prescription = "Antihistamine",
                    Notes = "Rest"
                }
            };

            _healthRecordRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(records);

            // Act
            var result = await _healthRecordService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().HealthRecordId.Should().Be(1);
            result.First().Diagnosis.Should().Be("Fever");
        }

        [Fact]
        public async Task GetByIdAsync_WhenHealthRecordExists_ShouldReturnHealthRecord()
        {
            // Arrange
            var record = new HealthRecord
            {
                HealthRecordId = 1,
                PatientId = 1,
                DoctorId = 2,
                AppointmentId = 1,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Drink fluids"
            };

            _healthRecordRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(record);

            // Act
            var result = await _healthRecordService.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.HealthRecordId.Should().Be(1);
            result.PatientId.Should().Be(1);
            result.DoctorId.Should().Be(2);
            result.AppointmentId.Should().Be(1);
            result.Diagnosis.Should().Be("Fever");
            result.Prescription.Should().Be("Paracetamol");
        }

        [Fact]
        public async Task GetByIdAsync_WhenHealthRecordDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _healthRecordRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((HealthRecord?)null);

            // Act
            var act = async () => await _healthRecordService.GetByIdAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Health record not found");
        }

        [Fact]
        public async Task CreateAsync_WhenValidHealthRecord_ShouldCreateHealthRecord()
        {
            // Arrange
            var dto = new CreateHealthRecordDTO
            {
                PatientId = 1,
                DoctorId = 2,
                AppointmentId = 1,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Drink fluids"
            };

            _healthRecordRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<HealthRecord>()))
                .Returns(Task.CompletedTask)
                .Callback<HealthRecord>(record =>
                {
                    record.HealthRecordId = 1;
                });

            // Act
            var result = await _healthRecordService.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.HealthRecordId.Should().Be(1);
            result.PatientId.Should().Be(dto.PatientId);
            result.DoctorId.Should().Be(dto.DoctorId);
            result.AppointmentId.Should().Be(dto.AppointmentId);
            result.Diagnosis.Should().Be(dto.Diagnosis);
            result.Prescription.Should().Be(dto.Prescription);
            result.Notes.Should().Be(dto.Notes);

            _healthRecordRepositoryMock.Verify(
                repo => repo.AddAsync(It.Is<HealthRecord>(record =>
                    record.PatientId == dto.PatientId &&
                    record.DoctorId == dto.DoctorId &&
                    record.AppointmentId == dto.AppointmentId &&
                    record.Diagnosis == dto.Diagnosis &&
                    record.Prescription == dto.Prescription &&
                    record.Notes == dto.Notes
                )),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteAsync_WhenHealthRecordExists_ShouldDeleteHealthRecordAndReturnTrue()
        {
            // Arrange
            _healthRecordRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(true);

            _healthRecordRepositoryMock
                .Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _healthRecordService.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();

            _healthRecordRepositoryMock.Verify(
                repo => repo.DeleteAsync(1),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteAsync_WhenHealthRecordDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _healthRecordRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(false);

            // Act
            var act = async () => await _healthRecordService.DeleteAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Health record not found");

            _healthRecordRepositoryMock.Verify(
                repo => repo.DeleteAsync(It.IsAny<int>()),
                Times.Never
            );
        }

        [Fact]
        public async Task GetByPatientAsync_WhenRecordsExist_ShouldReturnHealthRecords()
        {
            // Arrange
            var records = new List<HealthRecord>
            {
                new HealthRecord
                {
                    HealthRecordId = 1,
                    PatientId = 1,
                    DoctorId = 2,
                    AppointmentId = 1,
                    VisitDate = DateTime.Today,
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Drink fluids"
                }
            };

            _healthRecordRepositoryMock
                .Setup(repo => repo.GetByPatient(1))
                .ReturnsAsync(records);

            // Act
            var result = await _healthRecordService.GetByPatientAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().PatientId.Should().Be(1);
            result.First().Diagnosis.Should().Be("Fever");
            result.First().Prescription.Should().Be("Paracetamol");
        }

        [Fact]
        public async Task GetByPatientAsync_WhenNoRecordsExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _healthRecordRepositoryMock
                .Setup(repo => repo.GetByPatient(1))
                .ReturnsAsync(new List<HealthRecord>());

            // Act
            var act = async () => await _healthRecordService.GetByPatientAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("No health records found for this patient");
        }

        [Fact]
        public async Task GetByPatientAsync_WhenRepositoryReturnsNull_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _healthRecordRepositoryMock
                .Setup(repo => repo.GetByPatient(1))
                .ReturnsAsync((IEnumerable<HealthRecord>?)null);

            // Act
            var act = async () => await _healthRecordService.GetByPatientAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("No health records found for this patient");
        }
    }
}