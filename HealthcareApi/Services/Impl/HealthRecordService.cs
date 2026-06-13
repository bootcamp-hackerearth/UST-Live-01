using SharedClasses.Dtos;
using SharedClasses.Enums;
using HealthcareApi.Exceptions;
using HealthcareApi.Models;
using HealthcareApi.Repositories;
using System.Collections.Generic;

namespace HealthcareApi.Services.Implementations
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

        public List<HealthRecordDto> GetAllRecords()
        {
            List<HealthRecord> records = _healthRecordRepository.GetAll();

            return MapToDtoList(records);
        }

        public HealthRecordDto GetRecordById(int healthRecordId)
        {
            HealthRecord record = GetHealthRecordEntityById(healthRecordId);

            return MapToDto(record);
        }

        public List<HealthRecordDto> GetRecordsByPatient(int patientId)
        {
            ValidatePatientExists(patientId);

            List<HealthRecord> records =
                _healthRecordRepository.GetByPatientId(patientId);

            return MapToDtoList(records);
        }

        public List<HealthRecordDto> GetRecordsByDoctor(int doctorId)
        {
            ValidateDoctorExists(doctorId);

            List<HealthRecord> records =
                _healthRecordRepository.GetByDoctorId(doctorId);

            return MapToDtoList(records);
        }

        public List<HealthRecordDto> GetRecordsByAppointment(int appointmentId)
        {
            GetAppointmentEntityById(appointmentId);

            List<HealthRecord> records =
                _healthRecordRepository.GetByAppointmentId(appointmentId);

            return MapToDtoList(records);
        }

        public HealthRecordDto AddRecord(AddHealthRecordDto dto)
        {
            if (dto == null)
            {
                throw new HealthRecordRuleException(
                    "Health record details are required.");
            }

            Appointment appointment = GetAppointmentEntityById(dto.AppointmentId);

            if (appointment.Status != AppointmentStatus.Completed)
            {
                throw new HealthRecordRuleException(
                    "Health record can be added only for completed appointments.");
            }

            if (_healthRecordRepository.ExistsByAppointmentId(dto.AppointmentId))
            {
                throw new HealthRecordRuleException(
                    "Health record already exists for this appointment.");
            }

            ValidateHealthRecordText(dto.Diagnosis, dto.Prescription, dto.Notes);

            HealthRecord record = new HealthRecord
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentId = appointment.AppointmentId,
                VisitDate = appointment.ScheduledDate.Date,
                Diagnosis = dto.Diagnosis.Trim(),
                Prescription = dto.Prescription.Trim(),
                Notes = dto.Notes == null ? string.Empty : dto.Notes.Trim()
            };

            HealthRecord savedRecord = _healthRecordRepository.Add(record);

            return MapToDto(savedRecord);
        }

        public HealthRecordDto UpdateRecord(int healthRecordId, UpdateHealthRecordDto dto)
        {
            if (dto == null)
            {
                throw new HealthRecordRuleException(
                    "Health record details are required.");
            }

            HealthRecord existingRecord = GetHealthRecordEntityById(healthRecordId);

            ValidateHealthRecordText(dto.Diagnosis, dto.Prescription, dto.Notes);

            existingRecord.Diagnosis = dto.Diagnosis.Trim();
            existingRecord.Prescription = dto.Prescription.Trim();
            existingRecord.Notes = dto.Notes == null ? string.Empty : dto.Notes.Trim();

            HealthRecord updatedRecord =
                _healthRecordRepository.Update(healthRecordId, existingRecord);

            if (updatedRecord == null)
            {
                throw new EntityNotFoundException("Health record", healthRecordId);
            }

            return MapToDto(updatedRecord);
        }

        public HealthRecordDto DeleteRecord(int healthRecordId)
        {
            ValidateHealthRecordId(healthRecordId);

            HealthRecord deletedRecord =
                _healthRecordRepository.Delete(healthRecordId);

            if (deletedRecord == null)
            {
                throw new EntityNotFoundException("Health record", healthRecordId);
            }

            return MapToDto(deletedRecord);
        }

        private HealthRecord GetHealthRecordEntityById(int healthRecordId)
        {
            ValidateHealthRecordId(healthRecordId);

            HealthRecord record = _healthRecordRepository.GetById(healthRecordId);

            if (record == null)
            {
                throw new EntityNotFoundException("Health record", healthRecordId);
            }

            return record;
        }

        private Appointment GetAppointmentEntityById(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                throw new HealthRecordRuleException(
                    "Please provide a valid appointment reference.");
            }

            Appointment appointment = _appointmentRepository.GetById(appointmentId);

            if (appointment == null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return appointment;
        }

        private void ValidatePatientExists(int patientId)
        {
            if (patientId <= 0)
            {
                throw new HealthRecordRuleException(
                    "Please provide a valid patient reference.");
            }

            Patient patient = _patientRepository.GetById(patientId);

            if (patient == null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }
        }

        private void ValidateDoctorExists(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new HealthRecordRuleException(
                    "Please provide a valid doctor reference.");
            }

            Doctor doctor = _doctorRepository.GetById(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }
        }

        private void ValidateHealthRecordId(int healthRecordId)
        {
            if (healthRecordId <= 0)
            {
                throw new HealthRecordRuleException(
                    "Please provide a valid health record reference.");
            }
        }

        private void ValidateHealthRecordText(
            string diagnosis,
            string prescription,
            string notes)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new HealthRecordRuleException(
                    "Diagnosis details are required.");
            }

            if (diagnosis.Trim().Length > 500)
            {
                throw new HealthRecordRuleException(
                    "Diagnosis details must not exceed 500 characters.");
            }

            if (string.IsNullOrWhiteSpace(prescription))
            {
                throw new HealthRecordRuleException(
                    "Prescription details are required.");
            }

            if (prescription.Trim().Length > 500)
            {
                throw new HealthRecordRuleException(
                    "Prescription details must not exceed 500 characters.");
            }

            if (!string.IsNullOrWhiteSpace(notes) && notes.Trim().Length > 1000)
            {
                throw new HealthRecordRuleException(
                    "Additional notes must not exceed 1000 characters.");
            }
        }

        private HealthRecordDto MapToDto(HealthRecord record)
        {
            if (record == null)
            {
                return null;
            }

            return new HealthRecordDto
            {
                HealthRecordId = record.HealthRecordId,
                PatientId = record.PatientId,
                DoctorId = record.DoctorId,
                AppointmentId = record.AppointmentId,
                VisitDate = record.VisitDate,
                Diagnosis = record.Diagnosis,
                Prescription = record.Prescription,
                Notes = record.Notes
            };
        }

        private List<HealthRecordDto> MapToDtoList(List<HealthRecord> records)
        {
            List<HealthRecordDto> recordDtos = new List<HealthRecordDto>();

            if (records == null)
            {
                return recordDtos;
            }

            foreach (HealthRecord record in records)
            {
                recordDtos.Add(MapToDto(record));
            }

            return recordDtos;
        }
    }
}