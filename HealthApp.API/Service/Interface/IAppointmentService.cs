using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;

namespace HealthApp.API.Service.Interface;

public interface IAppointmentService
{
    Task<List<AppointmentDto>> GetAppointmentsAsync(
        int? patientId = null,
        int? doctorId = null);

    Task<List<AppointmentDto>> GetAllAppointmentsAsync();

    Task<AppointmentDto> GetAppointmentByIdAsync(int appointmentId);

    Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId);

    Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId);

    Task<List<AppointmentDto>> GetAppointmentsByStatusAsync(
        AppointmentStatus status);

    Task<AppointmentDto> BookAppointmentAsync(BookAppointmentDto dto);

    Task<AppointmentDto> ChangeAppointmentStatusAsync(
        int appointmentId,
        UpdateAppointmentStatusDto dto);

    Task<AppointmentDto> CancelAppointmentAsync(
        int appointmentId,
        string? reason);
}