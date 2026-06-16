using AutoMapper;
using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;

namespace HealthAxis.API.Services
{
    public class AppointmentService
        : Service<Appointment, AppointmentReadDto, AppointmentCreateDto, AppointmentUpdateDto>, IAppointmentService
    {
        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IMapper mapper)
            : base(appointmentRepository, mapper)
        {
        }
    }
}
