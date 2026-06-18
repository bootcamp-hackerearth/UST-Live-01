using HealthAxis.API.DTOs.Auth;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using HealthAxis.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

            IdentityUser? existingIdentityUser =
                await _userManager.FindByEmailAsync(request.Email);

            if (existingIdentityUser != null)
            {
                return (
                    false,
                    "Email already exists.",
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
                UserName = request.Email,
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
                    new Claim(ClaimTypes.Role, "Patient"));

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

        public async Task<(bool Success, string Message, string AccessToken, string RefreshToken, int ExpiresIn, string UserId, string Email, string Role, int ReferenceId)> LoginAsync(
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
                    0,
                    string.Empty,
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
                    0,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    0);
            }

            IList<Claim> userClaims =
                await _userManager.GetClaimsAsync(user);

            string role =
                userClaims.FirstOrDefault(claim =>
                    claim.Type == ClaimTypes.Role)?.Value ?? string.Empty;

            string referenceIdValue =
                userClaims.FirstOrDefault(claim =>
                    claim.Type == "ReferenceId")?.Value ?? "0";

            int referenceId =
                int.TryParse(referenceIdValue, out int parsedReferenceId)
                    ? parsedReferenceId
                    : 0;

            string accessToken =
                await GenerateJwtTokenAsync(user);

            string refreshToken =
                Guid.NewGuid().ToString();

            DateTime refreshTokenExpiryTime =
                DateTime.UtcNow.AddDays(GetRefreshTokenExpiryDays());

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
                GetAccessTokenExpirySeconds(),
                user.Id,
                user.Email ?? string.Empty,
                role,
                referenceId);
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

            string newAccessToken =
                await GenerateJwtTokenAsync(user);

            string newRefreshToken =
                Guid.NewGuid().ToString();

            DateTime newRefreshTokenExpiryTime =
                DateTime.UtcNow.AddDays(GetRefreshTokenExpiryDays());

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

        private async Task<string> GenerateJwtTokenAsync(
            IdentityUser user)
        {
            IConfigurationSection jwtSettings =
                _configuration.GetSection("Jwt");

            string keyValue =
                jwtSettings["Key"]
                ?? throw new InvalidOperationException("JWT Key is missing.");

            SymmetricSecurityKey securityKey =
                new(Encoding.UTF8.GetBytes(keyValue));

            SigningCredentials credentials =
                new(securityKey, SecurityAlgorithms.HmacSha256);

            IList<Claim> userClaims =
                await _userManager.GetClaimsAsync(user);

            List<Claim> claims = new()
            {
                new Claim("UserId", user.Id),

                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.Id),

                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email ?? string.Empty),

                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()),

                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id),

                new Claim(
                    ClaimTypes.Email,
                    user.Email ?? string.Empty)
            };

            claims.AddRange(userClaims);

            int accessTokenMinutes =
                GetAccessTokenExpiryMinutes();

            JwtSecurityToken token =
                new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(accessTokenMinutes),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        private int GetAccessTokenExpiryMinutes()
        {
            IConfigurationSection jwtSettings =
                _configuration.GetSection("Jwt");

            string? value =
                jwtSettings["AccessTokenExpirationMinutes"]
                ?? jwtSettings["AccessTokenMinutes"];

            return int.Parse(value ?? "30");
        }

        private int GetAccessTokenExpirySeconds()
        {
            return GetAccessTokenExpiryMinutes() * 60;
        }

        private int GetRefreshTokenExpiryDays()
        {
            IConfigurationSection jwtSettings =
                _configuration.GetSection("Jwt");

            string? value =
                jwtSettings["RefreshTokenDays"];

            return int.Parse(value ?? "7");
        }
    }
}
