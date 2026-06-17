using HealthApp.Api.Dto;

namespace HealthApp.Api.Service.Interface
{
    public interface IAppointmentService
    {
        Task<AppointmentDto> Add(AppointmentDto dto);

        Task<AppointmentDto> CancelAppointment(int appointmentId, string reason);
        Task<AppointmentDto> GetAppointmentById(int id);
        Task<AppointmentDto> CompleteAppointment(int appointmentId);

        Task<List<AppointmentDto>> GetAllAppointments();

        Task<List<AppointmentDto>> GetAppointmentsByPatient(int patientId);

        Task<List<string>> CheckDoctorAvailability(int doctorId, DateTime date);

        Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctor(int doctorId, DateTime fromDate, DateTime toDate);

        Task<AppointmentDto> ConfirmAppointment(int appointmentId);
    }
}
