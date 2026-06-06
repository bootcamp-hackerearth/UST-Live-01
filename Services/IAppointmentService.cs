using System;
using System.Collections.Generic;
using HealthcareMvcApp.Models;

namespace HealthcareMvcApp.Services
{
    public interface IAppointmentService
    {
        Appointment BookAppointment(
            int patientId,
            int doctorId,
            DateTime date,
            int slotNumber);

        Appointment GetAppointmentById(int appointmentId);

        Appointment ConfirmAppointment(int appointmentId, int doctorId);

        Appointment CancelAppointmentByPatient(
            int appointmentId,
            int patientId,
            string reason);

        Appointment CancelAppointmentByDoctor(
            int appointmentId,
            int doctorId,
            string reason);

        Appointment CompleteAppointment(int appointmentId, int doctorId);

        List<Appointment> GetAllAppointments();

        List<Appointment> GetAppointmentsByPatient(int patientId);

        List<Appointment> GetAppointmentsByDoctor(int doctorId);

        List<Appointment> GetUpcomingAppointments();

        List<Appointment> GetUpcomingAppointmentsByPatient(int patientId);

        List<Appointment> GetUpcomingAppointmentsByDoctor(int doctorId);

        List<Appointment> GetPendingAppointmentsByPatient(int patientId);

        List<Appointment> GetPendingAppointmentsByDoctor(int doctorId);

        List<Appointment> GetTodayConfirmedAppointmentsByDoctor(int doctorId);
        Appointment UpdateAppointment(Appointment appointment);
        Appointment DeleteAppointment(int appointmentId);
        List<Appointment> GetCancelledAppointmentsByPatient(int patientId);
    }
}