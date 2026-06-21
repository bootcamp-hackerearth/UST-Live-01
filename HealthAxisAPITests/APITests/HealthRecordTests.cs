using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using HealthAxisApplicn.Services.Impl;
using HealthAxisApplicn.Repositories;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Dto.HealthRecords;
using System.Collections.Generic;
using System.Threading.Tasks;

public class HealthRecordServiceTests
{
    private readonly Mock<IHealthRecordRepository> _repoMock;
    private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly HealthRecordService _service;

    public HealthRecordServiceTests()
    {
        _repoMock = new Mock<IHealthRecordRepository>();
        _appointmentRepoMock = new Mock<IAppointmentRepository>();
        _mapperMock = new Mock<IMapper>();

        _service = new HealthRecordService(
            _repoMock.Object,
            _appointmentRepoMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Create_Record_When_Valid()
    {
        var dto = new CreateHealthRecordDto
        {
            AppointmentId = 1,
            Diagnosis = "Flu",
            Prescription = "Medicine",
            VisitDate = System.DateTime.Now
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Completed",
            PatientId = 10,
            DoctorId = 20
        };

        var record = new HealthRecord();
        var resultDto = new HealthRecordDto();

        _appointmentRepoMock.Setup(a => a.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        _repoMock.Setup(r => r.CreateAsync(It.IsAny<HealthRecord>(), default))
            .ReturnsAsync(record);

        _mapperMock.Setup(m => m.Map<HealthRecordDto>(record))
            .Returns(resultDto);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Diagnosis_Empty()
    {
        var dto = new CreateHealthRecordDto
        {
            Diagnosis = "",
            Prescription = "Med"
        };

        await Assert.ThrowsAsync<System.Exception>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Prescription_Empty()
    {
        var dto = new CreateHealthRecordDto
        {
            Diagnosis = "Flu",
            Prescription = ""
        };

        await Assert.ThrowsAsync<System.Exception>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Appointment_NotFound()
    {
        var dto = new CreateHealthRecordDto
        {
            AppointmentId = 1,
            Diagnosis = "Flu",
            Prescription = "Med"
        };

        _appointmentRepoMock.Setup(a => a.GetByIdAsync(1, default))
            .ReturnsAsync((Appointment?)null);

        await Assert.ThrowsAsync<System.Exception>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Appointment_NotCompleted()
    {
        var dto = new CreateHealthRecordDto
        {
            AppointmentId = 1,
            Diagnosis = "Flu",
            Prescription = "Med"
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Scheduled"
        };

        _appointmentRepoMock.Setup(a => a.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<System.Exception>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_List()
    {
        var list = new List<HealthRecord> { new HealthRecord() };
        var dtos = new List<HealthRecordDto> { new HealthRecordDto() };

        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(list);
        _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(list)).Returns(dtos);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Record()
    {
        var record = new HealthRecord { HealthRecordId = 1 };
        var dto = new HealthRecordDto { HealthRecordId = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(record);
        _mapperMock.Setup(m => m.Map<HealthRecordDto?>(record)).Returns(dto);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.HealthRecordId.Should().Be(1);
    }

    [Fact]
    public async Task GetRecordsByPatientIdAsync_Should_Return_List()
    {
        var list = new List<HealthRecord> { new HealthRecord() };
        var dtos = new List<HealthRecordDto> { new HealthRecordDto() };

        _repoMock.Setup(r => r.GetRecordsByPatientIdAsync(1, default)).ReturnsAsync(list);
        _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(list)).Returns(dtos);

        var result = await _service.GetRecordsByPatientIdAsync(1);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetRecordsByDoctorIdAsync_Should_Return_List()
    {
        var list = new List<HealthRecord> { new HealthRecord() };
        var dtos = new List<HealthRecordDto> { new HealthRecordDto() };

        _repoMock.Setup(r => r.GetRecordsByDoctorIdAsync(1, default)).ReturnsAsync(list);
        _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(list)).Returns(dtos);

        var result = await _service.GetRecordsByDoctorIdAsync(1);

        result.Should().HaveCount(1);
    }
}