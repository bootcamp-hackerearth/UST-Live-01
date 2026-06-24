
using HealthApp.Shared.Dto;

public interface IAppointmentService
{
    Task<List<AppointmentDto>> GetAllAppointmentsAsync();
    Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
    Task<List<string>> CheckDoctorAvailabilityAsync(int doctorId, DateTime date);
    Task<List<AppointmentDto>> GetUpcomingAppointmentsAsync(int doctorId, DateTime fromDate, DateTime toDate);
    Task<List<AppointmentDto>> GetByPatientDoctorAsync(int patientId, int doctorId);

    Task<int> GetAppointmentCountAsync();
}
