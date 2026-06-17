using HealthAxisApplicn.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthAxisApplicn.Services.Impl
{
    public class AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration) : IAuthService
    {
        public async Task<(bool success, string message, string token, int ExpiresIn)> LoginAsync(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return (false, "Invalid credentials", string.Empty, 0);
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
            
            if(!isPasswordValid)
            {
                return (false, "Invalid password", string.Empty, 0);
            }

            var token = await GenerateJwtToken(user);
            var expiry = int.Parse(configuration.GetSection("jwt")["AccessTokenExpirationMinutes"]);

            return (true, "Login successful", token, expiry);


        }

        public async Task<(bool Success, string Message, string UserId)> RegisterAsync(RegisterDto request)
        {
            //Password validation
            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Passwords do not match", string.Empty);
            }

            //Role Validation
            if(request.Role != "Admin" && request.Role!= "Patient" && request.Role!="Doctor")
            {
                return (false, "Invalid role. Role should be either Admin, Doctor or Patient", string.Empty);
            }

            //Creating new identity user
            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email

            };

            //Create user with hashed password
            var result = await userManager.CreateAsync(user, request.Password);

            //If user creation failed, return error message
            if(!result.Succeeded)
            {
                var errors = string.Join(".", result.Errors.Select(e => e.Description));
                return (false, errors, string.Empty);
            }

            await userManager.AddToRoleAsync(user, request.Role);
            return (true, "User registered successfully", user.Id);
        }

        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            //get Jwt section from appsettings.json
            var jwtSetting = configuration.GetSection("Jwt");

            //Creating signing key from secret
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            //Adding roles
            var roles = await userManager.GetRolesAsync(user);

            //Create Claim
            var claim = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach(var role in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role, role));

            }
            var expirationMinutes = int.Parse(jwtSetting["AccessTokenExpirationMinutes"]);

            var token = new JwtSecurityToken(

                issuer: jwtSetting["Issuer"],
                audience: jwtSetting["Audience"],
                claims: claim,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials

                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

