using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HealthApp.API.Exceptions;
using HealthApp.API.Identity;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HealthApp.API.Service.Impl;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IConfiguration config,
    IRefreshTokenRepository refreshTokenRepository,
    IPatientRepository patientRepository,
    IDoctorRepository doctorRepository) : IAuthService
{
    private const string InvalidCredentialsMessage = "Invalid email or password.";
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        if (dto is null)
        {
            throw new BusinessRuleException("Registration details are required.");
        }

        if (string.IsNullOrWhiteSpace(dto.FullName))
        {
            throw new BusinessRuleException("Full name is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new BusinessRuleException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new BusinessRuleException("Password is required.");
        }

        if (!dto.DateOfBirth.HasValue)
        {
            throw new BusinessRuleException("Date of birth is required.");
        }

        if (!dto.Gender.HasValue)
        {
            throw new BusinessRuleException("Gender is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
        {
            throw new BusinessRuleException("Phone number is required.");
        }

        var fullName = dto.FullName.Trim();
        var email = dto.Email.Trim();
        var dateOfBirth = dto.DateOfBirth.Value.Date;
        var gender = dto.Gender.Value;
        var phoneNumber = dto.PhoneNumber.Trim();

        if (await userManager.FindByEmailAsync(email) is not null)
        {
            throw new ConflictException("Email already registered.");
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName,
            EmailConfirmed = true,
            MustChangePassword = false
        };

        var result = await userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            throw new BusinessRuleException(
                string.Join("; ", result.Errors.Select(error => error.Description)));
        }

        await userManager.AddToRoleAsync(user, Roles.Patient);

        await patientRepository.AddAsync(new Patient
        {
            UserId = user.Id,
            PatientName = fullName,
            Email = email,
            DateOfBirth = dateOfBirth,
            Gender = gender.ToString(),
            PhoneNumber = phoneNumber,
            InsuranceId = dto.InsuranceId,
            CreatedDate = DateTime.Now
        });

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email)
            ?? throw new BusinessRuleException(InvalidCredentialsMessage);

        if (!await userManager.CheckPasswordAsync(user, dto.Password))
        {
            throw new BusinessRuleException(InvalidCredentialsMessage);
        }

        var roles = await userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? Roles.Patient;

        if (user.MustChangePassword)
        {
            int? patientId = null;
            int? doctorId = null;

            if (role == Roles.Patient)
            {
                patientId = (await patientRepository.GetByUserIdAsync(user.Id))?.PatientId;
            }

            if (role == Roles.Doctor)
            {
                doctorId = (await doctorRepository.GetByUserIdAsync(user.Id))?.DoctorId;
            }

            return new AuthResponseDto
            {
                Message = "Password change required before login.",
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                Role = role,
                MustChangePassword = true,
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                AccessTokenExpiresAt = DateTime.MinValue,
                PatientId = patientId,
                DoctorId = doctorId
            };
        }

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(
        RefreshTokenRequestDto dto)
    {
        var stored = await refreshTokenRepository.GetByTokenAsync(dto.RefreshToken)
            ?? throw new BusinessRuleException("Invalid refresh token.");

        if (stored.IsUsed ||
            stored.IsRevoked ||
            stored.ExpiryDate <= DateTime.UtcNow)
        {
            throw new BusinessRuleException(
                "Refresh token expired or invalid.");
        }

        if (stored.User.MustChangePassword)
        {
            throw new PasswordChangeRequiredException(
                "Password change required before refreshing token.");
        }

        stored.IsUsed = true;

        await refreshTokenRepository.SaveChangesAsync();

        return await GenerateAuthResponseAsync(stored.User);
    }

    public async Task ChangePasswordAsync(ChangePasswordDto dto)
    {
        if (dto is null)
        {
            throw new BusinessRuleException("Password change details are required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new BusinessRuleException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.CurrentPassword))
        {
            throw new BusinessRuleException("Current password is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            throw new BusinessRuleException("New password is required.");
        }

        if (dto.NewPassword != dto.ConfirmNewPassword)
        {
            throw new BusinessRuleException(
                "New password and confirm password do not match.");
        }

        if (dto.CurrentPassword == dto.NewPassword)
        {
            throw new BusinessRuleException(
                "New password must be different from current password.");
        }

        var user = await userManager.FindByEmailAsync(dto.Email)
            ?? throw new BusinessRuleException(InvalidCredentialsMessage);

        var isCurrentPasswordValid = await userManager.CheckPasswordAsync(
            user,
            dto.CurrentPassword);

        if (!isCurrentPasswordValid)
        {
            throw new BusinessRuleException(InvalidCredentialsMessage);
        }

        var changePasswordResult = await userManager.ChangePasswordAsync(
            user,
            dto.CurrentPassword,
            dto.NewPassword);

        if (!changePasswordResult.Succeeded)
        {
            throw new BusinessRuleException(
                string.Join("; ", changePasswordResult.Errors.Select(e => e.Description)));
        }

        if (user.MustChangePassword)
        {
            user.MustChangePassword = false;

            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new BusinessRuleException(
                    string.Join("; ", updateResult.Errors.Select(e => e.Description)));
            }
        }
    }

    private async Task<AuthResponseDto> GenerateAuthResponseAsync(
        ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? Roles.Patient;

        var jwtId = Guid.NewGuid().ToString();

        var expires = DateTime.UtcNow.AddMinutes(
            int.Parse(config["Jwt:AccessTokenExpirationMinutes"] ?? "15"));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(JwtRegisteredClaimNames.Jti, jwtId),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? ""),
            new(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"]!));

        var token = new JwtSecurityToken(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256));

        var access = new JwtSecurityTokenHandler().WriteToken(token);

        var refresh = Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(64));

        await refreshTokenRepository.AddAsync(new RefreshToken
        {
            Token = refresh,
            JwtId = jwtId,
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddDays(
                int.Parse(config["Jwt:RefreshTokenExpirationDays"] ?? "14"))
        });

        int? patientId = null;
        int? doctorId = null;

        if (role == Roles.Patient)
        {
            patientId = (await patientRepository.GetByUserIdAsync(user.Id))?.PatientId;
        }

        if (role == Roles.Doctor)
        {
            doctorId = (await doctorRepository.GetByUserIdAsync(user.Id))?.DoctorId;
        }

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email ?? "",
            Role = role,
            AccessToken = access,
            RefreshToken = refresh,
            AccessTokenExpiresAt = expires,
            MustChangePassword = user.MustChangePassword,
            PatientId = patientId,
            DoctorId = doctorId
        };
    }
}