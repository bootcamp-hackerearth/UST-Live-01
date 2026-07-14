using AutoMapper;
using HealthAxisApplicn.Dto.Doctors;
using HealthAxisApplicn.Mappings;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StackExchange.Redis;
using System.Text.Json;

namespace HealthAxisApplicn.Services.Impl
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper, IConnectionMultiplexer redis) : IDoctorService
    {
        public async Task<DoctorDto> CreateAsync(CreateDoctorDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            var savedEntity = await repository.CreateAsync(doctor);
            await InvalidateActiveDoctorsCache();
            return mapper.Map<DoctorDto>(savedEntity);
        }

        public async Task<DoctorDto> DeactivateDoctorAsync(int id)
        {

            var existing = await repository.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Doctor Not Found");

            existing.IsActive = false;

            var updated = await repository.UpdateAsync(id, existing);
            await InvalidateActiveDoctorsCache();

            return mapper.Map<DoctorDto>(updated);

        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetAllAsync());
        }

        public async Task<List<DoctorDto>> GetActiveDoctorsAsync()
        {
            Log.Information("DOCTOR SERVICE HIT");

            var db = redis.GetDatabase();

            const string cacheKey = "doctors:active";

            var cachedDoctors =
                await db.StringGetAsync(cacheKey);

            if (!cachedDoctors.IsNullOrEmpty)
            {
                Log.Information("CACHE HIT !!");

                return JsonSerializer.Deserialize<List<DoctorDto>>(
                    cachedDoctors.ToString())!;
            }

            Log.Information("CACHE MISS !!");

            var doctors = mapper.Map<List<DoctorDto>>(
                await repository.GetActiveDoctorsAsync());

            await db.StringSetAsync(
                cacheKey,
                JsonSerializer.Serialize(doctors),
                TimeSpan.FromMinutes(5));

            return doctors;
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
            existing.Email = entity.Email;
            existing.YearsOfExperience = entity.YearsOfExperience;
            existing.ConsultationFee = entity.ConsultationFee;
            existing.IsActive = entity.IsActive;

            var updated = await repository.UpdateAsync(id, existing);
            await InvalidateActiveDoctorsCache();

            return mapper.Map<DoctorDto?>(updated);

        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            var doctor = await repository.GetByIdAsync(id);

            if (doctor == null)
                return false;

            doctor.IsActive = !doctor.IsActive;

            await repository.UpdateAsync(id, doctor);
            await InvalidateActiveDoctorsCache();

            return true;
        }
        public async Task<List<DoctorDto>> SearchAsync(string query)
        {
            var doctors = await repository.SearchAsync(query);
            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<Doctor?> GetByUserIdAsync(string userId)
        {
            return await repository.GetByUserIdAsync(userId);
        }
        public async Task<List<DoctorDto>> FilterAsync(string? name, string? specialization)
        {
            var doctors = await repository.FilterAsync(name, specialization);
            return mapper.Map<List<DoctorDto>>(doctors);
        }

        private async Task InvalidateActiveDoctorsCache()
        {
            var db = redis.GetDatabase();

            await db.KeyDeleteAsync("doctors:active");

            Log.Information("ACTIVE DOCTORS CACHE INVALIDATED");
        }


    }
}
