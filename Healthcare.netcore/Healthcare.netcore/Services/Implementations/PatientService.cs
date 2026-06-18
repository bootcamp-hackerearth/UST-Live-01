using AutoMapper;
using HealthAxis.API.DTOs;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;

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

        // ✅ Get All
        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            var patients = await _patientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        // ✅ Get By Id
        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
                throw new NotFoundException("Patient not found");

            return _mapper.Map<PatientDto>(patient);
        }

        // ✅ Update
        public async Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
                throw new NotFoundException("Patient not found");

            _mapper.Map(dto, patient);

            await _patientRepository.UpdateAsync(id, patient, CancellationToken.None);

            return _mapper.Map<PatientDto>(patient);
        }

        // ✅ Get Health Records
        public async Task<IEnumerable<HealthRecordDto>> GetHealthRecordsAsync(int patientId)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            if (patient == null)
                throw new NotFoundException("Patient not found");

            var records = await _healthRecordRepository.GetAllAsync();

            var patientRecords = records
                .Where(r => r.PatientId == patientId);

            return _mapper.Map<IEnumerable<HealthRecordDto>>(patientRecords);
        }

        // ✅ SEARCH BY NAME (done in service ✅)
        public async Task<IEnumerable<PatientDto>> SearchByNameAsync(string name)
        {
            var patients = await _patientRepository.GetAllAsync();

            var result = patients
                .Where(p => p.FullName.Contains(name, StringComparison.OrdinalIgnoreCase));

            return _mapper.Map<IEnumerable<PatientDto>>(result);
        }

        // ✅ GET BY EMAIL
        public async Task<PatientDto?> GetByEmailAsync(string email)
        {
            var patients = await _patientRepository.GetAllAsync();

            var patient = patients
                .FirstOrDefault(p => p.Email == email);

            if (patient == null)
                throw new NotFoundException("Patient not found");

            return _mapper.Map<PatientDto>(patient);
        }

        // ✅ GET BY PHONE
        public async Task<PatientDto?> GetByPhoneAsync(string phone)
        {
            var patients = await _patientRepository.GetAllAsync();

            var patient = patients
                .FirstOrDefault(p => p.PhoneNumber == phone);

            if (patient == null)
                throw new NotFoundException("Patient not found");

            return _mapper.Map<PatientDto>(patient);
        }
    }
}