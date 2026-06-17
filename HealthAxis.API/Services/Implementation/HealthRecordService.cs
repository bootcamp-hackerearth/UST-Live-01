using AutoMapper;
using HealthAxis.API.DTO;
using HealthAxis.API.Enums;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.DTO.HealthRecordDto;

namespace HealthAxis.API.Services.Implementation
{
    public class HealthRecordService(
        IHealthRecordRepository healthRecordRepository,
        IPatientRepository patientRepository,
        IAppointmentRepository appointmentRepository,
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

            return mapper.Map<List<HealthRecordDto>>(patientRecords);
        }

        public async Task<HealthRecordDto> GetByIdAsync(int id)
        {
            var record = await healthRecordRepository.GetByIdAsync(id);

            if (record == null)
            {
                throw new NotFoundException("Health record not found");
            }

            return mapper.Map<HealthRecordDto>(record);
        }

        public async Task<HealthRecordDto> AddAsync(CreateHealthRecordDto healthRecordDto)
        {
            var patient = await patientRepository.GetByIdAsync(healthRecordDto.PatientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            var appointment = await appointmentRepository.GetByIdAsync(healthRecordDto.AppointmentId);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found");
            }

            if (appointment.PatientId != healthRecordDto.PatientId)
            {
                throw new BusinessRuleException("Appointment does not belong to this patient");
            }

            if (appointment.Status != AppointmentStatus.Completed)
            {
                throw new BusinessRuleException( "Health record can be added only for completed appointments");
            }

            if (healthRecordDto.VisitDate == default)
            {
                throw new ValidationException("Visit date is required");
            }

            if (healthRecordDto.VisitDate.Date > DateTime.Today)
            {
                throw new ValidationException( "Visit date cannot be in the future");
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
                PatientId = healthRecordDto.PatientId,
                DoctorId = appointment.DoctorId,
                VisitDate = healthRecordDto.VisitDate,
                Diagnosis = healthRecordDto.Diagnosis,
                Prescription = healthRecordDto.Prescription,
                Notes = healthRecordDto.Notes
            };

            var savedRecord =await healthRecordRepository.AddAsync(healthRecord);

            return mapper.Map<HealthRecordDto>(savedRecord);
        }
    }
}