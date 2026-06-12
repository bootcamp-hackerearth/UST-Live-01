using AutoMapper;
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
        private const int MaximumSlotsPerDoctorPerDay = 10;

        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IHealthRecordRepository _healthRecordRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
             IAppointmentRepository appointmentRepository,
             IPatientRepository patientRepository,
             IDoctorRepository doctorRepository,
             IHealthRecordRepository healthRecordRepository,
             IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _healthRecordRepository = healthRecordRepository;
            _mapper = mapper;
        }

        public List<AppointmentDto> GetAllAppointments()
        {
            AutoCancelExpiredPendingAppointments();

            List<Appointment> appointments = _appointmentRepository.GetAll();

            return MapAppointmentsToDtos(appointments);
        }

        public AppointmentDto GetAppointmentById(int appointmentId)
        {

            Appointment appointment = GetAppointmentEntityById(appointmentId);

            return MapAppointmentToDto(appointment);
        }

        public List<AppointmentDto> GetAppointmentsByPatient(int patientId)
        {
            ValidatePatientExists(patientId);

            AutoCancelExpiredPendingAppointments();

            List<Appointment> appointments =
                _appointmentRepository.GetByPatientId(patientId);

            return MapAppointmentsToDtos(appointments);
        }

        public List<AppointmentDto> GetAppointmentsByDoctor(int doctorId)
        {
            ValidateDoctorExists(doctorId);

            AutoCancelExpiredPendingAppointments();

            List<Appointment> appointments =
                _appointmentRepository.GetByDoctorId(doctorId);

            return MapAppointmentsToDtos(appointments);
        }

        public List<AppointmentDto> GetUpcomingAppointmentsByPatient(int patientId)
        {
            ValidatePatientExists(patientId);

            AutoCancelExpiredPendingAppointments();

            List<Appointment> appointments =
                _appointmentRepository.GetUpcomingAppointmentsByPatientId(patientId);

            return MapAppointmentsToDtos(appointments);
        }

        public List<AppointmentDto> GetUpcomingAppointmentsByDoctor(int doctorId)
        {
            ValidateDoctorExists(doctorId);

            AutoCancelExpiredPendingAppointments();

            List<Appointment> appointments =
                _appointmentRepository.GetUpcomingAppointmentsByDoctorId(doctorId);

            return MapAppointmentsToDtos(appointments);
        }

        public List<AppointmentDto> GetCancelledAppointmentsByPatient(int patientId)
        {
            ValidatePatientExists(patientId);

            AutoCancelExpiredPendingAppointments();

            List<Appointment> appointments =
                _appointmentRepository.GetCancelledAppointmentsByPatientId(patientId);

            return MapAppointmentsToDtos(appointments);
        }

        public AppointmentDto BookAppointment(BookAppointmentDto dto)
        {
            if (dto == null)
            {
                throw new AppointmentRuleException("Appointment details are required.");
            }

            DateTime appointmentDate = dto.ScheduledDate.Date;

            ValidatePatientExists(dto.PatientId);

            Doctor doctor = ValidateDoctorExists(dto.DoctorId);

            ValidateAppointmentDate(appointmentDate);
            ValidateDoctorAvailability(doctor, appointmentDate);

            ValidatePatientDuplicateAppointment(
                dto.PatientId,
                dto.DoctorId,
                appointmentDate);

            int assignedSlotNumber =
                FindNextAvailableSlot(dto.DoctorId, appointmentDate);

            Appointment appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = appointmentDate,
                SlotNumber = assignedSlotNumber,
                Status = AppointmentStatus.Pending,
                CancellationReason = string.Empty
            };

            Appointment savedAppointment = _appointmentRepository.Add(appointment);

            return MapAppointmentToDto(savedAppointment);
        }

        public AppointmentDto UpdateAppointment(int appointmentId, UpdateAppointmentDto dto)
        {
            ValidateAppointmentId(appointmentId);

            if (dto == null)
            {
                throw new AppointmentRuleException("Appointment details are required.");
            }

            Appointment existingAppointment = GetAppointmentEntityById(appointmentId);

            ValidateAppointmentCanBeEdited(existingAppointment);

            ValidatePatientExists(dto.PatientId);

            Doctor doctor = ValidateDoctorExists(dto.DoctorId);

            ValidateAppointmentDate(dto.ScheduledDate);
            ValidateSlotNumber(dto.SlotNumber);
            ValidateDoctorAvailability(doctor, dto.ScheduledDate);
            ValidatePatientDuplicateAppointmentForUpdate(
                appointmentId,
                dto.PatientId,
                dto.DoctorId,
                dto.ScheduledDate);

            bool slotTakenByAnotherAppointment =
                _appointmentRepository.IsSlotBookedByAnotherAppointment(
                    appointmentId,
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.SlotNumber);

            if (slotTakenByAnotherAppointment)
            {
                throw new AppointmentRuleException("Selected appointment slot is already booked.");
            }

            if (slotTakenByAnotherAppointment)
            {
                throw new AppointmentRuleException("Selected appointment slot is already booked.");
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

            return MapAppointmentToDto(updatedAppointment);
        }

        public AppointmentDto DeleteAppointment(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            Appointment existingAppointment = GetAppointmentEntityById(appointmentId);

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

            return MapAppointmentToDto(deletedAppointment);
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

            return MapAppointmentToDto(updatedAppointment);
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

            return MapAppointmentToDto(updatedAppointment);
        }
        public List<AppointmentDto> GetCancelledAppointmentsByDoctor(int doctorId)
        {
            ValidateDoctorExists(doctorId);

            AutoCancelExpiredPendingAppointments();

            List<Appointment> appointments =
                _appointmentRepository.GetCancelledAppointmentsByDoctorId(doctorId);

            return MapAppointmentsToDtos(appointments);
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
            
            if (appointment.Status == AppointmentStatus.Confirmed &&
                appointment.ScheduledDate.Date == DateTime.Today)
            {
                throw new AppointmentRuleException(
                    "Doctors cannot cancel confirmed appointments on the appointment date.");
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

            return MapAppointmentToDto(updatedAppointment);
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

            return MapAppointmentToDto(updatedAppointment);
        }

        
        public List<AppointmentDto> SearchAppointments(string query)
        {
            AutoCancelExpiredPendingAppointments();
            List<Appointment> appointments =
                _appointmentRepository.SearchAppointments(query);

            return MapAppointmentsToDtos(appointments);
        }
        public List<AppointmentDto> SearchCancelledAppointmentsByPatient(
            int patientId,
            string query)
        {
            ValidatePatientExists(patientId);

            AutoCancelExpiredPendingAppointments();

            List<Appointment> appointments =
                _appointmentRepository.SearchCancelledAppointmentsByPatientId(
                    patientId,
                    query);

            return MapAppointmentsToDtos(appointments);
        }
        public List<AppointmentDto> SearchCancelledAppointmentsByDoctor(
            int doctorId,
            string query)
        {
            ValidateDoctorExists(doctorId);

            AutoCancelExpiredPendingAppointments();

            List<Appointment> appointments =
                _appointmentRepository.SearchCancelledAppointmentsByDoctorId(
                    doctorId,
                    query);

            return MapAppointmentsToDtos(appointments);
        }
        public List<AppointmentDto> SearchUpcomingAppointmentsByDoctor(
            int doctorId,
            string query,
            AppointmentStatus? status)
        {
            ValidateDoctorExists(doctorId);

            AutoCancelExpiredPendingAppointments();

            List<Appointment> appointments =
                _appointmentRepository.SearchUpcomingAppointmentsByDoctorId(
                    doctorId,
                    query,
                    status);

            return MapAppointmentsToDtos(appointments);
        }
        private Appointment GetAppointmentEntityById(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            AutoCancelExpiredPendingAppointments();

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
                throw new AppointmentRuleException("Valid Appointment ID is required.");
            }
        }

        private void ValidatePatientExists(int patientId)
        {
            if (patientId <= 0)
            {
                throw new AppointmentRuleException("Valid Patient ID is required.");
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
                throw new AppointmentRuleException("Valid Doctor ID is required.");
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
            if (scheduledDate.Date <= DateTime.Today)
            {
                throw new AppointmentRuleException(
                    "Appointments must be booked at least one day in advance.");
            }
        }

        private void ValidateSlotNumber(int slotNumber)
        {
            if (slotNumber < 1 || slotNumber > MaximumSlotsPerDoctorPerDay)
            {
                throw new AppointmentRuleException(
                    "Slot number must be between 1 and 10.");
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

        private void ValidatePatientDuplicateAppointmentForUpdate(
    int appointmentId,
    int patientId,
    int doctorId,
    DateTime scheduledDate)
        {
            bool duplicate =
                _appointmentRepository.PatientHasAnotherAppointmentWithDoctorOnDate(
                    appointmentId,
                    patientId,
                    doctorId,
                    scheduledDate.Date);

            if (duplicate)
            {
                throw new AppointmentRuleException(
                    "Patient already has an appointment with this doctor on the selected date.");
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

        private void ValidateAppointmentCanBeEdited(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new AppointmentRuleException("Appointment details are required.");
            }

            if (appointment.Status != AppointmentStatus.Pending)
            {
                throw new AppointmentRuleException(
                    "Only pending appointments can be edited.");
            }
        }
        private int FindNextAvailableSlot(int doctorId, DateTime scheduledDate)
        {
            for (int slotNumber = 1; slotNumber <= MaximumSlotsPerDoctorPerDay; slotNumber++)
            {
                bool slotBooked =
                    _appointmentRepository.IsSlotBooked(
                        doctorId,
                        scheduledDate.Date,
                        slotNumber);

                if (!slotBooked)
                {
                    return slotNumber;
                }
            }

            throw new AppointmentRuleException(
                "Doctor has reached the maximum number of appointments for this date.");
        }
        private AppointmentDto MapAppointmentToDto(Appointment appointment)
        {
            AppointmentDto dto = _mapper.Map<AppointmentDto>(appointment);
            PopulateAppointmentNames(dto);

            return dto;
        }

        private List<AppointmentDto> MapAppointmentsToDtos(List<Appointment> appointments)
        {
            List<AppointmentDto> dtos = _mapper.Map<List<AppointmentDto>>(appointments); 

            PopulateAppointmentNames(dtos);

            return dtos;
        }

        private void PopulateAppointmentNames(List<AppointmentDto> appointments)
        {
            if (appointments == null)
            {
                return;
            }

            foreach (AppointmentDto appointment in appointments)
            {
                PopulateAppointmentNames(appointment);
            }
        }

        private void PopulateAppointmentNames(AppointmentDto appointment)
        {
            if (appointment == null)
            {
                return;
            }

            Patient patient = _patientRepository.GetById(appointment.PatientId);
            Doctor doctor = _doctorRepository.GetById(appointment.DoctorId);

            appointment.PatientName = patient == null
                ? "Unknown Patient"
                : patient.FullName;

            appointment.DoctorName = doctor == null
                ? "Unknown Doctor"
                : doctor.FullName;
        }

        private void AutoCancelExpiredPendingAppointments()
        {
            DateTime today = DateTime.Today;

            List<Appointment> appointments =
                _appointmentRepository.GetExpiredPendingAppointments(today);

            foreach (Appointment appointment in appointments)
            {
                appointment.Status = AppointmentStatus.Cancelled;
                appointment.CancellationReason =
                    "Automatically cancelled because the appointment was still pending on the scheduled date - Cancelled by Doctor";

                _appointmentRepository.Update(appointment.AppointmentId, appointment);
            }
        }
    }

}
