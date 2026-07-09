using AutoMapper;
using HealthAxis.API.Events;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Messaging;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.AppointmentDtos;
using Microsoft.Extensions.Caching.Distributed;
using HealthAxis.Shared.Enums;
using System.Globalization;

namespace HealthAxis.API.Services.Implementation
{
    public class AppointmentService(
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        IMapper mapper,
        ILogger<AppointmentService> logger,
        IEventPublisher eventPublisher, IDistributedCache distributedCache) : IAppointmentService
    {
        private static readonly HashSet<string> AllowedTimeSlots =
            new(StringComparer.OrdinalIgnoreCase)
            {
                "09:00 AM - 10:00 AM",
                "10:00 AM - 11:00 AM",
                "11:00 AM - 12:00 PM",
                "12:00 PM - 01:00 PM",
                "02:00 PM - 03:00 PM",
                "03:00 PM - 04:00 PM",
                "04:00 PM - 05:00 PM",
                "05:00 PM - 06:00 PM",
                "06:00 PM - 07:00 PM",
                "07:00 PM - 08:00 PM",
                "08:00 PM - 09:00 PM",
                "09:00 PM - 10:00 PM"
            };

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            var appointments = await appointmentRepository.GetAllAsync();

            return await MapAppointmentListAsync(appointments);
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointment = await appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                return null;
            }

