using HealthApp.ConsoleApp.Models;
namespace HealthApp.ConsoleApp.Interfaces
{
    public interface IAppointmentRepository
    {
        string AddAppointment(Appointment appointment);
        List<Appointment> GetAllAppointments();
        Appointment? GetAppointmentById(int id);
        Appointment UpdateAppointment(Appointment existingAppointment, Appointment appointment);
        List<Appointment> GetAppointmentsByDoctorId(int doctorId);
        List<Appointment> GetAppointmentsByPatientId(int patientId);
    }
}