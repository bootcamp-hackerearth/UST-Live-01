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

public class HealthRecordServiceTests
{
    private readonly Mock<IHealthRecordRepository> _repo;
    private readonly HealthRecordService _service;

    public HealthRecordServiceTests()
    {
        _repo = new Mock<IHealthRecordRepository>();
        _service = new HealthRecordService(_repo.Object);
    }

    [Fact]
    public void GetByPatient_ShouldReturnMappedDtos()
    {
        _repo.Setup(r => r.GetByPatient(1))
            .Returns(new List<HealthRecord>
            {
                new HealthRecord
                {
                    RecordId = 1,
                    PatientId = 1,
                    DoctorId = 2,
                    Diagnosis = "Test",
                    Prescription = "Test",
                    VisitDate = DateTime.Today
                }
            });

        var result = _service.GetByPatient(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Count());
    }

    [Fact]
    public void Create_ShouldFail_WhenDuplicateAppointmentRecordExists()
    {
        _repo.Setup(r => r.GetByAppointmentId(1))
            .Returns(new HealthRecord());

        var dto = new HealthRecordDto
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 1,
            Diagnosis = "Test",
            Prescription = "Test"
        };

        var result = _service.Create(dto, out string error);

        Assert.False(result);
        Assert.Equal("A health record already exists for this appointment.", error);
    }

    [Fact]
    public void Create_ShouldSucceed_WhenNoDuplicate()
    {
        _repo.Setup(r => r.GetByAppointmentId(It.IsAny<int>()))
            .Returns((HealthRecord)null);

        var dto = new HealthRecordDto
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 1,
            Diagnosis = "Test",
            Prescription = "Test",
            Notes = "Note"
        };

        var result = _service.Create(dto, out string error);

        Assert.True(result);
        Assert.Equal(string.Empty, error);
        _repo.Verify(r => r.Add(It.IsAny<HealthRecord>()), Times.Once);
    }

    [Fact]
    public void Create_ShouldSucceed_WhenAppointmentIdIsNull()
    {
        var dto = new HealthRecordDto
        {
            AppointmentId = null,
            PatientId = 1,
            DoctorId = 1,
            Diagnosis = "Test",
            Prescription = "Test"
        };

        var result = _service.Create(dto, out string error);

        Assert.True(result);
        Assert.Equal(string.Empty, error);
        _repo.Verify(r => r.Add(It.IsAny<HealthRecord>()), Times.Once);
    }

    [Fact]
    public void GetByAppointmentId_ShouldReturnNull_WhenNotFound()
    {
        _repo.Setup(r => r.GetByAppointmentId(1))
            .Returns((HealthRecord)null);

        var result = _service.GetByAppointmentId(1);

        Assert.Null(result);
    }

    [Fact]
    public void GetByAppointmentId_ShouldReturnDto_WhenFound()
    {
        _repo.Setup(r => r.GetByAppointmentId(1))
            .Returns(new HealthRecord
            {
                RecordId = 1,
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 2,
                Diagnosis = "D",
                Prescription = "P",
                Notes = "N",
                VisitDate = DateTime.Today
            });

        var result = _service.GetByAppointmentId(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.RecordId);
        Assert.Equal("D", result.Diagnosis);
    }
}