using HealthAxis.API.DTOs.Auth;
using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using HealthAxis.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HealthAxis.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IPatientRepository _patientRepository;

        private const string LoginProvider = "HealthAxis";
        private const string RefreshTokenName = "RefreshToken";
        private const string RefreshTokenExpiryName = "RefreshTokenExpiryTime";

        public AuthService(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            IPatientRepository patientRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _patientRepository = patientRepository;
        }

        public async Task<(bool Success, string Message, string UserId)> RegisterAsync(
            RegisterDto request,
            CancellationToken ct = default)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (
                    false,
                    "Password and confirm password do not match.",
                    string.Empty);
            }

            IdentityUser? existingUserByEmail =
                await _userManager.FindByEmailAsync(request.Email);

            if (existingUserByEmail != null)
            {
                return (
                    false,
                    "Email already exists.",
                    string.Empty);
            }

            IdentityUser? existingUserByName =
                await _userManager.FindByNameAsync(request.UserName);

            if (existingUserByName != null)
            {
                return (
                    false,
                    "Username already exists.",
                    string.Empty);
            }

            IdentityUser user = new()
            {
                UserName = request.UserName,
                Email = request.Email
            };

            IdentityResult createUserResult =
                await _userManager.CreateAsync(user, request.Password);

            if (!createUserResult.Succeeded)
            {
                string errorMessage = string.Join(
                    ", ",
                    createUserResult.Errors.Select(error => error.Description));

                return (
                    false,
                    errorMessage,
                    string.Empty);
            }

            IdentityResult roleClaimResult =
                await _userManager.AddClaimAsync(
                    user,
                    new Claim(ClaimTypes.Role, request.Role.ToString()));

            if (!roleClaimResult.Succeeded)
            {
                string errorMessage = string.Join(
                    ", ",
                    roleClaimResult.Errors.Select(error => error.Description));

                return (
                    false,
                    errorMessage,
                    string.Empty);
            }

            IdentityResult referenceClaimResult =
                await _userManager.AddClaimAsync(
                    user,
                    new Claim("ReferenceId", request.ReferenceId.ToString()));

            if (!referenceClaimResult.Succeeded)
            {
                string errorMessage = string.Join(
                    ", ",
                    referenceClaimResult.Errors.Select(error => error.Description));

                return (
                    false,
                    errorMessage,
                    string.Empty);
            }

            return (
                true,
                "User registered successfully.",
                user.Id);
        }

        public async Task<(bool Success, string Message, string UserId, int PatientId)> RegisterPatientAsync(
            RegisterPatientDto request,
            CancellationToken ct = default)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (
                    false,
                    "Password and confirm password do not match.",
                    string.Empty,
                    0);
            }

            IdentityUser? existingUserByEmail =
                await _userManager.FindByEmailAsync(request.Email);

            if (existingUserByEmail != null)
            {
                return (
                    false,
                    "Email already exists.",
                    string.Empty,
                    0);
            }

            IdentityUser? existingUserByName =
                await _userManager.FindByNameAsync(request.UserName);

            if (existingUserByName != null)
            {
                return (
                    false,
                    "Username already exists.",
                    string.Empty,
                    0);
            }

            Patient patient = new()
            {
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                CreatedDate = DateTime.Now
            };

            Patient createdPatient =
                await _patientRepository.CreateAsync(patient, ct);

            IdentityUser user = new()
            {
                UserName = request.UserName,
                Email = request.Email
            };

            IdentityResult createUserResult =
                await _userManager.CreateAsync(user, request.Password);

            if (!createUserResult.Succeeded)
            {
                string errorMessage = string.Join(
                    ", ",
                    createUserResult.Errors.Select(error => error.Description));

                return (
                    false,
                    errorMessage,
                    string.Empty,
                    createdPatient.PatientId);
            }

            IdentityResult roleClaimResult =
                await _userManager.AddClaimAsync(
                    user,
                    new Claim(ClaimTypes.Role, Role.Patient.ToString()));

            if (!roleClaimResult.Succeeded)
            {
                string errorMessage = string.Join(
                    ", ",
                    roleClaimResult.Errors.Select(error => error.Description));

                return (
                    false,
                    errorMessage,
                    string.Empty,
                    createdPatient.PatientId);
            }

            IdentityResult referenceClaimResult =
                await _userManager.AddClaimAsync(
                    user,
                    new Claim("ReferenceId", createdPatient.PatientId.ToString()));

            if (!referenceClaimResult.Succeeded)
            {
                string errorMessage = string.Join(
                    ", ",
                    referenceClaimResult.Errors.Select(error => error.Description));

                return (
                    false,
                    errorMessage,
                    string.Empty,
                    createdPatient.PatientId);
            }

            return (
                true,
                "Patient registered successfully.",
                user.Id,
                createdPatient.PatientId);
        }

        public async Task<(bool Success, string Message, string AccessToken, string RefreshToken, int ExpiresIn)> LoginAsync(
            LoginDto request,
            CancellationToken ct = default)
        {
            IdentityUser? user =
                await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return (
                    false,
                    "Invalid email or password.",
                    string.Empty,
                    string.Empty,
                    0);
            }

            bool isPasswordValid =
                await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return (
                    false,
                    "Invalid email or password.",
                    string.Empty,
                    string.Empty,
                    0);
            }

            string accessToken = await GenerateJwtTokenAsync(user);
            string refreshToken = GenerateRefreshToken();
            DateTime refreshTokenExpiryTime = GetRefreshTokenExpiryTime();

            await _userManager.SetAuthenticationTokenAsync(
                user,
                LoginProvider,
                RefreshTokenName,
                refreshToken);

            await _userManager.SetAuthenticationTokenAsync(
                user,
                LoginProvider,
                RefreshTokenExpiryName,
                refreshTokenExpiryTime.ToString("O"));

            return (
                true,
                "Login successful.",
                accessToken,
                refreshToken,
                GetAccessTokenExpirySeconds());
        }

        public async Task<(bool Success, string Message, string AccessToken, string RefreshToken, int ExpiresIn)> RefreshTokenAsync(
            RefreshTokenDto request,
            CancellationToken ct = default)
        {
            IdentityUser? user =
                await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return (
                    false,
                    "Invalid refresh token request.",
                    string.Empty,
                    string.Empty,
                    0);
            }

            string? storedRefreshToken =
                await _userManager.GetAuthenticationTokenAsync(
                    user,
                    LoginProvider,
                    RefreshTokenName);

            if (storedRefreshToken != request.RefreshToken)
            {
                return (
                    false,
                    "Invalid refresh token.",
                    string.Empty,
                    string.Empty,
                    0);
            }

            string? expiryValue =
                await _userManager.GetAuthenticationTokenAsync(
                    user,
                    LoginProvider,
                    RefreshTokenExpiryName);

            if (string.IsNullOrWhiteSpace(expiryValue) ||
                !DateTime.TryParse(expiryValue, out DateTime refreshTokenExpiryTime) ||
                refreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return (
                    false,
                    "Refresh token expired.",
                    string.Empty,
                    string.Empty,
                    0);
            }

            string newAccessToken = await GenerateJwtTokenAsync(user);
            string newRefreshToken = GenerateRefreshToken();
            DateTime newRefreshTokenExpiryTime = GetRefreshTokenExpiryTime();

            await _userManager.SetAuthenticationTokenAsync(
                user,
                LoginProvider,
                RefreshTokenName,
                newRefreshToken);

            await _userManager.SetAuthenticationTokenAsync(
                user,
                LoginProvider,
                RefreshTokenExpiryName,
                newRefreshTokenExpiryTime.ToString("O"));

            return (
                true,
                "Token refreshed successfully.",
                newAccessToken,
                newRefreshToken,
                GetAccessTokenExpirySeconds());
        }

        private async Task<string> GenerateJwtTokenAsync(IdentityUser user)
        {
            string issuer = _configuration["Jwt:Issuer"]!;
            string audience = _configuration["Jwt:Audience"]!;
            string key = _configuration["Jwt:Key"]!;

            int accessTokenMinutes = Convert.ToInt32(
                _configuration["Jwt:AccessTokenMinutes"]);

            IList<Claim> userClaims =
                await _userManager.GetClaimsAsync(user);

            string role = userClaims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.Role)
                ?.Value ?? string.Empty;

            List<Claim> claims = new()
            {
                new Claim("UserId", user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, role)
            };

            claims.AddRange(userClaims.Where(claim =>
                claim.Type != ClaimTypes.Role));

            SymmetricSecurityKey securityKey = new(
                Encoding.UTF8.GetBytes(key));

            SigningCredentials signingCredentials = new(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(accessTokenMinutes),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes);
        }

        private DateTime GetRefreshTokenExpiryTime()
        {
            int refreshTokenDays = Convert.ToInt32(
                _configuration["Jwt:RefreshTokenDays"]);

            return DateTime.UtcNow.AddDays(refreshTokenDays);
        }

        private int GetAccessTokenExpirySeconds()
        {
            int accessTokenMinutes = Convert.ToInt32(
                _configuration["Jwt:AccessTokenMinutes"]);

            return accessTokenMinutes * 60;
        }
    }
}