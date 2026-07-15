using AutoMapper;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Extensions;
using HealthAxisCore_Api.Messaging.Contracts;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;
using System.Globalization;
using System.Security.Claims;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class AppointmentService(
        IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository,
        IMapper mapper,
        IPublishEndpoint publishEndpoint,
        IDistributedCache distributedCache,
        AppDbContext dbContext
    ) : IAppointmentService
    {
        private static readonly Serilog.ILogger Logger =
            Log.ForContext<AppointmentService>();

        public async Task<List<AppointmentDto>> GetAppointmentsAsync(
            int? patientId,
            int? doctorId,
            DateTime? date,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            if (user.IsPatient())
            {
                patientId = user.GetPatientId()
                    ?? throw new UnauthorizedException("PatientId claim missing");
            }

            if (user.IsDoctor())
            {
                doctorId = user.GetDoctorId()
                    ?? throw new UnauthorizedException("DoctorId claim missing");
            }

            return mapper.Map<List<AppointmentDto>>(
                await appointmentRepository.GetAppointmentsAsync(
                    patientId,
                    doctorId,
                    date,
                    ct));
        }

        public async Task<AppointmentDto> CreateAsync(
            CreateAppointmentDto request,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            var patientId = user.GetPatientId()
                ?? throw new UnauthorizedException("PatientId claim missing");

            var patientAlreadyBookedAtSameTime =
                await appointmentRepository.PatientHasAppointmentAtSlotAsync(
                    patientId,
                    request.ScheduledDate,
                    request.TimeSlot,
                    ct);

            if (patientAlreadyBookedAtSameTime)
            {
                throw new InvalidException(
                    "You already have an appointment booked at this date and time.");
            }

            var doctor = await doctorRepository.GetByIdAsync(
                request.DoctorId,
                ct)
                ?? throw new NotFoundException("Doctor not found");

            if (!doctor.IsActive)
            {
                throw new InvalidException("Cannot book appointment with inactive doctor");
            }

            if (request.ScheduledDate.Date < DateTime.Now.Date)
            {
                throw new InvalidException("Cannot book past date");
            }

            EnsureWithinDoctorWorkingHours(request.TimeSlot);

            EnsureSameDaySlotIsInFuture(
                request.ScheduledDate,
                request.TimeSlot);

            EnsureBookingIsAtLeastTwoHoursBeforeSlot(
                request.ScheduledDate,
                request.TimeSlot);

            var isDoctorAlreadyBooked =
                await appointmentRepository.DoctorHasAppointmentAtSlotAsync(
                    request.DoctorId,
                    request.ScheduledDate,
                    request.TimeSlot,
                    ct);

            if (isDoctorAlreadyBooked)
            {
                throw new InvalidException(
                    "Doctor already has an appointment at this time slot");
            }

            var slots = await doctorRepository.GetAvailableSlotsAsync(
                request.DoctorId,
                request.ScheduledDate,
                ct);

            if (!slots.Contains(request.TimeSlot))
            {
                throw new InvalidException("Slot not available");
            }

            var appointment = mapper.Map<Appointment>(request);

            appointment.PatientId = patientId;
            appointment.Status = "Pending";
            appointment.CancellationReason = string.Empty;

            var savedAppointment = await appointmentRepository.CreateAsync(
                appointment,
                ct);

            var availabilityCacheKey =
                BuildDoctorAvailabilityCacheKey(
                    savedAppointment.DoctorId,
                    savedAppointment.ScheduledDate);

            await distributedCache.RemoveAsync(
                availabilityCacheKey,
                ct);

            Logger.Information(
                "[CACHE-INVALIDATE] Doctor availability cache removed | DoctorId={DoctorId} | Date={Date} | Key={CacheKey}",
                savedAppointment.DoctorId,
                savedAppointment.ScheduledDate.Date.ToString("yyyy-MM-dd"),
                availabilityCacheKey);

            var savedAppointmentDetails =
                await appointmentRepository.GetDetailsAsync(
                    savedAppointment.AppointmentId,
                    ct) ?? savedAppointment;

            var appointmentDto =
                mapper.Map<AppointmentDto>(savedAppointmentDetails);

            Logger.Information(
                "[APPOINTMENT-BOOKED] Appointment saved | AppointmentId={AppointmentId} | PatientId={PatientId} | DoctorId={DoctorId} | Date={Date} | Slot={TimeSlot}",
                savedAppointmentDetails.AppointmentId,
                savedAppointmentDetails.PatientId,
                savedAppointmentDetails.DoctorId,
                savedAppointmentDetails.ScheduledDate.ToString("yyyy-MM-dd"),
                savedAppointmentDetails.TimeSlot);

            await publishEndpoint.Publish(
                new AppointmentBookedEvent
                {
                    AppointmentId = savedAppointmentDetails.AppointmentId,
                    PatientId = savedAppointmentDetails.PatientId,
                    PatientName = savedAppointmentDetails.Patient?.PatientName
                        ?? appointmentDto.PatientName,
                    DoctorId = savedAppointmentDetails.DoctorId,
                    ScheduledDate = savedAppointmentDetails.ScheduledDate,
                    TimeSlot = savedAppointmentDetails.TimeSlot,
                    OccurredAt = DateTime.UtcNow
                },
                ct);

            Logger.Information(
                "[RABBITMQ-PUBLISH] AppointmentBookedEvent published | AppointmentId={AppointmentId} | DoctorId={DoctorId}",
                savedAppointmentDetails.AppointmentId,
                savedAppointmentDetails.DoctorId);

            return appointmentDto;
        }

        public async Task<AppointmentDto> UpdateStatusAsync(
            int id,
            UpdateAppointmentStatusDto request,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            var appointment = await appointmentRepository.GetDetailsAsync(id, ct)
                ?? throw new NotFoundException("Appointment not found");

            if (appointment.Status == "Cancelled")
            {
                throw new InvalidException("Cancelled appointment cannot be updated");
            }

            if (appointment.Status == "Completed")
            {
                throw new InvalidException("Completed appointment cannot be changed");
            }

            if (user.IsPatient())
            {
                ValidatePatientStatusUpdate(
                    appointment,
                    request,
                    user);
            }

            if (user.IsDoctor())
            {
                ValidateDoctorStatusUpdate(
                    appointment,
                    request,
                    user);
            }

            if (request.Status == "Cancelled")
            {
                if (string.IsNullOrWhiteSpace(request.CancellationReason))
                {
                    throw new InvalidException("Cancellation reason is required");
                }

                EnsureCancellationAllowed(appointment);

                return await ArchiveAndRemoveCancelledAppointmentAsync(
                    appointment,
                    request.CancellationReason,
                    user,
                    wasAutoCancelled: false,
                    ct);
            }

            var oldStatus = appointment.Status;

            appointment.Status = request.Status;
            appointment.CancellationReason = request.CancellationReason ?? string.Empty;

            var updatedAppointment = await appointmentRepository.UpdateAsync(
                id,
                appointment,
                ct)
                ?? throw new NotFoundException("Appointment not found");

            // CHANGE:
            // Mark notification as read when doctor confirms or completes the appointment.
            if (request.Status == "Confirmed" || request.Status == "Completed")
            {
                await MarkAppointmentNotificationAsReadAsync(
                    updatedAppointment.AppointmentId,
                    updatedAppointment.DoctorId,
                    request.Status,
                    ct);
            }

            Logger.Information(
                "[APPOINTMENT-STATUS] Status changed | AppointmentId={AppointmentId} | PatientId={PatientId} | DoctorId={DoctorId} | {OldStatus} -> {NewStatus}",
                updatedAppointment.AppointmentId,
                updatedAppointment.PatientId,
                updatedAppointment.DoctorId,
                oldStatus,
                request.Status);

            return mapper.Map<AppointmentDto>(
                await appointmentRepository.GetDetailsAsync(
                    updatedAppointment.AppointmentId,
                    ct) ?? updatedAppointment);
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken ct = default)
        {
            if (await appointmentRepository.DeleteAsync(id, ct) == null)
            {
                throw new NotFoundException("Appointment not found");
            }
        }

        private async Task<AppointmentDto> ArchiveAndRemoveCancelledAppointmentAsync(
            Appointment appointment,
            string cancellationReason,
            ClaimsPrincipal user,
            bool wasAutoCancelled,
            CancellationToken ct)
        {
            var cancelledByRole = GetCancellerRole(user);

            var cancelledByUserId =
                user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Unknown";

            var cancelledDto = new AppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient?.PatientName ?? string.Empty,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor?.DoctorName ?? string.Empty,
                Specialisation = appointment.Doctor?.Specialisation ?? string.Empty,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.TimeSlot,
                Status = "Cancelled",
                CancellationReason = cancellationReason
            };

            var archive = new CancelledAppointmentArchive
            {
                OriginalAppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient?.PatientName ?? string.Empty,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor?.DoctorName ?? string.Empty,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.TimeSlot,
                CancellationReason = cancellationReason,
                CancelledByRole = cancelledByRole,
                CancelledByUserId = cancelledByUserId,
                CancelledAt = DateTime.UtcNow,
                WasAutoCancelled = wasAutoCancelled,
                ArchivedAt = DateTime.UtcNow,
                LegalHold = false
            };

            await dbContext.CancelledAppointmentArchives.AddAsync(
                archive,
                ct);

            await MarkAppointmentNotificationAsReadAsync(
                appointment.AppointmentId,
                appointment.DoctorId,
                "Cancelled",
                ct);

            var availabilityCacheKey =
                BuildDoctorAvailabilityCacheKey(
                    appointment.DoctorId,
                    appointment.ScheduledDate);

            await distributedCache.RemoveAsync(
                availabilityCacheKey,
                ct);

            dbContext.Appointments.Remove(appointment);

            await dbContext.SaveChangesAsync(ct);

            Logger.Warning(
                "[APPOINTMENT-ARCHIVED] Cancelled appointment moved to archive and removed from active table | AppointmentId={AppointmentId} | PatientId={PatientId} | DoctorId={DoctorId} | CancelledBy={CancelledByRole} | CacheKey={CacheKey}",
                appointment.AppointmentId,
                appointment.PatientId,
                appointment.DoctorId,
                cancelledByRole,
                availabilityCacheKey);

            return cancelledDto;
        }

        private static string GetCancellerRole(ClaimsPrincipal user)
        {
            if (user.IsPatient())
            {
                return "Patient";
            }

            if (user.IsDoctor())
            {
                return "Doctor";
            }

            if (user.IsInRole("Admin"))
            {
                return "Admin";
            }

            return "Unknown";
        }

        private static void ValidatePatientStatusUpdate(
            Appointment appointment,
            UpdateAppointmentStatusDto request,
            ClaimsPrincipal user)
        {
            var patientId = user.GetPatientId()
                ?? throw new UnauthorizedException("PatientId claim missing");

            if (appointment.PatientId != patientId)
            {
                throw new UnauthorizedException(
                    "You can update only your own appointment");
            }

            if (request.Status != "Cancelled")
            {
                throw new UnauthorizedException(
                    "Patients can only cancel appointments");
            }
        }

        private static void ValidateDoctorStatusUpdate(
            Appointment appointment,
            UpdateAppointmentStatusDto request,
            ClaimsPrincipal user)
        {
            var doctorId = user.GetDoctorId()
                ?? throw new UnauthorizedException("DoctorId claim missing");

            if (appointment.DoctorId != doctorId)
            {
                throw new UnauthorizedException(
                    "You can update only your own appointment");
            }

            if (request.Status != "Confirmed" &&
                request.Status != "Completed" &&
                request.Status != "Cancelled")
            {
                throw new UnauthorizedException(
                    "Doctors can only confirm, complete, or cancel appointments");
            }

            if (request.Status == "Confirmed" &&
                appointment.Status != "Pending")
            {
                throw new InvalidException(
                    "Only pending appointments can be confirmed");
            }

            if (request.Status == "Completed" &&
                appointment.Status != "Confirmed")
            {
                throw new InvalidException(
                    "Appointment must be confirmed before it can be completed");
            }
        }

        private async Task MarkAppointmentNotificationAsReadAsync(
            int appointmentId,
            int doctorId,
            string appointmentStatus,
            CancellationToken ct)
        {
            var notification = await dbContext.Notifications
                .Where(notification =>
                    notification.AppointmentId == appointmentId &&
                    notification.DoctorId == doctorId &&
                    !notification.IsRead)
                .OrderByDescending(notification => notification.CreatedAt)
                .FirstOrDefaultAsync(ct);

            if (notification == null)
            {
                Logger.Warning(
                    "[NOTIFICATION-READ-SKIPPED] No unread notification found | AppointmentId={AppointmentId} | DoctorId={DoctorId} | Status={AppointmentStatus}",
                    appointmentId,
                    doctorId,
                    appointmentStatus);

                return;
            }

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;

            // IMPORTANT:
            // This is required when notification is marked read during Confirmed/Completed flow.
            await dbContext.SaveChangesAsync(ct);

            Logger.Information(
                "[NOTIFICATION-READ] Notification marked as read | NotificationId={NotificationId} | AppointmentId={AppointmentId} | DoctorId={DoctorId} | Status={AppointmentStatus}",
                notification.NotificationId,
                appointmentId,
                doctorId,
                appointmentStatus);
        }

        private static void EnsureBookingIsAtLeastTwoHoursBeforeSlot(
            DateTime scheduledDate,
            string timeSlot)
        {
            var appointmentDateTime = BuildAppointmentDateTime(
                scheduledDate,
                timeSlot);

            var minimumAllowedBookingTime = appointmentDateTime.AddHours(-2);

            if (DateTime.Now >= minimumAllowedBookingTime)
            {
                throw new InvalidException(
                    "Appointments must be booked at least 2 hours before the scheduled time.");
            }
        }

        private static string BuildDoctorAvailabilityCacheKey(
            int doctorId,
            DateTime scheduledDate)
        {
            return $"doctors:{doctorId}:availability:{scheduledDate:yyyy-MM-dd}";
        }

        private static void EnsureWithinDoctorWorkingHours(string timeSlot)
        {
            var slot = ParseTimeSlot(timeSlot);

            var workingStart = new TimeOnly(9, 0);
            var workingEnd = new TimeOnly(17, 0);

            if (slot < workingStart || slot >= workingEnd)
            {
                throw new InvalidException(
                    "Doctor working hours are from 09:00 to 17:00");
            }
        }

        private static void EnsureSameDaySlotIsInFuture(
            DateTime scheduledDate,
            string timeSlot)
        {
            var today = DateTime.Now.Date;

            if (scheduledDate.Date != today)
            {
                return;
            }

            var selectedSlot = ParseTimeSlot(timeSlot);

            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            if (selectedSlot <= currentTime)
            {
                throw new InvalidException(
                    "Cannot book a time slot that has already passed for today");
            }
        }

        private static void EnsureCancellationAllowed(
            Appointment appointment)
        {
            var appointmentDateTime = BuildAppointmentDateTime(
                appointment.ScheduledDate,
                appointment.TimeSlot);

            var cancellationCutOff = appointmentDateTime.AddHours(-2);

            if (DateTime.Now >= cancellationCutOff)
            {
                throw new InvalidException(
                    "Cannot cancel appointment within 2 hours before the slot time");
            }
        }

        private static DateTime BuildAppointmentDateTime(
            DateTime scheduledDate,
            string timeSlot)
        {
            var slot = ParseTimeSlot(timeSlot);

            return scheduledDate.Date
                .AddHours(slot.Hour)
                .AddMinutes(slot.Minute);
        }

        private static TimeOnly ParseTimeSlot(string timeSlot)
        {
            if (!TimeOnly.TryParse(
                    timeSlot,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var parsedTime))
            {
                throw new FormatException("Invalid time slot format");
            }

            return parsedTime;
        }
    }
}