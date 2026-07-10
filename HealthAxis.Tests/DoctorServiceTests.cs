using AutoMapper;
using FluentAssertions;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementation;
using HealthAxis.Shared.DTO.DoctorDtos;
using HealthAxis.Shared.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using System.Text.Json;

namespace HealthAxis.API.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IDistributedCache> _distributedCacheMock;
        private readonly Mock<ILogger<DoctorService>> _loggerMock;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();
            _distributedCacheMock = new Mock<IDistributedCache>();
            _loggerMock = new Mock<ILogger<DoctorService>>();

            _mapperMock
                .Setup(mapper => mapper.Map<DoctorDto>(It.IsAny<Doctor>()))
                .Returns((Doctor doctor) => MapDoctorDto(doctor));

            _mapperMock
                .Setup(mapper => mapper.Map<List<DoctorDto>>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    var doctors = source as IEnumerable<Doctor>
                        ?? new List<Doctor>();

                    return doctors.Select(MapDoctorDto).ToList();
                });

            SetupCacheMiss();

            _service = new DoctorService(
                _doctorRepositoryMock.Object,
                _appointmentRepositoryMock.Object,
                _mapperMock.Object,
                _distributedCacheMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_WhenDoctorsExist_ReturnsDoctorDtos()
        {
            var doctors = new List<Doctor>
            {
                CreateDoctor(id: 1, fullName: "Dr John"),
                CreateDoctor(id: 2, fullName: "Dr Smith")
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result[0].DoctorId.Should().Be(1);
            result[0].FullName.Should().Be("Dr John");
            result[1].DoctorId.Should().Be(2);
            result[1].FullName.Should().Be("Dr Smith");

            _doctorRepositoryMock.Verify(
                repository => repository.GetAllAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoDoctors_ReturnsEmptyList()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _doctorRepositoryMock.Verify(
                repository => repository.GetAllAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorExists_ReturnsDoctorDto()
        {
            var doctor = CreateDoctor(id: 1, fullName: "Dr John");

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(1);
            result.FullName.Should().Be("Dr John");

            _doctorRepositoryMock.Verify(
                repository => repository.GetByIdAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            Func<Task> act = async () => await _service.GetByIdAsync(99);

            await act.Should().ThrowAsync<NotFoundException>();

            _doctorRepositoryMock.Verify(
                repository => repository.GetByIdAsync(99),
                Times.Once);
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenDoctorProfileExists_ReturnsDoctorDto()
        {
            var doctors = new List<Doctor>
            {
                CreateDoctor(id: 1, fullName: "Dr John", userId: "doctor-user-1"),
                CreateDoctor(id: 2, fullName: "Dr Smith", userId: "doctor-user-2")
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var result = await _service.GetByUserIdAsync("doctor-user-2");

            result.Should().NotBeNull();
            result!.DoctorId.Should().Be(2);
            result.FullName.Should().Be("Dr Smith");

            _doctorRepositoryMock.Verify(
                repository => repository.GetAllAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenDoctorsListIsEmpty_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            Func<Task> act = async () =>
                await _service.GetByUserIdAsync("missing-user");

            await act.Should().ThrowAsync<NotFoundException>();

            _doctorRepositoryMock.Verify(
                repository => repository.GetAllAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenMatchingDoctorDoesNotExist_ThrowsNotFoundException()
        {
            var doctors = new List<Doctor>
            {
                CreateDoctor(id: 1, fullName: "Dr John", userId: "doctor-user-1")
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            Func<Task> act = async () =>
                await _service.GetByUserIdAsync("wrong-user");

            await act.Should().ThrowAsync<NotFoundException>();

            _doctorRepositoryMock.Verify(
                repository => repository.GetAllAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorIsActive_ReturnsDoctorDetailsWithAvailableSlots()
        {
            var doctor = CreateDoctor(
                id: 1,
                fullName: "Dr John",
                isActive: true);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            var result = await _service.GetAvailabilityAsync(
                1,
                DateTime.Today.AddDays(1));

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(1);
            result.FullName.Should().Be("Dr John");
            result.IsActive.Should().BeTrue();
            result.Message.Should().Be("Doctor is available");
            result.AvailableSlots.Should().NotBeEmpty();

            _doctorRepositoryMock.Verify(
                repository => repository.GetByIdAsync(1),
                Times.Once);

            _appointmentRepositoryMock.Verify(
                repository => repository.GetAllAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorIsInactive_ReturnsEmptyAvailableSlots()
        {
            var doctor = CreateDoctor(
                id: 1,
                fullName: "Dr John",
                isActive: false);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            var result = await _service.GetAvailabilityAsync(
                1,
                DateTime.Today.AddDays(1));

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(1);
            result.IsActive.Should().BeFalse();
            result.Message.Should().Be("Doctor is not available");
            result.AvailableSlots.Should().BeEmpty();

            _doctorRepositoryMock.Verify(
                repository => repository.GetByIdAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorHasBookedSlot_RemovesBookedSlotFromAvailableSlots()
        {
            var doctor = CreateDoctor(
                id: 1,
                fullName: "Dr John",
                isActive: true);

            var appointmentDate = DateTime.Today.AddDays(1);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>
                {
                    CreateAppointment(
                        doctorId: 1,
                        scheduledDate: appointmentDate,
                        timeSlot: "10:00 AM - 11:00 AM",
                        status: AppointmentStatus.Pending)
                });

            var result = await _service.GetAvailabilityAsync(
                1,
                appointmentDate);

            result.AvailableSlots.Should().NotContain("10:00 AM - 11:00 AM");
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenCacheHasValue_ReturnsCachedAvailability()
        {
            var cachedAvailability = new DoctorAvailabilityDto
            {
                DoctorId = 1,
                FullName = "Dr Cached",
                IsActive = true,
                Date = DateTime.Today.AddDays(1),
                Message = "Doctor is available",
                AvailableSlots = new List<string>
                {
                    "09:00 AM - 10:00 AM"
                }
            };

            SetupCacheHit(cachedAvailability);

            var result = await _service.GetAvailabilityAsync(
                1,
                DateTime.Today.AddDays(1));

            result.DoctorId.Should().Be(1);
            result.FullName.Should().Be("Dr Cached");
            result.AvailableSlots.Should().ContainSingle();

            _doctorRepositoryMock.Verify(
                repository => repository.GetByIdAsync(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDateIsPast_ThrowsValidationException()
        {
            Func<Task> act = async () =>
                await _service.GetAvailabilityAsync(
                    1,
                    DateTime.Today.AddDays(-1));

            await act.Should().ThrowAsync<ValidationExceptions>();
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(100))
                .ReturnsAsync((Doctor?)null);

            Func<Task> act = async () =>
                await _service.GetAvailabilityAsync(
                    100,
                    DateTime.Today.AddDays(1));

            await act.Should().ThrowAsync<NotFoundException>();

            _doctorRepositoryMock.Verify(
                repository => repository.GetByIdAsync(100),
                Times.Once);
        }

        private void SetupCacheMiss()
        {
            _distributedCacheMock
                .Setup(cache => cache.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[]?)null);

            _distributedCacheMock
                .Setup(cache => cache.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<byte[]>(),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupCacheHit(
            DoctorAvailabilityDto availability)
        {
            var serializedAvailability = JsonSerializer.Serialize(availability);
            var bytes = Encoding.UTF8.GetBytes(serializedAvailability);

            _distributedCacheMock
                .Setup(cache => cache.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(bytes);
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
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = isActive
            };
        }

        private static Appointment CreateAppointment(
            int doctorId = 1,
            DateTime? scheduledDate = null,
            string timeSlot = "10:00 AM - 11:00 AM",
            AppointmentStatus status = AppointmentStatus.Pending)
        {
            return new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = doctorId,
                ScheduledDate = scheduledDate ?? DateTime.Today.AddDays(1),
                TimeSlot = timeSlot,
                Status = status
            };
        }

        private static DoctorDto MapDoctorDto(Doctor doctor)
        {
            return new DoctorDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                Specialisation = doctor.Specialisation,
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive
            };
        }
    }
}