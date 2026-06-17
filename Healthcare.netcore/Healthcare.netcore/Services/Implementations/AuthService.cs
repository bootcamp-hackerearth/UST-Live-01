using HealthAxis.API.Models.Auth;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthAxis.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<(bool, string, string)> Register(RegisterDto request)
        {
            if (request.Password != request.ConfirmPassword)
                return (false, "Passwords do not match", "");

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return (false, "Registration failed", "");

            await _userManager.AddToRoleAsync(user, request.Role);

            return (true, "User Registered", user.Id);
        }

        public async Task<(bool, string, string, int)> Login(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return (false, "Invalid credentials", "", 0);

            var valid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!valid)
                return (false, "Invalid credentials", "", 0);

            var token = await GenerateToken(user);

            return (true, "Login successful", token, 60);
        }

        private async Task<string> GenerateToken(IdentityUser user)
        {
            var jwt = _config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var token = new JwtSecurityToken(
                jwt["Issuer"],
                jwt["Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
