using HealthCareApi.Repositories.Interfaces;
using HealthCareApi.Services.Interfaces;
//using HealthCareApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthCare.Shared;

namespace HealthCareApi.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<Doctor> GetDoctorByIdAsync(int id)
        {
            return await _doctorRepository.GetByIdAsync(id);
        }

        public async Task<PagedResult<Doctor>> GetFilteredDoctorsAsync(
            string specialization = null,
            string searchTerm = null,
            bool orderByDescending = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
            return await _doctorRepository.GetDoctorsAsync(
                specialization,
                searchTerm,
                orderByDescending,
                pageNumber,
                pageSize);
        }

        public async Task<Doctor> AddDoctorAsync(Doctor doctor)
        {
            await _doctorRepository.AddAsync(doctor);
            return doctor;
        }

        public async Task<Doctor> UpdateDoctorAsync(Doctor updatedDoctor)
        {
            // Step 1: Get existing data via repo
            var existingDoctor = await _doctorRepository.GetByIdAsync(updatedDoctor.DoctorId);

            if (existingDoctor == null)
                return null;

            // Step 2: Update only allowed fields
            existingDoctor.FullName = updatedDoctor.FullName;
            existingDoctor.Specialisation = updatedDoctor.Specialisation;
            existingDoctor.YearsOfExperience = updatedDoctor.YearsOfExperience;
            existingDoctor.ConsultationFee = updatedDoctor.ConsultationFee;

            // Step 3: Call repo to save
            await _doctorRepository.UpdateAsync(existingDoctor);

            return existingDoctor;
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
                return false;

            // Soft delete
            doctor.IsActive = false;

            await _doctorRepository.UpdateAsync(doctor);

            return true;
        }
    }
}