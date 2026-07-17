using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class AdminService(
        IDoctorRepository doctorRepository,
        IPatientRepository patientRepository,
        IAppointmentRepository appointmentRepository,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper
    ) : IAdminService
    {
        public async Task<PagedResultDto<DoctorDto>> GetDoctorsAsync(
            PaginationQueryDto query,
            CancellationToken ct = default)
        {
            var doctors = await doctorRepository.GetAllAsync(ct);

            var orderedDoctors = doctors
                .OrderBy(doctor => doctor.DoctorId)
                .ToList();

            var totalCount = orderedDoctors.Count;

            var pagedDoctors = orderedDoctors
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            var doctorDtos = mapper.Map<List<DoctorDto>>(pagedDoctors);

            return CreatePagedResult(
                doctorDtos,
                query.PageNumber,
                query.PageSize,
                totalCount);
        }

        public async Task<DoctorDto> CreateDoctorAsync(
            CreateDoctorDto request,
            CancellationToken ct = default)
        {
            if (await userManager.FindByEmailAsync(request.Email) != null)
            {
                throw new InvalidException("Email already exists");
            }

            var doctor = mapper.Map<Doctor>(request);

            doctor.IsActive = true;

            var saved = await doctorRepository.CreateAsync(doctor, ct);

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DoctorId = saved.DoctorId,
                IsActive = true,
                EmailConfirmed = true,
                FirstLogin = true
            };

            var createResult = await userManager.CreateAsync(
                user,
                request.Password);

            if (!createResult.Succeeded)
            {
                throw new InvalidException(
                    string.Join(
                        ", ",
                        createResult.Errors.Select(error => error.Description)));
            }

            if (!await roleManager.RoleExistsAsync("Doctor"))
            {
                throw new InvalidException("Doctor role does not exist");
            }

            var roleResult = await userManager.AddToRoleAsync(
                user,
                "Doctor");

            if (!roleResult.Succeeded)
            {
                throw new InvalidException(
                    string.Join(
                        ", ",
                        roleResult.Errors.Select(error => error.Description)));
            }

            return mapper.Map<DoctorDto>(saved);
        }

        public async Task<DoctorDto> UpdateDoctorAsync(
            int id,
            UpdateDoctorDto request,
            CancellationToken ct = default)
        {
            var doctor = await doctorRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Doctor not found");

            mapper.Map(request, doctor);

            var updated = await doctorRepository.UpdateAsync(
                id,
                doctor,
                ct)
                ?? throw new NotFoundException("Doctor not found");

            return mapper.Map<DoctorDto>(updated);
        }

        public async Task<PagedResultDto<UserDto>> GetUsersAsync(
            string? role,
            PaginationQueryDto query,
            CancellationToken ct = default)
        {
            if (!string.IsNullOrWhiteSpace(role))
            {
                return await GetUsersByRolePagedAsync(role, query, ct);
            }

            return await GetAllUsersPagedAsync(query, ct);
        }

        private async Task<PagedResultDto<UserDto>> GetAllUsersPagedAsync(
            PaginationQueryDto query,
            CancellationToken ct)
        {
            var usersQuery = userManager.Users
                .AsNoTracking()
                .OrderBy(user => user.Email);

            var totalCount = await usersQuery.CountAsync(ct);

            var pagedUsers = await usersQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(ct);

            var userDtos = new List<UserDto>();

            foreach (var user in pagedUsers)
            {
                var userDto = await BuildUserDtoAsync(user, ct);

                userDtos.Add(userDto);
            }

            return CreatePagedResult(
                userDtos,
                query.PageNumber,
                query.PageSize,
                totalCount);
        }

        private async Task<PagedResultDto<UserDto>> GetUsersByRolePagedAsync(
            string role,
            PaginationQueryDto query,
            CancellationToken ct)
        {
            var normalizedRole = role.Trim();

            var usersInRole = await userManager.GetUsersInRoleAsync(normalizedRole);

            var orderedUsers = usersInRole
                .OrderBy(user => user.Email)
                .ToList();

            var totalCount = orderedUsers.Count;

            var pagedUsers = orderedUsers
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            var userDtos = new List<UserDto>();

            foreach (var user in pagedUsers)
            {
                var userDto = await BuildUserDtoAsync(user, ct);

                userDtos.Add(userDto);
            }

            return CreatePagedResult(
                userDtos,
                query.PageNumber,
                query.PageSize,
                totalCount);
        }

        private async Task<UserDto> BuildUserDtoAsync(
            ApplicationUser user,
            CancellationToken ct)
        {
            var roles = await userManager.GetRolesAsync(user);

            var userRole = roles.FirstOrDefault() ?? string.Empty;

            var fullName = await GetUserFullNameAsync(user, ct);

            return new UserDto
            {
                Id = user.Id,
                FullName = fullName,
                Email = user.Email ?? string.Empty,
                Role = userRole,
                IsActive = user.IsActive
            };
        }

        private async Task<string> GetUserFullNameAsync(
            ApplicationUser user,
            CancellationToken ct)
        {
            if (user.PatientId.HasValue)
            {
                var patient = await patientRepository.GetByIdAsync(
                    user.PatientId.Value,
                    ct);

                if (patient is not null)
                {
                    return patient.PatientName;
                }
            }

            if (user.DoctorId.HasValue)
            {
                var doctor = await doctorRepository.GetByIdAsync(
                    user.DoctorId.Value,
                    ct);

                if (doctor is not null)
                {
                    return doctor.DoctorName;
                }
            }

            return user.UserName ?? user.Email ?? string.Empty;
        }

        public async Task<List<AppointmentReportDto>> GetAppointmentReportAsync(
            CancellationToken ct = default)
        {
            return await appointmentRepository.GetAppointmentReportAsync(ct);
        }

        public async Task UpdatePatientStatusAsync(
            int patientId,
            bool isActive,
            CancellationToken ct = default)
        {
            var patient = await patientRepository.GetByIdAsync(patientId, ct)
                ?? throw new NotFoundException("Patient not found");

            patient.IsActive = isActive;

            var user = await userManager.Users
                .FirstOrDefaultAsync(
                    user => user.PatientId == patientId,
                    ct);

            if (user != null)
            {
                user.IsActive = isActive;

                var result = await userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    throw new InvalidException(
                        string.Join(
                            ", ",
                            result.Errors.Select(error => error.Description)));
                }
            }

            await patientRepository.SaveChangesAsync(ct);
        }

        public async Task UpdateDoctorStatusAsync(
            int doctorId,
            bool isActive,
            CancellationToken ct = default)
        {
            var doctor = await doctorRepository.GetByIdAsync(doctorId, ct)
                ?? throw new NotFoundException("Doctor not found");

            doctor.IsActive = isActive;

            var user = await userManager.Users
                .FirstOrDefaultAsync(
                    user => user.DoctorId == doctorId,
                    ct);

            if (user != null)
            {
                user.IsActive = isActive;

                var result = await userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    throw new InvalidException(
                        string.Join(
                            ", ",
                            result.Errors.Select(error => error.Description)));
                }
            }

            await doctorRepository.SaveChangesAsync(ct);
        }

        private static PagedResultDto<T> CreatePagedResult<T>(
            List<T> items,
            int pageNumber,
            int pageSize,
            int totalCount)
        {
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResultDto<T>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }
    }
}