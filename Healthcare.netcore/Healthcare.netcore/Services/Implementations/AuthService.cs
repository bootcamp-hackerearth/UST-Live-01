using HealthAxis.API.Models;
using HealthAxis.API.Models.Auth;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthAxis.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IConfiguration _config;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IRepository<Patient> patientRepository,
            IConfiguration config)
        {
            _userManager = userManager;
            _patientRepository = patientRepository;
            _config = config;
        }

        public async Task<(bool Success, string Message, string UserId)> Register(RegisterDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Passwords do not match", string.Empty);
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                MustChangePassword = false
            };

            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                return (false, errors, string.Empty);
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, "Patient");

            if (!addRoleResult.Succeeded)
            {
                var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                return (false, errors, string.Empty);
            }

            var patient = new Patient
            {
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                InsuranceId = request.InsuranceId,
                CreatedDate = DateTime.Now
            };

            await _patientRepository.AddAsync(patient);

            return (true, "Patient registered successfully", user.Id);
        }

        public async Task<(bool Success, string Message, string Token, int ExpiresIn, bool RequiresPasswordChange)> Login(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return (false, "Invalid credentials", string.Empty, 0, false);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return (false, "Invalid credentials", string.Empty, 0, false);
            }

            if (user.MustChangePassword)
            {
                return (true, "Password change required", string.Empty, 0, true);
            }

            var token = await GenerateToken(user);

            var expiresIn = int.Parse(
                _config.GetSection("Jwt")["AccessTokenExpirationMinutes"]!);

            return (true, "Login successful", token, expiresIn, false);
        }

        public async Task<(bool Success, string Message)> ChangePassword(ChangePasswordDto request)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                return (false, "New password and confirm password do not match");
            }

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return (false, "User not found");
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                request.OldPassword,
                request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            user.MustChangePassword = false;

            await _userManager.UpdateAsync(user);

            return (true, "Password changed successfully");
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var jwt = _config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

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

            var expirationMinutes = int.Parse(
                jwt["AccessTokenExpirationMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}