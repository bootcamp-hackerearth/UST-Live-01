using HealthcareApi.Data;
using HealthcareApi.Enums;
using HealthcareApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareApi.Repositories.Implementations
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HealthcareDbContext _context;

        public AppointmentRepository(HealthcareDbContext context)
        {
            _context = context;
        }

        public List<Appointment> GetAll()
        {
            return _context.Appointments
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }

        public Appointment GetById(int appointmentId)
        {
            return _context.Appointments
                .FirstOrDefault(a => a.AppointmentId == appointmentId);
        }

        public List<Appointment> GetByPatientId(int patientId)
        {
            return _context.Appointments
                .Where(a => a.PatientId == patientId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }

        public List<Appointment> GetByDoctorId(int doctorId)
        {
            return _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }

        public List<Appointment> GetByStatus(AppointmentStatus status)
        {
            return _context.Appointments
                .Where(a => a.Status == status)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }

        public List<Appointment> GetUpcomingAppointments()
        {
            DateTime today = DateTime.Today;

            return _context.Appointments
                .Where(a =>
                    a.ScheduledDate >= today &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.Status != AppointmentStatus.Completed)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }

        public List<Appointment> GetUpcomingAppointmentsByPatientId(int patientId)
        {
            DateTime today = DateTime.Today;

            return _context.Appointments
                .Where(a =>
                    a.PatientId == patientId &&
                    a.ScheduledDate >= today &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.Status != AppointmentStatus.Completed)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }

        public List<Appointment> GetUpcomingAppointmentsByDoctorId(int doctorId)
        {
            DateTime today = DateTime.Today;

            return _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate >= today &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.Status != AppointmentStatus.Completed)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }

        public List<Appointment> GetPendingAppointmentsByPatientId(int patientId)
        {
            return _context.Appointments
                .Where(a =>
                    a.PatientId == patientId &&
                    a.Status == AppointmentStatus.Pending)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }

        public List<Appointment> GetPendingAppointmentsByDoctorId(int doctorId)
        {
            return _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.Status == AppointmentStatus.Pending)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }

        public List<Appointment> GetTodayConfirmedAppointmentsByDoctorId(int doctorId)
        {
            DateTime today = DateTime.Today;

            return _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate == today &&
                    a.Status == AppointmentStatus.Confirmed)
                .OrderBy(a => a.SlotNumber)
                .ToList();
        }

        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            return GetByPatientId(patientId);
        }

        public List<Appointment> GetAppointmentsByDoctorId(int doctorId)
        {
            return GetByDoctorId(doctorId);
        }

        public List<Appointment> GetCancelledAppointmentsByPatientId(int patientId)
        {
            return _context.Appointments
                .Where(a =>
                    a.PatientId == patientId &&
                    a.Status == AppointmentStatus.Cancelled)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }

        public int CountActiveAppointmentsByDoctorAndDate(int doctorId, DateTime date)
        {
            DateTime selectedDate = date.Date;

            return _context.Appointments.Count(a =>
                a.DoctorId == doctorId &&
                a.ScheduledDate == selectedDate &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public bool IsSlotBooked(int doctorId, DateTime date, int slotNumber)
        {
            DateTime selectedDate = date.Date;

            return _context.Appointments.Any(a =>
                a.DoctorId == doctorId &&
                a.ScheduledDate == selectedDate &&
                a.SlotNumber == slotNumber &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public bool PatientHasActiveAppointmentWithDoctorOnDate(
            int patientId,
            int doctorId,
            DateTime date)
        {
            DateTime selectedDate = date.Date;

            return _context.Appointments.Any(a =>
                a.PatientId == patientId &&
                a.DoctorId == doctorId &&
                a.ScheduledDate == selectedDate &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public Appointment Add(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return appointment;
        }

        public Appointment Update(int appointmentId, Appointment appointment)
        {
            Appointment existingAppointment = GetById(appointmentId);

            if (existingAppointment == null)
            {
                return null;
            }

            existingAppointment.PatientId = appointment.PatientId;
            existingAppointment.DoctorId = appointment.DoctorId;
            existingAppointment.ScheduledDate = appointment.ScheduledDate.Date;
            existingAppointment.SlotNumber = appointment.SlotNumber;
            existingAppointment.Status = appointment.Status;
            existingAppointment.CancellationReason = appointment.CancellationReason;

            _context.SaveChanges();

            return existingAppointment;
        }

        public Appointment Delete(int appointmentId)
        {
            Appointment existingAppointment = GetById(appointmentId);

            if (existingAppointment == null)
            {
                return null;
            }

            _context.Appointments.Remove(existingAppointment);
            _context.SaveChanges();

            return existingAppointment;
        }
    }
}