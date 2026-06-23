using HealthApp.Api.Dto;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthApp.Api.Service.Impl
{
    public class AuthService(UserManager<IdentityUser> userManager, IConfiguration config,
        IPatientRepository patientRepository, IDoctorRepository doctorRepository) : IAuthService
    {
        public async Task<(bool success, string message, string token, int ExpiresIn)> Login(LoginDto register)
        {
            var user = await userManager.FindByEmailAsync(register.Email);

            if (user is null)
            {
                return (false, "Invalid credentials", string.Empty, 0);
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

            if (!string.Equals(register.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return (false, "Only Admin role can be created using this endpoint", string.Empty);
            }

            var existingUser = await userManager.FindByEmailAsync(register.Email);
            if (existingUser != null)
            {
                return (false, "Email already exists", string.Empty);
            }

            var user = new IdentityUser
            {
                UserName = register.Email,
                Email = register.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errors, string.Empty);
            }

            var roleResult = await userManager.AddToRoleAsync(user, "Admin");

            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return (false, errors, string.Empty);
            }

            return (true, "Admin created successfully", user.Id);
        }

        public async Task<(bool success, string message, string userId)> RegisterPatientAsync(PatientRegisterDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return (false, "Passwords do not match", string.Empty);
            }

            if (request.DateOfBirth == null)
            {
                return (false, "Date of birth is required", string.Empty);
            }

            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return (false, "Email already exists", string.Empty);
            }

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errors, string.Empty);
            }

            var roleResult = await userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return (false, errors, string.Empty);
            }

            try
            {
                var patient = new Patient
                {
                    FullName = request.FullName,
                    DateOfBirth = request.DateOfBirth.Value.Date,
                    Gender = request.Gender,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    InsuranceId = request.InsuranceId,
                    CreatedDate = DateTime.Today,
                    IdentityUserId = user.Id
                };

                await patientRepository.addAsync(patient);
            }
            catch
            {
                await userManager.DeleteAsync(user);
                return (false, "Patient creation failed", string.Empty);
            }

            return (true, "Patient account created successfully", user.Id);
        }

        public async Task<(bool success, string message, string userId, string temporaryPassword)>
            RegisterDoctorByAdminAsync(DoctorRegisterDto request)
        {
            const string temporaryPassword = "Doctor@01";

            if (request.PracticeStartDate == null)
            {
                return (false, "Practice start date is required", string.Empty, string.Empty);
            }

            var existingIdentityUser = await userManager.FindByEmailAsync(request.Email!);
            if (existingIdentityUser != null)
            {
                return (false, "Email already exists", string.Empty, string.Empty);
            }

            var existingDoctor = await doctorRepository.getallAsync();
            if (existingDoctor != null &&
                existingDoctor.Any(d => !string.IsNullOrWhiteSpace(d.Email) &&
                                        d.Email.ToLower() == request.Email!.ToLower()))
            {
                return (false, "A doctor with this email already exists", string.Empty, string.Empty);
            }

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var createResult = await userManager.CreateAsync(user, temporaryPassword);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return (false, errors, string.Empty, string.Empty);
            }

            var roleResult = await userManager.AddToRoleAsync(user, "Doctor");

            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return (false, errors, string.Empty, string.Empty);
            }

            try
            {
                var doctor = new Doctor
                {
                    FullName = request.FullName!,
                    Specialisation = request.Specialisation!,
                    Email = request.Email!,
                    PracticeStartDate = request.PracticeStartDate.Value.Date,
                    ConsultationFee = request.ConsultationFee ?? 0,
                    DoctorPhoneNumber = request.DoctorPhoneNumber,
                    IsActive = request.IsActive ?? true,
                    IdentityUserId = user.Id
                };

                await doctorRepository.addAsync(doctor);
            }
            catch
            {
                await userManager.DeleteAsync(user);
                return (false, "Doctor creation failed", string.Empty, string.Empty);
            }

            return (true, "Doctor account created successfully", user.Id, temporaryPassword);
        }

        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var jwtSettings = config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

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
