using AutoMapper;
using FluentAssertions;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementation;
using HealthAxis.Shared.DTO.DoctorDtos;
using HealthAxis.Shared.Enums;
using Microsoft.Extensions.Logging;
using Moq;

using ApiValidationException =
    HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository>
            _doctorRepositoryMock;

        private readonly Mock<IAppointmentRepository>
            _appointmentRepositoryMock;

        private readonly Mock<IMapper>
            _mapperMock;

        private readonly Mock<ILogger<DoctorService>>
            _loggerMock;

        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _doctorRepositoryMock =
                new Mock<IDoctorRepository>();

            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _loggerMock =
                new Mock<ILogger<DoctorService>>();

            _mapperMock
                .Setup(mapper =>
                    mapper.Map<DoctorDto>(
                        It.IsAny<Doctor>()))
                .Returns((Doctor doctor) =>
                    MapDoctorDto(doctor));

            _mapperMock
                .Setup(mapper =>
                    mapper.Map<List<DoctorDto>>(
                        It.IsAny<object>()))
                .Returns((object source) =>
                {
                    var doctors =
                        source as IEnumerable<Doctor> ??
                        [];

                    return doctors
                        .Select(MapDoctorDto)
                        .ToList();
                });

            _service = CreateService();
        }

        [Fact]
        public async Task
            GetAllAsync_WhenDoctorsExist_ReturnsMappedDoctors()
        {
            var doctors = new List<Doctor>
            {
                CreateDoctor(
                    id: 1,
                    fullName: "Dr John"),

                CreateDoctor(
                    id: 2,
                    fullName: "Dr Smith")
            };

            _doctorRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var result =
                await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result[0].DoctorId.Should().Be(1);
            result[0].FullName.Should().Be("Dr John");
            result[1].DoctorId.Should().Be(2);
            result[1].FullName.Should().Be("Dr Smith");
        }

        [Fact]
        public async Task
            GetAllAsync_WhenNoDoctors_ReturnsEmptyList()
        {
            _doctorRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync([]);

            var result =
                await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task
            GetByIdAsync_WhenDoctorExists_ReturnsMappedDoctor()
        {
            _doctorRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(1))
                .ReturnsAsync(
                    CreateDoctor(
                        id: 1,
                        fullName: "Dr John"));

            var result =
                await _service.GetByIdAsync(1);

            result.DoctorId.Should().Be(1);
            result.FullName.Should().Be("Dr John");
        }

        [Fact]
        public async Task
            GetByIdAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            Func<Task> act = async () =>
                await _service.GetByIdAsync(99);

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Doctor not found");
        }

        [Fact]
        public async Task
            GetByUserIdAsync_WhenProfileExists_ReturnsMappedDoctor()
        {
            _doctorRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync(
                [
                    CreateDoctor(
                        id: 1,
                        userId: "doctor-user-1"),

                    CreateDoctor(
                        id: 2,
                        fullName: "Dr Smith",
                        userId: "doctor-user-2")
                ]);

            var result =
                await _service.GetByUserIdAsync(
                    "doctor-user-2");

            result.Should().NotBeNull();
            result!.DoctorId.Should().Be(2);
            result.FullName.Should().Be("Dr Smith");
        }

        [Fact]
        public async Task
            GetByUserIdAsync_WhenProfileDoesNotExist_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync(
                [
                    CreateDoctor(
                        userId: "doctor-user-1")
                ]);

            Func<Task> act = async () =>
                await _service.GetByUserIdAsync(
                    "missing-user");

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(
                    "Doctor profile not found");
        }

        [Fact]
        public async Task
            GetAvailabilityAsync_WhenIdIsZero_ThrowsValidationException()
        {
            Func<Task> act = async () =>
                await _service.GetAvailabilityAsync(
                    0,
                    DateTime.Today);

            await act.Should()
                .ThrowAsync<ApiValidationException>()
                .WithMessage(
                    "Doctor id must be greater than zero.");
        }

        [Fact]
        public async Task
            GetAvailabilityAsync_WhenIdIsNegative_ThrowsValidationException()
        {
            Func<Task> act = async () =>
                await _service.GetAvailabilityAsync(
                    -1,
                    DateTime.Today);

            await act.Should()
                .ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task
            GetAvailabilityAsync_WhenDateIsPast_ThrowsValidationException()
        {
            Func<Task> act = async () =>
                await _service.GetAvailabilityAsync(
                    1,
                    DateTime.Today.AddDays(-1));

            await act.Should()
                .ThrowAsync<ApiValidationException>()
                .WithMessage(
                    "Cannot check doctor availability for a past date.");
        }

        [Fact]
        public async Task
            GetAvailabilityAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            Func<Task> act = async () =>
                await _service.GetAvailabilityAsync(
                    99,
                    DateTime.Today.AddDays(1));

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Doctor not found");
        }

        [Fact]
        public async Task
            GetAvailabilityAsync_WhenDateIsNull_UsesToday()
        {
            SetupActiveDoctorWithAppointments([]);

            var result =
                await _service.GetAvailabilityAsync(
                    1,
                    null);

            result.Date.Should().Be(
                DateTime.Today);

            result.AvailableSlots.Should()
                .HaveCount(12);
        }

        [Fact]
        public async Task
            GetAvailabilityAsync_WhenDoctorIsInactive_ReturnsNoSlots()
        {
            _doctorRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(1))
                .ReturnsAsync(
                    CreateDoctor(
                        isActive: false));

            var result =
                await _service.GetAvailabilityAsync(
                    1,
                    DateTime.Today.AddDays(1));

            result.IsActive.Should().BeFalse();

            result.Message.Should().Be(
                "Doctor is not available");

            result.AvailableSlots.Should()
                .BeEmpty();

            _appointmentRepositoryMock.Verify(
                repository =>
                    repository.GetAllAsync(),
                Times.Never);
        }

        [Fact]
        public async Task
            GetAvailabilityAsync_WhenDoctorIsActive_ReturnsAllUnbookedSlots()
        {
            SetupActiveDoctorWithAppointments([]);

            var result =
                await _service.GetAvailabilityAsync(
                    1,
                    DateTime.Today.AddDays(1));

            result.DoctorId.Should().Be(1);
            result.FullName.Should().Be("Dr John");
            result.IsActive.Should().BeTrue();

            result.Message.Should().Be(
                "Doctor is available");

            result.AvailableSlots.Should()
                .HaveCount(12);
        }

        [Fact]
        public async Task
            GetAvailabilityAsync_WhenPendingSlotExists_RemovesSlot()
        {
            var date =
                DateTime.Today.AddDays(1);

            SetupActiveDoctorWithAppointments(
            [
                CreateAppointment(
                    date: date,
                    status:
                        AppointmentStatus.Pending,
                    timeSlot:
                        "10:00 AM - 11:00 AM")
            ]);

            var result =
                await _service.GetAvailabilityAsync(
                    1,
                    date);

            result.AvailableSlots.Should()
                .NotContain(
                    "10:00 AM - 11:00 AM");

            result.AvailableSlots.Should()
                .HaveCount(11);
        }

        [Fact]
        public async Task
            GetAvailabilityAsync_WhenConfirmedSlotExists_RemovesSlot()
        {
            var date =
                DateTime.Today.AddDays(1);

            SetupActiveDoctorWithAppointments(
            [
                CreateAppointment(
                    date: date,
                    status:
                        AppointmentStatus.Confirmed,
                    timeSlot:
                        "11:00 AM - 12:00 PM")
            ]);

            var result =
                await _service.GetAvailabilityAsync(
                    1,
                    date);

            result.AvailableSlots.Should()
                .NotContain(
                    "11:00 AM - 12:00 PM");
        }

        [Fact]
        public async Task
            GetAvailabilityAsync_WhenCancelledAndCompletedSlotsExist_KeepsSlotsAvailable()
        {
            var date =
                DateTime.Today.AddDays(1);

            SetupActiveDoctorWithAppointments(
            [
                CreateAppointment(
                    id: 1,
                    date: date,
                    status:
                        AppointmentStatus.Cancelled,
                    timeSlot:
                        "10:00 AM - 11:00 AM"),

                CreateAppointment(
                    id: 2,
                    date: date,
                    status:
                        AppointmentStatus.Completed,
                    timeSlot:
                        "11:00 AM - 12:00 PM")
            ]);

            var result =
                await _service.GetAvailabilityAsync(
                    1,
                    date);

            result.AvailableSlots.Should()
                .Contain(
                    "10:00 AM - 11:00 AM");

            result.AvailableSlots.Should()
                .Contain(
                    "11:00 AM - 12:00 PM");

            result.AvailableSlots.Should()
                .HaveCount(12);
        }

        [Fact]
        public async Task
            GetAvailabilityAsync_WhenAppointmentsBelongToOtherDoctorOrDate_IgnoresThem()
        {
            var requestedDate =
                DateTime.Today.AddDays(1);

            SetupActiveDoctorWithAppointments(
            [
                CreateAppointment(
                    id: 1,
                    doctorId: 2,
                    date: requestedDate,
                    timeSlot:
                        "09:00 AM - 10:00 AM"),

                CreateAppointment(
                    id: 2,
                    doctorId: 1,
                    date:
                        requestedDate.AddDays(1),
                    timeSlot:
                        "10:00 AM - 11:00 AM")
            ]);

            var result =
                await _service.GetAvailabilityAsync(
                    1,
                    requestedDate);

            result.AvailableSlots.Should()
                .HaveCount(12);
        }

        [Fact]
        public async Task
            GetAvailabilityAsync_WhenBookedSlotContainsOuterSpaces_NormalizesSlot()
        {
            var date =
                DateTime.Today.AddDays(1);

            SetupActiveDoctorWithAppointments(
            [
                CreateAppointment(
                    date: date,
                    status:
                        AppointmentStatus.Pending,
                    timeSlot:
                        " 10:00 AM - 11:00 AM ")
            ]);

            var result =
                await _service.GetAvailabilityAsync(
                    1,
                    date);

            result.AvailableSlots.Should()
                .NotContain(
                    "10:00 AM - 11:00 AM");
        }

        [Fact]
        public async Task
            GetAvailabilityAsync_WhenInformationLoggingIsEnabled_LogsAndReturnsResult()
        {
            SetupActiveDoctorWithAppointments([]);

            var testLogger =
                new TestLogger<DoctorService>(
                    LogLevel.Information);

            var service = new DoctorService(
                _doctorRepositoryMock.Object,
                _appointmentRepositoryMock.Object,
                _mapperMock.Object,
                testLogger);

            var result =
                await service.GetAvailabilityAsync(
                    1,
                    DateTime.Today.AddDays(1));

            result.AvailableSlots.Should()
                .HaveCount(12);

            testLogger.Messages.Should()
                .ContainSingle(message =>
                    message.Contains(
                        "Doctor availability loaded from database",
                        StringComparison.Ordinal));
        }

        private DoctorService CreateService()
        {
            return new DoctorService(
                _doctorRepositoryMock.Object,
                _appointmentRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        private void SetupActiveDoctorWithAppointments(
            List<Appointment> appointments)
        {
            _doctorRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(1))
                .ReturnsAsync(
                    CreateDoctor(
                        isActive: true));

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync(appointments);
        }

        private static Doctor CreateDoctor(
            int id = 1,
            string fullName = "Dr John",
            string userId = "doctor-user-1",
            bool isActive = true)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = fullName,
                UserId = userId,
                Specialisation =
                    Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = isActive
            };
        }

        private static Appointment
            CreateAppointment(
                int id = 1,
                int doctorId = 1,
                DateTime? date = null,
                string timeSlot =
                    "10:00 AM - 11:00 AM",
                AppointmentStatus status =
                    AppointmentStatus.Pending)
        {
            return new Appointment
            {
                AppointmentId = id,
                PatientId = 1,
                DoctorId = doctorId,
                ScheduledDate =
                    date ??
                    DateTime.Today.AddDays(1),
                TimeSlot = timeSlot,
                Status = status
            };
        }

        private static DoctorDto MapDoctorDto(
            Doctor doctor)
        {
            return new DoctorDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                Specialisation =
                    doctor.Specialisation,
                YearsOfExperience =
                    doctor.YearsOfExperience,
                ConsultationFee =
                    doctor.ConsultationFee,
                IsActive = doctor.IsActive
            };
        }

        private sealed class TestLogger<T>(
            LogLevel enabledLevel) : ILogger<T>
        {
            public List<string> Messages { get; } = [];

            public IDisposable? BeginScope<TState>(
                TState state)
                where TState : notnull
            {
                return null;
            }

            public bool IsEnabled(
                LogLevel logLevel)
            {
                return logLevel == enabledLevel;
            }

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception? exception,
                Func<TState, Exception?, string>
                    formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                Messages.Add(
                    formatter(
                        state,
                        exception));
            }
        }
    }
}