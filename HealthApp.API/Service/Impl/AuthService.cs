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
    IPatientRepository patientRepository) : IAuthService
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        if (await userManager.FindByEmailAsync(dto.Email) is not null)
        {
            throw new ConflictException("Email already registered.");
        }

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            EmailConfirmed = true,
            MustChangePassword = false
        };

        var result = await userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            throw new BusinessRuleException(
                string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        await userManager.AddToRoleAsync(user, Roles.Patient);

        await patientRepository.AddAsync(new Patient
            {
                UserId = user.Id,
                PatientName = dto.FullName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth.Value.Date,
                Gender = dto.Gender.Value.ToString(),
                PhoneNumber = dto.PhoneNumber,
                InsuranceId = dto.InsuranceId,
                CreatedDate = DateTime.Now
            });
        return await GenerateAuthResponseAsync(user);

    }




    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email)
            ?? throw new BusinessRuleException("Invalid email or password.");

        if (!await userManager.CheckPasswordAsync(user, dto.Password))
        {
            throw new BusinessRuleException("Invalid email or password.");
        }

        if (user.MustChangePassword)
        {
            throw new PasswordChangeRequiredException(
                "Password change required before login.");
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
            ?? throw new BusinessRuleException("Invalid email or password.");

        var isCurrentPasswordValid = await userManager.CheckPasswordAsync(
            user,
            dto.CurrentPassword);

        if (!isCurrentPasswordValid)
        {
            throw new BusinessRuleException("Invalid email or password.");
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

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email ?? "",
            Role = role,
            AccessToken = access,
            RefreshToken = refresh,
            AccessTokenExpiresAt = expires
        };
    }
}