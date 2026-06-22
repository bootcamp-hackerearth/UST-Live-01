using AutoMapper;
using HealthCareApp.Dtos;
using HealthCareApp.Models;
using HealthCareApp.Models.Dtos;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthCareApp.Services.Impl
{
    public class AuthService(
        UserManager<IdentityUser> userManager,
        IPatientRepository patientRepository,
        IConfiguration config) : IAuthService
    {
        public async Task<(bool Success, string Message, int PatientId)> RegisterPatientAsync(PatientRegisterDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Password and Confirm Password do not match.", 0);
            }

            if (request.DateOfBirth.Date > DateTime.Today)
            {
                return (false, "Date of birth cannot be a future date.", 0);
            }

            var existingUser = await userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return (false, "Email is already registered.", 0);
            }

            var identityUser = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };

            var createUserResult = await userManager.CreateAsync(identityUser, request.Password);

            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(",", createUserResult.Errors.Select(e => e.Description));
                return (false, errors, 0);
            }

            var roleResult = await userManager.AddToRoleAsync(identityUser, "Patient");

            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(identityUser);

                var errors = string.Join(",", roleResult.Errors.Select(e => e.Description));
                return (false, errors, 0);
            }

            var patient = new Patient
            {
                PatientName = request.FullName,
                DateOfBirth = request.DateOfBirth.Date,
                Gender = request.Gender,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                InsuranceID = request.InsuranceId,
                IdentityUserId = identityUser.Id,
                CreatedDate = DateTime.Now
            };

            var savedPatient = await patientRepository.CreateAsync(patient);

            return (true, "Patient registered successfully.", savedPatient.PatientId);
        }

        public async Task<(bool Success, string Message, string Token, int ExpiresIn)> Login(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return (false, "Invalid Credentials", string.Empty, 0);
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return (false, "Invalid Credentials", string.Empty, 0);
            }

            var token = await GenerateToken(user);

            var expiry = int.Parse(config.GetSection("Jwt")["AccessTokenExpirationMinutes"]!);

            return (true, "Login Successful", token, expiry);
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(string userId, ChangePasswordDto request)
        {
            if (string.Equals(request.CurrentPassword, request.NewPassword, StringComparison.Ordinal))
            {
                return (false, "New password cannot be the same as current password.");
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return (false, "New Password and Confirm New Password do not match.");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return (false, "User not found.");
            }

            var result = await userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            return (true, "Password changed successfully.");
        }

        private async Task<string> GenerateToken(IdentityUser user)
        {
            var jwtSettings = config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expirationMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}