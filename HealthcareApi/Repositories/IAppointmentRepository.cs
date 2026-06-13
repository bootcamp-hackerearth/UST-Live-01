using SharedClasses.Enums;
using HealthcareApi.Models;
using System;
using System.Collections.Generic;

namespace HealthcareApi.Repositories
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAll();

        Appointment GetById(int appointmentId);

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

        List<Appointment> GetCancelledAppointmentsByPatientId(int patientId);

        List<Appointment> GetCancelledAppointmentsByDoctorId(int doctorId);

        int CountActiveAppointmentsByDoctorAndDate(int doctorId, DateTime date);

        bool IsSlotBooked(int doctorId, DateTime date, int slotNumber);

        bool PatientHasActiveAppointmentWithDoctorOnDate(
            int patientId,
            int doctorId,
            DateTime date);
        bool PatientHasActiveAppointmentOnDateAndSlot(
    int patientId,
    DateTime date,
    int slotNumber);


        Appointment Add(Appointment appointment);

        Appointment Update(int appointmentId, Appointment appointment);

        Appointment Delete(int appointmentId);
    }
}