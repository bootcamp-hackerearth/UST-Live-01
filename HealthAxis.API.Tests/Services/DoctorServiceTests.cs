using AutoMapper;
using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using Moq;
using Xunit;

namespace HealthAxis.API.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DoctorService _doctorService;

        public DoctorServiceTests()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();

            _doctorService = new DoctorService(
                _doctorRepositoryMock.Object,
                _appointmentRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorDoesNotExist_ReturnsNull()
        {
            // Arrange
            const int doctorId = 100;
            DateTime requestedDate = new DateTime(2026, 6, 20, 10, 30, 0);
            const string timeSlot = "10:00-10:30";

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            // Act
            DoctorAvailabilityDto? result =
                await _doctorService.GetAvailabilityAsync(
                    doctorId,
                    requestedDate,
                    timeSlot);

            // Assert
            Assert.Null(result);

            _doctorRepositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _appointmentRepositoryMock.Verify(
                repository => repository.IsSlotBookedAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenSlotIsBooked_ReturnsUnavailable()
        {
            // Arrange
            const int doctorId = 1;
            DateTime requestedDate = new DateTime(2026, 6, 20, 15, 45, 0);
            const string timeSlot = "10:00-10:30";

            Doctor doctor = CreateDoctor(doctorId);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    doctorId,
                    requestedDate,
                    timeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            DoctorAvailabilityDto? result =
                await _doctorService.GetAvailabilityAsync(
                    doctorId,
                    requestedDate,
                    timeSlot);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(doctorId, result.DoctorId);
            Assert.Equal(requestedDate.Date, result.Date);
            Assert.Equal(timeSlot, result.TimeSlot);
            Assert.False(result.IsAvailable);

            _doctorRepositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _appointmentRepositoryMock.Verify(
                repository => repository.IsSlotBookedAsync(
                    doctorId,
                    requestedDate,
                    timeSlot,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenSlotIsNotBooked_ReturnsAvailable()
        {
            // Arrange
            const int doctorId = 2;
            DateTime requestedDate = new DateTime(2026, 6, 21, 9, 15, 0);
            const string timeSlot = "11:00-11:30";

            Doctor doctor = CreateDoctor(doctorId);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    doctorId,
                    requestedDate,
                    timeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            DoctorAvailabilityDto? result =
                await _doctorService.GetAvailabilityAsync(
                    doctorId,
                    requestedDate,
                    timeSlot);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(doctorId, result.DoctorId);
            Assert.Equal(requestedDate.Date, result.Date);
            Assert.Equal(timeSlot, result.TimeSlot);
            Assert.True(result.IsAvailable);

            _doctorRepositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _appointmentRepositoryMock.Verify(
                repository => repository.IsSlotBookedAsync(
                    doctorId,
                    requestedDate,
                    timeSlot,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDateHasTimeComponent_ReturnsOnlyDate()
        {
            // Arrange
            const int doctorId = 3;
            DateTime requestedDateWithTime = new DateTime(2026, 7, 1, 18, 45, 30);
            DateTime expectedDateOnly = requestedDateWithTime.Date;
            const string timeSlot = "14:00-14:30";

            Doctor doctor = CreateDoctor(doctorId);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    doctorId,
                    requestedDateWithTime,
                    timeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            DoctorAvailabilityDto? result =
                await _doctorService.GetAvailabilityAsync(
                    doctorId,
                    requestedDateWithTime,
                    timeSlot);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDateOnly, result.Date);
            Assert.Equal(TimeSpan.Zero, result.Date.TimeOfDay);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenCancellationTokenProvided_PassesTokenToRepositories()
        {
            // Arrange
            const int doctorId = 4;
            DateTime requestedDate = new DateTime(2026, 7, 2);
            const string timeSlot = "15:00-15:30";

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Doctor doctor = CreateDoctor(doctorId);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    cancellationToken))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    doctorId,
                    requestedDate,
                    timeSlot,
                    cancellationToken))
                .ReturnsAsync(false);

            // Act
            DoctorAvailabilityDto? result =
                await _doctorService.GetAvailabilityAsync(
                    doctorId,
                    requestedDate,
                    timeSlot,
                    cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsAvailable);

            _doctorRepositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    doctorId,
                    cancellationToken),
                Times.Once);

            _appointmentRepositoryMock.Verify(
                repository => repository.IsSlotBookedAsync(
                    doctorId,
                    requestedDate,
                    timeSlot,
                    cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenSlotBookedRepositoryReturnsTrue_SetsIsAvailableFalse()
        {
            // Arrange
            const int doctorId = 5;
            DateTime date = new DateTime(2026, 8, 10);
            const string timeSlot = "16:00-16:30";

            Doctor doctor = CreateDoctor(doctorId);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    doctorId,
                    date,
                    timeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            DoctorAvailabilityDto? result =
                await _doctorService.GetAvailabilityAsync(
                    doctorId,
                    date,
                    timeSlot);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsAvailable);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenSlotBookedRepositoryReturnsFalse_SetsIsAvailableTrue()
        {
            // Arrange
            const int doctorId = 6;
            DateTime date = new DateTime(2026, 8, 11);
            const string timeSlot = "17:00-17:30";

            Doctor doctor = CreateDoctor(doctorId);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    doctorId,
                    date,
                    timeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            DoctorAvailabilityDto? result =
                await _doctorService.GetAvailabilityAsync(
                    doctorId,
                    date,
                    timeSlot);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsAvailable);
        }

        private static Doctor CreateDoctor(int doctorId)
        {
            return new Doctor
            {
                DoctorId = doctorId,
                FullName = "Dr. Test Doctor",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 700,
                IsActive = true
            };
        }
    }
}
