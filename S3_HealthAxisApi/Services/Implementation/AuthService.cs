using Microsoft.IdentityModel.Tokens;
using S3_HealthAxisApi.DTOs.Auth;
using S3_HealthAxisApi.Enums;
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

        public async Task<(bool Success, string Message, AuthResponseDto? Data)> RegisterAsync(RegisterDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Passwords do not match.", null);
            }

            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                return (false, "Email already exists.", null);
            }

            var user = new User
            {
                Email = request.Email.Trim().ToLower(),
                PasswordHash = HashPassword(request.Password),
                Role = request.Role,
                CreatedDate = DateTime.UtcNow
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
                    Role = user.Role.ToString()
                });
        }

        public async Task<(bool Success,string Message, AuthResponseDto? Data)>RegisterPatientAsync(RegisterPatientDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Passwords do not match.", null);
            }

            if (await _userRepository.EmailExistsAsync(
                request.Email))
            {
                return (false, "Email already exists.", null);
            }

            var patient = new Patient
            {
                FullName = request.FullName.Trim(),
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber.Trim(),
                Email = request.Email.Trim().ToLower(),
                InsuranceNumber = request.InsuranceNumber,
                IsActive = true
            };

            await _patientRepository.AddAsync(patient);
            await _patientRepository.SaveChangesAsync();

            var user = new User
            {
                Email = request.Email.Trim().ToLower(),
                PasswordHash = HashPassword(request.Password),
                Role = UserRole.Patient,
                ReferenceId = patient.PatientId
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var accessToken =
                GenerateToken(user);

            var refreshToken =
                GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime =
                DateTime.UtcNow.AddDays(7);

            await _userRepository.SaveChangesAsync();

            return (
                true,
                "Patient registered successfully.",
                new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Email = user.Email,
                    Role = user.Role.ToString()
                });
        }

        public async Task<(bool Success, string Message, AuthResponseDto? Data)>
            LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetByEmailAsync(
                request.Email.Trim().ToLower());

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
                    Role = user.Role.ToString()
                });
        }

        public async Task<(bool Success, string Message, AuthResponseDto? Data)>
            RefreshTokenAsync(RefreshTokenDto request)
        {
            var user = await _userRepository
                .GetByRefreshTokenAsync(request.RefreshToken);

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
                    Role = user.Role.ToString()
                });
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
                    user.Role.ToString())
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

            byte[] bytes = Encoding.UTF8.GetBytes(password);

            byte[] hash = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}