using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.Models;

namespace HealthAxis.API.Services
{
    public interface IAppointmentService
        : IService<Appointment, AppointmentReadDto, AppointmentCreateDto, AppointmentUpdateDto>
    {
    }
}
