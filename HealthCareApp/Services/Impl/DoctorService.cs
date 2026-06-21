using AutoMapper;
using HealthCareApp.Constants;
using HealthCareApp.Dtos;
using HealthCareApp.Enums;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using Microsoft.AspNetCore.Identity;

namespace HealthCareApp.Services
{
    public class DoctorService(
        IDoctorRepository repository,
        IMapper mapper,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager) : IDoctorService
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
            ValidateDoctorId(doctorId);

            var doctor = await repository.GetByIdAsync(doctorId);

            if (doctor is null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
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

        public async Task<DoctorCreatedResponseDto> CreateDoctorByAdminAsync(CreateDoctorDto dto)
        {
            ValidateCreateDoctorDto(dto);

            string normalizedEmail = dto.Email.Trim().ToLower();

            bool doctorEmailExists = await repository.ExistsByEmailAsync(normalizedEmail);

            if (doctorEmailExists)
            {
                throw new ConflictException("A doctor with this email already exists.");
            }

            var existingIdentityUser = await userManager.FindByEmailAsync(normalizedEmail);

            if (existingIdentityUser is not null)
            {
                throw new ConflictException("A login account with this email already exists.");
            }

            string temporaryPassword = GenerateTemporaryPassword(dto.FullName);

            var identityUser = new IdentityUser
            {
                UserName = normalizedEmail,
                Email = normalizedEmail,
                EmailConfirmed = true
            };

            var createUserResult = await userManager.CreateAsync(identityUser, temporaryPassword);

            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(",", createUserResult.Errors.Select(e => e.Description));
                throw new BusinessRuleException(errors);
            }

            if (!await roleManager.RoleExistsAsync("Doctor"))
            {
                await roleManager.CreateAsync(new IdentityRole("Doctor"));
            }

            var roleResult = await userManager.AddToRoleAsync(identityUser, "Doctor");

            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(identityUser);

                var errors = string.Join(",", roleResult.Errors.Select(e => e.Description));
                throw new BusinessRuleException(errors);
            }

            var doctor = mapper.Map<Doctor>(dto);

            doctor.DoctorName = dto.FullName.Trim();
            doctor.Email = normalizedEmail;
            doctor.YearsOfExperience = CalculateYearsOfExperience(dto.PracticeStartDate);
            doctor.IsActive = true;
            doctor.IdentityUserId = identityUser.Id;
            doctor.CreatedDate = DateTime.Now;

            var savedDoctor = await repository.CreateAsync(doctor);

            return new DoctorCreatedResponseDto
            {
                DoctorId = savedDoctor.DoctorId,
                DoctorName = savedDoctor.DoctorName,
                Email = savedDoctor.Email,
                TemporaryPassword = temporaryPassword,
                Message = "Doctor account created successfully."
            };
        }

        public async Task<DoctorDto> UpdateDoctorAsync(int doctorId, UpdateDoctorDto dto)
        {
            ValidateDoctorId(doctorId);

            ValidateUpdateDoctorDto(dto);

            var existingDoctor = await repository.GetByIdAsync(doctorId);

            if (existingDoctor is null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            var doctor = mapper.Map<Doctor>(dto);

            doctor.DoctorId = doctorId;
            doctor.Email = existingDoctor.Email;
            doctor.IdentityUserId = existingDoctor.IdentityUserId;
            doctor.YearsOfExperience = CalculateYearsOfExperience(dto.PracticeStartDate);
            doctor.CreatedDate = existingDoctor.CreatedDate;

            var updatedDoctor = await repository.UpdateAsync(doctorId, doctor);

            if (updatedDoctor is null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return mapper.Map<DoctorDto>(updatedDoctor);
        }

        public async Task<DoctorDto> DeleteDoctorAsync(int doctorId)
        {
            ValidateDoctorId(doctorId);

            var deletedDoctor = await repository.DeleteAsync(doctorId);

            if (deletedDoctor is null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return mapper.Map<DoctorDto>(deletedDoctor);
        }
        public async Task<DoctorDto> GetMyProfileAsync(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged-in user.");
            }

            var doctor = await repository.GetByIdentityUserIdAsync(identityUserId);

            if (doctor is null)
            {
                throw new EntityNotFoundException("Doctor profile for logged-in user", 0);
            }

            return mapper.Map<DoctorDto>(doctor);
        }

        private void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new BusinessRuleException("Please provide a valid doctor reference.");
            }
        }

        private void ValidateCreateDoctorDto(CreateDoctorDto dto)
        {
            if (dto is null)
            {
                throw new BusinessRuleException("Doctor details are required.");
            }

            ValidateDoctorCommonFields(
                dto.FullName,
                dto.Email,
                dto.PracticeStartDate,
                dto.ConsultationFee);
        }

        private void ValidateUpdateDoctorDto(UpdateDoctorDto dto)
        {
            if (dto is null)
            {
                throw new BusinessRuleException("Doctor details are required.");
            }

            ValidateDoctorCommonFields(
                dto.FullName,
                null,
                dto.PracticeStartDate,
                dto.ConsultationFee);
        }

        private void ValidateDoctorCommonFields(
            string fullName,
            string? email,
            DateTime practiceStartDate,
            decimal consultationFee)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new BusinessRuleException("Doctor full name is required.");
            }

            if (email is not null && string.IsNullOrWhiteSpace(email))
            {
                throw new BusinessRuleException("Doctor email is required.");
            }

            if (practiceStartDate.Date > DateTime.Today)
            {
                throw new BusinessRuleException("Practice start date cannot be in the future.");
            }

            if (consultationFee < 0)
            {
                throw new BusinessRuleException("Consultation fee cannot be negative.");
            }
        }

        private int CalculateYearsOfExperience(DateTime practiceStartDate)
        {
            int years = DateTime.Today.Year - practiceStartDate.Year;

            if (practiceStartDate.Date > DateTime.Today.AddYears(-years))
            {
                years--;
            }

            return years;
        }

        private string GenerateTemporaryPassword(string doctorName)
        {
            string cleanedName = new string(
                doctorName
                    .Where(char.IsLetter)
                    .Take(5)
                    .ToArray());

            if (string.IsNullOrWhiteSpace(cleanedName))
            {
                cleanedName = "Doctor";
            }

            string formattedName =
                char.ToUpper(cleanedName[0]) + cleanedName.Substring(1).ToLower();

            return $"{formattedName}@{DateTime.Today.Year}";
        }

        public async Task<List<string>> GetDoctorAvailabilityAsync(int doctorId)
        {
            ValidateDoctorId(doctorId);

            var doctor = await repository.GetByIdAsync(doctorId);

            if (doctor is null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            if (!doctor.IsActive)
            {
                throw new BusinessRuleException("Doctor is inactive and not available for appointments.");
            }

            return TimeSlots.Slots;
        }
    }
}