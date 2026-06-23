using AutoMapper;
using HealthAxis.Shared.DTO;
using HealthAxis.Shared.DTO.AdminDtos;
using HealthAxis.Shared.DTO.DoctorDtos;
using HealthAxis.Shared.Enums;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAxis.API.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AdminService(
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();

            return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorCreatedDto> AddDoctorAsync(CreateDoctorDto doctorDto)
        {
            if (string.IsNullOrWhiteSpace(doctorDto.FullName))
            {
                throw new ValidationException("Doctor name is required");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch( doctorDto.FullName, @"^[A-Za-z ]+$"))
            {
                throw new ValidationException(
                    "Doctor name should contain only alphabets and spaces");
            }

            if (string.IsNullOrWhiteSpace(doctorDto.Email))
            {
                throw new ValidationException("Doctor email is required");
            }

            //if (!doctorDto.Email.EndsWith( "@gmail.com",
            //        StringComparison.OrdinalIgnoreCase))
            //{
            //    throw new ValidationException("Only Gmail address is allowed");
            //}

            if (!Enum.IsDefined( typeof(Specialisation), doctorDto.Specialisation))
            {
                throw new ValidationException("Invalid specialisation");
            }

            if (doctorDto.YearsOfExperience < 0 ||
                doctorDto.YearsOfExperience > 60)
            {
                throw new ValidationException(
                    "Years of experience must be between 0 and 60");
            }

            if (doctorDto.ConsultationFee < 100 ||
                doctorDto.ConsultationFee > 10000)
            {
                throw new ValidationException(
                    "Consultation fee must be between 100 and 10000");
            }

            var existingUser =
                await _userManager.FindByEmailAsync(doctorDto.Email);

            if (existingUser != null)
            {
                throw new BusinessRuleException("Doctor email already exists");
            }

            var roleExists = _roleManager.RoleExistsAsync("Doctor").GetAwaiter().GetResult();
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));
            }

            var temporaryPassword = GenerateTemporaryPassword();

            var user = new IdentityUser
            {
                UserName = doctorDto.Email,
                Email = doctorDto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, temporaryPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ",  result.Errors.Select(error => error.Description));

                throw new ValidationException(errors);
            }

            await _userManager.AddToRoleAsync(user, "Doctor");

            var doctor = new Doctor
            {
                FullName = doctorDto.FullName,
                Specialisation = doctorDto.Specialisation,
                YearsOfExperience = doctorDto.YearsOfExperience,
                ConsultationFee = doctorDto.ConsultationFee,
                IsActive = doctorDto.IsActive,
                UserId = user.Id
            };

            var savedDoctor = await _doctorRepository.AddAsync(doctor);

            await _userManager.AddClaimAsync(user, new Claim("DoctorId", savedDoctor.DoctorId.ToString()));

            return new DoctorCreatedDto
            {
                DoctorId = savedDoctor.DoctorId,
                FullName = savedDoctor.FullName,
                Email = doctorDto.Email,
                Specialisation = savedDoctor.Specialisation,
                YearsOfExperience = savedDoctor.YearsOfExperience,
                ConsultationFee = savedDoctor.ConsultationFee,
                IsActive = savedDoctor.IsActive,
                TemporaryPassword = temporaryPassword
            };
        }

        public async Task<DoctorDto> UpdateDoctorAsync( int id, UpdateDoctorDto doctorDto)
        {
            var existingDoctor = await _doctorRepository.GetByIdAsync(id);

            if (existingDoctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            if (string.IsNullOrWhiteSpace(doctorDto.FullName))
            {
                throw new ValidationException("Doctor full name is required");
            }

            if (doctorDto.YearsOfExperience < 0 ||
                doctorDto.YearsOfExperience > 60)
            {
                throw new ValidationException(
                    "Years of experience must be between 0 and 60");
            }

            if (doctorDto.ConsultationFee <= 0)
            {
                throw new ValidationException(
                    "Consultation fee must be greater than 0");
            }

            existingDoctor.FullName = doctorDto.FullName;
            existingDoctor.Specialisation = doctorDto.Specialisation;
            existingDoctor.YearsOfExperience = doctorDto.YearsOfExperience;
            existingDoctor.ConsultationFee = doctorDto.ConsultationFee;
            existingDoctor.IsActive = doctorDto.IsActive;

            var updatedDoctor = await _doctorRepository.UpdateAsync(id, existingDoctor);

            return _mapper.Map<DoctorDto>(updatedDoctor);
        }

        public async Task<List<AdminDto>> GetAppointmentReportsAsync()
        {
            var appointments =  await _appointmentRepository.GetAllAsync();

            var reports = appointments
                .GroupBy(a => a.ScheduledDate.Date)
                .Select(group => new AdminDto
                {
                    Date = group.Key,

                    ConfirmedCount = group.Count(a =>
                        a.Status == AppointmentStatus.Confirmed),

                    CancelledCount = group.Count(a =>
                        a.Status == AppointmentStatus.Cancelled),

                    CompletedCount = group.Count(a =>
                        a.Status == AppointmentStatus.Completed)
                })
                .OrderBy(report => report.Date)
                .ToList();

            return reports;
        }

        public async Task<List<AdminUserDto>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var doctors = await _doctorRepository.GetAllAsync();

            var result = new List<AdminUserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? string.Empty;

                string fullName = user.Email ?? string.Empty;
                bool isActive = true;

                if (string.Equals(role, "Doctor", StringComparison.OrdinalIgnoreCase))
                {
                    var doc = doctors.FirstOrDefault(d => d.UserId == user.Id);
                    if (doc != null)
                    {
                        fullName = doc.FullName;
                        isActive = doc.IsActive;
                    }
                }

                result.Add(new AdminUserDto
                {
                    UserId = user.Id,
                    FullName = fullName,
                    Email = user.Email ?? string.Empty,
                    Role = role,
                    IsActive = isActive
                });
            }

            return result;
        }
        private static string GenerateTemporaryPassword()
        {
            return "Doctor@" + Random.Shared.Next(1000, 9999);
        }
        private static void ValidateDoctor(DoctorDto doctorDto)
        {
            if (string.IsNullOrWhiteSpace(doctorDto.FullName))
            {
                throw new ValidationException("Doctor full name is required");
            }

            if (!Enum.IsDefined(typeof(Specialisation), doctorDto.Specialisation))
            {
                throw new ValidationException("Invalid specialisation");
            }

            if (doctorDto.YearsOfExperience < 0 ||
                doctorDto.YearsOfExperience > 60)
            {
                throw new ValidationException(
                    "Years of experience must be between 0 and 60");
            }

            if (doctorDto.ConsultationFee <= 0)
            {
                throw new ValidationException(
                    "Consultation fee must be greater than 0");
            }
        }
    }
}