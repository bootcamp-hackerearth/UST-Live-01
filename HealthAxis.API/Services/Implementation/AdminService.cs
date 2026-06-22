using AutoMapper;
using HealthAxis.API.DTO;
using HealthAxis.API.DTO.AdminDtos;
using HealthAxis.API.DTO.DoctorDtos;
using HealthAxis.API.Enums;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HealthAxis.API.Services.Implementation
{
    public class AdminService(
        IDoctorRepository doctorRepository,
        IAppointmentRepository appointmentRepository,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper) : IAdminService
    {
        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await doctorRepository.GetAllAsync();

            return mapper.Map<List<DoctorDto>>(doctors);
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
                await userManager.FindByEmailAsync(doctorDto.Email);

            if (existingUser != null)
            {
                throw new BusinessRuleException("Doctor email already exists");
            }

            if (!await roleManager.RoleExistsAsync("Doctor"))
            {
                await roleManager.CreateAsync(new IdentityRole("Doctor"));
            }

            var temporaryPassword = GenerateTemporaryPassword();

            var user = new IdentityUser
            {
                UserName = doctorDto.Email,
                Email = doctorDto.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, temporaryPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ",  result.Errors.Select(error => error.Description));

                throw new ValidationException(errors);
            }

            await userManager.AddToRoleAsync(user, "Doctor");

            var doctor = new Doctor
            {
                FullName = doctorDto.FullName,
                Specialisation = doctorDto.Specialisation,
                YearsOfExperience = doctorDto.YearsOfExperience,
                ConsultationFee = doctorDto.ConsultationFee,
                IsActive = doctorDto.IsActive,
                UserId = user.Id
            };

            var savedDoctor = await doctorRepository.AddAsync(doctor);

            await userManager.AddClaimAsync(user, new Claim("DoctorId", savedDoctor.DoctorId.ToString()));

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
            var existingDoctor = await doctorRepository.GetByIdAsync(id);

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

            var updatedDoctor = await doctorRepository.UpdateAsync(id, existingDoctor);

            return mapper.Map<DoctorDto>(updatedDoctor);
        }

        public async Task<List<AdminDto>> GetAppointmentReportsAsync()
        {
            var appointments =  await appointmentRepository.GetAllAsync();

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