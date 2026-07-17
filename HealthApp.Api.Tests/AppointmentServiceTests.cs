using System.Text;
using System.Text.Json;
using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Dependencies;
using HealthApp.Api.Services.Impl;
using HealthApp.Shared.Constants;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using HealthApp.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HealthApp.Api.Tests.Services;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _appointmentRepo = new();
    private readonly Mock<IPatientRepository> _patientRepo = new();
    private readonly Mock<IDoctorRepository> _doctorRepo = new();
    private readonly Mock<IDoctorLeaveRepository> _doctorLeaveRepo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IPublishEndpoint> _publishEndpoint = new();
    private readonly Mock<IDistributedCache> _cache = new();
    private readonly Mock<ILogger<AppointmentService>> _logger = new();
    private readonly AppointmentService _service;

    public AppointmentServiceTests()
    {
        _cache
            .Setup(cache => cache.GetAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);

        _cache
            .Setup(cache => cache.SetAsync(
                It.IsAny<string>(),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _cache
            .Setup(cache => cache.RemoveAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _doctorLeaveRepo
            .Setup(repo => repo.GetLeaveForDateAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((DoctorLeave?)null);

        _publishEndpoint
            .Setup(endpoint => endpoint.Publish(
                It.IsAny<AppointmentBookedEvent>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var repositories = new AppointmentServiceRepositories(
            _appointmentRepo.Object,
            _patientRepo.Object,
            _doctorRepo.Object,
            _doctorLeaveRepo.Object);

        _service = new AppointmentService(
            repositories,
            _mapper.Object,
            _publishEndpoint.Object,
            _cache.Object,
            _logger.Object);
    }

    [Fact]
    public void Constructor_NullRepositories_ShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(() => new AppointmentService(
            null!,
            _mapper.Object,
            _publishEndpoint.Object,
            _cache.Object,
            _logger.Object));
    }

    [Fact]
    public async Task GetAppointments_NullFilter_ShouldUseDefaults()
    {
        var repositoryResult =
            (Items: Enumerable.Empty<Appointment>(), TotalCount: 0);

        _appointmentRepo
            .Setup(repo => repo.GetAppointmentsAsync(
                It.Is<AppointmentFilterDto>(filter =>
                    filter.PageNumber == 1 && filter.PageSize == 10),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(repositoryResult);

        _mapper
            .Setup(mapper => mapper.Map<List<AppointmentDto>>(
                It.IsAny<IEnumerable<Appointment>>()))
            .Returns([]);

        var result = await _service.GetAppointmentsAsync(null!);

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
    }

    [Fact]
    public async Task GetAppointments_ExactDateAndRange_ShouldThrow()
    {
        var filter = new AppointmentFilterDto
        {
            Date = DateOnly.FromDateTime(DateTime.Today),
            FromDate = DateOnly.FromDateTime(DateTime.Today)
        };

        var exception = await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.GetAppointmentsAsync(filter));

        Assert.Equal(
            "Use either exact date or date range, not both.",
            exception.Message);

        _appointmentRepo.Verify(
            repo => repo.GetAppointmentsAsync(
                It.IsAny<AppointmentFilterDto>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetAppointments_FromDateAfterToDate_ShouldThrow()
    {
        var filter = new AppointmentFilterDto
        {
            FromDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
            ToDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
        };

        var exception = await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.GetAppointmentsAsync(filter));

        Assert.Equal(
            "From date cannot be greater than to date.",
            exception.Message);
    }

    [Fact]
    public async Task GetAppointmentById_NavigationAlreadyLoaded_ShouldNotReload()
    {
        var appointment = new Appointment
        {
            AppointmentId = 15,
            PatientId = 2,
            DoctorId = 3,
            Patient = CreatePatient(2),
            Doctor = CreateDoctor(3)
        };

        _appointmentRepo
            .Setup(repo => repo.GetByIdAsync(
                15,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        _mapper
            .Setup(mapper => mapper.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto { AppointmentId = 15 });

        var result = await _service.GetAppointmentByIdAsync(15);

        Assert.Equal(15, result.AppointmentId);
        _patientRepo.Verify(
            repo => repo.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        _doctorRepo.Verify(
            repo => repo.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task BookAppointment_InvalidDoctorId_ShouldThrow()
    {
        var dto = ValidDto();
        dto.DoctorId = 0;

        var exception = await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.BookAppointmentAsync(dto));

        Assert.Equal("Valid doctor is required.", exception.Message);
    }

    [Fact]
    public async Task BookAppointment_DefaultScheduledDate_ShouldThrow()
    {
        var dto = ValidDto();
        dto.ScheduledDate = default;

        var exception = await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.BookAppointmentAsync(dto));

        Assert.Equal("Scheduled date is required.", exception.Message);
    }

    [Fact]
    public async Task BookAppointment_MissingPatientUserId_ShouldThrow()
    {
        var dto = ValidDto();

        _patientRepo
            .Setup(repo => repo.GetByIdAsync(
                dto.PatientId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreatePatient(dto.PatientId));

        _patientRepo
            .Setup(repo => repo.GetPatientUserIdAsync(
                dto.PatientId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        var exception = await Assert.ThrowsAsync<
            BusinessRuleViolationException>(
            () => _service.BookAppointmentAsync(dto));

        Assert.Equal(
            "Patient user account is not linked.",
            exception.Message);

        _doctorRepo.Verify(
            repo => repo.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task BookAppointment_DoctorNotFound_ShouldThrow()
    {
        var dto = ValidDto();
        SetupPatient(dto.PatientId);

        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(
                dto.DoctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointment_Valid_ShouldTrimSlotAndPublishExpectedEvent()
    {
        var dto = ValidDto();
        dto.TimeSlot = " 10AM ";
        var scheduledDate = DateOnly.FromDateTime(dto.ScheduledDate);
        var patient = CreatePatient(dto.PatientId);
        var doctor = CreateDoctor(dto.DoctorId);
        var savedAppointment = new Appointment
        {
            AppointmentId = 501,
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            ScheduledDate = scheduledDate,
            TimeSlot = "10AM"
        };

        SetupPatient(dto.PatientId, patient);
        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(
                dto.DoctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);
        SetupNoBookingConflicts();

        _mapper
            .Setup(mapper => mapper.Map<Appointment>(dto))
            .Returns(new Appointment());
        _appointmentRepo
            .Setup(repo => repo.Add(
                It.Is<Appointment>(appointment =>
                    appointment.TimeSlot == "10AM" &&
                    appointment.Status == AppointmentStatus.Pending &&
                    appointment.CancellationReason == null),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(savedAppointment);
        _mapper
            .Setup(mapper => mapper.Map<AppointmentDto>(savedAppointment))
            .Returns(new AppointmentDto { AppointmentId = 501 });

        var result = await _service.BookAppointmentAsync(dto);

        Assert.Equal(501, result.AppointmentId);
        _publishEndpoint.Verify(endpoint => endpoint.Publish(
            It.Is<AppointmentBookedEvent>(message =>
                message.AppointmentId == 501 &&
                message.PatientId == dto.PatientId &&
                message.DoctorId == dto.DoctorId &&
                message.TimeSlot == "10AM"),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateStatus_CancelledWithReason_ShouldTrimReason()
    {
        var appointment = CreateAppointment(
            status: AppointmentStatus.Pending);

        _appointmentRepo
            .Setup(repo => repo.GetByIdAsync(
                appointment.AppointmentId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        await _service.UpdateAppointmentStatusAsync(
            appointment.AppointmentId,
            AppointmentStatus.Cancelled,
            "  Patient requested cancellation  ");

        Assert.Equal(AppointmentStatus.Cancelled, appointment.Status);
        Assert.Equal(
            "Patient requested cancellation",
            appointment.CancellationReason);
    }

    [Fact]
    public async Task UpdateStatus_NonCancelled_ShouldClearOldReason()
    {
        var appointment = CreateAppointment(
            status: AppointmentStatus.Pending);
        appointment.CancellationReason = "Old reason";

        _appointmentRepo
            .Setup(repo => repo.GetByIdAsync(
                appointment.AppointmentId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        await _service.UpdateAppointmentStatusAsync(
            appointment.AppointmentId,
            AppointmentStatus.Confirmed);

        Assert.Equal(AppointmentStatus.Confirmed, appointment.Status);
        Assert.Null(appointment.CancellationReason);
    }

    [Fact]
    public async Task DeleteAppointment_NotFound_ShouldThrow()
    {
        _appointmentRepo
            .Setup(repo => repo.GetByIdAsync(
                404,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Appointment?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.DeleteAppointmentAsync(404));
    }

    [Fact]
    public async Task GetDoctorAvailability_PastDate_ShouldThrow()
    {
        var pastDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));

        var exception = await Assert.ThrowsAsync<
            BusinessRuleViolationException>(
            () => _service.GetDoctorAvailabilityAsync(1, pastDate));

        Assert.Equal("Past date is not allowed.", exception.Message);
        _doctorRepo.Verify(
            repo => repo.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetDoctorAvailability_CacheHit_ShouldReturnCachedValue()
    {
        var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var cached = new DoctorAvailabilityDto
        {
            DoctorId = 1,
            Date = date,
            IsDoctorOnLeave = false,
            Message = "Cached",
            Slots =
            [
                new DoctorAvailabilitySlotDto
                {
                    TimeSlot = "10AM",
                    IsAvailable = true,
                    Status = "Available"
                }
            ]
        };
        var json = JsonSerializer.Serialize(cached);

        SetupActiveDoctor(1);
        _cache
            .Setup(cache => cache.GetAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Encoding.UTF8.GetBytes(json));

        var result = await _service.GetDoctorAvailabilityAsync(1, date);

        Assert.Equal("Cached", result.Message);
        _doctorLeaveRepo.Verify(
            repo => repo.GetLeaveForDateAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        _cache.Verify(cache => cache.SetAsync(
            It.IsAny<string>(),
            It.IsAny<byte[]>(),
            It.IsAny<DistributedCacheEntryOptions>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetDoctorAvailability_InvalidCachedJson_ShouldRemoveAndRebuild()
    {
        var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

        SetupActiveDoctor(1);
        _cache
            .Setup(cache => cache.GetAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Encoding.UTF8.GetBytes("{invalid-json"));
        _appointmentRepo
            .Setup(repo => repo.IsDoctorSlotBookedAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _service.GetDoctorAvailabilityAsync(1, date);

        Assert.False(result.IsDoctorOnLeave);
        Assert.Equal(TimeSlots.Slots.Count, result.Slots.Count);
        _cache.Verify(cache => cache.RemoveAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()),
            Times.Once);
        _cache.Verify(cache => cache.SetAsync(
            It.IsAny<string>(),
            It.IsAny<byte[]>(),
            It.Is<DistributedCacheEntryOptions>(options =>
                options.AbsoluteExpirationRelativeToNow ==
                    TimeSpan.FromMinutes(5) &&
                options.SlidingExpiration == TimeSpan.FromMinutes(2)),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetDoctorAvailability_BookedSlot_ShouldMarkOnlyThatSlotUnavailable()
    {
        var date = DateOnly.FromDateTime(
            DateTime.Today.AddDays(1));
        var bookedSlot = TimeSlots.Slots.First();

        SetupActiveDoctor(1);

        _appointmentRepo
            .Setup(repo => repo.IsDoctorSlotBookedAsync(
                1,
                date,
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((
                int _,
                DateOnly _,
                string slot,
                CancellationToken _) => slot == bookedSlot);

        var result = await _service.GetDoctorAvailabilityAsync(1, date);

        var unavailable = Assert.Single(
            result.Slots,
            slot => !slot.IsAvailable);

        Assert.Equal(bookedSlot, unavailable.TimeSlot);
        Assert.Equal("Booked", unavailable.Status);

        Assert.All(
            result.Slots.Where(slot => slot.TimeSlot != bookedSlot),
            slot => Assert.Equal("Available", slot.Status));
    }

    [Fact]
    public async Task GetAppointmentById_InvalidId_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.GetAppointmentByIdAsync(0));
    }

    [Fact]
    public async Task GetAppointmentById_NotFound_ShouldThrow()
    {
        _appointmentRepo
            .Setup(repo => repo.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Appointment?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.GetAppointmentByIdAsync(1));
    }

    [Fact]
    public async Task BookAppointment_NullDto_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.BookAppointmentAsync(null!));
    }

    [Fact]
    public async Task BookAppointment_PastDate_ShouldThrow()
    {
        var dto = ValidDto();
        dto.ScheduledDate = DateTime.Today.AddDays(-1);

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointment_PatientNotFound_ShouldThrow()
    {
        var dto = ValidDto();
        _patientRepo
            .Setup(repo => repo.GetByIdAsync(
                dto.PatientId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointment_DoctorInactive_ShouldThrow()
    {
        var dto = ValidDto();
        SetupPatient(dto.PatientId);
        var inactiveDoctor = CreateDoctor(dto.DoctorId);
        inactiveDoctor.IsActive = false;
        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(
                dto.DoctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(inactiveDoctor);

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointment_DoctorOnLeave_ShouldThrow()
    {
        var dto = ValidDto();
        var date = DateOnly.FromDateTime(dto.ScheduledDate);
        SetupPatient(dto.PatientId);
        SetupActiveDoctor(dto.DoctorId);
        _doctorLeaveRepo
            .Setup(repo => repo.GetLeaveForDateAsync(
                dto.DoctorId,
                date,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DoctorLeave
            {
                DoctorLeaveId = 20,
                DoctorId = dto.DoctorId,
                StartDate = date,
                EndDate = date.AddDays(1),
                Reason = "Conference"
            });

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.BookAppointmentAsync(dto));
        _appointmentRepo.Verify(
            repo => repo.Add(
                It.IsAny<Appointment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task BookAppointment_SameDoctorSameDay_ShouldThrow()
    {
        var dto = ValidDto();
        SetupPatient(dto.PatientId);
        SetupActiveDoctor(dto.DoctorId);
        _appointmentRepo
            .Setup(repo => repo.HasAppointmentWithDoctorOnSameDayAsync(
                dto.PatientId,
                dto.DoctorId,
                It.IsAny<DateOnly>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointment_PatientSlotConflict_ShouldThrow()
    {
        var dto = ValidDto();
        SetupPatient(dto.PatientId);
        SetupActiveDoctor(dto.DoctorId);
        _appointmentRepo
            .Setup(repo => repo.HasPatientSlotConflictAsync(
                dto.PatientId,
                It.IsAny<DateOnly>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task BookAppointment_DoctorSlotBooked_ShouldThrow()
    {
        var dto = ValidDto();
        SetupPatient(dto.PatientId);
        SetupActiveDoctor(dto.DoctorId);
        _appointmentRepo
            .Setup(repo => repo.IsDoctorSlotBookedAsync(
                dto.DoctorId,
                It.IsAny<DateOnly>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.BookAppointmentAsync(dto));
    }

    [Fact]
    public async Task UpdateStatus_InvalidId_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.UpdateAppointmentStatusAsync(
                0,
                AppointmentStatus.Pending));
    }

    [Fact]
    public async Task UpdateStatus_NotFound_ShouldThrow()
    {
        _appointmentRepo
            .Setup(repo => repo.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Appointment?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.UpdateAppointmentStatusAsync(
                1,
                AppointmentStatus.Pending));
    }

    [Fact]
    public async Task UpdateStatus_Completed_ShouldThrow()
    {
        var appointment = CreateAppointment(AppointmentStatus.Completed);
        _appointmentRepo
            .Setup(repo => repo.GetByIdAsync(
                appointment.AppointmentId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.UpdateAppointmentStatusAsync(
                appointment.AppointmentId,
                AppointmentStatus.Cancelled,
                "Reason"));
    }

    [Fact]
    public async Task UpdateStatus_CancelWithoutReason_ShouldThrow()
    {
        var appointment = CreateAppointment(AppointmentStatus.Pending);
        _appointmentRepo
            .Setup(repo => repo.GetByIdAsync(
                appointment.AppointmentId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.UpdateAppointmentStatusAsync(
                appointment.AppointmentId,
                AppointmentStatus.Cancelled));
    }

    [Fact]
    public async Task Delete_InvalidId_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.DeleteAppointmentAsync(0));
    }

    [Fact]
    public async Task Delete_NotCancelled_ShouldThrow()
    {
        var appointment = CreateAppointment(AppointmentStatus.Pending);
        _appointmentRepo
            .Setup(repo => repo.GetByIdAsync(
                appointment.AppointmentId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.DeleteAppointmentAsync(
                appointment.AppointmentId));
    }

    [Fact]
    public async Task Delete_Failure_ShouldThrow()
    {
        var appointment = CreateAppointment(AppointmentStatus.Cancelled);
        _appointmentRepo
            .Setup(repo => repo.GetByIdAsync(
                appointment.AppointmentId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);
        _appointmentRepo
            .Setup(repo => repo.DeleteAsync(appointment.AppointmentId))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.DeleteAppointmentAsync(
                appointment.AppointmentId));
    }

    [Fact]
    public async Task Delete_Valid_ShouldDeleteAndInvalidateCache()
    {
        var appointment = CreateAppointment(AppointmentStatus.Cancelled);
        _appointmentRepo
            .Setup(repo => repo.GetByIdAsync(
                appointment.AppointmentId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);
        _appointmentRepo
            .Setup(repo => repo.DeleteAsync(appointment.AppointmentId))
            .ReturnsAsync(true);

        await _service.DeleteAppointmentAsync(appointment.AppointmentId);

        _appointmentRepo.Verify(
            repo => repo.DeleteAsync(appointment.AppointmentId),
            Times.Once);
        _cache.Verify(
            cache => cache.RemoveAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetDoctorAvailability_InvalidDoctor_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.GetDoctorAvailabilityAsync(
                0,
                DateOnly.FromDateTime(DateTime.Today)));
    }

    [Fact]
    public async Task GetDoctorAvailability_DoctorNotFound_ShouldThrow()
    {
        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.GetDoctorAvailabilityAsync(
                1,
                DateOnly.FromDateTime(DateTime.Today)));
    }

    [Fact]
    public async Task GetDoctorAvailability_DoctorInactive_ShouldThrow()
    {
        var doctor = CreateDoctor(1);
        doctor.IsActive = false;
        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.GetDoctorAvailabilityAsync(
                1,
                DateOnly.FromDateTime(DateTime.Today.AddDays(1))));
    }

    [Fact]
    public async Task GetDoctorAvailability_DoctorOnLeave_ShouldDisableAllSlots()
    {
        var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        SetupActiveDoctor(1);
        _doctorLeaveRepo
            .Setup(repo => repo.GetLeaveForDateAsync(
                1,
                date,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DoctorLeave
            {
                DoctorLeaveId = 10,
                DoctorId = 1,
                StartDate = date,
                EndDate = date.AddDays(2),
                Reason = "Conference"
            });

        var result = await _service.GetDoctorAvailabilityAsync(1, date);

        Assert.True(result.IsDoctorOnLeave);
        Assert.All(result.Slots, slot =>
        {
            Assert.False(slot.IsAvailable);
            Assert.Equal("DoctorOnLeave", slot.Status);
        });
        _appointmentRepo.Verify(
            repo => repo.IsDoctorSlotBookedAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static AppointmentCreateDto ValidDto() => new()
    {
        PatientId = 1,
        DoctorId = 1,
        ScheduledDate = DateTime.Today.AddDays(1),
        TimeSlot = "10AM"
    };

    private static Patient CreatePatient(int patientId) => new()
    {
        PatientId = patientId,
        FullName = "Coverage Patient"
    };

    private static Doctor CreateDoctor(int doctorId) => new()
    {
        DoctorId = doctorId,
        FullName = "Dr Coverage",
        IsActive = true,
        Specialisation = SpecialisationType.Cardiologist
    };

    private static Appointment CreateAppointment(
        AppointmentStatus status) => new()
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 1,
            ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TimeSlot = "10AM",
            Status = status
        };

    private void SetupPatient(
        int patientId,
        Patient? patient = null)
    {
        _patientRepo
            .Setup(repo => repo.GetByIdAsync(
                patientId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient ?? CreatePatient(patientId));
        _patientRepo
            .Setup(repo => repo.GetPatientUserIdAsync(
                patientId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync($"patient-user-{patientId}");
    }

    private void SetupActiveDoctor(int doctorId)
    {
        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(
                doctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateDoctor(doctorId));
    }

    private void SetupNoBookingConflicts()
    {
        _appointmentRepo
            .Setup(repo => repo.HasAppointmentWithDoctorOnSameDayAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _appointmentRepo
            .Setup(repo => repo.HasPatientSlotConflictAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _appointmentRepo
            .Setup(repo => repo.IsDoctorSlotBookedAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
    }
}