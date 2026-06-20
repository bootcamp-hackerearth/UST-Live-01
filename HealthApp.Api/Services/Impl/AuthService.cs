using AutoMapper;
using HealthApp.Api.Dtos;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Impl;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthApp.Api.Services.Impl
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        IConfiguration config,
        IMapper mapper,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository) : IAuthService
    {
        public async Task<(bool success, string message, string userId)> RegisterPatient(
            RegisterPatientDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Passwords do not match.", string.Empty);
            }

            var existingUser = await userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return (false, "Email is already registered.", string.Empty);
            }

            var existingPatients = await patientRepository.GetPatientsAsync(
                null,
                request.Email);

            if (existingPatients.Any(p =>
                    p.Email != null &&
                    p.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return (false, "Patient email is already registered.", string.Empty);
            }

            var patient = mapper.Map<Patient>(request);

            var createdPatient = await patientRepository.Add(patient);

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PatientId = createdPatient.PatientId,
                DoctorId = null
            };

            var createUserResult = await userManager.CreateAsync(user, request.Password);

            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(", ",
                    createUserResult.Errors.Select(e => e.Description));

                return (false, errors, string.Empty);
            }

            var roleResult = await userManager.AddToRoleAsync(user, "Patient");

            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(user);

                var errors = string.Join(", ",
                    roleResult.Errors.Select(e => e.Description));

                return (false, errors, string.Empty);
            }

            return (true, "Patient registered successfully.", user.Id);
        }

        public async Task<(bool success, string message, string userId, string temporaryPassword)> RegisterDoctor(DoctorCreateDto request)
        {
            var existingIdentityUser = await userManager.FindByEmailAsync(request.DoctorEmail);

            if (existingIdentityUser != null)
            {

                throw new DuplicateEntityException(
                    $"User with email '{request.DoctorEmail}' already exists.");
            }

            var doctorEmailExists = await doctorRepository.ExistsByEmailAsync(request.DoctorEmail);

            if (doctorEmailExists)
            {

                throw new DuplicateEntityException(
                    $"Doctor with email '{request.DoctorEmail}' already exists.");
            }

            var doctor = mapper.Map<Doctor>(request);

            await doctorRepository.Add(doctor);

            var temporaryPassword = GenerateTemporaryDoctorPassword(request.FullName);

            var user = new ApplicationUser
            {
                UserName = request.DoctorEmail,
                Email = request.DoctorEmail,
                PatientId = null,
                DoctorId = doctor.DoctorId
            };

            var createResult = await userManager.CreateAsync(user, temporaryPassword);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(" ", createResult.Errors.Select(error => error.Description));
                throw new BusinessRuleViolationException(errors);
            }

            var roleResult = await userManager.AddToRoleAsync(user, "Doctor");

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(" ", roleResult.Errors.Select(error => error.Description));
                throw new BusinessRuleViolationException(errors);
            }

            return (
                true,
                "Doctor registered successfully.",
                user.Id,
                temporaryPassword
            );
        }

        private static string GenerateTemporaryDoctorPassword(string doctorName)
        {
            if (string.IsNullOrWhiteSpace(doctorName))
            {
                throw new InvalidRequestException("Doctor name is required to generate temporary password.");
            }

            var cleanName = doctorName.Trim();

            if (cleanName.Length < 3)
            {
                throw new InvalidRequestException("Doctor name must contain at least 3 characters.");
            }

            var firstThreeLetters = cleanName.Substring(0, 3);

            var currentYear = DateTime.Now.Year;

            return $"{firstThreeLetters}@{currentYear}";
        }

        public async Task<(bool success, string message, string token, int expiresIn)> Login(
            LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return (false, "Invalid credentials.", string.Empty, 0);
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(
                user,
                request.Password);

            if (!isPasswordValid)
            {
                return (false, "Invalid credentials.", string.Empty, 0);
            }

            var token = await GenerateToken(user);

            var expiry = int.Parse(
                config.GetSection("Jwt")["AccessTokenExpirationMinutes"]!);

            return (true, "User logged in successfully.", token, expiry);
        }

        public async Task ChangePasswordAsync(string userId, ChangePasswordDto request)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessAppException("Please login to continue.");
            }

            if (request == null)
            {
                throw new InvalidRequestException("Password details are required.");
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                throw new InvalidRequestException("New password and confirm password do not match.");
            }

            if (request.CurrentPassword == request.NewPassword)
            {
                throw new InvalidRequestException("New password must be different from current password.");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new EntityNotFoundException("User", userId);
            }

            var result = await userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(" ",
                    result.Errors.Select(error => error.Description));

                throw new InvalidRequestException(errors);
            }
        }


        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var jwtSettings = config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }

            if (user.PatientId.HasValue)
            {
                claims.Add(new Claim("PatientId", user.PatientId.Value.ToString()));
            }

            if (user.DoctorId.HasValue)
            {
                claims.Add(new Claim("DoctorId", user.DoctorId.Value.ToString()));
            }

            claims.Add(new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expirationMinutes = int.Parse(
                jwtSettings["AccessTokenExpirationMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}