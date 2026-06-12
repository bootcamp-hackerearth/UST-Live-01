using AutoMapper;
using HealthCare.Shared;
using HealthCare.Shared.DTOs.Doctor;
using HealthCareApi.Repositories.Implementations;
using HealthCareApi.Repositories.Interfaces;
using HealthCareApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCareApi.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
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

        public async Task<Doctor> AddDoctorAsync(CreateDoctorDto dto)
        {
            //  Map doctor
            var doctor = _mapper.Map<Doctor>(dto);

            //  Save doctor first
            doctor.IsActive = true;
            await _doctorRepository.AddAsync(doctor);

            //  check if slots provided
            if (dto.TimeSlots != null && dto.TimeSlots.Any())
            {
                //create slots records
                var slots = dto.TimeSlots.Select(slot => new DoctorAvailableSlot
                {
                    DoctorId = doctor.DoctorId,
                    TimeSlot = slot
                }).ToList();

                await _doctorRepository.AddRangeAsync(slots);
            }

            return doctor;
        }

        public async Task<List<Doctor>> GetBySpecializationAsync(string specialization)
        {
            if (string.IsNullOrWhiteSpace(specialization))
                throw new ArgumentException("Specialization is required", nameof(specialization));

            return await _doctorRepository
                .GetBySpecializationAsync(specialization);
        }

        public async Task<Doctor> UpdateDoctorAsync(Doctor updatedDoctor)
        {
            // Get existing data via repo
            var existingDoctor = await _doctorRepository.GetByIdAsync(updatedDoctor.DoctorId);

            if (existingDoctor == null)
                return null;

            // Update only allowed fields
            existingDoctor.FullName = updatedDoctor.FullName;
            existingDoctor.Specialisation = updatedDoctor.Specialisation;
            existingDoctor.YearsOfExperience = updatedDoctor.YearsOfExperience;
            existingDoctor.ConsultationFee = updatedDoctor.ConsultationFee;

            //Call repo to save
            await _doctorRepository.UpdateAsync(existingDoctor);

            return existingDoctor;
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
                return false;

            
            doctor.IsActive = false;

            await _doctorRepository.UpdateAsync(doctor);

            return true;
        }
    }
}