using AutoMapper;
using HealthApp.Api.Data;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Dependencies;
using HealthApp.Api.Services.Impl;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HealthApp.Api.Tests;

public class DoctorLeaveServiceTests : IDisposable
{
    private readonly Mock<IDoctorLeaveRepository> _leaveRepo = new();
    private readonly Mock<IDoctorRepository> _doctorRepo = new();
    private readonly Mock<IAppointmentRepository> _appointmentRepo = new();
    private readonly Mock<IPatientRepository> _patientRepo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IPublishEndpoint> _publisher = new();
    private readonly Mock<IDistributedCache> _cache = new();
    private readonly Mock<ILogger<DoctorLeaveService>> _logger = new();
    private readonly HealthAppDbContext _context;
    private readonly DoctorLeaveService _service;

    public DoctorLeaveServiceTests()
    {
        var options = new DbContextOptionsBuilder<HealthAppDbContext>()
            .Options;
        _context = new HealthAppDbContext(options);

        var dependencies = new DoctorLeaveServiceDependencies(
            _context,
            _leaveRepo.Object,
            _doctorRepo.Object,
            _appointmentRepo.Object,
            _patientRepo.Object);

        _service = new DoctorLeaveService(
            dependencies,
            _mapper.Object,
            _publisher.Object,
            _cache.Object,
            _logger.Object);
    }

