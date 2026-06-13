using SharedClasses.Dtos;
using SharedClasses.Enums;
using HealthcareApi.Exceptions;
using HealthcareApi.Models;
using HealthcareApi.Repositories;
using System;
using System.Collections.Generic;

namespace HealthcareApi.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private const int MaximumSlotsPerDoctorPerDay = 16;
        private const int MaximumAdvanceBookingDays = 30;

        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IHealthRecordRepository _healthRecordRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IHealthRecordRepository healthRecordRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _healthRecordRepository = healthRecordRepository;
        }

        public List<AppointmentDto> GetAllAppointments()
        {
            List<Appointment> appointments = _appointmentRepository.GetAll();

            return MapToDtoList(appointments);
        }

        public AppointmentDto GetAppointmentById(int appointmentId)
        {
            Appointment appointment = GetAppointmentEntityById(appointmentId);

            return MapToDto(appointment);
        }

        public List<AppointmentDto> GetAppointmentsByPatient(int patientId)
        {
            ValidatePatientExists(patientId);

            List<Appointment> appointments =
                _appointmentRepository.GetAppointmentsByPatientId(patientId);

            return MapToDtoList(appointments);
        }

        public List<AppointmentDto> GetAppointmentsByDoctor(int doctorId)
        {
            ValidateDoctorExists(doctorId);

            List<Appointment> appointments =
                _appointmentRepository.GetAppointmentsByDoctorId(doctorId);

            return MapToDtoList(appointments);
        }

        public List<AppointmentDto> GetUpcomingAppointmentsByPatient(int patientId)
        {
            ValidatePatientExists(patientId);

            List<Appointment> appointments =
                _appointmentRepository.GetUpcomingAppointmentsByPatientId(patientId);

            return MapToDtoList(appointments);
        }

        public List<AppointmentDto> GetUpcomingAppointmentsByDoctor(int doctorId)
        {
            ValidateDoctorExists(doctorId);

            List<Appointment> appointments =
                _appointmentRepository.GetUpcomingAppointmentsByDoctorId(doctorId);

            return MapToDtoList(appointments);
        }

        public List<AppointmentDto> GetCancelledAppointmentsByPatient(int patientId)
        {
            ValidatePatientExists(patientId);

            List<Appointment> appointments =
                _appointmentRepository.GetCancelledAppointmentsByPatientId(patientId);

            return MapToDtoList(appointments);
        }

        public List<AppointmentDto> GetCancelledAppointmentsByDoctor(int doctorId)
        {
            ValidateDoctorExists(doctorId);

            List<Appointment> appointments =
                _appointmentRepository.GetCancelledAppointmentsByDoctorId(doctorId);

            return MapToDtoList(appointments);
        }

        public AppointmentDto BookAppointment(BookAppointmentDto dto)
        {
            if (dto == null)
            {
                throw new AppointmentRuleException("Appointment details are required.");
            }

            ValidatePatientExists(dto.PatientId);

            Doctor doctor = ValidateDoctorExists(dto.DoctorId);

            ValidateAppointmentDate(dto.ScheduledDate);
            ValidateSlotNumber(dto.SlotNumber);
            ValidateDoctorAvailability(doctor, dto.ScheduledDate);

            ValidatePatientSlotConflict(
                dto.PatientId,
                dto.ScheduledDate,
                dto.SlotNumber);

            ValidatePatientDuplicateAppointment(
                dto.PatientId,
                dto.DoctorId,
                dto.ScheduledDate);

            bool slotAlreadyBooked =
                _appointmentRepository.IsSlotBooked(
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    dto.SlotNumber);

            if (slotAlreadyBooked)
            {
                throw new AppointmentRuleException(
                    "Selected appointment slot is already booked.");
            }

            Appointment appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = dto.ScheduledDate.Date,
                SlotNumber = dto.SlotNumber,
                Status = AppointmentStatus.Pending,
                CancellationReason = string.Empty
            };

            Appointment savedAppointment = _appointmentRepository.Add(appointment);

            return MapToDto(savedAppointment);
        }

        public AppointmentDto UpdateAppointment(int appointmentId, UpdateAppointmentDto dto)
        {
            ValidateAppointmentId(appointmentId);

            if (dto == null)
            {
                throw new AppointmentRuleException("Appointment details are required.");
            }

            Appointment existingAppointment = GetAppointmentEntityById(appointmentId);

            ValidatePatientExists(dto.PatientId);

            Doctor doctor = ValidateDoctorExists(dto.DoctorId);

            ValidateAppointmentDate(dto.ScheduledDate);
            ValidateSlotNumber(dto.SlotNumber);
            ValidateDoctorAvailability(doctor, dto.ScheduledDate);

            bool slotTakenByAnotherAppointment =
                _appointmentRepository.IsSlotBooked(
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    dto.SlotNumber)
                &&
                !(existingAppointment.DoctorId == dto.DoctorId &&
                  existingAppointment.ScheduledDate.Date == dto.ScheduledDate.Date &&
                  existingAppointment.SlotNumber == dto.SlotNumber);

            if (slotTakenByAnotherAppointment)
            {
                throw new AppointmentRuleException(
                    "Selected appointment slot is already booked.");
            }

            existingAppointment.PatientId = dto.PatientId;
            existingAppointment.DoctorId = dto.DoctorId;
            existingAppointment.ScheduledDate = dto.ScheduledDate.Date;
            existingAppointment.SlotNumber = dto.SlotNumber;

            Appointment updatedAppointment =
                _appointmentRepository.Update(appointmentId, existingAppointment);

            if (updatedAppointment == null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return MapToDto(updatedAppointment);
        }

        public AppointmentDto DeleteAppointment(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            GetAppointmentEntityById(appointmentId);

            bool hasHealthRecord =
                _healthRecordRepository.ExistsByAppointmentId(appointmentId);

            if (hasHealthRecord)
            {
                throw new AppointmentRuleException(
                    "This appointment cannot be deleted because it has an associated health record.");
            }

            Appointment deletedAppointment = _appointmentRepository.Delete(appointmentId);

            if (deletedAppointment == null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return MapToDto(deletedAppointment);
        }

        public AppointmentDto ConfirmAppointment(int appointmentId, ConfirmAppointmentDto dto)
        {
            if (dto == null)
            {
                throw new AppointmentRuleException("Doctor details are required.");
            }

            ValidateDoctorExists(dto.DoctorId);

            Appointment appointment = GetAppointmentEntityById(appointmentId);

            if (appointment.DoctorId != dto.DoctorId)
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

            Appointment updatedAppointment =
                _appointmentRepository.Update(appointmentId, appointment);

            if (updatedAppointment == null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return MapToDto(updatedAppointment);
        }

        public AppointmentDto CancelAppointmentByPatient(int appointmentId, CancelByPatientDto dto)
        {
            if (dto == null)
            {
                throw new AppointmentRuleException("Cancellation details are required.");
            }

            ValidatePatientExists(dto.PatientId);

            Appointment appointment = GetAppointmentEntityById(appointmentId);

            if (appointment.PatientId != dto.PatientId)
            {
                throw new AppointmentRuleException(
                    "This appointment does not belong to the selected patient.");
            }

            ValidateCancellationReason(dto.Reason);

            if (!appointment.CanCancel())
            {
                throw new AppointmentRuleException(
                    "This appointment cannot be cancelled.");
            }

            if (appointment.ScheduledDate.Date <= DateTime.Today)
            {
                throw new AppointmentRuleException(
                    "Patients can cancel appointments only before the appointment date.");
            }

            string signedReason = dto.Reason.Trim() + " - Cancelled by Patient";

            appointment.Cancel(signedReason);

            Appointment updatedAppointment =
                _appointmentRepository.Update(appointmentId, appointment);

            if (updatedAppointment == null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return MapToDto(updatedAppointment);
        }

        public AppointmentDto CancelAppointmentByDoctor(int appointmentId, CancelByDoctorDto dto)
        {
            if (dto == null)
            {
                throw new AppointmentRuleException("Cancellation details are required.");
            }

            ValidateDoctorExists(dto.DoctorId);

            Appointment appointment = GetAppointmentEntityById(appointmentId);

            if (appointment.DoctorId != dto.DoctorId)
            {
                throw new AppointmentRuleException(
                    "This appointment does not belong to the selected doctor.");
            }

            ValidateCancellationReason(dto.Reason);

            if (!appointment.CanCancel())
            {
                throw new AppointmentRuleException(
                    "This appointment cannot be cancelled.");
            }

            string signedReason = dto.Reason.Trim() + " - Cancelled by Doctor";

            appointment.Cancel(signedReason);

            Appointment updatedAppointment =
                _appointmentRepository.Update(appointmentId, appointment);

            if (updatedAppointment == null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return MapToDto(updatedAppointment);
        }

        public AppointmentDto CompleteAppointment(int appointmentId, CompleteAppointmentDto dto)
        {
            if (dto == null)
            {
                throw new AppointmentRuleException("Doctor details are required.");
            }

            ValidateDoctorExists(dto.DoctorId);

            Appointment appointment = GetAppointmentEntityById(appointmentId);

            if (appointment.DoctorId != dto.DoctorId)
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

            Appointment updatedAppointment =
                _appointmentRepository.Update(appointmentId, appointment);

            if (updatedAppointment == null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return MapToDto(updatedAppointment);
        }

        private Appointment GetAppointmentEntityById(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            Appointment appointment = _appointmentRepository.GetById(appointmentId);

            if (appointment == null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return appointment;
        }

        private void ValidateAppointmentId(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                throw new AppointmentRuleException(
                    "Please provide a valid appointment reference.");
            }
        }

        private void ValidatePatientExists(int patientId)
        {
            if (patientId <= 0)
            {
                throw new AppointmentRuleException(
                    "Please provide a valid patient reference.");
            }

            Patient patient = _patientRepository.GetById(patientId);

            if (patient == null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }
        }

        private Doctor ValidateDoctorExists(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new AppointmentRuleException(
                    "Please provide a valid doctor reference.");
            }

            Doctor doctor = _doctorRepository.GetById(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return doctor;
        }

        private void ValidateAppointmentDate(DateTime scheduledDate)
        {
            if (scheduledDate.Date < DateTime.Today)
            {
                throw new AppointmentRuleException(
                    "Appointment date cannot be in the past.");
            }

            if (scheduledDate.Date > DateTime.Today.AddDays(MaximumAdvanceBookingDays))
            {
                throw new AppointmentRuleException(
                    "Appointment date must be within the next 30 days.");
            }
        }

        private void ValidateSlotNumber(int slotNumber)
        {
            if (slotNumber < 1 || slotNumber > MaximumSlotsPerDoctorPerDay)
            {
                throw new AppointmentRuleException(
                    "Please select a valid appointment time slot.");
            }
        }

        private void ValidateDoctorAvailability(Doctor doctor, DateTime scheduledDate)
        {
            if (!doctor.IsActive)
            {
                throw new AppointmentRuleException(
                    "Doctor is inactive and cannot accept appointments.");
            }

            if (!doctor.IsAvailable(scheduledDate.Date))
            {
                throw new AppointmentRuleException(
                    "Doctor is unavailable on the selected date.");
            }
        }

        private void ValidatePatientSlotConflict(
            int patientId,
            DateTime scheduledDate,
            int slotNumber)
        {
            bool conflict =
                _appointmentRepository.PatientHasActiveAppointmentOnDateAndSlot(
                    patientId,
                    scheduledDate.Date,
                    slotNumber);

            if (conflict)
            {
                throw new AppointmentRuleException(
                    "Patient already has an appointment during the selected time slot.");
            }
        }

        private void ValidatePatientDuplicateAppointment(
            int patientId,
            int doctorId,
            DateTime scheduledDate)
        {
            bool duplicate =
                _appointmentRepository.PatientHasActiveAppointmentWithDoctorOnDate(
                    patientId,
                    doctorId,
                    scheduledDate.Date);

            if (duplicate)
            {
                throw new AppointmentRuleException(
                    "Patient already has an active appointment with this doctor on the selected date.");
            }
        }

        private void ValidateCancellationReason(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new AppointmentRuleException(
                    "Cancellation reason is required.");
            }

            if (reason.Trim().Length > 500)
            {
                throw new AppointmentRuleException(
                    "Cancellation reason cannot exceed 500 characters.");
            }
        }

        private AppointmentDto MapToDto(Appointment appointment)
        {
            if (appointment == null)
            {
                return null;
            }

            return new AppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                ScheduledDate = appointment.ScheduledDate,
                SlotNumber = appointment.SlotNumber,
                Status = appointment.Status,
                CancellationReason = appointment.CancellationReason
            };
        }

        private List<AppointmentDto> MapToDtoList(List<Appointment> appointments)
        {
            List<AppointmentDto> appointmentDtos = new List<AppointmentDto>();

            if (appointments == null)
            {
                return appointmentDtos;
            }

            foreach (Appointment appointment in appointments)
            {
                appointmentDtos.Add(MapToDto(appointment));
            }

            return appointmentDtos;
        }
    }
}