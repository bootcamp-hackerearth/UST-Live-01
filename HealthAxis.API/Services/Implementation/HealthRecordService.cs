using HealthAxis.API.Data;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.HealthRecordDtos;
using HealthAxis.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Services.Implementation
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly ApplicationDbContext _context;

        public HealthRecordService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HealthRecordDto>> GetByPatientIdAsync(int patientId)
        {
            if (patientId <= 0)
            {
                return new List<HealthRecordDto>();
            }

            var patientExists = await _context.Patients
                .AsNoTracking()
                .AnyAsync(patient => patient.PatientId == patientId);

            if (!patientExists)
            {
                return new List<HealthRecordDto>();
            }

            var records = await _context.HealthRecords
                .AsNoTracking()
                .Where(record => record.PatientId == patientId)
                .OrderByDescending(record => record.VisitDate)
                .Select(record => new HealthRecordDto
                {
                    HealthRecordId = record.HealthRecordId,
                    RecordId = record.HealthRecordId,
                    AppointmentId = record.AppointmentId,
                    PatientId = record.PatientId,
                    DoctorId = record.DoctorId,
                    VisitDate = record.VisitDate,
                    Diagnosis = record.Diagnosis ?? string.Empty,
                    Prescription = record.Prescription ?? string.Empty,
                    Notes = record.Notes ?? string.Empty,
                    UpdatedDate = record.UpdatedDate,
                    PatientName = record.Patient.FullName,
                    DoctorName = record.Doctor.FullName,
                    Specialisation = record.Doctor.Specialisation.ToString()
                })
                .ToListAsync();

            return records;
        }

        public async Task<HealthRecordDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            return await _context.HealthRecords
                .AsNoTracking()
                .Where(record => record.HealthRecordId == id)
                .Select(record => new HealthRecordDto
                {
                    HealthRecordId = record.HealthRecordId,
                    RecordId = record.HealthRecordId,
                    AppointmentId = record.AppointmentId,
                    PatientId = record.PatientId,
                    DoctorId = record.DoctorId,
                    VisitDate = record.VisitDate,
                    Diagnosis = record.Diagnosis ?? string.Empty,
                    Prescription = record.Prescription ?? string.Empty,
                    Notes = record.Notes ?? string.Empty,
                    UpdatedDate = record.UpdatedDate,
                    PatientName = record.Patient.FullName,
                    DoctorName = record.Doctor.FullName,
                    Specialisation = record.Doctor.Specialisation.ToString()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<HealthRecordDto> AddAsync(
            CreateHealthRecordDto healthRecordDto,
            int loggedInDoctorId)
        {
            ArgumentNullException.ThrowIfNull(healthRecordDto);

            ValidateNewHealthRecordDetails(
                healthRecordDto.VisitDate,
                healthRecordDto.Diagnosis,
                healthRecordDto.Prescription);

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(item =>
                    item.AppointmentId == healthRecordDto.AppointmentId);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found");
            }

            if (appointment.DoctorId != loggedInDoctorId)
            {
                throw new BusinessRuleException(
                    "You cannot add health record for another doctor's appointment");
            }

            if (appointment.Status != AppointmentStatus.Confirmed)
            {
                throw new BusinessRuleException(
                    "Health record can be added only for confirmed appointments");
            }

            var recordAlreadyExists = await _context.HealthRecords
                .AnyAsync(record =>
                    record.AppointmentId == healthRecordDto.AppointmentId);

            if (recordAlreadyExists)
            {
                throw new BusinessRuleException(
                    "Health record already exists for this appointment");
            }

            var healthRecord = new HealthRecord
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                VisitDate = healthRecordDto.VisitDate.Date,
                Diagnosis = healthRecordDto.Diagnosis.Trim(),
                Prescription = healthRecordDto.Prescription.Trim(),
                Notes = (healthRecordDto.Notes ?? string.Empty).Trim(),
                UpdatedDate = null
            };

            _context.HealthRecords.Add(healthRecord);

            appointment.Status = AppointmentStatus.Completed;

            await _context.SaveChangesAsync();

            var savedRecord = await GetByIdAsync(healthRecord.HealthRecordId);

            if (savedRecord == null)
            {
                throw new NotFoundException("Health record not found after saving");
            }

            return savedRecord;
        }

        public async Task<HealthRecordDto?> UpdateAsync(
            int id,
            UpdateHealthRecordDto healthRecordDto,
            int loggedInDoctorId)
        {
            ArgumentNullException.ThrowIfNull(healthRecordDto);

            ValidateUpdatedHealthRecordDetails(
                healthRecordDto.Diagnosis,
                healthRecordDto.Prescription);

            var record = await _context.HealthRecords
                .FirstOrDefaultAsync(item => item.HealthRecordId == id);

            if (record == null)
            {
                return null;
            }

            if (record.DoctorId != loggedInDoctorId)
            {
                throw new BusinessRuleException(
                    "You cannot edit another doctor's health record");
            }

            record.Diagnosis = healthRecordDto.Diagnosis.Trim();
            record.Prescription = healthRecordDto.Prescription.Trim();
            record.Notes = (healthRecordDto.Notes ?? string.Empty).Trim();
            record.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        private static void ValidateNewHealthRecordDetails(
            DateTime visitDate,
            string? diagnosis,
            string? prescription)
        {
            if (visitDate == default)
            {
                throw new ValidationExceptions("Visit date is required");
            }

            if (visitDate.Date > DateTime.Today)
            {
                throw new ValidationExceptions(
                    "Visit date cannot be in the future");
            }

            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new ValidationExceptions("Diagnosis is required");
            }

            if (string.IsNullOrWhiteSpace(prescription))
            {
                throw new ValidationExceptions("Prescription is required");
            }
        }

        private static void ValidateUpdatedHealthRecordDetails(
            string? diagnosis,
            string? prescription)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new ValidationExceptions("Diagnosis is required");
            }

            if (string.IsNullOrWhiteSpace(prescription))
            {
                throw new ValidationExceptions("Prescription is required");
            }
        }
    }
}