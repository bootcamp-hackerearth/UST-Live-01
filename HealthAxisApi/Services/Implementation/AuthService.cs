using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using HealthAxisCore_Api.DTOs.User;

using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Services.Interfaces;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Repositories;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using HealthAxisCore_Api.Data;
using HealthAxis.Shared.DTOs.Auth;
using HealthAxis.Shared.DTOs.User;

namespace HealthAxisCore_Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IPatientRepository _patientRepository;
        private readonly HealthAppDbContext _context;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IPatientRepository patientRepository,
            HealthAppDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _patientRepository = patientRepository;
            _context = context;
        }

        // ✅ REGISTER (UNCHANGED)
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
                throw new BusinessRuleException(string.Join(", ", result.Errors.Select(e => e.Description)));

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

        // ✅ LOGIN (UPDATED WITH REFRESH TOKEN)
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new UnauthorizedException("Invalid email or password");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
                throw new UnauthorizedException("Invalid email or password");

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = await GenerateJwtToken(user);

            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                Token = accessToken,
                RefreshToken = refreshToken.Token,   // ✅ NEW
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? "User",
                ReferenceId = user.ReferenceId,
                IsFirstLogin = user.IsFirstLogin
            };
        }

        // ✅ REFRESH TOKEN (NEW)
        public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (token == null || token.IsExpired || token.IsRevoked)
                throw new UnauthorizedException("Invalid or expired refresh token");

            var newAccessToken = await GenerateJwtToken(token.User);

            var roles = await _userManager.GetRolesAsync(token.User);

            return new AuthResponseDTO
            {
                Token = newAccessToken,
                RefreshToken = refreshToken,
                Email = token.User.Email!,
                Role = roles.FirstOrDefault() ?? "User",
                ReferenceId = token.User.ReferenceId,
                IsFirstLogin = token.User.IsFirstLogin
            };
        }

        // ✅ LOGOUT (REVOKE TOKEN)
        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (token == null)
                throw new EntityNotFoundException("Token not found");

            token.IsRevoked = true;
            token.Revoked = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // ✅ CHANGE PASSWORD (UNCHANGED)
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
                throw new BusinessRuleException(string.Join(", ", result.Errors.Select(e => e.Description)));

            user.IsFirstLogin = false;
            user.TemporaryPassword = null;

            await _userManager.UpdateAsync(user);
        }

        // ✅ JWT TOKEN
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

        // ✅ REFRESH TOKEN GENERATOR
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}
