using HealthAxis.Api.Data;
using HealthAxis.Api.Repositories.Interfaces;
using HealthAxis.Api.Services;
using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _appointmentRepo;
    private readonly Mock<IPatientRepository> _patientRepo;
    private readonly Mock<IDoctorRepository> _doctorRepo;

    private readonly AppointmentService _service;

    public AppointmentServiceTests()
    {
        _appointmentRepo = new Mock<IAppointmentRepository>();
        _patientRepo = new Mock<IPatientRepository>();
        _doctorRepo = new Mock<IDoctorRepository>();

        _service = new AppointmentService(
            _appointmentRepo.Object,
            _patientRepo.Object,
            _doctorRepo.Object);
    }

    [Fact]
    public void GetAll_ShouldReturnMappedDtos()
    {
        _appointmentRepo.Setup(r => r.GetAll()).Returns(new List<Appointment>
        {
            new Appointment { AppointmentId = 1 }
        });

        var result = _service.GetAll();

        Assert.NotNull(result);
        Assert.Equal(1, result.Count());
    }

    [Fact]
    public void GetByPatient_ShouldReturnResults()
    {
        _appointmentRepo.Setup(r => r.GetByPatient(1)).Returns(new List<Appointment>
        {
            new Appointment { AppointmentId = 1 }
        });

        var result = _service.GetByPatient(1);

        Assert.Equal(1, result.Count());
    }

    [Fact]
    public void GetByDoctor_ShouldReturnResults()
    {
        _appointmentRepo.Setup(r => r.GetByDoctor(1)).Returns(new List<Appointment>
        {
            new Appointment { AppointmentId = 1 }
        });

        var result = _service.GetByDoctor(1);

        Assert.Equal(1, result.Count());
    }

    [Fact]
    public void GetTodaySchedule_ShouldCallRepo()
    {
        _appointmentRepo.Setup(r => r.GetByDoctorAndDate(
            It.IsAny<int>(), It.IsAny<DateTime>()))
            .Returns(new List<Appointment>());

        var result = _service.GetTodaySchedule(1);

        Assert.NotNull(result);
    }

    [Fact]
    public void GetWeeklySchedule_ShouldCallRepo()
    {
        _appointmentRepo.Setup(r => r.GetByDoctorAndDateRange(
            It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(new List<Appointment>());

        var result = _service.GetWeeklySchedule(1, DateTime.Today);

        Assert.NotNull(result);
    }

    private AppointmentDto GetValidDto()
    {
        return new AppointmentDto
        {
            PatientId = 1,
            DoctorId = 1,
            ScheduledDate = DateTime.Today.AddDays(1),
            TimeSlot = "10AM"
        };
    }

    [Fact]
    public void Book_InvalidPatient_ShouldReturnFalse()
    {
        _patientRepo.Setup(p => p.GetById(1)).Returns((Patient)null);

        var result = _service.Book(GetValidDto(), out string error);

        Assert.False(result);
        Assert.Equal("Invalid patient.", error);
    }

    [Fact]
    public void Book_InvalidDoctor_ShouldReturnFalse()
    {
        _patientRepo.Setup(p => p.GetById(1)).Returns(new Patient());
        _doctorRepo.Setup(d => d.GetById(1)).Returns((Doctor)null);

        var result = _service.Book(GetValidDto(), out string error);

        Assert.False(result);
        Assert.Equal("Invalid doctor.", error);
    }

    [Fact]
    public void Book_PastDate_ShouldReturnFalse()
    {
        var dto = GetValidDto();
        dto.ScheduledDate = DateTime.Today.AddDays(-1);

        _patientRepo.Setup(p => p.GetById(1)).Returns(new Patient());
        _doctorRepo.Setup(d => d.GetById(1)).Returns(new Doctor());

        var result = _service.Book(dto, out string error);

        Assert.False(result);
        Assert.Equal("Appointment date cannot be in the past.", error);
    }

    [Fact]
    public void Book_Valid_ShouldSucceed()
    {
        _patientRepo.Setup(p => p.GetById(1)).Returns(new Patient());
        _doctorRepo.Setup(d => d.GetById(1)).Returns(new Doctor());

        _appointmentRepo.Setup(r => r.Exists(It.IsAny<Func<Appointment, bool>>()))
            .Returns(false);

        _appointmentRepo.Setup(r => r.IsSlotAvailable(
            It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>()))
            .Returns(true);

        var result = _service.Book(GetValidDto(), out string error);

        Assert.True(result);
        Assert.Equal(string.Empty, error);

        _appointmentRepo.Verify(r => r.Add(It.IsAny<Appointment>()), Times.Once);
    }

    [Fact]
    public void UpdateStatus_CancelWithoutReason_ShouldFail()
    {
        var dto = new AppointmentStatusUpdateDto
        {
            Status = AppointmentStatusEnum.Cancelled,
            CancellationReason = ""
        };

        var result = _service.UpdateStatus(1, dto, out string error);

        Assert.False(result);
        Assert.Equal("Cancellation reason is required.", error);
    }

    [Fact]
    public void UpdateStatus_Valid_ShouldCallRepo()
    {
        var dto = new AppointmentStatusUpdateDto
        {
            Status = AppointmentStatusEnum.Completed
        };

        _appointmentRepo.Setup(r => r.UpdateStatus(1, "Completed", null))
            .Returns(true);

        var result = _service.UpdateStatus(1, dto, out string error);

        Assert.True(result);
    }

    [Fact]
    public void Delete_ShouldCallRepo()
    {
        _appointmentRepo.Setup(r => r.Delete(1)).Returns(true);

        var result = _service.Delete(1);

        Assert.True(result);
    }
}
