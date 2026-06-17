using AutoMapper;
using HealthApp.Api.Dto;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;

namespace HealthApp.Api.Service.Impl
{
    public class DoctorService : IDoctorService
    {
        private IDoctorRepository _repo;
        private IMapper _mapper;

        public DoctorService(IDoctorRepository repo,IMapper mapper)
        {
            _repo=repo;
            _mapper=mapper;
        }
        public async Task<DoctorDto> AddDoctorAsync(DoctorDto dto)
        {
            var d=_mapper.Map<Doctor>(dto);
            var savedd= await _repo.addasync(d);
            return _mapper.Map<DoctorDto>(savedd);
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var savedd= await _repo.getallasync();
            return _mapper.Map<List<DoctorDto>>(savedd);
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int id)
        {
            var d = _mapper.Map<Doctor>(id);
            var savedd = await _repo.getbyidasync(d.DoctorId);
            return _mapper.Map<DoctorDto>(savedd);
        }

        public async Task<List<DoctorDto>> SearchBySpecialisationAsync(string specialisation)
        {
            var d = _mapper.Map<Doctor>(specialisation);
            var savedd = await _repo.searchbyspecialisationAsync(d.Specialisation);
            return _mapper.Map<List<DoctorDto>>(savedd);
        }

        public async Task<DoctorDto> UpdateDoctorByIdAsync(int id, DoctorDto doctorDto)
        {
            var d = _mapper.Map<Doctor>(doctorDto);
            var savedd =await _repo.updateasync(id, d);
            return _mapper.Map<DoctorDto>(savedd);
        }
    }
}
