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

        List<Appointment> GetCancelledAppointmentsByPatientId(int patientId);

        List<Appointment> GetCancelledAppointmentsByDoctorId(int doctorId);

        bool IsSlotBooked(int doctorId, DateTime date, int slotNumber);

        bool PatientHasActiveAppointmentWithDoctorOnDate(
            int patientId,
            int doctorId,
            DateTime date);

        bool HasConfirmedAppointmentForDoctorOnDate(int doctorId, DateTime date);

        Appointment Add(Appointment appointment);

        Appointment Update(int appointmentId, Appointment appointment);

        Appointment Delete(int appointmentId);

        List<Appointment> SearchAppointments(string query);

        List<Appointment> SearchAppointmentsByPatientId(int patientId, string query);

        List<Appointment> SearchAppointmentsByDoctorId(int doctorId, string query);

        List<Appointment> SearchCancelledAppointmentsByPatientId(int patientId, string query);

        List<Appointment> SearchCancelledAppointmentsByDoctorId(int doctorId, string query);

        List<Appointment> SearchUpcomingAppointmentsByDoctorId(int doctorId, string query, AppointmentStatus? status);

        List<Appointment> GetExpiredPendingAppointments(DateTime today);

        bool PatientHasAnotherAppointmentWithDoctorOnDate(
            int appointmentId,
            int patientId,
            int doctorId,
            DateTime date);

        bool IsSlotBookedByAnotherAppointment(
            int appointmentId,
            int doctorId,
            DateTime date,
            int slotNumber);

    }
}