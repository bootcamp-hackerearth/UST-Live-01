using AutoMapper;
using HealthApp.Api.Dto;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;

namespace HealthApp.Api.Service.Impl
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository repo,IMapper mapper)
        {
            _repo=repo;
            _mapper=mapper;
        }

        public async Task<PatientDto> AddPatientAsync(PatientDto patientDto)
        {
            var p=_mapper.Map<Patient>(patientDto);
            var savedp=await _repo.addasync(p);
            return _mapper.Map<PatientDto>(savedp);

        }

        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            var p = await _repo.getallasync();
            return _mapper.Map<List<PatientDto>>(p);

        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {

            var p = await _repo.getbyidasync(id);
            return _mapper.Map<PatientDto>(p);

        }

        public async Task<PatientDto> UpdatePatientByIdAsync(int id, PatientDto patientDto)
        {
            var p = _mapper.Map<Patient>(patientDto);
            var savedp = await _repo.updateasync(id,p);
            return _mapper.Map<PatientDto>(savedp);
        }
    }
    }
