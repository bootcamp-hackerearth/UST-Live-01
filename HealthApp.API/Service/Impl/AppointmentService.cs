using AutoMapper;
using HealthApp.API.Data;
using HealthApp.API.Events;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace HealthApp.API.Service.Impl;

public class AppointmentService(
    IAppointmentRepository appointmentRepository,
    IPatientRepository patientRepository,
    IDoctorRepository doctorRepository,
    IDoctorLeaveRepository doctorLeaveRepository,
    IHttpContextAccessor httpContextAccessor,
    IDistributedCache distributedCache,
    HealthAppDbContext dbContext,
    IMapper mapper,
    ILogger<AppointmentService> logger) : IAppointmentService
{
    private const string AppointmentAccessDeniedMessage =
        "You are not allowed to access appointments.";

    private const string InvalidStatusTransitionMessage =
        "Invalid appointment status transition.";

    private const string DoctorOnLeaveBookingMessage =
        "The selected doctor is on leave on the selected date. Please choose another date.";

    public async Task<List<AppointmentDto>> GetAppointmentsAsync(
        int? patientId = null,
        int? doctorId = null)
    {
        if (IsPatient())
        {
            var patient = await GetLoggedInPatientAsync();

            if (patientId.HasValue && patientId.Value != patient.PatientId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another patient's appointments.");
            }

            return mapper.Map<List<AppointmentDto>>(
                await appointmentRepository.GetByPatientIdAsync(patient.PatientId));
        }

        if (IsDoctor())
        {
            var doctor = await GetLoggedInDoctorAsync();

            if (doctorId.HasValue && doctorId.Value != doctor.DoctorId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another doctor's appointments.");
            }

            var appointments = await appointmentRepository.GetByDoctorIdAsync(doctor.DoctorId);

            if (patientId.HasValue)
            {
                appointments = appointments
                    .Where(a => a.PatientId == patientId.Value)
                    .ToList();
            }

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        if (IsAdmin())
        {
            if (patientId.HasValue)
            {
                await ValidatePatientExistsAsync(patientId.Value);

                return mapper.Map<List<AppointmentDto>>(
                    await appointmentRepository.GetByPatientIdAsync(patientId.Value));
            }

            if (doctorId.HasValue)
            {
                await ValidateDoctorExistsAsync(doctorId.Value);

                return mapper.Map<List<AppointmentDto>>(
                    await appointmentRepository.GetByDoctorIdAsync(doctorId.Value));
            }

            return mapper.Map<List<AppointmentDto>>(
                await appointmentRepository.GetAllWithDetailsAsync());
        }

        throw new ForbiddenAccessException(AppointmentAccessDeniedMessage);
    }

    public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
    {
        if (IsPatient())
        {
            var patient = await GetLoggedInPatientAsync();

            return mapper.Map<List<AppointmentDto>>(
                await appointmentRepository.GetByPatientIdAsync(patient.PatientId));
        }

        if (IsDoctor())
        {
            var doctor = await GetLoggedInDoctorAsync();

            return mapper.Map<List<AppointmentDto>>(
                await appointmentRepository.GetByDoctorIdAsync(doctor.DoctorId));
        }

        if (IsAdmin())
        {
            return mapper.Map<List<AppointmentDto>>(
                await appointmentRepository.GetAllWithDetailsAsync());
        }

        throw new ForbiddenAccessException(AppointmentAccessDeniedMessage);
    }

    public async Task<AppointmentDto> GetAppointmentByIdAsync(int appointmentId)
    {
        ValidateAppointmentId(appointmentId);

        var appointment = await appointmentRepository.GetByIdWithDetailsAsync(appointmentId)
            ?? throw new EntityNotFoundException("Appointment", appointmentId);

        await EnsureAppointmentAccessAsync(appointment);

        return mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId)
    {
        await ValidatePatientExistsAsync(patientId);

        if (IsPatient())
        {
            var loggedInPatient = await GetLoggedInPatientAsync();

            if (loggedInPatient.PatientId != patientId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another patient's appointments.");
            }

            return mapper.Map<List<AppointmentDto>>(
                await appointmentRepository.GetByPatientIdAsync(patientId));
        }

        if (IsDoctor())
        {
            var loggedInDoctor = await GetLoggedInDoctorAsync();

            var doctorAppointments = await appointmentRepository
                .GetByDoctorIdAsync(loggedInDoctor.DoctorId);

            var patientAppointments = doctorAppointments
                .Where(a => a.PatientId == patientId)
                .ToList();

            return mapper.Map<List<AppointmentDto>>(patientAppointments);
        }

        if (IsAdmin())
        {
            return mapper.Map<List<AppointmentDto>>(
                await appointmentRepository.GetByPatientIdAsync(patientId));
        }

        throw new ForbiddenAccessException(AppointmentAccessDeniedMessage);
    }

    public async Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId)
    {
        await ValidateDoctorExistsAsync(doctorId);

        if (IsPatient())
        {
            throw new ForbiddenAccessException(
                "Patients are not allowed to search appointments by doctor.");
        }

        if (IsDoctor())
        {
            var loggedInDoctor = await GetLoggedInDoctorAsync();

            if (loggedInDoctor.DoctorId != doctorId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another doctor's appointments.");
            }
        }

        return mapper.Map<List<AppointmentDto>>(
            await appointmentRepository.GetByDoctorIdAsync(doctorId));
    }

    public async Task<List<AppointmentDto>> GetAppointmentsByStatusAsync(AppointmentStatus status)
    {
        if (IsPatient())
        {
            var patient = await GetLoggedInPatientAsync();

            var appointments = await appointmentRepository.GetByPatientIdAsync(patient.PatientId);

            appointments = appointments
                .Where(a => a.Status == status.ToString())
                .ToList();

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        if (IsDoctor())
        {
            var doctor = await GetLoggedInDoctorAsync();

            var appointments = await appointmentRepository.GetByDoctorIdAsync(doctor.DoctorId);

            appointments = appointments
                .Where(a => a.Status == status.ToString())
                .ToList();

            return mapper.Map<List<AppointmentDto>>(appointments);
        }

        if (IsAdmin())
        {
            return mapper.Map<List<AppointmentDto>>(
                await appointmentRepository.GetByStatusAsync(status));
        }

        throw new ForbiddenAccessException(AppointmentAccessDeniedMessage);
    }

    public async Task<AppointmentDto> BookAppointmentAsync(BookAppointmentDto dto)
    {
        if (dto is null)
        {
            throw new AppointmentRuleException("Appointment details are required.");
        }

        var patient = await GetLoggedInPatientAsync();
        var doctor = await ValidateDoctorExistsAsync(dto.DoctorId);
        var appointmentDate = dto.ScheduledDate.Date;

        if (!doctor.IsActive)
        {
            throw new AppointmentRuleException("Doctor is inactive.");
        }

        if (appointmentDate < DateTime.Today)
        {
            throw new AppointmentRuleException("Appointment date cannot be in the past.");
        }

        if (!TimeSlots.Slots.Contains(dto.TimeSlot))
        {
            throw new AppointmentRuleException("Invalid time slot selected.");
        }

        if (IsPastTimeSlot(appointmentDate, dto.TimeSlot))
        {
            throw new AppointmentRuleException("Selected time slot has already passed.");
        }

        var doctorLeave = await doctorLeaveRepository.GetLeaveForDateAsync(
            dto.DoctorId,
            appointmentDate);

        if (doctorLeave is not null)
        {
            logger.LogWarning(
                "Appointment booking blocked because doctor is on leave. PatientId: {PatientId}, DoctorId: {DoctorId}, AppointmentDate: {AppointmentDate}, DoctorLeaveId: {DoctorLeaveId}",
                patient.PatientId,
                dto.DoctorId,
                appointmentDate,
                doctorLeave.DoctorLeaveId);

            throw new AppointmentRuleException(DoctorOnLeaveBookingMessage);
        }

        if (await appointmentRepository.IsSlotBookedAsync(
                dto.DoctorId,
                appointmentDate,
                dto.TimeSlot))
        {
            throw new ConflictException("This time slot is already booked.");
        }

        if (await appointmentRepository.PatientHasActiveAppointmentOnDateAndSlotAsync(
                patient.PatientId,
                appointmentDate,
                dto.TimeSlot))
        {
            throw new ConflictException("Patient already has an appointment in this slot.");
        }

        if (await appointmentRepository.PatientHasActiveAppointmentWithDoctorOnDateAsync(
                patient.PatientId,
                dto.DoctorId,
                appointmentDate))
        {
            throw new ConflictException(
                "Patient already has an active appointment with this doctor on the selected date.");
        }

        var appointment = mapper.Map<Appointment>(dto);

        appointment.PatientId = patient.PatientId;
        appointment.DoctorId = dto.DoctorId;
        appointment.ScheduledDate = appointmentDate;
        appointment.TimeSlots = dto.TimeSlot;
        appointment.Status = AppointmentStatus.Pending.ToString();
        appointment.CancellationReason = null;
        appointment.CreatedDate = DateTime.Now;

        await using var transaction =
            await dbContext.Database.BeginTransactionAsync();

        Appointment savedAppointment;

        try
        {
            savedAppointment = await appointmentRepository.AddAsync(appointment);

            logger.LogInformation(
                "Appointment saved inside outbox transaction. AppointmentId: {AppointmentId}, PatientId: {PatientId}, DoctorId: {DoctorId}, ScheduledDate: {ScheduledDate}, TimeSlot: {TimeSlot}",
                savedAppointment.AppointmentId,
                savedAppointment.PatientId,
                savedAppointment.DoctorId,
                savedAppointment.ScheduledDate,
                savedAppointment.TimeSlots);

            var appointmentBookedEvent = new AppointmentBookedEvent
            {
                AppointmentId = savedAppointment.AppointmentId,
                PatientName = patient.PatientName,
                DoctorId = savedAppointment.DoctorId,
                ScheduledDate = savedAppointment.ScheduledDate,
                TimeSlot = savedAppointment.TimeSlots
            };

            var outboxMessage = new OutboxMessage
            {
                OutboxMessageId = Guid.NewGuid(),
                EventType = OutboxConstants.AppointmentBookedEventType,
                Payload = JsonSerializer.Serialize(appointmentBookedEvent),
                CreatedDate = DateTime.UtcNow,
                ProcessedDate = null,
                RetryCount = 0,
                LastAttemptDate = null,
                ErrorMessage = null
            };

            await dbContext.OutboxMessages.AddAsync(outboxMessage);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            logger.LogInformation(
                "Appointment and outbox message committed successfully. AppointmentId: {AppointmentId}, OutboxMessageId: {OutboxMessageId}, EventType: {EventType}",
                savedAppointment.AppointmentId,
                outboxMessage.OutboxMessageId,
                outboxMessage.EventType);
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();

            logger.LogError(
                exception,
                "Appointment booking transaction failed. The appointment and outbox message were rolled back. PatientId: {PatientId}, DoctorId: {DoctorId}, ScheduledDate: {ScheduledDate}, TimeSlot: {TimeSlot}",
                patient.PatientId,
                dto.DoctorId,
                appointmentDate,
                dto.TimeSlot);

            throw;
        }

        await TryInvalidateAvailabilityCacheAsync(
            savedAppointment.DoctorId,
            savedAppointment.ScheduledDate);

        var appointmentWithDetails = await appointmentRepository
            .GetByIdWithDetailsAsync(savedAppointment.AppointmentId);

        return mapper.Map<AppointmentDto>(appointmentWithDetails ?? savedAppointment);
    }

    private async Task<Patient> GetLoggedInPatientAsync()
    {
        var userId = CurrentUser?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenAccessException("Unable to identify logged-in user.");
        }

        var patient = await patientRepository.GetByUserIdAsync(userId);

        if (patient is null)
        {
            throw new EntityNotFoundException("Patient", userId);
        }

        return patient;
    }

    private async Task<Doctor> GetLoggedInDoctorAsync()
    {
        var userId = CurrentUser?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenAccessException("Unable to identify logged-in user.");
        }

        var doctor = await doctorRepository.GetByUserIdAsync(userId);

        if (doctor is null)
        {
            throw new EntityNotFoundException("Doctor", userId);
        }

        return doctor;
    }

    public async Task<AppointmentDto> ChangeAppointmentStatusAsync(
        int appointmentId,
        UpdateAppointmentStatusDto dto)
    {
        ValidateAppointmentId(appointmentId);

        if (dto is null)
        {
            throw new AppointmentRuleException("Appointment status details are required.");
        }

        var appointment = await appointmentRepository.GetByIdWithDetailsAsync(appointmentId)
            ?? throw new EntityNotFoundException("Appointment", appointmentId);

        await EnsureAppointmentAccessAsync(appointment);

        ValidateStatusChange(appointment, dto);

        appointment.Status = dto.Status.ToString();

        appointment.CancellationReason = dto.Status == AppointmentStatus.Cancelled
            ? dto.CancellationReason?.Trim()
            : null;

        var updated = await appointmentRepository.UpdateAsync(appointmentId, appointment)
            ?? throw new EntityNotFoundException("Appointment", appointmentId);

        if (dto.Status == AppointmentStatus.Cancelled)
        {
            await TryInvalidateAvailabilityCacheAsync(
                updated.DoctorId,
                updated.ScheduledDate);
        }

        logger.LogInformation(
            "Appointment status updated. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}, NewStatus: {Status}",
            updated.AppointmentId,
            updated.DoctorId,
            updated.Status);

        var updatedWithDetails = await appointmentRepository
            .GetByIdWithDetailsAsync(updated.AppointmentId);

        return mapper.Map<AppointmentDto>(updatedWithDetails ?? updated);
    }

    private void ValidateStatusChange(
        Appointment appointment,
        UpdateAppointmentStatusDto dto)
    {
        var currentStatus = Enum.Parse<AppointmentStatus>(appointment.Status);

        if (IsAdmin())
        {
            throw new ForbiddenAccessException(
                "Admins are not allowed to update appointment status.");
        }

        EnsureAppointmentCanBeUpdated(currentStatus);
        EnsureCancellationReasonIfRequired(dto);

        if (IsPatient())
        {
            ValidatePatientStatusChange(currentStatus, dto.Status);
            return;
        }

        if (IsDoctor())
        {
            ValidateDoctorStatusChange(currentStatus, dto.Status);
            return;
        }

        throw new ForbiddenAccessException(
            "You are not allowed to update appointment status.");
    }

    public async Task<AppointmentDto> CancelAppointmentAsync(
        int appointmentId,
        string? reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new AppointmentRuleException("Cancellation reason is required.");
        }

        return await ChangeAppointmentStatusAsync(
            appointmentId,
            new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Cancelled,
                CancellationReason = reason.Trim()
            });
    }

    private ClaimsPrincipal? CurrentUser =>
        httpContextAccessor.HttpContext?.User;

    private bool IsPatient()
    {
        return CurrentUser?.IsInRole(Roles.Patient) == true;
    }

    private bool IsDoctor()
    {
        return CurrentUser?.IsInRole(Roles.Doctor) == true;
    }

    private bool IsAdmin()
    {
        return CurrentUser?.IsInRole(Roles.Admin) == true;
    }

    private async Task EnsureAppointmentAccessAsync(Appointment appointment)
    {
        if (IsAdmin())
        {
            return;
        }

        if (IsPatient())
        {
            var loggedInPatient = await GetLoggedInPatientAsync();

            if (appointment.PatientId != loggedInPatient.PatientId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another patient's appointment.");
            }

            return;
        }

        if (IsDoctor())
        {
            var loggedInDoctor = await GetLoggedInDoctorAsync();

            if (appointment.DoctorId != loggedInDoctor.DoctorId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another doctor's appointment.");
            }

            return;
        }

        throw new ForbiddenAccessException("You are not allowed to access this appointment.");
    }

    private async Task TryInvalidateAvailabilityCacheAsync(
        int doctorId,
        DateTime date)
    {
        var cacheKey =
            $"doctors:{doctorId}:availability:{date:yyyy-MM-dd}";

        try
        {
            await distributedCache.RemoveAsync(cacheKey);

            logger.LogInformation(
                "Doctor availability cache invalidated after appointment change. DoctorId: {DoctorId}, Date: {Date}, CacheKey: {CacheKey}",
                doctorId,
                date.Date,
                cacheKey);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Appointment operation succeeded, but doctor availability cache invalidation failed. DoctorId: {DoctorId}, Date: {Date}, CacheKey: {CacheKey}",
                doctorId,
                date.Date,
                cacheKey);
        }
    }

    private static void ValidateAppointmentId(int id)
    {
        if (id <= 0)
        {
            throw new AppointmentRuleException(
                "Please provide a valid appointment reference.");
        }
    }

    private async Task ValidatePatientExistsAsync(int id)
    {
        if (id <= 0)
        {
            throw new AppointmentRuleException("Invalid patient.");
        }

        if (await patientRepository.GetByIdAsync(id) is null)
        {
            throw new EntityNotFoundException("Patient", id);
        }
    }

    private async Task<Doctor> ValidateDoctorExistsAsync(int id)
    {
        if (id <= 0)
        {
            throw new AppointmentRuleException("Invalid doctor.");
        }

        return await doctorRepository.GetByIdAsync(id)
            ?? throw new EntityNotFoundException("Doctor", id);
    }

    private static bool IsPastTimeSlot(DateTime scheduledDate, string timeSlot)
    {
        if (scheduledDate.Date != DateTime.Today)
        {
            return false;
        }

        var slotStartTime = GetSlotStartTime(timeSlot);
        var slotStartDateTime = scheduledDate.Date.Add(slotStartTime);

        return slotStartDateTime <= DateTime.Now;
    }

    private static TimeSpan GetSlotStartTime(string timeSlot)
    {
        var startText = timeSlot.Split('-')[0].Trim();

        if (!DateTime.TryParse(
                startText,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsedStartTime))
        {
            throw new AppointmentRuleException("Invalid time slot format.");
        }

        return parsedStartTime.TimeOfDay;
    }

    private static void EnsureAppointmentCanBeUpdated(AppointmentStatus currentStatus)
    {
        if (currentStatus == AppointmentStatus.Completed ||
            currentStatus == AppointmentStatus.Cancelled)
        {
            throw new AppointmentRuleException(
                "Completed or cancelled appointments cannot be updated.");
        }
    }

    private static void EnsureCancellationReasonIfRequired(
        UpdateAppointmentStatusDto dto)
    {
        if (dto.Status == AppointmentStatus.Cancelled &&
            string.IsNullOrWhiteSpace(dto.CancellationReason))
        {
            throw new AppointmentRuleException("Cancellation reason is required.");
        }
    }

    private static void ValidatePatientStatusChange(
        AppointmentStatus currentStatus,
        AppointmentStatus requestedStatus)
    {
        if (requestedStatus != AppointmentStatus.Cancelled)
        {
            throw new ForbiddenAccessException(
                "Patients are allowed only to cancel their own appointments.");
        }

        if (currentStatus != AppointmentStatus.Pending &&
            currentStatus != AppointmentStatus.Confirmed)
        {
            throw new AppointmentRuleException(
                "Only pending or confirmed appointments can be cancelled.");
        }
    }

    private static void ValidateDoctorStatusChange(
        AppointmentStatus currentStatus,
        AppointmentStatus requestedStatus)
    {
        if (requestedStatus == AppointmentStatus.Confirmed &&
            currentStatus == AppointmentStatus.Pending)
        {
            return;
        }

        if (requestedStatus == AppointmentStatus.Completed &&
            currentStatus == AppointmentStatus.Confirmed)
        {
            return;
        }

        if (requestedStatus == AppointmentStatus.Cancelled &&
            (currentStatus == AppointmentStatus.Pending ||
             currentStatus == AppointmentStatus.Confirmed))
        {
            return;
        }

        throw new AppointmentRuleException(InvalidStatusTransitionMessage);
    }
}
