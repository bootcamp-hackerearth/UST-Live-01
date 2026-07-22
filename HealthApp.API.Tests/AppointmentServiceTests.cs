using System.Security.Claims;
using AutoMapper;
using HealthApp.API.Data;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HealthApp.API.Tests;

public class AppointmentServiceTests : IDisposable
{
    private readonly Mock<IAppointmentRepository> appointmentRepositoryMock = new();
    private readonly Mock<IPatientRepository> patientRepositoryMock = new();
    private readonly Mock<IDoctorRepository> doctorRepositoryMock = new();
    private readonly Mock<IDoctorLeaveRepository> doctorLeaveRepositoryMock = new();
    private readonly Mock<IHttpContextAccessor> httpContextAccessorMock = new();
    private readonly Mock<IDistributedCache> distributedCacheMock = new();
    private readonly Mock<IMapper> mapperMock = new();
    private readonly Mock<ILogger<AppointmentService>> loggerMock = new();

    private readonly HealthAppDbContext dbContext;
    private readonly AppointmentService appointmentService;

    public AppointmentServiceTests()
    {
        var options = new DbContextOptionsBuilder<HealthAppDbContext>()
            .UseInMemoryDatabase($"AppointmentServiceTests-{Guid.NewGuid()}")
            .ConfigureWarnings(warnings =>
                warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        dbContext = new HealthAppDbContext(options);

        appointmentService = new AppointmentService(
            appointmentRepositoryMock.Object,
            patientRepositoryMock.Object,
            doctorRepositoryMock.Object,
            doctorLeaveRepositoryMock.Object,
            httpContextAccessorMock.Object,
            distributedCacheMock.Object,
            dbContext,
            mapperMock.Object,
            loggerMock.Object);
    }

    // ---------------------------------------------------------------------
    // GetAppointmentsByStatusAsync
    // ---------------------------------------------------------------------

    [Theory]
    [InlineData(Roles.Patient)]
    [InlineData(Roles.Doctor)]
    public async Task GetAppointmentsByStatus_UserRole_FiltersAppointments(string role)
    {
        SetCurrentUser(role);

        var matching = CreateAppointment(1, status: AppointmentStatus.Pending);
        var other = CreateAppointment(2, status: AppointmentStatus.Completed);
        var appointments = new List<Appointment> { matching, other };
        var expected = new List<AppointmentDto> { new() };

        if (role == Roles.Patient)
        {
            var patient = CreatePatient(matching.PatientId);
            patientRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1")).ReturnsAsync(patient);
            appointmentRepositoryMock.Setup(x => x.GetByPatientIdAsync(patient.PatientId))
                .ReturnsAsync(appointments);
        }
        else
        {
            var doctor = CreateDoctor(matching.DoctorId);
            doctorRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1")).ReturnsAsync(doctor);
            appointmentRepositoryMock.Setup(x => x.GetByDoctorIdAsync(doctor.DoctorId))
                .ReturnsAsync(appointments);
        }

        mapperMock.Setup(x => x.Map<List<AppointmentDto>>(
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
        var appointments = new List<Appointment> { CreateAppointment() };
        var expected = new List<AppointmentDto> { new() };

        appointmentRepositoryMock.Setup(x => x.GetByStatusAsync(AppointmentStatus.Pending))
            .ReturnsAsync(appointments);
        mapperMock.Setup(x => x.Map<List<AppointmentDto>>(appointments)).Returns(expected);

        var result = await appointmentService.GetAppointmentsByStatusAsync(
            AppointmentStatus.Pending);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentsByStatus_UnknownRole_ThrowsForbidden()
    {
        SetCurrentUser("Unknown");

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentsByStatusAsync(AppointmentStatus.Pending));
    }

    // ---------------------------------------------------------------------
    // Patient-based appointment access
    // ---------------------------------------------------------------------

    [Fact]
    public async Task GetAppointmentsByPatientId_InvalidId_ThrowsRuleException()
    {
        SetCurrentUser(Roles.Admin);

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.GetAppointmentsByPatientIdAsync(0));
    }

    [Fact]
    public async Task GetAppointmentsByPatientId_MissingPatient_ThrowsNotFound()
    {
        SetCurrentUser(Roles.Admin);
        patientRepositoryMock.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            appointmentService.GetAppointmentsByPatientIdAsync(99));
    }

    [Fact]
    public async Task GetAppointmentsByPatientId_PatientReturnsOwnAppointments()
    {
        SetCurrentUser(Roles.Patient);
        var patient = CreatePatient(10);
        var appointments = new List<Appointment> { CreateAppointment(patientId: 10) };
        var expected = new List<AppointmentDto> { new() };

        patientRepositoryMock.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(patient);
        patientRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1")).ReturnsAsync(patient);
        appointmentRepositoryMock.Setup(x => x.GetByPatientIdAsync(10)).ReturnsAsync(appointments);
        mapperMock.Setup(x => x.Map<List<AppointmentDto>>(appointments)).Returns(expected);

        var result = await appointmentService.GetAppointmentsByPatientIdAsync(10);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentsByPatientId_PatientRequestsAnotherPatient_ThrowsForbidden()
    {
        SetCurrentUser(Roles.Patient);
        patientRepositoryMock.Setup(x => x.GetByIdAsync(20)).ReturnsAsync(CreatePatient(20));
        patientRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync(CreatePatient(10));

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentsByPatientIdAsync(20));
    }

    [Fact]
    public async Task GetAppointmentsByPatientId_DoctorReturnsOnlyOwnPatientAppointments()
    {
        SetCurrentUser(Roles.Doctor);
        var doctor = CreateDoctor(20);
        var patient = CreatePatient(10);
        var appointments = new List<Appointment>
        {
            CreateAppointment(1, 10, 20),
            CreateAppointment(2, 30, 20)
        };
        var expected = new List<AppointmentDto> { new() };

        patientRepositoryMock.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(patient);
        doctorRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1")).ReturnsAsync(doctor);
        appointmentRepositoryMock.Setup(x => x.GetByDoctorIdAsync(20)).ReturnsAsync(appointments);
        mapperMock.Setup(x => x.Map<List<AppointmentDto>>(
                It.Is<List<Appointment>>(list => list.Count == 1 && list[0].PatientId == 10)))
            .Returns(expected);

        var result = await appointmentService.GetAppointmentsByPatientIdAsync(10);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentsByPatientId_AdminReturnsPatientAppointments()
    {
        SetCurrentUser(Roles.Admin);
        var patient = CreatePatient();
        var appointments = new List<Appointment> { CreateAppointment(patientId: patient.PatientId) };
        var expected = new List<AppointmentDto> { new() };

        patientRepositoryMock.Setup(x => x.GetByIdAsync(patient.PatientId)).ReturnsAsync(patient);
        appointmentRepositoryMock.Setup(x => x.GetByPatientIdAsync(patient.PatientId))
            .ReturnsAsync(appointments);
        mapperMock.Setup(x => x.Map<List<AppointmentDto>>(appointments)).Returns(expected);

        var result = await appointmentService.GetAppointmentsByPatientIdAsync(patient.PatientId);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentsByPatientId_UnknownRole_ThrowsForbidden()
    {
        SetCurrentUser("Unknown");
        var patient = CreatePatient();
        patientRepositoryMock.Setup(x => x.GetByIdAsync(patient.PatientId)).ReturnsAsync(patient);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentsByPatientIdAsync(patient.PatientId));
    }

    // ---------------------------------------------------------------------
    // Doctor-based appointment access
    // ---------------------------------------------------------------------

    [Fact]
    public async Task GetAppointmentsByDoctorId_InvalidId_ThrowsRuleException()
    {
        SetCurrentUser(Roles.Admin);

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.GetAppointmentsByDoctorIdAsync(0));
    }

    [Fact]
    public async Task GetAppointmentsByDoctorId_MissingDoctor_ThrowsNotFound()
    {
        SetCurrentUser(Roles.Admin);
        doctorRepositoryMock.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            appointmentService.GetAppointmentsByDoctorIdAsync(99));
    }

