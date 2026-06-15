using AutoMapper;

using HealthCareApp.Dtos;

using HealthCareApp.Models;

using HealthCareApp.Repository.Interface;

using HealthCareApp.Enums;

namespace HealthCareApp.Services
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper) : IDoctorService
    {
        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await repository.GetAllAsync();

            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<List<DoctorDto>> GetAllActiveDoctorsAsync()
        {
            var doctors = await repository.GetAllActiveAsync();

            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int doctorId)
        {
            var doctor = await repository.GetByIdAsync(doctorId);

            if (doctor is null)
            {
                throw new Exception("Doctor not found.");
            }

            return mapper.Map<DoctorDto>(doctor);
        }

        public async Task<List<DoctorDto>> GetDoctorsBySpecialisationAsync(SpecialisationType specialisation)
        {
            var doctors = await repository.GetBySpecialisationAsync(specialisation);

            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<List<DoctorDto>> GetActiveDoctorsBySpecialisationAsync(SpecialisationType specialisation)
        {
            var doctors = await repository.GetActiveBySpecialisationAsync(specialisation);

            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto)
        {
            var doctor = mapper.Map<Doctor>(dto);

            doctor.IsActive = true;

            var savedDoctor = await repository.CreateAsync(doctor);

            return mapper.Map<DoctorDto>(savedDoctor);
        }

        public async Task<DoctorDto> UpdateDoctorAsync(int doctorId, UpdateDoctorDto dto)
        {
            var doctor = mapper.Map<Doctor>(dto);

            doctor.DoctorId = doctorId;

            var updatedDoctor = await repository.UpdateAsync(doctorId, doctor);

            if (updatedDoctor is null)
            {
                throw new Exception("Doctor not found.");
            }

            return mapper.Map<DoctorDto>(updatedDoctor);
        }

        public async Task<DoctorDto> DeleteDoctorAsync(int doctorId)
        {
            var deletedDoctor = await repository.DeleteAsync(doctorId);

            if (deletedDoctor is null)
            {
                throw new Exception("Doctor not found.");
            }

            return mapper.Map<DoctorDto>(deletedDoctor);
        }
    }
}