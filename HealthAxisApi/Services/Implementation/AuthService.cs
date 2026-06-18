using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using HealthAxisCore_Api.DTOs.User;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Services.Interfaces;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Repositories;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HealthAxisCore_Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IPatientRepository _patientRepository;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IPatientRepository patientRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _patientRepository = patientRepository;
        }

        // ✅ REGISTER (ONLY PATIENT)
        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO request)
        {
            if (request.Password != request.ConfirmPassword)
                throw new BusinessRuleException("Passwords do not match");

            if (request.Role != "Patient")
                throw new BusinessRuleException("Only patients can register themselves");

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                throw new BusinessRuleException("User already exists");

            var patient = new Patient
            {
                PatientName = request.PatientName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                CreatedDate = DateTime.Now
            };

            await _patientRepository.AddAsync(patient);

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Role = "Patient",
                ReferenceId = patient.PatientId,
                IsFirstLogin = false,
                TemporaryPassword = null
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessRuleException(errors);
            }

            await _userManager.AddToRoleAsync(user, "Patient");

            var token = await GenerateJwtToken(user);

            return new AuthResponseDTO
            {
                Token = token,
                Email = user.Email!,
                Role = "Patient",
                ReferenceId = user.ReferenceId,
                IsFirstLogin = user.IsFirstLogin
            };
        }

        // ✅ LOGIN
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new UnauthorizedException("Invalid email or password");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
                throw new UnauthorizedException("Invalid email or password");

            var roles = await _userManager.GetRolesAsync(user);

            var token = await GenerateJwtToken(user);

            return new AuthResponseDTO
            {
                Token = token,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? "User",
                ReferenceId = user.ReferenceId,
                IsFirstLogin = user.IsFirstLogin
            };
        }

        // ✅ CHANGE PASSWORD
        public async Task ChangePasswordAsync(ChangePasswordDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new EntityNotFoundException("User not found");

            if (request.NewPassword == user.TemporaryPassword)
                throw new BusinessRuleException("New password cannot be same as temporary password");

            var result = await _userManager.ChangePasswordAsync(
                user,
                request.OldPassword,
                request.NewPassword
            );

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessRuleException(errors);
            }

            user.IsFirstLogin = false;
            user.TemporaryPassword = null;

            await _userManager.UpdateAsync(user);
        }

        // ✅ JWT TOKEN GENERATION
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            int expiryMinutes = int.Parse(jwtSettings["DurationInMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}