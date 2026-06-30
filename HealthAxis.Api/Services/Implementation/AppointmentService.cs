using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Extensions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Interfaces;
using System.Globalization;
using System.Security.Claims;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class AppointmentService(
        IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository,
        IMapper mapper
    ) : IAppointmentService
    {
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

            var appt = mapper.Map<Appointment>(request);

            appt.PatientId = patientId;
            appt.Status = "Pending";
            appt.CancellationReason = string.Empty;

            var saved = await appointmentRepository.CreateAsync(
                appt,
                ct);

            return mapper.Map<AppointmentDto>(
                await appointmentRepository.GetDetailsAsync(
                    saved.AppointmentId,
                    ct) ?? saved);
        }

        public async Task<AppointmentDto> UpdateStatusAsync(
            int id,
            UpdateAppointmentStatusDto request,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            var appt = await appointmentRepository.GetDetailsAsync(id, ct)
                ?? throw new NotFoundException("Appointment not found");

            if (user.IsPatient())
            {
                var patientId = user.GetPatientId()
                    ?? throw new UnauthorizedException("PatientId claim missing");

                if (appt.PatientId != patientId)
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

                if (appt.DoctorId != doctorId)
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

                EnsureCancellationAllowed(appt);
            }
            if (appt.Status == "Cancelled")
            {
                throw new InvalidException("Cancelled appointment cannot be updated");
            }

            if (appt.Status == "Completed" && request.Status != "Completed")
            {
                throw new InvalidException("Completed appointment cannot be changed");
            }

            appt.Status = request.Status;
            appt.CancellationReason = request.CancellationReason ?? string.Empty;

            var updated = await appointmentRepository.UpdateAsync(
                id,
                appt,
                ct)
                ?? throw new NotFoundException("Appointment not found");

            return mapper.Map<AppointmentDto>(
                await appointmentRepository.GetDetailsAsync(
                    updated.AppointmentId,
                    ct) ?? updated);
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