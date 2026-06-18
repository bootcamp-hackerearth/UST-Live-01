using AutoMapper;
using HealthAxisCore_Api.DTOs.Doctor;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Interfaces;
using HealthAxisCore_Api.Enums;
using HealthAxisCore_Api.Exceptions;

using Microsoft.AspNetCore.Identity;

namespace HealthAxisCore_Api.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorService(
            IDoctorRepository repository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }

        // ✅ Get All Doctors
        public async Task<IEnumerable<DoctorResponseDTO>> GetAllAsync()
        {
            var doctors = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorResponseDTO>>(doctors);
        }

        // ✅ Get Doctor By Id
        public async Task<DoctorResponseDTO?> GetByIdAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor == null)
                throw new EntityNotFoundException("Doctor not found");

            return _mapper.Map<DoctorResponseDTO>(doctor);
        }

        // ✅ ✅ CREATE DOCTOR (UPDATED 🔥)
        public async Task<DoctorResponseDTO> CreateAsync(CreateDoctorDTO dto)
        {
            // ✅ Step 1: Create Doctor entity
            var doctor = _mapper.Map<Doctor>(dto);
            doctor.CreatedDate = DateTime.Now;

            await _repository.AddAsync(doctor);

            // ✅ Step 2: Generate temporary password
            var tempPassword = "Temp@" + new Random().Next(1000, 9999);

            // ✅ Step 3: Create ApplicationUser
            var user = new ApplicationUser
            {
                UserName = doctor.Email,
                Email = doctor.Email,
                Role = "Doctor",
                ReferenceId = doctor.DoctorId,
                IsFirstLogin = true, // ✅ important
                TemporaryPassword = tempPassword
            };

            var result = await _userManager.CreateAsync(user, tempPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessRuleException(errors);
            }

            // ✅ Step 4: Assign role
            await _userManager.AddToRoleAsync(user, "Doctor");

            // ✅ IMPORTANT (for now testing only)
            Console.WriteLine($"Doctor Temporary Password: {tempPassword}");

            // 👉 In real apps → send via Email/SMS

            return _mapper.Map<DoctorResponseDTO>(doctor);
        }

        // ✅ Update Doctor
        public async Task<bool> UpdateAsync(int id, CreateDoctorDTO dto)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor == null)
                throw new EntityNotFoundException("Doctor not found");

            _mapper.Map(dto, doctor);

            await _repository.UpdateAsync(doctor);

            return true;
        }

        // ✅ Delete Doctor
        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _repository.Exists(id);

            if (!exists)
                throw new EntityNotFoundException("Doctor not found");

            await _repository.DeleteAsync(id);

            return true;
        }

        // ✅ Filter Doctors
        public async Task<IEnumerable<DoctorResponseDTO>> FilterAsync(
            string? name,
            SpecialisationType? specialization,
            bool? isActive)
        {
            var doctors = await _repository.GetDoctors(name, specialization, isActive);

            return _mapper.Map<IEnumerable<DoctorResponseDTO>>(doctors);
        }

        // ✅ Activate / Deactivate Doctor
        public async Task<bool> SetStatusAsync(int doctorId, bool status)
        {
            var doctor = await _repository.GetByIdAsync(doctorId);

            if (doctor == null)
                throw new EntityNotFoundException("Doctor not found");

            await _repository.SetStatus(doctorId, status);

            return true;
        }
    }
}
