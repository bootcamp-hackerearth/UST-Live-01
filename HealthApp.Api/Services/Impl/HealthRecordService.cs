using AutoMapper;
using HealthApp.Api.Dtos;
using HealthApp.Api.Enums;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;

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

            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);

            if (patient == null)
            {
                throw new EntityNotFoundException("Patient", dto.PatientId);
            }

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", dto.DoctorId);
            }

            Appointment? appointment = null;

            if (dto.AppointmentId.HasValue)
            {
                appointment = await _appointmentRepository.GetByIdAsync(dto.AppointmentId.Value);

                if (appointment == null)
                {
                    throw new EntityNotFoundException("Appointment", dto.AppointmentId.Value);
                }

                if (appointment.PatientId != dto.PatientId)
                {
                    throw new BusinessRuleViolationException(
                        "Appointment does not belong to the selected patient.");
                }

                if (appointment.DoctorId != dto.DoctorId)
                {
                    throw new BusinessRuleViolationException(
                        "Appointment does not belong to the selected doctor.");
                }

                if (appointment.Status != AppointmentStatus.Confirmed)
                {
                    throw new BusinessRuleViolationException(
                        "Health record can be added only for confirmed appointments.");
                }

                bool recordExists = await ExistsByAppointmentIdAsync(dto.AppointmentId.Value);

                if (recordExists)
                {
                    throw new DuplicateEntityException(
                        "Health record already exists for this appointment.");
                }
            }

            var record = _mapper.Map<HealthRecord>(dto);

            record.PatientId = dto.PatientId;
            record.DoctorId = dto.DoctorId;
            record.AppointmentId = dto.AppointmentId;
            record.VisitDate = dto.VisitDate;
            record.Diagnosis = dto.Diagnosis.Trim();
            record.Prescription = dto.Prescription.Trim();
            record.Notes = dto.Notes?.Trim();

            await _healthRecordRepository.Add(record);

            if (appointment != null)
            {
                appointment.Status = AppointmentStatus.Completed;

                await _appointmentRepository.Update(
                    appointment.AppointmentId,
                    appointment);
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

        private async Task LoadHealthRecordNavigationDataAsync(HealthRecord record)
        {
            if (record.Patient == null && record.PatientId.HasValue)
            {
                record.Patient = await _patientRepository.GetByIdAsync(record.PatientId.Value);
            }

            if (record.Doctor == null && record.DoctorId.HasValue)
            {
                record.Doctor = await _doctorRepository.GetByIdAsync(record.DoctorId.Value);
            }

            if (record.Appointment == null && record.AppointmentId.HasValue)
            {
                record.Appointment = await _appointmentRepository.GetByIdAsync(
                    record.AppointmentId.Value);
            }
        }
    }
}