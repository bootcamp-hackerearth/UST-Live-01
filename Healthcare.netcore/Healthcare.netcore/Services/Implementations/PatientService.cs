using AutoMapper;
using HealthAxis.API.DTOs;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using CustomValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<HealthRecord> _healthRecordRepository;
        private readonly IMapper _mapper;

        public PatientService(
            IRepository<Patient> patientRepository,
            IRepository<Appointment> appointmentRepository,
            IRepository<HealthRecord> healthRecordRepository,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _healthRecordRepository = healthRecordRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            var patients = await _patientRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found.");
            }

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto)
        {
            var existingPatient = await _patientRepository.GetByIdAsync(id);

            if (existingPatient == null)
            {
                throw new NotFoundException("Patient not found.");
            }

            _mapper.Map(dto, existingPatient);

            await _patientRepository.UpdateAsync(
                id,
                existingPatient,
                CancellationToken.None);

            return _mapper.Map<PatientDto>(existingPatient);
        }

        public async Task<IEnumerable<HealthRecordDto>> GetHealthRecordsAsync(int patientId)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found.");
            }

            var appointments = await _appointmentRepository.GetAllAsync();

            var appointmentIds = appointments
                .Where(a => a.PatientId == patientId)
                .Select(a => a.AppointmentId)
                .ToHashSet();

            var healthRecords = await _healthRecordRepository.GetAllAsync();

            var patientHealthRecords = healthRecords
                .Where(hr => appointmentIds.Contains(hr.AppointmentId));

            return _mapper.Map<IEnumerable<HealthRecordDto>>(patientHealthRecords);
        }
    }
}
