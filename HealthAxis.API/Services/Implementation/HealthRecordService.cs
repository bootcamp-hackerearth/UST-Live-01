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

        private readonly INotificationService?
            _notificationService;

        public HealthRecordService(
            ApplicationDbContext context,
            INotificationService? notificationService = null)
        {
            ArgumentNullException.ThrowIfNull(context);

            _context = context;
            _notificationService = notificationService;
        }

        public async Task<List<HealthRecordDto>>
            GetByPatientIdAsync(int patientId)
        {
            if (patientId <= 0)
            {
                return new List<HealthRecordDto>();
            }

            var patientExists = await _context.Patients
                .AsNoTracking()
                .AnyAsync(patient =>
                    patient.PatientId == patientId);

            if (!patientExists)
            {
                return new List<HealthRecordDto>();
            }

            /*
             * Records are loaded first and then mapped in memory.
             * EF Core cannot translate the custom mapping method
             * directly into SQL.
             */
            var records = await _context.HealthRecords
                .AsNoTracking()
                .Include(record => record.Patient)
                .Include(record => record.Doctor)
                .Where(record =>
                    record.PatientId == patientId)
                .OrderByDescending(record =>
                    record.CreatedAt ??
                    record.VisitDate)
                .ToListAsync();

            return records
                .Select(MapHealthRecord)
                .ToList();
        }

        public async Task<List<HealthRecordDto>>
            GetByDoctorIdAsync(int doctorId)
        {
            if (doctorId <= 0)
            {
                return new List<HealthRecordDto>();
            }

            var records = await _context.HealthRecords
                .AsNoTracking()
                .Include(record => record.Patient)
                .Include(record => record.Doctor)
                .Where(record =>
                    record.DoctorId == doctorId)
                .OrderByDescending(record =>
                    record.CreatedAt ??
                    record.VisitDate)
                .ToListAsync();

            return records
                .Select(MapHealthRecord)
                .ToList();
        }

        public async Task<HealthRecordDto?>
            GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            var record = await _context.HealthRecords
                .AsNoTracking()
                .Include(healthRecord =>
                    healthRecord.Patient)
                .Include(healthRecord =>
                    healthRecord.Doctor)
                .FirstOrDefaultAsync(healthRecord =>
                    healthRecord.HealthRecordId == id);

            if (record == null)
            {
                return null;
            }

            return MapHealthRecord(record);
        }

        public async Task<HealthRecordDto> AddAsync(
            CreateHealthRecordDto healthRecordDto,
            int loggedInDoctorId)
        {
            ArgumentNullException.ThrowIfNull(
                healthRecordDto);

            ValidateNewHealthRecordDetails(
                healthRecordDto.VisitDate,
                healthRecordDto.Diagnosis,
                healthRecordDto.Prescription);

            var appointment =
                await _context.Appointments
                    .FirstOrDefaultAsync(item =>
                        item.AppointmentId ==
                        healthRecordDto.AppointmentId);

            if (appointment == null)
            {
                throw new NotFoundException(
                    "Appointment not found.");
            }

            if (appointment.DoctorId !=
                loggedInDoctorId)
            {
                throw new BusinessRuleException(
                    "You cannot add a health record for another doctor's appointment.");
            }

            if (appointment.Status !=
                AppointmentStatus.Confirmed)
            {
                throw new BusinessRuleException(
                    "Health record can be added only for confirmed appointments.");
            }

            var recordAlreadyExists =
                await _context.HealthRecords
                    .AnyAsync(record =>
                        record.AppointmentId ==
                        healthRecordDto.AppointmentId);

            if (recordAlreadyExists)
            {
                throw new BusinessRuleException(
                    "Health record already exists for this appointment.");
            }

            var healthRecord = new HealthRecord
            {
                AppointmentId =
                    appointment.AppointmentId,

                PatientId =
                    appointment.PatientId,

                DoctorId =
                    appointment.DoctorId,

                VisitDate =
                    healthRecordDto.VisitDate.Date,

                Diagnosis =
                    healthRecordDto.Diagnosis.Trim(),

                Prescription =
                    healthRecordDto.Prescription.Trim(),

                Notes =
                    NormalizeOptionalText(
                        healthRecordDto.Notes),

                /*
                 * This stores the exact UTC time at which
                 * the health record was created.
                 */
                CreatedAt = DateTime.UtcNow,

                UpdatedDate = null
            };

            _context.HealthRecords.Add(
                healthRecord);

            /*
             * A health record is added after the consultation.
             * Therefore, the appointment becomes completed.
             */
            appointment.Complete();

            /*
             * The health record and appointment status are saved
             * together in the same database operation.
             */
            await _context.SaveChangesAsync();

            /*
             * The appointment is already completed successfully.
             * Now create the patient notification.
             *
             * NotificationService handles duplicate notifications
             * and prevents a notification error from undoing the
             * saved health record.
             */
            await CreateCompletionNotificationAsync(
                appointment);

            var savedRecord = await GetByIdAsync(
                healthRecord.HealthRecordId);

            if (savedRecord == null)
            {
                throw new NotFoundException(
                    "Health record was saved, but it could not be loaded.");
            }

            return savedRecord;
        }

        public async Task<HealthRecordDto?> UpdateAsync(
            int id,
            UpdateHealthRecordDto healthRecordDto,
            int loggedInDoctorId)
        {
            ArgumentNullException.ThrowIfNull(
                healthRecordDto);

            if (id <= 0)
            {
                return null;
            }

            ValidateUpdatedHealthRecordDetails(
                healthRecordDto.Diagnosis,
                healthRecordDto.Prescription);

            var record = await _context.HealthRecords
                .FirstOrDefaultAsync(item =>
                    item.HealthRecordId == id);

            if (record == null)
            {
                return null;
            }

            if (record.DoctorId !=
                loggedInDoctorId)
            {
                throw new BusinessRuleException(
                    "You cannot edit another doctor's health record.");
            }

            record.Diagnosis =
                healthRecordDto.Diagnosis.Trim();

            record.Prescription =
                healthRecordDto.Prescription.Trim();

            record.Notes =
                NormalizeOptionalText(
                    healthRecordDto.Notes);

            record.UpdatedDate =
                DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        private async Task
            CreateCompletionNotificationAsync(
                Appointment appointment)
        {
            /*
             * Existing unit tests may create the service using
             * only ApplicationDbContext. In that case, notification
             * creation is safely skipped.
             */
            if (_notificationService == null)
            {
                return;
            }

            await _notificationService
                .CreateAppointmentStatusNotificationsAsync(
                    appointment);
        }

        private static HealthRecordDto
            MapHealthRecord(HealthRecord record)
        {
            var patientName =
                record.Patient == null
                    ? string.Empty
                    : record.Patient.FullName;

            var doctorName =
                record.Doctor == null
                    ? string.Empty
                    : record.Doctor.FullName;

            var specialisation =
                record.Doctor == null
                    ? string.Empty
                    : record.Doctor
                        .Specialisation
                        .ToString();

            return new HealthRecordDto
            {
                HealthRecordId =
                    record.HealthRecordId,

                RecordId =
                    record.HealthRecordId,

                AppointmentId =
                    record.AppointmentId,

                PatientId =
                    record.PatientId,

                PatientName =
                    patientName,

                DoctorId =
                    record.DoctorId,

                DoctorName =
                    doctorName,

                Specialisation =
                    specialisation,

                VisitDate =
                    record.VisitDate,

                CreatedAt =
                    record.CreatedAt,

                UpdatedDate =
                    record.UpdatedDate,

                Diagnosis =
                    record.Diagnosis ??
                    string.Empty,

                Prescription =
                    record.Prescription ??
                    string.Empty,

                Notes =
                    record.Notes ??
                    string.Empty
            };
        }

        private static string NormalizeOptionalText(
            string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value.Trim();
        }

        private static void
            ValidateNewHealthRecordDetails(
                DateTime visitDate,
                string? diagnosis,
                string? prescription)
        {
            if (visitDate == default)
            {
                throw new ValidationException(
                    "Visit date is required.");
            }

            if (visitDate.Date > DateTime.Today)
            {
                throw new ValidationException(
                    "Visit date cannot be in the future.");
            }

            ValidateRequiredMedicalDetails(
                diagnosis,
                prescription);
        }

        private static void
            ValidateUpdatedHealthRecordDetails(
                string? diagnosis,
                string? prescription)
        {
            ValidateRequiredMedicalDetails(
                diagnosis,
                prescription);
        }

        private static void
            ValidateRequiredMedicalDetails(
                string? diagnosis,
                string? prescription)
        {
            if (string.IsNullOrWhiteSpace(
                diagnosis))
            {
                throw new ValidationException(
                    "Diagnosis is required.");
            }

            if (string.IsNullOrWhiteSpace(
                prescription))
            {
                throw new ValidationException(
                    "Prescription is required.");
            }
        }
    }
}