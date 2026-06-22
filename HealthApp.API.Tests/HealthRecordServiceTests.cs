using AutoMapper;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using HealthApp.API.Models;
using HealthApp.API.Models.DTOs;
using HealthApp.API.Enums;
using HealthApp.API.Exceptions;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;

namespace HealthApp.API.Tests;

public class HealthRecordServiceTests
{
    private readonly Mock<IHealthRecordRepository> _healthRepoMock;
    private readonly Mock<IPatientRepository> _patientRepoMock;
    private readonly Mock<IDoctorRepository> _doctorRepoMock;
    private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly HealthRecordService _service;

    public HealthRecordServiceTests()
    {
        _healthRepoMock = new Mock<IHealthRecordRepository>();
        _patientRepoMock = new Mock<IPatientRepository>();
        _doctorRepoMock = new Mock<IDoctorRepository>();
        _appointmentRepoMock = new Mock<IAppointmentRepository>();
        _mapperMock = new Mock<IMapper>();

        _service = new HealthRecordService(
            _healthRepoMock.Object,
            _patientRepoMock.Object,
            _doctorRepoMock.Object,
            _appointmentRepoMock.Object,
            _mapperMock.Object);
    }

    #region GetAllHealthRecordsAsync

    [Fact]
    public async Task GetAllHealthRecordsAsync_ShouldReturnMappedList()
    {
        var records = new List<HealthRecord> { new(), new() };
        var dtos = new List<HealthRecordDto> { new(), new() };

        _healthRepoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(records);

        _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(records))
            .Returns(dtos);

        var result = await _service.GetAllHealthRecordsAsync();

        Assert.Equal(2, result.Count);
    }

    #endregion

    #region GetHealthRecordByIdAsync

    [Fact]
    public async Task GetHealthRecordByIdAsync_ShouldThrow_WhenInvalidId()
    {
        await Assert.ThrowsAsync<HealthRecordRuleException>(() =>
            _service.GetHealthRecordByIdAsync(0));
    }

    [Fact]
    public async Task GetHealthRecordByIdAsync_ShouldThrow_WhenNotFound()
    {
        _healthRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((HealthRecord?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _service.GetHealthRecordByIdAsync(1));
    }

    [Fact]
    public async Task GetHealthRecordByIdAsync_ShouldReturnDto_WhenValid()
    {
        var record = new HealthRecord { HealthRecordId = 1 };
        var dto = new HealthRecordDto { HealthRecordId = 1 };

        _healthRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(record);

        _mapperMock.Setup(m => m.Map<HealthRecordDto>(record))
            .Returns(dto);

        var result = await _service.GetHealthRecordByIdAsync(1);

        Assert.Equal(1, result.HealthRecordId);
    }

    #endregion

    #region GetHealthRecordsByPatientIdAsync

    [Fact]
    public async Task GetHealthRecordsByPatientIdAsync_ShouldThrow_WhenPatientNotFound()
    {
        _patientRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _service.GetHealthRecordsByPatientIdAsync(1));
    }

    [Fact]
    public async Task GetHealthRecordsByPatientIdAsync_ShouldReturnList()
    {
        var records = new List<HealthRecord> { new() };

        _patientRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Patient());

        _healthRepoMock.Setup(r => r.GetByPatientIdAsync(1))
            .ReturnsAsync(records);

        _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(records))
            .Returns(new List<HealthRecordDto> { new() });

        var result = await _service.GetHealthRecordsByPatientIdAsync(1);

        Assert.Single(result);
    }

    #endregion

    #region AddHealthRecordAsync

    [Fact]
    public async Task AddHealthRecordAsync_ShouldThrow_WhenDtoNull()
    {
        await Assert.ThrowsAsync<HealthRecordRuleException>(() =>
            _service.AddHealthRecordAsync(null!));
    }

    [Fact]
    public async Task AddHealthRecordAsync_ShouldThrow_WhenAppointmentNotFound()
    {
        var dto = new AddHealthRecordDto
        {
            AppointmentId = 1,
            PatientId = 1
        };

        _appointmentRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Appointment?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _service.AddHealthRecordAsync(dto));
    }

    [Fact]
    public async Task AddHealthRecordAsync_ShouldThrow_WhenPatientMismatch()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 2,
            Status = AppointmentStatus.Completed.ToString()
        };

        var dto = new AddHealthRecordDto
        {
            AppointmentId = 1,
            PatientId = 1
        };

        _appointmentRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<HealthRecordRuleException>(() =>
            _service.AddHealthRecordAsync(dto));
    }

    [Fact]
    public async Task AddHealthRecordAsync_ShouldThrow_WhenAppointmentNotCompleted()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 1,
            Status = AppointmentStatus.Pending.ToString()
        };

        var dto = new AddHealthRecordDto
        {
            AppointmentId = 1,
            PatientId = 1
        };

        _appointmentRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<HealthRecordRuleException>(() =>
            _service.AddHealthRecordAsync(dto));
    }

    [Fact]
    public async Task AddHealthRecordAsync_ShouldThrow_WhenAlreadyExists()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 1,
            Status = AppointmentStatus.Completed.ToString()
        };

        var dto = new AddHealthRecordDto
        {
            AppointmentId = 1,
            PatientId = 1
        };

        _appointmentRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        _healthRepoMock.Setup(r => r.ExistsByAppointmentIdAsync(1))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ConflictException>(() =>
            _service.AddHealthRecordAsync(dto));
    }

    [Fact]
    public async Task AddHealthRecordAsync_ShouldCreateRecord_WhenValid()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 10,
            Status = AppointmentStatus.Confirmed.ToString()
        };

        var dto = new AddHealthRecordDto
        {
            AppointmentId = 1,
            PatientId = 1
        };

        var record = new HealthRecord();
        var saved = new HealthRecord { HealthRecordId = 1 };
        var dtoResult = new HealthRecordDto { HealthRecordId = 1 };

        _appointmentRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        _healthRepoMock.Setup(r => r.ExistsByAppointmentIdAsync(1))
            .ReturnsAsync(false);

        _mapperMock.Setup(m => m.Map<HealthRecord>(dto))
            .Returns(record);

        _healthRepoMock.Setup(r => r.AddAsync(It.IsAny<HealthRecord>()))
            .ReturnsAsync(saved);

        _appointmentRepoMock.Setup(r =>
                r.UpdateAsync(1, appointment))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(m => m.Map<HealthRecordDto>(saved))
            .Returns(dtoResult);

        var result = await _service.AddHealthRecordAsync(dto);

        Assert.NotNull(result);
        Assert.Equal(1, result.HealthRecordId);

        // Ensure appointment marked completed
        Assert.Equal(AppointmentStatus.Completed.ToString(), appointment.Status);
    }

    #endregion
}
