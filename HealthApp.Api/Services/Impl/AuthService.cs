using HealthApp.Api.Dtos;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthApp.Api.Services.Impl
{
    public class AuthService(UserManager<IdentityUser> userManager, IConfiguration config) : IAuthService
    {
        public async Task<(bool success, string message, string token, int expiresIn)> Login(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return (false, "Invalid credentials", string.Empty, 0);
            }
            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return (false, "Invalid credentials", string.Empty, 0);
            }
            var token = await GenerateToken(user);
            var expiry = int.Parse(config.GetSection("Jwt")["AccessTokenExpirationMinutes"]!);
            return (true, "User Logged in Successfully", token, expiry);
        }

        public async Task<(bool success, string message, string userId)> Register(RegisterDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return(false, "Password Do not Match", string.Empty);
            }
            if (request.Role != "Admin" && request.Role != "Doctor" && request.Role != "Patient")
            {
                return(false, "Invalid Role", string.Empty);
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
            var jwtSettings = config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var roles = await userManager.GetRolesAsync(user);


            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.Id))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            }

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            }

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expirationMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"]!);

            var token = new JwtSecurityToken(

                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials


                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

