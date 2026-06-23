using AutoMapper;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using HealthApp.API.Models;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using HealthApp.API.Exceptions;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.Constants;

namespace HealthApp.API.Tests;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
    private readonly Mock<IPatientRepository> _patientRepoMock;
    private readonly Mock<IDoctorRepository> _doctorRepoMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly AppointmentService _service;

    public AppointmentServiceTests()
    {
        _appointmentRepoMock = new Mock<IAppointmentRepository>();
        _patientRepoMock = new Mock<IPatientRepository>();
        _doctorRepoMock = new Mock<IDoctorRepository>();
        _mapperMock = new Mock<IMapper>();

        _service = new AppointmentService(
            _appointmentRepoMock.Object,
            _patientRepoMock.Object,
            _doctorRepoMock.Object,
            _mapperMock.Object);
    }

    #region GetAllAppointmentsAsync

    [Fact]
    public async Task GetAllAppointmentsAsync_ShouldReturnMappedList()
    {
        var appointments = new List<Appointment> { new(), new() };
        var dtos = new List<AppointmentDto> { new(), new() };

        _appointmentRepoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(appointments);

        _mapperMock.Setup(m => m.Map<List<AppointmentDto>>(appointments))
            .Returns(dtos);

        var result = await _service.GetAllAppointmentsAsync();

        Assert.Equal(2, result.Count);
    }

    #endregion

    #region GetAppointmentByIdAsync

    [Fact]
    public async Task GetAppointmentByIdAsync_ShouldThrow_WhenInvalidId()
    {
        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            _service.GetAppointmentByIdAsync(0));
    }

    [Fact]
    public async Task GetAppointmentByIdAsync_ShouldThrow_WhenNotFound()
    {
        _appointmentRepoMock.Setup(r => r.GetByIdWithDetailsAsync(1))
            .ReturnsAsync((Appointment?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _service.GetAppointmentByIdAsync(1));
    }

    [Fact]
    public async Task GetAppointmentByIdAsync_ShouldReturnDto_WhenValid()
    {
        var appointment = new Appointment { AppointmentId = 1 };
        var dto = new AppointmentDto { AppointmentId = 1 };

        _appointmentRepoMock.Setup(r => r.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(m => m.Map<AppointmentDto>(appointment))
            .Returns(dto);

        var result = await _service.GetAppointmentByIdAsync(1);

        Assert.Equal(1, result.AppointmentId);
    }

    #endregion

    #region GetAppointmentsByPatientIdAsync

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_ShouldThrow_WhenPatientInvalid()
    {
        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            _service.GetAppointmentsByPatientIdAsync(0));
    }

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_ShouldThrow_WhenPatientNotFound()
    {
        _patientRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _service.GetAppointmentsByPatientIdAsync(1));
    }

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_ShouldReturnList()
    {
        var patient = new Patient { PatientId = 1 };
        var appointments = new List<Appointment> { new() };

        _patientRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(patient);

        _appointmentRepoMock.Setup(r => r.GetByPatientIdAsync(1))
            .ReturnsAsync(appointments);

        _mapperMock.Setup(m => m.Map<List<AppointmentDto>>(appointments))
            .Returns(new List<AppointmentDto> { new() });

        var result = await _service.GetAppointmentsByPatientIdAsync(1);

        Assert.Single(result);
    }

    #endregion

    #region GetAppointmentsByDoctorIdAsync

    [Fact]
    public async Task GetAppointmentsByDoctorIdAsync_ShouldThrow_WhenDoctorInvalid()
    {
        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            _service.GetAppointmentsByDoctorIdAsync(0));
    }

    [Fact]
    public async Task GetAppointmentsByDoctorIdAsync_ShouldThrow_WhenDoctorNotFound()
    {
        _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _service.GetAppointmentsByDoctorIdAsync(1));
    }

    [Fact]
    public async Task GetAppointmentsByDoctorIdAsync_ShouldReturnList()
    {
        var doctor = new Doctor { DoctorId = 1, IsActive = true };
        var appointments = new List<Appointment> { new() };

        _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(doctor);

        _appointmentRepoMock.Setup(r => r.GetByDoctorIdAsync(1))
            .ReturnsAsync(appointments);

        _mapperMock.Setup(m => m.Map<List<AppointmentDto>>(appointments))
            .Returns(new List<AppointmentDto> { new() });

        var result = await _service.GetAppointmentsByDoctorIdAsync(1);

        Assert.Single(result);
    }

    #endregion

    #region GetAppointmentsByStatusAsync

    [Fact]
    public async Task GetAppointmentsByStatusAsync_ShouldReturnList()
    {
        var appointments = new List<Appointment> { new() };

        _appointmentRepoMock.Setup(r =>
                r.GetByStatusAsync(AppointmentStatus.Pending))
            .ReturnsAsync(appointments);

        _mapperMock.Setup(m => m.Map<List<AppointmentDto>>(appointments))
            .Returns(new List<AppointmentDto> { new() });

        var result = await _service.GetAppointmentsByStatusAsync(AppointmentStatus.Pending);

        Assert.Single(result);
    }

    #endregion

    #region BookAppointmentAsync

    [Fact]
    public async Task BookAppointmentAsync_ShouldThrow_WhenDtoNull()
    {
        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            _service.BookAppointmentAsync(null!));
    }

    [Fact]
    public async Task BookAppointmentAsync_ShouldThrow_WhenDoctorInactive()
    {
        var dto = new BookAppointmentDto
        {
            PatientId = 1,
            DoctorId = 1,
            ScheduledDate = DateTime.Today,
            TimeSlot = TimeSlots.Slots[0]
        };

        _patientRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Patient());

        _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Doctor { DoctorId = 1, IsActive = false });

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            _service.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointmentAsync_ShouldThrow_WhenPastDate()
    {
        var dto = new BookAppointmentDto
        {
            PatientId = 1,
            DoctorId = 1,
            ScheduledDate = DateTime.Today.AddDays(-1),
            TimeSlot = TimeSlots.Slots[0]
        };

        _patientRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Patient());

        _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Doctor { DoctorId = 1, IsActive = true });

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            _service.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointmentAsync_ShouldThrow_WhenInvalidSlot()
    {
        var dto = new BookAppointmentDto
        {
            PatientId = 1,
            DoctorId = 1,
            ScheduledDate = DateTime.Today,
            TimeSlot = "INVALID"
        };

        _patientRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Patient());

        _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Doctor { DoctorId = 1, IsActive = true });

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            _service.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointmentAsync_ShouldThrow_WhenSlotAlreadyBooked()
    {
        var dto = new BookAppointmentDto
        {
            PatientId = 1,
            DoctorId = 1,
            ScheduledDate = DateTime.Today,
            TimeSlot = TimeSlots.Slots[0]
        };

        _patientRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Patient());

        _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Doctor { DoctorId = 1, IsActive = true });

        _appointmentRepoMock.Setup(r =>
                r.IsSlotBookedAsync(1, dto.ScheduledDate.Date, dto.TimeSlot))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ConflictException>(() =>
            _service.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointmentAsync_ShouldThrow_WhenPatientHasConflict()
    {
        var dto = new BookAppointmentDto
        {
            PatientId = 1,
            DoctorId = 1,
            ScheduledDate = DateTime.Today,
            TimeSlot = TimeSlots.Slots[0]
        };

        _patientRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Patient());

        _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Doctor { DoctorId = 1, IsActive = true });
            
        _appointmentRepoMock.Setup(r =>
                r.IsSlotBookedAsync(1, dto.ScheduledDate.Date, dto.TimeSlot))
            .ReturnsAsync(false);

        _appointmentRepoMock.Setup(r =>
                r.PatientHasActiveAppointmentOnDateAndSlotAsync(
                    dto.PatientId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ConflictException>(() =>
            _service.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointmentAsync_ShouldCreateAppointment_WhenValid()
    {
        var dto = new BookAppointmentDto
        {
            PatientId = 1,
            DoctorId = 1,
            ScheduledDate = DateTime.Today,
            TimeSlot = TimeSlots.Slots[0]
        };

        var appointment = new Appointment();
        var saved = new Appointment { AppointmentId = 1 };
        var dtoResult = new AppointmentDto { AppointmentId = 1 };

        _patientRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Patient());

        _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Doctor { DoctorId = 1, IsActive = true });

        _appointmentRepoMock.Setup(r =>
                r.IsSlotBookedAsync(1, dto.ScheduledDate.Date, dto.TimeSlot))
            .ReturnsAsync(false);

        _appointmentRepoMock.Setup(r =>
                r.PatientHasActiveAppointmentOnDateAndSlotAsync(
                    dto.PatientId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot))
            .ReturnsAsync(false);

        _mapperMock.Setup(m => m.Map<Appointment>(dto))
            .Returns(appointment);

        _appointmentRepoMock.Setup(r => r.AddAsync(It.IsAny<Appointment>()))
            .ReturnsAsync(saved);

        _mapperMock.Setup(m => m.Map<AppointmentDto>(saved))
            .Returns(dtoResult);

        var result = await _service.BookAppointmentAsync(dto);

        Assert.Equal(1, result.AppointmentId);
    }

    #endregion

    #region ChangeAppointmentStatusAsync

    [Fact]
    public async Task ChangeAppointmentStatusAsync_ShouldThrow_WhenInvalidId()
    {
        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            _service.ChangeAppointmentStatusAsync(0, new UpdateAppointmentStatusDto()));
    }

    [Fact]
    public async Task ChangeAppointmentStatusAsync_ShouldThrow_WhenNotFound()
    {
        _appointmentRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Appointment?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _service.ChangeAppointmentStatusAsync(1, new UpdateAppointmentStatusDto()));
    }

    [Fact]
    public async Task ChangeAppointmentStatusAsync_ShouldThrow_WhenCancelWithoutReason()
    {
        var appointment = new Appointment { AppointmentId = 1 };

        _appointmentRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            _service.ChangeAppointmentStatusAsync(1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Cancelled
                }));
    }

    [Fact]
    public async Task ChangeAppointmentStatusAsync_ShouldUpdateStatus()
    {
        var appointment = new Appointment { AppointmentId = 1 };

        var updated = new Appointment { AppointmentId = 1 };
        var dto = new AppointmentDto { AppointmentId = 1 };

        _appointmentRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        _appointmentRepoMock.Setup(r => r.UpdateAsync(1, appointment))
            .ReturnsAsync(updated);

        _mapperMock.Setup(m => m.Map<AppointmentDto>(updated))
            .Returns(dto);

        var result = await _service.ChangeAppointmentStatusAsync(1,
            new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Confirmed
            });

        Assert.Equal(1, result.AppointmentId);
    }

    #endregion

    #region CancelAppointmentAsync

    [Fact]
    public async Task CancelAppointmentAsync_ShouldSetDefaultReason()
    {
        var appointment = new Appointment { AppointmentId = 1 };
        var updated = new Appointment { AppointmentId = 1 };
        var dto = new AppointmentDto { AppointmentId = 1 };

        _appointmentRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        _appointmentRepoMock.Setup(r => r.UpdateAsync(1, appointment))
            .ReturnsAsync(updated);

        _mapperMock.Setup(m => m.Map<AppointmentDto>(updated))
            .Returns(dto);

        var result = await _service.CancelAppointmentAsync(1, null);

        Assert.Equal(1, result.AppointmentId);
    }

    #endregion
}