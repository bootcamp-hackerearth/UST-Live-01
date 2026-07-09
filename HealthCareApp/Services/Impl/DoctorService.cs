using AutoMapper;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services.Interface;
using HealthCareApp.Shared.Constants;
using HealthCareApp.Shared.Dtos.Doctors;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace HealthCareApp.Services
{
    public class DoctorService(
        IDoctorRepository repository,
        IAppointmentRepository appointmentRepository,
        IMapper mapper,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ICacheService cacheService,
        ILogger<DoctorService> logger) : IDoctorService
    {
        private const string DoctorEntityName = "Doctor";
        private const string DoctorRoleName = "Doctor";
        private const string DoctorDetailsRequiredMessage = "Doctor details are required.";

        private static readonly Regex DoctorFullNameRegex = new(
            @"^[A-Za-z]+(?: [A-Za-z]+)*$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant,
            TimeSpan.FromMilliseconds(250));

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await repository.GetAllAsync();

            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<PagedResponse<DoctorDto>> GetAllDoctorsPagedAsync(DoctorPaginationQueryDto query)
        {
            query ??= new DoctorPaginationQueryDto();

            int pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;

            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            pageSize = pageSize > 100 ? 100 : pageSize;

            var doctors = await repository.GetAllAsync();

            var filteredDoctors = doctors.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                string searchTerm = query.SearchTerm.Trim();

                filteredDoctors = filteredDoctors.Where(doctor =>
                    doctor.DoctorName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    doctor.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (query.Specialisation is not null)
            {
                filteredDoctors = filteredDoctors.Where(doctor =>
                    doctor.Specialisation == query.Specialisation.Value);
            }

            if (query.IsActive is not null)
            {
                filteredDoctors = filteredDoctors.Where(doctor =>
                    doctor.IsActive == query.IsActive.Value);
            }

            int totalRecords = filteredDoctors.Count();

            var pagedDoctors = filteredDoctors
                .OrderBy(doctor => doctor.DoctorId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mappedDoctors = mapper.Map<List<DoctorDto>>(pagedDoctors);

            return new PagedResponse<DoctorDto>
            {
                Items = mappedDoctors,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
            };
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
                throw new EntityNotFoundException(DoctorEntityName, doctorId);
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
                var errors = string.Join(",", createUserResult.Errors.Select(error => error.Description));
                throw new BusinessRuleException(errors);
            }

            if (!await roleManager.RoleExistsAsync(DoctorRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(DoctorRoleName));
            }

            var roleResult = await userManager.AddToRoleAsync(identityUser, DoctorRoleName);

            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(identityUser);

                var errors = string.Join(",", roleResult.Errors.Select(error => error.Description));
                throw new BusinessRuleException(errors);
            }

            var doctor = mapper.Map<Doctor>(dto);

            doctor.DoctorName = dto.FullName.Trim();
            doctor.Email = normalizedEmail;
            doctor.YearsOfExperience = CalculateYearsOfExperience(dto.PracticeStartDate);
            doctor.IsActive = true;
            doctor.MustChangePassword = true;
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
                throw new EntityNotFoundException(DoctorEntityName, doctorId);
            }

            var doctor = mapper.Map<Doctor>(dto);

            doctor.DoctorId = doctorId;
            doctor.Email = existingDoctor.Email;
            doctor.IdentityUserId = existingDoctor.IdentityUserId;
            doctor.YearsOfExperience = CalculateYearsOfExperience(dto.PracticeStartDate);
            doctor.CreatedDate = existingDoctor.CreatedDate;
            doctor.MustChangePassword = existingDoctor.MustChangePassword;

            var updatedDoctor = await repository.UpdateAsync(doctorId, doctor);

            if (updatedDoctor is null)
            {
                throw new EntityNotFoundException(DoctorEntityName, doctorId);
            }

            return mapper.Map<DoctorDto>(updatedDoctor);
        }

        public async Task<DoctorDto> DeleteDoctorAsync(int doctorId)
        {
            ValidateDoctorId(doctorId);

            var deletedDoctor = await repository.DeleteAsync(doctorId);

            if (deletedDoctor is null)
            {
                throw new EntityNotFoundException(DoctorEntityName, doctorId);
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

        public async Task<List<SlotAvailabilityDto>> GetDoctorAvailabilityAsync(
            int doctorId,
            DateTime? date)
        {
            ValidateDoctorId(doctorId);

            var cacheKey = $"doctors:{doctorId}:availability:{date:yyyy-MM-dd}";
            var cachedAvailability = await cacheService.GetAsync<List<SlotAvailabilityDto>>(cacheKey);

           
            if (cachedAvailability != null)
            {
                logger.LogInformation(
                    "Availability Cache Hit for doctor {DoctorId}",
                    doctorId);

                return cachedAvailability;
            }

            logger.LogInformation("Availability Cache Miss for doctor {DoctorId}",doctorId);

            var doctor = await repository.GetByIdAsync(doctorId);


            if (doctor is null)
            {
                throw new EntityNotFoundException(DoctorEntityName, doctorId);
            }

            if (!doctor.IsActive)
            {
                throw new BusinessRuleException("Doctor is inactive and not available for appointments.");
            }

            var bookedSlots = new List<string>();

            if (date is not null)
            {
                bookedSlots = await appointmentRepository.GetBookedTimeSlotsByDoctorAndDateAsync(
                    doctorId,
                    date.Value);
            }

            var bookedSlotSet = bookedSlots.ToHashSet(StringComparer.OrdinalIgnoreCase);

            var availability = TimeSlots.Slots
                .Select(slot => new SlotAvailabilityDto
                {
                    TimeSlot = slot,
                    IsBooked = bookedSlotSet.Contains(slot)
                })
                .ToList();

            await cacheService.SetAsync(cacheKey,availability,TimeSpan.FromMinutes(5));
            logger.LogInformation("Availability cached for doctor {DoctorId} with key {CacheKey}",doctorId,cacheKey);
            return availability;
        }

        private static void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new BusinessRuleException("Please provide a valid doctor reference.");
            }
        }

        private static void ValidateCreateDoctorDto(CreateDoctorDto dto)
        {
            if (dto is null)
            {
                throw new BusinessRuleException(DoctorDetailsRequiredMessage);
            }

            ValidateDoctorCommonFields(
                dto.FullName,
                dto.Email,
                dto.PracticeStartDate,
                dto.ConsultationFee);
        }

        private static void ValidateUpdateDoctorDto(UpdateDoctorDto dto)
        {
            if (dto is null)
            {
                throw new BusinessRuleException(DoctorDetailsRequiredMessage);
            }

            ValidateDoctorCommonFields(
                dto.FullName,
                null,
                dto.PracticeStartDate,
                dto.ConsultationFee);
        }

        private static void ValidateDoctorCommonFields(
            string fullName,
            string? email,
            DateTime practiceStartDate,
            decimal consultationFee)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new BusinessRuleException("Doctor full name is required.");
            }

            string trimmedFullName = fullName.Trim();

            if (!DoctorFullNameRegex.IsMatch(trimmedFullName))
            {
                throw new BusinessRuleException(
                    "Doctor name can contain only letters and single spaces between words.");
            }

            if (email is not null && string.IsNullOrWhiteSpace(email))
            {
                throw new BusinessRuleException("Doctor email is required.");
            }

            if (practiceStartDate.Date > DateTime.Today)
            {
                throw new BusinessRuleException("Practice start date cannot be in the future.");
            }

            if (consultationFee < 1 || consultationFee > 100000)
            {
                throw new BusinessRuleException("Consultation fee must be between 1 and 100,000.");
            }
        }

        private static int CalculateYearsOfExperience(DateTime practiceStartDate)
        {
            int years = DateTime.Today.Year - practiceStartDate.Year;

            if (practiceStartDate.Date > DateTime.Today.AddYears(-years))
            {
                years--;
            }

            return years;
        }

        private static string GenerateTemporaryPassword(string doctorName)
        {
            string cleanedName = new string(
                doctorName
                    .Where(char.IsLetter)
                    .Take(5)
                    .ToArray());

            if (string.IsNullOrWhiteSpace(cleanedName))
            {
                cleanedName = DoctorEntityName;
            }

            string formattedName =
                char.ToUpper(cleanedName[0]) + cleanedName.Substring(1).ToLower();

            return $"{formattedName}@{DateTime.Today.Year}";
        }
    }
}