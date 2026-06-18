using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        public async Task<AuthResponseDto> RegisterPatientAsync(
            RegisterPatientDto request,
            CancellationToken ct = default
        )
        {
            if (await userManager.FindByEmailAsync(request.Email) != null)
                throw new InvalidException("Email already exists");

            if (!await roleManager.RoleExistsAsync("Patient"))
                throw new InvalidException("Patient role does not exist");

            using var tx =
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
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(
                user,
                request.Password
            );

            if (!createResult.Succeeded)
            {
                throw new InvalidException(
                    string.Join(
                        ", ",
                        createResult.Errors.Select(e => e.Description)
                    )
                );
            }

            var roleResult = await userManager.AddToRoleAsync(
                user,
                "Patient"
            );

            if (!roleResult.Succeeded)
            {
                throw new InvalidException(
                    string.Join(
                        ", ",
                        roleResult.Errors.Select(e => e.Description)
                    )
                );
            }

            var refresh = CreateRefreshToken(user);

            await context.RefreshTokens.AddAsync(refresh, ct);
            await context.SaveChangesAsync(ct);

            await tx.CommitAsync(ct);

            return await CreateResponse(
                user,
                "Patient",
                refresh.Token
            );
        }

        public async Task<AuthResponseDto> LoginAsync(
            LoginDto request,
            CancellationToken ct = default
        )
        {
            var user = await context.Users
                .Include(u => u.Patient)
                .Include(u => u.Doctor)
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(
                    u => u.Email == request.Email,
                    ct
                )
                ?? throw new InvalidException(
                    "Invalid email or password"
                );

            if (!user.IsActive)
                throw new UnauthorizedException(
                    "User is inactive"
                );

            if (user.Patient != null && !user.Patient.IsActive)
                throw new UnauthorizedException(
                    "Patient is inactive"
                );

            if (user.Doctor != null && !user.Doctor.IsActive)
                throw new UnauthorizedException(
                    "Doctor is inactive"
                );

            var result = await signInManager.CheckPasswordSignInAsync(
                user,
                request.Password,
                false
            );

            if (!result.Succeeded)
                throw new InvalidException(
                    "Invalid email or password"
                );

            var role =
                (await userManager.GetRolesAsync(user))
                .FirstOrDefault()
                ?? throw new InvalidException(
                    "User has no role"
                );

            var refresh = CreateRefreshToken(user);

            user.RefreshTokens.Add(refresh);

            await context.SaveChangesAsync(ct);

            return await CreateResponse(
                user,
                role,
                refresh.Token
            );
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(
            RefreshTokenRequestDto request,
            CancellationToken ct = default
        )
        {
            var user = await context.Users
                .Include(u => u.Patient)
                .Include(u => u.Doctor)
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(
                    u => u.Id == request.UserId,
                    ct
                )
                ?? throw new NotFoundException(
                    "User not found"
                );

            var existing =
                user.RefreshTokens.FirstOrDefault(r =>
                    r.Token == request.RefreshToken &&
                    !r.IsRevoked &&
                    r.ExpiresAt > DateTime.UtcNow)
                ?? throw new UnauthorizedException(
                    "Invalid refresh token"
                );

            existing.IsRevoked = true;

            var refresh = CreateRefreshToken(user);

            user.RefreshTokens.Add(refresh);

            await context.SaveChangesAsync(ct);

            var role =
                (await userManager.GetRolesAsync(user))
                .FirstOrDefault()
                ?? throw new InvalidException(
                    "User has no role"
                );

            return await CreateResponse(
                user,
                role,
                refresh.Token
            );
        }

        private RefreshToken CreateRefreshToken(
            ApplicationUser user
        ) =>
            new RefreshToken
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

        private async Task<AuthResponseDto> CreateResponse(
            ApplicationUser user,
            string role,
            string refresh
        ) =>
            new AuthResponseDto
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
                        configuration["Jwt:AccessTokenExpirationMinutes"]
                    ) * 60
            };
    }
}