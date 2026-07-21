using HealthAxis.API.Exceptions;
using HealthAxis.API.Services;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxis.API.Controller
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize(Roles = "Patient,Doctor")]
    public sealed class NotificationController : ControllerBase
    {
        private readonly INotificationService
            _notificationService;

        private readonly IPatientService
            _patientService;

        private readonly IDoctorService
            _doctorService;

        public NotificationController(
            INotificationService notificationService,
            IPatientService patientService,
            IDoctorService doctorService)
        {
            _notificationService = notificationService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotifications(
            CancellationToken cancellationToken)
        {
            var recipient =
                await GetCurrentRecipientAsync();

            if (recipient.PatientId is int patientId)
            {
                var notifications =
                    await _notificationService
                        .GetPatientNotificationsAsync(
                            patientId,
                            cancellationToken);

                return Ok(notifications);
            }

            var doctorId =
                GetDoctorId(recipient);

            var doctorNotifications =
                await _notificationService
                    .GetDoctorNotificationsAsync(
                        doctorId,
                        cancellationToken);

            return Ok(doctorNotifications);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount(
            CancellationToken cancellationToken)
        {
            var recipient =
                await GetCurrentRecipientAsync();

            int unreadCount;

            if (recipient.PatientId is int patientId)
            {
                unreadCount =
                    await _notificationService
                        .GetPatientUnreadCountAsync(
                            patientId,
                            cancellationToken);
            }
            else
            {
                unreadCount =
                    await _notificationService
                        .GetDoctorUnreadCountAsync(
                            GetDoctorId(recipient),
                            cancellationToken);
            }

            return Ok(new
            {
                unreadCount
            });
        }

        [HttpPut("{notificationId:int}/read")]
        public async Task<IActionResult> MarkAsRead(
            int notificationId,
            CancellationToken cancellationToken)
        {
            var recipient =
                await GetCurrentRecipientAsync();

            if (recipient.PatientId is int patientId)
            {
                await _notificationService
                    .MarkPatientNotificationAsReadAsync(
                        notificationId,
                        patientId,
                        cancellationToken);
            }
            else
            {
                await _notificationService
                    .MarkDoctorNotificationAsReadAsync(
                        notificationId,
                        GetDoctorId(recipient),
                        cancellationToken);
            }

            return Ok(new
            {
                message =
                    "Notification marked as read."
            });
        }

        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsRead(
            CancellationToken cancellationToken)
        {
            var recipient =
                await GetCurrentRecipientAsync();

            int updatedCount;

            if (recipient.PatientId is int patientId)
            {
                updatedCount =
                    await _notificationService
                        .MarkAllPatientNotificationsAsReadAsync(
                            patientId,
                            cancellationToken);
            }
            else
            {
                updatedCount =
                    await _notificationService
                        .MarkAllDoctorNotificationsAsReadAsync(
                            GetDoctorId(recipient),
                            cancellationToken);
            }

            return Ok(new
            {
                message =
                    "Notifications marked as read.",

                updatedCount
            });
        }

        private async Task<NotificationRecipient>
            GetCurrentRecipientAsync()
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException();
            }

            if (User.IsInRole("Patient"))
            {
                var patient =
                    await _patientService
                        .GetByUserIdAsync(userId);

                if (patient == null)
                {
                    throw new NotFoundException(
                        "Patient profile not found.");
                }

                return new NotificationRecipient(
                    patient.PatientId,
                    null);
            }

            if (User.IsInRole("Doctor"))
            {
                var doctor =
                    await _doctorService
                        .GetByUserIdAsync(userId);

                if (doctor == null)
                {
                    throw new NotFoundException(
                        "Doctor profile not found.");
                }

                return new NotificationRecipient(
                    null,
                    doctor.DoctorId);
            }

            throw new UnauthorizedAccessException();
        }

        private static int GetDoctorId(
            NotificationRecipient recipient)
        {
            return recipient.DoctorId ??
                   throw new UnauthorizedAccessException();
        }

        private readonly record struct NotificationRecipient(
            int? PatientId,
            int? DoctorId);
    }
}