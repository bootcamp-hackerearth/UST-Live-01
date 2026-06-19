using HealthAxis.API.DTO.AuthDtos;
using HealthAxis.API.Models;
using HealthAxis.API.Enums;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthAxis.API.Services.Implementation
{
    public class AuthService(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config,
        IRepository<Patient> patientRepository) : IAuthService
    {
        public async Task<(bool Success, string Message, string Token, int ExpiresIn)> Login(
            LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return (false, "Invalid credentials", string.Empty, 0);
            }

            var isPasswordValid =
                await userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return (false, "Invalid credentials", string.Empty, 0);
            }

            var token = await GenerateToken(user);

            var expiry =
                int.Parse(config.GetSection("Jwt")["AccessTokenExpirationMinutes"]!);

            return (true, "User logged in successfully", token, expiry);
        }

        public async Task<(bool Success, string Message, string UserId, int StatusCode)> Register(
    RegisterDto request)
        {
            var patients = await patientRepository.GetAllAsync();

            var emailExists = patients
                .Any(p => p.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));

            if (emailExists)
            {
                return (false, "Email already registered", string.Empty, 400);
            }

            var phoneExists = patients
                .Any(p => p.PhoneNumber == request.PhoneNumber);

            if (phoneExists)
            {
                return (false, "Phone number already registered", string.Empty, 400);
            }

            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Password and Confirm Password do not match", string.Empty, 400);
            }

            var identityUser = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await userManager.CreateAsync(identityUser, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errors, string.Empty, 400);
            }

            await userManager.AddToRoleAsync(identityUser, "Patient");

            var patient = new Patient
            {
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                UserId = identityUser.Id,
                CreatedDate = DateTime.Now
            };

            await patientRepository.AddAsync(patient);

            return (true, "Patient registered successfully", identityUser.Id, 200);
        }

        private async Task<string> GenerateToken(IdentityUser user)
        {
            var jwtSettings = config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expirationMinutes =
                int.Parse(jwtSettings["AccessTokenExpirationMinutes"]!);

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