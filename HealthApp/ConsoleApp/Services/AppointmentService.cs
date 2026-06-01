using System;
using System.Collections.Generic;
using System.Linq;
using HealthApp.ConsoleApp.Exceptions;
using HealthApp.ConsoleApp.Repositories;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepo;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepo = appointmentRepository;
        }
        // Book an appointment with date and time slot
        public string BookAppointment(Patient patient, Doctor doctor, DateTime date, string slot)
        {
            if (date < DateTime.Now)
            {
                throw new PastDateException("Cannot book appointment in the past.");
            }

            if (!doctor.IsAvailable(date))
            {
                throw new DoctorUnavailableException("Doctor is not available on selected date.");
            }

            var appointments = _appointmentRepo.GetAllAppointments();

            bool isSlotTaken = appointments.Any(a =>
                a.Doctor.DoctorId == doctor.DoctorId &&
                a.ScheduledDate.Date == date.Date &&
                a.TimeSlot == slot &&
                a.Status != AppointmentStatus.Cancelled);

            if (isSlotTaken)
            {
                throw new AppointmentConflictException("Selected time slot is already booked.");
            }

            var appointment = new Appointment
            {
                AppointmentId = AppointmentIdGenerator(appointments),
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = date,
                TimeSlot = slot,
                Status = AppointmentStatus.Pending
            };

            _appointmentRepo.AddAppointment(appointment);
            return $"Appointment of ID {appointment.AppointmentId} has been created successfully";
        }

        // Get appointment by patient id
        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            List<Appointment> appointments = _appointmentRepo.GetAppointmentsByPatientId(patientId);
            if (appointments.Count == 0)
            {
                throw new AppointmentNotFoundException($"No appointments found for patient ID {patientId}.");
            }

            return appointments;
        }

        // Get appointment by doctor id
        public List<Appointment> GetAppointmentsByDoctorId(int doctorId)
        {
            var appointments = _appointmentRepo.GetAppointmentsByDoctorId(doctorId);
            if (appointments.Count == 0)
            {
                throw new AppointmentNotFoundException($"No appointments found for doctor ID {doctorId}.");
            }
            
            return appointments;
        }

        // Get appointment by id
        public Appointment GetAppointmentById(int appointmentId)
        {
            Appointment? appointment = _appointmentRepo.GetAppointmentById(appointmentId);

            if (appointment is null)
            {
                throw new AppointmentNotFoundException($"Appointment of ID {appointmentId} does not exist");
            }
            return appointment;
        }

        // Assign appointment id based on latest record id
        public static int AppointmentIdGenerator(List<Appointment> appointments)
        {       
            return appointments.Count > 0
                ? appointments.Max(a => a.AppointmentId) + 1
                : 101;
        }

        //  Cancel an appointment and update reason
        public string CancelAppointment(int appointmentId, string reason)
        {
            var appointment = _appointmentRepo.GetAppointmentById(appointmentId);

            if (appointment is null)
            {
                throw new AppointmentNotFoundException($"Appointment of ID {appointmentId} does not exist");
            }
            appointment.Cancel(reason);
            return $"Appointment of ID {appointmentId} has been cancelled successfully";
        }

        //  Cancel an appointment and update reason
        public string ConfirmAppointment(int appointmentId)
        {
            var appointment = _appointmentRepo.GetAppointmentById(appointmentId);

            if (appointment is null)
            {
                throw new AppointmentNotFoundException($"Appointment of ID {appointmentId} does not exist");
            }
            appointment.Confirm();
            return $"Appointment of ID {appointmentId} has been cancelled successfully";
        }

        //  Get list of confirmed (upcoming) appointments
        public List<Appointment> GetUpcomingAppointments()
        {
            List<Appointment> upcomingAppointments =  _appointmentRepo
                .GetAllAppointments()
                .Where(a => a.ScheduledDate > DateTime.Now &&
                            a.Status != AppointmentStatus.Completed && 
                            a.Status != AppointmentStatus.Cancelled)
                .OrderBy(a => a.ScheduledDate)
                .ToList();

            if (upcomingAppointments is null)
            {
                throw new AppointmentNotFoundException("There are no upcoming appointments");
            }
            return upcomingAppointments;
        }
        public Appointment UpdateAppointment(Appointment appointment)
        {
            Appointment? existingAppointment = GetAppointmentById(appointment.AppointmentId);

            if (existingAppointment is null)
            {
                throw new AppointmentNotFoundException($"Appointment of ID {appointment.AppointmentId} does not exist");
            }
            return _appointmentRepo.UpdateAppointment(existingAppointment, appointment);
        }

        public List<Appointment> GetAllAppointments()
        {
            var appointments = _appointmentRepo.GetAllAppointments();

            if (appointments == null || appointments.Count == 0)
            {
                throw new AppointmentNotFoundException("No appointments found.");
            }

            return appointments;
        }
    }
}
