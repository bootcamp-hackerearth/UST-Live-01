using HealthcareMvcApp.Enums;
using HealthcareMvcApp.Exceptions;
using HealthcareMvcApp.Models;
using HealthcareMvcApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareMvcApp.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private const int MaximumSlotsPerDoctorPerDay = 10;

        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public Appointment BookAppointment(
            int patientId,
            int doctorId,
            DateTime date,
            int slotNumber)
        {
            DateTime appointmentDate = date.Date;

            ValidatePatientExists(patientId);
            Doctor doctor = ValidateDoctorExists(doctorId);

            if (appointmentDate < DateTime.Today)
            {
                throw new AppointmentRuleException("Cannot book an appointment in the past.");
            }

            if (slotNumber < 1 || slotNumber > MaximumSlotsPerDoctorPerDay)
            {
                throw new AppointmentRuleException(
                    "Slot number must be between 1 and 10.");
            }

            if (!doctor.IsAvailable(appointmentDate))
            {
                throw new AppointmentRuleException(
                    "Doctor is unavailable on the selected date.");
            }

            int activeAppointmentCount = _appointmentRepository
                .CountActiveAppointmentsByDoctorAndDate(doctorId, appointmentDate);

            if (activeAppointmentCount >= MaximumSlotsPerDoctorPerDay)
            {
                throw new AppointmentRuleException(
                    "Doctor is fully booked on the selected date.");
            }

            bool slotBooked = _appointmentRepository
                .IsSlotBooked(doctorId, appointmentDate, slotNumber);

            if (slotBooked)
            {
                throw new AppointmentRuleException(
                    "Selected appointment slot is already booked.");
            }

            bool alreadyBooked = _appointmentRepository
                .PatientHasActiveAppointmentWithDoctorOnDate(
                    patientId,
                    doctorId,
                    appointmentDate);

            if (alreadyBooked)
            {
                throw new AppointmentRuleException(
                    "Patient already has an active appointment with this doctor on the selected date.");
            }

            Appointment appointment = new Appointment(
                patientId,
                doctorId,
                appointmentDate,
                slotNumber);

            _appointmentRepository.Add(appointment);

            return appointment;
        }

        public Appointment GetAppointmentById(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            Appointment appointment = _appointmentRepository.GetById(appointmentId);

            if (appointment == null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return appointment;
        }

        public Appointment ConfirmAppointment(int appointmentId, int doctorId)
        {
            ValidateDoctorExists(doctorId);

            Appointment appointment = GetAppointmentById(appointmentId);

            if (appointment.DoctorId != doctorId)
            {
                throw new AppointmentRuleException(
                    "This appointment does not belong to the selected doctor.");
            }

            if (!appointment.CanConfirm())
            {
                throw new AppointmentRuleException(
                    "Only pending appointments can be confirmed.");
            }

            appointment.Confirm();

            bool updated = _appointmentRepository.Update(appointment);

            if (!updated)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return appointment;
        }

        public Appointment CancelAppointmentByPatient(
            int appointmentId,
            int patientId,
            string reason)
        {
            ValidatePatientExists(patientId);

            Appointment appointment = GetAppointmentById(appointmentId);

            if (appointment.PatientId != patientId)
            {
                throw new AppointmentRuleException(
                    "This appointment does not belong to the selected patient.");
            }

            ValidateCancellationReason(reason);

            if (!appointment.CanCancel())
            {
                throw new AppointmentRuleException(
                    "This appointment cannot be cancelled.");
            }

            if (DateTime.Today >= appointment.ScheduledDate.Date)
            {
                throw new AppointmentRuleException(
                    "Patient can cancel only at least one day before the appointment date.");
            }

            string signedReason = reason.Trim() + " - Cancelled by Patient";

            appointment.Cancel(signedReason);

            bool updated = _appointmentRepository.Update(appointment);

            if (!updated)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return appointment;
        }

        public Appointment CancelAppointmentByDoctor(
            int appointmentId,
            int doctorId,
            string reason)
        {
            ValidateDoctorExists(doctorId);

            Appointment appointment = GetAppointmentById(appointmentId);

            if (appointment.DoctorId != doctorId)
            {
                throw new AppointmentRuleException(
                    "This appointment does not belong to the selected doctor.");
            }

            ValidateCancellationReason(reason);

            if (!appointment.CanCancel())
            {
                throw new AppointmentRuleException(
                    "This appointment cannot be cancelled.");
            }

            string signedReason = reason.Trim() + " - Cancelled by Doctor";

            appointment.Cancel(signedReason);

            bool updated = _appointmentRepository.Update(appointment);

            if (!updated)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return appointment;
        }

        public Appointment CompleteAppointment(int appointmentId, int doctorId)
        {
            ValidateDoctorExists(doctorId);

            Appointment appointment = GetAppointmentById(appointmentId);

            if (appointment.DoctorId != doctorId)
            {
                throw new AppointmentRuleException(
                    "This appointment does not belong to the selected doctor.");
            }

            if (!appointment.CanComplete())
            {
                throw new AppointmentRuleException(
                    "Only confirmed appointments can be completed.");
            }
            if (appointment.ScheduledDate.Date != DateTime.Today)
            {
                throw new AppointmentRuleException(
                    "Only appointments scheduled for today can be completed.");
            }
            appointment.Complete();

            bool updated = _appointmentRepository.Update(appointment);

            if (!updated)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return appointment;
        }

        public List<Appointment> GetAllAppointments()
        {
            return _appointmentRepository.GetAll();
        }

        public List<Appointment> GetAppointmentsByPatient(int patientId)
        {
            ValidatePatientExists(patientId);

            return _appointmentRepository.GetAppointmentsByPatientId(patientId);
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            ValidateDoctorExists(doctorId);

            return _appointmentRepository.GetAppointmentsByDoctorId(doctorId);
        }

        public List<Appointment> GetUpcomingAppointments()
        {
            return _appointmentRepository.GetUpcomingAppointments();
        }

        public List<Appointment> GetUpcomingAppointmentsByPatient(int patientId)
        {
            ValidatePatientExists(patientId);

            return _appointmentRepository.GetUpcomingAppointmentsByPatientId(patientId);
        }

        public List<Appointment> GetUpcomingAppointmentsByDoctor(int doctorId)
        {
            ValidateDoctorExists(doctorId);

            return _appointmentRepository.GetUpcomingAppointmentsByDoctorId(doctorId);
        }

        public List<Appointment> GetPendingAppointmentsByPatient(int patientId)
        {
            ValidatePatientExists(patientId);

            return _appointmentRepository.GetPendingAppointmentsByPatientId(patientId);
        }

        public List<Appointment> GetPendingAppointmentsByDoctor(int doctorId)
        {
            ValidateDoctorExists(doctorId);

            return _appointmentRepository.GetPendingAppointmentsByDoctorId(doctorId);
        }

        public List<Appointment> GetTodayConfirmedAppointmentsByDoctor(int doctorId)
        {
            ValidateDoctorExists(doctorId);

            return _appointmentRepository.GetTodayConfirmedAppointmentsByDoctorId(doctorId);
        }

        private Patient ValidatePatientExists(int patientId)
        {
            if (patientId <= 0)
            {
                throw new BusinessRuleException("Valid Patient ID is required.");
            }

            Patient patient = _patientRepository.GetById(patientId);

            if (patient == null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            return patient;
        }

        private Doctor ValidateDoctorExists(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new BusinessRuleException("Valid Doctor ID is required.");
            }

            Doctor doctor = _doctorRepository.GetById(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return doctor;
        }

        private static void ValidateAppointmentId(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                throw new BusinessRuleException("Valid Appointment ID is required.");
            }
        }

        private static void ValidateCancellationReason(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new AppointmentRuleException("Cancellation reason is required.");
            }
        }

        public Appointment UpdateAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new AppointmentRuleException("Appointment details are required.");
            }

            ValidateAppointmentId(appointment.AppointmentId);

            Appointment existingAppointment = _appointmentRepository.GetById(appointment.AppointmentId);

            if (existingAppointment == null)
            {
                throw new EntityNotFoundException("Appointment", appointment.AppointmentId);
            }

            ValidatePatientExists(appointment.PatientId);
            Doctor doctor = ValidateDoctorExists(appointment.DoctorId);

            if (appointment.ScheduledDate.Date < DateTime.Today)
            {
                throw new AppointmentRuleException("Appointment date cannot be in the past.");
            }

            if (appointment.SlotNumber < 1 || appointment.SlotNumber > MaximumSlotsPerDoctorPerDay)
            {
                throw new AppointmentRuleException("Slot number must be between 1 and 10.");
            }

            if (!doctor.IsAvailable(appointment.ScheduledDate.Date))
            {
                throw new AppointmentRuleException("Doctor is unavailable on the selected date.");
            }

            bool slotTakenByAnotherAppointment = _appointmentRepository
                .GetByDoctorId(appointment.DoctorId)
                .Any(a =>
                    a.AppointmentId != appointment.AppointmentId &&
                    a.ScheduledDate.Date == appointment.ScheduledDate.Date &&
                    a.SlotNumber == appointment.SlotNumber &&
                    a.Status != AppointmentStatus.Cancelled);

            if (slotTakenByAnotherAppointment)
            {
                throw new AppointmentRuleException("Selected appointment slot is already booked.");
            }

            bool updated = _appointmentRepository.Update(appointment);

            if (!updated)
            {
                throw new EntityNotFoundException("Appointment", appointment.AppointmentId);
            }

            return appointment;
        }

        public Appointment DeleteAppointment(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            Appointment existingAppointment = _appointmentRepository.GetById(appointmentId);

            if (existingAppointment == null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            bool deleted = _appointmentRepository.Delete(appointmentId);

            if (!deleted)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return existingAppointment;
        }
        public List<Appointment> GetCancelledAppointmentsByPatient(int patientId)
        {
            ValidatePatientExists(patientId);

            return _appointmentRepository.GetCancelledAppointmentsByPatientId(patientId);
        }
    }
}