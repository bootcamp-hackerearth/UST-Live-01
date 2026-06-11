using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Services.Interfaces
{
    public interface IAppointmentApiService
    {
        Task<IEnumerable<AppointmentDto>>
            GetAllAppointmentsAsync();

        Task<AppointmentDto>
            GetAppointmentByIdAsync(
                int id);

        Task<int>
            CreateAppointmentAsync(
                CreateAppointmentDto dto);

        Task
            UpdateAppointmentAsync(
                int id,
                UpdateAppointmentDto dto);

        Task
            DeleteAppointmentAsync(
                int id);

        //Task<IEnumerable<AppointmentDto>>
        //    GetAppointmentsByPatientAsync(
        //        int patientId);

        Task<AppointmentDto>
            GetNextAppointmentByPatientAsync(
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

        Task
            ConfirmAppointmentAsync(
                int appointmentId);

        Task
            CompleteAppointmentAsync(
                int appointmentId);

        Task
            CancelAppointmentAsync(
                int appointmentId,
                string reason);

        Task<IEnumerable<AppointmentDto>>
            GetAppointmentsByPatientAsync(
                int patientId);
    }
}