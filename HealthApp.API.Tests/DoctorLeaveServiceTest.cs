using AutoMapper;
using HealthApp.API.Data;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Security.Claims;

namespace HealthApp.API.Tests;

public class DoctorLeaveServiceTests
{
    private const int DoctorId = 8;
    private const string UserId = "doctor-user-8";

    private readonly Mock<IDoctorLeaveRepository>
        doctorLeaveRepositoryMock = new();

    private readonly Mock<IDoctorRepository>
        doctorRepositoryMock = new();

    private readonly Mock<IAppointmentRepository>
        appointmentRepositoryMock = new();

    private readonly Mock<IDistributedCache>
        distributedCacheMock = new();

    private readonly Mock<IMapper>
        mapperMock = new();

    private readonly HealthAppDbContext dbContext;

    private readonly Doctor doctor;

    public DoctorLeaveServiceTests()
    {
        var options =
            new DbContextOptionsBuilder<HealthAppDbContext>()
                .UseInMemoryDatabase(
                    $"DoctorLeaveTests-{Guid.NewGuid()}")
                .Options;

        dbContext = new HealthAppDbContext(options);

        doctor = new Doctor
        {
            DoctorId = DoctorId,
            IsActive = true
        };

        doctorRepositoryMock
            .Setup(repository =>
                repository.GetByUserIdAsync(
                    UserId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        doctorRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(
                    DoctorId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        doctorLeaveRepositoryMock
            .Setup(repository =>
                repository.HasOverlappingLeaveAsync(
                    DoctorId,
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenNoAppointmentsExist_ReturnsNoConfirmation()
    {
        // Arrange
        appointmentRepositoryMock
            .Setup(repository =>
                repository.CountActiveAppointmentsInDateRangeAsync(
                    DoctorId,
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        var service = CreateService();

        var request = CreateValidRequest();

        // Act
        var result =
            await service.PreviewMyLeaveImpactAsync(request);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(
            0,
            result.AffectedAppointmentCount);

        Assert.False(result.RequiresConfirmation);

        Assert.Equal(
            "No active appointments are affected.",
            result.Message);

        appointmentRepositoryMock.Verify(
            repository =>
                repository.CountActiveAppointmentsInDateRangeAsync(
                    DoctorId,
                    request.StartDate.Date,
                    request.EndDate.Date,
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenAppointmentsExist_RequiresConfirmation()
    {
        // Arrange
        appointmentRepositoryMock
            .Setup(repository =>
                repository.CountActiveAppointmentsInDateRangeAsync(
                    DoctorId,
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);

        var service = CreateService();

        // Act
        var result =
            await service.PreviewMyLeaveImpactAsync(
                CreateValidRequest());

        // Assert
        Assert.Equal(
            2,
            result.AffectedAppointmentCount);

        Assert.True(result.RequiresConfirmation);

        Assert.Equal(
            "This leave affects 2 active appointments.",
            result.Message);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenOneAppointmentExists_UsesSingularMessage()
    {
        // Arrange
        appointmentRepositoryMock
            .Setup(repository =>
                repository.CountActiveAppointmentsInDateRangeAsync(
                    DoctorId,
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = CreateService();

        // Act
        var result =
            await service.PreviewMyLeaveImpactAsync(
                CreateValidRequest());

        // Assert
        Assert.Equal(
            "This leave affects 1 active appointment.",
            result.Message);

        Assert.True(result.RequiresConfirmation);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenLeaveOverlaps_ThrowsConflictException()
    {
        // Arrange
        doctorLeaveRepositoryMock
            .Setup(repository =>
                repository.HasOverlappingLeaveAsync(
                    DoctorId,
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var service = CreateService();

        // Act
        var action = async () =>
            await service.PreviewMyLeaveImpactAsync(
                CreateValidRequest());

        // Assert
        var exception =
            await Assert.ThrowsAsync<ConflictException>(
                action);

        Assert.Equal(
            "The selected leave period overlaps with an existing leave.",
            exception.Message);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenDoctorIsInactive_ThrowsForbiddenAccessException()
    {
        // Arrange
        doctor.IsActive = false;

        var service = CreateService();

        // Act
        var action = async () =>
            await service.PreviewMyLeaveImpactAsync(
                CreateValidRequest());

        // Assert
        var exception =
            await Assert.ThrowsAsync<
                ForbiddenAccessException>(
                action);

        Assert.Equal(
            "Inactive doctors are not allowed to create leave.",
            exception.Message);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenStartDateIsPast_ThrowsBusinessRuleException()
    {
        // Arrange
        var service = CreateService();

        var request = CreateValidRequest();

        request.StartDate =
            DateTime.Today.AddDays(-1);

        // Act
        var action = async () =>
            await service.PreviewMyLeaveImpactAsync(
                request);

        // Assert
        var exception =
            await Assert.ThrowsAsync<
                BusinessRuleException>(
                action);

        Assert.Equal(
            "Leave start date cannot be in the past.",
            exception.Message);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenEndDateIsBeforeStartDate_ThrowsBusinessRuleException()
    {
        // Arrange
        var service = CreateService();

        var request = CreateValidRequest();

        request.StartDate =
            DateTime.Today.AddDays(10);

        request.EndDate =
            DateTime.Today.AddDays(9);

        // Act
        var action = async () =>
            await service.PreviewMyLeaveImpactAsync(
                request);

        // Assert
        var exception =
            await Assert.ThrowsAsync<
                BusinessRuleException>(
                action);

        Assert.Equal(
            "Leave end date cannot be before the start date.",
            exception.Message);
    }

    [Fact]
    public async Task PreviewMyLeaveImpactAsync_WhenReasonIsEmpty_ThrowsBusinessRuleException()
    {
        // Arrange
        var service = CreateService();

        var request = CreateValidRequest();

        request.Reason = " ";

        // Act
        var action = async () =>
            await service.PreviewMyLeaveImpactAsync(
                request);

        // Assert
        var exception =
            await Assert.ThrowsAsync<
                BusinessRuleException>(
                action);

        Assert.Equal(
            "Leave reason is required.",
            exception.Message);
    }

    [Fact]
    public async Task GetDoctorLeaveStatusAsync_WhenDoctorIsAvailable_ReturnsAvailableStatus()
    {
        // Arrange
        var selectedDate =
            DateTime.Today.AddDays(5);

        doctorLeaveRepositoryMock
            .Setup(repository =>
                repository.GetLeaveForDateAsync(
                    DoctorId,
                    selectedDate.Date,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((DoctorLeave?)null);

        var service = CreateService();

        // Act
        var result =
            await service.GetDoctorLeaveStatusAsync(
                DoctorId,
                selectedDate);

        // Assert
        Assert.Equal(DoctorId, result.DoctorId);
        Assert.Equal(selectedDate.Date, result.Date);
        Assert.False(result.IsOnLeave);
        Assert.Null(result.Leave);

        Assert.Equal(
            "Doctor is available on the selected date.",
            result.Message);
    }

    [Fact]
    public async Task GetDoctorLeaveStatusAsync_WhenDoctorIsOnLeave_ReturnsLeaveStatus()
    {
        // Arrange
        var selectedDate =
            DateTime.Today.AddDays(5);

        var doctorLeave = new DoctorLeave
        {
            DoctorLeaveId = 1,
            DoctorId = DoctorId,
            StartDate = selectedDate,
            EndDate = selectedDate.AddDays(2),
            Reason = "Personal Leave",
            CreatedDate = DateTime.Now
        };

        var mappedLeave = new DoctorLeaveDto
        {
            DoctorLeaveId =
                doctorLeave.DoctorLeaveId,

            DoctorId =
                doctorLeave.DoctorId,

            StartDate =
                doctorLeave.StartDate,

            EndDate =
                doctorLeave.EndDate,

            Reason =
                doctorLeave.Reason,

            CreatedDate =
                doctorLeave.CreatedDate
        };

        doctorLeaveRepositoryMock
            .Setup(repository =>
                repository.GetLeaveForDateAsync(
                    DoctorId,
                    selectedDate.Date,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctorLeave);

        mapperMock
            .Setup(mapper =>
                mapper.Map<DoctorLeaveDto>(
                    doctorLeave))
            .Returns(mappedLeave);

        var service = CreateService();

        // Act
        var result =
            await service.GetDoctorLeaveStatusAsync(
                DoctorId,
                selectedDate);

        // Assert
        Assert.True(result.IsOnLeave);
        Assert.NotNull(result.Leave);

        Assert.Equal(
            doctorLeave.DoctorLeaveId,
            result.Leave.DoctorLeaveId);

        Assert.Equal(
            "Doctor is on leave on the selected date.",
            result.Message);
    }

    [Fact]
    public async Task GetMyLeaveHistoryAsync_ReturnsDoctorLeaveHistory()
    {
        // Arrange
        var leaves = new List<DoctorLeave>
        {
            new()
            {
                DoctorLeaveId = 1,
                DoctorId = DoctorId,
                StartDate = DateTime.Today.AddDays(5),
                EndDate = DateTime.Today.AddDays(7),
                Reason = "Medical Conference",
                CreatedDate = DateTime.Now
            }
        };

        var mappedLeaves = new List<DoctorLeaveDto>
        {
            new()
            {
                DoctorLeaveId = 1,
                DoctorId = DoctorId,
                StartDate = leaves[0].StartDate,
                EndDate = leaves[0].EndDate,
                Reason = leaves[0].Reason,
                CreatedDate = leaves[0].CreatedDate
            }
        };

        doctorLeaveRepositoryMock
            .Setup(repository =>
                repository.GetByDoctorIdAsync(
                    DoctorId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(leaves);

        mapperMock
            .Setup(mapper =>
                mapper.Map<List<DoctorLeaveDto>>(
                    leaves))
            .Returns(mappedLeaves);

        var service = CreateService();

        // Act
        var result =
            await service.GetMyLeaveHistoryAsync();

        // Assert
        Assert.Single(result);

        Assert.Equal(
            "Medical Conference",
            result[0].Reason);
    }

    private DoctorLeaveService CreateService()
    {
        var httpContext =
            new DefaultHttpContext();

        httpContext.User =
            new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[]
                    {
                        new Claim(
                            ClaimTypes.NameIdentifier,
                            UserId)
                    },
                    "TestAuthentication"));

        var httpContextAccessor =
            new HttpContextAccessor
            {
                HttpContext = httpContext
            };

        return new DoctorLeaveService(
            doctorLeaveRepositoryMock.Object,
            doctorRepositoryMock.Object,
            appointmentRepositoryMock.Object,
            dbContext,
            httpContextAccessor,
            distributedCacheMock.Object,
            mapperMock.Object,
            NullLogger<DoctorLeaveService>.Instance);
    }

    private static CreateDoctorLeaveDto
        CreateValidRequest()
    {
        return new CreateDoctorLeaveDto
        {
            StartDate =
                DateTime.Today.AddDays(5),

            EndDate =
                DateTime.Today.AddDays(7),

            Reason =
                "Personal Leave",

            ConfirmAppointmentCancellation =
                false
        };
    }
}