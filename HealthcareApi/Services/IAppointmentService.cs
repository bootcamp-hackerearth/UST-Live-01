using SharedClasses.Dtos;
using SharedClasses.Enums;
using System.Collections.Generic;

namespace HealthcareApi.Services
{
    public interface IAppointmentService
    {
        List<AppointmentDto> GetAllAppointments();

        AppointmentDto GetAppointmentById(int appointmentId);

        List<AppointmentDto> GetAppointmentsByPatient(int patientId);

        List<AppointmentDto> GetAppointmentsByDoctor(int doctorId);

        List<AppointmentDto> GetUpcomingAppointmentsByPatient(int patientId);

        List<AppointmentDto> GetUpcomingAppointmentsByDoctor(int doctorId);

        List<AppointmentDto> GetCancelledAppointmentsByPatient(int patientId);

        AppointmentDto BookAppointment(BookAppointmentDto dto);

        AppointmentDto UpdateAppointment(int appointmentId, UpdateAppointmentDto dto);

        AppointmentDto DeleteAppointment(int appointmentId);

        AppointmentDto ConfirmAppointment(int appointmentId, ConfirmAppointmentDto dto);

        AppointmentDto CancelAppointmentByPatient(int appointmentId, CancelByPatientDto dto);

        AppointmentDto CancelAppointmentByDoctor(int appointmentId, CancelByDoctorDto dto);

        AppointmentDto CompleteAppointment(int appointmentId, CompleteAppointmentDto dto);

        List<AppointmentDto> GetCancelledAppointmentsByDoctor(int doctorId);

        List<AppointmentDto> SearchAppointments(string query);
        List<AppointmentDto> SearchCancelledAppointmentsByPatient(
            int patientId,
            string query);

        List<AppointmentDto> SearchCancelledAppointmentsByDoctor(
            int doctorId,
            string query);

        List<AppointmentDto> SearchUpcomingAppointmentsByDoctor(
            int doctorId,
            string query,
            AppointmentStatus? status);
    }
}