using System.Text;
using HealthCare.Api.DTOs.Authentication;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HealthCare.Api.Services.Implementations
{
    public class AuthService(UserManager<IdentityUser>userManager,IConfiguration configuration) :IAuthService
    {
        public async Task<(bool Success, string message, string UserId)> Register(RegisterDto request)
        {
            if (request.Password != request.ConfirmePassword)
                return (false, "Password do not match",string.Empty);

            // var validRoles = new List<string> { "Admin", "Patient", "Doctor" };

            if (request.Role != "Patient" && request.Role != "Admin" && request.Role != "Doctor")

                return (false, "Invalid Role,must be Patient OR Doctor OR Admin",string.Empty);

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await userManager.CreateAsync(user,request.Password);

            if(!result.Succeeded)
            {
                var errors= string.Join(",",result.Errors.Select(e=>e.Description));
                return (false, errors,string.Empty);

            }
            await userManager.AddToRoleAsync(user,request.Role);

            return (true,"User Registered Sucessfully",user.Id);
        }

        public async Task<(bool Success, string message, string token, int expiresIn)> Login(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if(user is null)
                return (false,"Invail Email or Password",string.Empty,0);

            var passwordValid = await userManager.CheckPasswordAsync(user,request.Password);

            if(!passwordValid)
                return (false,"Invalid Email or Password",string.Empty,0) ;

            var token = await GenerateJwtToken(user);

            var expirationMinutes = int.Parse(configuration.GetSection("Jwt")["AccessTokenExpirationMinutes"]!);

            return (true, "Login Sucessfully", token, expirationMinutes);
        }

        private async Task <string>GenerateJwtToken(IdentityUser user)
        {
            var jwtSetting = configuration.GetSection("jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting["key"]!));
               
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

                //  Add roles to token
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var expirationMinutes = int.Parse(jwtSetting["AccessTokenExpirationMinutes"]!);

                var token = new JwtSecurityToken(
                    issuer: jwtSetting["Issuer"],
                    audience: jwtSetting["Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

        }
    }
