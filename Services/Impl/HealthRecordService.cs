using HealthcareMvcApp.Enums;
using HealthcareMvcApp.Exceptions;
using HealthcareMvcApp.Models;
using HealthcareMvcApp.Repositories;
using System.Collections.Generic;


namespace HealthcareMvcApp.Services.Implementations
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _healthRecordRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public HealthRecordService(
            IHealthRecordRepository healthRecordRepository,
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository)
        {
            _healthRecordRepository = healthRecordRepository;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public HealthRecord AddRecord(
            int appointmentId,
            string diagnosis,
            string prescription,
            string notes)
        {
            Appointment appointment = ValidateAppointmentExists(appointmentId);

            if (appointment.Status != AppointmentStatus.Completed)
            {
                throw new HealthRecordRuleException(
                    "Health record can only be added after an appointment is completed.");
            }

            if (_healthRecordRepository.ExistsByAppointmentId(appointmentId))
            {
                throw new HealthRecordRuleException(
                    "A health record already exists for this appointment.");
            }

            ValidateHealthRecordText(diagnosis, prescription);

            if (string.IsNullOrWhiteSpace(notes))
            {
                notes = "No additional notes.";
            }

            HealthRecord record = new HealthRecord
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentId = appointment.AppointmentId,
                VisitDate = appointment.ScheduledDate.Date,
                Diagnosis = diagnosis.Trim(),
                Prescription = prescription.Trim(),
                Notes = notes.Trim()
            };

            _healthRecordRepository.Add(record);

            return record;
        }

        public HealthRecord GetRecordById(int recordId)
        {
            ValidateRecordId(recordId);

            HealthRecord record = _healthRecordRepository.GetById(recordId);

            if (record == null)
            {
                throw new EntityNotFoundException("Health record", recordId);
            }

            return record;
        }

        public List<HealthRecord> GetAllRecords()
        {
            return _healthRecordRepository.GetAll();
        }

        public List<HealthRecord> GetRecordsByPatient(int patientId)
        {
            ValidatePatientExists(patientId);

            return _healthRecordRepository.GetByPatientId(patientId);
        }

        public List<HealthRecord> GetRecordsByDoctor(int doctorId)
        {
            ValidateDoctorExists(doctorId);

            return _healthRecordRepository.GetByDoctorId(doctorId);
        }

        public List<HealthRecord> GetRecordsByAppointment(int appointmentId)
        {
            ValidateAppointmentExists(appointmentId);

            return _healthRecordRepository.GetByAppointmentId(appointmentId);
        }

        public HealthRecord UpdateRecord(HealthRecord record)
        {
            ValidateHealthRecord(record);

            HealthRecord existingRecord = _healthRecordRepository.GetById(record.HealthRecordId);

            if (existingRecord == null)
            {
                throw new EntityNotFoundException("Health record", record.HealthRecordId);
            }

            ValidatePatientExists(record.PatientId);
            ValidateDoctorExists(record.DoctorId);

            Appointment appointment = ValidateAppointmentExists(record.AppointmentId);

            if (appointment.Status != AppointmentStatus.Completed)
            {
                throw new HealthRecordRuleException(
                    "Health record must be linked to a completed appointment.");
            }

            if (appointment.PatientId != record.PatientId ||
                appointment.DoctorId != record.DoctorId)
            {
                throw new HealthRecordRuleException(
                    "Health record patient and doctor must match the linked appointment.");
            }

            record.VisitDate = record.VisitDate.Date;
            record.Diagnosis = record.Diagnosis.Trim();
            record.Prescription = record.Prescription.Trim();

            if (string.IsNullOrWhiteSpace(record.Notes))
            {
                record.Notes = "No additional notes.";
            }
            else
            {
                record.Notes = record.Notes.Trim();
            }

            bool updated = _healthRecordRepository.Update(record);

            if (!updated)
            {
                throw new EntityNotFoundException("Health record", record.HealthRecordId);
            }

            return record;
        }

        public HealthRecord DeleteRecord(int recordId)
        {
            ValidateRecordId(recordId);

            HealthRecord existingRecord = _healthRecordRepository.GetById(recordId);

            if (existingRecord == null)
            {
                throw new EntityNotFoundException("Health record", recordId);
            }

            bool deleted = _healthRecordRepository.Delete(recordId);

            if (!deleted)
            {
                throw new EntityNotFoundException("Health record", recordId);
            }

            return existingRecord;
        }
        private Appointment ValidateAppointmentExists(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                throw new BusinessRuleException("Valid Appointment ID is required.");
            }

            Appointment appointment = _appointmentRepository.GetById(appointmentId);

            if (appointment == null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return appointment;
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

        private static void ValidateRecordId(int recordId)
        {
            if (recordId <= 0)
            {
                throw new BusinessRuleException("Valid Health Record ID is required.");
            }
        }

        private static void ValidateHealthRecord(HealthRecord record)
        {
            if (record == null)
            {
                throw new HealthRecordRuleException("Health record details are required.");
            }

            if (record.HealthRecordId <= 0)
            {
                throw new BusinessRuleException("Valid Health Record ID is required.");
            }

            if (record.PatientId <= 0)
            {
                throw new BusinessRuleException("Valid Patient ID is required.");
            }

            if (record.DoctorId <= 0)
            {
                throw new BusinessRuleException("Valid Doctor ID is required.");
            }

            if (record.AppointmentId <= 0)
            {
                throw new BusinessRuleException("Valid Appointment ID is required.");
            }

            ValidateHealthRecordText(record.Diagnosis, record.Prescription);
        }

        private static void ValidateHealthRecordText(
            string diagnosis,
            string prescription)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new HealthRecordRuleException("Diagnosis is required.");
            }

            if (string.IsNullOrWhiteSpace(prescription))
            {
                throw new HealthRecordRuleException("Prescription is required.");
            }
        }

        public HealthRecord GetRecordForDoctor(int recordId, int doctorId)
        {
            ValidateDoctorExists(doctorId);

            HealthRecord record = GetRecordById(recordId);

            if (record.DoctorId != doctorId)
            {
                throw new HealthRecordRuleException(
                    "This health record does not belong to the selected doctor.");
            }

            return record;
        }

        public HealthRecord UpdateRecordByDoctor(int doctorId, HealthRecord record)
        {
            ValidateDoctorExists(doctorId);

            if (record == null)
            {
                throw new HealthRecordRuleException("Health record details are required.");
            }

            HealthRecord existingRecord = GetRecordById(record.HealthRecordId);

            if (existingRecord.DoctorId != doctorId)
            {
                throw new HealthRecordRuleException(
                    "This health record does not belong to the selected doctor.");
            }

            if (record.DoctorId != doctorId)
            {
                throw new HealthRecordRuleException(
                    "Doctor cannot update another doctor's health record.");
            }

            return UpdateRecord(record);
        }
    }
}

