using AutoMapper;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace HealthApp.API.Tests;

public class PatientServiceTests
{
    private readonly Mock<IPatientRepository> patientRepositoryMock = new();
    private readonly Mock<IDoctorRepository> doctorRepositoryMock = new();
    private readonly Mock<IAppointmentRepository> appointmentRepositoryMock = new();
    private readonly Mock<IHttpContextAccessor> httpContextAccessorMock = new();
    private readonly Mock<IMapper> mapperMock = new();

    private readonly PatientService patientService;

    public PatientServiceTests()
    {
        patientService = new PatientService(
            patientRepositoryMock.Object,
            doctorRepositoryMock.Object,
            appointmentRepositoryMock.Object,
            httpContextAccessorMock.Object,
            mapperMock.Object);
    }

    [Fact]
    public async Task GetAllPatientsAsync_ShouldReturnMappedPatients()
    {
        var patients = new List<Patient>
        {
            new()
            {
                PatientId = 1,
                PatientName = "Kevin Baby",
                Email = "kevin@gmail.com",
                Gender = GenderType.Male.ToString(),
                PhoneNumber = "9876543210"
            }
        };

        var patientDtos = new List<PatientDto>
        {
            new()
            {
                PatientId = 1,
                FullName = "Kevin Baby",
                Email = "kevin@gmail.com",
                Gender = GenderType.Male,
                PhoneNumber = "9876543210"
            }
        };

        patientRepositoryMock
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(patients);

        mapperMock
            .Setup(mapper => mapper.Map<List<PatientDto>>(patients))
            .Returns(patientDtos);

        var result = await patientService.GetAllPatientsAsync();

        Assert.Single(result);
        Assert.Equal("Kevin Baby", result[0].FullName);
    }

