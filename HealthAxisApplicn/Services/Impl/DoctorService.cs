using AutoMapper;
using HealthAxisApplicn.Dto.Doctors;
using HealthAxisApplicn.Mappings;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisApplicn.Services.Impl
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper) : IDoctorService
    {
        public async Task<DoctorDto> CreateAsync(CreateDoctorDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            var savedEntity = await repository.CreateAsync(doctor);
            return mapper.Map<DoctorDto>(savedEntity);
        }

        public async Task<DoctorDto> DeactivateDoctorAsync(int id)
        {

            var existing = await repository.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Doctor Not Found");

            existing.IsActive = false;

            var updated = await repository.UpdateAsync(id, existing);

            return mapper.Map<DoctorDto>(updated);

        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetAllAsync());
        }

        public async Task<List<DoctorDto>> GetActiveDoctorsAsync()
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetActiveDoctorsAsync());
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

        public async Task<List<DoctorDto>> SearchByNameAsync(string name)
        {
            var doctors = await repository.SearchByNameAsync(name);
            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto?> UpdateAsync(int id, UpdateDoctorDto entity)
        {

            var existing = await repository.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Doctor Not Found");

            existing.DoctorName = entity.DoctorName;
            existing.Specialisation = entity.Specialisation;
            existing.YearsOfExperience = entity.YearsOfExperience;
            existing.ConsultationFee = entity.ConsultationFee;
            existing.IsActive = entity.IsActive;

            var updated = await repository.UpdateAsync(id, existing);

            return mapper.Map<DoctorDto?>(updated);

        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            var doctor = await repository.GetByIdAsync(id);

            if (doctor == null)
                return false;

            doctor.IsActive = !doctor.IsActive;

            await repository.UpdateAsync(id, doctor);

            return true;
        }
        public async Task<List<DoctorDto>> SearchAsync(string query)
        {
            var doctors = await repository.SearchAsync(query);
            return mapper.Map<List<DoctorDto>>(doctors);
        }


    }
}
