using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;

namespace HealthApp.Api.Services.Impl
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

        public async Task<IEnumerable<HealthRecordDto>> GetAllAsync()
        {
            var records = await _healthRecordRepository.GetHealthRecordsAsync();

            foreach (var record in records)
            {
                await LoadHealthRecordNavigationDataAsync(record);
            }

            return _mapper.Map<IEnumerable<HealthRecordDto>>(records);
        }

        public async Task<HealthRecordDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidRequestException("Valid health record id is required.");
            }

            var record = await _healthRecordRepository.GetByIdAsync(id);

            if (record == null)
            {
                throw new EntityNotFoundException("Health record", id);
            }

            await LoadHealthRecordNavigationDataAsync(record);

            return _mapper.Map<HealthRecordDto>(record);
        }

        public async Task AddAsync(HealthRecordCreateDto dto)
        {
            ValidateHealthRecordCreateDto(dto);

            await EnsurePatientExistsAsync(dto.PatientId);
            await EnsureDoctorExistsAsync(dto.DoctorId);

            var appointment = await ValidateAppointmentForHealthRecordAsync(dto);

            var record = CreateHealthRecord(dto);

            await _healthRecordRepository.Add(record);

            if (appointment != null)
            {
                await CompleteAppointmentAsync(appointment);
            }
        }

        public async Task<IEnumerable<HealthRecordDto>> GetPatientHistoryAsync(int patientId)
        {
            return await GetByPatientIdAsync(patientId);
        }

        public async Task<IEnumerable<HealthRecordDto>> GetByPatientIdAsync(int patientId)
        {
            if (patientId <= 0)
            {
                throw new InvalidRequestException("Valid patient is required.");
            }

            var records = await _healthRecordRepository.GetHealthRecordsAsync(
                patientId,
                null);

            foreach (var record in records)
            {
                await LoadHealthRecordNavigationDataAsync(record);
            }

            return _mapper.Map<IEnumerable<HealthRecordDto>>(records);
        }

        public async Task<bool> ExistsByAppointmentIdAsync(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                throw new InvalidRequestException("Valid appointment is required.");
            }

            var records = await _healthRecordRepository.GetHealthRecordsAsync(
                null,
                appointmentId);

            return records.Any();
        }

        private static void ValidateHealthRecordCreateDto(HealthRecordCreateDto dto)
        {
            if (dto == null)
            {
                throw new InvalidRequestException("Health record data is required.");
            }

            if (dto.PatientId <= 0)
            {
                throw new InvalidRequestException("Valid patient is required.");
            }

            if (dto.DoctorId <= 0)
            {
                throw new InvalidRequestException("Valid doctor is required.");
            }

            if (dto.VisitDate == default)
            {
                throw new InvalidRequestException("Visit date is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Diagnosis))
            {
                throw new InvalidRequestException("Diagnosis is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Prescription))
            {
                throw new InvalidRequestException("Prescription is required.");
            }
        }

        private async Task EnsurePatientExistsAsync(int patientId)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }
        }

        private async Task EnsureDoctorExistsAsync(int doctorId)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }
        }

        private async Task<Appointment?> ValidateAppointmentForHealthRecordAsync(
            HealthRecordCreateDto dto)
        {
            if (!dto.AppointmentId.HasValue)
            {
                return null;
            }

            var appointment = await GetAppointmentOrThrowAsync(
                dto.AppointmentId.Value);

            ValidateAppointmentOwnership(
                appointment,
                dto.PatientId,
                dto.DoctorId);

            await EnsureAppointmentCanHaveHealthRecordAsync(appointment);

            return appointment;
        }

        private async Task<Appointment> GetAppointmentOrThrowAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                throw new EntityNotFoundException("Appointment", appointmentId);
            }

            return appointment;
        }

        private static void ValidateAppointmentOwnership(
            Appointment appointment,
            int patientId,
            int doctorId)
        {
            if (appointment.PatientId != patientId)
            {
                throw new BusinessRuleViolationException(
                    "Appointment does not belong to the selected patient.");
            }

            if (appointment.DoctorId != doctorId)
            {
                throw new BusinessRuleViolationException(
                    "Appointment does not belong to the selected doctor.");
            }
        }

        private async Task EnsureAppointmentCanHaveHealthRecordAsync(
            Appointment appointment)
        {
            if (appointment.Status != AppointmentStatus.Confirmed)
            {
                throw new BusinessRuleViolationException(
                    "Health record can be added only for confirmed appointments.");
            }

            bool recordExists = await ExistsByAppointmentIdAsync(
                appointment.AppointmentId);

            if (recordExists)
            {
                throw new DuplicateEntityException(
                    "Health record already exists for this appointment.");
            }
        }

        private HealthRecord CreateHealthRecord(HealthRecordCreateDto dto)
        {
            var record = _mapper.Map<HealthRecord>(dto);

            record.PatientId = dto.PatientId;
            record.DoctorId = dto.DoctorId;
            record.AppointmentId = dto.AppointmentId;
            record.VisitDate = (DateOnly)dto.VisitDate!;
            record.Diagnosis = dto.Diagnosis.Trim();
            record.Prescription = dto.Prescription.Trim();
            record.Notes = dto.Notes?.Trim();

            return record;
        }

        private async Task CompleteAppointmentAsync(Appointment appointment)
        {
            appointment.Status = AppointmentStatus.Completed;

            await _appointmentRepository.Update(
                appointment.AppointmentId,
                appointment);
        }

        private async Task LoadHealthRecordNavigationDataAsync(HealthRecord record)
        {
            if (record.Patient == null && record.PatientId.HasValue)
            {
                record.Patient = await _patientRepository.GetByIdAsync(
                    record.PatientId.Value);
            }

            if (record.Doctor == null && record.DoctorId.HasValue)
            {
                record.Doctor = await _doctorRepository.GetByIdAsync(
                    record.DoctorId.Value);
            }

            if (record.Appointment == null && record.AppointmentId.HasValue)
            {
                record.Appointment = await _appointmentRepository.GetByIdAsync(
                    record.AppointmentId.Value);
            }
        }
    }
}