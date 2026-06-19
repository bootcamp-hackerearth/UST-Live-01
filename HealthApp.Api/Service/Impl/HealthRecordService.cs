using AutoMapper;
using HealthApp.Api.Dto;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;
using HealthApp.Api.Exceptions;

namespace HealthApp.Api.Service.Impl
{
    public class HealthRecordService : IHealthRecordService
    {
        private IHealthRecordRepository _repo;
        private IMapper _mapper;

        public HealthRecordService(IHealthRecordRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<HealthRecordDto> AddRecordAsync(HealthRecordDto dto)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(dto.Diagnosis))
                throw new HealthRecordRuleException("Diagnosis is required.");

            if (string.IsNullOrWhiteSpace(dto.Prescription))
                throw new HealthRecordRuleException("Prescription is required.");

            var h = _mapper.Map<HealthRecord>(dto);
            var savedh = await _repo.addAsync(h);

            if (savedh == null)
                throw new HealthRecordRuleException("Unable to create health record.");

            return _mapper.Map<HealthRecordDto>(savedh);
        }

        public async Task<List<HealthRecordDto>> GetAllRecordsAsync()
        {
            var savedh = await _repo.getallAsync();

            return _mapper.Map<List<HealthRecordDto>>(savedh ?? new List<HealthRecord>());
        }

        public async Task<HealthRecordDto?> GetRecordByIdAsync(int id)
        {
            if (id <= 0)
                throw new HealthRecordRuleException("Invalid health record id.");

            var savedh = await _repo.getbyidAsync(id);

            if (savedh == null)
                throw new EntityNotFoundException("HealthRecord", id);

            return _mapper.Map<HealthRecordDto>(savedh);
        }

        public async Task<List<HealthRecordDto>?> GetHealthRecordsByDoctorAsync(int? doctorId, int? patientId)
        {
            if (doctorId == null || doctorId <= 0)
                throw new HealthRecordRuleException("Invalid doctor reference.");

            if (patientId == null || patientId <= 0)
                throw new HealthRecordRuleException("Invalid patient reference.");


            var savedh = await _repo.GetHealthRecordsByDoctorAndPatientAsync(doctorId, patientId);

            if (savedh == null)
                throw new EntityNotFoundException("HealthRecord", 0);

            return _mapper.Map<List<HealthRecordDto>>(savedh);
        }
    }
}