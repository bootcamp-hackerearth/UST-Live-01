using AutoMapper;
using HealthApp.Api.Dto;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;

namespace HealthApp.Api.Service.Impl
{
    public class HealthRecordService : IHealthRecordService
    {
        private IHealthRecordRepository _repo;
        private IMapper _mapper;

        public HealthRecordService(IHealthRecordRepository repo,IMapper mapper)
        {
            _repo=repo;
            _mapper=mapper;
            
        }
        public async Task<HealthRecordDto> AddRecordAsync(HealthRecordDto dto)
        {
            var h = _mapper.Map<HealthRecord>(dto);
            var savedh = await _repo.addasync(h);
            return _mapper.Map<HealthRecordDto>(savedh);
        }

        public async Task<List<HealthRecordDto>> GetAllRecordsAsync()
        {
            var savedh = await _repo.getallasync();
            return _mapper.Map<List<HealthRecordDto>>(savedh);
        }

        public async Task<List<HealthRecordDto>?> GetHealthRecordsByDoctorAsync(int? doctorId, int? patientId)
        {
            var h = _mapper.Map<HealthRecord>(doctorId);
            var savedh = await _repo.GetHealthRecordsByDoctorAndPatientAsync(doctorId, patientId);
            return _mapper.Map<List<HealthRecordDto>>(savedh);
        }


    }
}
