using HealthAxisApplicn.Data;
using HealthAxisApplicn.Dto.Auth;
using HealthAxisApplicn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisApplicn.Services.Impl
{
    public class AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration, AppDbContext dbContext) : IAuthService
    {
        public async Task<(bool success, string message, string token, int ExpiresIn, string refreshToken)> LoginAsync(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return (false, "Invalid credentials", "", 0, "");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                return (false, "Invalid password", "", 0, "");

            var token = await GenerateJwtToken(user);

            // ✅ Generate refresh token
            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            // ✅ Save to DB
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(2),
                IsRevoked = false,
                UserId = user.Id
            };

            await dbContext.RefreshTokens.AddAsync(refreshTokenEntity);
            await dbContext.SaveChangesAsync();

            var expiry = int.Parse(configuration["Jwt:AccessTokenExpirationMinutes"]!);

            return (true, "Login successful", token, expiry, refreshToken);
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


        public async Task<AuthResponse?> RefreshAsync(string refreshToken)
        {
            // ✅ Find token in DB
            var storedToken = await dbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            // ✅ Validate token
            if (storedToken == null || storedToken.IsRevoked || storedToken.Expires < DateTime.UtcNow)
            {
                return null;
            }

            // ✅ Get user
            var user = await userManager.FindByIdAsync(storedToken.UserId);
            if (user == null)
                return null;

            // ✅ Generate NEW access token
            var newAccessToken = await GenerateJwtToken(user);

            var expiry = int.Parse(configuration["Jwt:AccessTokenExpirationMinutes"]!);

            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshToken, // reuse for now
                ExpiresIn = expiry,
                Message = "Token refreshed successfully"
            };
        }

    }
}

