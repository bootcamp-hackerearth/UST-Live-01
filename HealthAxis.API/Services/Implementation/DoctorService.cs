using AutoMapper;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.DoctorDtos;
using HealthAxis.Shared.Enums;

namespace HealthAxis.API.Services.Implementation
{
    public class DoctorService(
        IDoctorRepository doctorRepository,
        IAppointmentRepository appointmentRepository,
        IMapper mapper,
        ILogger<DoctorService> logger) : IDoctorService
    {
        private static readonly string[] HospitalTimeSlots =
        [
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
        ];

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            var doctors = await doctorRepository.GetAllAsync();

            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            var doctor = await doctorRepository.GetByIdAsync(id);

            if (doctor is null)
            {
                throw new NotFoundException("Doctor not found");
            }

            return mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto?> GetByUserIdAsync(string userId)
        {
            var doctors = await doctorRepository.GetAllAsync();

            var doctor = doctors.FirstOrDefault(
                item => item.UserId == userId);

            if (doctor is null)
            {
                throw new NotFoundException(
                    "Doctor profile not found");
            }

            return mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorAvailabilityDto> GetAvailabilityAsync(
            int id,
            DateTime? date = null)
        {
            if (id <= 0)
            {
                throw new ValidationException(
                    "Doctor id must be greater than zero.");
            }

            var availabilityDate =
                (date ?? DateTime.Today).Date;

            if (availabilityDate < DateTime.Today)
            {
                throw new ValidationException(
                    "Cannot check doctor availability for a past date.");
            }

            var doctor = await doctorRepository.GetByIdAsync(id);

            if (doctor is null)
            {
                throw new NotFoundException("Doctor not found");
            }

            var availableSlots = await GetAvailableSlotsAsync(
                id,
                availabilityDate,
                doctor.IsActive);

            LogAvailabilityLoaded(
                doctor.DoctorId,
                availabilityDate,
                availableSlots.Count);

            return new DoctorAvailabilityDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                IsActive = doctor.IsActive,
                Date = availabilityDate,
                Message = doctor.IsActive
                    ? "Doctor is available"
                    : "Doctor is not available",
                AvailableSlots = availableSlots
            };
        }

        private async Task<List<string>> GetAvailableSlotsAsync(
            int doctorId,
            DateTime availabilityDate,
            bool isDoctorActive)
        {
            if (!isDoctorActive)
            {
                return [];
            }

            var appointments =
                await appointmentRepository.GetAllAsync();

            var bookedSlots = appointments
                .Where(appointment =>
                    appointment.DoctorId == doctorId &&
                    appointment.ScheduledDate.Date == availabilityDate &&
                    IsActiveAppointmentStatus(appointment.Status))
                .Select(appointment =>
                    NormalizeTimeSlot(appointment.TimeSlot))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return HospitalTimeSlots
                .Where(slot =>
                    !bookedSlots.Contains(NormalizeTimeSlot(slot)))
                .ToList();
        }

        private static bool IsActiveAppointmentStatus(
            AppointmentStatus status)
        {
            return status is
                AppointmentStatus.Pending or
                AppointmentStatus.Confirmed;
        }

        private static string NormalizeTimeSlot(string timeSlot)
        {
            return timeSlot.Trim();
        }

        private void LogAvailabilityLoaded(
            int doctorId,
            DateTime date,
            int availableSlotCount)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "Doctor availability loaded from database. Doctor Id: {DoctorId}, Date: {Date:yyyy-MM-dd}, Available Slots: {AvailableSlotCount}",
                doctorId,
                date,
                availableSlotCount);
        }
    }
}