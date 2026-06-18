using AutoMapper;
using HealthAxis.API.DTOs;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Models.Auth;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HealthAxis.API.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public DoctorService(
            IDoctorRepository repository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _doctorRepository = repository;
            _userManager = userManager;
            _mapper = mapper;
        }

        // ✅ GET /api/doctors
        public async Task<IEnumerable<DoctorDto>> GetAllAsync(CancellationToken ct = default)
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        // ✅ GET /api/doctors/{id}
        public async Task<DoctorDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            return _mapper.Map<DoctorDto>(doctor);
        }

        // ✅ GET /api/doctors/{id}/availability
        public async Task<object> GetAvailabilityAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            return new
            {
                doctorId = doctor.DoctorId,
                doctorName = doctor.FullName,
                isActive = doctor.IsActive,
                availableSlots = doctor.IsActive
                    ? new List<string>
                    {
                        "09:00 AM",
                        "10:00 AM",
                        "11:00 AM",
                        "02:00 PM",
                        "03:00 PM"
                    }
                    : new List<string>()
            };
        }

        // ✅ Admin creates Doctor login + Doctor profile
        public async Task<DoctorDto> AddAsync(CreateDoctorDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                throw new BusinessRuleException("Doctor login already exists with this email.");
            }

            var doctorUser = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true,
                MustChangePassword = true
            };

            var createUserResult = await _userManager.CreateAsync(
                doctorUser,
                dto.TemporaryPassword);

            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                throw new ValidationException(errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(doctorUser, "Doctor");

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new ValidationException(errors);
            }

            var doctor = new Doctor
            {
                FullName = dto.FullName,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = true
            };

            await _doctorRepository.AddAsync(doctor);

            return _mapper.Map<DoctorDto>(doctor);
        }

        // ✅ Admin updates Doctor profile
        public async Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            _mapper.Map(dto, doctor);

            await _doctorRepository.UpdateAsync(id, doctor, CancellationToken.None);

            return _mapper.Map<DoctorDto>(doctor);
        }
    }
}