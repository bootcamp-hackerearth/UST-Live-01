using AutoMapper;
using FluentAssertions;
using HealthAxisApplicn.Dto.Appointments;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;
using HealthAxisApplicn.Services;
using HealthAxisApplicn.Services.Impl;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AppointmentService _service;
    private readonly Mock<IHealthRecordService> _healthRecordMock;


    public AppointmentServiceTests()
    {
        _repoMock = new Mock<IAppointmentRepository>();
        _mapperMock = new Mock<IMapper>();
        _healthRecordMock = new Mock<IHealthRecordService>();
        _service = new AppointmentService(
            _repoMock.Object,
            _healthRecordMock.Object,
            _mapperMock.Object);

    }

    [Fact]
    public async Task CreateAsync_Should_Create_Appointment()
    {
        var dto = new CreateAppointmentDto
        {
            ScheduledDate = DateTime.UtcNow.AddDays(1), 
            TimeSlot = "10:00",
            DoctorId = 1
        };

        var appointment = new Appointment();
        var resultDto = new AppointmentDto();
        var patientId = 1;

        _mapperMock.Setup(m => m.Map<Appointment>(dto))
                   .Returns(appointment);

        _repoMock.Setup(r =>
            r.DoctorHasConflictAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), default))
            .ReturnsAsync(false);

        _repoMock.Setup(r =>
            r.PatientHasConflictAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), default))
            .ReturnsAsync(false);

        _repoMock.Setup(r =>
            r.PatientHasAppointmentOnDateAsync(It.IsAny<int>(), It.IsAny<DateTime>(), default))
            .ReturnsAsync(false);

        _repoMock.Setup(r =>
            r.CreateAsync(It.IsAny<Appointment>(), default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(m => m.Map<AppointmentDto>(appointment))
                   .Returns(resultDto);

        var result = await _service.CreateAsync(dto, patientId);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_List()
    {
        var list = new List<Appointment> { new Appointment { AppointmentId = 1 } };
        var dtos = new List<AppointmentDto> { new AppointmentDto { AppointmentId = 1 } };

        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(list);
        _mapperMock.Setup(m => m.Map<List<AppointmentDto>>(list)).Returns(dtos);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Appointment()
    {
        var appointment = new Appointment { AppointmentId = 1 };
        var dto = new AppointmentDto { AppointmentId = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(appointment);
        _mapperMock.Setup(m => m.Map<AppointmentDto?>(appointment)).Returns(dto);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.AppointmentId.Should().Be(1);
    }

    [Fact]
    public async Task DeleteAppointmentAsync_Should_Return_True_When_Deleted()
    {
        _repoMock.Setup(r => r.DeleteAsync(1, default)).ReturnsAsync(true);

        var result = await _service.DeleteAppointmentAsync(1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetAppointmentsByDoctorIdAsync_Should_Return_List()
    {
        var list = new List<Appointment> { new Appointment() };
        var dtos = new List<AppointmentDto> { new AppointmentDto() };

        _repoMock.Setup(r => r.GetUpcomingAppointmentsByDoctorIdAsync(1, default)).ReturnsAsync(list);
        _mapperMock.Setup(m => m.Map<List<AppointmentDto>>(list)).Returns(dtos);

        var result = await _service.GetAppointmentsByDoctorIdAsync(1);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_Should_Return_List()
    {
        var list = new List<Appointment> { new Appointment() };
        var dtos = new List<AppointmentDto> { new AppointmentDto() };

        _repoMock.Setup(r => r.GetAppointmentsByPatientIdAsync(1, default)).ReturnsAsync(list);
        _mapperMock.Setup(m => m.Map<List<AppointmentDto>>(list)).Returns(dtos);

        var result = await _service.GetAppointmentsByPatientIdAsync(1);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Null_When_NotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync((Appointment?)null);

        var result = await _service.UpdateAsync(1, new UpdateAppointmentStatusDto(), "Doctor");
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Completed()
    {
        var existing = new Appointment { AppointmentId = 1, Status = "Completed" };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(existing);


        await Assert.ThrowsAsync<System.Exception>(() =>
            _service.UpdateAsync(1, new UpdateAppointmentStatusDto { Status = "Cancelled" }, "Doctor"));

    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Cancelled_Without_Reason()
    {
        var existing = new Appointment { AppointmentId = 1, Status = "Scheduled" };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(existing);


        await Assert.ThrowsAsync<System.Exception>(() =>
            _service.UpdateAsync(1, new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = ""
            }, "Doctor"));

    }

    [Fact]
    public async Task UpdateAsync_Should_Update_When_Valid()
    {
        var existing = new Appointment { AppointmentId = 1, Status = "Scheduled" };
        var updated = new Appointment { AppointmentId = 1, Status = "Cancelled" };
        var dto = new AppointmentDto { AppointmentId = 1, Status = "Cancelled" };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.UpdateAsync(1, existing, default)).ReturnsAsync(updated);
        _mapperMock.Setup(m => m.Map<AppointmentDto?>(updated)).Returns(dto);


        var result = await _service.UpdateAsync(1, new UpdateAppointmentStatusDto
        {
            Status = "Cancelled",
            CancellationReason = "Patient request"
        }, "Doctor");


        result.Status.Should().Be("Cancelled");
    }

    [Fact]
    public async Task UpdateAsync_Should_Call_CreateFromAppointment_When_Completed()
    {
        var existing = new Appointment { AppointmentId = 1, Status = "Confirmed" };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.UpdateAsync(1, existing, default))
                 .ReturnsAsync(existing);

        _mapperMock.Setup(m => m.Map<AppointmentDto>(existing))
                   .Returns(new AppointmentDto());


        await _service.UpdateAsync(1, new UpdateAppointmentStatusDto
        {
            Status = "Completed"
        }, "Doctor");


        _healthRecordMock.Verify(x =>
            x.CreateFromAppointment(It.IsAny<Appointment>()),
            Times.Once);
    }
}