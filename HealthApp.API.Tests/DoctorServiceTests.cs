using AutoMapper;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace HealthApp.API.Tests;

public class DoctorServiceTests
{
    private readonly Mock<IDoctorRepository>
        doctorRepositoryMock = new();

    private readonly Mock<IAppointmentRepository>
        appointmentRepositoryMock = new();

    private readonly Mock<IDoctorLeaveRepository>
        doctorLeaveRepositoryMock = new();

    private readonly Mock<IHttpContextAccessor>
        httpContextAccessorMock = new();

    private readonly Mock<IDistributedCache>
        distributedCacheMock = new();

    private readonly Mock<IMapper>
        mapperMock = new();

    private readonly DoctorService doctorService;

    public DoctorServiceTests()
    {
        httpContextAccessorMock
            .SetupGet(accessor => accessor.HttpContext)
            .Returns(new DefaultHttpContext());

        doctorService = new DoctorService(
            doctorRepositoryMock.Object,
            appointmentRepositoryMock.Object,
            doctorLeaveRepositoryMock.Object,
            httpContextAccessorMock.Object,
            distributedCacheMock.Object,
            mapperMock.Object,
            NullLogger<DoctorService>.Instance);
    }

    [Fact]
    public async Task GetAllDoctorsAsync_ReturnsActiveDoctors()
    {
        // Arrange
        var doctors = new List<Doctor>
        {
            new()
            {
                DoctorId = 1,
                IsActive = true
            }
        };

        var doctorDtos = new List<DoctorDto>
        {
            new()
            {
                DoctorId = 1
            }
        };

        doctorRepositoryMock
            .Setup(repository =>
                repository.GetAllActiveAsync())
            .ReturnsAsync(doctors);

        mapperMock
            .Setup(mapper =>
                mapper.Map<List<DoctorDto>>(doctors))
            .Returns(doctorDtos);

        // Act
        var result =
            await doctorService.GetAllDoctorsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(1, result[0].DoctorId);

        doctorRepositoryMock.Verify(
            repository =>
                repository.GetAllActiveAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenDoctorExists_ReturnsDoctor()
    {
        // Arrange
        const int doctorId = 1;

        var doctor = new Doctor
        {
            DoctorId = doctorId,
            IsActive = true
        };

        var doctorDto = new DoctorDto
        {
            DoctorId = doctorId
        };

        doctorRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        mapperMock
            .Setup(mapper =>
                mapper.Map<DoctorDto>(doctor))
            .Returns(doctorDto);

        // Act
        var result =
            await doctorService.GetDoctorByIdAsync(
                doctorId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(doctorId, result.DoctorId);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenDoctorDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        const int doctorId = 999;

        doctorRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        // Act
        var action = async () =>
            await doctorService.GetDoctorByIdAsync(
                doctorId);

        // Assert
        await Assert.ThrowsAsync<
            EntityNotFoundException>(action);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenDoctorIsInactive_ThrowsEntityNotFoundException()
    {
        // Arrange
        const int doctorId = 1;

        var doctor = new Doctor
        {
            DoctorId = doctorId,
            IsActive = false
        };

        doctorRepositoryMock
            .Setup(repository =>
                repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        // Act
        var action = async () =>
            await doctorService.GetDoctorByIdAsync(
                doctorId);

        // Assert
        await Assert.ThrowsAsync<
            EntityNotFoundException>(action);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenDoctorIdIsInvalid_ThrowsBusinessRuleException()
    {
        // Act
        var action = async () =>
            await doctorService.GetDoctorByIdAsync(0);

        // Assert
        var exception =
            await Assert.ThrowsAsync<
                BusinessRuleException>(action);

        Assert.Equal(
            "Please provide a valid doctor reference.",
            exception.Message);
    }

    [Fact]
    public async Task GetDoctorsBySpecialisationAsync_WhenValid_ReturnsDoctors()
    {
        // Arrange
        var specialisation =
            SpecialisationType.Cardiologist;

        var doctors = new List<Doctor>
        {
            new()
            {
                DoctorId = 1,
                IsActive = true
            }
        };

        var doctorDtos = new List<DoctorDto>
        {
            new()
            {
                DoctorId = 1
            }
        };

        doctorRepositoryMock
            .Setup(repository =>
                repository
                    .GetActiveBySpecialisationAsync(
                        specialisation))
            .ReturnsAsync(doctors);

        mapperMock
            .Setup(mapper =>
                mapper.Map<List<DoctorDto>>(doctors))
            .Returns(doctorDtos);

        // Act
        var result =
            await doctorService
                .GetDoctorsBySpecialisationAsync(
                    specialisation);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(1, result[0].DoctorId);
    }

    [Fact]
    public async Task GetDoctorsBySpecialisationAsync_WhenInvalid_ThrowsBusinessRuleException()
    {
        // Arrange
        var invalidSpecialisation =
            (SpecialisationType)999;

        // Act
        var action = async () =>
            await doctorService
                .GetDoctorsBySpecialisationAsync(
                    invalidSpecialisation);

        // Assert
        var exception =
            await Assert.ThrowsAsync<
                BusinessRuleException>(action);

        Assert.Equal(
            "Invalid specialisation.",
            exception.Message);
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_WhenDateIsPast_ThrowsBusinessRuleException()
    {
        // Arrange
        const int doctorId = 1;

        var pastDate =
            DateTime.Today.AddDays(-1);

        // Act
        var action = async () =>
            await doctorService
                .GetDoctorAvailabilityAsync(
                    doctorId,
                    pastDate);

        // Assert
        var exception =
            await Assert.ThrowsAsync<
                BusinessRuleException>(action);

        Assert.Equal(
            "Cannot check availability for past dates.",
            exception.Message);
    }

    [Fact]
    public async Task InvalidateDoctorAvailabilityCacheAsync_RemovesExpectedCacheKey()
    {
        // Arrange
        const int doctorId = 1;

        var date =
            DateTime.Today.AddDays(1);

        var expectedCacheKey =
            $"doctors:{doctorId}:availability:{date:yyyy-MM-dd}";

        distributedCacheMock
            .Setup(cache =>
                cache.RemoveAsync(
                    expectedCacheKey,
                    It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await doctorService
            .InvalidateDoctorAvailabilityCacheAsync(
                doctorId,
                date);

        // Assert
        distributedCacheMock.Verify(
            cache =>
                cache.RemoveAsync(
                    expectedCacheKey,
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }
}