using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Impl;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using Moq;
using Xunit;

namespace HealthApp.Api.Tests.Services;

public class HealthRecordServiceTests
{
    private readonly Mock<IHealthRecordRepository> _healthRepository = new();
    private readonly Mock<IAppointmentRepository> _appointmentRepository = new();
    private readonly Mock<IPatientRepository> _patientRepository = new();
    private readonly Mock<IDoctorRepository> _doctorRepository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly HealthRecordService _service;

    public HealthRecordServiceTests()
    {
        _service = new HealthRecordService(
            _healthRepository.Object,
            _appointmentRepository.Object,
            _patientRepository.Object,
            _doctorRepository.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedRecords()
    {
        var records = new List<HealthRecord>
        {
            CreateRecord(1, 1, 2),
            CreateRecord(2, 3, 4)
        };
        var dtos = new List<HealthRecordDto>
        {
            new() { RecordId = 1 },
            new() { RecordId = 2 }
        };

        _healthRepository
            .Setup(repository => repository.GetHealthRecordsAsync())
            .ReturnsAsync(records);
        _patientRepository
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<int>()))
            .ReturnsAsync(new Patient());
        _doctorRepository
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<int>()))
            .ReturnsAsync(new Doctor());
        _mapper
            .Setup(mapper => mapper.Map<IEnumerable<HealthRecordDto>>(
                records))
            .Returns(dtos);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count());
        _patientRepository.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Exactly(2));
        _doctorRepository.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task GetAllAsync_WhenNoRecords_ShouldReturnEmpty()
    {
        var records = new List<HealthRecord>();
        _healthRepository
            .Setup(repository => repository.GetHealthRecordsAsync())
            .ReturnsAsync(records);
        _mapper
            .Setup(mapper => mapper.Map<IEnumerable<HealthRecordDto>>(
                records))
            .Returns([]);

        var result = await _service.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_WhenNavigationAlreadyLoaded_ShouldNotReload()
    {
        var record = CreateRecord(1, 1, 2);
        record.Patient = new Patient { PatientId = 1 };
        record.Doctor = new Doctor { DoctorId = 2 };
        record.AppointmentId = 10;
        record.Appointment = new Appointment { AppointmentId = 10 };
        var records = new List<HealthRecord> { record };

        _healthRepository
            .Setup(repository => repository.GetHealthRecordsAsync())
            .ReturnsAsync(records);
        _mapper
            .Setup(mapper => mapper.Map<IEnumerable<HealthRecordDto>>(
                records))
            .Returns([new HealthRecordDto { RecordId = 1 }]);

        await _service.GetAllAsync();

        _patientRepository.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Never);
        _doctorRepository.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Never);
        _appointmentRepository.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByIdAsync_WhenIdIsInvalid_ShouldThrow(int id)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.GetByIdAsync(id));
    }

    [Fact]
    public async Task GetByIdAsync_WhenRecordDoesNotExist_ShouldThrow()
    {
        _healthRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync((HealthRecord?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.GetByIdAsync(1));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldLoadNavigationAndReturnMappedRecord()
    {
        var record = CreateRecord(1, 1, 2);
        record.AppointmentId = 10;
        var dto = new HealthRecordDto { RecordId = 1 };

        _healthRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(record);
        _patientRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(new Patient { PatientId = 1 });
        _doctorRepository
            .Setup(repository => repository.GetByIdAsync(2))
            .ReturnsAsync(new Doctor { DoctorId = 2 });
        _appointmentRepository
            .Setup(repository => repository.GetByIdAsync(10))
            .ReturnsAsync(new Appointment { AppointmentId = 10 });
        _mapper
            .Setup(mapper => mapper.Map<HealthRecordDto>(record))
            .Returns(dto);

        var result = await _service.GetByIdAsync(1);

        Assert.Equal(1, result.RecordId);
        Assert.NotNull(record.Patient);
        Assert.NotNull(record.Doctor);
        Assert.NotNull(record.Appointment);
    }

    [Fact]
    public async Task AddAsync_WhenDtoIsNull_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.AddAsync(null!));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task AddAsync_WhenPatientIdIsInvalid_ShouldThrow(int patientId)
    {
        var dto = CreateDto();
        dto.PatientId = patientId;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.AddAsync(dto));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task AddAsync_WhenDoctorIdIsInvalid_ShouldThrow(int doctorId)
    {
        var dto = CreateDto();
        dto.DoctorId = doctorId;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.AddAsync(dto));
    }

    [Fact]
    public async Task AddAsync_WhenVisitDateIsMissing_ShouldThrow()
    {
        var dto = CreateDto();
        dto.VisitDate = default;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.AddAsync(dto));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AddAsync_WhenDiagnosisIsMissing_ShouldThrow(
        string? diagnosis)
    {
        var dto = CreateDto();
        dto.Diagnosis = diagnosis!;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.AddAsync(dto));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AddAsync_WhenPrescriptionIsMissing_ShouldThrow(
        string? prescription)
    {
        var dto = CreateDto();
        dto.Prescription = prescription!;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.AddAsync(dto));
    }

    [Fact]
    public async Task AddAsync_WhenPatientDoesNotExist_ShouldThrow()
    {
        var dto = CreateDto();
        _patientRepository
            .Setup(repository => repository.GetByIdAsync(dto.PatientId))
            .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.AddAsync(dto));

        _doctorRepository.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task AddAsync_WhenDoctorDoesNotExist_ShouldThrow()
    {
        var dto = CreateDto();
        _patientRepository
            .Setup(repository => repository.GetByIdAsync(dto.PatientId))
            .ReturnsAsync(new Patient());
        _doctorRepository
            .Setup(repository => repository.GetByIdAsync(dto.DoctorId))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.AddAsync(dto));
    }

    [Fact]
    public async Task AddAsync_WhenAppointmentDoesNotExist_ShouldThrow()
    {
        var dto = CreateDto(10);
        SetupPatientAndDoctor(dto);
        _appointmentRepository
            .Setup(repository => repository.GetByIdAsync(10))
            .ReturnsAsync((Appointment?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.AddAsync(dto));
    }

    [Fact]
    public async Task AddAsync_WhenAppointmentPatientDoesNotMatch_ShouldThrow()
    {
        var dto = CreateDto(10);
        SetupPatientAndDoctor(dto);
        _appointmentRepository
            .Setup(repository => repository.GetByIdAsync(10))
            .ReturnsAsync(CreateAppointment(
                10,
                patientId: 99,
                doctorId: dto.DoctorId,
                AppointmentStatus.Confirmed));

        var exception = await Assert.ThrowsAsync<
            BusinessRuleViolationException>(
            () => _service.AddAsync(dto));

        Assert.Equal(
            "Appointment does not belong to the selected patient.",
            exception.Message);
    }

    [Fact]
    public async Task AddAsync_WhenAppointmentDoctorDoesNotMatch_ShouldThrow()
    {
        var dto = CreateDto(10);
        SetupPatientAndDoctor(dto);
        _appointmentRepository
            .Setup(repository => repository.GetByIdAsync(10))
            .ReturnsAsync(CreateAppointment(
                10,
                dto.PatientId,
                doctorId: 99,
                AppointmentStatus.Confirmed));

        var exception = await Assert.ThrowsAsync<
            BusinessRuleViolationException>(
            () => _service.AddAsync(dto));

        Assert.Equal(
            "Appointment does not belong to the selected doctor.",
            exception.Message);
    }

    [Theory]
    [InlineData(AppointmentStatus.Pending)]
    [InlineData(AppointmentStatus.Completed)]
    [InlineData(AppointmentStatus.Cancelled)]
    public async Task AddAsync_WhenAppointmentIsNotConfirmed_ShouldThrow(
        AppointmentStatus status)
    {
        var dto = CreateDto(10);
        SetupPatientAndDoctor(dto);
        _appointmentRepository
            .Setup(repository => repository.GetByIdAsync(10))
            .ReturnsAsync(CreateAppointment(
                10,
                dto.PatientId,
                dto.DoctorId,
                status));

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.AddAsync(dto));
    }

    [Fact]
    public async Task AddAsync_WhenRecordAlreadyExistsForAppointment_ShouldThrow()
    {
        var dto = CreateDto(10);
        SetupValidConfirmedAppointment(dto);
        _healthRepository
            .Setup(repository => repository.GetHealthRecordsAsync(
                null,
                10))
            .ReturnsAsync([new HealthRecord { RecordId = 1 }]);

        await Assert.ThrowsAsync<DuplicateEntityException>(
            () => _service.AddAsync(dto));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByPatientIdAsync_WhenPatientIdIsInvalid_ShouldThrow(
        int patientId)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.GetByPatientIdAsync(patientId));
    }

    [Fact]
    public async Task GetByPatientIdAsync_ShouldLoadAndMapRecords()
    {
        var record = CreateRecord(1, 1, 2);
        var records = new List<HealthRecord> { record };
        _healthRepository
            .Setup(repository => repository.GetHealthRecordsAsync(1, null))
            .ReturnsAsync(records);
        _patientRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(new Patient());
        _doctorRepository
            .Setup(repository => repository.GetByIdAsync(2))
            .ReturnsAsync(new Doctor());
        _mapper
            .Setup(mapper => mapper.Map<IEnumerable<HealthRecordDto>>(
                records))
            .Returns([new HealthRecordDto { RecordId = 1 }]);

        var result = await _service.GetByPatientIdAsync(1);

        var dto = Assert.Single(result);
        Assert.Equal(1, dto.RecordId);
    }

    [Fact]
    public async Task GetPatientHistoryAsync_ShouldDelegateToPatientRecords()
    {
        var records = new List<HealthRecord>();
        _healthRepository
            .Setup(repository => repository.GetHealthRecordsAsync(1, null))
            .ReturnsAsync(records);
        _mapper
            .Setup(mapper => mapper.Map<IEnumerable<HealthRecordDto>>(
                records))
            .Returns([]);

        var result = await _service.GetPatientHistoryAsync(1);

        Assert.Empty(result);
        _healthRepository.Verify(
            repository => repository.GetHealthRecordsAsync(1, null),
            Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task ExistsByAppointmentIdAsync_WhenIdIsInvalid_ShouldThrow(
        int appointmentId)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.ExistsByAppointmentIdAsync(appointmentId));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ExistsByAppointmentIdAsync_ShouldReturnWhetherRecordExists(
        bool exists)
    {
        _healthRepository
            .Setup(repository => repository.GetHealthRecordsAsync(
                null,
                10))
            .ReturnsAsync(exists
                ? [new HealthRecord { RecordId = 1 }]
                : []);

        var result = await _service.ExistsByAppointmentIdAsync(10);

        Assert.Equal(exists, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByAppointmentIdAsync_WhenIdIsInvalid_ShouldThrow(
        int appointmentId)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.GetByAppointmentIdAsync(appointmentId));
    }

    [Fact]
    public async Task GetByAppointmentIdAsync_WhenRecordDoesNotExist_ShouldThrow()
    {
        _healthRepository
            .Setup(repository => repository.GetHealthRecordsAsync(
                null,
                10))
            .ReturnsAsync([]);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.GetByAppointmentIdAsync(10));
    }

    [Fact]
    public async Task GetByAppointmentIdAsync_ShouldUseFirstRecordAndLoadNavigation()
    {
        var first = CreateRecord(1, 1, 2);
        first.AppointmentId = 10;
        var second = CreateRecord(2, 3, 4);
        second.AppointmentId = 10;
        var records = new List<HealthRecord> { first, second };
        _healthRepository
            .Setup(repository => repository.GetHealthRecordsAsync(
                null,
                10))
            .ReturnsAsync(records);
        _patientRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(new Patient());
        _doctorRepository
            .Setup(repository => repository.GetByIdAsync(2))
            .ReturnsAsync(new Doctor());
        _appointmentRepository
            .Setup(repository => repository.GetByIdAsync(10))
            .ReturnsAsync(new Appointment());
        _mapper
            .Setup(mapper => mapper.Map<HealthRecordDto>(first))
            .Returns(new HealthRecordDto { RecordId = 1 });

        var result = await _service.GetByAppointmentIdAsync(10);

        Assert.Equal(1, result.RecordId);
        _mapper.Verify(
            mapper => mapper.Map<HealthRecordDto>(first),
            Times.Once);
        _mapper.Verify(
            mapper => mapper.Map<HealthRecordDto>(second),
            Times.Never);
    }

    private void SetupPatientAndDoctor(HealthRecordCreateDto dto)
    {
        _patientRepository
            .Setup(repository => repository.GetByIdAsync(dto.PatientId))
            .ReturnsAsync(new Patient { PatientId = dto.PatientId });
        _doctorRepository
            .Setup(repository => repository.GetByIdAsync(dto.DoctorId))
            .ReturnsAsync(new Doctor { DoctorId = dto.DoctorId });
    }

    private Appointment SetupValidConfirmedAppointment(
        HealthRecordCreateDto dto)
    {
        SetupPatientAndDoctor(dto);
        var appointment = CreateAppointment(
            dto.AppointmentId!.Value,
            dto.PatientId,
            dto.DoctorId,
            AppointmentStatus.Confirmed);
        _appointmentRepository
            .Setup(repository => repository.GetByIdAsync(
                dto.AppointmentId.Value))
            .ReturnsAsync(appointment);
        return appointment;
    }

    private static HealthRecordCreateDto CreateDto(
        int? appointmentId = null) => new()
        {
            PatientId = 1,
            DoctorId = 2,
            AppointmentId = appointmentId,
            VisitDate = DateOnly.FromDateTime(DateTime.Today),
            Diagnosis = "Flu",
            Prescription = "Medicine",
            Notes = "Rest"
        };

    private static HealthRecord CreateRecord(
        int recordId,
        int patientId,
        int doctorId) => new()
        {
            RecordId = recordId,
            PatientId = patientId,
            DoctorId = doctorId,
            VisitDate = DateOnly.FromDateTime(DateTime.Today),
            Diagnosis = "Diagnosis",
            Prescription = "Prescription"
        };

    private static Appointment CreateAppointment(
        int appointmentId,
        int patientId,
        int doctorId,
        AppointmentStatus status) => new()
        {
            AppointmentId = appointmentId,
            PatientId = patientId,
            DoctorId = doctorId,
            Status = status
        };
}