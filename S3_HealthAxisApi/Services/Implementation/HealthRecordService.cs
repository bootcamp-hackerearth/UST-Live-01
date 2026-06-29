using S3_HealthAxis.Shared.DTOs.HealthRecord;
using S3_HealthAxis.Shared.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Interface;

namespace S3_HealthAxisApi.Services.Implementation
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

        public async Task<HealthRecordDto?> GetByIdAsync(int id)
        {
            var record = await _healthRecordRepository.GetByIdAsync(id);

            return record == null
                ? null
                : MapToDto(record);
        }

        public async Task<HealthRecordDto?> GetByAppointmentIdAsync(int appointmentId)
        {
            var record =
                await _healthRecordRepository.GetByAppointmentIdAsync(appointmentId);

            return record == null
                ? null
                : MapToDto(record);
        }

        public async Task<IEnumerable<HealthRecordDto>> GetByPatientIdAsync(int patientId)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new KeyNotFoundException(
                    $"Patient with Id {patientId} not found.");
            }

            var records =
                await _healthRecordRepository.GetByPatientIdAsync(patientId);

            return records.Select(MapToDto);
        }

        public async Task<HealthRecordDto> CreateAsync(CreateHealthRecordDto dto)
        {
            ValidateCreateDto(dto);

            var appointment =
                await _appointmentRepository.GetByIdAsync(dto.AppointmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found.");
            }

            if (appointment.Status != AppointmentStatus.Completed)
            {
                throw new InvalidOperationException(
                    "Health record can only be created for completed appointments.");
            }

            var existingRecord =
                await _healthRecordRepository.GetByAppointmentIdAsync(
                    dto.AppointmentId);

            if (existingRecord != null)
            {
                throw new InvalidOperationException(
                    "A health record already exists for this appointment.");
            }

            var patient =
                await _patientRepository.GetByIdAsync(dto.PatientId);

            if (patient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            var doctor =
                await _doctorRepository.GetByIdAsync(dto.DoctorId);

            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }

            if (appointment.PatientId != dto.PatientId)
            {
                throw new InvalidOperationException(
                    "Patient does not match appointment.");
            }

            if (appointment.DoctorId != dto.DoctorId)
            {
                throw new InvalidOperationException(
                    "Doctor does not match appointment.");
            }

            var record = new HealthRecord
            {
                AppointmentId = dto.AppointmentId,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                Diagnosis = dto.Diagnosis!.Trim(),
                Prescription = dto.Prescription!.Trim(),
                Notes = dto.Notes?.Trim(),
                CreatedOn = DateTime.UtcNow
            };

            await _healthRecordRepository.AddAsync(record);
            await _healthRecordRepository.SaveChangesAsync();

            var createdRecord =
                await _healthRecordRepository.GetByIdAsync(record.HealthRecordId);

            return MapToDto(createdRecord ?? record);
        }

        public async Task UpdateAsync(
            int id,
            UpdateHealthRecordDto dto)
        {
            ValidateUpdateDto(dto);

            var record =
                await _healthRecordRepository.GetByIdAsync(id);

            if (record == null)
            {
                throw new KeyNotFoundException(
                    $"Health record {id} not found.");
            }

            record.Diagnosis = dto.Diagnosis!.Trim();
            record.Prescription = dto.Prescription!.Trim();
            record.Notes = dto.Notes?.Trim();

            await _healthRecordRepository.UpdateAsync(record);
            await _healthRecordRepository.SaveChangesAsync();
        }

        private static void ValidateCreateDto(
            CreateHealthRecordDto dto)
        {
            if (dto.AppointmentId <= 0)
            {
                throw new ArgumentException(
                    "AppointmentId is required.");
            }

            if (dto.PatientId <= 0)
            {
                throw new ArgumentException(
                    "PatientId is required.");
            }

            if (dto.DoctorId <= 0)
            {
                throw new ArgumentException(
                    "DoctorId is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Diagnosis))
            {
                throw new ArgumentException(
                    "Diagnosis is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Prescription))
            {
                throw new ArgumentException(
                    "Prescription is required.");
            }
        }

        private static void ValidateUpdateDto(
            UpdateHealthRecordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Diagnosis))
            {
                throw new ArgumentException(
                    "Diagnosis is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Prescription))
            {
                throw new ArgumentException(
                    "Prescription is required.");
            }
        }

        private static HealthRecordDto MapToDto(
            HealthRecord record)
        {
            return new HealthRecordDto
            {
                HealthRecordId = record.HealthRecordId,
                AppointmentId = record.AppointmentId,
                PatientId = record.PatientId,
                DoctorId = record.DoctorId,
                DoctorName = record.Doctor?.FullName ?? string.Empty,
                DoctorSpecialisation = record.Doctor == null
                    ? 0
                    : (int)record.Doctor.Specialisation,
                CreatedOn = record.CreatedOn,
                Diagnosis = record.Diagnosis,
                Prescription = record.Prescription,
                Notes = record.Notes
            };
        }
    }
}

