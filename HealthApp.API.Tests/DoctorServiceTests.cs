using AutoMapper;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace HealthApp.API.Tests;

public sealed class DoctorServiceTests
{
    private const int DoctorId = 8;
    private const int OtherDoctorId = 9;
    private const string UserId = "doctor-user-8";

    private readonly Mock<IDoctorRepository> doctorRepositoryMock = new();
    private readonly Mock<IAppointmentRepository> appointmentRepositoryMock = new();
    private readonly Mock<IDoctorLeaveRepository> doctorLeaveRepositoryMock = new();
    private readonly Mock<IHttpContextAccessor> httpContextAccessorMock = new();
    private readonly Mock<IDistributedCache> distributedCacheMock = new();
    private readonly Mock<IMapper> mapperMock = new();

    private readonly DoctorService doctorService;

    public DoctorServiceTests()
    {
        SetCurrentUser(isDoctor: false);
        ConfigureCacheMiss();

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
    public async Task GetAllDoctorsAsync_WhenCallerIsNotDoctor_ReturnsMappedActiveDoctors()
    {
        var doctors = new List<Doctor> { ActiveDoctor(DoctorId) };
        var expected = new List<DoctorDto> { new() { DoctorId = DoctorId } };

        doctorRepositoryMock
            .Setup(repository => repository.GetAllActiveAsync())
            .ReturnsAsync(doctors);

        mapperMock
            .Setup(mapper => mapper.Map<List<DoctorDto>>(doctors))
            .Returns(expected);

        var result = await doctorService.GetAllDoctorsAsync();

        Assert.Same(expected, result);
        doctorRepositoryMock.Verify(repository => repository.GetAllActiveAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllDoctorsAsync_WhenCallerIsDoctor_ThrowsForbiddenAccessException()
    {
        SetCurrentUser(isDoctor: true);

        var exception = await Assert.ThrowsAsync<ForbiddenAccessException>(
            () => doctorService.GetAllDoctorsAsync());

        Assert.Equal("Doctors are not allowed to search all doctors.", exception.Message);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenIdIsInvalid_ThrowsBusinessRuleException()
    {
        var exception = await Assert.ThrowsAsync<BusinessRuleException>(
            () => doctorService.GetDoctorByIdAsync(0));

        Assert.Equal("Please provide a valid doctor reference.", exception.Message);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenActiveDoctorExists_ReturnsMappedDoctor()
    {
        var doctor = ActiveDoctor(DoctorId);
        var expected = new DoctorDto { DoctorId = DoctorId };

        doctorRepositoryMock.Setup(repository => repository.GetByIdAsync(DoctorId))
            .ReturnsAsync(doctor);
        mapperMock.Setup(mapper => mapper.Map<DoctorDto>(doctor)).Returns(expected);

        var result = await doctorService.GetDoctorByIdAsync(DoctorId);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenDoctorDoesNotExist_ThrowsEntityNotFoundException()
    {
        doctorRepositoryMock.Setup(repository => repository.GetByIdAsync(DoctorId))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => doctorService.GetDoctorByIdAsync(DoctorId));
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenDoctorIsInactive_ThrowsEntityNotFoundException()
    {
        doctorRepositoryMock.Setup(repository => repository.GetByIdAsync(DoctorId))
            .ReturnsAsync(new Doctor { DoctorId = DoctorId, IsActive = false });

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => doctorService.GetDoctorByIdAsync(DoctorId));
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenDoctorReadsOwnProfile_ReturnsDoctor()
    {
        SetCurrentUser(isDoctor: true);
        var doctor = ActiveDoctor(DoctorId);
        var expected = new DoctorDto { DoctorId = DoctorId };

        doctorRepositoryMock.Setup(repository => repository.GetByUserIdAsync(UserId))
            .ReturnsAsync(doctor);
        doctorRepositoryMock.Setup(repository => repository.GetByIdAsync(DoctorId))
            .ReturnsAsync(doctor);
        mapperMock.Setup(mapper => mapper.Map<DoctorDto>(doctor)).Returns(expected);

        var result = await doctorService.GetDoctorByIdAsync(DoctorId);

        Assert.Equal(DoctorId, result.DoctorId);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenDoctorReadsAnotherProfile_ThrowsForbiddenAccessException()
    {
        SetCurrentUser(isDoctor: true);
        doctorRepositoryMock.Setup(repository => repository.GetByUserIdAsync(UserId))
            .ReturnsAsync(ActiveDoctor(DoctorId));

        var exception = await Assert.ThrowsAsync<ForbiddenAccessException>(
            () => doctorService.GetDoctorByIdAsync(OtherDoctorId));

        Assert.Equal("You are not allowed to access another doctor's profile.", exception.Message);
    }

    [Fact]
    public async Task GetLoggedInDoctorProfileAsync_WhenDoctorIsActive_ReturnsMappedProfile()
    {
        SetCurrentUser(isDoctor: true);
        var doctor = ActiveDoctor(DoctorId);
        var expected = new DoctorDto { DoctorId = DoctorId };

        doctorRepositoryMock.Setup(repository => repository.GetByUserIdAsync(UserId))
            .ReturnsAsync(doctor);
        mapperMock.Setup(mapper => mapper.Map<DoctorDto>(doctor)).Returns(expected);

        var result = await doctorService.GetLoggedInDoctorProfileAsync();

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetLoggedInDoctorProfileAsync_WhenDoctorIsInactive_ThrowsForbiddenAccessException()
    {
        SetCurrentUser(isDoctor: true);
        doctorRepositoryMock.Setup(repository => repository.GetByUserIdAsync(UserId))
            .ReturnsAsync(new Doctor { DoctorId = DoctorId, IsActive = false });

        var exception = await Assert.ThrowsAsync<ForbiddenAccessException>(
            () => doctorService.GetLoggedInDoctorProfileAsync());

        Assert.Equal("Your doctor profile is inactive.", exception.Message);
    }

    [Fact]
    public async Task GetLoggedInDoctorProfileAsync_WhenNameIdentifierIsMissing_ThrowsForbiddenAccessException()
    {
        httpContextAccessorMock.SetupGet(accessor => accessor.HttpContext)
            .Returns(new DefaultHttpContext());

        var exception = await Assert.ThrowsAsync<ForbiddenAccessException>(
            () => doctorService.GetLoggedInDoctorProfileAsync());

        Assert.Equal("Unable to identify logged-in user.", exception.Message);
    }

    [Fact]
    public async Task GetLoggedInDoctorProfileAsync_WhenDoctorRecordIsMissing_ThrowsEntityNotFoundException()
    {
        SetCurrentUser(isDoctor: true);
        doctorRepositoryMock.Setup(repository => repository.GetByUserIdAsync(UserId))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => doctorService.GetLoggedInDoctorProfileAsync());
    }

    [Fact]
    public async Task GetDoctorsBySpecialisationAsync_WhenValid_ReturnsMappedDoctors()
    {
        var specialisation = SpecialisationType.Cardiologist;
        var doctors = new List<Doctor> { ActiveDoctor(DoctorId) };
        var expected = new List<DoctorDto> { new() { DoctorId = DoctorId } };

        doctorRepositoryMock
            .Setup(repository => repository.GetActiveBySpecialisationAsync(specialisation))
            .ReturnsAsync(doctors);
        mapperMock.Setup(mapper => mapper.Map<List<DoctorDto>>(doctors)).Returns(expected);

        var result = await doctorService.GetDoctorsBySpecialisationAsync(specialisation);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetDoctorsBySpecialisationAsync_WhenCallerIsDoctor_ThrowsForbiddenAccessException()
    {
        SetCurrentUser(isDoctor: true);

        var exception = await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            doctorService.GetDoctorsBySpecialisationAsync(SpecialisationType.Cardiologist));

        Assert.Equal("Doctors are not allowed to search doctors by specialisation.", exception.Message);
    }

    [Fact]
    public async Task GetDoctorsBySpecialisationAsync_WhenEnumIsInvalid_ThrowsBusinessRuleException()
    {
        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            doctorService.GetDoctorsBySpecialisationAsync((SpecialisationType)999));

        Assert.Equal("Invalid specialisation.", exception.Message);
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_WhenDoctorIdIsInvalid_ThrowsBusinessRuleException()
    {
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            doctorService.GetDoctorAvailabilityAsync(0, DateTime.Today));
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_WhenDateIsPast_ThrowsBusinessRuleException()
    {
        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            doctorService.GetDoctorAvailabilityAsync(DoctorId, DateTime.Today.AddDays(-1)));

        Assert.Equal("Cannot check availability for past dates.", exception.Message);
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_WhenDoctorReadsOwnAvailability_ReturnsAvailability()
    {
        SetCurrentUser(isDoctor: true);
        var date = DateTime.Today.AddDays(2);
        var doctor = ActiveDoctor(DoctorId);

        doctorRepositoryMock.Setup(repository => repository.GetByUserIdAsync(UserId))
            .ReturnsAsync(doctor);
        ConfigureAvailableDoctor(date, doctor, new List<Appointment>());

        var result = await doctorService.GetDoctorAvailabilityAsync(DoctorId, date);

        Assert.False(result.IsOnLeave);
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_WhenDoctorReadsAnotherAvailability_ThrowsForbiddenAccessException()
    {
        SetCurrentUser(isDoctor: true);
        doctorRepositoryMock.Setup(repository => repository.GetByUserIdAsync(UserId))
            .ReturnsAsync(ActiveDoctor(DoctorId));

        var exception = await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            doctorService.GetDoctorAvailabilityAsync(OtherDoctorId, DateTime.Today.AddDays(1)));

        Assert.Equal("You are not allowed to access another doctor's availability.", exception.Message);
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_WhenCacheContainsAvailability_ReturnsCachedValue()
    {
        var date = DateTime.Today.AddDays(2);
        var cached = new DoctorAvailabilityDto
        {
            DoctorId = DoctorId,
            Date = date,
            IsOnLeave = true,
            Message = "Doctor On Leave",
            AvailableSlots = new List<string>()
        };
        ConfigureCacheValue(JsonSerializer.Serialize(cached));

        var result = await doctorService.GetDoctorAvailabilityAsync(DoctorId, date);

        Assert.True(result.IsOnLeave);
        Assert.Equal("Doctor On Leave", result.Message);
        doctorRepositoryMock.Verify(repository => repository.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_WhenCachedJsonIsNull_ReturnsFallbackAvailability()
    {
        var date = DateTime.Today.AddDays(2);
        ConfigureCacheValue("null");

        var result = await doctorService.GetDoctorAvailabilityAsync(DoctorId, date);

        Assert.Equal(DoctorId, result.DoctorId);
        Assert.Equal(date.Date, result.Date);
        Assert.False(result.IsOnLeave);
        Assert.Empty(result.AvailableSlots);
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_WhenDoctorDoesNotExist_ThrowsEntityNotFoundException()
    {
        doctorRepositoryMock.Setup(repository => repository.GetByIdAsync(DoctorId))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            doctorService.GetDoctorAvailabilityAsync(DoctorId, DateTime.Today.AddDays(1)));
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_WhenDoctorIsInactive_ThrowsBusinessRuleException()
    {
        doctorRepositoryMock.Setup(repository => repository.GetByIdAsync(DoctorId))
            .ReturnsAsync(new Doctor { DoctorId = DoctorId, IsActive = false });

        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            doctorService.GetDoctorAvailabilityAsync(DoctorId, DateTime.Today.AddDays(1)));

        Assert.Equal("Doctor is inactive.", exception.Message);
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_WhenDoctorIsOnLeave_ReturnsLeaveAvailabilityAndCachesIt()
    {
        var date = DateTime.Today.AddDays(2);
        doctorRepositoryMock.Setup(repository => repository.GetByIdAsync(DoctorId))
            .ReturnsAsync(ActiveDoctor(DoctorId));
        doctorLeaveRepositoryMock
            .Setup(repository => repository.GetLeaveForDateAsync(DoctorId, date.Date))
            .ReturnsAsync(new DoctorLeave
            {
                DoctorLeaveId = 100,
                DoctorId = DoctorId,
                StartDate = date,
                EndDate = date,
                Reason = "Personal Leave"
            });

        var result = await doctorService.GetDoctorAvailabilityAsync(DoctorId, date);

        Assert.True(result.IsOnLeave);
        Assert.Equal("Doctor On Leave", result.Message);
        Assert.Empty(result.AvailableSlots);
        VerifyCacheSetOnce();
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_WhenSlotIsBooked_RemovesBookedSlot()
    {
        var date = DateTime.Today.AddDays(3);
        var bookedSlot = GetConfiguredTimeSlot(0);
        var secondSlot = GetConfiguredTimeSlot(1);
        var appointments = new List<Appointment>
        {
            new()
            {
                DoctorId = DoctorId,
                ScheduledDate = date,
                TimeSlots = bookedSlot,
                Status = AppointmentStatus.Confirmed.ToString()
            },
            new()
            {
                DoctorId = DoctorId,
                ScheduledDate = date,
                TimeSlots = secondSlot,
                Status = AppointmentStatus.Cancelled.ToString()
            },
            new()
            {
                DoctorId = DoctorId,
                ScheduledDate = date.AddDays(1),
                TimeSlots = secondSlot,
                Status = AppointmentStatus.Confirmed.ToString()
            }
        };
        ConfigureAvailableDoctor(date, ActiveDoctor(DoctorId), appointments);

        var result = await doctorService.GetDoctorAvailabilityAsync(DoctorId, date);

        Assert.DoesNotContain(bookedSlot, result.AvailableSlots);
        Assert.Contains(secondSlot, result.AvailableSlots);
        VerifyCacheSetOnce();
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_ForFutureDate_DoesNotRemoveSlotsAsPast()
    {
        var date = DateTime.Today.AddDays(5);
        ConfigureAvailableDoctor(date, ActiveDoctor(DoctorId), new List<Appointment>());

        var result = await doctorService.GetDoctorAvailabilityAsync(DoctorId, date);

        Assert.Equal(TimeSlots.Slots.Count, result.AvailableSlots.Count);
    }

    [Fact]
    public async Task GetDoctorAvailabilityAsync_ForToday_RemovesPastTimeSlots()
    {
        ConfigureAvailableDoctor(DateTime.Today, ActiveDoctor(DoctorId), new List<Appointment>());

        var result = await doctorService.GetDoctorAvailabilityAsync(DoctorId, DateTime.Today);

        Assert.All(result.AvailableSlots, slot =>
        {
            var startText = slot.Split('-')[0].Trim();
            var startTime = DateTime.Parse(startText).TimeOfDay;
            Assert.True(DateTime.Today.Add(startTime) > DateTime.Now);
        });
    }

    [Fact]
    public async Task InvalidateDoctorAvailabilityCacheAsync_WhenIdIsInvalid_ThrowsBusinessRuleException()
    {
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            doctorService.InvalidateDoctorAvailabilityCacheAsync(0, DateTime.Today));
    }

    [Fact]
    public async Task InvalidateDoctorAvailabilityCacheAsync_RemovesExpectedKey()
    {
        var date = DateTime.Today.AddDays(1);
        var key = $"doctors:{DoctorId}:availability:{date:yyyy-MM-dd}";

        await doctorService.InvalidateDoctorAvailabilityCacheAsync(DoctorId, date);

        distributedCacheMock.Verify(cache => cache.RemoveAsync(
            key,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void GetSlotStartTime_WhenFormatIsInvalid_ThrowsBusinessRuleException()
    {
        var method = GetPrivateStaticMethod("GetSlotStartTime");

        var wrapper = Assert.Throws<TargetInvocationException>(() =>
            method.Invoke(null, new object[] { "not-a-time-slot" }));

        var exception = Assert.IsType<BusinessRuleException>(wrapper.InnerException);
        Assert.Equal("Invalid time slot format.", exception.Message);
    }

    [Fact]
    public void GetSlotStartTime_WhenFormatIsValid_ReturnsStartTime()
    {
        var method = GetPrivateStaticMethod("GetSlotStartTime");

        var result = Assert.IsType<TimeSpan>(
            method.Invoke(null, new object[] { "09:00 AM - 09:30 AM" }));

        Assert.Equal(new TimeSpan(9, 0, 0), result);
    }

    [Fact]
    public void IsPastTimeSlot_WhenDateIsFuture_ReturnsFalse()
    {
        var method = GetPrivateStaticMethod("IsPastTimeSlot");

        var result = Assert.IsType<bool>(method.Invoke(
            null,
            new object[] { DateTime.Today.AddDays(1), "09:00 AM - 09:30 AM" }));

        Assert.False(result);
    }

    [Fact]
    public void IsPastTimeSlot_WhenTodaySlotIsPast_ReturnsTrue()
    {
        var method = GetPrivateStaticMethod("IsPastTimeSlot");

        var result = Assert.IsType<bool>(method.Invoke(
            null,
            new object[] { DateTime.Today, "12:00 AM - 12:30 AM" }));

        Assert.True(result);
    }

    private void ConfigureAvailableDoctor(
        DateTime date,
        Doctor doctor,
        List<Appointment> appointments)
    {
        doctorRepositoryMock.Setup(repository => repository.GetByIdAsync(DoctorId))
            .ReturnsAsync(doctor);
        doctorLeaveRepositoryMock
            .Setup(repository => repository.GetLeaveForDateAsync(DoctorId, date.Date))
            .ReturnsAsync((DoctorLeave?)null);
        appointmentRepositoryMock.Setup(repository => repository.GetByDoctorIdAsync(DoctorId))
            .ReturnsAsync(appointments);
    }

    private void SetCurrentUser(bool isDoctor)
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, UserId) };
        if (isDoctor)
        {
            claims.Add(new Claim(ClaimTypes.Role, Roles.Doctor));
        }

        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthentication"))
        };

        httpContextAccessorMock.SetupGet(accessor => accessor.HttpContext).Returns(context);
    }

    private void ConfigureCacheMiss()
    {
        distributedCacheMock
            .Setup(cache => cache.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);
        distributedCacheMock
            .Setup(cache => cache.SetAsync(
                It.IsAny<string>(),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        distributedCacheMock
            .Setup(cache => cache.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    private void ConfigureCacheValue(string value)
    {
        distributedCacheMock
            .Setup(cache => cache.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Encoding.UTF8.GetBytes(value));
    }

    private void VerifyCacheSetOnce()
    {
        distributedCacheMock.Verify(cache => cache.SetAsync(
            It.IsAny<string>(),
            It.IsAny<byte[]>(),
            It.Is<DistributedCacheEntryOptions>(options =>
                options.AbsoluteExpirationRelativeToNow == TimeSpan.FromMinutes(5)),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    private static Doctor ActiveDoctor(int doctorId) => new()
    {
        DoctorId = doctorId,
        IsActive = true
    };

    private static string GetConfiguredTimeSlot(int index)
    {
        Assert.True(TimeSlots.Slots.Count > index, "TimeSlots.Slots does not contain enough entries for this test.");
        return TimeSlots.Slots[index];
    }

    private static MethodInfo GetPrivateStaticMethod(string name)
    {
        return typeof(DoctorService).GetMethod(
                   name,
                   BindingFlags.NonPublic | BindingFlags.Static)
               ?? throw new InvalidOperationException($"Private method {name} was not found.");
    }
}
