using AutoMapper;
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
        private readonly IMapper _mapper;

        public HealthRecordService(
            IHealthRecordRepository healthRecordRepository,
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _healthRecordRepository = healthRecordRepository;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public List<HealthRecordDto> GetAllRecords()
        {
            List<HealthRecord> records = _healthRecordRepository.GetAll();

            return MapHealthRecordsToDtos(records);
        }

        public HealthRecordDto GetRecordById(int healthRecordId)
        {
            HealthRecord record = GetHealthRecordEntityById(healthRecordId);

            return MapHealthRecordToDto(record);
        }

        public List<HealthRecordDto> GetRecordsByPatient(int patientId)
        {
            ValidatePatientExists(patientId);

            List<HealthRecord> records =
                _healthRecordRepository.GetByPatientId(patientId);

            return MapHealthRecordsToDtos(records);
        }

        public List<HealthRecordDto> GetRecordsByDoctor(int doctorId)
        {
            ValidateDoctorExists(doctorId);

            List<HealthRecord> records =
                _healthRecordRepository.GetByDoctorId(doctorId);

            return MapHealthRecordsToDtos(records);
        }

        public List<HealthRecordDto> GetRecordsByAppointment(int appointmentId)
        {
            GetAppointmentEntityById(appointmentId);

            List<HealthRecord> records =
                _healthRecordRepository.GetByAppointmentId(appointmentId);

            return MapHealthRecordsToDtos(records);
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

            return MapHealthRecordToDto(savedRecord);
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

            return MapHealthRecordToDto(updatedRecord);
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

            return MapHealthRecordToDto(deletedRecord);
        }

        public List<HealthRecordDto> SearchHealthRecords(string query)
        {
            List<HealthRecord> records =
                _healthRecordRepository.SearchHealthRecords(query);

            return MapHealthRecordsToDtos(records);
        }
        public List<HealthRecordDto> SearchHealthRecordsByPatient(
            int patientId,
            string query)
                {
            ValidatePatientExists(patientId);

            List<HealthRecord> records =
                _healthRecordRepository.SearchHealthRecordsByPatientId(
                    patientId,
                    query);

            return MapHealthRecordsToDtos(records);
        }
        public List<HealthRecordDto> SearchHealthRecordsByDoctor(
            int doctorId,
            string query)
        {
            ValidateDoctorExists(doctorId);

            List<HealthRecord> records =
                _healthRecordRepository.SearchHealthRecordsByDoctorId(
                    doctorId,
                    query);

            return MapHealthRecordsToDtos(records);
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
                    "Valid Appointment ID is required.");
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
                    "Valid Patient ID is required.");
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
                    "Valid Doctor ID is required.");
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
                    "Valid Health Record ID is required.");
            }
        }

        private void ValidateHealthRecordText(
            string diagnosis,
            string prescription,
            string notes)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new HealthRecordRuleException("Diagnosis is required.");
            }

            if (diagnosis.Trim().Length > 500)
            {
                throw new HealthRecordRuleException(
                    "Diagnosis cannot exceed 500 characters.");
            }

            if (string.IsNullOrWhiteSpace(prescription))
            {
                throw new HealthRecordRuleException("Prescription is required.");
            }

            if (prescription.Trim().Length > 500)
            {
                throw new HealthRecordRuleException(
                    "Prescription cannot exceed 500 characters.");
            }

            if (!string.IsNullOrWhiteSpace(notes) && notes.Trim().Length > 1000)
            {
                throw new HealthRecordRuleException(
                    "Notes cannot exceed 1000 characters.");
            }
        }
        private HealthRecordDto MapHealthRecordToDto(HealthRecord record)
        {
            HealthRecordDto dto = _mapper.Map<HealthRecordDto>(record);
            PopulateHealthRecordNames(dto);

            return dto;
        }

        private List<HealthRecordDto> MapHealthRecordsToDtos(List<HealthRecord> records)
        {
            List<HealthRecordDto> dtos = _mapper.Map<List<HealthRecordDto>>(records);
            PopulateHealthRecordNames(dtos);

            return dtos;
        }

        private void PopulateHealthRecordNames(List<HealthRecordDto> records)
        {
            if (records == null)
            {
                return;
            }

            foreach (HealthRecordDto record in records)
            {
                PopulateHealthRecordNames(record);
            }
        }

        private void PopulateHealthRecordNames(HealthRecordDto record)
        {
            if (record == null)
            {
                return;
            }

            Patient patient = _patientRepository.GetById(record.PatientId);
            Doctor doctor = _doctorRepository.GetById(record.DoctorId);

            record.PatientName = patient == null
                ? "Unknown Patient"
                : patient.FullName;

            record.DoctorName = doctor == null
                ? "Unknown Doctor"
                : doctor.FullName;
        }
    }
}