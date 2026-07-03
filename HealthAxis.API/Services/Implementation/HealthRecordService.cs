using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.HealthRecordDtos;
using HealthAxis.Shared.Enums;

namespace HealthAxis.API.Services.Implementation
{
    public class HealthRecordService(
        IHealthRecordRepository healthRecordRepository,
        IPatientRepository patientRepository,
        IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository) : IHealthRecordService
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
                .OrderByDescending(record => record.VisitDate)
                .ToList();

            var result = new List<HealthRecordDto>();

            foreach (var record in patientRecords)
            {
                result.Add(await MapToDtoAsync(record));
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

            return await MapToDtoAsync(record);
        }

        public async Task<HealthRecordDto> AddAsync(
            CreateHealthRecordDto healthRecordDto,
            int loggedInDoctorId)
        {
            ArgumentNullException.ThrowIfNull(healthRecordDto);

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

            ValidateNewHealthRecordDetails(
                healthRecordDto.VisitDate,
                healthRecordDto.Diagnosis,
                healthRecordDto.Prescription);

            var healthRecord = new HealthRecord
            {
                AppointmentId = healthRecordDto.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                VisitDate = healthRecordDto.VisitDate.Date,
                Diagnosis = healthRecordDto.Diagnosis.Trim(),
                Prescription = healthRecordDto.Prescription.Trim(),
                Notes = healthRecordDto.Notes.Trim(),
                UpdatedDate = null
            };

            var savedRecord = await healthRecordRepository.AddAsync(healthRecord);

            appointment.Status = AppointmentStatus.Completed;

            await appointmentRepository.UpdateAsync(
                appointment.AppointmentId,
                appointment);

            return await MapToDtoAsync(savedRecord);
        }

        public async Task<HealthRecordDto> UpdateAsync(
            int id,
            UpdateHealthRecordDto healthRecordDto,
            int loggedInDoctorId)
        {
            ArgumentNullException.ThrowIfNull(healthRecordDto);

            var record = await healthRecordRepository.GetByIdAsync(id);

            if (record == null)
            {
                throw new NotFoundException("Health record not found");
            }

            var appointment = await appointmentRepository.GetByIdAsync(
                record.AppointmentId);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found");
            }

            if (appointment.DoctorId != loggedInDoctorId ||
                record.DoctorId != loggedInDoctorId)
            {
                throw new BusinessRuleException(
                    "You cannot edit another doctor's health record");
            }

            ValidateUpdatedHealthRecordDetails(
                healthRecordDto.Diagnosis,
                healthRecordDto.Prescription);

            record.Diagnosis = healthRecordDto.Diagnosis.Trim();
            record.Prescription = healthRecordDto.Prescription.Trim();
            record.Notes = healthRecordDto.Notes.Trim();
            record.UpdatedDate = DateTime.UtcNow;

            var updatedRecord = await healthRecordRepository.UpdateAsync(
                id,
                record);

            if (updatedRecord == null)
            {
                throw new NotFoundException("Health record not found");
            }

            return await MapToDtoAsync(updatedRecord);
        }

        private async Task<HealthRecordDto> MapToDtoAsync(HealthRecord record)
        {
            var patient = await patientRepository.GetByIdAsync(record.PatientId);
            var doctor = await doctorRepository.GetByIdAsync(record.DoctorId);

            return new HealthRecordDto
            {
                RecordId = record.HealthRecordId,
                AppointmentId = record.AppointmentId,
                PatientId = record.PatientId,
                PatientName = patient?.FullName ?? string.Empty,
                DoctorId = record.DoctorId,
                DoctorName = doctor?.FullName ?? string.Empty,
                Specialisation = doctor?.Specialisation.ToString() ?? string.Empty,
                VisitDate = record.VisitDate,
                Diagnosis = record.Diagnosis,
                Prescription = record.Prescription,
                Notes = record.Notes,
                UpdatedDate = record.UpdatedDate
            };
        }

        private static void ValidateNewHealthRecordDetails(
            DateTime visitDate,
            string diagnosis,
            string prescription)
        {
            if (visitDate == default)
            {
                throw new ValidationException("Visit date is required");
            }

            if (visitDate.Date > DateTime.Today)
            {
                throw new ValidationException(
                    "Visit date cannot be in the future");
            }

            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new ValidationException("Diagnosis is required");
            }

            if (string.IsNullOrWhiteSpace(prescription))
            {
                throw new ValidationException("Prescription is required");
            }
        }

        private static void ValidateUpdatedHealthRecordDetails(
            string diagnosis,
            string prescription)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new ValidationException("Diagnosis is required");
            }

            if (string.IsNullOrWhiteSpace(prescription))
            {
                throw new ValidationException("Prescription is required");
            }
        }
    }
}