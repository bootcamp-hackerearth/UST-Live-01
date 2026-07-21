using HealthAxis.API.Data;
using HealthAxis.API.Events;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.NotificationDtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Services.Implementation
{
    public sealed class NotificationService(
        ApplicationDbContext dbContext,
        ILogger<NotificationService> logger)
        : INotificationService
    {
        private const int DuplicateIndexErrorNumber = 2601;

        private const int UniqueConstraintErrorNumber = 2627;

        private const string AppointmentBookedTitle =
            "New Appointment Booked";

        private const string AppointmentBookedType =
            "AppointmentBooked";

        private const string NotificationNotFound =
            "Notification not found.";

        public async Task<bool>
            CreateAppointmentBookedNotificationAsync(
                AppointmentBookedEvent appointmentBookedEvent,
                CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(
                appointmentBookedEvent);

            var notificationAlreadyExists =
                await dbContext.Notifications
                    .AsNoTracking()
                    .AnyAsync(
                        notification =>
                            notification.AppointmentId ==
                            appointmentBookedEvent.AppointmentId &&
                            notification.DoctorId ==
                            appointmentBookedEvent.DoctorId &&
                            notification.NotificationType ==
                            AppointmentBookedType,
                        cancellationToken);

            if (notificationAlreadyExists)
            {
                LogDuplicateNotification(
                    appointmentBookedEvent);

                return false;
            }

            var notification =
                CreateAppointmentBookedNotification(
                    appointmentBookedEvent);

            try
            {
                await dbContext.Notifications.AddAsync(
                    notification,
                    cancellationToken);

                await dbContext.SaveChangesAsync(
                    cancellationToken);

                logger.LogInformation(
                    "Doctor notification created. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}",
                    appointmentBookedEvent.AppointmentId,
                    appointmentBookedEvent.DoctorId);

                return true;
            }
            catch (DbUpdateException exception)
                when (IsUniqueConstraintViolation(exception))
            {
                LogDuplicateNotification(
                    appointmentBookedEvent);

                return false;
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "Notification creation failed. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}.",
                    appointmentBookedEvent.AppointmentId,
                    appointmentBookedEvent.DoctorId);

                throw;
            }
        }

        public Task<List<NotificationDto>>
            GetPatientNotificationsAsync(
                int patientId,
                CancellationToken cancellationToken = default)
        {
            ValidateRecipientId(patientId);

            return GetNotificationsAsync(
                patientId,
                null,
                cancellationToken);
        }

        public Task<List<NotificationDto>>
            GetDoctorNotificationsAsync(
                int doctorId,
                CancellationToken cancellationToken = default)
        {
            ValidateRecipientId(doctorId);

            return GetNotificationsAsync(
                null,
                doctorId,
                cancellationToken);
        }

        public Task<int> GetPatientUnreadCountAsync(
            int patientId,
            CancellationToken cancellationToken = default)
        {
            ValidateRecipientId(patientId);

            return GetUnreadCountAsync(
                patientId,
                null,
                cancellationToken);
        }

        public Task<int> GetDoctorUnreadCountAsync(
            int doctorId,
            CancellationToken cancellationToken = default)
        {
            ValidateRecipientId(doctorId);

            return GetUnreadCountAsync(
                null,
                doctorId,
                cancellationToken);
        }

        public Task MarkPatientNotificationAsReadAsync(
            int notificationId,
            int patientId,
            CancellationToken cancellationToken = default)
        {
            ValidateRecipientId(patientId);

            return MarkNotificationAsReadAsync(
                notificationId,
                patientId,
                null,
                cancellationToken);
        }

        public Task MarkDoctorNotificationAsReadAsync(
            int notificationId,
            int doctorId,
            CancellationToken cancellationToken = default)
        {
            ValidateRecipientId(doctorId);

            return MarkNotificationAsReadAsync(
                notificationId,
                null,
                doctorId,
                cancellationToken);
        }

        public Task<int>
            MarkAllPatientNotificationsAsReadAsync(
                int patientId,
                CancellationToken cancellationToken = default)
        {
            ValidateRecipientId(patientId);

            return MarkAllNotificationsAsReadAsync(
                patientId,
                null,
                cancellationToken);
        }

        public Task<int>
            MarkAllDoctorNotificationsAsReadAsync(
                int doctorId,
                CancellationToken cancellationToken = default)
        {
            ValidateRecipientId(doctorId);

            return MarkAllNotificationsAsReadAsync(
                null,
                doctorId,
                cancellationToken);
        }

        private async Task<List<NotificationDto>>
            GetNotificationsAsync(
                int? patientId,
                int? doctorId,
                CancellationToken cancellationToken)
        {
            var notificationDetails =
                await (
                    from notification in GetRecipientQuery(
                            patientId,
                            doctorId)
                        .AsNoTracking()

                    join appointment
                        in dbContext.Appointments.AsNoTracking()
                        on notification.AppointmentId
                        equals
                        (int?)appointment.AppointmentId
                        into matchingAppointments

                    from appointment
                        in matchingAppointments.DefaultIfEmpty()

                    join doctor
                        in dbContext.Doctors.AsNoTracking()
                        on appointment.DoctorId
                        equals doctor.DoctorId
                        into matchingDoctors

                    from doctor
                        in matchingDoctors.DefaultIfEmpty()

                    orderby
                        notification.CreatedDate descending,
                        notification.NotificationId descending

                    select new
                    {
                        Notification = notification,
                        Doctor = doctor
                    })
                .ToListAsync(cancellationToken);

            return notificationDetails
                .Select(item =>
                    CreateNotificationDto(
                        item.Notification,
                        item.Doctor))
                .ToList();
        }

        private async Task<int> GetUnreadCountAsync(
            int? patientId,
            int? doctorId,
            CancellationToken cancellationToken)
        {
            return await GetRecipientQuery(
                    patientId,
                    doctorId)
                .AsNoTracking()
                .CountAsync(
                    notification => !notification.IsRead,
                    cancellationToken);
        }

        private async Task
            MarkNotificationAsReadAsync(
                int notificationId,
                int? patientId,
                int? doctorId,
                CancellationToken cancellationToken)
        {
            if (notificationId <= 0)
            {
                throw new ValidationException(
                    "Valid notification id is required.");
            }

            var notification =
                await GetRecipientQuery(
                        patientId,
                        doctorId)
                    .FirstOrDefaultAsync(
                        item =>
                            item.NotificationId ==
                            notificationId,
                        cancellationToken);

            if (notification == null)
            {
                throw new NotFoundException(
                    NotificationNotFound);
            }

            if (notification.IsRead)
            {
                return;
            }

            notification.IsRead = true;

            await dbContext.SaveChangesAsync(
                cancellationToken);
        }

        private async Task<int>
            MarkAllNotificationsAsReadAsync(
                int? patientId,
                int? doctorId,
                CancellationToken cancellationToken)
        {
            return await GetRecipientQuery(
                    patientId,
                    doctorId)
                .Where(
                    notification =>
                        !notification.IsRead)
                .ExecuteUpdateAsync(
                    setters =>
                        setters.SetProperty(
                            notification =>
                                notification.IsRead,
                            true),
                    cancellationToken);
        }

        private IQueryable<Notification>
            GetRecipientQuery(
                int? patientId,
                int? doctorId)
        {
            if (patientId.HasValue)
            {
                return dbContext.Notifications.Where(
                    notification =>
                        notification.PatientId ==
                        patientId.Value);
            }

            if (doctorId.HasValue)
            {
                return dbContext.Notifications.Where(
                    notification =>
                        notification.DoctorId ==
                        doctorId.Value);
            }

            throw new ValidationException(
                "A valid notification recipient is required.");
        }

        private static NotificationDto
            CreateNotificationDto(
                Notification notification,
                Doctor? doctor)
        {
            return new NotificationDto
            {
                NotificationId =
                    notification.NotificationId,

                AppointmentId =
                    notification.AppointmentId,

                Title =
                    notification.Title,

                Message =
                    notification.Message,

                NotificationType =
                    notification.NotificationType,

                IsRead =
                    notification.IsRead,

                CreatedDate =
                    notification.CreatedDate,

                DoctorName =
                    doctor?.FullName,

                DoctorSpecialisation =
                    doctor?.Specialisation.ToString()
            };
        }

        private static Notification
            CreateAppointmentBookedNotification(
                AppointmentBookedEvent appointmentBookedEvent)
        {
            return new Notification
            {
                AppointmentId =
                    appointmentBookedEvent.AppointmentId,

                PatientId = null,

                DoctorId =
                    appointmentBookedEvent.DoctorId,

                Title =
                    AppointmentBookedTitle,

                Message =
                    $"New appointment booked by " +
                    $"{appointmentBookedEvent.PatientName} " +
                    $"on " +
                    $"{appointmentBookedEvent.ScheduledDate:yyyy-MM-dd} " +
                    $"at {appointmentBookedEvent.TimeSlot}.",

                NotificationType =
                    AppointmentBookedType,

                IsRead = false,

                CreatedDate =
                    DateTime.UtcNow
            };
        }

        private void LogDuplicateNotification(
            AppointmentBookedEvent appointmentBookedEvent)
        {
            logger.LogInformation(
                "Duplicate appointment notification skipped. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}",
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.DoctorId);
        }

        private static bool
            IsUniqueConstraintViolation(
                DbUpdateException exception)
        {
            if (exception.InnerException
                is not SqlException sqlException)
            {
                return false;
            }

            return sqlException.Number ==
                       DuplicateIndexErrorNumber ||
                   sqlException.Number ==
                       UniqueConstraintErrorNumber;
        }

        private static void ValidateRecipientId(
            int recipientId)
        {
            if (recipientId <= 0)
            {
                throw new ValidationException(
                    "Valid notification recipient id is required.");
            }
        }
    }
}