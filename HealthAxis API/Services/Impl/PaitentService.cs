using AutoMapper;
using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.DTOs.Patients;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;

namespace HealthAxis.API.Services
{
    public class PatientService
        : Service<Patient, PatientReadDto, PatientCreateDto, PatientUpdateDto>,
          IPatientService
    {
        private readonly IHealthRecordRepository _healthRecordRepository;
        private readonly IMapper _mapper;

        public PatientService(
            IPatientRepository patientRepository,
            IHealthRecordRepository healthRecordRepository,
            IMapper mapper)
            : base(patientRepository, mapper)
        {
            _healthRecordRepository = healthRecordRepository;
            _mapper = mapper;
        }

        public async Task<List<HealthRecordReadDto>> GetHealthRecordsAsync(
            int patientId,
            CancellationToken ct = default)
        {
            List<HealthRecord> healthRecords =
                await _healthRecordRepository.GetByPatientIdAsync(patientId, ct);

            return _mapper.Map<List<HealthRecordReadDto>>(healthRecords);
        }
    }
}
