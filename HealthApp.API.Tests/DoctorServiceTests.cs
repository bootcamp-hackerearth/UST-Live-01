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

public class DoctorServiceTests
{
    private readonly Mock<IDoctorRepository> doctorRepositoryMock = new();
    private readonly Mock<IAppointmentRepository> appointmentRepositoryMock = new();
    private readonly Mock<IHttpContextAccessor> httpContextAccessorMock = new();
    private readonly Mock<IMapper> mapperMock = new();

    private readonly DoctorService doctorService;

    public DoctorServiceTests()
    {
        doctorService = new DoctorService(
            doctorRepositoryMock.Object,
            appointmentRepositoryMock.Object,
            httpContextAccessorMock.Object,
            mapperMock.Object);
    }

    [Fact]
    public async Task GetAllDoctorsAsync_ShouldThrowForbiddenAccessException_WhenUserIsDoctor()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            doctorService.GetAllDoctorsAsync());
    }

    [Fact]
    public async Task GetAllDoctorsAsync_ShouldReturnActiveDoctors_WhenUserIsPatient()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var doctors = new List<Doctor>
        {
            new()
            {
                DoctorId = 1,
                DoctorName = "Dr Sneha Paul",
                Specialisation = SpecialisationType.Cardiologist.ToString(),
                YearsOfExperience = 5,
                ConsultationFee = 800,
                IsActive = true
            }
        };

        var doctorDtos = new List<DoctorDto>
        {
            new()
            {
                DoctorId = 1,
                FullName = "Dr Sneha Paul",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 5,
                ConsultationFee = 800,
                IsActive = true
            }
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetAllActiveAsync())
            .ReturnsAsync(doctors);

        mapperMock
            .Setup(mapper => mapper.Map<List<DoctorDto>>(doctors))
            .Returns(doctorDtos);

        // Act
        var result = await doctorService.GetAllDoctorsAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("Dr Sneha Paul", result[0].FullName);
        Assert.Equal(SpecialisationType.Cardiologist, result[0].Specialisation);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_ShouldThrowBusinessRuleException_WhenDoctorIdInvalid()
    {
        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            doctorService.GetDoctorByIdAsync(0));
    }

    [Fact]
    public async Task GetDoctorByIdAsync_ShouldThrowForbiddenAccessException_WhenDoctorAccessesAnotherDoctorProfile()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var loggedInDoctor = new Doctor
        {
            DoctorId = 1,
            UserId = "doctor-user-id",
            DoctorName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist.ToString(),
            IsActive = true
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(loggedInDoctor);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            doctorService.GetDoctorByIdAsync(2));
    }

    [Fact]
    public async Task GetDoctorByIdAsync_ShouldThrowEntityNotFoundException_WhenDoctorNotFound()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync((Doctor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            doctorService.GetDoctorByIdAsync(1));
    }

    [Fact]
    public async Task GetDoctorByIdAsync_ShouldThrowEntityNotFoundException_WhenDoctorInactive()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var doctor = new Doctor
        {
            DoctorId = 1,
            DoctorName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist.ToString(),
            IsActive = false
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(doctor);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            doctorService.GetDoctorByIdAsync(1));
    }

    [Fact]
    public async Task GetDoctorByIdAsync_ShouldReturnDoctor_WhenPatientRequestsActiveDoctor()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var doctor = new Doctor
        {
            DoctorId = 1,
            DoctorName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist.ToString(),
            YearsOfExperience = 5,
            ConsultationFee = 800,
            IsActive = true
        };

        var doctorDto = new DoctorDto
        {
            DoctorId = 1,
            FullName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist,
            YearsOfExperience = 5,
            ConsultationFee = 800,
            IsActive = true
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(doctor);

        mapperMock
            .Setup(mapper => mapper.Map<DoctorDto>(doctor))
            .Returns(doctorDto);

        // Act
        var result = await doctorService.GetDoctorByIdAsync(1);

        // Assert
        Assert.Equal(1, result.DoctorId);
        Assert.Equal("Dr Sneha Paul", result.FullName);
        Assert.Equal(SpecialisationType.Cardiologist, result.Specialisation);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_ShouldReturnOwnProfile_WhenDoctorRequestsOwnProfile()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var doctor = new Doctor
        {
            DoctorId = 1,
            UserId = "doctor-user-id",
            DoctorName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist.ToString(),
            YearsOfExperience = 5,
            ConsultationFee = 800,
            IsActive = true
        };

        var doctorDto = new DoctorDto
        {
            DoctorId = 1,
            FullName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist,
            YearsOfExperience = 5,
            ConsultationFee = 800,
            IsActive = true
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(doctor);

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(doctor);

        mapperMock
            .Setup(mapper => mapper.Map<DoctorDto>(doctor))
            .Returns(doctorDto);

        // Act
        var result = await doctorService.GetDoctorByIdAsync(1);

        // Assert
        Assert.Equal(1, result.DoctorId);
        Assert.Equal("Dr Sneha Paul", result.FullName);
    }

    [Fact]
    public async Task GetLoggedInDoctorProfileAsync_ShouldThrowForbiddenAccessException_WhenNoLoggedInUser()
    {
        // Arrange
        httpContextAccessorMock
            .Setup(accessor => accessor.HttpContext)
            .Returns(new DefaultHttpContext());

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            doctorService.GetLoggedInDoctorProfileAsync());
    }

    [Fact]
    public async Task GetLoggedInDoctorProfileAsync_ShouldThrowEntityNotFoundException_WhenDoctorNotFoundForUser()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync((Doctor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            doctorService.GetLoggedInDoctorProfileAsync());
    }

    [Fact]
    public async Task GetLoggedInDoctorProfileAsync_ShouldThrowForbiddenAccessException_WhenDoctorInactive()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var doctor = new Doctor
        {
            DoctorId = 1,
            UserId = "doctor-user-id",
            DoctorName = "Dr Sneha Paul",
            IsActive = false
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(doctor);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            doctorService.GetLoggedInDoctorProfileAsync());
    }

    [Fact]
    public async Task GetLoggedInDoctorProfileAsync_ShouldReturnDoctorProfile_WhenDoctorActive()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var doctor = new Doctor
        {
            DoctorId = 1,
            UserId = "doctor-user-id",
            DoctorName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist.ToString(),
            YearsOfExperience = 5,
            ConsultationFee = 800,
            IsActive = true
        };

        var doctorDto = new DoctorDto
        {
            DoctorId = 1,
            FullName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist,
            YearsOfExperience = 5,
            ConsultationFee = 800,
            IsActive = true
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(doctor);

        mapperMock
            .Setup(mapper => mapper.Map<DoctorDto>(doctor))
            .Returns(doctorDto);

        // Act
        var result = await doctorService.GetLoggedInDoctorProfileAsync();

        // Assert
        Assert.Equal(1, result.DoctorId);
        Assert.Equal("Dr Sneha Paul", result.FullName);
    }

    [Fact]
    public async Task GetDoctorsBySpecialisationAsync_ShouldThrowForbiddenAccessException_WhenUserIsDoctor()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            doctorService.GetDoctorsBySpecialisationAsync(SpecialisationType.Cardiologist));
    }

    [Fact]
    public async Task GetDoctorsBySpecialisationAsync_ShouldThrowBusinessRuleException_WhenSpecialisationInvalid()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var invalidSpecialisation = (SpecialisationType)999;

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            doctorService.GetDoctorsBySpecialisationAsync(invalidSpecialisation));
    }

    [Fact]
    public async Task GetDoctorsBySpecialisationAsync_ShouldReturnDoctors_WhenSpecialisationValid()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var doctors = new List<Doctor>
        {
            new()
            {
                DoctorId = 1,
                DoctorName = "Dr Sneha Paul",
                Specialisation = SpecialisationType.Cardiologist.ToString(),
                YearsOfExperience = 5,
                ConsultationFee = 800,
                IsActive = true
            }
        };

        var doctorDtos = new List<DoctorDto>
        {
            new()
            {
                DoctorId = 1,
                FullName = "Dr Sneha Paul",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 5,
                ConsultationFee = 800,
                IsActive = true
            }
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetActiveBySpecialisationAsync(SpecialisationType.Cardiologist))
            .ReturnsAsync(doctors);

        mapperMock
            .Setup(mapper => mapper.Map<List<DoctorDto>>(doctors))
            .Returns(doctorDtos);

        // Act
        var result = await doctorService.GetDoctorsBySpecialisationAsync(SpecialisationType.Cardiologist);

        // Assert
        Assert.Single(result);
        Assert.Equal(SpecialisationType.Cardiologist, result[0].Specialisation);
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_ShouldThrowBusinessRuleException_WhenDoctorIdInvalid()
    {
        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            doctorService.GetDoctorAvailabilityAsync(0, DateTime.Today));
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_ShouldThrowForbiddenAccessException_WhenDoctorRequestsAnotherDoctorAvailability()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var loggedInDoctor = new Doctor
        {
            DoctorId = 1,
            UserId = "doctor-user-id"
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(loggedInDoctor);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            doctorService.GetDoctorAvailabilityAsync(2, DateTime.Today.AddDays(1)));
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_ShouldThrowEntityNotFoundException_WhenDoctorNotFound()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync((Doctor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            doctorService.GetDoctorAvailabilityAsync(1, DateTime.Today.AddDays(1)));
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_ShouldThrowBusinessRuleException_WhenDoctorInactive()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var doctor = new Doctor
        {
            DoctorId = 1,
            DoctorName = "Dr Sneha Paul",
            IsActive = false
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(doctor);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            doctorService.GetDoctorAvailabilityAsync(1, DateTime.Today.AddDays(1)));
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_ShouldThrowBusinessRuleException_WhenDateIsPast()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var doctor = new Doctor
        {
            DoctorId = 1,
            DoctorName = "Dr Sneha Paul",
            IsActive = true
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(doctor);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            doctorService.GetDoctorAvailabilityAsync(1, DateTime.Today.AddDays(-1)));
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_ShouldReturnAvailableSlots_ExcludingBookedSlots()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var date = DateTime.Today.AddDays(1);

        var doctor = new Doctor
        {
            DoctorId = 1,
            DoctorName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist.ToString(),
            IsActive = true
        };

        var appointments = new List<Appointment>
        {
            new()
            {
                AppointmentId = 1,
                DoctorId = 1,
                PatientId = 1,
                ScheduledDate = date.Date,
                TimeSlots = "09:00 AM - 09:30 AM",
                Status = AppointmentStatus.Pending.ToString()
            },
            new()
            {
                AppointmentId = 2,
                DoctorId = 1,
                PatientId = 2,
                ScheduledDate = date.Date,
                TimeSlots = "10:00 AM - 10:30 AM",
                Status = AppointmentStatus.Cancelled.ToString()
            }
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByDoctorIdAsync(1))
            .ReturnsAsync(appointments);

        // Act
        var result = await doctorService.GetDoctorAvailabilityAsync(1, date);

        // Assert
        Assert.Equal(1, result.DoctorId);
        Assert.Equal(date.Date, result.Date);
        Assert.DoesNotContain("09:00 AM - 09:30 AM", result.AvailableSlots);
        Assert.Contains("10:00 AM - 10:30 AM", result.AvailableSlots);
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_ShouldReturnOwnAvailability_WhenLoggedInDoctorRequestsOwnAvailability()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var date = DateTime.Today.AddDays(1);

        var doctor = new Doctor
        {
            DoctorId = 1,
            UserId = "doctor-user-id",
            DoctorName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist.ToString(),
            IsActive = true
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(doctor);

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByDoctorIdAsync(1))
            .ReturnsAsync(new List<Appointment>());

        // Act
        var result = await doctorService.GetDoctorAvailabilityAsync(1, date);

        // Assert
        Assert.Equal(1, result.DoctorId);
        Assert.Equal(date.Date, result.Date);
        Assert.NotEmpty(result.AvailableSlots);
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
    public async Task GetDoctorAvailabilityAsync_WhenTodayContainsPastSlots_DoesNotReturnPastSlots()
    {
        SetCurrentUser("patient-user-id", Roles.Patient);

        var doctor = new Doctor
        {
            DoctorId = 1,
            DoctorName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist.ToString(),
            IsActive = true
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByDoctorIdAsync(1))
            .ReturnsAsync(new List<Appointment>());

        var result = await doctorService.GetDoctorAvailabilityAsync(1, DateTime.Today);

        Assert.All(result.AvailableSlots, slot =>
        {
            var startText = slot.Split('-')[0].Trim();
            var parsed = DateTime.Parse(startText, System.Globalization.CultureInfo.InvariantCulture);
            var slotDateTime = DateTime.Today.Add(parsed.TimeOfDay);

            Assert.True(slotDateTime > DateTime.Now);
        });
    }
}