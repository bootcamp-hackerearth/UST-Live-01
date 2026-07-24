using AutoMapper;
using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;

namespace HealthAxis.API.Services
{
    public class DoctorService
        : Service<Doctor, DoctorReadDto, DoctorCreateDto, DoctorUpdateDto>,
          IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            IMapper mapper)
            : base(doctorRepository, mapper)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<DoctorAvailabilityDto?> GetAvailabilityAsync(
            int doctorId,
            DateTime date,
            string timeSlot,
            CancellationToken ct = default)
        {
            Doctor? doctor =
                await _doctorRepository.GetByIdAsync(
                    doctorId,
                    ct);

            if (doctor == null)
            {
                return null;
            }

            if (!doctor.IsActive)
            {
                return new DoctorAvailabilityDto
                {
                    DoctorId = doctorId,
                    Date = date.Date,
                    TimeSlot = timeSlot,
                    IsAvailable = false
                };
            }

            bool slotBooked =
                await _appointmentRepository.IsSlotBookedAsync(
                    doctorId,
                    date.Date,
                    timeSlot,
                    ct);

            return new DoctorAvailabilityDto
            {
                DoctorId = doctorId,
                Date = date.Date,
                TimeSlot = timeSlot,
                IsAvailable = !slotBooked
            };
        }

        public async Task<DoctorReadDto?> UpdateActiveStatusAsync(
            int doctorId,
            bool isActive,
            CancellationToken ct = default)
        {
            Doctor? doctor =
                await _doctorRepository.GetByIdAsync(
                    doctorId,
                    ct);

            if (doctor == null)
            {
                return null;
            }

            doctor.IsActive = isActive;

            await _doctorRepository.SaveChangesAsync(ct);

            return _mapper.Map<DoctorReadDto>(doctor);
        }

        public async Task<List<string>> GetAvailableSlotsAsync(
            int doctorId,
            DateTime date,
            CancellationToken ct = default)
        {
            Doctor? doctor =
                await _doctorRepository.GetByIdAsync(
                    doctorId,
                    ct);

            if (doctor == null)
            {
                return new List<string>();
            }

            if (!doctor.IsActive)
            {
                return new List<string>();
            }

            if (date.Date < DateTime.Today)
            {
                return new List<string>();
            }

            if (date.Date > DateTime.Today.AddMonths(6))
            {
                return new List<string>();
            }

            List<string> allSlots = new()
            {
                "09:00-09:30",
                "09:30-10:00",
                "10:00-10:30",
                "10:30-11:00",
                "11:00-11:30",
                "11:30-12:00",
                "14:00-14:30",
                "14:30-15:00",
                "15:00-15:30",
                "15:30-16:00",
                "16:00-16:30",
                "16:30-17:00"
            };

            List<Appointment> appointments =
                await _appointmentRepository.GetAllAsync(ct);

            List<string> bookedSlots =
                appointments
                    .Where(appointment =>
                        appointment.DoctorId == doctorId &&
                        appointment.ScheduledDate.Date == date.Date &&
                        appointment.Status != AppointmentStatus.Cancelled)
                    .Select(appointment =>
                        appointment.TimeSlot)
                    .ToList();

            List<string> availableSlots =
                allSlots
                    .Where(slot =>
                        !bookedSlots.Contains(slot))
                    .ToList();

            if (date.Date == DateTime.Today)
            {
                availableSlots =
                    availableSlots
                        .Where(slot =>
                            !IsPastSlot(slot))
                        .ToList();
            }

            return availableSlots;
        }

        private static bool IsPastSlot(
            string timeSlot)
        {
            string startTime =
                timeSlot.Split('-')[0];

            if (!TimeSpan.TryParse(
                    startTime,
                    out TimeSpan slotStartTime))
            {
                return true;
            }

            TimeSpan currentTime =
                DateTime.Now.TimeOfDay;

            return slotStartTime <= currentTime;
        }
    }
}