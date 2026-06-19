using HealthApp.Api.Dto;
using HealthApp.Api.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthApp.Api.Service.Impl
{
    public class AuthService(UserManager<IdentityUser> userManager,IConfiguration config) : IAuthService
    {
        public async Task<(bool success, string message, string token, int ExpiresIn)> Login(LoginDto register)
        {
            var user =await userManager.FindByEmailAsync(register.Email);
            if (user is null) 
            {
                return (false, "Invalid credentials",string.Empty, 0);
            }
            var isPasswordValid = await userManager.CheckPasswordAsync(user, register.Password);
            if (!isPasswordValid)
            {
                return (false, "Invalid credentials", string.Empty, 0);
            }

            var token = await GenerateJwtToken(user);

            var jwtSection = config.GetSection("Jwt");

            var expiryStr = jwtSection["AccessTokenExpirationMinutes"];

            if (string.IsNullOrEmpty(expiryStr))
                throw new Exception("AccessTokenExpirationMinutes missing in Jwt config");
            var expiry = int.Parse(expiryStr);

            return (true, "Login successful", token, expiry);


        }




        public async Task<(bool success, string message, string userId)> Register(RegisterDto register)
        {
            if (register.Password != register.ConfirmPassword) 
            {
                return (false, "Passwords do not match", string.Empty);
            }
            if (register.Role != "Admin" && register.Role != "User" && register.Role!= "Doctor")
            {
                return (false, "Invalid role", string.Empty);
            }

            var user = new IdentityUser 
            { 
                UserName = register.Email, 
                Email = register.Email 
            };

            var result = await userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                return (false, "Failed to create user", string.Empty);
            }

            await userManager.AddToRoleAsync(user, register.Role);
            return (true, "User created successfully", user.Id);

        }
        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var jwtSettings = config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var roles =await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.Id),

            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var expirationMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"]!);

            var token = new JwtSecurityToken
            (
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
