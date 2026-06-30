using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.Models;

namespace HealthAxis.API.Services
{
    public interface IAppointmentService
        : IService<Appointment, AppointmentReadDto, AppointmentCreateDto, AppointmentUpdateDto>
    {
        Task<List<AppointmentReadDto>> GetAllWithDetailsAsync(
            CancellationToken ct = default);

        Task<List<AppointmentReadDto>> GetAppointmentsByPatientIdAsync(
            int patientId,
            CancellationToken ct = default);

        Task<List<AppointmentReadDto>> GetAppointmentsByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default);

        Task<AppointmentReadDto?> UpdateStatusAsync(
            int appointmentId,
            AppointmentStatusUpdateDto statusUpdateDto,
            CancellationToken ct = default);

        Task<List<AppointmentReportDto>> GetAppointmentReportAsync(
            CancellationToken ct = default);
    }
}
