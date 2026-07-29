using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using HealthCare.Shared.DTOs.Authentication;
using HealthCare.Shared.DTOs.Doctor;
using HealthCare.Shared.DTOs.Patient;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Serilog.Core;
using System.Data;


namespace HealthCare.Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;
        private readonly HealthCareDbContext _context;


#pragma warning disable S107
        public AuthService(
            UserManager<IdentityUser> userManager,
            IMapper mapper,
            IPatientRepository patientRepo,
            IDoctorRepository doctorRepo,
            IJwtService jwtService,
            ILogger<AuthService> logger,
            HealthCareDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _patientRepo = patientRepo;
            _doctorRepo = doctorRepo;
            _jwtService = jwtService;
            _context = context;
            _logger = logger;
        }
#pragma warning restore S107

        private async Task<IdentityUser> CreateUserWithRoleAsync(
     string email,
     string password,
     string role)
        {
            // Check whether the email already exists in AspNetUsers
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exists");
            }

            // Create Identity user
            var user = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
            }

            // Assign role
            var roleResult = await _userManager.AddToRoleAsync(user, role);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", roleResult.Errors.Select(e => e.Description))
                );
            }

            return user;
        }

        public async Task RegisterPatientAsync(CreatePatientDto dto)
        {
            var user = await CreateUserWithRoleAsync(dto.Email, dto.Password, "Patient");

            // Create Patient entity
            var patient = _mapper.Map<Patient>(dto);
            patient.UserId = user.Id;

            await _patientRepo.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task RegisterDoctorAsync(CreateDoctorDto dto)
        {
            var user = await CreateUserWithRoleAsync(dto.Email, dto.Password, "Doctor");

            // Create Doctor entity
            var doctor = _mapper.Map<Doctor>(dto);
            doctor.UserId = user.Id;

            await _doctorRepo.AddAsync(doctor);

            await _context.SaveChangesAsync();

            await _doctorRepo.CreateSlots(doctor.DoctorId, dto.TimeSlots);

            await _context.SaveChangesAsync();

        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // Find user
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new InvalidOperationException("User does not exist");

            // Verify password
            var isValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isValid)
                throw new UnauthorizedAccessException("Invalid credentials");

            // Get roles
            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any())
            {
                throw new InvalidOperationException("Role is not assigned.");
            }

            // Getting patient or doctor Id using UserId
            string token;
            var role = roles[0];
            if (role == "Patient")
            {
                var patient = await _patientRepo.GetByUserIdAsync(user.Id);

                if (patient == null)
                    throw new InvalidOperationException("Patient record not found.");

                // Generate JWT
                token = await _jwtService.GenerateToken(user, patientId: patient.PatientId);
            }
            else if (role == "Doctor")
            {
                var doctor = await _doctorRepo.GetByUserIdAsync(user.Id);

                if (doctor == null)
                    throw new InvalidOperationException("Doctor record not found.");

                // Generate JWT
                token = await _jwtService.GenerateToken(user, doctorId: doctor.DoctorId);
            }
            else if (role == "Admin")
            {
                // Generate JWT
                token = await _jwtService.GenerateToken(user);
            }
            else
            {
                throw new InvalidOperationException("Invalid role.");
            }


            var response = new AuthResponseDto
            {
                AccessToken = token,
                Role = role
            };

            return response;
        }

        public async Task ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new InvalidOperationException("User not found");

            var result = await _userManager.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword
            );

            if (!result.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
        }

        public async Task UpdatePatientEmailAsync(string userId, string newEmail)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new InvalidOperationException("User not found");

            // CHECK UNIQUE EMAIL (IDENTITY TABLE)
            var existingUser = await _userManager.FindByEmailAsync(newEmail);

            if (existingUser != null && existingUser.Id != userId)
                throw new InvalidOperationException("Email already exists");

            // UPDATE EMAIL
            user.Email = newEmail;
            user.UserName = newEmail;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
            }
        }

    }
}