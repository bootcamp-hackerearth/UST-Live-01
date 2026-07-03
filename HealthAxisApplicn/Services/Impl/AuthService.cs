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
    public class AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, AppDbContext dbContext) : IAuthService
    {
        public async Task<AuthResponse> LoginAsync(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new AuthResponse
                {
                    AccessToken = "",
                    RefreshToken = "",
                    ExpiresIn = 0,
                    Message = "Invalid credentials",
                    IsFirstLogin = false
                };
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return new AuthResponse
                {
                    AccessToken = "",
                    RefreshToken = "",
                    ExpiresIn = 0,
                    Message = "Invalid password",
                    IsFirstLogin = false
                };
            }

            var token = await GenerateJwtToken(user);

            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

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

            return new AuthResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresIn = expiry,
                Message = "Login successful",

                // ✅ CRITICAL
                IsFirstLogin = user.IsFirstLogin
            };
        }




        public async Task<AuthRegisterResponse> RegisterAsync(RegisterDto request)
        {
            // Password validation
            if (request.Password != request.ConfirmPassword)
            {
                return new AuthRegisterResponse
                {
                    Success = false,
                    Message = "Passwords do not match",
                    UserId = ""
                };
            }

            // Role validation
            if (request.Role != "Admin" && request.Role != "Patient" && request.Role != "Doctor")
            {
                return new AuthRegisterResponse
                {
                    Success = false,
                    Message = "Invalid role. Role should be either Admin, Doctor or Patient",
                    UserId = ""
                };
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(".", result.Errors.Select(e => e.Description));

                return new AuthRegisterResponse
                {
                    Success = false,
                    Message = errors,
                    UserId = ""
                };
            }

            await userManager.AddToRoleAsync(user, request.Role);

            // ✅ SET FIRST LOGIN FLAG
            if (request.Role == "Patient")
                user.IsFirstLogin = false;
            else
                user.IsFirstLogin = true;

            await userManager.UpdateAsync(user);

            // ✅ PATIENT CREATION (KEEP)
            if (request.Role == "Patient")
            {
                if (string.IsNullOrEmpty(request.Name) ||
                    request.DateOfBirth == null ||
                    string.IsNullOrEmpty(request.Gender) ||
                    string.IsNullOrEmpty(request.PhoneNo))
                {
                    return new AuthRegisterResponse
                    {
                        Success = false,
                        Message = "Missing patient details",
                        UserId = ""
                    };
                }

                var patient = new Patient
                {
                    PatientName = request.Name,
                    Email = request.Email,
                    PhoneNo = request.PhoneNo,
                    DateOfBirth = request.DateOfBirth.Value,
                    Gender = request.Gender,
                    InsuranceID = request.InsuranceID,
                    IsActive = true,
                    UserId = user.Id
                };

                await dbContext.Patients.AddAsync(patient);
                await dbContext.SaveChangesAsync();
            }

            // ❌ DOCTOR CREATION REMOVED (IMPORTANT FIX)

            // ✅ SUCCESS RESPONSE
            return new AuthRegisterResponse
            {
                Success = true,
                Message = "User registered successfully",
                UserId = user.Id
            };
        }


        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var jwtSetting = configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(user);

            var claim = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

            foreach (var role in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role, role));
            }

            // ✅ ✅ ADD THIS BLOCK
            var patient = await dbContext.Patients
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (patient != null)
            {
                claim.Add(new Claim("PatientId", patient.PatientId.ToString())); 
            }


            var doctor = await dbContext.Doctors
                    .FirstOrDefaultAsync(d => d.UserId == user.Id);

            if (doctor != null)
            {
                claim.Add(new Claim("DoctorId", doctor.DoctorId.ToString()));
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

