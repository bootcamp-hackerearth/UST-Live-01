using HealthcareApi.Data;
using HealthcareApi.Models;
using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderBy(a => a.AppointmentId)
                .ToList();
        }

        public Appointment GetById(int appointmentId)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefault(a => a.AppointmentId == appointmentId);
        }

        public List<Appointment> GetByPatientId(int patientId)
        {
            return _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderBy(a => a.AppointmentId)
                .ToList();
        }

        public List<Appointment> GetByDoctorId(int doctorId)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.AppointmentId)
                .ToList();
        }

        public List<Appointment> GetByStatus(AppointmentStatus status)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Status == status)
                .OrderBy(a => a.AppointmentId)
                .ToList();
        }

        public List<Appointment> GetUpcomingAppointments()
        {
            DateTime today = DateTime.Today;

            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
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
                .Include(a => a.Doctor)
                .Where(a =>
                    a.PatientId == patientId &&
                    a.ScheduledDate >= today &&
                    (a.Status == AppointmentStatus.Pending ||
                        a.Status == AppointmentStatus.Confirmed))
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }

        public List<Appointment> GetUpcomingAppointmentsByDoctorId(int doctorId)
        {
            DateTime today = DateTime.Today;

            return _context.Appointments
                .Include(a => a.Patient)
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate >= today &&
                   (a.Status == AppointmentStatus.Pending ||
                       a.Status == AppointmentStatus.Confirmed))
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }

        public List<Appointment> GetPendingAppointmentsByPatientId(int patientId)
        {
            return _context.Appointments
                .Include(a=> a.Doctor)
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
                .Include(a => a.Patient)
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
                .Include(a => a.Patient)
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate == today &&
                    a.Status == AppointmentStatus.Confirmed)
                .OrderBy(a => a.SlotNumber)
                .ToList();
        }

        public List<Appointment> GetCancelledAppointmentsByPatientId(int patientId)
        {
            return _context.Appointments
                .Include(a => a.Doctor)
                .Where(a =>
                    a.PatientId == patientId &&
                    a.Status == AppointmentStatus.Cancelled)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }

        public List<Appointment> GetCancelledAppointmentsByDoctorId(int doctorId)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.Status == AppointmentStatus.Cancelled)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }
        public List<Appointment> SearchAppointments(string query)
        {
            IQueryable<Appointment> appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor);

            appointments = ApplyAppointmentSearch(appointments, query);

            return appointments
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
        }


        public List<Appointment> SearchAppointmentsByPatientId(int patientId, string query)
        {
            IQueryable<Appointment> appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId);

            appointments = ApplyAppointmentSearch(appointments, query);

            return appointments
                .OrderBy(a => a.AppointmentId)
                .ToList();
        }

        public List<Appointment> SearchAppointmentsByDoctorId(int doctorId, string query)
        {
            IQueryable<Appointment> appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId);

            appointments = ApplyAppointmentSearch(appointments, query);

            return appointments
                .OrderBy(a => a.AppointmentId)
                .ToList();
        }

        public List<Appointment> SearchCancelledAppointmentsByPatientId(
            int patientId,
            string query)
        {
            IQueryable<Appointment> appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.PatientId == patientId &&
                    a.Status == AppointmentStatus.Cancelled);

            appointments = ApplyAppointmentSearch(appointments, query);

            return appointments
                .OrderBy(a => a.AppointmentId)
                .ToList();
        }

        public List<Appointment> SearchCancelledAppointmentsByDoctorId(
            int doctorId,
            string query)
        {
            IQueryable<Appointment> appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.Status == AppointmentStatus.Cancelled);

            appointments = ApplyAppointmentSearch(appointments, query);

            return appointments
                .OrderBy(a => a.AppointmentId)
                .ToList();
        }

        public List<Appointment> SearchUpcomingAppointmentsByDoctorId(
            int doctorId,
            string query, AppointmentStatus? status)
        {
            DateTime today = DateTime.Today;

            IQueryable<Appointment> appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate >= today &&
                    (a.Status == AppointmentStatus.Pending ||
                        a.Status == AppointmentStatus.Confirmed));


            if (status.HasValue)
            {
                appointments = appointments.Where(a => a.Status == status.Value);
            }

            appointments = ApplyAppointmentSearch(appointments, query);

            return appointments
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.SlotNumber)
                .ToList();
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
                a.Status != AppointmentStatus.Cancelled );
        }

        public bool HasConfirmedAppointmentForDoctorOnDate(int doctorId, DateTime date)
        {
            return _context.Appointments.Any(a =>
                a.DoctorId == doctorId &&
                a.ScheduledDate == date &&
                a.Status == AppointmentStatus.Confirmed);
        }

        public Appointment Add(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return GetById(appointment.AppointmentId);
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

            return GetById(appointmentId);
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
        private IQueryable<Appointment> ApplyAppointmentSearch(
             IQueryable<Appointment> appointments,
             string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return appointments;
            }

            string searchTerm = query.Trim().ToLower();

            return appointments.Where(a =>
                (a.Patient != null &&
                    a.Patient.FullName.ToLower().Contains(searchTerm)) ||
                (a.Doctor != null &&
                    a.Doctor.FullName.ToLower().Contains(searchTerm)));
        }
        public List<Appointment> GetExpiredPendingAppointments(DateTime today)
        {
            return _context.Appointments
                .Where(a =>
                    a.Status == AppointmentStatus.Pending &&
                    a.ScheduledDate <= today)
                .ToList();
        }

        public bool PatientHasAnotherAppointmentWithDoctorOnDate(
            int appointmentId,
            int patientId,
            int doctorId,
            DateTime date)
        {
            DateTime selectedDate = date.Date;

            return _context.Appointments.Any(a =>
                a.AppointmentId != appointmentId &&
                a.PatientId == patientId &&
                a.DoctorId == doctorId &&
                a.ScheduledDate == selectedDate &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public bool IsSlotBookedByAnotherAppointment(
            int appointmentId,
            int doctorId,
            DateTime date,
            int slotNumber)
        {
            DateTime selectedDate = date.Date;

            return _context.Appointments.Any(a =>
                a.AppointmentId != appointmentId &&
                a.DoctorId == doctorId &&
                a.ScheduledDate == selectedDate &&
                a.SlotNumber == slotNumber &&
                a.Status != AppointmentStatus.Cancelled);
        }

    }
}

