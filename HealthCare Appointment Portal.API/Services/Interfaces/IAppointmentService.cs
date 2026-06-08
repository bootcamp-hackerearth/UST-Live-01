using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>>
            GetAllAppointmentsAsync();

        Task<AppointmentDto>
            GetAppointmentByIdAsync(
                int appointmentId);

        Task<int>
            AddAppointmentAsync(
                CreateAppointmentDto appointmentDto);

        Task UpdateAppointmentAsync(
            int appointmentId,
            UpdateAppointmentDto appointmentDto);

        Task DeleteAppointmentAsync(
            int appointmentId);

        Task ConfirmAppointmentAsync(
            int appointmentId);

        Task CancelAppointmentAsync(
            int appointmentId,
            string reason);

        Task CompleteAppointmentAsync(
            int appointmentId);

        Task<IEnumerable<AppointmentDto>>
            GetAppointmentsByPatientAsync(
                int patientId);

        Task<IEnumerable<AppointmentDto>>
            GetAppointmentsByDoctorAsync(
                int doctorId);

        Task<IEnumerable<AppointmentDto>>
            GetTodayScheduleAsync(
                int doctorId);

        Task<IEnumerable<AppointmentDto>>
            GetWeeklyScheduleAsync(
                int doctorId);

        Task<AppointmentDto>
            GetNextAppointmentByPatientAsync(
                int patientId);
    }
}