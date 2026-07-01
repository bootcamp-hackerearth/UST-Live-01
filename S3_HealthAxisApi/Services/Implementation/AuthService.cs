using Microsoft.IdentityModel.Tokens;
using S3_HealthAxis.Shared.DTOs.Auth;
using S3_HealthAxis.Shared.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace S3_HealthAxisApi.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IPatientRepository _patientRepository;

        public AuthService(
            IUserRepository userRepository,
            IPatientRepository patientRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _patientRepository = patientRepository;
            _configuration = configuration;
        }

        public async Task<(bool Success, string Message, AuthResponseDto? Data)> RegisterAsync(
            RegisterDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Passwords do not match.", null);
            }

            var email = request.Email.Trim().ToLower();

            if (await _userRepository.EmailExistsAsync(email))
            {
                return (false, "Email already exists.", null);
            }

            var user = new User
            {
                Email = email,
                PasswordHash = HashPassword(request.Password),
                Role = request.Role,
                CreatedDate = DateTime.UtcNow,
                MustChangePassword = false
            };

            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var accessToken = GenerateToken(user);

            return (
                true,
                "User registered successfully.",
                new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    ReferenceId = user.ReferenceId,
                    MustChangePassword = user.MustChangePassword
                });
        }

        public async Task<(bool Success, string Message, AuthResponseDto? Data)> RegisterPatientAsync(
            RegisterPatientDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Passwords do not match.", null);
            }

            var email = request.Email.Trim().ToLower();

            if (await _userRepository.EmailExistsAsync(email))
            {
                return (false, "Email already exists.", null);
            }

            var patient = new Patient
            {
                FullName = request.FullName.Trim(),
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber.Trim(),
                Email = email,
                InsuranceNumber = request.InsuranceNumber,
                IsActive = true
            };

            await _patientRepository.AddAsync(patient);
            await _patientRepository.SaveChangesAsync();

            var user = new User
            {
                Email = email,
                PasswordHash = HashPassword(request.Password),
                Role = UserRole.Patient,
                ReferenceId = patient.PatientId,
                CreatedDate = DateTime.UtcNow,
                MustChangePassword = false
            };

            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var accessToken = GenerateToken(user);

            return (
                true,
                "Patient registered successfully.",
                new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    ReferenceId = user.ReferenceId,
                    MustChangePassword = user.MustChangePassword
                });
        }

        public async Task<(bool Success, string Message, AuthResponseDto? Data)> LoginAsync(
            LoginDto request)
        {
            var email = request.Email.Trim().ToLower();

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                return (false, "Invalid email or password.", null);
            }

            var hashedPassword = HashPassword(request.Password);

            if (user.PasswordHash != hashedPassword)
            {
                return (false, "Invalid email or password.", null);
            }

            var accessToken = GenerateToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return (
                true,
                "Login successful.",
                new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    ReferenceId = user.ReferenceId,
                    MustChangePassword = user.MustChangePassword
                });
        }

        public async Task<(bool Success, string Message, AuthResponseDto? Data)> RefreshTokenAsync(
            RefreshTokenDto request)
        {
            var user =
                await _userRepository.GetByRefreshTokenAsync(
                    request.RefreshToken);

            if (user == null)
            {
                return (false, "Invalid refresh token.", null);
            }

            if (!user.RefreshTokenExpiryTime.HasValue ||
                user.RefreshTokenExpiryTime.Value <= DateTime.UtcNow)
            {
                return (false, "Refresh token has expired.", null);
            }

            var newAccessToken = GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return (
                true,
                "Token refreshed successfully.",
                new AuthResponseDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    ReferenceId = user.ReferenceId,
                    MustChangePassword = user.MustChangePassword
                });
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(
    string email,
    ChangePasswordDto request)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, "Invalid authenticated user.");
            }

            if (string.IsNullOrWhiteSpace(request.CurrentPassword))
            {
                return (false, "Current password is required.");
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return (false, "New password is required.");
            }

            if (string.IsNullOrWhiteSpace(request.ConfirmNewPassword))
            {
                return (false, "Confirm password is required.");
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return (false, "New password and confirm password do not match.");
            }

            if (request.CurrentPassword == request.NewPassword)
            {
                return (false, "New password cannot be the same as current password.");
            }

            var passwordValidationMessage = ValidatePasswordStrength(request.NewPassword);

            if (!string.IsNullOrWhiteSpace(passwordValidationMessage))
            {
                return (false, passwordValidationMessage);
            }

            var user =
                await _userRepository.GetByEmailAsync(
                    email.Trim().ToLower());

            if (user == null)
            {
                return (false, "User account not found.");
            }

            var currentPasswordHash = HashPassword(request.CurrentPassword);

            if (user.PasswordHash != currentPasswordHash)
            {
                return (false, "Current password is incorrect.");
            }

            user.PasswordHash = HashPassword(request.NewPassword);

            user.MustChangePassword = false;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return (true, "Password changed successfully.");
        }

        private static string? ValidatePasswordStrength(string password)
        {
            if (password.Length < 8)
            {
                return "Password must be at least 8 characters long.";
            }

            if (!password.Any(char.IsUpper))
            {
                return "Password must contain at least one uppercase letter.";
            }

            if (!password.Any(char.IsLower))
            {
                return "Password must contain at least one lowercase letter.";
            }

            if (!password.Any(char.IsDigit))
            {
                return "Password must contain at least one number.";
            }

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                return "Password must contain at least one special character.";
            }

            return null;
        }

        private string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.UserId.ToString()),

                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email),

                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()),

                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                new Claim(
                    ClaimTypes.Role,
                    user.Role.ToString()),

                new Claim(
                    "ReferenceId",
                    user.ReferenceId.ToString()),

                new Claim(
                    "MustChangePassword",
                    user.MustChangePassword.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(jwtSettings["AccessTokenExpirationMinutes"]!)),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(64));
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}