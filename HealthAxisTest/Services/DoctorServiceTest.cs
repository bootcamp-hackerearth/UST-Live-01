using HealthAxis.Api.Data;
using HealthAxis.Api.Repositories.Interfaces;
using HealthAxis.Api.Services;
using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class DoctorServiceTests
{
    private readonly Mock<IDoctorRepository> _doctorRepo;
    private readonly DoctorService _service;

    public DoctorServiceTests()
    {
        _doctorRepo = new Mock<IDoctorRepository>();
        _service = new DoctorService(_doctorRepo.Object);
    }

    [Fact]
    public void GetAll_ShouldReturnMappedDtos()
    {
        _doctorRepo.Setup(r => r.GetAll(null, false))
            .Returns(new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Test",
                    Specialisation = "Cardiology",
                    YearsOfExperience = 5,
                    ConsultationFee = 500,
                    IsActive = true
                }
            });

        var result = _service.GetAll();

        Assert.NotNull(result);
        Assert.Equal(1, result.Count());
    }

    [Fact]
    public void GetById_ShouldReturnNull_WhenDoctorNotFound()
    {
        _doctorRepo.Setup(r => r.GetById(1)).Returns((Doctor)null);

        var result = _service.GetById(1);

        Assert.Null(result);
    }

    [Fact]
    public void GetById_ShouldReturnDto_WithAppointmentCount()
    {
        _doctorRepo.Setup(r => r.GetById(1)).Returns(new Doctor
        {
            DoctorId = 1,
            FullName = "Doc",
            Specialisation = "Cardiology",
            YearsOfExperience = 5,
            ConsultationFee = 500,
            IsActive = true
        });

        _doctorRepo.Setup(r => r.GetUpcomingAppointmentCount(1))
            .Returns(10);

        var result = _service.GetById(1);

        Assert.NotNull(result);
        Assert.Equal(10, result.UpcomingAppointmentCount);
    }

    [Fact]
    public void Create_ShouldFail_WhenFeeNegative()
    {
        var dto = new DoctorDto
        {
            ConsultationFee = -1
        };

        var result = _service.Create(dto, out string error, out int doctorId);

        Assert.False(result);
        Assert.Equal("Fee cannot be negative.", error);
        Assert.Equal(0, doctorId);
    }

    [Fact]
    public void Create_ShouldSucceed()
    {
        _doctorRepo.Setup(r => r.Add(It.IsAny<Doctor>()))
            .Returns(new Doctor { DoctorId = 5 });

        var dto = new DoctorDto
        {
            FullName = "Doc",
            Specialisation = SpecialisationEnum.Cardiologist,
            YearsOfExperience = 5,
            ConsultationFee = 500
        };

        var result = _service.Create(dto, out string error, out int doctorId);

        Assert.True(result);
        Assert.Equal(5, doctorId);
        Assert.Equal(string.Empty, error);
    }

    [Fact]
    public void Update_ShouldFail_WhenFeeNegative()
    {
        var dto = new DoctorDto
        {
            ConsultationFee = -1
        };

        var result = _service.Update(1, dto, out string error);

        Assert.False(result);
        Assert.Equal("Fee cannot be negative.", error);
    }

    [Fact]
    public void Update_ShouldFail_WhenDoctorNotFound()
    {
        _doctorRepo.Setup(r => r.Update(It.IsAny<Doctor>()))
            .Returns(false);

        var dto = new DoctorDto
        {
            FullName = "Doc",
            Specialisation = SpecialisationEnum.Cardiologist,
            YearsOfExperience = 5,
            ConsultationFee = 500
        };

        var result = _service.Update(1, dto, out string error);

        Assert.False(result);
        Assert.Equal("Doctor not found.", error);
    }

    [Fact]
    public void Update_ShouldSucceed()
    {
        _doctorRepo.Setup(r => r.Update(It.IsAny<Doctor>()))
            .Returns(true);

        var dto = new DoctorDto
        {
            FullName = "Doc",
            Specialisation = SpecialisationEnum.Cardiologist,
            YearsOfExperience = 5,
            ConsultationFee = 500,
            IsActive = true
        };

        var result = _service.Update(1, dto, out string error);

        Assert.True(result);
        Assert.Equal(string.Empty, error);
    }

    [Fact]
    public void ToggleStatus_ShouldCallRepo()
    {
        _doctorRepo.Setup(r => r.ToggleStatus(1)).Returns(true);

        var result = _service.ToggleStatus(1);

        Assert.True(result);
    }
}