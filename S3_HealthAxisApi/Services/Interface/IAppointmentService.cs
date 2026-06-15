using S3_HealthAxisApi.Models;

namespace S3_HealthAxisApi.Services.Interface
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();

        Task<Appointment?> GetAppointmentByIdAsync(int id);

        Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId);

        Task<IEnumerable<Appointment>> GetDoctorTodayScheduleAsync(int doctorId);

        Task<IEnumerable<Appointment>> GetDoctorWeekScheduleAsync(int doctorId);

        Task BookAppointmentAsync(Appointment appointment);

        Task CancelAppointmentAsync(int appointmentId, string reason);

        Task CompleteAppointmentAsync(int appointmentId);

        Task UpdateAppointmentAsync(int appointmentId, Appointment appointment);
    }
}