    [Fact]
    public async Task GetAppointmentsByDoctorId_Patient_ThrowsForbidden()
    {
        SetCurrentUser(Roles.Patient);
        var doctor = CreateDoctor();
        doctorRepositoryMock.Setup(x => x.GetByIdAsync(doctor.DoctorId)).ReturnsAsync(doctor);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentsByDoctorIdAsync(doctor.DoctorId));
    }

    [Fact]
    public async Task GetAppointmentsByDoctorId_DoctorReturnsOwnAppointments()
    {
        SetCurrentUser(Roles.Doctor);
        var doctor = CreateDoctor();
        var appointments = new List<Appointment> { CreateAppointment(doctorId: doctor.DoctorId) };
        var expected = new List<AppointmentDto> { new() };

        doctorRepositoryMock.Setup(x => x.GetByIdAsync(doctor.DoctorId)).ReturnsAsync(doctor);
        doctorRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1")).ReturnsAsync(doctor);
        appointmentRepositoryMock.Setup(x => x.GetByDoctorIdAsync(doctor.DoctorId))
            .ReturnsAsync(appointments);
        mapperMock.Setup(x => x.Map<List<AppointmentDto>>(appointments)).Returns(expected);

        var result = await appointmentService.GetAppointmentsByDoctorIdAsync(doctor.DoctorId);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentsByDoctorId_DoctorRequestsAnotherDoctor_ThrowsForbidden()
    {
        SetCurrentUser(Roles.Doctor);
        var requested = CreateDoctor(20);
        var loggedIn = CreateDoctor(30);

        doctorRepositoryMock.Setup(x => x.GetByIdAsync(20)).ReturnsAsync(requested);
        doctorRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1")).ReturnsAsync(loggedIn);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentsByDoctorIdAsync(20));
    }

    [Fact]
    public async Task GetAppointmentsByDoctorId_AdminReturnsAppointments()
    {
        SetCurrentUser(Roles.Admin);
        var doctor = CreateDoctor();
        var appointments = new List<Appointment> { CreateAppointment(doctorId: doctor.DoctorId) };
        var expected = new List<AppointmentDto> { new() };

        doctorRepositoryMock.Setup(x => x.GetByIdAsync(doctor.DoctorId)).ReturnsAsync(doctor);
        appointmentRepositoryMock.Setup(x => x.GetByDoctorIdAsync(doctor.DoctorId))
            .ReturnsAsync(appointments);
        mapperMock.Setup(x => x.Map<List<AppointmentDto>>(appointments)).Returns(expected);

        var result = await appointmentService.GetAppointmentsByDoctorIdAsync(doctor.DoctorId);

        Assert.Same(expected, result);
    }

    // ---------------------------------------------------------------------
    // Individual appointment and status coverage
    // ---------------------------------------------------------------------

    [Fact]
    public async Task GetAppointmentById_OwnPatient_ReturnsAppointment()
    {
        SetCurrentUser(Roles.Patient);
        var patient = CreatePatient();
        var appointment = CreateAppointment(patientId: patient.PatientId);
        var expected = new AppointmentDto();

        patientRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1")).ReturnsAsync(patient);
        appointmentRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(appointment.AppointmentId))
            .ReturnsAsync(appointment);
        mapperMock.Setup(x => x.Map<AppointmentDto>(appointment)).Returns(expected);

        var result = await appointmentService.GetAppointmentByIdAsync(appointment.AppointmentId);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentById_OwnDoctor_ReturnsAppointment()
    {
        SetCurrentUser(Roles.Doctor);
        var doctor = CreateDoctor();
        var appointment = CreateAppointment(doctorId: doctor.DoctorId);
        var expected = new AppointmentDto();

        doctorRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1")).ReturnsAsync(doctor);
        appointmentRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(appointment.AppointmentId))
            .ReturnsAsync(appointment);
        mapperMock.Setup(x => x.Map<AppointmentDto>(appointment)).Returns(expected);

        var result = await appointmentService.GetAppointmentByIdAsync(appointment.AppointmentId);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetAppointmentById_UnknownRole_ThrowsForbidden()
    {
        SetCurrentUser("Unknown");
        var appointment = CreateAppointment();
        appointmentRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(appointment.AppointmentId))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAppointmentByIdAsync(appointment.AppointmentId));
    }

    [Fact]
    public async Task ChangeStatus_UnknownRole_ThrowsForbidden()
    {
        SetCurrentUser("Unknown");
        var appointment = CreateAppointment(status: AppointmentStatus.Pending);
        appointmentRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(appointment.AppointmentId))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.ChangeAppointmentStatusAsync(
                appointment.AppointmentId,
                CreateStatusDto(AppointmentStatus.Confirmed)));
    }

    [Fact]
    public async Task ChangeStatus_UpdateReturnsNull_ThrowsNotFound()
    {
        var appointment = ArrangeDoctorAccess(AppointmentStatus.Pending);
        appointmentRepositoryMock.Setup(x => x.UpdateAsync(appointment.AppointmentId, appointment))
            .ReturnsAsync((Appointment?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            appointmentService.ChangeAppointmentStatusAsync(
                appointment.AppointmentId,
                CreateStatusDto(AppointmentStatus.Confirmed)));
    }

    [Fact]
    public async Task CancelAppointment_ConfirmedPatientAppointment_Succeeds()
    {
        SetCurrentUser(Roles.Patient);
        var patient = CreatePatient();
        var appointment = CreateAppointment(
            patientId: patient.PatientId,
            status: AppointmentStatus.Confirmed);

        patientRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1")).ReturnsAsync(patient);
        appointmentRepositoryMock.SetupSequence(x =>
                x.GetByIdWithDetailsAsync(appointment.AppointmentId))
            .ReturnsAsync(appointment)
            .ReturnsAsync(appointment);
        appointmentRepositoryMock.Setup(x => x.UpdateAsync(appointment.AppointmentId, appointment))
            .ReturnsAsync(appointment);
        mapperMock.Setup(x => x.Map<AppointmentDto>(appointment)).Returns(new AppointmentDto());

        var result = await appointmentService.CancelAppointmentAsync(
            appointment.AppointmentId,
            "  Schedule changed  ");

        Assert.NotNull(result);
        Assert.Equal(AppointmentStatus.Cancelled.ToString(), appointment.Status);
        Assert.Equal("Schedule changed", appointment.CancellationReason);
    }

    [Theory]
    [InlineData(AppointmentStatus.Completed)]
    [InlineData(AppointmentStatus.Cancelled)]
    public async Task PatientCannotCancelFinalAppointment(AppointmentStatus currentStatus)
    {
        SetCurrentUser(Roles.Patient);
        var patient = CreatePatient();
        var appointment = CreateAppointment(patientId: patient.PatientId, status: currentStatus);

        patientRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1")).ReturnsAsync(patient);
        appointmentRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(appointment.AppointmentId))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.CancelAppointmentAsync(
                appointment.AppointmentId,
                "No longer required"));
    }

    // ---------------------------------------------------------------------
    // Transactional outbox booking coverage
    // ---------------------------------------------------------------------

    [Fact]
    public async Task BookAppointment_Success_CreatesPendingOutboxMessage()
    {
        ArrangeBooking();
        var booking = CreateBooking(date: DateTime.Today.AddDays(1));
        var mapped = CreateAppointment(appointmentId: 0);
        var saved = CreateAppointment(appointmentId: 100);
        var expected = new AppointmentDto();

        ArrangeSuccessfulBookingPersistence(booking, mapped, saved, expected);

        var result = await appointmentService.BookAppointmentAsync(booking);

        Assert.Same(expected, result);

        var outboxMessage = await dbContext.OutboxMessages.SingleAsync();
        Assert.Equal(OutboxConstants.AppointmentBookedEventType, outboxMessage.EventType);
        Assert.Null(outboxMessage.ProcessedDate);
        Assert.Null(outboxMessage.LastAttemptDate);
        Assert.Null(outboxMessage.ErrorMessage);
        Assert.Equal(0, outboxMessage.RetryCount);
        Assert.Contains("\"AppointmentId\":100", outboxMessage.Payload);
        Assert.Contains("\"PatientName\":\"Test Patient\"", outboxMessage.Payload);
        Assert.Contains("\"DoctorId\":20", outboxMessage.Payload);
    }

    [Fact]
    public async Task BookAppointment_CacheFailure_StillReturnsAndKeepsOutboxMessage()
    {
        ArrangeBooking();
        var booking = CreateBooking(date: DateTime.Today.AddDays(1));
        var mapped = CreateAppointment(appointmentId: 0);
        var saved = CreateAppointment(appointmentId: 101);
        var expected = new AppointmentDto();

        ArrangeSuccessfulBookingPersistence(booking, mapped, saved, expected);

        distributedCacheMock.Setup(x => x.RemoveAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Redis unavailable"));

        var result = await appointmentService.BookAppointmentAsync(booking);

        Assert.Same(expected, result);
        Assert.Equal(1, await dbContext.OutboxMessages.CountAsync());
    }

    [Fact]
    public async Task BookAppointment_FutureDate_DoesNotEvaluateAsPastSlot()
    {
        ArrangeBooking();
        var booking = CreateBooking(date: DateTime.Today.AddDays(2));
        var mapped = CreateAppointment(appointmentId: 0);
        var saved = CreateAppointment(appointmentId: 102);
        var expected = new AppointmentDto();

        ArrangeSuccessfulBookingPersistence(booking, mapped, saved, expected);

        var result = await appointmentService.BookAppointmentAsync(booking);

        Assert.Same(expected, result);
        Assert.Single(await dbContext.OutboxMessages.ToListAsync());
    }

    [Fact]
    public async Task BookAppointment_NullDto_ThrowsRuleException()
    {
        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.BookAppointmentAsync(null!));
    }

    [Fact]
    public async Task BookAppointment_InactiveDoctor_ThrowsRuleException()
    {
        ArrangeBooking(doctorActive: false);

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.BookAppointmentAsync(CreateBooking()));

        Assert.Empty(await dbContext.OutboxMessages.ToListAsync());
    }

    [Fact]
    public async Task BookAppointment_PastDate_ThrowsRuleException()
    {
        ArrangeBooking();

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.BookAppointmentAsync(
                CreateBooking(date: DateTime.Today.AddDays(-1))));

        Assert.Empty(await dbContext.OutboxMessages.ToListAsync());
    }

    [Fact]
    public async Task BookAppointment_InvalidTimeSlot_ThrowsRuleException()
    {
        ArrangeBooking();

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.BookAppointmentAsync(
                CreateBooking(timeSlot: "invalid-slot")));

        Assert.Empty(await dbContext.OutboxMessages.ToListAsync());
    }

    [Fact]
    public async Task BookAppointment_DoctorOnLeave_ThrowsRuleException()
    {
        ArrangeBooking();
        var booking = CreateBooking();

        doctorLeaveRepositoryMock.Setup(x => x.GetLeaveForDateAsync(
                booking.DoctorId,
                booking.ScheduledDate.Date))
            .ReturnsAsync(new DoctorLeave { DoctorLeaveId = 50, DoctorId = booking.DoctorId });

        await Assert.ThrowsAsync<AppointmentRuleException>(() =>
            appointmentService.BookAppointmentAsync(booking));

        Assert.Empty(await dbContext.OutboxMessages.ToListAsync());
    }

    [Fact]
    public async Task BookAppointment_SlotAlreadyBooked_ThrowsConflict()
    {
        ArrangeBooking();
        var booking = CreateBooking();
        appointmentRepositoryMock.Setup(x => x.IsSlotBookedAsync(
                booking.DoctorId,
                booking.ScheduledDate.Date,
                booking.TimeSlot))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ConflictException>(() =>
            appointmentService.BookAppointmentAsync(booking));
    }

    [Fact]
    public async Task BookAppointment_PatientHasSameSlot_ThrowsConflict()
    {
        ArrangeBooking();
        var booking = CreateBooking();
        appointmentRepositoryMock.Setup(x =>
                x.PatientHasActiveAppointmentOnDateAndSlotAsync(
                    10,
                    booking.ScheduledDate.Date,
                    booking.TimeSlot))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ConflictException>(() =>
            appointmentService.BookAppointmentAsync(booking));
    }

    [Fact]
    public async Task BookAppointment_PatientHasDoctorAppointmentOnDate_ThrowsConflict()
    {
        ArrangeBooking();
        var booking = CreateBooking();
        appointmentRepositoryMock.Setup(x =>
                x.PatientHasActiveAppointmentWithDoctorOnDateAsync(
                    10,
                    booking.DoctorId,
                    booking.ScheduledDate.Date))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ConflictException>(() =>
            appointmentService.BookAppointmentAsync(booking));
    }

    [Fact]
    public async Task BookAppointment_RepositoryFailure_DoesNotCreateOutboxMessage()
    {
        ArrangeBooking();
        var booking = CreateBooking();
        var mapped = CreateAppointment(appointmentId: 0);

        mapperMock.Setup(x => x.Map<Appointment>(booking)).Returns(mapped);
        appointmentRepositoryMock.Setup(x => x.AddAsync(mapped))
            .ThrowsAsync(new InvalidOperationException("Database unavailable"));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            appointmentService.BookAppointmentAsync(booking));

        Assert.Empty(await dbContext.OutboxMessages.ToListAsync());
    }

    // ---------------------------------------------------------------------
    // Logged-in entity validation
    // ---------------------------------------------------------------------

    [Fact]
    public async Task GetAllAppointments_DoctorIdentifierMissing_ThrowsForbidden()
    {
        SetCurrentUser(Roles.Doctor, userId: null);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAllAppointmentsAsync());
    }

    [Fact]
    public async Task GetAllAppointments_DoctorEntityMissing_ThrowsNotFound()
    {
        SetCurrentUser(Roles.Doctor);
        doctorRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            appointmentService.GetAllAppointmentsAsync());
    }

    [Fact]
    public async Task GetAllAppointments_PatientIdentifierMissing_ThrowsForbidden()
    {
        SetCurrentUser(Roles.Patient, userId: null);

        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            appointmentService.GetAllAppointmentsAsync());
    }

    [Fact]
    public async Task GetAllAppointments_PatientEntityMissing_ThrowsNotFound()
    {
        SetCurrentUser(Roles.Patient);
        patientRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1"))
            .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            appointmentService.GetAllAppointmentsAsync());
    }

    // ---------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------

    private void ArrangeSuccessfulBookingPersistence(
        BookAppointmentDto booking,
        Appointment mapped,
        Appointment saved,
        AppointmentDto expected)
    {
        mapperMock.Setup(x => x.Map<Appointment>(booking)).Returns(mapped);
        appointmentRepositoryMock.Setup(x => x.AddAsync(mapped)).ReturnsAsync(saved);
        appointmentRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(saved.AppointmentId))
            .ReturnsAsync(saved);
        mapperMock.Setup(x => x.Map<AppointmentDto>(saved)).Returns(expected);
    }

    private void SetCurrentUser(string role, string? userId = "user-1")
    {
        var claims = new List<Claim> { new(ClaimTypes.Role, role) };

        if (!string.IsNullOrWhiteSpace(userId))
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
        }

        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(claims, "TestAuthentication"));

        httpContextAccessorMock.SetupGet(x => x.HttpContext)
            .Returns(new DefaultHttpContext { User = principal });
    }

    private static Patient CreatePatient(
        int patientId = 10,
        string patientName = "Test Patient") =>
        new()
        {
            PatientId = patientId,
            PatientName = patientName
        };

    private static Doctor CreateDoctor(int doctorId = 20, bool isActive = true) =>
        new()
        {
            DoctorId = doctorId,
            IsActive = isActive
        };

    private static Appointment CreateAppointment(
        int appointmentId = 1,
        int patientId = 10,
        int doctorId = 20,
        AppointmentStatus status = AppointmentStatus.Pending,
        DateTime? scheduledDate = null,
        string? timeSlot = null) =>
        new()
        {
            AppointmentId = appointmentId,
            PatientId = patientId,
            DoctorId = doctorId,
            ScheduledDate = scheduledDate?.Date ?? DateTime.Today.AddDays(1),
            TimeSlots = timeSlot ?? GetValidTimeSlot(),
            Status = status.ToString(),
            CancellationReason = null,
            CreatedDate = DateTime.Now
        };

    private static BookAppointmentDto CreateBooking(
        int doctorId = 20,
        DateTime? date = null,
        string? timeSlot = null) =>
        new()
        {
            DoctorId = doctorId,
            ScheduledDate = date?.Date ?? DateTime.Today.AddDays(1),
            TimeSlot = timeSlot ?? GetValidTimeSlot()
        };

    private static UpdateAppointmentStatusDto CreateStatusDto(
        AppointmentStatus status,
        string? cancellationReason = null) =>
        new()
        {
            Status = status,
            CancellationReason = cancellationReason
        };

    private void ArrangeBooking(
        bool doctorActive = true,
        int patientId = 10,
        int doctorId = 20)
    {
        SetCurrentUser(Roles.Patient);
        var patient = CreatePatient(patientId);
        var doctor = CreateDoctor(doctorId, doctorActive);

        patientRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1")).ReturnsAsync(patient);
        doctorRepositoryMock.Setup(x => x.GetByIdAsync(doctorId)).ReturnsAsync(doctor);
        doctorLeaveRepositoryMock.Setup(x => x.GetLeaveForDateAsync(
                doctorId,
                It.IsAny<DateTime>()))
            .ReturnsAsync((DoctorLeave?)null);
        appointmentRepositoryMock.Setup(x => x.IsSlotBookedAsync(
                doctorId,
                It.IsAny<DateTime>(),
                It.IsAny<string>()))
            .ReturnsAsync(false);
        appointmentRepositoryMock.Setup(x =>
                x.PatientHasActiveAppointmentOnDateAndSlotAsync(
                    patientId,
                    It.IsAny<DateTime>(),
                    It.IsAny<string>()))
            .ReturnsAsync(false);
        appointmentRepositoryMock.Setup(x =>
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
            appointmentId,
            patientId,
            doctorId,
            currentStatus);

        doctorRepositoryMock.Setup(x => x.GetByUserIdAsync("user-1")).ReturnsAsync(doctor);
        appointmentRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(appointmentId))
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

    public void Dispose()
    {
        dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
