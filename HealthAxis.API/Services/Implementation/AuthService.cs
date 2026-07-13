using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.AuthDtos;
using HealthAxis.Shared.DTO.DoctorDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HealthAxis.API.Services.Implementation
{
    public class AuthService(
        UserManager<IdentityUser> userManager,
        IConfiguration config,
        IRepository<Patient> patientRepository) : IAuthService
    {
        private const string LoginProvider = "HealthAxisAPI";
        private const string RefreshTokenName = "RefreshToken";
        private const string RefreshTokenExpiryName = "RefreshTokenExpiry";

        private const string InvalidCredentials = "Invalid credentials";
        private const string UserNotFound = "User not found";

        private static readonly string[] AdminResetAllowedRoles =
        [
            "Patient",
            "Doctor"
        ];

        public async Task<(bool Success, string Message, string AccessToken, string RefreshToken, int ExpiresIn)> Login(
            LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return (false, InvalidCredentials, string.Empty, string.Empty, 0);
            }

            var isPasswordValid =
                await userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return (false, InvalidCredentials, string.Empty, string.Empty, 0);
            }

            var accessToken = await GenerateToken(user);

            var refreshToken = GenerateRefreshToken();

            await SaveRefreshToken(user, refreshToken);

            var expiry =
                int.Parse(config.GetSection("Jwt")["AccessTokenExpirationMinutes"]!);

            return (
                true,
                "User logged in successfully",
                accessToken,
                refreshToken,
                expiry);
        }

        public async Task<(bool Success, string Message, string AccessToken, string RefreshToken, int ExpiresIn, int StatusCode)> RefreshToken(
            RefreshTokenDto request)
        {
            ClaimsPrincipal principal;

            try
            {
                principal = GetPrincipalFromExpiredToken(request.AccessToken);
            }
            catch
            {
                return (
                    false,
                    "Invalid access token",
                    string.Empty,
                    string.Empty,
                    0,
                    401);
            }

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return (
                    false,
                    "Invalid access token",
                    string.Empty,
                    string.Empty,
                    0,
                    401);
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return (
                    false,
                    UserNotFound,
                    string.Empty,
                    string.Empty,
                    0,
                    404);
            }

            var savedRefreshToken =
                await userManager.GetAuthenticationTokenAsync(
                    user,
                    LoginProvider,
                    RefreshTokenName);

            if (savedRefreshToken != request.RefreshToken)
            {
                return (
                    false,
                    "Invalid refresh token",
                    string.Empty,
                    string.Empty,
                    0,
                    401);
            }

            var savedExpiry =
                await userManager.GetAuthenticationTokenAsync(
                    user,
                    LoginProvider,
                    RefreshTokenExpiryName);

            if (string.IsNullOrWhiteSpace(savedExpiry))
            {
                return (
                    false,
                    "Refresh token expiry not found",
                    string.Empty,
                    string.Empty,
                    0,
                    401);
            }

            var isValidDate = DateTime.TryParse(
                savedExpiry,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind,
                out var refreshTokenExpiry);

            if (!isValidDate || refreshTokenExpiry < DateTime.UtcNow)
            {
                return (
                    false,
                    "Refresh token expired. Please login again",
                    string.Empty,
                    string.Empty,
                    0,
                    401);
            }

            var newAccessToken = await GenerateToken(user);

            var newRefreshToken = GenerateRefreshToken();

            await SaveRefreshToken(user, newRefreshToken);

            var expiry =
                int.Parse(config.GetSection("Jwt")["AccessTokenExpirationMinutes"]!);

            return (
                true,
                "Token refreshed successfully",
                newAccessToken,
                newRefreshToken,
                expiry,
                200);
        }

        public async Task<(bool Success, string Message, string UserId, int StatusCode)> Register(
            RegisterDto request)
        {
            var patients = await patientRepository.GetAllAsync();

            var emailExists = patients
                .Any(patient => patient.Email.Equals(
                    request.Email,
                    StringComparison.OrdinalIgnoreCase));

            if (emailExists)
            {
                return (false, "Email already registered", string.Empty, 400);
            }

            var phoneExists = patients
                .Any(patient => patient.PhoneNumber == request.PhoneNumber);

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

            var result = await userManager.CreateAsync(
                identityUser,
                request.Password);

            if (!result.Succeeded)
            {
                var errors = BuildIdentityErrorMessage(result);

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

        public async Task<(bool Success, string Message, int StatusCode)> ChangePassword(
            string userId,
            ChangePasswordDto request)
        {
            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return (false, "New password and confirm password do not match", 400);
            }

            if (request.CurrentPassword == request.NewPassword)
            {
                return (false, "New password cannot be same as current password", 400);
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return (false, UserNotFound, 404);
            }

            var result = await userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = BuildIdentityErrorMessage(result);

                return (false, errors, 400);
            }

            return (true, "Password changed successfully", 200);
        }

        public async Task<(bool Success, string Message, int StatusCode)> AdminResetPassword(
            AdminResetPasswordDto request)
        {
            var validationResult = ValidateAdminResetPasswordRequest(request);

            if (!validationResult.Success)
            {
                return validationResult;
            }

            var user = await FindUserForPasswordResetAsync(request);

            if (user is null)
            {
                return (false, UserNotFound, 404);
            }

            var roleValidationResult = await ValidateAdminResetTargetRoleAsync(user);

            if (!roleValidationResult.Success)
            {
                return roleValidationResult;
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetResult = await userManager.ResetPasswordAsync(
                user,
                resetToken,
                request.NewPassword);

            if (!resetResult.Succeeded)
            {
                var errors = BuildIdentityErrorMessage(resetResult);

                return (false, errors, 400);
            }

            await ClearRefreshTokenAsync(user);

            return (true, "Password reset successfully by admin.", 200);
        }

        private static (bool Success, string Message, int StatusCode) ValidateAdminResetPasswordRequest(
            AdminResetPasswordDto request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) &&
                string.IsNullOrWhiteSpace(request.Email))
            {
                return (false, "User id or email is required.", 400);
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return (false, "New password and confirm password do not match.", 400);
            }

            return (true, string.Empty, 200);
        }

        private async Task<IdentityUser?> FindUserForPasswordResetAsync(
            AdminResetPasswordDto request)
        {
            if (!string.IsNullOrWhiteSpace(request.UserId))
            {
                return await userManager.FindByIdAsync(request.UserId.Trim());
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                return await userManager.FindByEmailAsync(request.Email.Trim());
            }

            return null;
        }

        private async Task<(bool Success, string Message, int StatusCode)> ValidateAdminResetTargetRoleAsync(
            IdentityUser user)
        {
            var roles = await userManager.GetRolesAsync(user);

            var isAllowedTarget = roles.Any(role =>
                AdminResetAllowedRoles.Contains(
                    role,
                    StringComparer.OrdinalIgnoreCase));

            if (!isAllowedTarget)
            {
                return (
                    false,
                    "Admin can reset password only for patient or doctor users.",
                    403);
            }

            return (true, string.Empty, 200);
        }

        private async Task ClearRefreshTokenAsync(IdentityUser user)
        {
            await userManager.RemoveAuthenticationTokenAsync(
                user,
                LoginProvider,
                RefreshTokenName);

            await userManager.RemoveAuthenticationTokenAsync(
                user,
                LoginProvider,
                RefreshTokenExpiryName);
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

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        private async Task SaveRefreshToken(
            IdentityUser user,
            string refreshToken)
        {
            var refreshTokenExpirationDays =
                int.Parse(config.GetSection("Jwt")["RefreshTokenExpirationDays"]!);

            var refreshTokenExpiry =
                DateTime.UtcNow.AddDays(refreshTokenExpirationDays);

            await userManager.SetAuthenticationTokenAsync(
                user,
                LoginProvider,
                RefreshTokenName,
                refreshToken);

            await userManager.SetAuthenticationTokenAsync(
                user,
                LoginProvider,
                RefreshTokenExpiryName,
                refreshTokenExpiry.ToString("O"));
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var jwtSettings = config.GetSection("Jwt");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,

                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),

                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(
                accessToken,
                tokenValidationParameters,
                out var securityToken);

            if (securityToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private static string BuildIdentityErrorMessage(
            IdentityResult result)
        {
            return string.Join(
                ", ",
                result.Errors.Select(error => error.Description));
        }
    }
}