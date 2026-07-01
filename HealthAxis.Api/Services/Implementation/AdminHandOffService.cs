using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Extensions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class AdminHandoffService(
        AppDbContext context,
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IConfiguration configuration
    ) : IAdminHandoffService
    {
        public async Task<CreateAdminHandoffResponseDto> CreateAsync(
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            if (!user.IsAdmin())
            {
                throw new UnauthorizedException("Only admin can create handoff code");
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedException("UserId claim missing");

            var appUser = await userManager.FindByIdAsync(userId)
                ?? throw new NotFoundException("User not found");

            if (!appUser.IsActive)
            {
                throw new UnauthorizedException("User is inactive");
            }

            var roles = await userManager.GetRolesAsync(appUser);

            if (!roles.Contains("Admin"))
            {
                throw new UnauthorizedException("User is not admin");
            }

            var rawCode = GenerateSecureCode();

            var codeHash = HashCode(rawCode);

            var expiresAt = DateTime.UtcNow.AddSeconds(90);

            var handoffCode = new AdminHandoffCode
            {
                UserId = appUser.Id,
                CodeHash = codeHash,
                ExpiresAt = expiresAt,
                IsUsed = false
            };

            await context.AdminHandoffCodes.AddAsync(handoffCode, ct);

            await context.SaveChangesAsync(ct);

            return new CreateAdminHandoffResponseDto
            {
                Code = rawCode,
                ExpiresAt = expiresAt
            };
        }

        public async Task<AuthResponseDto> ExchangeAsync(
            ExchangeAdminHandoffRequestDto request,
            CancellationToken ct = default)
        {
            var codeHash = HashCode(request.Code);

            var handoffCode = await context.AdminHandoffCodes
                .FirstOrDefaultAsync(code =>
                    code.CodeHash == codeHash &&
                    !code.IsUsed,
                    ct)
                ?? throw new UnauthorizedException("Invalid or expired handoff code");

            if (handoffCode.ExpiresAt < DateTime.UtcNow)
            {
                throw new UnauthorizedException("Handoff code expired");
            }

            handoffCode.IsUsed = true;

            var user = await context.Users
                .Include(applicationUser => applicationUser.RefreshTokens)
                .FirstOrDefaultAsync(
                    applicationUser => applicationUser.Id == handoffCode.UserId,
                    ct)
                ?? throw new NotFoundException("User not found");

            if (!user.IsActive)
            {
                throw new UnauthorizedException("User is inactive");
            }

            var roles = await userManager.GetRolesAsync(user);

            var role = roles.FirstOrDefault()
                ?? throw new InvalidException("User has no role");

            if (role != "Admin")
            {
                throw new UnauthorizedException("Only admin handoff is allowed");
            }

            var refreshToken = CreateRefreshToken(user);

            user.RefreshTokens.Add(refreshToken);

            await context.SaveChangesAsync(ct);

            return new AuthResponseDto
            {
                UserId = user.Id,
                PatientId = user.PatientId,
                DoctorId = user.DoctorId,
                FullName = user.UserName ?? user.Email ?? "System Admin",
                Email = user.Email ?? string.Empty,
                Role = role,
                AccessToken = await jwtService.GenerateAccessTokenAsync(user),
                RefreshToken = refreshToken.Token,
                ExpiresIn =
                    Convert.ToInt32(
                        configuration["Jwt:AccessTokenExpirationMinutes"]
                    ) * 60
            };
        }

        private RefreshToken CreateRefreshToken(ApplicationUser user)
        {
            return new RefreshToken
            {
                Token = jwtService.GenerateRefreshToken(),
                ApplicationUserId = user.Id,
                ApplicationUser = user,
                ExpiresAt = DateTime.UtcNow.AddDays(
                    Convert.ToDouble(
                        configuration["Jwt:RefreshTokenExpiryDays"]
                    )
                ),
                IsRevoked = false
            };
        }

        private static string GenerateSecureCode()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);

            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        private static string HashCode(string rawCode)
        {
            var bytes = Encoding.UTF8.GetBytes(rawCode);

            var hashBytes = SHA256.HashData(bytes);

            return Convert.ToBase64String(hashBytes);
        }
    }
}