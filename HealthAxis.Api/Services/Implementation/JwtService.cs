using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class JwtService(
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager
    ) : IJwtService
    {
        public async Task<string> GenerateAccessTokenAsync(
            ApplicationUser user
        )
        {
            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new("UserId", user.Id),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(
                    ClaimTypes.Email,
                    user.Email ?? string.Empty
                )
            };

            foreach (var role in roles)
            {
                claims.Add(
                    new Claim(ClaimTypes.Role, role)
                );

                claims.Add(
                    new Claim("Role", role)
                );
            }

            if (user.PatientId.HasValue)
            {
                claims.Add(
                    new Claim(
                        "PatientId",
                        user.PatientId.Value.ToString()
                    )
                );
            }

            if (user.DoctorId.HasValue)
            {
                claims.Add(
                    new Claim(
                        "DoctorId",
                        user.DoctorId.Value.ToString()
                    )
                );
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    configuration["Jwt:Key"]!
                )
            );

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(
                        configuration[
                            "Jwt:AccessTokenExpirationMinutes"
                        ]
                    )
                ),
                signingCredentials: new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256
                )
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}