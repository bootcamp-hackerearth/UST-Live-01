using HealthCareApp.Models.Dtos;
using HealthCareApp.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthCareApp.Services.Impl
{
    public class AuthService(UserManager<IdentityUser> userManager, IConfiguration config) : IAuthService
    {

        public async Task<(bool Success, string Message, string UserId)> Register(RegisterDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Password Do not Match", string.Empty);
            }

            if (request.Role != "Patient" && request.Role != "Doctor" && request.Role != "Admin")
            {
                return (false, "Invalid Role", string.Empty);
            }

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                return (false, errors, string.Empty);
            }

            await userManager.AddToRoleAsync(user, request.Role);

            return (true, "User Registered Successfully", user.Id);
        }
        private async Task<string> GenerateToken(IdentityUser user)
        {
            var JwtSettings = config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email??String.Empty),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

            }
            var expirationMinutes = int.Parse(JwtSettings["AccessTokenExpirationMinutes"]);
            var token = new JwtSecurityToken(
                issuer: JwtSettings["Issuer"],
                audience: JwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<(bool Success, string Message, string Token, int ExpiresIn)> Login(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return (false, "Invalid Credentials", string.Empty, 0);
            }
            var isPasswordValid =await userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return (false, "Invalid Credentials", string.Empty, 0);
            }
            var token = await GenerateToken(user);
            var expiry=int.Parse(config.GetSection("Jwt")["AccessTokenExpirationMinutes"]);
            return (true, "Login Successful", token, expiry);
        }
    }
}
