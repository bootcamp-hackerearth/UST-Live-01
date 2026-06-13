using SharedClasses.Dtos;
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

        List<AppointmentDto> GetCancelledAppointmentsByDoctor(int doctorId);

        AppointmentDto BookAppointment(BookAppointmentDto dto);

        AppointmentDto UpdateAppointment(int appointmentId, UpdateAppointmentDto dto);

        AppointmentDto DeleteAppointment(int appointmentId);

        AppointmentDto ConfirmAppointment(int appointmentId, ConfirmAppointmentDto dto);

        AppointmentDto CancelAppointmentByPatient(int appointmentId, CancelByPatientDto dto);

        AppointmentDto CancelAppointmentByDoctor(int appointmentId, CancelByDoctorDto dto);

        AppointmentDto CompleteAppointment(int appointmentId, CompleteAppointmentDto dto);
    }
}