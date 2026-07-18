using AutoMapper;
using HealthApp.API.Data;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Reflection;
using System.Security.Claims;

namespace HealthApp.API.Tests;

public sealed class DoctorLeaveServiceTests : IDisposable
{
    private const int DoctorId = 8;
    private const int MissingDoctorId = 999;
    private const string UserId = "doctor-user-8";
    private const string CancellationPrefix =
        "Cancelled because the doctor is unavailable due to scheduled leave: ";

    private readonly Mock<IDoctorLeaveRepository> doctorLeaveRepositoryMock = new();
    private readonly Mock<IDoctorRepository> doctorRepositoryMock = new();
    private readonly Mock<IAppointmentRepository> appointmentRepositoryMock = new();
    private readonly Mock<IHttpContextAccessor> httpContextAccessorMock = new();
    private readonly Mock<IDistributedCache> distributedCacheMock = new();
    private readonly Mock<IMapper> mapperMock = new();

    private readonly HealthAppDbContext dbContext;
    private readonly DoctorLeaveService doctorLeaveService;
    private readonly Doctor doctor;

    public enum InvalidLeaveRequestCase
    {
        MissingStartDate,
        MissingEndDate,
        PastStartDate,
        EndDateBeforeStartDate,
        MissingReason,
        ReasonTooShort,
        ReasonTooLong
    }

