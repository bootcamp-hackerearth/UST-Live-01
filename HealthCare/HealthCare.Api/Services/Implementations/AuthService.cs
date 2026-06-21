using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs.Authentication;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HealthCare.Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IConfiguration _configuration;
        private readonly HealthCareDbContext _context;

        public AuthService(
            UserManager<IdentityUser> userManager,
            IMapper mapper,
            IPatientRepository patientRepo,
            IDoctorRepository doctorRepo,
            IConfiguration configuration,
            HealthCareDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _patientRepo = patientRepo;
            _doctorRepo = doctorRepo;
            _configuration = configuration;
            _context = context;
        }

        //  COMMON USER CREATION
        private async Task<IdentityUser> CreateUserAsync(string email, string password, string role)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
                throw new Exception("Email already exists");

            var user = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, role);

            return user;
        }

        //  PATIENT REGISTRATION 
        public async Task RegisterPatientAsync(PatientRegisterDto dto)
        {


            if (string.IsNullOrEmpty(dto.Email))
            {
                throw new ArgumentException("Email is required");
            }


            if (dto.Password != dto.ConfirmPassword)
                throw new Exception("Passwords do not match");


            var user = await CreateUserAsync(dto.Email, dto.Password, "Patient");

            var patient = _mapper.Map<Patient>(dto);

            //  Link Identity UserId
            patient.UserId = user.Id;

            await _patientRepo.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        //  DOCTOR CREATION (ADMIN ONLY)
        public async Task RegisterDoctorAsync(DoctorRegisterDto dto)
        {

            if (dto.Password != dto.ConfirmPassword)
                throw new Exception("Passwords do not match");


            var user = await CreateUserAsync(dto.Email, dto.Password, "Doctor");

            var doctor = _mapper.Map<Doctor>(dto);

            doctor.UserId = user.Id;
            await _doctorRepo.AddAsync(doctor);
            await _context.SaveChangesAsync();

            await _doctorRepo.CreateSlots(doctor.DoctorId, dto.TimeSlots);
            await _context.SaveChangesAsync();
        }

        //  LOGIN
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                throw new Exception("Invalid email or password");

            var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!validPassword)
                throw new Exception("Invalid email or password");

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Any())
                throw new Exception("User has no role assigned");


            var role = roles.First();

            string token;

            if (role == "Patient")
            {
                var patient = await _patientRepo.GetByUserIdAsync(user.Id);

                if (patient == null)
                    throw new Exception("Patient record not found");

                token = GenerateJwtToken(user, role, patientId: patient.PatientId);
            }
            else if (role == "Doctor")
            {
                var doctor = await _doctorRepo.GetByUserIdAsync(user.Id);
                await _context.SaveChangesAsync();

                await _context.SaveChangesAsync();

                if (doctor == null)
                    throw new Exception("Doctor record not found");

                token = GenerateJwtToken(user, role, doctorId: doctor.DoctorId);
            }
            else if (role == "Admin")
            {
                // Generate JWT
                token = GenerateJwtToken(user, role);
            }
            else
            {
                throw new Exception("Doctor record not found");
            }

            return new AuthResponseDto
            {
                AccessToken = token,
                Role = role
            };
        }


        //Chnage Password
        public async Task ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new InvalidOperationException("User not found");

            var result = await _userManager.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword
            );

            if (!result.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
        }

        //  JWT TOKEN GENERATION
        private string GenerateJwtToken(
            IdentityUser user,
            string role,
            int? patientId = null,
            int? doctorId = null)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //  Add optional IDs
            if (patientId.HasValue)
                claims.Add(new Claim("PatientId", patientId.Value.ToString()));

            if (doctorId.HasValue)
                claims.Add(new Claim("DoctorId", doctorId.Value.ToString()));

            var expiryMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
