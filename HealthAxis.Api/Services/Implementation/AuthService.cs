using HealthAxisCore_Api.Models.DTOs;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration) : IAuthService
    {
        public async Task<(bool Success, string Error, string Token, int ExpiresIn)> Login(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return (false, "Wrong Credentials", string.Empty, 0);
            }

            var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordValid)
            {
                return (false, "Wrong Credentials", string.Empty, 0);
            }

            var token = await GenerateToken(user);
            var expiresIn = int.Parse(configuration.GetSection("Jwt")["AccessTokenExpirationMinutes"]!);
            return (true, "User Logged in Successfuly", token, expiresIn);
        }

        public async Task<(bool Success, string Error, string UserId)> Register(RegisterDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Password Do Not Match", string.Empty);
            }

            if (request.Role != "Admin" && request.Role != "Employee")
            {
                return (false, "Invalid Role", string.Empty);
            }

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                return (false, errors, string.Empty);
            }

            await userManager.AddToRoleAsync(user, request.Role);
            return (true, "User Register Successfully", user.Id);


        }
        public async Task<(bool Success, string Message)> DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return (false, "User not found");

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            return (true, "User deleted successfully");
        }

        private async Task<string> GenerateToken(IdentityUser user)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var role = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };

            foreach (var r in role)
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            }

            var expirationMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
               signingCredentials: cred
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
