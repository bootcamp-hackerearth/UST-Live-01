using AutoMapper;
using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using Moq;

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
            _doctorRepositoryMock =
                new Mock<IDoctorRepository>();

            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _doctorService =
                new DoctorService(
                    _doctorRepositoryMock.Object,
                    _appointmentRepositoryMock.Object,
                    _mapperMock.Object);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorDoesNotExist_ReturnsNull()
        {
            const int doctorId = 404;
            DateTime date = DateTime.Today.AddDays(1);
            string timeSlot = "09:00-09:30";

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            DoctorAvailabilityDto? result =
                await _doctorService.GetAvailabilityAsync(
                    doctorId,
                    date,
                    timeSlot);

            Assert.Null(result);

            _appointmentRepositoryMock.Verify(
                repository => repository.IsSlotBookedAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorIsInactive_ReturnsUnavailable()
        {
            const int doctorId = 1;
            DateTime date = DateTime.Today.AddDays(1);
            string timeSlot = "09:00-09:30";

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Test",
                IsActive = false
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            DoctorAvailabilityDto? result =
                await _doctorService.GetAvailabilityAsync(
                    doctorId,
                    date,
                    timeSlot);

            Assert.NotNull(result);
            Assert.Equal(doctorId, result.DoctorId);
            Assert.Equal(date.Date, result.Date);
            Assert.Equal(timeSlot, result.TimeSlot);
            Assert.False(result.IsAvailable);

            _appointmentRepositoryMock.Verify(
                repository => repository.IsSlotBookedAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenActiveDoctorAndSlotIsBooked_ReturnsUnavailable()
        {
            const int doctorId = 1;
            DateTime date = DateTime.Today.AddDays(1);
            string timeSlot = "10:00-10:30";

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Active",
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    doctorId,
                    date.Date,
                    timeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            DoctorAvailabilityDto? result =
                await _doctorService.GetAvailabilityAsync(
                    doctorId,
                    date,
                    timeSlot);

            Assert.NotNull(result);
            Assert.False(result.IsAvailable);
            Assert.Equal(doctorId, result.DoctorId);
            Assert.Equal(date.Date, result.Date);
            Assert.Equal(timeSlot, result.TimeSlot);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenActiveDoctorAndSlotIsNotBooked_ReturnsAvailable()
        {
            const int doctorId = 1;
            DateTime date = DateTime.Today.AddDays(1).AddHours(5);
            string timeSlot = "11:00-11:30";

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Active",
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    doctorId,
                    date.Date,
                    timeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            DoctorAvailabilityDto? result =
                await _doctorService.GetAvailabilityAsync(
                    doctorId,
                    date,
                    timeSlot);

            Assert.NotNull(result);
            Assert.True(result.IsAvailable);
            Assert.Equal(date.Date, result.Date);

            _appointmentRepositoryMock.Verify(
                repository => repository.IsSlotBookedAsync(
                    doctorId,
                    date.Date,
                    timeSlot,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateActiveStatusAsync_WhenDoctorDoesNotExist_ReturnsNull()
        {
            const int doctorId = 404;

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            DoctorReadDto? result =
                await _doctorService.UpdateActiveStatusAsync(
                    doctorId,
                    true);

            Assert.Null(result);

            _doctorRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);

            _mapperMock.Verify(
                mapper => mapper.Map<DoctorReadDto>(
                    It.IsAny<Doctor>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateActiveStatusAsync_WhenDoctorExists_UpdatesStatusSavesAndReturnsMappedDoctor()
        {
            const int doctorId = 1;

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Test",
                IsActive = true
            };

            DoctorReadDto expectedDto = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Test",
                IsActive = false
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<DoctorReadDto>(doctor))
                .Returns(expectedDto);

            DoctorReadDto? result =
                await _doctorService.UpdateActiveStatusAsync(
                    doctorId,
                    false);

            Assert.NotNull(result);
            Assert.False(doctor.IsActive);
            Assert.Equal(expectedDto.DoctorId, result.DoctorId);
            Assert.Equal(expectedDto.FullName, result.FullName);
            Assert.False(result.IsActive);

            _doctorRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _mapperMock.Verify(
                mapper => mapper.Map<DoctorReadDto>(doctor),
                Times.Once);
        }

        [Fact]
        public async Task UpdateActiveStatusAsync_WhenUpdatingToActive_SetsDoctorActiveTrue()
        {
            const int doctorId = 1;

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Test",
                IsActive = false
            };

            DoctorReadDto expectedDto = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Test",
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<DoctorReadDto>(doctor))
                .Returns(expectedDto);

            DoctorReadDto? result =
                await _doctorService.UpdateActiveStatusAsync(
                    doctorId,
                    true);

            Assert.NotNull(result);
            Assert.True(doctor.IsActive);
            Assert.True(result.IsActive);
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_WhenDoctorDoesNotExist_ReturnsEmptyList()
        {
            const int doctorId = 404;
            DateTime date = DateTime.Today.AddDays(1);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            List<string> result =
                await _doctorService.GetAvailableSlotsAsync(
                    doctorId,
                    date);

            Assert.Empty(result);

            _appointmentRepositoryMock.Verify(
                repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_WhenDoctorIsInactive_ReturnsEmptyList()
        {
            const int doctorId = 1;
            DateTime date = DateTime.Today.AddDays(1);

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Inactive",
                IsActive = false
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            List<string> result =
                await _doctorService.GetAvailableSlotsAsync(
                    doctorId,
                    date);

            Assert.Empty(result);

            _appointmentRepositoryMock.Verify(
                repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_WhenDateIsInPast_ReturnsEmptyList()
        {
            const int doctorId = 1;
            DateTime date = DateTime.Today.AddDays(-1);

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Active",
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            List<string> result =
                await _doctorService.GetAvailableSlotsAsync(
                    doctorId,
                    date);

            Assert.Empty(result);

            _appointmentRepositoryMock.Verify(
                repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_WhenDateIsMoreThanSixMonthsAhead_ReturnsEmptyList()
        {
            const int doctorId = 1;
            DateTime date = DateTime.Today.AddMonths(6).AddDays(1);

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Active",
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            List<string> result =
                await _doctorService.GetAvailableSlotsAsync(
                    doctorId,
                    date);

            Assert.Empty(result);

            _appointmentRepositoryMock.Verify(
                repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_WhenNoAppointmentsExist_ReturnsAllSlots()
        {
            // Arrange
            const int doctorId = 1;
            DateTime date = DateTime.Today.AddDays(1);

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Active",
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Appointment>());

            // Act
            List<string> result =
                await _doctorService.GetAvailableSlotsAsync(
                    doctorId,
                    date);

            // Assert
            Assert.Equal(GetAllSlots().Count, result.Count);
            Assert.Equal(GetAllSlots(), result);

            _appointmentRepositoryMock.Verify(
                repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_WhenSlotsBookedForDoctor_ReturnsOnlyUnbookedSlots()
        {
            const int doctorId = 1;
            DateTime date = DateTime.Today.AddDays(1);

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Active",
                IsActive = true
            };

            List<Appointment> appointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    DoctorId = doctorId,
                    PatientId = 1,
                    ScheduledDate = date,
                    TimeSlot = "09:00-09:30",
                    Status = AppointmentStatus.Scheduled
                },
                new Appointment
                {
                    AppointmentId = 2,
                    DoctorId = doctorId,
                    PatientId = 2,
                    ScheduledDate = date,
                    TimeSlot = "10:00-10:30",
                    Status = AppointmentStatus.Confirmed
                }
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            List<string> result =
                await _doctorService.GetAvailableSlotsAsync(
                    doctorId,
                    date);

            Assert.DoesNotContain("09:00-09:30", result);
            Assert.DoesNotContain("10:00-10:30", result);
            Assert.Contains("09:30-10:00", result);
            Assert.Contains("16:30-17:00", result);
            Assert.Equal(GetAllSlots().Count - 2, result.Count);
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_WhenBookedAppointmentIsCancelled_DoesNotBlockSlot()
        {
            const int doctorId = 1;
            DateTime date = DateTime.Today.AddDays(1);

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Active",
                IsActive = true
            };

            List<Appointment> appointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    DoctorId = doctorId,
                    PatientId = 1,
                    ScheduledDate = date,
                    TimeSlot = "09:00-09:30",
                    Status = AppointmentStatus.Cancelled
                }
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            List<string> result =
                await _doctorService.GetAvailableSlotsAsync(
                    doctorId,
                    date);

            Assert.Contains("09:00-09:30", result);
            Assert.Equal(GetAllSlots().Count, result.Count);
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_WhenAppointmentsBelongToOtherDoctor_DoesNotBlockSlots()
        {
            const int doctorId = 1;
            const int otherDoctorId = 2;
            DateTime date = DateTime.Today.AddDays(1);

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Active",
                IsActive = true
            };

            List<Appointment> appointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    DoctorId = otherDoctorId,
                    PatientId = 1,
                    ScheduledDate = date,
                    TimeSlot = "09:00-09:30",
                    Status = AppointmentStatus.Scheduled
                }
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            List<string> result =
                await _doctorService.GetAvailableSlotsAsync(
                    doctorId,
                    date);

            Assert.Contains("09:00-09:30", result);
            Assert.Equal(GetAllSlots().Count, result.Count);
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_WhenAppointmentsAreOnDifferentDate_DoesNotBlockSlots()
        {
            const int doctorId = 1;
            DateTime selectedDate = DateTime.Today.AddDays(1);
            DateTime differentDate = selectedDate.AddDays(1);

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Active",
                IsActive = true
            };

            List<Appointment> appointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    DoctorId = doctorId,
                    PatientId = 1,
                    ScheduledDate = differentDate,
                    TimeSlot = "09:00-09:30",
                    Status = AppointmentStatus.Scheduled
                }
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            List<string> result =
                await _doctorService.GetAvailableSlotsAsync(
                    doctorId,
                    selectedDate);

            Assert.Contains("09:00-09:30", result);
            Assert.Equal(GetAllSlots().Count, result.Count);
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_WhenDateIsExactlySixMonthsAhead_ReturnsSlots()
        {
            const int doctorId = 1;
            DateTime date = DateTime.Today.AddMonths(6);

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Active",
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Appointment>());

            List<string> result =
                await _doctorService.GetAvailableSlotsAsync(
                    doctorId,
                    date);

            Assert.NotEmpty(result);
            Assert.Equal(GetAllSlots().Count, result.Count);
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_WhenDateIsToday_ReturnsOnlyFutureSlots()
        {
            const int doctorId = 1;
            DateTime date = DateTime.Today;

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Active",
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Appointment>());

            List<string> result =
                await _doctorService.GetAvailableSlotsAsync(
                    doctorId,
                    date);

            Assert.Equal(GetFutureSlotsForToday(), result);

            Assert.All(result, slot =>
            {
                string startTime =
                    slot.Split('-')[0];

                Assert.True(
                    TimeSpan.Parse(startTime) > DateTime.Now.TimeOfDay);
            });
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_WhenDateIsTodayAndFutureSlotIsBooked_ExcludesBookedFutureSlot()
        {
            const int doctorId = 1;
            DateTime date = DateTime.Today;

            string? futureSlot =
                GetFutureSlotsForToday()
                    .FirstOrDefault();

            if (futureSlot == null)
            {
                return;
            }

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Dr. Active",
                IsActive = true
            };

            List<Appointment> appointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    DoctorId = doctorId,
                    PatientId = 1,
                    ScheduledDate = date,
                    TimeSlot = futureSlot,
                    Status = AppointmentStatus.Scheduled
                }
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            List<string> result =
                await _doctorService.GetAvailableSlotsAsync(
                    doctorId,
                    date);

            Assert.DoesNotContain(futureSlot, result);
        }

        private static List<string> GetAllSlots()
        {
            return new List<string>
            {
                "09:00-09:30",
                "09:30-10:00",
                "10:00-10:30",
                "10:30-11:00",
                "11:00-11:30",
                "11:30-12:00",
                "14:00-14:30",
                "14:30-15:00",
                "15:00-15:30",
                "15:30-16:00",
                "16:00-16:30",
                "16:30-17:00"
            };
        }

        private static List<string> GetFutureSlotsForToday()
        {
            TimeSpan currentTime =
                DateTime.Now.TimeOfDay;

            return GetAllSlots()
                .Where(slot =>
                {
                    string startTime =
                        slot.Split('-')[0];

                    return TimeSpan.TryParse(
                               startTime,
                               out TimeSpan slotStartTime) &&
                           slotStartTime > currentTime;
                })
                .ToList();
        }
    }
}
