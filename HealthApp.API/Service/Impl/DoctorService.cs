using AutoMapper;
using HealthApp.API.Models;
using HealthApp.API.Models.DTOs;
using HealthApp.API.Repository.Impl;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
namespace HealthApp.API.Service.Impl
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper) : IDoctorService
    {
        public async Task<DoctorDto> AddAsync(DoctorDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            var savedEntity = await repository.AddAsync(doctor);
            return mapper.Map<DoctorDto>(savedEntity);
        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetAllAsync());
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            return mapper.Map<DoctorDto>(await repository.GetByIdAsync(id));
        }

        public async Task<DoctorDto> UpdateAsync(int id, DoctorDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            doctor.DoctorId = id;
            var updated = await repository.UpdateAsync(id, doctor);
            return mapper.Map<DoctorDto>(updated);
        }
    }
}
