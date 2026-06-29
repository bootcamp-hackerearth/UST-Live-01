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
    public async Task UpdateAsync_Should_Update_Record_When_Valid()
    {
        var record = new HealthRecord
        {
            HealthRecordId = 1,
            AppointmentId = 1
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Completed"
        };

        var dto = new UpdateHealthRecordDto
        {
            Diagnosis = "Flu",
            Prescription = "Medicine"
        };

        var updated = new HealthRecord();

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(record);
        _appointmentRepoMock.Setup(a => a.GetByIdAsync(1, default)).ReturnsAsync(appointment);
        _repoMock.Setup(r => r.UpdateAsync(1, record, default)).ReturnsAsync(updated);
        _mapperMock.Setup(m => m.Map<HealthRecordDto>(updated)).Returns(new HealthRecordDto());

        var result = await _service.UpdateAsync(1, dto);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Not_Completed()
    {
        var record = new HealthRecord { HealthRecordId = 1, AppointmentId = 1 };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Pending"
        };

        var dto = new UpdateHealthRecordDto
        {
            Diagnosis = "Flu",
            Prescription = "Med"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(record);
        _appointmentRepoMock.Setup(a => a.GetByIdAsync(1, default)).ReturnsAsync(appointment);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, dto));
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Diagnosis_Empty()
    {
        var record = new HealthRecord { HealthRecordId = 1, AppointmentId = 1 };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Completed"
        };

        var dto = new UpdateHealthRecordDto
        {
            Diagnosis = "",
            Prescription = "Med"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(record);
        _appointmentRepoMock.Setup(a => a.GetByIdAsync(1, default)).ReturnsAsync(appointment);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, dto));
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