using AutoMapper;
using HealthAxisApplicn.Mappings;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Models.Dto;
using HealthAxisApplicn.Repositories;

namespace HealthAxisApplicn.Services.Impl
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper) : IDoctorService
    {
        public async Task<DoctorDto> CreateAsync(DoctorDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            var savedEntity = await repository.CreateAsync(doctor);
            return mapper.Map<DoctorDto>(savedEntity);
        }

        public async Task<DoctorDto> DeactivateDoctorAsync(int id)
        {
            var doctor = mapper.Map<Doctor>(await repository.GetByIdAsync(id));
            doctor.IsActive = false;
            return mapper.Map<DoctorDto>(doctor);
        }

        public async Task<List<DoctorDto?>> GetAllAsync()
        {
            return mapper.Map<List<DoctorDto?>>(await repository.GetAllAsync());
        }

        public async Task<List<DoctorDto>> GetAvailableDoctorsAsync()
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetAvailableDoctorsAsync());
        }

        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            var doctor = await repository.GetByIdAsync(id);
            return mapper.Map<DoctorDto?>(doctor);
        }

        public async Task<List<DoctorDto>> SearchBySpecialisationAsync(string specialisation)
        {
            var doctors = await repository.SearchBySpecialisationAsync(specialisation);
            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<List<DoctorDto>> SearchDoctorByNameAsync(string name)
        {
            var doctors = await repository.SearchDoctorByNameAsync(name);
            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto?> UpdatebyAsync(int id, DoctorDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            var updatedDoctor = await repository.UpdatebyAsync(id, doctor);
            return mapper.Map<DoctorDto?>(updatedDoctor);
        }
    }
}
