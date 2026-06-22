using AutoMapper;
using HealthAxis.API.DTO.HealthRecordDtos;
using HealthAxis.API.Enums;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;

namespace HealthAxis.API.Services.Implementation
{
    public class HealthRecordService(
        IHealthRecordRepository healthRecordRepository,
        IPatientRepository patientRepository,
        IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository,
        IMapper mapper) : IHealthRecordService
    {
        public async Task<List<HealthRecordDto>> GetByPatientIdAsync(int patientId)
        {
            var patient = await patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            var records = await healthRecordRepository.GetAllAsync();

            var patientRecords = records
                .Where(record => record.PatientId == patientId)
                .ToList();

            var result = new List<HealthRecordDto>();

            foreach (var record in patientRecords)
            {
                var doctor = await doctorRepository.GetByIdAsync(record.DoctorId);

                result.Add(new HealthRecordDto
                {
                    RecordId = record.RecordId,
                    AppointmentId = record.AppointmentId,
                    PatientId = record.PatientId,
                    DoctorId = record.DoctorId,
                    Specialisation = doctor?.Specialisation.ToString() ?? string.Empty,
                    VisitDate = record.VisitDate,
                    Diagnosis = record.Diagnosis,
                    Prescription = record.Prescription,
                    Notes = record.Notes
                });
            }

            return result;
        }

        public async Task<HealthRecordDto> GetByIdAsync(int id)
        {
            var record = await healthRecordRepository.GetByIdAsync(id);

            if (record == null)
            {
                throw new NotFoundException("Health record not found");
            }

            var doctor = await doctorRepository.GetByIdAsync(record.DoctorId);

            return new HealthRecordDto
            {
                RecordId = record.RecordId,
                AppointmentId = record.AppointmentId,
                PatientId = record.PatientId,
                DoctorId = record.DoctorId,
                Specialisation = doctor?.Specialisation.ToString() ?? string.Empty,
                VisitDate = record.VisitDate,
                Diagnosis = record.Diagnosis,
                Prescription = record.Prescription,
                Notes = record.Notes
            };
        }

        public async Task<HealthRecordDto> AddAsync(
     CreateHealthRecordDto healthRecordDto,
     int loggedInDoctorId)
        {
            var appointment = await appointmentRepository.GetByIdAsync(
                healthRecordDto.AppointmentId);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found");
            }

            if (appointment.DoctorId != loggedInDoctorId)
            {
                throw new BusinessRuleException(
                    "You cannot add health record for another doctor's appointment");
            }

            var patient = await patientRepository.GetByIdAsync(
                appointment.PatientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            var doctor = await doctorRepository.GetByIdAsync(
                appointment.DoctorId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            if (appointment.Status != AppointmentStatus.Confirmed)
            {
                throw new BusinessRuleException(
                    "Health record can be added only for confirmed appointments");
            }

            var existingRecords = await healthRecordRepository.GetAllAsync();

            var recordAlreadyExists = existingRecords.Any(record =>
                record.AppointmentId == healthRecordDto.AppointmentId);

            if (recordAlreadyExists)
            {
                throw new BusinessRuleException(
                    "Health record already exists for this appointment");
            }

            if (healthRecordDto.VisitDate == default)
            {
                throw new ValidationException("Visit date is required");
            }

            if (healthRecordDto.VisitDate.Date > DateTime.Today)
            {
                throw new ValidationException(
                    "Visit date cannot be in the future");
            }

            if (string.IsNullOrWhiteSpace(healthRecordDto.Diagnosis))
            {
                throw new ValidationException("Diagnosis is required");
            }

            if (string.IsNullOrWhiteSpace(healthRecordDto.Prescription))
            {
                throw new ValidationException("Prescription is required");
            }

            var healthRecord = new HealthRecord
            {
                AppointmentId = healthRecordDto.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                VisitDate = healthRecordDto.VisitDate,
                Diagnosis = healthRecordDto.Diagnosis,
                Prescription = healthRecordDto.Prescription,
                Notes = healthRecordDto.Notes
            };

            var savedRecord = await healthRecordRepository.AddAsync(healthRecord);

            appointment.Status = AppointmentStatus.Completed;

            await appointmentRepository.UpdateAsync(
                appointment.AppointmentId,
                appointment);

            return new HealthRecordDto
            {
                RecordId = savedRecord.RecordId,
                AppointmentId = savedRecord.AppointmentId,
                PatientId = savedRecord.PatientId,
                DoctorId = savedRecord.DoctorId,
                Specialisation = doctor.Specialisation.ToString(),
                VisitDate = savedRecord.VisitDate,
                Diagnosis = savedRecord.Diagnosis,
                Prescription = savedRecord.Prescription,
                Notes = savedRecord.Notes
            };
        }
    }
}