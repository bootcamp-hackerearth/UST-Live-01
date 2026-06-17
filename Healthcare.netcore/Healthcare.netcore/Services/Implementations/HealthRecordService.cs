using AutoMapper;
using HealthAxis.API.DTOs;
using HealthAxis.API.Enums;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using CustomValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Services.Implementations
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IRepository<HealthRecord> _healthRecordRepository;
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IMapper _mapper;

        public HealthRecordService(
            IRepository<HealthRecord> healthRecordRepository,
            IRepository<Appointment> appointmentRepository,
            IRepository<Patient> patientRepository,
            IMapper mapper)
        {
            _healthRecordRepository = healthRecordRepository;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HealthRecordDto>> GetByPatientIdAsync(int patientId)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found.");
            }

            var appointments = await _appointmentRepository.GetAllAsync();

            var patientAppointmentIds = appointments
                .Where(a => a.PatientId == patientId)
                .Select(a => a.AppointmentId)
                .ToHashSet();

            var healthRecords = await _healthRecordRepository.GetAllAsync();

            var records = healthRecords
                .Where(hr => patientAppointmentIds.Contains(hr.AppointmentId));

            return _mapper.Map<IEnumerable<HealthRecordDto>>(records);
        }

        public async Task<HealthRecordDto> GetByIdAsync(int id)
        {
            var healthRecord = await _healthRecordRepository.GetByIdAsync(id);

            if (healthRecord == null)
            {
                throw new NotFoundException("Health record not found.");
            }

            return _mapper.Map<HealthRecordDto>(healthRecord);
        }

        public async Task<HealthRecordDto> AddAsync(CreateHealthRecordDto dto)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(dto.AppointmentId);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.Status != AppointmentStatus.Completed)
            {
                throw new CustomValidationException(
                    "Health records can only be created for completed appointments.");
            }

            var records = await _healthRecordRepository.GetAllAsync();

            bool exists = records.Any(hr => hr.AppointmentId == dto.AppointmentId);

            if (exists)
            {
                throw new CustomValidationException(
                    "Health record already exists for this appointment.");
            }

            var healthRecord = new HealthRecord
            {
                AppointmentId = dto.AppointmentId,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                VisitDate = appointment.ScheduledDate,
                Diagnosis = dto.Diagnosis,
                Prescription = dto.Prescription,
                Notes = dto.Notes
            };

            await _healthRecordRepository.AddAsync(healthRecord);

            return _mapper.Map<HealthRecordDto>(healthRecord);
        }
    }
}