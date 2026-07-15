using System.Security.Claims;
using AutoMapper;
using HealthApp.API.Events;
using HealthApp.API.Exceptions;
using HealthApp.Shared.DTOs;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.Constants;
using HealthApp.Shared.Enums;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using HealthApp.API.Models;

namespace HealthApp.API.Tests;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> appointmentRepositoryMock = new();
    private readonly Mock<IPatientRepository> patientRepositoryMock = new();
    private readonly Mock<IDoctorRepository> doctorRepositoryMock = new();
    private readonly Mock<IDoctorLeaveRepository> doctorLeaveRepositoryMock = new();
    private readonly Mock<IHttpContextAccessor> httpContextAccessorMock = new();
    private readonly Mock<IDistributedCache> distributedCacheMock = new();
    private readonly Mock<IMapper> mapperMock = new();
    private readonly Mock<IPublishEndpoint> publishEndpointMock = new();
    private readonly Mock<ILogger<AppointmentService>> loggerMock = new();

    private readonly AppointmentService appointmentService;

    public AppointmentServiceTests()
    {
        appointmentService = new AppointmentService(
            appointmentRepositoryMock.Object,
            patientRepositoryMock.Object,
            doctorRepositoryMock.Object,
            doctorLeaveRepositoryMock.Object,
            httpContextAccessorMock.Object,
            distributedCacheMock.Object,
            mapperMock.Object,
            publishEndpointMock.Object,
            loggerMock.Object);
    }
    // ---------------------------------------------------------------------
    // GetAppointmentsByStatusAsync
    // ---------------------------------------------------------------------

    [Theory]
    [InlineData(Roles.Patient)]
    [InlineData(Roles.Doctor)]
    public async Task GetAppointmentsByStatus_UserRole_FiltersAppointments(
        string role)
    {
        SetCurrentUser(role);

        var matchingAppointment = CreateAppointment(
            appointmentId: 1,
            status: AppointmentStatus.Pending);

        var otherAppointment = CreateAppointment(
            appointmentId: 2,
            status: AppointmentStatus.Completed);

        var appointments = new List<Appointment>
    {
        matchingAppointment,
        otherAppointment
    };

        var expected = new List<AppointmentDto>
    {
        new()
    };

        if (role == Roles.Patient)
        {
            var patient = CreatePatient(
                patientId: matchingAppointment.PatientId);

            patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync("user-1"))
                .ReturnsAsync(patient);

            appointmentRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(patient.PatientId))
                .ReturnsAsync(appointments);
        }
        else
        {
            var doctor = CreateDoctor(
                doctorId: matchingAppointment.DoctorId);

            doctorRepositoryMock
                .Setup(x => x.GetByUserIdAsync("user-1"))
                .ReturnsAsync(doctor);

            appointmentRepositoryMock
                .Setup(x => x.GetByDoctorIdAsync(doctor.DoctorId))
                .ReturnsAsync(appointments);
        }

        mapperMock
            .Setup(x => x.Map<List<AppointmentDto>>(
                It.Is<List<Appointment>>(list =>
                    list.Count == 1 &&
                    list[0].Status == AppointmentStatus.Pending.ToString())))
            .Returns(expected);

        var result = await appointmentService.GetAppointmentsByStatusAsync(
            AppointmentStatus.Pending);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentsByStatus_Admin_ReturnsRepositoryResult()
    {
        SetCurrentUser(Roles.Admin);

        var appointments = new List<Appointment>
    {
        CreateAppointment(status: AppointmentStatus.Pending)
    };

        var expected = new List<AppointmentDto>
    {
        new()
    };

        appointmentRepositoryMock
            .Setup(x => x.GetByStatusAsync(AppointmentStatus.Pending))
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(x => x.Map<List<AppointmentDto>>(appointments))
            .Returns(expected);

        var result = await appointmentService.GetAppointmentsByStatusAsync(
            AppointmentStatus.Pending);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentsByStatus_UnknownRole_ThrowsForbidden()
    {
        SetCurrentUser("Unknown");

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentsByStatusAsync(
                AppointmentStatus.Pending));
    }

    // ---------------------------------------------------------------------
    // GetAppointmentsByPatientIdAsync
    // ---------------------------------------------------------------------

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_InvalidPatientId_ThrowsRuleException()
    {
        SetCurrentUser(Roles.Admin);

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.GetAppointmentsByPatientIdAsync(0));
    }

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_MissingPatient_ThrowsNotFound()
    {
        SetCurrentUser(Roles.Admin);

        patientRepositoryMock
            .Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            appointmentService.GetAppointmentsByPatientIdAsync(99));
    }

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_PatientReturnsOwnAppointments()
    {
        SetCurrentUser(Roles.Patient);

        var patient = CreatePatient(patientId: 10);
        var appointments = new List<Appointment>
    {
        CreateAppointment(patientId: patient.PatientId)
    };

        var expected = new List<AppointmentDto>
    {
        new()
    };

        patientRepositoryMock
            .Setup(x => x.GetByIdAsync(patient.PatientId))
            .ReturnsAsync(patient);

        patientRepositoryMock
            .Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync(patient);

        appointmentRepositoryMock
            .Setup(x => x.GetByPatientIdAsync(patient.PatientId))
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(x => x.Map<List<AppointmentDto>>(appointments))
            .Returns(expected);

        var result = await appointmentService.GetAppointmentsByPatientIdAsync(
            patient.PatientId);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_PatientRequestsAnotherPatient_ThrowsForbidden()
    {
        SetCurrentUser(Roles.Patient);

        patientRepositoryMock
            .Setup(x => x.GetByIdAsync(20))
            .ReturnsAsync(CreatePatient(patientId: 20));

        patientRepositoryMock
            .Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync(CreatePatient(patientId: 10));

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentsByPatientIdAsync(20));
    }

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_DoctorReturnsOnlyOwnPatientAppointments()
    {
        SetCurrentUser(Roles.Doctor);

        var doctor = CreateDoctor(doctorId: 20);
        var patient = CreatePatient(patientId: 10);

        var doctorAppointments = new List<Appointment>
    {
        CreateAppointment(
            appointmentId: 1,
            patientId: patient.PatientId,
            doctorId: doctor.DoctorId),

        CreateAppointment(
            appointmentId: 2,
            patientId: 30,
            doctorId: doctor.DoctorId)
    };

        var expected = new List<AppointmentDto>
    {
        new()
    };

        patientRepositoryMock
            .Setup(x => x.GetByIdAsync(patient.PatientId))
            .ReturnsAsync(patient);

        doctorRepositoryMock
            .Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(x => x.GetByDoctorIdAsync(doctor.DoctorId))
            .ReturnsAsync(doctorAppointments);

        mapperMock
            .Setup(x => x.Map<List<AppointmentDto>>(
                It.Is<List<Appointment>>(list =>
                    list.Count == 1 &&
                    list[0].PatientId == patient.PatientId)))
            .Returns(expected);

        var result = await appointmentService.GetAppointmentsByPatientIdAsync(
            patient.PatientId);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_AdminReturnsPatientAppointments()
    {
        SetCurrentUser(Roles.Admin);

        var patient = CreatePatient();
        var appointments = new List<Appointment>
    {
        CreateAppointment(patientId: patient.PatientId)
    };

        var expected = new List<AppointmentDto>
    {
        new()
    };

        patientRepositoryMock
            .Setup(x => x.GetByIdAsync(patient.PatientId))
            .ReturnsAsync(patient);

        appointmentRepositoryMock
            .Setup(x => x.GetByPatientIdAsync(patient.PatientId))
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(x => x.Map<List<AppointmentDto>>(appointments))
            .Returns(expected);

        var result = await appointmentService.GetAppointmentsByPatientIdAsync(
            patient.PatientId);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_UnknownRole_ThrowsForbidden()
    {
        SetCurrentUser("Unknown");

        var patient = CreatePatient();

        patientRepositoryMock
            .Setup(x => x.GetByIdAsync(patient.PatientId))
            .ReturnsAsync(patient);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentsByPatientIdAsync(
                patient.PatientId));
    }

    // ---------------------------------------------------------------------
    // GetAppointmentsByDoctorIdAsync
    // ---------------------------------------------------------------------

    [Fact]
    public async Task GetAppointmentsByDoctorIdAsync_InvalidDoctorId_ThrowsRuleException()
    {
        SetCurrentUser(Roles.Admin);

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.GetAppointmentsByDoctorIdAsync(0));
    }

    [Fact]
    public async Task GetAppointmentsByDoctorIdAsync_MissingDoctor_ThrowsNotFound()
    {
        SetCurrentUser(Roles.Admin);

        doctorRepositoryMock
            .Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            appointmentService.GetAppointmentsByDoctorIdAsync(99));
    }

    [Fact]
    public async Task GetAppointmentsByDoctorIdAsync_Patient_ThrowsForbidden()
    {
        SetCurrentUser(Roles.Patient);

        var doctor = CreateDoctor();

        doctorRepositoryMock
            .Setup(x => x.GetByIdAsync(doctor.DoctorId))
            .ReturnsAsync(doctor);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentsByDoctorIdAsync(
                doctor.DoctorId));
    }

    [Fact]
    public async Task GetAppointmentsByDoctorIdAsync_DoctorReturnsOwnAppointments()
    {
        SetCurrentUser(Roles.Doctor);

        var doctor = CreateDoctor();
        var appointments = new List<Appointment>
    {
        CreateAppointment(doctorId: doctor.DoctorId)
    };

        var expected = new List<AppointmentDto>
    {
        new()
    };

        doctorRepositoryMock
            .Setup(x => x.GetByIdAsync(doctor.DoctorId))
            .ReturnsAsync(doctor);

        doctorRepositoryMock
            .Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(x => x.GetByDoctorIdAsync(doctor.DoctorId))
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(x => x.Map<List<AppointmentDto>>(appointments))
            .Returns(expected);

        var result = await appointmentService.GetAppointmentsByDoctorIdAsync(
            doctor.DoctorId);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentsByDoctorIdAsync_DoctorRequestsAnotherDoctor_ThrowsForbidden()
    {
        SetCurrentUser(Roles.Doctor);

        var requestedDoctor = CreateDoctor(doctorId: 20);
        var loggedInDoctor = CreateDoctor(doctorId: 30);

        doctorRepositoryMock
            .Setup(x => x.GetByIdAsync(requestedDoctor.DoctorId))
            .ReturnsAsync(requestedDoctor);

        doctorRepositoryMock
            .Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync(loggedInDoctor);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentsByDoctorIdAsync(
                requestedDoctor.DoctorId));
    }

    [Fact]
    public async Task GetAppointmentsByDoctorIdAsync_AdminReturnsAppointments()
    {
        SetCurrentUser(Roles.Admin);

        var doctor = CreateDoctor();
        var appointments = new List<Appointment>
    {
        CreateAppointment(doctorId: doctor.DoctorId)
    };

        var expected = new List<AppointmentDto>
    {
        new()
    };

        doctorRepositoryMock
            .Setup(x => x.GetByIdAsync(doctor.DoctorId))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(x => x.GetByDoctorIdAsync(doctor.DoctorId))
            .ReturnsAsync(appointments);

        mapperMock
            .Setup(x => x.Map<List<AppointmentDto>>(appointments))
            .Returns(expected);

        var result = await appointmentService.GetAppointmentsByDoctorIdAsync(
            doctor.DoctorId);

        Assert.Same(expected, result);
    }

    // ---------------------------------------------------------------------
    // Additional access and status coverage
    // ---------------------------------------------------------------------

    [Fact]
    public async Task GetAppointmentByIdAsync_OwnPatient_ReturnsAppointment()
    {
        SetCurrentUser(Roles.Patient);

        var patient = CreatePatient();
        var appointment = CreateAppointment(patientId: patient.PatientId);
        var expected = new AppointmentDto();

        patientRepositoryMock
            .Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync(patient);

        appointmentRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(appointment.AppointmentId))
            .ReturnsAsync(appointment);

        mapperMock
            .Setup(x => x.Map<AppointmentDto>(appointment))
            .Returns(expected);

        var result = await appointmentService.GetAppointmentByIdAsync(
            appointment.AppointmentId);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentByIdAsync_OwnDoctor_ReturnsAppointment()
    {
        SetCurrentUser(Roles.Doctor);

        var doctor = CreateDoctor();
        var appointment = CreateAppointment(doctorId: doctor.DoctorId);
        var expected = new AppointmentDto();

        doctorRepositoryMock
            .Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(appointment.AppointmentId))
            .ReturnsAsync(appointment);

        mapperMock
            .Setup(x => x.Map<AppointmentDto>(appointment))
            .Returns(expected);

        var result = await appointmentService.GetAppointmentByIdAsync(
            appointment.AppointmentId);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentByIdAsync_UnknownRole_ThrowsForbidden()
    {
        SetCurrentUser("Unknown");

        var appointment = CreateAppointment();

        appointmentRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(appointment.AppointmentId))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentByIdAsync(
                appointment.AppointmentId));
    }

    [Fact]
    public async Task ChangeStatus_UnknownRole_ThrowsForbidden()
    {
        SetCurrentUser("Unknown");

        var appointment = CreateAppointment(
            status: AppointmentStatus.Pending);

        appointmentRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(appointment.AppointmentId))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.ChangeAppointmentStatusAsync(
                appointment.AppointmentId,
                CreateStatusDto(AppointmentStatus.Confirmed)));
    }

    [Fact]
    public async Task ChangeStatus_UpdateReturnsNull_ThrowsNotFound()
    {
        var appointment = ArrangeDoctorAccess(
            AppointmentStatus.Pending);

        appointmentRepositoryMock
            .Setup(x => x.UpdateAsync(
                appointment.AppointmentId,
                appointment))
            .ReturnsAsync((Appointment?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            appointmentService.ChangeAppointmentStatusAsync(
                appointment.AppointmentId,
                CreateStatusDto(AppointmentStatus.Confirmed)));
    }

    [Fact]
    public async Task CancelAppointmentAsync_ConfirmedPatientAppointment_Succeeds()
    {
        SetCurrentUser(Roles.Patient);

        var patient = CreatePatient();

        var appointment = CreateAppointment(
            patientId: patient.PatientId,
            status: AppointmentStatus.Confirmed);

        patientRepositoryMock
            .Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync(patient);

        appointmentRepositoryMock
            .SetupSequence(x =>
                x.GetByIdWithDetailsAsync(appointment.AppointmentId))
            .ReturnsAsync(appointment)
            .ReturnsAsync(appointment);

        appointmentRepositoryMock
            .Setup(x => x.UpdateAsync(
                appointment.AppointmentId,
                appointment))
            .ReturnsAsync(appointment);

        mapperMock
            .Setup(x => x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        var result = await appointmentService.CancelAppointmentAsync(
            appointment.AppointmentId,
            "  Schedule changed  ");

        Assert.NotNull(result);
        Assert.Equal(
            AppointmentStatus.Cancelled.ToString(),
            appointment.Status);

        Assert.Equal(
            "Schedule changed",
            appointment.CancellationReason);
    }

    [Theory]
    [InlineData(AppointmentStatus.Completed)]
    [InlineData(AppointmentStatus.Cancelled)]
    public async Task PatientCannotCancelFinalAppointment(
        AppointmentStatus currentStatus)
    {
        SetCurrentUser(Roles.Patient);

        var patient = CreatePatient();

        var appointment = CreateAppointment(
            patientId: patient.PatientId,
            status: currentStatus);

        patientRepositoryMock
            .Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync(patient);

        appointmentRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(appointment.AppointmentId))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.CancelAppointmentAsync(
                appointment.AppointmentId,
                "No longer required"));
    }

    // ---------------------------------------------------------------------
    // Booking cache and time validation coverage
    // ---------------------------------------------------------------------

    [Fact]
    public async Task BookAppointmentAsync_CacheFailureStillReturnsAppointment()
    {
        ArrangeBooking();

        var booking = CreateBooking(
            date: DateTime.Today.AddDays(1));

        var mapped = CreateAppointment(appointmentId: 0);
        var saved = CreateAppointment(appointmentId: 100);
        var expected = new AppointmentDto();

        mapperMock
            .Setup(x => x.Map<Appointment>(booking))
            .Returns(mapped);

        appointmentRepositoryMock
            .Setup(x => x.AddAsync(mapped))
            .ReturnsAsync(saved);

        distributedCacheMock
            .Setup(x => x.RemoveAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Redis unavailable"));

        publishEndpointMock
            .Setup(x => x.Publish(
                It.IsAny<AppointmentBookedEvent>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        appointmentRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(saved.AppointmentId))
            .ReturnsAsync(saved);

        mapperMock
            .Setup(x => x.Map<AppointmentDto>(saved))
            .Returns(expected);

        var result = await appointmentService.BookAppointmentAsync(booking);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task BookAppointmentAsync_FutureDate_DoesNotEvaluateAsPastSlot()
    {
        ArrangeBooking();

        var booking = CreateBooking(
            date: DateTime.Today.AddDays(2));

        var mapped = CreateAppointment(appointmentId: 0);
        var saved = CreateAppointment(appointmentId: 101);
        var expected = new AppointmentDto();

        mapperMock
            .Setup(x => x.Map<Appointment>(booking))
            .Returns(mapped);

        appointmentRepositoryMock
            .Setup(x => x.AddAsync(mapped))
            .ReturnsAsync(saved);

        appointmentRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(saved.AppointmentId))
            .ReturnsAsync(saved);

        mapperMock
            .Setup(x => x.Map<AppointmentDto>(saved))
            .Returns(expected);

        publishEndpointMock
            .Setup(x => x.Publish(
                It.IsAny<AppointmentBookedEvent>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await appointmentService.BookAppointmentAsync(booking);

        Assert.Same(expected, result);
    }

    // ---------------------------------------------------------------------
    // Logged-in doctor validation coverage
    // ---------------------------------------------------------------------

    [Fact]
    public async Task GetAllAppointmentsAsync_DoctorIdentifierMissing_ThrowsForbidden()
    {
        SetCurrentUser(Roles.Doctor, userId: null);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAllAppointmentsAsync());
    }

    [Fact]
    public async Task GetAllAppointmentsAsync_DoctorEntityMissing_ThrowsNotFound()
    {
        SetCurrentUser(Roles.Doctor);

        doctorRepositoryMock
            .Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            appointmentService.GetAllAppointmentsAsync());
    }

    [Fact]
    public async Task GetAllAppointmentsAsync_PatientIdentifierMissing_ThrowsForbidden()
    {
        SetCurrentUser(Roles.Patient, userId: null);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAllAppointmentsAsync());
    }

    [Fact]
    public async Task GetAllAppointmentsAsync_PatientEntityMissing_ThrowsNotFound()
    {
        SetCurrentUser(Roles.Patient);

        patientRepositoryMock
            .Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            appointmentService.GetAllAppointmentsAsync());
    }

    // =====================================================================
    // Helpers
    // =====================================================================

    private void SetCurrentUser(
        string role,
        string? userId = "user-1")
    {
        var claims = new List<Claim>
    {
        new(ClaimTypes.Role, role)
    };

        if (!string.IsNullOrWhiteSpace(userId))
        {
            claims.Add(
                new Claim(
                    ClaimTypes.NameIdentifier,
                    userId));
        }

        var identity = new ClaimsIdentity(
            claims,
            authenticationType: "TestAuthentication");

        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = principal
        };

        httpContextAccessorMock
            .SetupGet(x => x.HttpContext)
            .Returns(httpContext);
    }

    private static Patient CreatePatient(
        int patientId = 10,
        string patientName = "Test Patient")
    {
        return new Patient
        {
            PatientId = patientId,
            PatientName = patientName
        };
    }

    private static Doctor CreateDoctor(
        int doctorId = 20,
        bool isActive = true)
    {
        return new Doctor
        {
            DoctorId = doctorId,
            IsActive = isActive
        };
    }

    private static Appointment CreateAppointment(
        int appointmentId = 1,
        int patientId = 10,
        int doctorId = 20,
        AppointmentStatus status = AppointmentStatus.Pending,
        DateTime? scheduledDate = null,
        string? timeSlot = null)
    {
        return new Appointment
        {
            AppointmentId = appointmentId,
            PatientId = patientId,
            DoctorId = doctorId,
            ScheduledDate =
                scheduledDate?.Date ??
                DateTime.Today.AddDays(1),
            TimeSlots =
                timeSlot ??
                GetValidTimeSlot(),
            Status = status.ToString(),
            CancellationReason = null,
            CreatedDate = DateTime.Now
        };
    }

    private static BookAppointmentDto CreateBooking(
        int doctorId = 20,
        DateTime? date = null,
        string? timeSlot = null)
    {
        return new BookAppointmentDto
        {
            DoctorId = doctorId,
            ScheduledDate =
                date?.Date ??
                DateTime.Today.AddDays(1),
            TimeSlot =
                timeSlot ??
                GetValidTimeSlot()
        };
    }

    private static UpdateAppointmentStatusDto CreateStatusDto(
        AppointmentStatus status,
        string? cancellationReason = null)
    {
        return new UpdateAppointmentStatusDto
        {
            Status = status,
            CancellationReason = cancellationReason
        };
    }

    private void ArrangePatient(
        int patientId = 10,
        string userId = "user-1")
    {
        SetCurrentUser(Roles.Patient, userId);

        var patient = CreatePatient(patientId);

        patientRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(patient);
    }

    private void ArrangeBooking(
        bool doctorActive = true,
        int patientId = 10,
        int doctorId = 20)
    {
        SetCurrentUser(Roles.Patient);

        var patient = CreatePatient(patientId);
        var doctor = CreateDoctor(doctorId, doctorActive);

        patientRepositoryMock
            .Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync(patient);

        doctorRepositoryMock
            .Setup(x => x.GetByIdAsync(doctorId))
            .ReturnsAsync(doctor);

        doctorLeaveRepositoryMock
            .Setup(x => x.GetLeaveForDateAsync(
                doctorId,
                It.IsAny<DateTime>()))
            .ReturnsAsync((DoctorLeave?)null);

        appointmentRepositoryMock
            .Setup(x => x.IsSlotBookedAsync(
                doctorId,
                It.IsAny<DateTime>(),
                It.IsAny<string>()))
            .ReturnsAsync(false);

        appointmentRepositoryMock
            .Setup(x =>
                x.PatientHasActiveAppointmentOnDateAndSlotAsync(
                    patientId,
                    It.IsAny<DateTime>(),
                    It.IsAny<string>()))
            .ReturnsAsync(false);

        appointmentRepositoryMock
            .Setup(x =>
                x.PatientHasActiveAppointmentWithDoctorOnDateAsync(
                    patientId,
                    doctorId,
                    It.IsAny<DateTime>()))
            .ReturnsAsync(false);
    }

    private Appointment ArrangeDoctorAccess(
        AppointmentStatus currentStatus,
        int appointmentId = 1,
        int doctorId = 20,
        int patientId = 10)
    {
        SetCurrentUser(Roles.Doctor);

        var doctor = CreateDoctor(doctorId);

        var appointment = CreateAppointment(
            appointmentId: appointmentId,
            patientId: patientId,
            doctorId: doctorId,
            status: currentStatus);

        doctorRepositoryMock
            .Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync(doctor);

        appointmentRepositoryMock
            .Setup(x =>
                x.GetByIdWithDetailsAsync(appointmentId))
            .ReturnsAsync(appointment);

        return appointment;
    }

    private static string GetValidTimeSlot()
    {
        if (TimeSlots.Slots is not [var timeSlot, ..] ||
            string.IsNullOrWhiteSpace(timeSlot))
        {
            throw new InvalidOperationException(
                "TimeSlots.Slots must contain at least one valid configured time slot.");
        }

        return timeSlot;
    }
}