    [Fact]
    public async Task GetPatientByIdAsync_ShouldThrowBusinessRuleException_WhenPatientIdInvalid()
    {
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            patientService.GetPatientByIdAsync(0));
    }

    [Fact]
    public async Task GetPatientByIdAsync_ShouldThrowEntityNotFoundException_WhenPatientDoesNotExist()
    {
        SetCurrentUser("admin-user-id", Roles.Admin);

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            patientService.GetPatientByIdAsync(1));
    }

    [Fact]
    public async Task GetPatientByIdAsync_ShouldReturnPatient_WhenAdminAccessesPatient()
    {
        SetCurrentUser("admin-user-id", Roles.Admin);

        var patient = CreatePatient(1, "patient-user-id");

        var patientDto = new PatientDto
        {
            PatientId = 1,
            FullName = "Kevin Baby",
            Email = "kevin@gmail.com",
            Gender = GenderType.Male,
            PhoneNumber = "9876543210"
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(patient);

        mapperMock
            .Setup(mapper => mapper.Map<PatientDto>(patient))
            .Returns(patientDto);

        var result = await patientService.GetPatientByIdAsync(1);

        Assert.Equal(1, result.PatientId);
        Assert.Equal("Kevin Baby", result.FullName);
    }

    [Fact]
    public async Task GetPatientByIdAsync_ShouldReturnOwnPatient_WhenPatientAccessesOwnProfile()
    {
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = CreatePatient(1, "patient-user-id");

        var patientDto = new PatientDto
        {
            PatientId = 1,
            FullName = "Kevin Baby",
            Email = "kevin@gmail.com",
            Gender = GenderType.Male,
            PhoneNumber = "9876543210"
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(patient);

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        mapperMock
            .Setup(mapper => mapper.Map<PatientDto>(patient))
            .Returns(patientDto);

        var result = await patientService.GetPatientByIdAsync(1);

        Assert.Equal(1, result.PatientId);
        Assert.Equal("Kevin Baby", result.FullName);
    }

    [Fact]
    public async Task GetPatientByIdAsync_ShouldThrowForbiddenAccessException_WhenPatientAccessesAnotherPatient()
    {
        SetCurrentUser("patient-user-id", Roles.Patient);

        var requestedPatient = CreatePatient(2, "another-patient-user-id");

        var loggedInPatient = CreatePatient(1, "patient-user-id");

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(2))
            .ReturnsAsync(requestedPatient);

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(loggedInPatient);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            patientService.GetPatientByIdAsync(2));
    }

    [Fact]
    public async Task GetPatientByIdAsync_ShouldReturnPatient_WhenDoctorHasAppointmentRelation()
    {
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var requestedPatient = CreatePatient(10, "patient-user-id");

        var loggedInDoctor = new Doctor
        {
            DoctorId = 5,
            UserId = "doctor-user-id",
            DoctorName = "Dr Sneha Paul",
            IsActive = true
        };

        var doctorAppointments = new List<Appointment>
        {
            new()
            {
                AppointmentId = 1,
                PatientId = 10,
                DoctorId = 5,
                Status = AppointmentStatus.Confirmed.ToString()
            }
        };

        var patientDto = new PatientDto
        {
            PatientId = 10,
            FullName = "Kevin Baby",
            Email = "kevin@gmail.com",
            Gender = GenderType.Male,
            PhoneNumber = "9876543210"
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(10))
            .ReturnsAsync(requestedPatient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(loggedInDoctor);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByDoctorIdAsync(5))
            .ReturnsAsync(doctorAppointments);

        mapperMock
            .Setup(mapper => mapper.Map<PatientDto>(requestedPatient))
            .Returns(patientDto);

        var result = await patientService.GetPatientByIdAsync(10);

        Assert.Equal(10, result.PatientId);
        Assert.Equal("Kevin Baby", result.FullName);
    }

    [Fact]
    public async Task GetPatientByIdAsync_ShouldThrowForbiddenAccessException_WhenDoctorHasOnlyCancelledAppointmentRelation()
    {
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var requestedPatient = CreatePatient(10, "patient-user-id");

        var loggedInDoctor = new Doctor
        {
            DoctorId = 5,
            UserId = "doctor-user-id",
            DoctorName = "Dr Sneha Paul",
            IsActive = true
        };

        var doctorAppointments = new List<Appointment>
        {
            new()
            {
                AppointmentId = 1,
                PatientId = 10,
                DoctorId = 5,
                Status = AppointmentStatus.Cancelled.ToString()
            }
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(10))
            .ReturnsAsync(requestedPatient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(loggedInDoctor);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByDoctorIdAsync(5))
            .ReturnsAsync(doctorAppointments);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            patientService.GetPatientByIdAsync(10));
    }

    [Fact]
    public async Task GetPatientByIdAsync_ShouldThrowForbiddenAccessException_WhenDoctorHasNoAppointmentRelation()
    {
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var requestedPatient = CreatePatient(10, "patient-user-id");

        var loggedInDoctor = new Doctor
        {
            DoctorId = 5,
            UserId = "doctor-user-id",
            DoctorName = "Dr Sneha Paul",
            IsActive = true
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(10))
            .ReturnsAsync(requestedPatient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(loggedInDoctor);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByDoctorIdAsync(5))
            .ReturnsAsync(new List<Appointment>());

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            patientService.GetPatientByIdAsync(10));
    }

    [Fact]
    public async Task RegisterPatientAsync_ShouldThrowBusinessRuleException_WhenDtoIsNull()
    {
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            patientService.RegisterPatientAsync(null!));
    }

    [Fact]
    public async Task RegisterPatientAsync_ShouldThrowBusinessRuleException_WhenNameMissing()
    {
        var dto = CreateValidCreatePatientDto();
        dto.FullName = "";

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            patientService.RegisterPatientAsync(dto));
    }

    [Fact]
    public async Task RegisterPatientAsync_ShouldThrowBusinessRuleException_WhenDateOfBirthInvalid()
    {
        var dto = CreateValidCreatePatientDto();
        dto.DateOfBirth = DateTime.Today.AddDays(1);

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            patientService.RegisterPatientAsync(dto));
    }

    [Fact]
    public async Task RegisterPatientAsync_ShouldThrowBusinessRuleException_WhenEmailMissing()
    {
        var dto = CreateValidCreatePatientDto();
        dto.Email = "";

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            patientService.RegisterPatientAsync(dto));
    }

    [Fact]
    public async Task RegisterPatientAsync_ShouldThrowBusinessRuleException_WhenPhoneMissing()
    {
        var dto = CreateValidCreatePatientDto();
        dto.PhoneNumber = "";

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            patientService.RegisterPatientAsync(dto));
    }

    [Fact]
    public async Task RegisterPatientAsync_ShouldCreatePatient()
    {
        var dto = CreateValidCreatePatientDto();

        var mappedPatient = new Patient
        {
            PatientName = dto.FullName,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth.Date,
            Gender = dto.Gender.ToString(),
            PhoneNumber = dto.PhoneNumber,
            InsuranceId = dto.InsuranceId
        };

        var savedPatient = new Patient
        {
            PatientId = 1,
            PatientName = dto.FullName,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth.Date,
            Gender = dto.Gender.ToString(),
            PhoneNumber = dto.PhoneNumber,
            InsuranceId = dto.InsuranceId,
            CreatedDate = DateTime.Now
        };

        var patientDto = new PatientDto
        {
            PatientId = 1,
            FullName = dto.FullName,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth.Date,
            Gender = dto.Gender,
            PhoneNumber = dto.PhoneNumber,
            InsuranceId = dto.InsuranceId
        };

        mapperMock
            .Setup(mapper => mapper.Map<Patient>(dto))
            .Returns(mappedPatient);

        patientRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Patient>()))
            .ReturnsAsync(savedPatient);

        mapperMock
            .Setup(mapper => mapper.Map<PatientDto>(savedPatient))
            .Returns(patientDto);

        var result = await patientService.RegisterPatientAsync(dto);

        Assert.Equal(1, result.PatientId);
        Assert.Equal(dto.FullName, result.FullName);

        patientRepositoryMock.Verify(repository =>
            repository.AddAsync(It.Is<Patient>(patient =>
                patient.PatientName == dto.FullName &&
                patient.Email == dto.Email &&
                patient.DateOfBirth == dto.DateOfBirth.Date &&
                patient.CreatedDate != default)),
            Times.Once);
    }

    [Fact]
    public async Task UpdatePatientAsync_ShouldThrowBusinessRuleException_WhenPatientIdInvalid()
    {
        var dto = CreateValidUpdatePatientDto();

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            patientService.UpdatePatientAsync(0, dto));
    }

    [Fact]
    public async Task UpdatePatientAsync_ShouldThrowBusinessRuleException_WhenDtoIsNull()
    {
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            patientService.UpdatePatientAsync(1, null!));
    }

    [Fact]
    public async Task UpdatePatientAsync_ShouldThrowEntityNotFoundException_WhenPatientDoesNotExist()
    {
        SetCurrentUser("admin-user-id", Roles.Admin);

        var dto = CreateValidUpdatePatientDto();

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            patientService.UpdatePatientAsync(1, dto));
    }

    [Fact]
    public async Task UpdatePatientAsync_ShouldThrowForbiddenAccessException_WhenPatientUpdatesAnotherPatient()
    {
        SetCurrentUser("patient-user-id", Roles.Patient);

        var dto = CreateValidUpdatePatientDto();

        var requestedPatient = CreatePatient(2, "another-user-id");

        var loggedInPatient = CreatePatient(1, "patient-user-id");

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(2))
            .ReturnsAsync(requestedPatient);

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(loggedInPatient);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            patientService.UpdatePatientAsync(2, dto));
    }

    [Fact]
    public async Task UpdatePatientAsync_ShouldUpdatePatient_WhenPatientUpdatesOwnProfile()
    {
        SetCurrentUser("patient-user-id", Roles.Patient);

        var dto = CreateValidUpdatePatientDto();

        var existing = CreatePatient(1, "patient-user-id");

        var mappedPatient = new Patient
        {
            PatientName = dto.FullName,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth.Date,
            Gender = dto.Gender.ToString(),
            PhoneNumber = dto.PhoneNumber,
            InsuranceId = dto.InsuranceId
        };

        var updatedPatient = new Patient
        {
            PatientId = 1,
            UserId = existing.UserId,
            PatientName = dto.FullName,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth.Date,
            Gender = dto.Gender.ToString(),
            PhoneNumber = dto.PhoneNumber,
            InsuranceId = dto.InsuranceId,
            CreatedDate = existing.CreatedDate
        };

        var patientDto = new PatientDto
        {
            PatientId = 1,
            FullName = dto.FullName,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth.Date,
            Gender = dto.Gender,
            PhoneNumber = dto.PhoneNumber,
            InsuranceId = dto.InsuranceId
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(existing);

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(existing);

        mapperMock
            .Setup(mapper => mapper.Map<Patient>(dto))
            .Returns(mappedPatient);

        patientRepositoryMock
            .Setup(repository => repository.UpdateAsync(1, It.IsAny<Patient>()))
            .ReturnsAsync(updatedPatient);

        mapperMock
            .Setup(mapper => mapper.Map<PatientDto>(updatedPatient))
            .Returns(patientDto);

        var result = await patientService.UpdatePatientAsync(1, dto);

        Assert.Equal(1, result.PatientId);
        Assert.Equal(dto.FullName, result.FullName);

        patientRepositoryMock.Verify(repository =>
            repository.UpdateAsync(1, It.Is<Patient>(patient =>
                patient.PatientId == 1 &&
                patient.UserId == existing.UserId &&
                patient.CreatedDate == existing.CreatedDate &&
                patient.PatientName == dto.FullName)),
            Times.Once);
    }

    [Fact]
    public async Task EnsurePatientAccessAsync_ShouldThrowBusinessRuleException_WhenPatientIdInvalid()
    {
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            patientService.EnsurePatientAccessAsync(0));
    }

    [Fact]
    public async Task EnsurePatientAccessAsync_ShouldThrowEntityNotFoundException_WhenPatientDoesNotExist()
    {
        SetCurrentUser("admin-user-id", Roles.Admin);

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            patientService.EnsurePatientAccessAsync(1));
    }

    [Fact]
    public async Task EnsurePatientAccessAsync_ShouldAllowAdmin()
    {
        // Arrange
        SetCurrentUser("admin-user-id", Roles.Admin);

        var patient = CreatePatient(1, "patient-user-id");

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(patient);

        // Act
        var exception = await Record.ExceptionAsync(() =>
            patientService.EnsurePatientAccessAsync(1));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task EnsurePatientAccessAsync_ShouldAllowDoctorWithAppointmentRelation()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var patient = CreatePatient(1, "patient-user-id");

        var doctor = new Doctor
        {
            DoctorId = 5,
            UserId = "doctor-user-id"
        };

        var appointments = new List<Appointment>
    {
        new()
        {
            AppointmentId = 11,
            PatientId = 1,
            DoctorId = 5,
            Status = AppointmentStatus.Confirmed.ToString()
        }
    };

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(patient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByDoctorIdAsync(5))
            .ReturnsAsync(appointments);

        // Act
        var exception = await Record.ExceptionAsync(() =>
            patientService.EnsurePatientAccessAsync(1));

        // Assert
        Assert.Null(exception);
    }

    private static Patient CreatePatient(int patientId, string userId)
    {
        return new Patient
        {
            PatientId = patientId,
            UserId = userId,
            PatientName = "Kevin Baby",
            Email = "kevin@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderType.Male.ToString(),
            PhoneNumber = "9876543210",
            InsuranceId = "INS123",
            CreatedDate = new DateTime(2024, 1, 1)
        };
    }

    private static CreatePatientDto CreateValidCreatePatientDto()
    {
        return new CreatePatientDto
        {
            FullName = "Kevin Baby",
            Email = "kevin@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderType.Male,
            PhoneNumber = "9876543210",
            InsuranceId = "INS123"
        };
    }

    private static UpdatePatientDto CreateValidUpdatePatientDto()
    {
        return new UpdatePatientDto
        {
            FullName = "Kevin Baby Updated",
            Email = "kevin.updated@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderType.Male,
            PhoneNumber = "9876543211",
            InsuranceId = "INS456"
        };
    }

    private void SetCurrentUser(string userId, string role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = principal
        };

        httpContextAccessorMock
            .Setup(accessor => accessor.HttpContext)
            .Returns(httpContext);
    }
    [Fact]
    public async Task RegisterPatientAsync_WhenDateOfBirthBeforeMinimumDate_ThrowsBusinessRuleException()
    {
        var dto = CreateValidCreatePatientDto();
        dto.DateOfBirth = new DateTime(1899, 12, 31);

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            patientService.RegisterPatientAsync(dto));
    }

    [Fact]
    public async Task UpdatePatientAsync_WhenDateOfBirthBeforeMinimumDate_ThrowsBusinessRuleException()
    {
        SetCurrentUser("admin-user-id", Roles.Admin);

        var dto = CreateValidUpdatePatientDto();
        dto.DateOfBirth = new DateTime(1899, 12, 31);

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(CreatePatient(1, "patient-user-id"));

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            patientService.UpdatePatientAsync(1, dto));
    }
}