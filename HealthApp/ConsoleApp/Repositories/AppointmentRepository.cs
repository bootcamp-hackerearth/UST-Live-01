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

        public string AddAppointment(Appointment appointment)
        {
            _appointmentDb.Appointments.Add(appointment);
            return $"Appointment ID {appointment.AppointmentId} added successfully!";
        }

        public List<Appointment> GetAllAppointments()
        {
            return _appointmentDb.Appointments;
        }

        public Appointment? GetAppointmentById(int id)
        {
            return _appointmentDb.Appointments.FirstOrDefault(a => a.AppointmentId == id);
        }

        public Appointment UpdateAppointment(Appointment existingAppointment, Appointment appointment)
        {
            existingAppointment.Patient = appointment.Patient;
            existingAppointment.Doctor = appointment.Doctor;
            existingAppointment.ScheduledDate = appointment.ScheduledDate;
            existingAppointment.TimeSlot = appointment.TimeSlot;
            existingAppointment.CancellationReason = appointment.CancellationReason;

            return existingAppointment;
        }

        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            return _appointmentDb.Appointments.Where(a => a.Patient.PatientId == patientId).ToList();
        }

        public List<Appointment> GetAppointmentsByDoctorId(int doctorId)
        {
            return _appointmentDb.Appointments.Where(a => a.Doctor.DoctorId == doctorId).ToList();
        }
    }
}