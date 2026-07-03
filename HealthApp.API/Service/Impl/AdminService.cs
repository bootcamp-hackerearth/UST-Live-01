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
using System.Security.Cryptography;

namespace HealthApp.API.Service.Impl;

public class AdminService(
    IDoctorRepository doctorRepository,
    IPatientRepository patientRepository,
    IAppointmentRepository appointmentRepository,
    UserManager<ApplicationUser> userManager,
    IMapper mapper) : IAdminService
{
    public async Task<PagedResultDto<DoctorDto>> GetDoctorsAsync(
     PaginationQueryDto? pagination = null)
    {
        pagination ??= new PaginationQueryDto();

        var result = await doctorRepository.GetPagedAsync(
            pagination.PageNumber,
            pagination.PageSize);

        return new PagedResultDto<DoctorDto>
        {
            Items = mapper.Map<List<DoctorDto>>(result.Items),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<CreateDoctorResponseDto> CreateDoctorAsync(CreateDoctorDto dto)
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

        var temporaryPassword = GenerateTemporaryPassword();

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
            temporaryPassword);

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

            return new CreateDoctorResponseDto
            {
                Message = "Doctor registered successfully. Share the temporary password securely.",
                Doctor = mapper.Map<DoctorDto>(savedDoctor),
                TemporaryPassword = temporaryPassword
            };
        }
        catch
        {
            await userManager.DeleteAsync(doctorUser);
            throw;
        }
    }

    private static string GenerateTemporaryPassword()
    {
        const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string lower = "abcdefghijkmnopqrstuvwxyz";
        const string digits = "23456789";
        const string symbols = "@#$!";
        const string all = upper + lower + digits + symbols;

        var passwordChars = new List<char>
    {
        GetRandomChar(upper),
        GetRandomChar(lower),
        GetRandomChar(digits),
        GetRandomChar(symbols)
    };

        while (passwordChars.Count < 10)
        {
            passwordChars.Add(GetRandomChar(all));
        }

        return new string(passwordChars
            .OrderBy(_ => RandomNumberGenerator.GetInt32(int.MaxValue))
            .ToArray());
    }

    private static char GetRandomChar(string source)
    {
        return source[RandomNumberGenerator.GetInt32(source.Length)];
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

    public async Task<PagedResultDto<AppointmentReportDto>> GetAppointmentReportsAsync(
    PaginationQueryDto? pagination = null)
    {
        pagination ??= new PaginationQueryDto();

        var all = await appointmentRepository.GetAllAsync();

        var reports = all
            .GroupBy(a => a.ScheduledDate.Date)
            .OrderByDescending(g => g.Key)
            .Select(g => new AppointmentReportDto
            {
                Date = g.Key,
                Pending = g.Count(a => a.Status == AppointmentStatus.Pending.ToString()),
                Confirmed = g.Count(a => a.Status == AppointmentStatus.Confirmed.ToString()),
                Cancelled = g.Count(a => a.Status == AppointmentStatus.Cancelled.ToString()),
                Completed = g.Count(a => a.Status == AppointmentStatus.Completed.ToString())
            })
            .ToList();

        var totalCount = reports.Count;

        var items = reports
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        return new PagedResultDto<AppointmentReportDto>
        {
            Items = items,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        };
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

        if (!Enum.IsDefined(dto.Specialisation))
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


        if (!Enum.IsDefined(dto.Specialisation))
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

    public async Task<PagedResultDto<PatientDto>> GetPatientsAsync(
    string? search = null,
    GenderType? gender = null,
    bool? hasInsurance = null,
    PaginationQueryDto? pagination = null)
    {
        if (gender.HasValue && !Enum.IsDefined(gender.Value))
        {
            throw new BusinessRuleException("Invalid gender filter.");
        }

        pagination ??= new PaginationQueryDto();

        var result = await patientRepository.GetFilteredPagedAsync(
            search,
            gender,
            hasInsurance,
            pagination.PageNumber,
            pagination.PageSize);

        return new PagedResultDto<PatientDto>
        {
            Items = mapper.Map<List<PatientDto>>(result.Items),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = result.TotalCount
        };
    }
}
