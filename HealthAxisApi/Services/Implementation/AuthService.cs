using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using HealthAxisCore_Api.DTOs.User;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Services.Interfaces;
using HealthAxisCore_Api.Exceptions;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HealthAxisCore_Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager,
                           IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // ✅ REGISTER
        public async Task<(bool Success, string Message, string AccessToken, int ExpiryInSeconds)>
            RegisterAsync(RegisterDTO request)
        {
            // ✅ Password validation
            if (request.Password != request.ConfirmPassword)
                throw new BusinessRuleException("Passwords do not match");

            // ✅ Role validation
            if (request.Role != "Admin" &&
                request.Role != "Doctor" &&
                request.Role != "Patient")
            {
                throw new BusinessRuleException("Invalid role");
            }

            // ✅ Check existing user
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                throw new BusinessRuleException("User already exists");

            // ✅ Create user
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Role = request.Role,
                ReferenceId = request.ReferenceId
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessRuleException(errors);
            }

            // ✅ Assign role
            await _userManager.AddToRoleAsync(user, request.Role);

            // ✅ Generate token
            var token = await GenerateJwtToken(user);

            int expiryMinutes = int.Parse(_configuration["Jwt:DurationInMinutes"]!);

            return (true, "User registered successfully", token, expiryMinutes * 60);
        }

        // ✅ LOGIN
        public async Task<(bool Success, string Message, string AccessToken, int ExpiryInSeconds)>
            LoginAsync(LoginDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new UnauthorizedException("Invalid email or password");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
                throw new UnauthorizedException("Invalid email or password");

            var token = await GenerateJwtToken(user);

            int expiryMinutes = int.Parse(_configuration["Jwt:DurationInMinutes"]!);

            return (true, "Login successful", token, expiryMinutes * 60);
        }

        // ✅ JWT TOKEN
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            int expiryMinutes = int.Parse(jwtSettings["DurationInMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
