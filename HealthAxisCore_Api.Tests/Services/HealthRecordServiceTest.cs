using AutoMapper;
using FluentAssertions;
using HealthAxis.Shared.DTOs.HealthRecord;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Implementations;
using Moq;
using Xunit;

namespace HealthAxisCore_Api.Tests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _healthRecordRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new HealthRecordService(
                _healthRecordRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        // ------------------------------------------------------------
        // GetAllAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetAllAsync_WhenRecordsExist_ShouldReturnMappedRecords()
        {
            var records = GetSampleHealthRecords();

            var mappedRecords = records
                .Select(ToHealthRecordResponseDto)
                .ToList();

            _healthRecordRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(records);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<HealthRecordResponseDto>>(records))
                .Returns(mappedRecords);

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(records.Count);
            result.First().HealthRecordId.Should().Be(1);

            _healthRecordRepositoryMock.Verify(
                repo => repo.GetAllAsync(),
                Times.Once
            );

            _mapperMock.Verify(
                mapper => mapper.Map<IEnumerable<HealthRecordResponseDto>>(records),
                Times.Once
            );
        }

        [Fact]
        public async Task GetAllAsync_WhenNoRecordsExist_ShouldReturnEmptyMappedList()
        {
            var records = new List<HealthRecord>();
            var mappedRecords = new List<HealthRecordResponseDto>();

            _healthRecordRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(records);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<HealthRecordResponseDto>>(records))
                .Returns(mappedRecords);

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _healthRecordRepositoryMock.Verify(
                repo => repo.GetAllAsync(),
                Times.Once
            );
        }

        // ------------------------------------------------------------
        // GetByIdAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetByIdAsync_WhenRecordExists_ShouldReturnMappedRecord()
        {
            var record = new HealthRecord
            {
                HealthRecordId = 1,
                PatientId = 10,
                DoctorId = 20,
                AppointmentId = 30,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest",
                CreatedDate = DateTime.Now
            };

            var mappedRecord = ToHealthRecordResponseDto(record);

            _healthRecordRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(record);

            _mapperMock
                .Setup(mapper => mapper.Map<HealthRecordResponseDto>(record))
                .Returns(mappedRecord);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.HealthRecordId.Should().Be(1);
            result.PatientId.Should().Be(10);
            result.DoctorId.Should().Be(20);
            result.AppointmentId.Should().Be(30);
            result.Diagnosis.Should().Be("Fever");
            result.Prescription.Should().Be("Paracetamol");

            _healthRecordRepositoryMock.Verify(
                repo => repo.GetByIdAsync(1),
                Times.Once
            );

            _mapperMock.Verify(
                mapper => mapper.Map<HealthRecordResponseDto>(record),
                Times.Once
            );
        }

        [Fact]
        public async Task GetByIdAsync_WhenRecordDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            _healthRecordRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((HealthRecord?)null);

            var act = async () => await _service.GetByIdAsync(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Health record not found");

            _mapperMock.Verify(
                mapper => mapper.Map<HealthRecordResponseDto>(It.IsAny<HealthRecord>()),
                Times.Never
            );
        }

        // ------------------------------------------------------------
        // CreateAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task CreateAsync_WhenHealthRecordAlreadyExistsForAppointment_ShouldThrowBusinessRuleException()
        {
            var dto = new CreateHealthRecordDto
            {
                PatientId = 10,
                DoctorId = 20,
                AppointmentId = 30,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Existing appointment record"
            };

            _healthRecordRepositoryMock
                .Setup(repo => repo.ExistsByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(true);

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Health record already exists for this appointment.");

            _healthRecordRepositoryMock.Verify(
                repo => repo.ExistsByAppointmentIdAsync(dto.AppointmentId),
                Times.Once
            );

            _healthRecordRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<HealthRecord>()),
                Times.Never
            );

            _mapperMock.Verify(
                mapper => mapper.Map<HealthRecord>(It.IsAny<CreateHealthRecordDto>()),
                Times.Never
            );
        }

        [Fact]
        public async Task CreateAsync_WhenValidRequest_ShouldCreateRecordAndReturnMappedResponse()
        {
            var dto = new CreateHealthRecordDto
            {
                PatientId = 10,
                DoctorId = 20,
                AppointmentId = 30,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest"
            };

            var record = new HealthRecord
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentId = dto.AppointmentId,
                VisitDate = dto.VisitDate,
                Diagnosis = dto.Diagnosis,
                Prescription = dto.Prescription,
                Notes = dto.Notes
            };

            var response = new HealthRecordResponseDto
            {
                HealthRecordId = 100,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentId = dto.AppointmentId,
                VisitDate = dto.VisitDate,
                Diagnosis = dto.Diagnosis,
                Prescription = dto.Prescription,
                Notes = dto.Notes
            };

            _healthRecordRepositoryMock
                .Setup(repo => repo.ExistsByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(mapper => mapper.Map<HealthRecord>(dto))
                .Returns(record);

            _healthRecordRepositoryMock
                .Setup(repo => repo.AddAsync(record))
                .Callback<HealthRecord>(healthRecord =>
                {
                    healthRecord.HealthRecordId = 100;
                })
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<HealthRecordResponseDto>(record))
                .Returns(response);

            var result = await _service.CreateAsync(dto);

            result.Should().NotBeNull();
            result.HealthRecordId.Should().Be(100);
            result.PatientId.Should().Be(dto.PatientId);
            result.DoctorId.Should().Be(dto.DoctorId);
            result.AppointmentId.Should().Be(dto.AppointmentId);
            result.VisitDate.Should().Be(dto.VisitDate);
            result.Diagnosis.Should().Be(dto.Diagnosis);
            result.Prescription.Should().Be(dto.Prescription);
            result.Notes.Should().Be(dto.Notes);

            record.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

            _healthRecordRepositoryMock.Verify(
                repo => repo.ExistsByAppointmentIdAsync(dto.AppointmentId),
                Times.Once
            );

            _healthRecordRepositoryMock.Verify(
                repo => repo.AddAsync(record),
                Times.Once
            );

            _mapperMock.Verify(
                mapper => mapper.Map<HealthRecord>(dto),
                Times.Once
            );

            _mapperMock.Verify(
                mapper => mapper.Map<HealthRecordResponseDto>(record),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorIdIsNull_ShouldCreateRecordSuccessfully()
        {
            var dto = new CreateHealthRecordDto
            {
                PatientId = 10,
                DoctorId = null,
                AppointmentId = 30,
                VisitDate = DateTime.Today,
                Diagnosis = "Cold",
                Prescription = "Steam inhalation",
                Notes = null
            };

            var record = new HealthRecord
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentId = dto.AppointmentId,
                VisitDate = dto.VisitDate,
                Diagnosis = dto.Diagnosis,
                Prescription = dto.Prescription,
                Notes = dto.Notes
            };

            var response = new HealthRecordResponseDto
            {
                HealthRecordId = 101,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentId = dto.AppointmentId,
                VisitDate = dto.VisitDate,
                Diagnosis = dto.Diagnosis,
                Prescription = dto.Prescription,
                Notes = dto.Notes
            };

            _healthRecordRepositoryMock
                .Setup(repo => repo.ExistsByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(mapper => mapper.Map<HealthRecord>(dto))
                .Returns(record);

            _healthRecordRepositoryMock
                .Setup(repo => repo.AddAsync(record))
                .Callback<HealthRecord>(healthRecord =>
                {
                    healthRecord.HealthRecordId = 101;
                })
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<HealthRecordResponseDto>(record))
                .Returns(response);

            var result = await _service.CreateAsync(dto);

            result.Should().NotBeNull();
            result.HealthRecordId.Should().Be(101);
            result.DoctorId.Should().BeNull();
            result.Notes.Should().BeNull();

            _healthRecordRepositoryMock.Verify(
                repo => repo.AddAsync(record),
                Times.Once
            );
        }

        // ------------------------------------------------------------
        // DeleteAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task DeleteAsync_WhenRecordExists_ShouldDeleteRecordAndReturnTrue()
        {
            _healthRecordRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(true);

            _healthRecordRepositoryMock
                .Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(1);

            result.Should().BeTrue();

            _healthRecordRepositoryMock.Verify(
                repo => repo.Exists(1),
                Times.Once
            );

            _healthRecordRepositoryMock.Verify(
                repo => repo.DeleteAsync(1),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteAsync_WhenRecordDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            _healthRecordRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(false);

            var act = async () => await _service.DeleteAsync(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Health record not found");

            _healthRecordRepositoryMock.Verify(
                repo => repo.Exists(1),
                Times.Once
            );

            _healthRecordRepositoryMock.Verify(
                repo => repo.DeleteAsync(It.IsAny<int>()),
                Times.Never
            );
        }

        // ------------------------------------------------------------
        // GetByPatientAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetByPatientAsync_WhenRecordsExist_ShouldReturnMappedRecords()
        {
            var patientId = 10;

            var records = new List<HealthRecord>
            {
                new HealthRecord
                {
                    HealthRecordId = 1,
                    PatientId = patientId,
                    DoctorId = 20,
                    AppointmentId = 30,
                    VisitDate = DateTime.Today,
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Take rest",
                    CreatedDate = DateTime.Now
                },
                new HealthRecord
                {
                    HealthRecordId = 2,
                    PatientId = patientId,
                    DoctorId = 21,
                    AppointmentId = 31,
                    VisitDate = DateTime.Today.AddDays(-5),
                    Diagnosis = "Cold",
                    Prescription = "Cough syrup",
                    Notes = "Drink warm water",
                    CreatedDate = DateTime.Now
                }
            };

            var mappedRecords = records
                .Select(ToHealthRecordResponseDto)
                .ToList();

            _healthRecordRepositoryMock
                .Setup(repo => repo.GetByPatient(patientId))
                .ReturnsAsync(records);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<HealthRecordResponseDto>>(records))
                .Returns(mappedRecords);

            var result = await _service.GetByPatientAsync(patientId);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().OnlyContain(record => record.PatientId == patientId);

            _healthRecordRepositoryMock.Verify(
                repo => repo.GetByPatient(patientId),
                Times.Once
            );

            _mapperMock.Verify(
                mapper => mapper.Map<IEnumerable<HealthRecordResponseDto>>(records),
                Times.Once
            );
        }

        [Fact]
        public async Task GetByPatientAsync_WhenRepositoryReturnsEmptyList_ShouldThrowEntityNotFoundException()
        {
            var patientId = 10;

            _healthRecordRepositoryMock
                .Setup(repo => repo.GetByPatient(patientId))
                .ReturnsAsync(new List<HealthRecord>());

            var act = async () => await _service.GetByPatientAsync(patientId);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("No health records found for this patient");

            _mapperMock.Verify(
                mapper => mapper.Map<IEnumerable<HealthRecordResponseDto>>(It.IsAny<IEnumerable<HealthRecord>>()),
                Times.Never
            );
        }

        [Fact]
        public async Task GetByPatientAsync_WhenRepositoryReturnsNull_ShouldThrowEntityNotFoundException()
        {
            var patientId = 10;

            _healthRecordRepositoryMock
                .Setup(repo => repo.GetByPatient(patientId))
                .ReturnsAsync((IEnumerable<HealthRecord>?)null);

            var act = async () => await _service.GetByPatientAsync(patientId);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("No health records found for this patient");

            _mapperMock.Verify(
                mapper => mapper.Map<IEnumerable<HealthRecordResponseDto>>(It.IsAny<IEnumerable<HealthRecord>>()),
                Times.Never
            );
        }

        // ------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------

        private static List<HealthRecord> GetSampleHealthRecords()
        {
            return new List<HealthRecord>
            {
                new HealthRecord
                {
                    HealthRecordId = 1,
                    PatientId = 10,
                    DoctorId = 20,
                    AppointmentId = 30,
                    VisitDate = DateTime.Today,
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Take rest",
                    CreatedDate = DateTime.Now
                },
                new HealthRecord
                {
                    HealthRecordId = 2,
                    PatientId = 11,
                    DoctorId = 21,
                    AppointmentId = 31,
                    VisitDate = DateTime.Today.AddDays(-2),
                    Diagnosis = "Cold",
                    Prescription = "Cough syrup",
                    Notes = "Drink warm water",
                    CreatedDate = DateTime.Now
                }
            };
        }

        private static HealthRecordResponseDto ToHealthRecordResponseDto(HealthRecord record)
        {
            return new HealthRecordResponseDto
            {
                HealthRecordId = record.HealthRecordId,
                PatientId = record.PatientId,
                DoctorId = record.DoctorId,
                AppointmentId = record.AppointmentId,
                VisitDate = record.VisitDate,
                Diagnosis = record.Diagnosis,
                Prescription = record.Prescription,
                Notes = record.Notes
            };
        }
    }
}

