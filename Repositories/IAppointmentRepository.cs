using System;
using System.Collections.Generic;
using HealthcareMvcApp.Enums;
using HealthcareMvcApp.Models;

namespace HealthcareMvcApp.Repositories
{
    public interface IAppointmentRepository
    {
        void Add(Appointment appointment);

        Appointment GetById(int appointmentId);

        List<Appointment> GetAll();

        List<Appointment> GetByPatientId(int patientId);

        List<Appointment> GetByDoctorId(int doctorId);

        List<Appointment> GetByStatus(AppointmentStatus status);

        List<Appointment> GetUpcomingAppointments();

        List<Appointment> GetUpcomingAppointmentsByPatientId(int patientId);

        List<Appointment> GetUpcomingAppointmentsByDoctorId(int doctorId);

        List<Appointment> GetPendingAppointmentsByPatientId(int patientId);

        List<Appointment> GetPendingAppointmentsByDoctorId(int doctorId);

        List<Appointment> GetTodayConfirmedAppointmentsByDoctorId(int doctorId);

        List<Appointment> GetAppointmentsByPatientId(int patientId);

        List<Appointment> GetAppointmentsByDoctorId(int doctorId);

        int CountActiveAppointmentsByDoctorAndDate(int doctorId, DateTime date);

        bool IsSlotBooked(int doctorId, DateTime date, int slotNumber);

        bool PatientHasActiveAppointmentWithDoctorOnDate(
            int patientId,
            int doctorId,
            DateTime date);

        bool Update(Appointment appointment);

        bool Delete(int appointmentId);
        List<Appointment> GetCancelledAppointmentsByPatientId(int patientId);
    }
}