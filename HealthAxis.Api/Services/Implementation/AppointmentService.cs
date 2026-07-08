using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Extensions;
using HealthAxisCore_Api.Messaging.Contracts;
using HealthAxisCore_Api.Messaging.Publishers;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Interfaces;
using Serilog;
using System.Globalization;
using System.Security.Claims;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class AppointmentService(
        IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository,
        IMapper mapper,
        RabbitMQPublisher publisher
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

            var doctor = await doctorRepository.GetByIdAsync(request.DoctorId, ct)
                ?? throw new NotFoundException("Doctor not found");

            if (!doctor.IsActive)
            {
                throw new InvalidException("Cannot book appointment with inactive doctor");
            }

            if (request.ScheduledDate.Date < DateTime.UtcNow.Date)
            {
                throw new InvalidException("Cannot book past date");
            }

            EnsureWithinDoctorWorkingHours(request.TimeSlot);

            EnsureSameDaySlotIsInFuture(
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

            var savedAppointmentDetails =
                await appointmentRepository.GetDetailsAsync(
                    savedAppointment.AppointmentId,
                    ct) ?? savedAppointment;

            var appointmentDto = mapper.Map<AppointmentDto>(savedAppointmentDetails);

            Logger.Information(
                "Appointment booked successfully. AppointmentId: {AppointmentId}, PatientId: {PatientId}, DoctorId: {DoctorId}, ScheduledDate: {ScheduledDate}, TimeSlot: {TimeSlot}",
                savedAppointmentDetails.AppointmentId,
                savedAppointmentDetails.PatientId,
                savedAppointmentDetails.DoctorId,
                savedAppointmentDetails.ScheduledDate,
                savedAppointmentDetails.TimeSlot);

            await publisher.PublishAppointmentBookedAsync(
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
                "AppointmentBookedEvent publish requested. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}",
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

            if (user.IsPatient())
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

            if (user.IsDoctor())
            {
                var doctorId = user.GetDoctorId()
                    ?? throw new UnauthorizedException("DoctorId claim missing");

                if (appointment.DoctorId != doctorId)
                {
                    throw new UnauthorizedException(
                        "You can update only your own appointment");
                }

                var allowedDoctorStatuses = new[] { "Confirmed", "Completed" };

                if (!allowedDoctorStatuses.Contains(request.Status))
                {
                    throw new UnauthorizedException(
                        "Doctors can only confirm or complete appointments");
                }
            }

            if (request.Status == "Cancelled")
            {
                if (string.IsNullOrWhiteSpace(request.CancellationReason))
                {
                    throw new InvalidException("Cancellation reason is required");
                }

                EnsureCancellationAllowed(appointment);
            }

            if (appointment.Status == "Cancelled")
            {
                throw new InvalidException("Cancelled appointment cannot be updated");
            }

            if (appointment.Status == "Completed" && request.Status != "Completed")
            {
                throw new InvalidException("Completed appointment cannot be changed");
            }

            appointment.Status = request.Status;
            appointment.CancellationReason = request.CancellationReason ?? string.Empty;

            var updatedAppointment = await appointmentRepository.UpdateAsync(
                id,
                appointment,
                ct)
                ?? throw new NotFoundException("Appointment not found");

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
            var today = DateTime.UtcNow.Date;

            if (scheduledDate.Date != today)
            {
                return;
            }

            var selectedSlot = ParseTimeSlot(timeSlot);

            var currentTime = TimeOnly.FromDateTime(DateTime.UtcNow);

            if (selectedSlot <= currentTime)
            {
                throw new InvalidException(
                    "Cannot book a time slot that has already passed for today");
            }
        }

        private static void EnsureCancellationAllowed(Appointment appointment)
        {
            var appointmentDateTime = BuildAppointmentDateTime(
                appointment.ScheduledDate,
                appointment.TimeSlot);

            var cancellationCutOff = appointmentDateTime.AddHours(-2);

            if (DateTime.UtcNow >= cancellationCutOff)
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