    [Fact]
    public void Constructor_NullDependencies_ShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(() => new DoctorLeaveService(
            null!,
            _mapper.Object,
            _publisher.Object,
            _cache.Object,
            _logger.Object));
    }

    [Fact]
    public async Task PreviewLeave_ReturnsPendingConfirmedAndTotalCounts()
    {
        const int doctorId = 7;
        var start = FutureDate(5);
        var end = start.AddDays(2);
        var dto = CreateLeaveDto(start, end);

        SetupValidDoctorAndNoOverlap(doctorId);
        _appointmentRepo
            .Setup(repo => repo.GetActiveAppointmentsForDoctorDateRangeAsync(
                doctorId,
                start,
                end,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Appointment>
            {
                CreateAppointment(1, doctorId, start, AppointmentStatus.Pending),
                CreateAppointment(
                    2,
                    doctorId,
                    start.AddDays(1),
                    AppointmentStatus.Pending),
                CreateAppointment(3, doctorId, end, AppointmentStatus.Confirmed),
                CreateAppointment(4, doctorId, end, AppointmentStatus.Cancelled)
            });

        var result = await _service.PreviewLeaveAsync(doctorId, dto);

        Assert.Equal(doctorId, result.DoctorId);
        Assert.Equal(start, result.StartDate);
        Assert.Equal(end, result.EndDate);
        Assert.Equal(2, result.PendingAppointmentCount);
        Assert.Equal(1, result.ConfirmedAppointmentCount);
        Assert.Equal(3, result.TotalAffectedAppointmentCount);
        Assert.Contains("3 active appointment(s)", result.Message);
    }

    [Fact]
    public async Task PreviewLeave_WhenNoAppointments_ReturnsZeroCounts()
    {
        const int doctorId = 4;
        var date = FutureDate(3);
        SetupValidDoctorAndNoOverlap(doctorId);
        _appointmentRepo
            .Setup(repo => repo.GetActiveAppointmentsForDoctorDateRangeAsync(
                doctorId,
                date,
                date,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var result = await _service.PreviewLeaveAsync(
            doctorId,
            CreateLeaveDto(date, date));

        Assert.Equal(0, result.PendingAppointmentCount);
        Assert.Equal(0, result.ConfirmedAppointmentCount);
        Assert.Equal(0, result.TotalAffectedAppointmentCount);
        Assert.Equal(
            "No active appointments will be affected by this leave.",
            result.Message);
    }

    [Fact]
    public async Task PreviewLeave_NullDto_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.PreviewLeaveAsync(1, null!));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task PreviewLeave_InvalidDoctorId_ShouldThrow(int doctorId)
    {
        var date = FutureDate(1);

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.PreviewLeaveAsync(
                doctorId,
                CreateLeaveDto(date, date)));

        _doctorRepo.Verify(
            repo => repo.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task PreviewLeave_PastStartDate_ShouldThrow()
    {
        var date = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));

        var exception = await Assert.ThrowsAsync<
            BusinessRuleViolationException>(
            () => _service.PreviewLeaveAsync(
                1,
                CreateLeaveDto(date, date)));

        Assert.Equal(
            "Leave start date cannot be in the past.",
            exception.Message);
    }

    [Fact]
    public async Task PreviewLeave_EndBeforeStart_ShouldThrow()
    {
        var start = FutureDate(5);

        var exception = await Assert.ThrowsAsync<
            BusinessRuleViolationException>(
            () => _service.PreviewLeaveAsync(
                1,
                CreateLeaveDto(start, start.AddDays(-1))));

        Assert.Equal(
            "Leave end date cannot be before the start date.",
            exception.Message);
    }

    [Theory]
    [InlineData(null, "Leave reason is required.")]
    [InlineData("", "Leave reason is required.")]
    [InlineData(" ", "Leave reason is required.")]
    [InlineData("ab", "Leave reason must be at least 3 characters long.")]
    [InlineData(" ab ", "Leave reason must be at least 3 characters long.")]
    public async Task PreviewLeave_InvalidReason_ShouldThrow(
        string? reason,
        string expectedMessage)
    {
        var date = FutureDate(2);
        var dto = CreateLeaveDto(date, date);
        dto.Reason = reason!;

        var exception = await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.PreviewLeaveAsync(1, dto));

        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task PreviewLeave_ReasonOver500Characters_ShouldThrow()
    {
        var date = FutureDate(2);
        var dto = CreateLeaveDto(date, date);
        dto.Reason = new string('a', 501);

        var exception = await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.PreviewLeaveAsync(1, dto));

        Assert.Equal(
            "Leave reason cannot exceed 500 characters.",
            exception.Message);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(500)]
    public async Task PreviewLeave_ReasonAtLengthBoundary_ShouldSucceed(
        int reasonLength)
    {
        const int doctorId = 9;
        var date = FutureDate(2);
        var dto = CreateLeaveDto(date, date);
        dto.Reason = new string('a', reasonLength);
        SetupValidDoctorAndNoOverlap(doctorId);
        _appointmentRepo
            .Setup(repo => repo.GetActiveAppointmentsForDoctorDateRangeAsync(
                doctorId,
                date,
                date,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var result = await _service.PreviewLeaveAsync(doctorId, dto);

        Assert.Equal(doctorId, result.DoctorId);
    }

    [Fact]
    public async Task PreviewLeave_DoctorNotFound_ShouldThrow()
    {
        const int doctorId = 99;
        var date = FutureDate(5);
        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(
                doctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.PreviewLeaveAsync(
                doctorId,
                CreateLeaveDto(date, date)));

        _leaveRepo.Verify(
            repo => repo.HasOverlappingLeaveAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<DateOnly>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task PreviewLeave_OverlappingLeave_ShouldThrow()
    {
        const int doctorId = 5;
        var start = FutureDate(4);
        var end = start.AddDays(2);
        SetupDoctor(doctorId);
        _leaveRepo
            .Setup(repo => repo.HasOverlappingLeaveAsync(
                doctorId,
                start,
                end,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.PreviewLeaveAsync(
                doctorId,
                CreateLeaveDto(start, end)));

        _appointmentRepo.Verify(
            repo => repo.GetActiveAppointmentsForDoctorDateRangeAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<DateOnly>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task PreviewLeave_ShouldPropagateCancellationToken()
    {
        const int doctorId = 3;
        var date = FutureDate(3);
        using var cancellation = new CancellationTokenSource();
        var token = cancellation.Token;
        SetupDoctor(doctorId);
        _leaveRepo
            .Setup(repo => repo.HasOverlappingLeaveAsync(
                doctorId,
                date,
                date,
                token))
            .ReturnsAsync(false);
        _appointmentRepo
            .Setup(repo => repo.GetActiveAppointmentsForDoctorDateRangeAsync(
                doctorId,
                date,
                date,
                token))
            .ReturnsAsync([]);

        await _service.PreviewLeaveAsync(
            doctorId,
            CreateLeaveDto(date, date),
            token);

        _doctorRepo.Verify(
            repo => repo.GetByIdAsync(doctorId, token),
            Times.Once);
        _leaveRepo.Verify(
            repo => repo.HasOverlappingLeaveAsync(
                doctorId,
                date,
                date,
                token),
            Times.Once);
        _appointmentRepo.Verify(
            repo => repo.GetActiveAppointmentsForDoctorDateRangeAsync(
                doctorId,
                date,
                date,
                token),
            Times.Once);
    }

    [Fact]
    public async Task CreateLeave_NullDto_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.CreateLeaveAsync(1, null!));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CreateLeave_InvalidDoctorId_ShouldThrow(int doctorId)
    {
        var date = FutureDate(1);

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.CreateLeaveAsync(
                doctorId,
                CreateLeaveDto(date, date)));
    }

    [Fact]
    public async Task CreateLeave_InvalidReason_ShouldThrowBeforeRepositoryCalls()
    {
        var date = FutureDate(2);
        var dto = CreateLeaveDto(date, date);
        dto.Reason = "ab";

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.CreateLeaveAsync(1, dto));

        _doctorRepo.Verify(
            repo => repo.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateLeave_DoctorNotFound_ShouldThrowBeforeTransaction()
    {
        const int doctorId = 99;
        var date = FutureDate(3);
        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(
                doctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.CreateLeaveAsync(
                doctorId,
                CreateLeaveDto(date, date)));
    }

    [Fact]
    public async Task CreateLeave_Overlap_ShouldThrowBeforeTransaction()
    {
        const int doctorId = 2;
        var date = FutureDate(3);
        SetupDoctor(doctorId);
        _leaveRepo
            .Setup(repo => repo.HasOverlappingLeaveAsync(
                doctorId,
                date,
                date,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.CreateLeaveAsync(
                doctorId,
                CreateLeaveDto(date, date)));

        _appointmentRepo.Verify(
            repo => repo.GetActiveAppointmentsForDoctorDateRangeAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<DateOnly>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetDoctorLeaves_ReturnsMappedLeaves()
    {
        const int doctorId = 3;
        SetupDoctor(doctorId);
        var leaves = new List<DoctorLeave>
        {
            new() { DoctorLeaveId = 1, DoctorId = doctorId },
            new() { DoctorLeaveId = 2, DoctorId = doctorId }
        };
        var dtos = new List<DoctorLeaveDto>
        {
            new() { DoctorLeaveId = 1 },
            new() { DoctorLeaveId = 2 }
        };
        _leaveRepo
            .Setup(repo => repo.GetByDoctorIdAsync(
                doctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(leaves);
        _mapper
            .Setup(mapper => mapper.Map<IEnumerable<DoctorLeaveDto>>(leaves))
            .Returns(dtos);

        var result = await _service.GetDoctorLeavesAsync(doctorId);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetDoctorLeaves_WhenNoLeaves_ShouldReturnEmpty()
    {
        const int doctorId = 3;
        SetupDoctor(doctorId);
        var leaves = new List<DoctorLeave>();
        _leaveRepo
            .Setup(repo => repo.GetByDoctorIdAsync(
                doctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(leaves);
        _mapper
            .Setup(mapper => mapper.Map<IEnumerable<DoctorLeaveDto>>(leaves))
            .Returns([]);

        var result = await _service.GetDoctorLeavesAsync(doctorId);

        Assert.Empty(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetDoctorLeaves_InvalidDoctorId_ShouldThrow(int doctorId)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.GetDoctorLeavesAsync(doctorId));
    }

    [Fact]
    public async Task GetDoctorLeaves_DoctorNotFound_ShouldThrow()
    {
        const int doctorId = 88;
        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(
                doctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.GetDoctorLeavesAsync(doctorId));

        _leaveRepo.Verify(
            repo => repo.GetByDoctorIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetDoctorLeaves_ShouldPropagateCancellationToken()
    {
        const int doctorId = 3;
        using var cancellation = new CancellationTokenSource();
        var token = cancellation.Token;
        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(doctorId, token))
            .ReturnsAsync(CreateDoctor(doctorId));
        var leaves = new List<DoctorLeave>();
        _leaveRepo
            .Setup(repo => repo.GetByDoctorIdAsync(doctorId, token))
            .ReturnsAsync(leaves);
        _mapper
            .Setup(mapper => mapper.Map<IEnumerable<DoctorLeaveDto>>(leaves))
            .Returns([]);

        await _service.GetDoctorLeavesAsync(doctorId, token);

        _leaveRepo.Verify(
            repo => repo.GetByDoctorIdAsync(doctorId, token),
            Times.Once);
    }

    [Fact]
    public async Task GetLeaveForDate_WhenFound_ReturnsMappedLeave()
    {
        const int doctorId = 4;
        var date = FutureDate(4);
        var leave = new DoctorLeave
        {
            DoctorLeaveId = 10,
            DoctorId = doctorId,
            StartDate = date,
            EndDate = date
        };
        var dto = new DoctorLeaveDto
        {
            DoctorLeaveId = 10,
            DoctorId = doctorId,
            StartDate = date,
            EndDate = date
        };
        SetupDoctor(doctorId);
        _leaveRepo
            .Setup(repo => repo.GetLeaveForDateAsync(
                doctorId,
                date,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(leave);
        _mapper
            .Setup(mapper => mapper.Map<DoctorLeaveDto>(leave))
            .Returns(dto);

        var result = await _service.GetLeaveForDateAsync(doctorId, date);

        Assert.NotNull(result);
        Assert.Equal(10, result.DoctorLeaveId);
    }

    [Fact]
    public async Task GetLeaveForDate_WhenMissing_ReturnsNullAndDoesNotMap()
    {
        const int doctorId = 4;
        var date = FutureDate(4);
        SetupDoctor(doctorId);
        _leaveRepo
            .Setup(repo => repo.GetLeaveForDateAsync(
                doctorId,
                date,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((DoctorLeave?)null);

        var result = await _service.GetLeaveForDateAsync(doctorId, date);

        Assert.Null(result);
        _mapper.Verify(
            mapper => mapper.Map<DoctorLeaveDto>(It.IsAny<DoctorLeave>()),
            Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetLeaveForDate_InvalidDoctorId_ShouldThrow(int doctorId)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.GetLeaveForDateAsync(
                doctorId,
                FutureDate(1)));
    }

    [Fact]
    public async Task GetLeaveForDate_DoctorNotFound_ShouldThrow()
    {
        const int doctorId = 77;
        var date = FutureDate(1);
        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(
                doctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.GetLeaveForDateAsync(doctorId, date));

        _leaveRepo.Verify(
            repo => repo.GetLeaveForDateAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task IsDoctorOnLeave_ReturnsRepositoryResult(bool expected)
    {
        const int doctorId = 6;
        var date = FutureDate(2);
        SetupDoctor(doctorId);
        _leaveRepo
            .Setup(repo => repo.IsDoctorOnLeaveAsync(
                doctorId,
                date,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _service.IsDoctorOnLeaveAsync(doctorId, date);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task IsDoctorOnLeave_InvalidDoctorId_ShouldThrow(int doctorId)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.IsDoctorOnLeaveAsync(
                doctorId,
                FutureDate(1)));
    }

    [Fact]
    public async Task IsDoctorOnLeave_DoctorNotFound_ShouldThrow()
    {
        const int doctorId = 66;
        var date = FutureDate(1);
        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(
                doctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.IsDoctorOnLeaveAsync(doctorId, date));

        _leaveRepo.Verify(
            repo => repo.IsDoctorOnLeaveAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task IsDoctorOnLeave_ShouldPropagateCancellationToken()
    {
        const int doctorId = 6;
        var date = FutureDate(2);
        using var cancellation = new CancellationTokenSource();
        var token = cancellation.Token;
        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(doctorId, token))
            .ReturnsAsync(CreateDoctor(doctorId));
        _leaveRepo
            .Setup(repo => repo.IsDoctorOnLeaveAsync(
                doctorId,
                date,
                token))
            .ReturnsAsync(true);

        var result = await _service.IsDoctorOnLeaveAsync(
            doctorId,
            date,
            token);

        Assert.True(result);
        _leaveRepo.Verify(
            repo => repo.IsDoctorOnLeaveAsync(doctorId, date, token),
            Times.Once);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    private void SetupDoctor(int doctorId)
    {
        _doctorRepo
            .Setup(repo => repo.GetByIdAsync(
                doctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateDoctor(doctorId));
    }

    private void SetupValidDoctorAndNoOverlap(int doctorId)
    {
        SetupDoctor(doctorId);
        _leaveRepo
            .Setup(repo => repo.HasOverlappingLeaveAsync(
                doctorId,
                It.IsAny<DateOnly>(),
                It.IsAny<DateOnly>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
    }

    private static DateOnly FutureDate(int days) =>
        DateOnly.FromDateTime(DateTime.Today.AddDays(days));

    private static DoctorLeaveCreateDto CreateLeaveDto(
        DateOnly startDate,
        DateOnly endDate) => new()
        {
            StartDate = startDate,
            EndDate = endDate,
            Reason = "Medical conference"
        };

    private static Doctor CreateDoctor(int doctorId) => new()
    {
        DoctorId = doctorId,
        FullName = "Dr. Test Doctor",
        IsActive = true
    };

    private static Appointment CreateAppointment(
        int appointmentId,
        int doctorId,
        DateOnly scheduledDate,
        AppointmentStatus status) => new()
        {
            AppointmentId = appointmentId,
            DoctorId = doctorId,
            PatientId = appointmentId + 100,
            ScheduledDate = scheduledDate,
            TimeSlot = "09:00 AM - 09:30 AM",
            Status = status
        };
}
