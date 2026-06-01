using System;
using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Interfaces
{   
public interface IAppointmentService
{
    string BookAppointment(Patient patient, Doctor doctor, DateTime date, string slot);
    string CancelAppointment(int appointmentId, string reason);
    List<Appointment> GetAppointmentsByPatientId(int patientId);
    List<Appointment> GetAppointmentsByDoctorId(int doctorId);
    Appointment GetAppointmentById(int appointmentId);
    List<Appointment> GetUpcomingAppointments();
    Appointment UpdateAppointment(Appointment appointment);
    List<Appointment> GetAllAppointments();
}
}