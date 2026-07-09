using AutoMapper;
using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HealthAxis.API.Services
{
    public class DoctorService
        : Service<Doctor, DoctorReadDto, DoctorCreateDto, DoctorUpdateDto>,
          IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            IMapper mapper,
            IDistributedCache cache)
            : base(doctorRepository, mapper)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
            _cache = cache;
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
            string cacheKey =
                $"doctors:{doctorId}:availability:{date:yyyy-MM-dd}";

            string? cachedSlots =
                await _cache.GetStringAsync(
                    cacheKey,
                    ct);

            if (!string.IsNullOrWhiteSpace(cachedSlots))
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine();
                Console.WriteLine("====================================");
                Console.WriteLine("CACHE HIT - DOCTOR AVAILABILITY");
                Console.WriteLine("====================================");
                Console.WriteLine($"Doctor Id : {doctorId}");
                Console.WriteLine($"Date      : {date:yyyy-MM-dd}");
                Console.WriteLine($"Key       : {cacheKey}");
                Console.WriteLine("Source    : Garnet Cache");
                Console.WriteLine("====================================");
                Console.WriteLine();

                Console.ResetColor();

                return JsonSerializer.Deserialize<List<string>>(
                           cachedSlots)
                       ?? new List<string>();
            }

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine();
            Console.WriteLine("====================================");
            Console.WriteLine("CACHE MISS - LOADING FROM DATABASE");
            Console.WriteLine("====================================");
            Console.WriteLine($"Doctor Id : {doctorId}");
            Console.WriteLine($"Date      : {date:yyyy-MM-dd}");
            Console.WriteLine($"Key       : {cacheKey}");
            Console.WriteLine("Source    : SQL Server");
            Console.WriteLine("====================================");
            Console.WriteLine();

            Console.ResetColor();

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

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(
                    availableSlots),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(5)
                },
                ct);

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine();
            Console.WriteLine("====================================");
            Console.WriteLine("CACHE STORED IN GARNET");
            Console.WriteLine("====================================");
            Console.WriteLine($"Doctor Id : {doctorId}");
            Console.WriteLine($"Date      : {date:yyyy-MM-dd}");
            Console.WriteLine($"Key       : {cacheKey}");
            Console.WriteLine($"TTL       : 5 Minutes");
            Console.WriteLine($"Slots     : {availableSlots.Count}");
            Console.WriteLine("====================================");
            Console.WriteLine();

            Console.ResetColor();

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