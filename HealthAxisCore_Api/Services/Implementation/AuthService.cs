using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class AuthService(
        AppDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService,
        IConfiguration configuration
    ) : IAuthService
    {
        private const string PatientRole = "Patient";

        private const string DoctorRole = "Doctor";

        private const string UserNotFoundMessage = "User not found";

        public async Task<AuthResponseDto> RegisterPatientAsync(
            RegisterPatientDto request,
            CancellationToken ct = default)
        {
            if (await userManager.FindByEmailAsync(request.Email) != null)
            {
                throw new InvalidException("Email already exists");
            }

            var emailDomainName = GetEmailDomainName(request.Email);

            if (emailDomainName.Equals("healthaxis", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidException("Patients cannot register using HealthAxis email domain");
            }

            if (request.DateOfBirth.Date > DateTime.UtcNow.Date)
            {
                throw new InvalidException("Date of birth cannot be greater than today's date");
            }

            var minimumDateOfBirth = DateTime.UtcNow.Date.AddYears(-120);

            if (request.DateOfBirth.Date < minimumDateOfBirth)
            {
                throw new InvalidException("Please enter a valid date of birth");
            }

            if (!await roleManager.RoleExistsAsync(PatientRole))
            {
                throw new InvalidException("Patient role does not exist");
            }

            using var transaction =
                await context.Database.BeginTransactionAsync(ct);

            var patient = new Patient
            {
                PatientName = request.PatientName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                InsuranceID = request.InsuranceID,
                IsActive = true
            };

            await context.Patients.AddAsync(patient, ct);
            await context.SaveChangesAsync(ct);

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PatientId = patient.PatientId,
                IsActive = true,
                EmailConfirmed = true,
                FirstLogin = false
            };

            var createResult = await userManager.CreateAsync(
                user,
                request.Password);

            if (!createResult.Succeeded)
            {
                throw new InvalidException(
                    string.Join(
                        ", ",
                        createResult.Errors.Select(error => error.Description)));
            }

            var roleResult = await userManager.AddToRoleAsync(
                user,
                PatientRole);

            if (!roleResult.Succeeded)
            {
                throw new InvalidException(
                    string.Join(
                        ", ",
                        roleResult.Errors.Select(error => error.Description)));
            }

            var refresh = CreateRefreshToken(user);

            await context.RefreshTokens.AddAsync(refresh, ct);
            await context.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);

            return await CreateResponse(
                user,
                PatientRole,
                refresh.Token);
        }

        public async Task<AuthResponseDto> LoginAsync(
            LoginDto request,
            CancellationToken ct = default)
        {
            var user = await context.Users
                .Include(user => user.Patient)
                .Include(user => user.Doctor)
                .Include(user => user.RefreshTokens)
                .FirstOrDefaultAsync(
                    user => user.Email == request.Email,
                    ct)
                ?? throw new InvalidException("Invalid email or password");

            if (!user.IsActive)
            {
                throw new UnauthorizedException("User is inactive");
            }

            if (user.Patient != null && !user.Patient.IsActive)
            {
                throw new UnauthorizedException("Patient is inactive");
            }

            if (user.Doctor != null && !user.Doctor.IsActive)
            {
                throw new UnauthorizedException("Doctor is inactive");
            }

            var result = await signInManager.CheckPasswordSignInAsync(
                user,
                request.Password,
                false);

            if (!result.Succeeded)
            {
                throw new InvalidException("Invalid email or password");
            }

            var role =
                (await userManager.GetRolesAsync(user))
                .FirstOrDefault()
                ?? throw new InvalidException("User has no role");

            var refresh = CreateRefreshToken(user);

            user.RefreshTokens.Add(refresh);

            await context.SaveChangesAsync(ct);

            return await CreateResponse(
                user,
                role,
                refresh.Token);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(
            RefreshTokenRequestDto request,
            CancellationToken ct = default)
        {
            var user = await context.Users
                .Include(user => user.Patient)
                .Include(user => user.Doctor)
                .Include(user => user.RefreshTokens)
                .FirstOrDefaultAsync(
                    user => user.Id == request.UserId,
                    ct)
                ?? throw new NotFoundException(UserNotFoundMessage);

            var existing =
                user.RefreshTokens.FirstOrDefault(refreshToken =>
                    refreshToken.Token == request.RefreshToken &&
                    !refreshToken.IsRevoked &&
                    refreshToken.ExpiresAt > DateTime.UtcNow)
                ?? throw new UnauthorizedException("Invalid refresh token");

            existing.IsRevoked = true;

            var refresh = CreateRefreshToken(user);

            user.RefreshTokens.Add(refresh);

            await context.SaveChangesAsync(ct);

            var role =
                (await userManager.GetRolesAsync(user))
                .FirstOrDefault()
                ?? throw new InvalidException("User has no role");

            return await CreateResponse(
                user,
                role,
                refresh.Token);
        }

        private RefreshToken CreateRefreshToken(
            ApplicationUser user)
        {
            return new RefreshToken
            {
                Token = jwtService.GenerateRefreshToken(),
                ApplicationUserId = user.Id,
                ApplicationUser = user,
                ExpiresAt = DateTime.UtcNow.AddDays(
                    Convert.ToDouble(
                        configuration["Jwt:RefreshTokenExpiryDays"])),
                IsRevoked = false
            };
        }

        private async Task<AuthResponseDto> CreateResponse(
            ApplicationUser user,
            string role,
            string refresh)
        {
            return new AuthResponseDto
            {
                UserId = user.Id,
                PatientId = user.PatientId,
                DoctorId = user.DoctorId,
                FullName =
                    user.Patient?.PatientName
                    ?? user.Doctor?.DoctorName
                    ?? "System Admin",
                Email = user.Email ?? string.Empty,
                Role = role,
                AccessToken =
                    await jwtService.GenerateAccessTokenAsync(user),
                RefreshToken = refresh,
                ExpiresIn =
                    Convert.ToInt32(
                        configuration["Jwt:AccessTokenExpirationMinutes"]) * 60,
                FirstLogin = user.FirstLogin
            };
        }

        public async Task<ForgotPasswordResponseDto> ForgotPasswordAsync(
            ForgotPasswordDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email)
                ?? throw new NotFoundException(UserNotFoundMessage);

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            return new ForgotPasswordResponseDto
            {
                Message = "Password reset token generated successfully",
                ResetToken = token
            };
        }

        public async Task<string> ResetPasswordAsync(
            ResetPasswordDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email)
                ?? throw new NotFoundException(UserNotFoundMessage);

            var result = await userManager.ResetPasswordAsync(
                user,
                request.Token,
                request.NewPassword);

            if (!result.Succeeded)
            {
                throw new InvalidException(
                    string.Join(
                        ", ",
                        result.Errors.Select(error => error.Description)));
            }

            return "Password reset successfully";
        }

        public async Task<string> ChangeFirstLoginPasswordAsync(
            ChangeFirstLoginPasswordDto request,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                throw new InvalidException("New password and confirm password do not match");
            }

            var userId = user
                .FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User ID claim missing");
            }

            var applicationUser = await context.Users
                .FirstOrDefaultAsync(
                    applicationUser => applicationUser.Id == userId,
                    ct)
                ?? throw new NotFoundException(UserNotFoundMessage);

            if (!applicationUser.IsActive)
            {
                throw new UnauthorizedException("User is inactive");
            }

            var roles = await userManager.GetRolesAsync(applicationUser);

            if (!roles.Contains(DoctorRole))
            {
                throw new UnauthorizedException("Only doctors can change first login password");
            }

            if (!applicationUser.FirstLogin)
            {
                throw new InvalidException("Password has already been changed");
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(
                applicationUser,
                request.CurrentPassword,
                request.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                throw new InvalidException(
                    string.Join(
                        ", ",
                        changePasswordResult.Errors.Select(error => error.Description)));
            }

            applicationUser.FirstLogin = false;

            await context.SaveChangesAsync(ct);

            return "Password changed successfully";
        }

        private static string GetEmailDomainName(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            {
                return string.Empty;
            }

            var domainPart = email.Split('@')[1];

            if (string.IsNullOrWhiteSpace(domainPart) || !domainPart.Contains('.'))
            {
                return string.Empty;
            }

            return domainPart.Split('.')[0];
        }

        public async Task<string> ChangePasswordAsync(
            ChangePasswordDto request,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                throw new InvalidException("New password and confirm password do not match");
            }

            var userId = user
                .FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User ID claim missing");
            }

            var applicationUser = await context.Users
                .FirstOrDefaultAsync(
                    applicationUser => applicationUser.Id == userId,
                    ct)
                ?? throw new NotFoundException(UserNotFoundMessage);

            if (!applicationUser.IsActive)
            {
                throw new UnauthorizedException("User is inactive");
            }

            var roles = await userManager.GetRolesAsync(applicationUser);

            if (!roles.Contains(PatientRole) && !roles.Contains(DoctorRole))
            {
                throw new UnauthorizedException("Only patients and doctors can change password here");
            }

            var result = await userManager.ChangePasswordAsync(
                applicationUser,
                request.CurrentPassword,
                request.NewPassword);

            if (!result.Succeeded)
            {
                throw new InvalidException(
                    string.Join(
                        ", ",
                        result.Errors.Select(error => error.Description)));
            }

            return "Password changed successfully";
        }
    }
}