using HealthAxis.API.Data;
using HealthAxis.API.Events;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.NotificationDtos;
using HealthAxis.Shared.Enums;
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

        private const string AppointmentConfirmedTitle =
            "Appointment Confirmed";

        private const string AppointmentConfirmedType =
            "AppointmentConfirmed";

        private const string AppointmentCancelledTitle =
            "Appointment Cancelled";

        private const string AppointmentCancelledType =
            "AppointmentCancelled";

        private const string AppointmentCompletedTitle =
            "Appointment Completed";

        private const string AppointmentCompletedType =
            "AppointmentCompleted";

        private const string NotificationNotFound =
            "Notification not found.";

        private const string CancelledByPatientPrefix =
            "Cancelled by patient";

        private const string CancelledByDoctorPrefix =
            "Cancelled by doctor";

        private const string CancelledByAdminPrefix =
            "Cancelled by admin";

        private const string CancellationReasonMarker =
            "Reason:";

        private const string NoAdditionalReason =
            "No additional reason was provided.";

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
                LogDuplicateBookedNotification(
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
                LogDuplicateBookedNotification(
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

        public async Task<int>
            CreateAppointmentStatusNotificationsAsync(
                Appointment appointment,
                CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(appointment);

            if (appointment.Status ==
                AppointmentStatus.Pending)
            {
                return 0;
            }

            try
            {
                var currentAppointment =
                    await dbContext.Appointments
                        .AsNoTracking()
                        .Include(item => item.Patient)
                        .Include(item => item.Doctor)
                        .FirstOrDefaultAsync(
                            item =>
                                item.AppointmentId ==
                                appointment.AppointmentId,
                            cancellationToken);

                if (currentAppointment == null)
                {
                    logger.LogWarning(
                        "Status notification skipped because appointment {AppointmentId} was not found.",
                        appointment.AppointmentId);

                    return 0;
                }

                var notificationCandidates =
                    CreateStatusNotifications(
                        currentAppointment);

                if (notificationCandidates.Count == 0)
                {
                    return 0;
                }

                var newNotifications =
                    await RemoveExistingNotificationsAsync(
                        notificationCandidates,
                        cancellationToken);

                if (newNotifications.Count == 0)
                {
                    logger.LogInformation(
                        "Appointment status notifications already exist. AppointmentId: {AppointmentId}, Status: {Status}",
                        currentAppointment.AppointmentId,
                        currentAppointment.Status);

                    return 0;
                }

                await dbContext.Notifications.AddRangeAsync(
                    newNotifications,
                    cancellationToken);

                await dbContext.SaveChangesAsync(
                    cancellationToken);

                logger.LogInformation(
                    "{NotificationCount} appointment status notification(s) created. AppointmentId: {AppointmentId}, Status: {Status}",
                    newNotifications.Count,
                    currentAppointment.AppointmentId,
                    currentAppointment.Status);

                return newNotifications.Count;
            }
            catch (DbUpdateException exception)
                when (IsUniqueConstraintViolation(exception))
            {
                logger.LogInformation(
                    "Duplicate appointment status notification skipped. AppointmentId: {AppointmentId}, Status: {Status}",
                    appointment.AppointmentId,
                    appointment.Status);

                return 0;
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                /*
                 * The appointment status has already been saved.
                 * A notification problem must not undo that valid update.
                 */
                logger.LogError(
                    exception,
                    "Appointment status was updated, but its notification could not be created. AppointmentId: {AppointmentId}, Status: {Status}",
                    appointment.AppointmentId,
                    appointment.Status);

                return 0;
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
            try
            {
                var notifications =
                    await GetRecipientQuery(
                            patientId,
                            doctorId)
                        .AsNoTracking()
                        .OrderByDescending(
                            notification =>
                                notification.CreatedDate)
                        .ThenByDescending(
                            notification =>
                                notification.NotificationId)
                        .Select(notification =>
                            new NotificationDto
                            {
                                NotificationId =
                                    notification.NotificationId,

                                AppointmentId =
                                    notification.AppointmentId,

                                Title =
                                    notification.Title ??
                                    string.Empty,

                                Message =
                                    notification.Message ??
                                    string.Empty,

                                NotificationType =
                                    notification.NotificationType ??
                                    "General",

                                IsRead =
                                    notification.IsRead,

                                CreatedDate =
                                    notification.CreatedDate
                            })
                        .ToListAsync(cancellationToken);

                if (notifications.Count == 0)
                {
                    return notifications;
                }

                var appointmentIds = notifications
                    .Where(notification =>
                        notification.AppointmentId.HasValue)
                    .Select(notification =>
                        notification.AppointmentId!.Value)
                    .Distinct()
                    .ToList();

                var doctorDetails =
                    await GetDoctorDetailsByAppointmentAsync(
                        appointmentIds,
                        cancellationToken);

                foreach (var notification in notifications)
                {
                    if (
                        notification.AppointmentId is int appointmentId &&
                        doctorDetails.TryGetValue(
                            appointmentId,
                            out var doctorDetail)
                    )
                    {
                        notification.DoctorName =
                            doctorDetail.DoctorName;

                        notification.DoctorSpecialisation =
                            doctorDetail.DoctorSpecialisation;
                    }
                }

                return notifications;
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
                    "Failed to load notifications. PatientId: {PatientId}, DoctorId: {DoctorId}.",
                    patientId,
                    doctorId);

                throw;
            }
        }

        private async Task<
            Dictionary<int, NotificationDoctorDetails>>
            GetDoctorDetailsByAppointmentAsync(
                List<int> appointmentIds,
                CancellationToken cancellationToken)
        {
            if (appointmentIds.Count == 0)
            {
                return [];
            }

            var appointmentDoctorLinks =
                await dbContext.Appointments
                    .AsNoTracking()
                    .Where(appointment =>
                        appointmentIds.Contains(
                            appointment.AppointmentId))
                    .Select(appointment => new
                    {
                        appointment.AppointmentId,
                        appointment.DoctorId
                    })
                    .ToListAsync(cancellationToken);

            if (appointmentDoctorLinks.Count == 0)
            {
                return [];
            }

            var doctorIds = appointmentDoctorLinks
                .Select(item => item.DoctorId)
                .Distinct()
                .ToList();

            var doctorRows =
                await dbContext.Doctors
                    .AsNoTracking()
                    .Where(doctor =>
                        doctorIds.Contains(
                            doctor.DoctorId))
                    .Select(doctor => new
                    {
                        doctor.DoctorId,
                        doctor.FullName,
                        doctor.Specialisation
                    })
                    .ToListAsync(cancellationToken);

            var doctorsById = doctorRows
                .ToDictionary(
                    doctor => doctor.DoctorId,
                    doctor =>
                        new NotificationDoctorDetails(
                            doctor.FullName ??
                            string.Empty,
                            doctor.Specialisation.ToString()));

            var result =
                new Dictionary<
                    int,
                    NotificationDoctorDetails>();

            foreach (
                var appointmentLink
                in appointmentDoctorLinks)
            {
                if (
                    doctorsById.TryGetValue(
                        appointmentLink.DoctorId,
                        out var doctorDetail)
                )
                {
                    result[appointmentLink.AppointmentId] =
                        doctorDetail;
                }
            }

            return result;
        }

        private async Task<List<Notification>>
            RemoveExistingNotificationsAsync(
                List<Notification> notificationCandidates,
                CancellationToken cancellationToken)
        {
            var notificationsToCreate =
                new List<Notification>();

            foreach (var candidate in notificationCandidates)
            {
                var notificationExists =
                    await NotificationExistsAsync(
                        candidate,
                        cancellationToken);

                if (!notificationExists)
                {
                    notificationsToCreate.Add(candidate);
                }
            }

            return notificationsToCreate;
        }

        private Task<bool> NotificationExistsAsync(
            Notification candidate,
            CancellationToken cancellationToken)
        {
            return dbContext.Notifications
                .AsNoTracking()
                .AnyAsync(
                    notification =>
                        notification.AppointmentId ==
                        candidate.AppointmentId &&
                        notification.PatientId ==
                        candidate.PatientId &&
                        notification.DoctorId ==
                        candidate.DoctorId &&
                        notification.NotificationType ==
                        candidate.NotificationType,
                    cancellationToken);
        }

        private static List<Notification>
            CreateStatusNotifications(
                Appointment appointment)
        {
            if (appointment.Status ==
                AppointmentStatus.Confirmed)
            {
                return
                [
                    CreatePatientConfirmationNotification(
                        appointment)
                ];
            }

            if (appointment.Status ==
                AppointmentStatus.Cancelled)
            {
                return CreateCancellationNotifications(
                    appointment);
            }

            if (appointment.Status ==
                AppointmentStatus.Completed)
            {
                return
                [
                    CreatePatientCompletionNotification(
                        appointment)
                ];
            }

            return [];
        }

        private static Notification
            CreatePatientConfirmationNotification(
                Appointment appointment)
        {
            var message =
                $"Your appointment with " +
                $"{appointment.Doctor.FullName} on " +
                $"{appointment.ScheduledDate:dd MMM yyyy} at " +
                $"{appointment.TimeSlot} has been confirmed.";

            return CreateNotification(
                appointment,
                appointment.PatientId,
                null,
                AppointmentConfirmedTitle,
                message,
                AppointmentConfirmedType);
        }

        private static Notification
            CreatePatientCompletionNotification(
                Appointment appointment)
        {
            var message =
                $"Your appointment with " +
                $"{appointment.Doctor.FullName} on " +
                $"{appointment.ScheduledDate:dd MMM yyyy} at " +
                $"{appointment.TimeSlot} has been completed.";

            return CreateNotification(
                appointment,
                appointment.PatientId,
                null,
                AppointmentCompletedTitle,
                message,
                AppointmentCompletedType);
        }

        private static List<Notification>
            CreateCancellationNotifications(
                Appointment appointment)
        {
            if (WasCancelledBy(
                appointment.CancellationReason,
                CancelledByPatientPrefix))
            {
                return
                [
                    CreateDoctorCancellationNotification(
                        appointment,
                        "the patient")
                ];
            }

            if (WasCancelledBy(
                appointment.CancellationReason,
                CancelledByDoctorPrefix))
            {
                return
                [
                    CreatePatientCancellationNotification(
                        appointment,
                        "the doctor")
                ];
            }

            if (WasCancelledBy(
                appointment.CancellationReason,
                CancelledByAdminPrefix))
            {
                return
                [
                    CreatePatientCancellationNotification(
                        appointment,
                        "the admin"),

                    CreateDoctorCancellationNotification(
                        appointment,
                        "the admin")
                ];
            }

            /*
             * Unknown or system cancellation:
             * both users should still be informed.
             */
            return
            [
                CreatePatientCancellationNotification(
                    appointment,
                    "the hospital"),

                CreateDoctorCancellationNotification(
                    appointment,
                    "the hospital")
            ];
        }

        private static Notification
            CreatePatientCancellationNotification(
                Appointment appointment,
                string cancelledBy)
        {
            var reason =
                GetCancellationExplanation(
                    appointment.CancellationReason);

            var message =
                $"Your appointment with " +
                $"{appointment.Doctor.FullName} on " +
                $"{appointment.ScheduledDate:dd MMM yyyy} at " +
                $"{appointment.TimeSlot} was cancelled by " +
                $"{cancelledBy}. Reason: {reason}";

            return CreateNotification(
                appointment,
                appointment.PatientId,
                null,
                AppointmentCancelledTitle,
                message,
                AppointmentCancelledType);
        }

        private static Notification
            CreateDoctorCancellationNotification(
                Appointment appointment,
                string cancelledBy)
        {
            var reason =
                GetCancellationExplanation(
                    appointment.CancellationReason);

            var message =
                $"Appointment with " +
                $"{appointment.Patient.FullName} on " +
                $"{appointment.ScheduledDate:dd MMM yyyy} at " +
                $"{appointment.TimeSlot} was cancelled by " +
                $"{cancelledBy}. Reason: {reason}";

            return CreateNotification(
                appointment,
                null,
                appointment.DoctorId,
                AppointmentCancelledTitle,
                message,
                AppointmentCancelledType);
        }

        private static Notification CreateNotification(
            Appointment appointment,
            int? patientId,
            int? doctorId,
            string title,
            string message,
            string notificationType)
        {
            return new Notification
            {
                AppointmentId =
                    appointment.AppointmentId,

                PatientId = patientId,

                DoctorId = doctorId,

                Title = title,

                Message = message,

                NotificationType =
                    notificationType,

                IsRead = false,

                CreatedDate =
                    DateTime.UtcNow
            };
        }

        private static string
            GetCancellationExplanation(
                string? cancellationReason)
        {
            if (string.IsNullOrWhiteSpace(
                cancellationReason))
            {
                return NoAdditionalReason;
            }

            var reasonMarkerIndex =
                cancellationReason.IndexOf(
                    CancellationReasonMarker,
                    StringComparison.OrdinalIgnoreCase);

            if (reasonMarkerIndex < 0)
            {
                return NoAdditionalReason;
            }

            var reasonStartIndex =
                reasonMarkerIndex +
                CancellationReasonMarker.Length;

            var reason = cancellationReason
                .Substring(reasonStartIndex)
                .Trim();

            return string.IsNullOrWhiteSpace(reason)
                ? NoAdditionalReason
                : reason;
        }

        private static bool WasCancelledBy(
            string? cancellationReason,
            string cancellationPrefix)
        {
            return cancellationReason?.StartsWith(
                cancellationPrefix,
                StringComparison.OrdinalIgnoreCase) == true;
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

        private void LogDuplicateBookedNotification(
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

        private readonly record struct
            NotificationDoctorDetails(
                string DoctorName,
                string DoctorSpecialisation);

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