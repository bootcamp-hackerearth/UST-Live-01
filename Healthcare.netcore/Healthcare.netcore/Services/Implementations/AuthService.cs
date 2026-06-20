using HealthAxis.API.Data;
using HealthAxis.API.Models;
using HealthAxis.API.Models.Auth;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HealthAxis.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Patient> _patientRepository;
        private readonly HealthAxisDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IRepository<Patient> patientRepository,
            HealthAxisDbContext context,
            IConfiguration config)
        {
            _userManager = userManager;
            _patientRepository = patientRepository;
            _context = context;
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
                UserId = user.Id,
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

        public async Task<(bool Success, string Message, string AccessToken, string RefreshToken, int ExpiresIn, bool RequiresPasswordChange)> Login(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return (false, "Invalid credentials", string.Empty, string.Empty, 0, false);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return (false, "Invalid credentials", string.Empty, string.Empty, 0, false);
            }

            if (user.MustChangePassword)
            {
                return (true, "Password change required", string.Empty, string.Empty, 0, true);
            }

            var accessToken = await GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            await SaveRefreshToken(user.Id, refreshToken);

            var expiresIn = int.Parse(
                _config.GetSection("Jwt")["AccessTokenExpirationMinutes"]!);

            return (true, "Login successful", accessToken, refreshToken, expiresIn, false);
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

        public async Task<(bool Success, string Message, string AccessToken, string RefreshToken, int ExpiresIn)> RefreshToken(RefreshTokenRequestDto request)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

            if (storedToken == null)
            {
                return (false, "Invalid refresh token", string.Empty, string.Empty, 0);
            }

            if (storedToken.IsRevoked)
            {
                return (false, "Refresh token is revoked", string.Empty, string.Empty, 0);
            }

            if (storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return (false, "Refresh token expired", string.Empty, string.Empty, 0);
            }

            var user = await _userManager.FindByIdAsync(storedToken.UserId);

            if (user == null)
            {
                return (false, "User not found", string.Empty, string.Empty, 0);
            }

            storedToken.IsRevoked = true;

            var newAccessToken = await GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();

            await SaveRefreshToken(user.Id, newRefreshToken);

            await _context.SaveChangesAsync();

            var expiresIn = int.Parse(
                _config.GetSection("Jwt")["AccessTokenExpirationMinutes"]!);

            return (true, "Token refreshed successfully", newAccessToken, newRefreshToken, expiresIn);
        }

        private async Task<string> GenerateAccessToken(ApplicationUser user)
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

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        private async Task SaveRefreshToken(string userId, string token)
        {
            var refreshTokenExpirationDays =
                int.TryParse(
                    _config.GetSection("Jwt")["RefreshTokenExpirationDays"],
                    out var days)
                    ? days
                    : 7;

            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = token,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays),
                IsRevoked = false
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}