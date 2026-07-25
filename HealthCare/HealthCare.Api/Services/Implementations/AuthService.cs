using AutoMapper;

using HealthCare.Api.Data;
using HealthCare.Api.DTOs.Auth;
using HealthCare.Api.DTOs.Doctor;
using HealthCare.Api.DTOs.Patient;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Numerics;


namespace HealthCare.Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IJwtService _jwtService;
        private readonly HealthCareDbContext _context;
   
        public AuthService(
            UserManager<User> userManager,
            IMapper mapper,
            IPatientRepository patientRepo,
            IDoctorRepository doctorRepo,
            IJwtService jwtService,
            HealthCareDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _patientRepo = patientRepo;
            _doctorRepo = doctorRepo;
            _jwtService = jwtService;
            _context = context;
        
        }

        private async Task<User> CreateUserWithRoleAsync(string email, string password, string role)
        {
            // Check email
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already in use");

            // Create user
            var user = new User
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

            // Assign role
            await _userManager.AddToRoleAsync(user, role);

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
                throw new UnauthorizedAccessException("Invalid email or password");
            // Verify password
            var isValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isValid)
                throw new UnauthorizedAccessException("Invalid email or password");

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

    }
}