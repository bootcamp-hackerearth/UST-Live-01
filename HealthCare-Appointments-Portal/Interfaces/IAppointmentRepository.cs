using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Interfaces
{

    public interface IAppointmentRepository
    {

        void AddAppointment(Appointment appointment);

        Appointment? GetAppointmentById(int appointmentId);

        List<Appointment> GetAllAppointments();

        void UpdateAppointment(Appointment updatedAppointment);

        void DeleteAppointmentById(int appointmentId);

        List<Appointment> GetAppointmentsByDoctorId(int doctorId);
        List<Appointment> GetAppointmentsByPatientId(int patientId);

        Appointment? GetConflictingAppointment(
            int doctorId,
            DateOnly date,
            TimeOnly slot);

        List<Appointment> GetUpcomingAppointments();

        List<Appointment> GetCompletedAppointments();
    }
}