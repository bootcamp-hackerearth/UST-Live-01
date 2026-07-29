using FluentAssertions;
using HealthAxis.API.Data;
using HealthAxis.API.Events;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Services.Implementation;
using HealthAxis.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

using ApiValidationException =
    HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Tests.Services
{
    public class NotificationServiceTests :
        IDisposable
    {
        private readonly ApplicationDbContext
            _context;

        private readonly Mock<
            ILogger<NotificationService>>
            _loggerMock;

        private readonly NotificationService
            _service;

        public NotificationServiceTests()
        {
            var options =
                new DbContextOptionsBuilder<
                    ApplicationDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString())
                .Options;

            _context =
                new ApplicationDbContext(
                    options);

            _loggerMock =
                new Mock<
                    ILogger<NotificationService>>();

            _loggerMock
                .Setup(logger =>
                    logger.IsEnabled(
                        LogLevel.Information))
                .Returns(true);

            _service =
                new NotificationService(
                    _context,
                    _loggerMock.Object);
        }

        [Fact]
        public async Task
            CreateAppointmentBookedNotificationAsync_WhenEventIsNull_ThrowsArgumentNullException()
        {
            Func<Task> act = async () =>
                await _service
                    .CreateAppointmentBookedNotificationAsync(
                        null!);

            await act.Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task
            CreateAppointmentBookedNotificationAsync_WhenNew_CreatesDoctorNotification()
        {
            var bookedEvent =
                CreateBookedEvent();

            var result =
                await _service
                    .CreateAppointmentBookedNotificationAsync(
                        bookedEvent);

            result.Should().BeTrue();

            var notification =
                await _context.Notifications
                    .SingleAsync();

            notification.AppointmentId
                .Should().Be(1);

            notification.DoctorId
                .Should().Be(1);

            notification.PatientId
                .Should().BeNull();

            notification.Title
                .Should().Be(
                    "New Appointment Booked");

            notification.NotificationType
                .Should().Be(
                    "AppointmentBooked");

            notification.Message
                .Should().Contain("Mona");

            notification.IsRead
                .Should().BeFalse();
        }

        [Fact]
        public async Task
            CreateAppointmentBookedNotificationAsync_WhenDuplicateExists_ReturnsFalse()
        {
            _context.Notifications.Add(
                CreateNotification(
                    notificationType:
                        "AppointmentBooked",
                    doctorId: 1,
                    patientId: null));

            await _context.SaveChangesAsync();

            var result =
                await _service
                    .CreateAppointmentBookedNotificationAsync(
                        CreateBookedEvent());

            result.Should().BeFalse();

            (await _context.Notifications
                .CountAsync())
                .Should().Be(1);
        }

        [Fact]
        public async Task
            CreateAppointmentStatusNotificationsAsync_WhenAppointmentIsNull_ThrowsArgumentNullException()
        {
            Func<Task> act = async () =>
                await _service
                    .CreateAppointmentStatusNotificationsAsync(
                        null!);

            await act.Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task
            CreateAppointmentStatusNotificationsAsync_WhenStatusIsPending_ReturnsZero()
        {
            var appointment =
                CreateAppointment(
                    status:
                        AppointmentStatus.Pending);

            var result =
                await _service
                    .CreateAppointmentStatusNotificationsAsync(
                        appointment);

            result.Should().Be(0);
        }

        [Fact]
        public async Task
            CreateAppointmentStatusNotificationsAsync_WhenAppointmentDoesNotExist_ReturnsZero()
        {
            var result =
                await _service
                    .CreateAppointmentStatusNotificationsAsync(
                        CreateAppointment(
                            id: 99,
                            status:
                                AppointmentStatus.Confirmed));

            result.Should().Be(0);
        }

        [Fact]
        public async Task
            CreateAppointmentStatusNotificationsAsync_WhenConfirmed_CreatesPatientNotification()
        {
            await SeedAppointmentAsync(
                AppointmentStatus.Confirmed);

            var result =
                await _service
                    .CreateAppointmentStatusNotificationsAsync(
                        CreateAppointment(
                            status:
                                AppointmentStatus.Confirmed));

            result.Should().Be(1);

            var notification =
                await _context.Notifications
                    .SingleAsync();

            notification.PatientId
                .Should().Be(1);

            notification.DoctorId
                .Should().BeNull();

            notification.NotificationType
                .Should().Be(
                    "AppointmentConfirmed");

            notification.Message
                .Should().Contain("confirmed");
        }

        [Fact]
        public async Task
            CreateAppointmentStatusNotificationsAsync_WhenCompleted_CreatesPatientNotification()
        {
            await SeedAppointmentAsync(
                AppointmentStatus.Completed);

            var result =
                await _service
                    .CreateAppointmentStatusNotificationsAsync(
                        CreateAppointment(
                            status:
                                AppointmentStatus.Completed));

            result.Should().Be(1);

            var notification =
                await _context.Notifications
                    .SingleAsync();

            notification.NotificationType
                .Should().Be(
                    "AppointmentCompleted");

            notification.Message
                .Should().Contain("completed");
        }

        [Fact]
        public async Task
            CreateAppointmentStatusNotificationsAsync_WhenCancelledByPatient_CreatesDoctorNotification()
        {
            await SeedAppointmentAsync(
                AppointmentStatus.Cancelled,
                "Cancelled by patient. Reason: Busy");

            var result =
                await _service
                    .CreateAppointmentStatusNotificationsAsync(
                        CreateAppointment(
                            status:
                                AppointmentStatus.Cancelled));

            result.Should().Be(1);

            var notification =
                await _context.Notifications
                    .SingleAsync();

            notification.DoctorId
                .Should().Be(1);

            notification.PatientId
                .Should().BeNull();

            notification.Message
                .Should().Contain(
                    "cancelled by the patient");

            notification.Message
                .Should().Contain("Busy");
        }

        [Fact]
        public async Task
            CreateAppointmentStatusNotificationsAsync_WhenCancelledByDoctor_CreatesPatientNotification()
        {
            await SeedAppointmentAsync(
                AppointmentStatus.Cancelled,
                "Cancelled by doctor. Reason: Emergency");

            var result =
                await _service
                    .CreateAppointmentStatusNotificationsAsync(
                        CreateAppointment(
                            status:
                                AppointmentStatus.Cancelled));

            result.Should().Be(1);

            var notification =
                await _context.Notifications
                    .SingleAsync();

            notification.PatientId
                .Should().Be(1);

            notification.DoctorId
                .Should().BeNull();

            notification.Message
                .Should().Contain(
                    "cancelled by the doctor");

            notification.Message
                .Should().Contain("Emergency");
        }

        [Fact]
        public async Task
            CreateAppointmentStatusNotificationsAsync_WhenCancelledByAdmin_CreatesPatientAndDoctorNotifications()
        {
            await SeedAppointmentAsync(
                AppointmentStatus.Cancelled,
                "Cancelled by admin. Reason: Hospital closed");

            var result =
                await _service
                    .CreateAppointmentStatusNotificationsAsync(
                        CreateAppointment(
                            status:
                                AppointmentStatus.Cancelled));

            result.Should().Be(2);

            var notifications =
                await _context.Notifications
                    .ToListAsync();

            notifications.Should()
                .ContainSingle(item =>
                    item.PatientId == 1);

            notifications.Should()
                .ContainSingle(item =>
                    item.DoctorId == 1);

            notifications.Should()
                .OnlyContain(item =>
                    item.Message.Contains(
                        "Hospital closed",
                        StringComparison.Ordinal));
        }

        [Fact]
        public async Task
            CreateAppointmentStatusNotificationsAsync_WhenCancellationSourceIsUnknown_NotifiesBothUsers()
        {
            await SeedAppointmentAsync(
                AppointmentStatus.Cancelled,
                "System cancellation");

            var result =
                await _service
                    .CreateAppointmentStatusNotificationsAsync(
                        CreateAppointment(
                            status:
                                AppointmentStatus.Cancelled));

            result.Should().Be(2);

            var notifications =
                await _context.Notifications
                    .ToListAsync();

            notifications.Should()
                .OnlyContain(item =>
                    item.Message.Contains(
                        "the hospital",
                        StringComparison.Ordinal));

            notifications.Should()
                .OnlyContain(item =>
                    item.Message.Contains(
                        "No additional reason was provided.",
                        StringComparison.Ordinal));
        }

        [Fact]
        public async Task
            CreateAppointmentStatusNotificationsAsync_WhenStatusNotificationExists_ReturnsZero()
        {
            await SeedAppointmentAsync(
                AppointmentStatus.Confirmed);

            _context.Notifications.Add(
                CreateNotification(
                    notificationType:
                        "AppointmentConfirmed",
                    patientId: 1,
                    doctorId: null));

            await _context.SaveChangesAsync();

            var result =
                await _service
                    .CreateAppointmentStatusNotificationsAsync(
                        CreateAppointment(
                            status:
                                AppointmentStatus.Confirmed));

            result.Should().Be(0);

            (await _context.Notifications
                .CountAsync())
                .Should().Be(1);
        }

        [Fact]
        public async Task
            CreateAppointmentStatusNotificationsAsync_WhenStatusIsUnknown_ReturnsZero()
        {
            await SeedAppointmentAsync(
                (AppointmentStatus)999);

            var result =
                await _service
                    .CreateAppointmentStatusNotificationsAsync(
                        CreateAppointment(
                            status:
                                (AppointmentStatus)999));

            result.Should().Be(0);
        }

        [Fact]
        public async Task
            CreateAppointmentStatusNotificationsAsync_WhenRelatedDoctorIsMissing_ReturnsZeroWithoutBreakingStatusUpdate()
        {
            var patient = CreatePatient();

            var appointment =
                CreateAppointment(
                    status:
                        AppointmentStatus.Confirmed);

            appointment.Patient = patient;
            appointment.Doctor = null!;

            _context.Patients.Add(patient);
            _context.Appointments.Add(
                appointment);

            await _context.SaveChangesAsync();

            var result =
                await _service
                    .CreateAppointmentStatusNotificationsAsync(
                        appointment);

            result.Should().Be(0);
        }

        [Fact]
        public async Task
            GetPatientNotificationsAsync_WhenRecipientIdIsInvalid_ThrowsValidationException()
        {
            Func<Task> act = async () =>
                await _service
                    .GetPatientNotificationsAsync(0);

            await act.Should()
                .ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task
            GetDoctorNotificationsAsync_WhenRecipientIdIsInvalid_ThrowsValidationException()
        {
            Func<Task> act = async () =>
                await _service
                    .GetDoctorNotificationsAsync(-1);

            await act.Should()
                .ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task
            GetPatientNotificationsAsync_WhenNotificationsExist_ReturnsNewestFirstWithDoctorDetails()
        {
            await SeedAppointmentAsync(
                AppointmentStatus.Confirmed);

            _context.Notifications.AddRange(
                CreateNotification(
                    id: 1,
                    patientId: 1,
                    doctorId: null,
                    createdDate:
                        DateTime.UtcNow.AddMinutes(-5)),

                CreateNotification(
                    id: 2,
                    patientId: 1,
                    doctorId: null,
                    createdDate:
                        DateTime.UtcNow));

            await _context.SaveChangesAsync();

            var result =
                await _service
                    .GetPatientNotificationsAsync(1);

            result.Should().HaveCount(2);

            result[0].NotificationId
                .Should().Be(2);

            result[0].DoctorName
                .Should().Be("Dr John");

            result[0].DoctorSpecialisation
                .Should().Be(
                    Specialisation.Cardiology
                        .ToString());
        }

        [Fact]
        public async Task
            GetDoctorNotificationsAsync_WhenNotificationsExist_ReturnsOnlyDoctorNotifications()
        {
            _context.Notifications.AddRange(
                CreateNotification(
                    id: 1,
                    patientId: null,
                    doctorId: 1),

                CreateNotification(
                    id: 2,
                    patientId: null,
                    doctorId: 2));

            await _context.SaveChangesAsync();

            var result =
                await _service
                    .GetDoctorNotificationsAsync(1);

            result.Should().ContainSingle();

            result[0].NotificationId
                .Should().Be(1);
        }

        [Fact]
        public async Task
            GetPatientUnreadCountAsync_WhenReadAndUnreadNotificationsExist_ReturnsUnreadCount()
        {
            _context.Notifications.AddRange(
                CreateNotification(
                    id: 1,
                    patientId: 1,
                    doctorId: null,
                    isRead: false),

                CreateNotification(
                    id: 2,
                    patientId: 1,
                    doctorId: null,
                    isRead: true));

            await _context.SaveChangesAsync();

            var result =
                await _service
                    .GetPatientUnreadCountAsync(1);

            result.Should().Be(1);
        }

        [Fact]
        public async Task
            GetDoctorUnreadCountAsync_WhenUnreadNotificationsExist_ReturnsUnreadCount()
        {
            _context.Notifications.AddRange(
                CreateNotification(
                    id: 1,
                    patientId: null,
                    doctorId: 1,
                    isRead: false),

                CreateNotification(
                    id: 2,
                    patientId: null,
                    doctorId: 1,
                    isRead: false));

            await _context.SaveChangesAsync();

            var result =
                await _service
                    .GetDoctorUnreadCountAsync(1);

            result.Should().Be(2);
        }

        [Fact]
        public async Task
            MarkPatientNotificationAsReadAsync_WhenNotificationIdIsInvalid_ThrowsValidationException()
        {
            Func<Task> act = async () =>
                await _service
                    .MarkPatientNotificationAsReadAsync(
                        0,
                        1);

            await act.Should()
                .ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task
            MarkPatientNotificationAsReadAsync_WhenNotificationDoesNotExist_ThrowsNotFoundException()
        {
            Func<Task> act = async () =>
                await _service
                    .MarkPatientNotificationAsReadAsync(
                        99,
                        1);

            await act.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task
            MarkPatientNotificationAsReadAsync_WhenUnread_MarksNotificationAsRead()
        {
            _context.Notifications.Add(
                CreateNotification(
                    patientId: 1,
                    doctorId: null,
                    isRead: false));

            await _context.SaveChangesAsync();

            await _service
                .MarkPatientNotificationAsReadAsync(
                    1,
                    1);

            var notification =
                await _context.Notifications
                    .SingleAsync();

            notification.IsRead.Should().BeTrue();
        }

        [Fact]
        public async Task
            MarkDoctorNotificationAsReadAsync_WhenAlreadyRead_DoesNotChangeNotification()
        {
            _context.Notifications.Add(
                CreateNotification(
                    patientId: null,
                    doctorId: 1,
                    isRead: true));

            await _context.SaveChangesAsync();

            await _service
                .MarkDoctorNotificationAsReadAsync(
                    1,
                    1);

            var notification =
                await _context.Notifications
                    .SingleAsync();

            notification.IsRead.Should().BeTrue();
        }

        [Fact]
        public async Task
            MarkAllPatientNotificationsAsReadAsync_WhenRecipientIdIsInvalid_ThrowsValidationException()
        {
            Func<Task> act = async () =>
                await _service
                    .MarkAllPatientNotificationsAsReadAsync(
                        0);

            await act.Should()
                .ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task
            MarkAllDoctorNotificationsAsReadAsync_WhenRecipientIdIsInvalid_ThrowsValidationException()
        {
            Func<Task> act = async () =>
                await _service
                    .MarkAllDoctorNotificationsAsReadAsync(
                        -1);

            await act.Should()
                .ThrowAsync<ApiValidationException>();
        }

        private async Task SeedAppointmentAsync(
            AppointmentStatus status,
            string? cancellationReason = null)
        {
            var patient = CreatePatient();
            var doctor = CreateDoctor();

            var appointment =
                CreateAppointment(
                    status: status,
                    cancellationReason:
                        cancellationReason);

            appointment.Patient = patient;
            appointment.Doctor = doctor;

            _context.Patients.Add(patient);
            _context.Doctors.Add(doctor);
            _context.Appointments.Add(
                appointment);

            await _context.SaveChangesAsync();
        }

        private static AppointmentBookedEvent
            CreateBookedEvent()
        {
            return new AppointmentBookedEvent
            {
                EventType =
                    "AppointmentBooked",
                PatientId = 1,
                PatientName = "Mona",
                DoctorName = "Dr John",
                DoctorId = 1,
                AppointmentId = 1,
                ScheduledDate =
                    DateTime.Today.AddDays(1),
                TimeSlot =
                    "10:00 AM - 11:00 AM",
                Status =
                    AppointmentStatus.Pending
                        .ToString(),
                OccurredAt = DateTime.UtcNow
            };
        }

        private static Notification
            CreateNotification(
                int id = 1,
                int? patientId = 1,
                int? doctorId = null,
                string notificationType =
                    "General",
                bool isRead = false,
                DateTime? createdDate = null)
        {
            return new Notification
            {
                NotificationId = id,
                AppointmentId = 1,
                PatientId = patientId,
                DoctorId = doctorId,
                Title = "Test notification",
                Message = "Test message",
                NotificationType =
                    notificationType,
                IsRead = isRead,
                CreatedDate =
                    createdDate ??
                    DateTime.UtcNow
            };
        }

        private static Appointment
            CreateAppointment(
                int id = 1,
                AppointmentStatus status =
                    AppointmentStatus.Pending,
                string? cancellationReason = null)
        {
            return new Appointment
            {
                AppointmentId = id,
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate =
                    DateTime.Today.AddDays(1),
                TimeSlot =
                    "10:00 AM - 11:00 AM",
                Status = status,
                CancellationReason =
                    cancellationReason
            };
        }

        private static Patient CreatePatient()
        {
            return new Patient
            {
                PatientId = 1,
                FullName = "Mona",
                DateOfBirth =
                    new DateTime(2004, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "9876543210",
                Email = "mona@gmail.com",
                UserId = "patient-user-1",
                CreatedDate = DateTime.Today
            };
        }

        private static Doctor CreateDoctor()
        {
            return new Doctor
            {
                DoctorId = 1,
                FullName = "Dr John",
                Specialisation =
                    Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true,
                UserId = "doctor-user-1"
            };
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}