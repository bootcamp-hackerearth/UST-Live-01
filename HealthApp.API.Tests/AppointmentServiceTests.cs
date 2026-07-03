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

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> appointmentRepositoryMock = new();
    private readonly Mock<IPatientRepository> patientRepositoryMock = new();
    private readonly Mock<IDoctorRepository> doctorRepositoryMock = new();
    private readonly Mock<IHttpContextAccessor> httpContextAccessorMock = new();
    private readonly Mock<IMapper> mapperMock = new();

    private readonly AppointmentService appointmentService;

    public AppointmentServiceTests()
    {
        appointmentService = new AppointmentService(
            appointmentRepositoryMock.Object,
            patientRepositoryMock.Object,
            doctorRepositoryMock.Object,
            httpContextAccessorMock.Object,
            mapperMock.Object);
    }

    [Fact]
    public async Task GetAppointmentsAsync_ShouldReturnLoggedInPatientAppointments_WhenUserIsPatient()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = new Patient
        {
            PatientId = 1,
            UserId = "patient-user-id",
            PatientName = "Kevin Baby"
        };

        var appointments = new List<Appointment>
        {
            new()
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlots = "09:00 AM - 09:30 AM",
                Status = AppointmentStatus.Pending.ToString()
            }
        };

        var appointmentDtos = new List<AppointmentDto>
        {
            new()
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "09:00 AM - 09:30 AM",
                Status = AppointmentStatus.Pending
            }
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByPatientIdAsync(1))
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(mapper => mapper.Map<List<AppointmentDto>>(appointments))
            .Returns(appointmentDtos);

        // Act
        var result = await appointmentService.GetAppointmentsAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].PatientId);

        appointmentRepositoryMock.Verify(
            repository => repository.GetByPatientIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetAppointmentsAsync_ShouldThrowForbiddenAccessException_WhenPatientRequestsAnotherPatientAppointments()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = new Patient
        {
            PatientId = 1,
            UserId = "patient-user-id"
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentsAsync(patientId: 2));
    }

    [Fact]
    public async Task GetAppointmentsAsync_ShouldReturnLoggedInDoctorAppointments_WhenUserIsDoctor()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var doctor = new Doctor
        {
            DoctorId = 5,
            UserId = "doctor-user-id",
            DoctorName = "Dr Sneha Paul",
            IsActive = true
        };

        var appointments = new List<Appointment>
        {
            new()
            {
                AppointmentId = 10,
                PatientId = 1,
                DoctorId = 5,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlots = "10:00 AM - 10:30 AM",
                Status = AppointmentStatus.Confirmed.ToString()
            }
        };

        var appointmentDtos = new List<AppointmentDto>
        {
            new()
            {
                AppointmentId = 10,
                PatientId = 1,
                DoctorId = 5,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM - 10:30 AM",
                Status = AppointmentStatus.Confirmed
            }
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByDoctorIdAsync(5))
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(mapper => mapper.Map<List<AppointmentDto>>(appointments))
            .Returns(appointmentDtos);

        // Act
        var result = await appointmentService.GetAppointmentsAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(5, result[0].DoctorId);
    }

    [Fact]
    public async Task GetAppointmentsAsync_ShouldThrowForbiddenAccessException_WhenDoctorRequestsAnotherDoctorAppointments()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var doctor = new Doctor
        {
            DoctorId = 5,
            UserId = "doctor-user-id"
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(doctor);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentsAsync(doctorId: 99));
    }

    [Fact]
    public async Task GetAllAppointmentsAsync_ShouldReturnAllAppointments_WhenUserIsAdmin()
    {
        // Arrange
        SetCurrentUser("admin-user-id", Roles.Admin);

        var appointments = new List<Appointment>
        {
            new()
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today,
                TimeSlots = "09:00 AM - 09:30 AM",
                Status = AppointmentStatus.Pending.ToString()
            },
            new()
            {
                AppointmentId = 2,
                PatientId = 2,
                DoctorId = 2,
                ScheduledDate = DateTime.Today,
                TimeSlots = "10:00 AM - 10:30 AM",
                Status = AppointmentStatus.Completed.ToString()
            }
        };

        var appointmentDtos = new List<AppointmentDto>
        {
            new()
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1,
                Status = AppointmentStatus.Pending
            },
            new()
            {
                AppointmentId = 2,
                PatientId = 2,
                DoctorId = 2,
                Status = AppointmentStatus.Completed
            }
        };

        appointmentRepositoryMock
            .Setup(repository => repository.GetAllWithDetailsAsync())
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(mapper => mapper.Map<List<AppointmentDto>>(appointments))
            .Returns(appointmentDtos);

        // Act
        var result = await appointmentService.GetAllAppointmentsAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAppointmentByIdAsync_ShouldThrowAppointmentRuleException_WhenIdInvalid()
    {
        // Act & Assert
        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.GetAppointmentByIdAsync(0));
    }

    [Fact]
    public async Task GetAppointmentByIdAsync_ShouldThrowEntityNotFoundException_WhenAppointmentNotFound()
    {
        // Arrange
        SetCurrentUser("admin-user-id", Roles.Admin);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByIdWithDetailsAsync(1))
            .ReturnsAsync((Appointment?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            appointmentService.GetAppointmentByIdAsync(1));
    }

    [Fact]
    public async Task GetAppointmentByIdAsync_ShouldThrowForbiddenAccessException_WhenDoctorAccessesAnotherDoctorAppointment()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var doctor = new Doctor
        {
            DoctorId = 5,
            UserId = "doctor-user-id"
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 99,
            ScheduledDate = DateTime.Today,
            TimeSlots = "09:00 AM - 09:30 AM",
            Status = AppointmentStatus.Pending.ToString()
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(appointment);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentByIdAsync(1));
    }

    [Fact]
    public async Task BookAppointmentAsync_ShouldThrowAppointmentRuleException_WhenDtoIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.BookAppointmentAsync(null!));
    }

    [Fact]
    public async Task BookAppointmentAsync_ShouldThrowAppointmentRuleException_WhenDoctorIsInactive()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = new Patient
        {
            PatientId = 1,
            UserId = "patient-user-id"
        };

        var doctor = new Doctor
        {
            DoctorId = 2,
            IsActive = false
        };

        var dto = new BookAppointmentDto
        {
            DoctorId = 2,
            ScheduledDate = DateTime.Today.AddDays(1),
            TimeSlot = "09:00 AM - 09:30 AM"
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(2))
            .ReturnsAsync(doctor);

        // Act & Assert
        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointmentAsync_ShouldThrowAppointmentRuleException_WhenDateIsInPast()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = new Patient
        {
            PatientId = 1,
            UserId = "patient-user-id"
        };

        var doctor = new Doctor
        {
            DoctorId = 2,
            IsActive = true
        };

        var dto = new BookAppointmentDto
        {
            DoctorId = 2,
            ScheduledDate = DateTime.Today.AddDays(-1),
            TimeSlot = "09:00 AM - 09:30 AM"
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(2))
            .ReturnsAsync(doctor);

        // Act & Assert
        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointmentAsync_ShouldThrowAppointmentRuleException_WhenSlotIsInvalid()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = new Patient
        {
            PatientId = 1,
            UserId = "patient-user-id"
        };

        var doctor = new Doctor
        {
            DoctorId = 2,
            IsActive = true
        };

        var dto = new BookAppointmentDto
        {
            DoctorId = 2,
            ScheduledDate = DateTime.Today.AddDays(1),
            TimeSlot = "Invalid Slot"
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(2))
            .ReturnsAsync(doctor);

        // Act & Assert
        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointmentAsync_ShouldThrowConflictException_WhenSlotAlreadyBooked()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = new Patient
        {
            PatientId = 1,
            UserId = "patient-user-id"
        };

        var doctor = new Doctor
        {
            DoctorId = 2,
            IsActive = true
        };

        var dto = new BookAppointmentDto
        {
            DoctorId = 2,
            ScheduledDate = DateTime.Today.AddDays(1),
            TimeSlot = "09:00 AM - 09:30 AM"
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(2))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(repository => repository.IsSlotBookedAsync(
                2,
                dto.ScheduledDate.Date,
                dto.TimeSlot))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() =>
            appointmentService.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointmentAsync_ShouldCreateAppointment_WhenRequestIsValid()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = new Patient
        {
            PatientId = 1,
            UserId = "patient-user-id"
        };

        var doctor = new Doctor
        {
            DoctorId = 2,
            IsActive = true
        };

        var dto = new BookAppointmentDto
        {
            DoctorId = 2,
            ScheduledDate = DateTime.Today.AddDays(1),
            TimeSlot = "09:00 AM - 09:30 AM"
        };

        var mappedAppointment = new Appointment();

        var savedAppointment = new Appointment
        {
            AppointmentId = 10,
            PatientId = 1,
            DoctorId = 2,
            ScheduledDate = dto.ScheduledDate.Date,
            TimeSlots = dto.TimeSlot,
            Status = AppointmentStatus.Pending.ToString()
        };

        var appointmentDto = new AppointmentDto
        {
            AppointmentId = 10,
            PatientId = 1,
            DoctorId = 2,
            ScheduledDate = dto.ScheduledDate.Date,
            TimeSlot = dto.TimeSlot,
            Status = AppointmentStatus.Pending
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(2))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(repository => repository.IsSlotBookedAsync(
                2,
                dto.ScheduledDate.Date,
                dto.TimeSlot))
            .ReturnsAsync(false);

        appointmentRepositoryMock
            .Setup(repository => repository.PatientHasActiveAppointmentOnDateAndSlotAsync(
                1,
                dto.ScheduledDate.Date,
                dto.TimeSlot))
            .ReturnsAsync(false);

        appointmentRepositoryMock
            .Setup(repository => repository.PatientHasActiveAppointmentWithDoctorOnDateAsync(
                1,
                2,
                dto.ScheduledDate.Date))
            .ReturnsAsync(false);

        mapperMock
            .Setup(mapper => mapper.Map<Appointment>(dto))
            .Returns(mappedAppointment);

        appointmentRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Appointment>()))
            .ReturnsAsync(savedAppointment);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByIdWithDetailsAsync(10))
            .ReturnsAsync(savedAppointment);

        mapperMock
            .Setup(mapper => mapper.Map<AppointmentDto>(savedAppointment))
            .Returns(appointmentDto);

        // Act
        var result = await appointmentService.BookAppointmentAsync(dto);

        // Assert
        Assert.Equal(10, result.AppointmentId);
        Assert.Equal(AppointmentStatus.Pending, result.Status);

        appointmentRepositoryMock.Verify(repository =>
            repository.AddAsync(It.Is<Appointment>(appointment =>
                appointment.PatientId == 1 &&
                appointment.DoctorId == 2 &&
                appointment.ScheduledDate == dto.ScheduledDate.Date &&
                appointment.TimeSlots == dto.TimeSlot &&
                appointment.Status == AppointmentStatus.Pending.ToString())),
            Times.Once);
    }

    [Fact]
    public async Task ChangeAppointmentStatusAsync_ShouldThrowAppointmentRuleException_WhenDtoIsNull()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        // Act & Assert
        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.ChangeAppointmentStatusAsync(1, null!));
    }

    [Fact]
    public async Task ChangeAppointmentStatusAsync_ShouldThrowForbiddenAccessException_WhenPatientTriesToConfirm()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = new Patient
        {
            PatientId = 1,
            UserId = "patient-user-id"
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 2,
            Status = AppointmentStatus.Pending.ToString()
        };

        var dto = new UpdateAppointmentStatusDto
        {
            Status = AppointmentStatus.Confirmed
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(appointment);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.ChangeAppointmentStatusAsync(1, dto));
    }

    [Fact]
    public async Task ChangeAppointmentStatusAsync_ShouldConfirmAppointment_WhenDoctorOwnsPendingAppointment()
    {
        // Arrange
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var doctor = new Doctor
        {
            DoctorId = 2,
            UserId = "doctor-user-id"
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 2,
            Status = AppointmentStatus.Pending.ToString()
        };

        var updatedAppointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 2,
            Status = AppointmentStatus.Confirmed.ToString()
        };

        var appointmentDto = new AppointmentDto
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 2,
            Status = AppointmentStatus.Confirmed
        };

        var dto = new UpdateAppointmentStatusDto
        {
            Status = AppointmentStatus.Confirmed
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .SetupSequence(repository => repository.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(appointment)
            .ReturnsAsync(updatedAppointment);

        appointmentRepositoryMock
            .Setup(repository => repository.UpdateAsync(1, It.IsAny<Appointment>()))
            .ReturnsAsync(updatedAppointment);

        mapperMock
            .Setup(mapper => mapper.Map<AppointmentDto>(updatedAppointment))
            .Returns(appointmentDto);

        // Act
        var result = await appointmentService.ChangeAppointmentStatusAsync(1, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(AppointmentStatus.Confirmed, result.Status);

        appointmentRepositoryMock.Verify(repository =>
            repository.UpdateAsync(1, It.Is<Appointment>(updated =>
                updated.Status == AppointmentStatus.Confirmed.ToString())),
            Times.Once);
    }

    [Fact]
    public async Task CancelAppointmentAsync_ShouldThrowAppointmentRuleException_WhenReasonMissing()
    {
        // Act & Assert
        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.CancelAppointmentAsync(1, ""));
    }

    [Fact]
    public async Task CancelAppointmentAsync_ShouldCancelAppointment_WhenPatientOwnsAppointment()
    {
        // Arrange
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = new Patient
        {
            PatientId = 1,
            UserId = "patient-user-id"
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 2,
            Status = AppointmentStatus.Pending.ToString()
        };

        var updatedAppointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 2,
            Status = AppointmentStatus.Cancelled.ToString(),
            CancellationReason = "Patient unavailable"
        };

        var appointmentDto = new AppointmentDto
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 2,
            Status = AppointmentStatus.Cancelled,
            CancellationReason = "Patient unavailable"
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        appointmentRepositoryMock
            .SetupSequence(repository => repository.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(appointment)
            .ReturnsAsync(updatedAppointment);

        appointmentRepositoryMock
            .Setup(repository => repository.UpdateAsync(1, It.IsAny<Appointment>()))
            .ReturnsAsync(updatedAppointment);

        mapperMock
            .Setup(mapper => mapper.Map<AppointmentDto>(updatedAppointment))
            .Returns(appointmentDto);

        // Act
        var result = await appointmentService.CancelAppointmentAsync(
            1,
            "Patient unavailable");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(AppointmentStatus.Cancelled, result.Status);
        Assert.Equal("Patient unavailable", result.CancellationReason);

        appointmentRepositoryMock.Verify(repository =>
            repository.UpdateAsync(1, It.Is<Appointment>(updated =>
                updated.Status == AppointmentStatus.Cancelled.ToString() &&
                updated.CancellationReason == "Patient unavailable")),
            Times.Once);
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
    public async Task GetAppointmentsAsync_WhenAdminWithoutFilters_ReturnsAllAppointments()
    {
        SetCurrentUser("admin-user-id", Roles.Admin);

        var appointments = new List<Appointment>
    {
        new()
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            ScheduledDate = DateTime.Today.AddDays(1),
            TimeSlots = "09:00 AM - 09:30 AM",
            Status = AppointmentStatus.Pending.ToString()
        }
    };

        var appointmentDtos = new List<AppointmentDto>
    {
        new()
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            ScheduledDate = DateTime.Today.AddDays(1),
            TimeSlot = "09:00 AM - 09:30 AM",
            Status = AppointmentStatus.Pending
        }
    };

        appointmentRepositoryMock
            .Setup(repository => repository.GetAllWithDetailsAsync())
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(mapper => mapper.Map<List<AppointmentDto>>(appointments))
            .Returns(appointmentDtos);

        var result = await appointmentService.GetAppointmentsAsync();

        Assert.Single(result);
        Assert.Equal(1, result[0].AppointmentId);
    }

    [Fact]
    public async Task GetAppointmentsAsync_WhenAdminFiltersByPatient_ReturnsPatientAppointments()
    {
        SetCurrentUser("admin-user-id", Roles.Admin);

        var patient = new Patient
        {
            PatientId = 10,
            PatientName = "Kevin Baby"
        };

        var appointments = new List<Appointment>
    {
        new()
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Pending.ToString()
        }
    };

        var appointmentDtos = new List<AppointmentDto>
    {
        new()
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Pending
        }
    };

        patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(10))
            .ReturnsAsync(patient);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByPatientIdAsync(10))
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(mapper => mapper.Map<List<AppointmentDto>>(appointments))
            .Returns(appointmentDtos);

        var result = await appointmentService.GetAppointmentsAsync(patientId: 10);

        Assert.Single(result);
        Assert.Equal(10, result[0].PatientId);
    }

    [Fact]
    public async Task GetAppointmentsAsync_WhenAdminFiltersByDoctor_ReturnsDoctorAppointments()
    {
        SetCurrentUser("admin-user-id", Roles.Admin);

        var doctor = new Doctor
        {
            DoctorId = 5,
            DoctorName = "Dr Sneha Paul",
            IsActive = true
        };

        var appointments = new List<Appointment>
    {
        new()
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Confirmed.ToString()
        }
    };

        var appointmentDtos = new List<AppointmentDto>
    {
        new()
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Confirmed
        }
    };

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(5))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByDoctorIdAsync(5))
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(mapper => mapper.Map<List<AppointmentDto>>(appointments))
            .Returns(appointmentDtos);

        var result = await appointmentService.GetAppointmentsAsync(doctorId: 5);

        Assert.Single(result);
        Assert.Equal(5, result[0].DoctorId);
    }

    [Fact]
    public async Task GetAppointmentsByDoctorIdAsync_WhenPatientRequestsDoctorAppointments_ThrowsForbidden()
    {
        SetCurrentUser("patient-user-id", Roles.Patient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(5))
            .ReturnsAsync(new Doctor
            {
                DoctorId = 5,
                DoctorName = "Dr Sneha Paul",
                IsActive = true
            });

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentsByDoctorIdAsync(5));
    }

    [Fact]
    public async Task GetAppointmentsByStatusAsync_WhenPatientRequestsStatus_ReturnsOwnFilteredAppointments()
    {
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = new Patient
        {
            PatientId = 10,
            UserId = "patient-user-id"
        };

        var appointments = new List<Appointment>
    {
        new()
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Pending.ToString()
        },
        new()
        {
            AppointmentId = 2,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Completed.ToString()
        }
    };

        var appointmentDtos = new List<AppointmentDto>
    {
        new()
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Pending
        }
    };

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByPatientIdAsync(10))
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(mapper => mapper.Map<List<AppointmentDto>>(
                It.Is<List<Appointment>>(items => items.Count == 1)))
            .Returns(appointmentDtos);

        var result = await appointmentService.GetAppointmentsByStatusAsync(AppointmentStatus.Pending);

        Assert.Single(result);
        Assert.Equal(AppointmentStatus.Pending, result[0].Status);
    }

    [Fact]
    public async Task GetAppointmentsByStatusAsync_WhenDoctorRequestsStatus_ReturnsOwnFilteredAppointments()
    {
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var doctor = new Doctor
        {
            DoctorId = 5,
            UserId = "doctor-user-id"
        };

        var appointments = new List<Appointment>
    {
        new()
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Confirmed.ToString()
        },
        new()
        {
            AppointmentId = 2,
            PatientId = 11,
            DoctorId = 5,
            Status = AppointmentStatus.Cancelled.ToString()
        }
    };

        var appointmentDtos = new List<AppointmentDto>
    {
        new()
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Confirmed
        }
    };

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByDoctorIdAsync(5))
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(mapper => mapper.Map<List<AppointmentDto>>(
                It.Is<List<Appointment>>(items => items.Count == 1)))
            .Returns(appointmentDtos);

        var result = await appointmentService.GetAppointmentsByStatusAsync(AppointmentStatus.Confirmed);

        Assert.Single(result);
        Assert.Equal(AppointmentStatus.Confirmed, result[0].Status);
    }

    [Fact]
    public async Task GetAppointmentsByStatusAsync_WhenAdminRequestsStatus_ReturnsAllMatchingAppointments()
    {
        SetCurrentUser("admin-user-id", Roles.Admin);

        var appointments = new List<Appointment>
    {
        new()
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Cancelled.ToString()
        }
    };

        var appointmentDtos = new List<AppointmentDto>
    {
        new()
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Cancelled
        }
    };

        appointmentRepositoryMock
            .Setup(repository => repository.GetByStatusAsync(AppointmentStatus.Cancelled))
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(mapper => mapper.Map<List<AppointmentDto>>(appointments))
            .Returns(appointmentDtos);

        var result = await appointmentService.GetAppointmentsByStatusAsync(AppointmentStatus.Cancelled);

        Assert.Single(result);
        Assert.Equal(AppointmentStatus.Cancelled, result[0].Status);
    }

    [Fact]
    public async Task BookAppointmentAsync_WhenPatientAlreadyHasSameSlot_ThrowsConflictException()
    {
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = new Patient
        {
            PatientId = 10,
            UserId = "patient-user-id"
        };

        var doctor = new Doctor
        {
            DoctorId = 5,
            IsActive = true
        };

        var dto = new BookAppointmentDto
        {
            DoctorId = 5,
            ScheduledDate = DateTime.Today.AddDays(1),
            TimeSlot = "09:00 AM - 09:30 AM"
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(5))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(repository => repository.IsSlotBookedAsync(5, dto.ScheduledDate.Date, dto.TimeSlot))
            .ReturnsAsync(false);

        appointmentRepositoryMock
            .Setup(repository => repository.PatientHasActiveAppointmentOnDateAndSlotAsync(10, dto.ScheduledDate.Date, dto.TimeSlot))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ConflictException>(() =>
            appointmentService.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointmentAsync_WhenPatientAlreadyHasActiveAppointmentWithDoctorOnDate_ThrowsConflictException()
    {
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = new Patient
        {
            PatientId = 10,
            UserId = "patient-user-id"
        };

        var doctor = new Doctor
        {
            DoctorId = 5,
            IsActive = true
        };

        var dto = new BookAppointmentDto
        {
            DoctorId = 5,
            ScheduledDate = DateTime.Today.AddDays(1),
            TimeSlot = "09:00 AM - 09:30 AM"
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(5))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(repository => repository.IsSlotBookedAsync(5, dto.ScheduledDate.Date, dto.TimeSlot))
            .ReturnsAsync(false);

        appointmentRepositoryMock
            .Setup(repository => repository.PatientHasActiveAppointmentOnDateAndSlotAsync(10, dto.ScheduledDate.Date, dto.TimeSlot))
            .ReturnsAsync(false);

        appointmentRepositoryMock
            .Setup(repository => repository.PatientHasActiveAppointmentWithDoctorOnDateAsync(10, 5, dto.ScheduledDate.Date))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ConflictException>(() =>
            appointmentService.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task ChangeAppointmentStatusAsync_WhenAdminUpdatesStatus_ThrowsForbidden()
    {
        SetCurrentUser("admin-user-id", Roles.Admin);

        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Pending.ToString()
        };

        appointmentRepositoryMock
            .Setup(repository => repository.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(appointment);

        var dto = new UpdateAppointmentStatusDto
        {
            Status = AppointmentStatus.Confirmed
        };

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.ChangeAppointmentStatusAsync(1, dto));
    }

    [Fact]
    public async Task ChangeAppointmentStatusAsync_WhenAppointmentAlreadyCompleted_ThrowsAppointmentRuleException()
    {
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var doctor = new Doctor
        {
            DoctorId = 5,
            UserId = "doctor-user-id"
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Completed.ToString()
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(appointment);

        var dto = new UpdateAppointmentStatusDto
        {
            Status = AppointmentStatus.Cancelled,
            CancellationReason = "Not available"
        };

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.ChangeAppointmentStatusAsync(1, dto));
    }

    [Fact]
    public async Task ChangeAppointmentStatusAsync_WhenCancellationReasonMissing_ThrowsAppointmentRuleException()
    {
        SetCurrentUser("patient-user-id", Roles.Patient);

        var patient = new Patient
        {
            PatientId = 10,
            UserId = "patient-user-id"
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Pending.ToString()
        };

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("patient-user-id"))
            .ReturnsAsync(patient);

        appointmentRepositoryMock
            .Setup(repository => repository.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(appointment);

        var dto = new UpdateAppointmentStatusDto
        {
            Status = AppointmentStatus.Cancelled,
            CancellationReason = ""
        };

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.ChangeAppointmentStatusAsync(1, dto));
    }

    [Fact]
    public async Task ChangeAppointmentStatusAsync_WhenDoctorCompletesConfirmedAppointment_ReturnsCompleted()
    {
        SetCurrentUser("doctor-user-id", Roles.Doctor);

        var doctor = new Doctor
        {
            DoctorId = 5,
            UserId = "doctor-user-id"
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Confirmed.ToString()
        };

        var updatedAppointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Completed.ToString()
        };

        var appointmentDto = new AppointmentDto
        {
            AppointmentId = 1,
            PatientId = 10,
            DoctorId = 5,
            Status = AppointmentStatus.Completed
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync("doctor-user-id"))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .SetupSequence(repository => repository.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(appointment)
            .ReturnsAsync(updatedAppointment);

        appointmentRepositoryMock
            .Setup(repository => repository.UpdateAsync(1, It.IsAny<Appointment>()))
            .ReturnsAsync(updatedAppointment);

        mapperMock
            .Setup(mapper => mapper.Map<AppointmentDto>(updatedAppointment))
            .Returns(appointmentDto);

        var dto = new UpdateAppointmentStatusDto
        {
            Status = AppointmentStatus.Completed
        };

        var result = await appointmentService.ChangeAppointmentStatusAsync(1, dto);

        Assert.Equal(AppointmentStatus.Completed, result.Status);
    }
}