    public DoctorLeaveServiceTests()
    {
        var options = new DbContextOptionsBuilder<HealthAppDbContext>()
            .UseInMemoryDatabase($"DoctorLeaveServiceTests-{Guid.NewGuid()}")
            .ConfigureWarnings(configuration =>
                configuration.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        dbContext = new HealthAppDbContext(options);

        doctor = new Doctor
        {
            DoctorId = DoctorId,
            IsActive = true
        };

        SetCurrentUser(includeNameIdentifier: true);
        ConfigureDefaultRepositories();
        ConfigureCacheSuccess();
        ConfigureMapper();

        doctorLeaveService = new DoctorLeaveService(
            doctorLeaveRepositoryMock.Object,
            doctorRepositoryMock.Object,
            appointmentRepositoryMock.Object,
            dbContext,
            httpContextAccessorMock.Object,
            distributedCacheMock.Object,
            mapperMock.Object,
            NullLogger<DoctorLeaveService>.Instance);
    }

    public void Dispose()
    {
        dbContext.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenDtoIsNull_ThrowsBusinessRuleException()
    {
        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            doctorLeaveService.PreviewMyLeaveImpactAsync(null!));

        Assert.Equal("Leave details are required.", exception.Message);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenDoctorIsInactive_ThrowsForbiddenAccessException()
    {
        doctor.IsActive = false;

        var exception = await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            doctorLeaveService.PreviewMyLeaveImpactAsync(ValidRequest()));

        Assert.Equal("Inactive doctors are not allowed to create leave.", exception.Message);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenNoAppointmentsExist_ReturnsNoConfirmation()
    {
        appointmentRepositoryMock
            .Setup(repository => repository.CountActiveAppointmentsInDateRangeAsync(
                DoctorId,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        var result = await doctorLeaveService.PreviewMyLeaveImpactAsync(ValidRequest());

        Assert.Equal(0, result.AffectedAppointmentCount);
        Assert.False(result.RequiresConfirmation);
        Assert.Equal("No active appointments are affected.", result.Message);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenOneAppointmentExists_UsesSingularMessage()
    {
        appointmentRepositoryMock
            .Setup(repository => repository.CountActiveAppointmentsInDateRangeAsync(
                DoctorId,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await doctorLeaveService.PreviewMyLeaveImpactAsync(ValidRequest());

        Assert.Equal(1, result.AffectedAppointmentCount);
        Assert.True(result.RequiresConfirmation);
        Assert.Equal("This leave affects 1 active appointment.", result.Message);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenMultipleAppointmentsExist_UsesPluralMessage()
    {
        appointmentRepositoryMock
            .Setup(repository => repository.CountActiveAppointmentsInDateRangeAsync(
                DoctorId,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(3);

        var result = await doctorLeaveService.PreviewMyLeaveImpactAsync(ValidRequest());

        Assert.Equal(3, result.AffectedAppointmentCount);
        Assert.True(result.RequiresConfirmation);
        Assert.Equal("This leave affects 3 active appointments.", result.Message);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenLeaveOverlaps_ThrowsConflictException()
    {
        doctorLeaveRepositoryMock
            .Setup(repository => repository.HasOverlappingLeaveAsync(
                DoctorId,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<ConflictException>(() =>
            doctorLeaveService.PreviewMyLeaveImpactAsync(ValidRequest()));

        Assert.Equal(
            "The selected leave period overlaps with an existing leave.",
            exception.Message);
    }

    [Theory]
    [MemberData(nameof(InvalidLeaveRequestCases))]
    public async Task PreviewMyLeaveImpactAsync_WhenRequestIsInvalid_ThrowsExpectedMessage(
    InvalidLeaveRequestCase requestCase,
    string expectedMessage)
    {
        var request =
            CreateInvalidLeaveRequest(requestCase);

        var exception =
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                doctorLeaveService.PreviewMyLeaveImpactAsync(request));

        Assert.Equal(
            expectedMessage,
            exception.Message);
    }

    [Fact]
    public async Task CreateMyLeaveAsync_WhenDtoIsNull_ThrowsBusinessRuleException()
    {
        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            doctorLeaveService.CreateMyLeaveAsync(null!));

        Assert.Equal("Leave details are required.", exception.Message);
    }

    [Fact]
    public async Task CreateMyLeaveAsync_WhenDoctorIsInactive_ThrowsForbiddenAccessException()
    {
        doctor.IsActive = false;

        var exception = await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            doctorLeaveService.CreateMyLeaveAsync(ValidRequest()));

        Assert.Equal("Inactive doctors are not allowed to create leave.", exception.Message);
    }

    [Fact]
    public async Task CreateMyLeaveAsync_WhenNoAppointmentsExist_CreatesLeave()
    {
        var request = ValidRequest(days: 2);

        var result = await doctorLeaveService.CreateMyLeaveAsync(request);

        Assert.Equal(0, result.CancelledAppointmentCount);
        Assert.Equal("Leave created successfully.", result.Message);
        Assert.Equal(request.StartDate.Date, result.Leave.StartDate);
        Assert.Equal(request.EndDate.Date, result.Leave.EndDate);
        Assert.Equal(request.Reason, result.Leave.Reason);
        Assert.Single(dbContext.DoctorLeaves);

        distributedCacheMock.Verify(cache => cache.RemoveAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Exactly(3));
    }

    [Fact]
    public async Task CreateMyLeaveAsync_WhenAppointmentExistsWithoutConfirmation_ThrowsConflictAndDoesNotSaveLeave()
    {
        appointmentRepositoryMock
            .Setup(repository => repository.GetActiveAppointmentsInDateRangeAsync(
                DoctorId,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Appointment>
            {
                Appointment(AppointmentStatus.Pending, 101)
            });

        var request = ValidRequest();
        request.ConfirmAppointmentCancellation = false;

        var exception = await Assert.ThrowsAsync<ConflictException>(() =>
            doctorLeaveService.CreateMyLeaveAsync(request));

        Assert.Equal(
            "This leave affects 1 active appointment. Confirmation is required before creating the leave.",
            exception.Message);
        Assert.Empty(dbContext.DoctorLeaves);
    }

    [Fact]
    public async Task CreateMyLeaveAsync_WhenAppointmentsExistWithConfirmation_CancelsAppointments()
    {
        var pending = Appointment(AppointmentStatus.Pending, 101);
        var confirmed = Appointment(AppointmentStatus.Confirmed, 102);

        appointmentRepositoryMock
            .Setup(repository => repository.GetActiveAppointmentsInDateRangeAsync(
                DoctorId,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Appointment> { pending, confirmed });

        var request = ValidRequest();
        request.ConfirmAppointmentCancellation = true;

        var result = await doctorLeaveService.CreateMyLeaveAsync(request);

        Assert.Equal(2, result.CancelledAppointmentCount);
        Assert.Equal(
            "Leave created and 2 affected appointments cancelled.",
            result.Message);

        Assert.All(new[] { pending, confirmed }, appointment =>
        {
            Assert.Equal(AppointmentStatus.Cancelled.ToString(), appointment.Status);
            Assert.Equal(CancellationPrefix + request.Reason, appointment.CancellationReason);
        });
    }

    [Fact]
    public async Task CreateMyLeaveAsync_WhenOneAppointmentExists_UsesSingularMessage()
    {
        appointmentRepositoryMock
            .Setup(repository => repository.GetActiveAppointmentsInDateRangeAsync(
                DoctorId,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Appointment>
            {
                Appointment(AppointmentStatus.Pending, 101)
            });

        var request = ValidRequest();
        request.ConfirmAppointmentCancellation = true;

        var result = await doctorLeaveService.CreateMyLeaveAsync(request);

        Assert.Equal(
            "Leave created and 1 affected appointment cancelled.",
            result.Message);
    }

    [Fact]
    public async Task CreateMyLeaveAsync_WhenReasonIsLong_TruncatesCancellationReasonToTwoHundredCharacters()
    {
        var appointment = Appointment(AppointmentStatus.Pending, 101);

        appointmentRepositoryMock
            .Setup(repository => repository.GetActiveAppointmentsInDateRangeAsync(
                DoctorId,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Appointment> { appointment });

        var request = ValidRequest();
        request.Reason = new string('A', 500);
        request.ConfirmAppointmentCancellation = true;

        await doctorLeaveService.CreateMyLeaveAsync(request);

        Assert.NotNull(appointment.CancellationReason);
        Assert.Equal(200, appointment.CancellationReason!.Length);
        Assert.StartsWith(CancellationPrefix, appointment.CancellationReason);
    }

    [Fact]
    public async Task CreateMyLeaveAsync_WhenOverlapIsDetectedInsideTransaction_ThrowsConflict()
    {
        doctorLeaveRepositoryMock
            .Setup(repository => repository.HasOverlappingLeaveAsync(
                DoctorId,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ConflictException>(() =>
            doctorLeaveService.CreateMyLeaveAsync(ValidRequest()));

        Assert.Empty(dbContext.DoctorLeaves);
    }

    [Fact]
    public async Task CreateMyLeaveAsync_WhenCacheRemovalThrows_StillReturnsSuccessfulResult()
    {
        distributedCacheMock
            .Setup(cache => cache.RemoveAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Garnet unavailable"));

        var result = await doctorLeaveService.CreateMyLeaveAsync(ValidRequest());

        Assert.Equal("Leave created successfully.", result.Message);
        Assert.Single(dbContext.DoctorLeaves);
    }

   

    [Fact]
    public async Task GetMyLeaveHistoryAsync_ReturnsMappedDoctorHistory()
    {
        var leaves = new List<DoctorLeave> { LeaveEntity(1) };
        var expected = new List<DoctorLeaveDto> { MapLeave(leaves[0]) };

        doctorLeaveRepositoryMock
            .Setup(repository => repository.GetByDoctorIdAsync(
                DoctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(leaves);
        mapperMock.Setup(mapper => mapper.Map<List<DoctorLeaveDto>>(leaves))
            .Returns(expected);

        var result = await doctorLeaveService.GetMyLeaveHistoryAsync();

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetDoctorLeaveHistoryAsync_WhenDoctorExists_ReturnsMappedHistory()
    {
        var leaves = new List<DoctorLeave> { LeaveEntity(1) };
        var expected = new List<DoctorLeaveDto> { MapLeave(leaves[0]) };

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                DoctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);
        doctorLeaveRepositoryMock
            .Setup(repository => repository.GetByDoctorIdAsync(
                DoctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(leaves);
        mapperMock.Setup(mapper => mapper.Map<List<DoctorLeaveDto>>(leaves))
            .Returns(expected);

        var result = await doctorLeaveService.GetDoctorLeaveHistoryAsync(DoctorId);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetDoctorLeaveHistoryAsync_WhenDoctorIdIsInvalid_ThrowsBusinessRuleException()
    {
        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            doctorLeaveService.GetDoctorLeaveHistoryAsync(0));

        Assert.Equal("Please provide a valid doctor reference.", exception.Message);
    }

    [Fact]
    public async Task GetDoctorLeaveHistoryAsync_WhenDoctorDoesNotExist_ThrowsEntityNotFoundException()
    {
        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                MissingDoctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            doctorLeaveService.GetDoctorLeaveHistoryAsync(MissingDoctorId));
    }

    [Fact]
    public async Task GetDoctorLeaveStatusAsync_WhenNoLeaveExists_ReturnsAvailableStatus()
    {
        var date = DateTime.Today.AddDays(5);

        doctorLeaveRepositoryMock
            .Setup(repository => repository.GetLeaveForDateAsync(
                DoctorId,
                date.Date,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((DoctorLeave?)null);

        var result = await doctorLeaveService.GetDoctorLeaveStatusAsync(DoctorId, date);

        Assert.Equal(DoctorId, result.DoctorId);
        Assert.Equal(date.Date, result.Date);
        Assert.False(result.IsOnLeave);
        Assert.Null(result.Leave);
        Assert.Equal("Doctor is available on the selected date.", result.Message);
    }

    [Fact]
    public async Task GetDoctorLeaveStatusAsync_WhenLeaveExists_ReturnsOnLeaveStatus()
    {
        var leave = LeaveEntity(1);
        var mapped = MapLeave(leave);

        doctorLeaveRepositoryMock
            .Setup(repository => repository.GetLeaveForDateAsync(
                DoctorId,
                leave.StartDate.Date,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(leave);
        mapperMock.Setup(mapper => mapper.Map<DoctorLeaveDto>(leave)).Returns(mapped);

        var result = await doctorLeaveService.GetDoctorLeaveStatusAsync(
            DoctorId,
            leave.StartDate);

        Assert.True(result.IsOnLeave);
        Assert.Same(mapped, result.Leave);
        Assert.Equal("Doctor is on leave on the selected date.", result.Message);
    }

    [Fact]
    public async Task GetMyLeaveHistoryAsync_WhenIdentityIsMissing_ThrowsForbiddenAccessException()
    {
        SetCurrentUser(includeNameIdentifier: false);

        var exception = await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            doctorLeaveService.GetMyLeaveHistoryAsync());

        Assert.Equal("Unable to identify the logged-in doctor.", exception.Message);
    }

    [Fact]
    public async Task GetMyLeaveHistoryAsync_WhenDoctorRecordIsMissing_ThrowsEntityNotFoundException()
    {
        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync(
                UserId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            doctorLeaveService.GetMyLeaveHistoryAsync());
    }

    [Fact]
    public void BuildAppointmentCancellationReason_WhenReasonIsShort_ReturnsPrefixAndReason()
    {
        var method = GetPrivateStaticMethod("BuildAppointmentCancellationReason");

        var result = Assert.IsType<string>(method.Invoke(null, new object[] { "  Training  " }));

        Assert.Equal(CancellationPrefix + "Training", result);
    }

    [Fact]
    public void GetDoctorAvailabilityCacheKey_ReturnsExpectedKey()
    {
        var method = GetPrivateStaticMethod("GetDoctorAvailabilityCacheKey");
        var date = new DateTime(2026, 8, 14);

        var result = Assert.IsType<string>(method.Invoke(null, new object[] { DoctorId, date }));

        Assert.Equal("doctors:8:availability:2026-08-14", result);
    }

    [Theory]
    [InlineData(1, "")]
    [InlineData(0, "s")]
    [InlineData(2, "s")]
    public void GetPluralSuffix_ReturnsExpectedSuffix(int count, string expected)
    {
        var method = GetPrivateStaticMethod("GetPluralSuffix");

        var result = Assert.IsType<string>(method.Invoke(null, new object[] { count }));

        Assert.Equal(expected, result);
    }

    public static TheoryData<
    InvalidLeaveRequestCase,
    string> InvalidLeaveRequestCases =>
    new()
    {
        {
            InvalidLeaveRequestCase.MissingStartDate,
            "Leave start date is required."
        },
        {
            InvalidLeaveRequestCase.MissingEndDate,
            "Leave end date is required."
        },
        {
            InvalidLeaveRequestCase.PastStartDate,
            "Leave start date cannot be in the past."
        },
        {
            InvalidLeaveRequestCase.EndDateBeforeStartDate,
            "Leave end date cannot be before the start date."
        },
        {
            InvalidLeaveRequestCase.MissingReason,
            "Leave reason is required."
        },
        {
            InvalidLeaveRequestCase.ReasonTooShort,
            "Leave reason must contain at least 3 characters."
        },
        {
            InvalidLeaveRequestCase.ReasonTooLong,
            "Leave reason must not exceed 500 characters."
        }
    };

    private void ConfigureDefaultRepositories()
    {
        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync(
                UserId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                DoctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        doctorLeaveRepositoryMock
            .Setup(repository => repository.HasOverlappingLeaveAsync(
                DoctorId,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        doctorLeaveRepositoryMock
            .Setup(repository => repository.GetByDoctorIdAsync(
                DoctorId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DoctorLeave>());

        doctorLeaveRepositoryMock
            .Setup(repository => repository.GetLeaveForDateAsync(
                DoctorId,
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((DoctorLeave?)null);

        appointmentRepositoryMock
            .Setup(repository => repository.CountActiveAppointmentsInDateRangeAsync(
                DoctorId,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        appointmentRepositoryMock
            .Setup(repository => repository.GetActiveAppointmentsInDateRangeAsync(
                DoctorId,
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Appointment>());
    }

    private void ConfigureCacheSuccess()
    {
        distributedCacheMock
            .Setup(cache => cache.RemoveAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    private void ConfigureMapper()
    {
        mapperMock
            .Setup(mapper => mapper.Map<DoctorLeaveDto>(It.IsAny<DoctorLeave>()))
            .Returns((DoctorLeave leave) => MapLeave(leave));
    }

    private void SetCurrentUser(bool includeNameIdentifier)
    {
        var claims = includeNameIdentifier
            ? new[] { new Claim(ClaimTypes.NameIdentifier, UserId) }
            : Array.Empty<Claim>();

        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthentication"))
        };

        httpContextAccessorMock.SetupGet(accessor => accessor.HttpContext).Returns(context);
    }

    private static CreateDoctorLeaveDto ValidRequest(int days = 1) => new()
    {
        StartDate = DateTime.Today.AddDays(10),
        EndDate = DateTime.Today.AddDays(10 + days),
        Reason = "Personal Leave",
        ConfirmAppointmentCancellation = false
    };

    private static Appointment Appointment(AppointmentStatus status, int appointmentId) => new()
    {
        AppointmentId = appointmentId,
        DoctorId = DoctorId,
        ScheduledDate = DateTime.Today.AddDays(10),
        TimeSlots = "09:00 AM - 09:30 AM",
        Status = status.ToString()
    };

    private DoctorLeave LeaveEntity(int id) => new()
    {
        DoctorLeaveId = id,
        DoctorId = DoctorId,
        Doctor = doctor,
        StartDate = DateTime.Today.AddDays(10),
        EndDate = DateTime.Today.AddDays(11),
        Reason = "Personal Leave",
        CreatedDate = DateTime.Now
    };

    private static DoctorLeaveDto MapLeave(DoctorLeave leave) => new()
    {
        DoctorLeaveId = leave.DoctorLeaveId,
        DoctorId = leave.DoctorId,
        StartDate = leave.StartDate,
        EndDate = leave.EndDate,
        Reason = leave.Reason,
        CreatedDate = leave.CreatedDate
    };

    private static MethodInfo GetPrivateStaticMethod(string name)
    {
        return typeof(DoctorLeaveService).GetMethod(
                   name,
                   BindingFlags.NonPublic | BindingFlags.Static)
               ?? throw new InvalidOperationException($"Private method {name} was not found.");
    }

    private static CreateDoctorLeaveDto
    CreateInvalidLeaveRequest(
        InvalidLeaveRequestCase requestCase)
    {
        var request = ValidRequest();

        switch (requestCase)
        {
            case InvalidLeaveRequestCase.MissingStartDate:
                request.StartDate = default;
                break;

            case InvalidLeaveRequestCase.MissingEndDate:
                request.EndDate = default;
                break;

            case InvalidLeaveRequestCase.PastStartDate:
                request.StartDate =
                    DateTime.Today.AddDays(-1);

                request.EndDate =
                    DateTime.Today.AddDays(2);
                break;

            case InvalidLeaveRequestCase.EndDateBeforeStartDate:
                request.StartDate =
                    DateTime.Today.AddDays(3);

                request.EndDate =
                    DateTime.Today.AddDays(2);
                break;

            case InvalidLeaveRequestCase.MissingReason:
                request.Reason = "   ";
                break;

            case InvalidLeaveRequestCase.ReasonTooShort:
                request.Reason = "AB";
                break;

            case InvalidLeaveRequestCase.ReasonTooLong:
                request.Reason =
                    new string('A', 501);
                break;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(requestCase),
                    requestCase,
                    "Unsupported invalid leave request case.");
        }

        return request;
    }
}
