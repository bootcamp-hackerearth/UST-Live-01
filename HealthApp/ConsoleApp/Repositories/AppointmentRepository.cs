using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Exceptions;
using HealthApp.ConsoleApp.Databases;
namespace HealthApp.ConsoleApp.Repositories
{
    // Repository class to manage appointments in the healthcare system
    public class AppointmentRepository : IAppointmentRepository
    {
        // Constructor to initialize the appointment database
        private readonly AppointmentDb _appointmentDb;
        public AppointmentRepository(AppointmentDb appointmentDb)
        {
            _appointmentDb = appointmentDb;
        }
        // Method to add a new appointment to the database
        public string AddAppointment(Appointment appointment)
        {
            _appointmentDb.Appointments.Add(appointment);
            return $"Appointment of ID {appointment.AppointmentId} has been created successfully";
        }
        // Method to Get All appointment from the database
        public List<Appointment> GetAllAppointments()
        {
            return _appointmentDb.Appointments;
        }
        // Method to Get appointment by ID from the database
        public Appointment? GetAppointmentById(int id)
        {
            return _appointmentDb.Appointments.FirstOrDefault(a => a.AppointmentId == id);
        }
        // Method to update an existing appointment in the database
        public Appointment UpdateAppointment(Appointment existingAppointment, Appointment appointment)
        {
            existingAppointment.Patient = appointment.Patient;
            existingAppointment.Doctor = appointment.Doctor;
            existingAppointment.ScheduledDate = appointment.ScheduledDate;
            existingAppointment.TimeSlot = appointment.TimeSlot;
            existingAppointment.CancellationReason=appointment.CancellationReason;

            return existingAppointment;
        }
        // Method to get appointments by patient ID from the database
        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            return _appointmentDb.Appointments.Where(a => a.Patient?.PatientId == patientId).ToList();
        }
        // Method to get appointments by doctor ID from the database
        public List<Appointment> GetAppointmentsByDoctorId(int doctorId)
        {
            return _appointmentDb.Appointments.Where(a => a.Doctor?.DoctorId == doctorId).ToList();
        }
    }
}