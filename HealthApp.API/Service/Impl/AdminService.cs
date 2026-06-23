using AutoMapper;
using HealthApp.Shared.Constants;
using HealthApp.Shared.Enums;
using HealthApp.API.Exceptions;
using HealthApp.API.Identity;
using HealthApp.API.Models;
using HealthApp.Shared.DTOs;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.API.Service.Impl;

public class AdminService(
    IDoctorRepository doctorRepository,
    IPatientRepository patientRepository,
    IAppointmentRepository appointmentRepository,
    UserManager<ApplicationUser> userManager,
    IMapper mapper) : IAdminService
{
    public async Task<List<DoctorDto>> GetDoctorsAsync()
        => mapper.Map<List<DoctorDto>>(await doctorRepository.GetAllAsync());

    public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto)
    {
        if (dto is null)
        {
            throw new BusinessRuleException("Doctor details are required.");
        }

        ValidateCreateDoctorDto(dto);

        var existingUser = await userManager.FindByEmailAsync(dto.Email);

        if (existingUser is not null)
        {
            throw new ConflictException("A user with this email already exists.");
        }

        var doctorUser = new ApplicationUser
        {
            UserName = dto.Email.Trim(),
            Email = dto.Email.Trim(),
            FullName = dto.FullName.Trim(),
            EmailConfirmed = true,
            CreatedDate = DateTime.UtcNow,
            MustChangePassword = true
        };

        var createUserResult = await userManager.CreateAsync(
            doctorUser,
            dto.TemporaryPassword);

        if (!createUserResult.Succeeded)
        {
            throw new BusinessRuleException(
                "Doctor user creation failed: " +
                string.Join(", ", createUserResult.Errors.Select(e => e.Description))
            );
        }

        var addRoleResult = await userManager.AddToRoleAsync(
            doctorUser,
            Roles.Doctor);

        if (!addRoleResult.Succeeded)
        {
            await userManager.DeleteAsync(doctorUser);

            throw new BusinessRuleException(
                "Doctor role assignment failed: " +
                string.Join(", ", addRoleResult.Errors.Select(e => e.Description))
            );
        }

        try
        {
            var doctor = mapper.Map<Doctor>(dto);

            doctor.UserId = doctorUser.Id;
            doctor.IsActive = true;
            doctor.YearsOfExperience = CalculateExperience(dto.PracticeStartDate);
            doctor.CreatedDate = DateTime.Now;

            var savedDoctor = await doctorRepository.AddAsync(doctor);

            return mapper.Map<DoctorDto>(savedDoctor);
        }
        catch
        {
            await userManager.DeleteAsync(doctorUser);
            throw;
        }
    }

    public async Task<DoctorDto> UpdateDoctorAsync(int doctorId, UpdateDoctorDto dto)
    {
        if (dto is null)
        {
            throw new BusinessRuleException("Doctor details are required.");
        }

        ValidateDoctorId(doctorId);
        ValidateUpdateDoctorDto(dto);

        var existing = await doctorRepository.GetByIdAsync(doctorId)
            ?? throw new EntityNotFoundException("Doctor", doctorId);

        var doctor = mapper.Map<Doctor>(dto);

        doctor.DoctorId = doctorId;
        doctor.UserId = existing.UserId;
        doctor.CreatedDate = existing.CreatedDate;
        doctor.YearsOfExperience = CalculateExperience(dto.PracticeStartDate);

        var updated = await doctorRepository.UpdateAsync(doctorId, doctor)
            ?? throw new EntityNotFoundException("Doctor", doctorId);

        return mapper.Map<DoctorDto>(updated);
    }

    public async Task<List<AppointmentReportDto>> GetAppointmentReportsAsync()
    {
        var all = await appointmentRepository.GetAllAsync();

        return all
            .GroupBy(a => a.ScheduledDate.Date)
            .OrderBy(g => g.Key)
            .Select(g => new AppointmentReportDto
            {
                Date = g.Key,
                Pending = g.Count(a => a.Status == AppointmentStatus.Pending.ToString()),
                Confirmed = g.Count(a => a.Status == AppointmentStatus.Confirmed.ToString()),
                Cancelled = g.Count(a => a.Status == AppointmentStatus.Cancelled.ToString()),
                Completed = g.Count(a => a.Status == AppointmentStatus.Completed.ToString())
            })
            .ToList();
    }

    private static int CalculateExperience(DateTime start)
    {
        var years = DateTime.Today.Year - start.Year;

        if (start.Date > DateTime.Today.AddYears(-years))
        {
            years--;
        }

        return Math.Max(0, years);
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
        if (string.IsNullOrWhiteSpace(dto.FullName))
        {
            throw new BusinessRuleException("Doctor full name is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new BusinessRuleException("Doctor email is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.TemporaryPassword))
        {
            throw new BusinessRuleException("Temporary password is required.");
        }


        if (!Enum.IsDefined(typeof(SpecialisationType), dto.Specialisation))
        {
            throw new BusinessRuleException("Invalid specialisation.");
        }


        if (dto.PracticeStartDate.Date > DateTime.Today)
        {
            throw new BusinessRuleException("Practice start date cannot be in the future.");
        }

        if (dto.ConsultationFee < 0)
        {
            throw new BusinessRuleException("Consultation fee cannot be negative.");
        }
    }

    private static void ValidateUpdateDoctorDto(UpdateDoctorDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
        {
            throw new BusinessRuleException("Doctor full name is required.");
        }


        if (!Enum.IsDefined(typeof(SpecialisationType), dto.Specialisation))
        {
            throw new BusinessRuleException("Invalid specialisation.");
        }


        if (dto.PracticeStartDate.Date > DateTime.Today)
        {
            throw new BusinessRuleException("Practice start date cannot be in the future.");
        }

        if (dto.ConsultationFee < 0)
        {
            throw new BusinessRuleException("Consultation fee cannot be negative.");
        }
    }

    public async Task<List<UserDto>> GetUsersAsync(string? role = null)
    {
        if (!string.IsNullOrWhiteSpace(role))
        {
            var selectedRole = role.Trim();

            var validRoles = new[]
            {
            Roles.Admin,
            Roles.Doctor,
            Roles.Patient
        };

            if (!validRoles.Contains(selectedRole))
            {
                throw new BusinessRuleException("Invalid role filter.");
            }

            var usersInRole = await userManager.GetUsersInRoleAsync(selectedRole);

            return usersInRole
                .Select(user => new UserDto
                {
                    UserId = user.Id,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName,
                    Role = selectedRole
                })
                .OrderBy(user => user.FullName)
                .ToList();
        }

        var users = await userManager.Users
            .OrderBy(user => user.FullName)
            .ToListAsync();

        var result = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);

            result.Add(new UserDto
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                Role = roles.FirstOrDefault() ?? "Unknown"
            });
        }

        return result;
    }

    public async Task<List<PatientDto>> GetPatientsAsync(
    string? search = null,
    GenderType? gender = null,
    bool? hasInsurance = null)
    {
        if (gender.HasValue &&
            !Enum.IsDefined(typeof(GenderType), gender.Value))
        {
            throw new BusinessRuleException("Invalid gender filter.");
        }

        var patients = await patientRepository.GetFilteredAsync(
            search,
            gender,
            hasInsurance);

        return mapper.Map<List<PatientDto>>(patients);
    }
}
