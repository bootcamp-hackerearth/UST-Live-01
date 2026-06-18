using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.Models;

namespace HealthAxis.API.Services
{
    public interface IAppointmentService
        : IService<Appointment, AppointmentReadDto, AppointmentCreateDto, AppointmentUpdateDto>
    {
        Task<AppointmentReadDto?> UpdateStatusAsync(
            int appointmentId,
            AppointmentStatusUpdateDto statusUpdateDto,
            CancellationToken ct = default);

        Task<AppointmentReportDto> GetAppointmentReportAsync(
            CancellationToken ct = default);
    }
}
