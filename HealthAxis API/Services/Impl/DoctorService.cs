using AutoMapper;
using HealthAxis.API.DTOs.Doctors;
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

        public DoctorService(
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            IMapper mapper)
            : base(doctorRepository, mapper)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<DoctorAvailabilityDto?> GetAvailabilityAsync(
            int doctorId,
            DateTime date,
            string timeSlot,
            CancellationToken ct = default)
        {
            Doctor? doctor = await _doctorRepository.GetByIdAsync(doctorId, ct);

            if (doctor == null)
            {
                return null;
            }

            bool slotBooked = await _appointmentRepository.IsSlotBookedAsync(
                doctorId,
                date,
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
    }
}