            return await MapAppointmentAsync(appointment);
        }

        public async Task<List<AppointmentDto>> GetByPatientIdAsync(int patientId)
        {
            var patient = await patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found.");
            }

            var appointments = await appointmentRepository.GetAllAsync();

            var patientAppointments = appointments
                .Where(appointment => appointment.PatientId == patientId)
                .OrderByDescending(appointment => appointment.ScheduledDate)
                .ThenBy(appointment => appointment.TimeSlot)
                .ToList();

            return await MapAppointmentListAsync(patientAppointments);
        }

        public async Task<List<AppointmentDto>> GetByDoctorIdAsync(int doctorId)
        {
            var doctor = await doctorRepository.GetByIdAsync(doctorId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found.");
            }

            var appointments = await appointmentRepository.GetAllAsync();

            var doctorAppointments = appointments
                .Where(appointment => appointment.DoctorId == doctorId)
                .OrderBy(appointment => appointment.ScheduledDate)
                .ThenBy(appointment => appointment.TimeSlot)
                .ToList();

            return await MapAppointmentListAsync(doctorAppointments);
        }

        public async Task<AppointmentDto> AddAsync(
            CreateAppointmentDto appointmentDto)
        {
            ArgumentNullException.ThrowIfNull(appointmentDto);

            ValidateAppointmentRequest(appointmentDto);

            var patient = await patientRepository.GetByIdAsync(
                appointmentDto.PatientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found.");
            }

            var doctor = await doctorRepository.GetByIdAsync(
                appointmentDto.DoctorId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found.");
            }

            if (!doctor.IsActive)
            {
                throw new BusinessRuleException(
                    "Selected doctor is inactive. Please choose another doctor.");
            }

            await ValidateAppointmentConflictAsync(appointmentDto);

            var appointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                DoctorId = appointmentDto.DoctorId,
                ScheduledDate = appointmentDto.ScheduledDate.Date,
                TimeSlot = NormalizeTimeSlot(appointmentDto.TimeSlot),
                Status = AppointmentStatus.Pending,
                CancellationReason = null
            };

            var savedAppointment = await appointmentRepository.AddAsync(
                appointment);
            await InvalidateDoctorAvailabilityCacheAsync(savedAppointment);

            var appointmentBookedEvent = CreateAppointmentBookedEvent(
                savedAppointment,
                patient.FullName,
                doctor.FullName);

            LogAppointmentBookedEvent(appointmentBookedEvent);

            await eventPublisher.PublishAppointmentBookedAsync(
                appointmentBookedEvent);

            return await MapAppointmentAsync(savedAppointment);
        }

        public async Task<AppointmentDto> UpdateStatusAsync(
            int id,
            UpdateAppointmentStatusDto statusDto)
        {
            ArgumentNullException.ThrowIfNull(statusDto);

            var appointment = await appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found.");
            }

            ValidateAppointmentStatus(statusDto.Status);

            ValidateStatusTransition(
                appointment.Status,
                statusDto.Status);

            appointment.Status = statusDto.Status;

            if (statusDto.Status == AppointmentStatus.Cancelled)
            {
                appointment.CancellationReason =
                    string.IsNullOrWhiteSpace(statusDto.CancellationReason)
                        ? null
                        : statusDto.CancellationReason.Trim();
            }
            else
            {
                appointment.CancellationReason = null;
            }

            var updatedAppointment = await appointmentRepository.UpdateAsync(
                id,
                appointment);

            if (updatedAppointment == null)
            {
                throw new NotFoundException("Appointment not found.");
            }

            return await MapAppointmentAsync(updatedAppointment);
        }

        public async Task<AppointmentDto?> DeleteAsync(int id)
        {
            var appointment = await appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.Status != AppointmentStatus.Pending)
            {
                throw new BusinessRuleException(
                    "Only pending appointments can be deleted.");
            }

            var deletedAppointment = await appointmentRepository.DeleteAsync(id);

            if (deletedAppointment == null)
            {
                return null;
            }

            return await MapAppointmentAsync(deletedAppointment);
        }

        private static AppointmentBookedEvent CreateAppointmentBookedEvent(
            Appointment appointment,
            string patientName,
            string doctorName)
        {
            return new AppointmentBookedEvent
            {
                EventType = "AppointmentBooked",
                PatientId = appointment.PatientId,
                PatientName = patientName,
                DoctorName = doctorName,
                DoctorId = appointment.DoctorId,
                AppointmentId = appointment.AppointmentId,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.TimeSlot,
                Status = appointment.Status.ToString(),
                OccurredAt = DateTime.UtcNow
            };
        }

        private async Task InvalidateDoctorAvailabilityCacheAsync(
    Appointment appointment)
        {
            var cacheKey =
                $"doctors:{appointment.DoctorId}:availability:{appointment.ScheduledDate:yyyy-MM-dd}";

            await distributedCache.RemoveAsync(cacheKey);

            logger.LogInformation(
                "Doctor availability Garnet cache invalidated after appointment booking. DoctorId: {DoctorId}, Date: {Date}, CacheKey: {CacheKey}",
                appointment.DoctorId,
                appointment.ScheduledDate.Date,
                cacheKey);
        }

        private void LogAppointmentBookedEvent(
            AppointmentBookedEvent appointmentBookedEvent)
        {
            logger.LogInformation(
                """
                ┌──────────────────────────────────────────────────────────────┐
                │                    HEALTHAXIS EVENT LOG                      │
                ├──────────────────────────────────────────────────────────────┤
                │ Event Type      : {EventType}
                │ Patient Name    : {PatientName}
                │ Doctor Name     : {DoctorName}
                │ Doctor ID       : {DoctorId}
                │ Appointment ID  : {AppointmentId}
                │ Scheduled Date  : {ScheduledDate:yyyy-MM-dd}
                │ Time Slot       : {TimeSlot}
                │ Status          : {Status}
                └──────────────────────────────────────────────────────────────┘
                """,
                appointmentBookedEvent.EventType,
                appointmentBookedEvent.PatientName,
                appointmentBookedEvent.DoctorName,
                appointmentBookedEvent.DoctorId,
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.ScheduledDate,
                appointmentBookedEvent.TimeSlot,
                appointmentBookedEvent.Status);
        }

        private async Task<List<AppointmentDto>> MapAppointmentListAsync(
            IEnumerable<Appointment> appointments)
        {
            var appointmentList = appointments.ToList();

            var patients = (await patientRepository.GetAllAsync())
                .ToDictionary(patient => patient.PatientId);

            var doctors = (await doctorRepository.GetAllAsync())
                .ToDictionary(doctor => doctor.DoctorId);

            return appointmentList
                .Select(appointment =>
                    MapAppointment(
                        appointment,
                        patients,
                        doctors))
                .ToList();
        }

        private async Task<AppointmentDto> MapAppointmentAsync(
            Appointment appointment)
        {
            var patients = (await patientRepository.GetAllAsync())
                .ToDictionary(patient => patient.PatientId);

            var doctors = (await doctorRepository.GetAllAsync())
                .ToDictionary(doctor => doctor.DoctorId);

            return MapAppointment(
                appointment,
                patients,
                doctors);
        }

        private AppointmentDto MapAppointment(
            Appointment appointment,
            IReadOnlyDictionary<int, Patient> patients,
            IReadOnlyDictionary<int, Doctor> doctors)
        {
            var appointmentDto = mapper.Map<AppointmentDto>(appointment);

            if (patients.TryGetValue(appointment.PatientId, out var patient))
            {
                appointmentDto.PatientName = patient.FullName;
            }
            else
            {
                appointmentDto.PatientName = "Not assigned";
            }

            if (doctors.TryGetValue(appointment.DoctorId, out var doctor))
            {
                appointmentDto.DoctorName = doctor.FullName;
                appointmentDto.Specialisation = doctor.Specialisation;
            }
            else
            {
                appointmentDto.DoctorName = "Not assigned";
            }

            return appointmentDto;
        }

        private static void ValidateAppointmentRequest(
            CreateAppointmentDto appointmentDto)
        {
            if (appointmentDto.PatientId <= 0)
            {
                throw new ValidationExceptions("Valid patient id is required.");
            }

            if (appointmentDto.DoctorId <= 0)
            {
                throw new ValidationExceptions("Please select a valid doctor.");
            }

            var appointmentDate = appointmentDto.ScheduledDate.Date;
            var today = DateTime.Today;
            var maxAllowedDate = today.AddMonths(6);

            if (appointmentDate < today)
            {
                throw new ValidationExceptions(
                    "Appointment date cannot be in the past.");
            }

            if (appointmentDate > maxAllowedDate)
            {
                throw new ValidationExceptions(
                    "Appointment date cannot be more than 6 months ahead.");
            }

            if (string.IsNullOrWhiteSpace(appointmentDto.TimeSlot))
            {
                throw new ValidationExceptions("Time slot is required.");
            }

            var timeSlot = NormalizeTimeSlot(appointmentDto.TimeSlot);

            if (!AllowedTimeSlots.Contains(timeSlot))
            {
                throw new ValidationExceptions(
                    "Invalid time slot selected. Please choose a valid hospital time slot.");
            }

            if (appointmentDate == today && IsPastTimeSlot(timeSlot))
            {
                throw new ValidationExceptions(
                    "Past time slot cannot be booked.");
            }
        }

        private async Task ValidateAppointmentConflictAsync(
            CreateAppointmentDto appointmentDto)
        {
            var appointments = await appointmentRepository.GetAllAsync();

            var appointmentDate = appointmentDto.ScheduledDate.Date;
            var requestedTimeSlot = NormalizeTimeSlot(appointmentDto.TimeSlot);

            var doctorAlreadyBooked = appointments.Any(appointment =>
                appointment.DoctorId == appointmentDto.DoctorId &&
                appointment.ScheduledDate.Date == appointmentDate &&
                IsSameTimeSlot(appointment.TimeSlot, requestedTimeSlot) &&
                IsActiveAppointmentStatus(appointment.Status));

            if (doctorAlreadyBooked)
            {
                throw new BusinessRuleException(
                    "This doctor already has an appointment for the selected date and time slot. Please choose another slot.");
            }

            var patientAlreadyBooked = appointments.Any(appointment =>
                appointment.PatientId == appointmentDto.PatientId &&
                appointment.ScheduledDate.Date == appointmentDate &&
                IsSameTimeSlot(appointment.TimeSlot, requestedTimeSlot) &&
                IsActiveAppointmentStatus(appointment.Status));

            if (patientAlreadyBooked)
            {
                throw new BusinessRuleException(
                    "You already have an appointment at this date and time slot.");
            }
        }

        private static bool IsPastTimeSlot(string timeSlot)
        {
            var startTimeText = timeSlot.Split('-')[0].Trim();

            var parsed = DateTime.TryParseExact(
                startTimeText,
                "hh:mm tt",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var startTime);

            if (!parsed)
            {
                return true;
            }

            var slotStartTime = DateTime.Today.Add(startTime.TimeOfDay);

            return slotStartTime <= DateTime.Now;
        }

        private static void ValidateAppointmentStatus(
            AppointmentStatus status)
        {
            if (!Enum.IsDefined(typeof(AppointmentStatus), status))
            {
                throw new ValidationExceptions("Invalid appointment status.");
            }
        }

        private static void ValidateStatusTransition(
            AppointmentStatus currentStatus,
            AppointmentStatus newStatus)
        {
            if (currentStatus == AppointmentStatus.Completed ||
                currentStatus == AppointmentStatus.Cancelled)
            {
                throw new BusinessRuleException(
                    "Completed or cancelled appointment cannot be changed.");
            }

            if (currentStatus == AppointmentStatus.Pending)
            {
                ValidatePendingStatusTransition(newStatus);
                return;
            }

            if (currentStatus == AppointmentStatus.Confirmed)
            {
                ValidateConfirmedStatusTransition(newStatus);
            }
        }

        private static void ValidatePendingStatusTransition(
            AppointmentStatus newStatus)
        {
            if (newStatus != AppointmentStatus.Confirmed &&
                newStatus != AppointmentStatus.Cancelled)
            {
                throw new BusinessRuleException(
                    "Pending appointment can only be confirmed or cancelled.");
            }
        }

        private static void ValidateConfirmedStatusTransition(
            AppointmentStatus newStatus)
        {
            if (newStatus != AppointmentStatus.Completed &&
                newStatus != AppointmentStatus.Cancelled)
            {
                throw new BusinessRuleException(
                    "Confirmed appointment can only be completed or cancelled.");
            }
        }

        private static bool IsActiveAppointmentStatus(
            AppointmentStatus status)
        {
            return status == AppointmentStatus.Pending ||
                   status == AppointmentStatus.Confirmed;
        }

        private static bool IsSameTimeSlot(
            string existingTimeSlot,
            string requestedTimeSlot)
        {
            return string.Equals(
                NormalizeTimeSlot(existingTimeSlot),
                requestedTimeSlot,
                StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeTimeSlot(string timeSlot)
        {
            return timeSlot.Trim();
        }
    }
}