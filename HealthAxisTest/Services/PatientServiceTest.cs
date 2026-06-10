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

public class PatientServiceTests
{
    private readonly Mock<IPatientRepository> _patientRepo;
    private readonly PatientService _service;

    public PatientServiceTests()
    {
        _patientRepo = new Mock<IPatientRepository>();
        _service = new PatientService(_patientRepo.Object);
    }

    [Fact]
    public void GetAll_ShouldReturnMappedDtos()
    {
        _patientRepo.Setup(r => r.GetAll(null))
            .Returns(new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    FullName = "Test",
                    DateOfBirth = DateTime.Today,
                    Gender = "Male",
                    PhoneNumber = "1234567890",
                    Email = "test@test.com"
                }
            });

        var result = _service.GetAll();

        Assert.NotNull(result);
        Assert.Equal(1, result.Count());
    }

    [Fact]
    public void GetById_ShouldReturnNull_WhenPatientNotFound()
    {
        _patientRepo.Setup(r => r.GetById(1)).Returns((Patient)null);

        var result = _service.GetById(1);

        Assert.Null(result);
    }

    [Fact]
    public void GetById_ShouldReturnDto_WithAppointmentCount()
    {
        _patientRepo.Setup(r => r.GetById(1)).Returns(new Patient
        {
            PatientId = 1,
            FullName = "Test",
            DateOfBirth = DateTime.Today,
            Gender = "Male",
            PhoneNumber = "1234567890",
            Email = "test@test.com"
        });

        _patientRepo.Setup(r => r.GetAppointmentCount(1))
            .Returns(5);

        var result = _service.GetById(1);

        Assert.NotNull(result);
        Assert.Equal(5, result.AppointmentCount);
    }

    [Fact]
    public void Create_ShouldFail_WhenEmailExists()
    {
        _patientRepo.Setup(r => r.GetByEmail("test@test.com"))
            .Returns(new Patient { PatientId = 1 });

        var dto = new PatientDto
        {
            Email = "test@test.com"
        };

        var result = _service.Create(dto, out string error, out int patientId);

        Assert.False(result);
        Assert.Equal("A patient with this email already exists.", error);
        Assert.Equal(0, patientId);
    }

    [Fact]
    public void Create_ShouldSucceed()
    {
        _patientRepo.Setup(r => r.GetByEmail(It.IsAny<string>()))
            .Returns((Patient)null);

        _patientRepo.Setup(r => r.Add(It.IsAny<Patient>()))
            .Returns(new Patient { PatientId = 10 });

        var dto = new PatientDto
        {
            FullName = "Test",
            DateOfBirth = DateTime.Today,
            Gender = GenderEnum.Male,
            PhoneNumber = "1234567890",
            Email = "test@test.com",
            InsuranceID = "INS123"
        };

        var result = _service.Create(dto, out string error, out int patientId);

        Assert.True(result);
        Assert.Equal(10, patientId);
        Assert.Equal(string.Empty, error);
    }

    [Fact]
    public void Update_ShouldFail_WhenDuplicateEmailExists()
    {
        _patientRepo.Setup(r => r.GetByEmail("test@test.com"))
            .Returns(new Patient { PatientId = 2 });

        var dto = new PatientDto
        {
            Email = "test@test.com"
        };

        var result = _service.Update(1, dto, out string error);

        Assert.False(result);
        Assert.Equal("Another patient with this email already exists.", error);
    }

    [Fact]
    public void Update_ShouldFail_WhenPatientNotFound()
    {
        _patientRepo.Setup(r => r.GetByEmail(It.IsAny<string>()))
            .Returns((Patient)null);

        _patientRepo.Setup(r => r.Update(It.IsAny<Patient>()))
            .Returns(false);

        var dto = new PatientDto
        {
            FullName = "Test",
            DateOfBirth = DateTime.Today,
            Gender = GenderEnum.Male,
            PhoneNumber = "1234567890",
            Email = "test@test.com"
        };

        var result = _service.Update(1, dto, out string error);

        Assert.False(result);
        Assert.Equal("Patient not found.", error);
    }

    [Fact]
    public void Update_ShouldSucceed()
    {
        _patientRepo.Setup(r => r.GetByEmail(It.IsAny<string>()))
            .Returns((Patient)null);

        _patientRepo.Setup(r => r.Update(It.IsAny<Patient>()))
            .Returns(true);

        var dto = new PatientDto
        {
            FullName = "Test",
            DateOfBirth = DateTime.Today,
            Gender = GenderEnum.Male,
            PhoneNumber = "1234567890",
            Email = "test@test.com",
            InsuranceID = "INS123"
        };

        var result = _service.Update(1, dto, out string error);

        Assert.True(result);
        Assert.Equal(string.Empty, error);
    }

    [Fact]
    public void Delete_ShouldCallRepo()
    {
        _patientRepo.Setup(r => r.Delete(1)).Returns(true);

        var result = _service.Delete(1);

        Assert.True(result);
    }
}
