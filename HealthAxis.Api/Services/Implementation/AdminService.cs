using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

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
        public async Task<List<DoctorDto>> GetDoctorsAsync(
            CancellationToken ct = default
        ) =>
            mapper.Map<List<DoctorDto>>(
                await doctorRepository.GetAllAsync(ct)
            );

        public async Task<DoctorDto> CreateDoctorAsync(
            CreateDoctorDto request,
            CancellationToken ct = default
        )
        {
            if (await userManager.FindByEmailAsync(request.Email) != null)
                throw new InvalidException("Email already exists");

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
                EmailConfirmed = true
            };

            var cr = await userManager.CreateAsync(
                user,
                request.Password
            );

            if (!cr.Succeeded)
                throw new InvalidException(
                    string.Join(", ", cr.Errors.Select(e => e.Description))
                );

            if (!await roleManager.RoleExistsAsync("Doctor"))
                throw new InvalidException(
                    "Doctor role does not exist"
                );

            var rr = await userManager.AddToRoleAsync(
                user,
                "Doctor"
            );

            if (!rr.Succeeded)
                throw new InvalidException(
                    string.Join(", ", rr.Errors.Select(e => e.Description))
                );

            return mapper.Map<DoctorDto>(saved);
        }

        public async Task<DoctorDto> UpdateDoctorAsync(
            int id,
            UpdateDoctorDto request,
            CancellationToken ct = default
        )
        {
            var doctor = await doctorRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Doctor not found");

            mapper.Map(request, doctor);

            var updated = await doctorRepository.UpdateAsync(
                id,
                doctor,
                ct
            ) ?? throw new NotFoundException("Doctor not found");

            return mapper.Map<DoctorDto>(updated);
        }

        public async Task<List<UserDto>> GetUsersAsync(string? role)
        {
            var result = new List<UserDto>();

            foreach (var user in userManager.Users.ToList())
            {
                var roles = await userManager.GetRolesAsync(user);

                var userRole =
                    roles.FirstOrDefault() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(role) &&
                    userRole != role)
                    continue;

                result.Add(
                    new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email ?? string.Empty,
                        Role = userRole,
                        IsActive = user.IsActive
                    }
                );
            }

            return result;
        }

        public async Task<List<AppointmentReportDto>> GetAppointmentReportAsync(
            CancellationToken ct = default
        ) =>
            await appointmentRepository.GetAppointmentReportAsync(ct);

        public async Task UpdatePatientStatusAsync(
     int patientId,
     bool isActive,
     CancellationToken ct = default)
        {
            var patient = await patientRepository.GetByIdAsync(patientId, ct)
                ?? throw new NotFoundException("Patient not found");

            patient.IsActive = isActive;

            var user = userManager.Users.FirstOrDefault(u => u.PatientId == patientId);

            if (user != null)
            {
                user.IsActive = isActive;

                var result = await userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    throw new InvalidException(
                        string.Join(", ", result.Errors.Select(e => e.Description)));
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

            var user = userManager.Users.FirstOrDefault(u => u.DoctorId == doctorId);

            if (user != null)
            {
                user.IsActive = isActive;

                var result = await userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    throw new InvalidException(
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            await doctorRepository.SaveChangesAsync(ct);
        }
    }
}