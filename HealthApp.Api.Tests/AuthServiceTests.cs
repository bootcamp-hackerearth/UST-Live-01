using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Impl;
using HealthApp.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OptionsHelper = Microsoft.Extensions.Options.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace HealthApp.Api.Tests.Services
{
    public class AuthServiceTests
    {
        private const string PatientRole = "Patient";
        private const string DoctorRole = "Doctor";
        private const string TestUserId = "user-1";
        private const string TestEmail = "test@test.com";
        private const string TestCredential = "Test@12345";
        private const string NewCredential = "New@12345";

        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<IPatientRepository> _patientRepo;
        private readonly Mock<IDoctorRepository> _doctorRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly IConfiguration _config;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _userManager = GetUserManagerMock();
            _patientRepo = new Mock<IPatientRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();
            _mapper = new Mock<IMapper>();

            var configValues = new Dictionary<string, string?>
            {
                { "Jwt:Key", "THIS_IS_A_TEST_SIGNING_KEY_WITH_MORE_THAN_32_CHARS_1234567890" },
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" },
                { "Jwt:AccessTokenExpirationMinutes", "60" }
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            _service = new AuthService(
                _userManager.Object,
                _config,
                _mapper.Object,
                _patientRepo.Object,
                _doctorRepo.Object);
        }

        private static Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var options = OptionsHelper.Create(new IdentityOptions());
            var passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
            var userValidators = new List<IUserValidator<ApplicationUser>>();
            var passwordValidators = new List<IPasswordValidator<ApplicationUser>>();
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errorDescriber = new IdentityErrorDescriber();
            var serviceProvider = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<ApplicationUser>>>();

            return new Mock<UserManager<ApplicationUser>>(
                store.Object,
                options,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                keyNormalizer.Object,
                errorDescriber,
                serviceProvider.Object,
                logger.Object);
        }

        private static RegisterPatientDto GetPatientDto()
        {
            return new RegisterPatientDto
            {
                Email = TestEmail,
                Password = TestCredential,
                ConfirmPassword = TestCredential
            };
        }

        private static DoctorCreateDto GetDoctorDto(string fullName = "John Doctor")
        {
            return new DoctorCreateDto
            {
                DoctorEmail = "doctor@test.com",
                FullName = fullName
            };
        }

        private static IdentityResult FailedIdentityResult(string description = "Identity operation failed.")
        {
            return IdentityResult.Failed(new IdentityError { Description = description });
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenPasswordsMismatch()
        {
            var dto = new RegisterPatientDto
            {
                Email = TestEmail,
                Password = TestCredential,
                ConfirmPassword = NewCredential
            };

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
            Assert.Equal("Passwords do not match.", result.message);
            Assert.Equal(string.Empty, result.userId);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenIdentityUserAlreadyExists()
        {
            var dto = GetPatientDto();

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(new ApplicationUser { Id = TestUserId });

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
            Assert.Equal("Email is already registered.", result.message);
            Assert.Equal(string.Empty, result.userId);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenPatientEmailAlreadyExists_IgnoringCase()
        {
            var dto = GetPatientDto();

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _patientRepo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>
                {
                    new Patient { Email = "other@test.com" },
                    new Patient { Email = TestEmail.ToUpperInvariant() }
                });

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
            Assert.Equal("Patient email is already registered.", result.message);
        }

        [Fact]
        public async Task RegisterPatient_ShouldContinue_WhenExistingPatientEmailIsNull()
        {
            var dto = GetPatientDto();
            var patient = new Patient { PatientId = 10, Email = dto.Email };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);
            _patientRepo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient> { new Patient { Email = null } });
            _mapper.Setup(x => x.Map<Patient>(dto)).Returns(patient);
            _patientRepo.Setup(x => x.Add(It.IsAny<Patient>())).ReturnsAsync(patient);
            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), PatientRole))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterPatient(dto);

            Assert.True(result.success);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenCreateUserFails()
        {
            var dto = GetPatientDto();
            var patient = new Patient { PatientId = 1 };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);
            _patientRepo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());
            _mapper.Setup(x => x.Map<Patient>(dto)).Returns(patient);
            _patientRepo.Setup(x => x.Add(It.IsAny<Patient>())).ReturnsAsync(patient);
            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(FailedIdentityResult("Create failed."));

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
            Assert.Equal("Create failed.", result.message);
            Assert.Equal(string.Empty, result.userId);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFailAndDeleteUser_WhenRoleAssignmentFails()
        {
            var dto = GetPatientDto();
            var patient = new Patient { PatientId = 1 };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);
            _patientRepo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());
            _mapper.Setup(x => x.Map<Patient>(dto)).Returns(patient);
            _patientRepo.Setup(x => x.Add(It.IsAny<Patient>())).ReturnsAsync(patient);
            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), PatientRole))
                .ReturnsAsync(FailedIdentityResult("Role failed."));
            _userManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
            Assert.Equal("Role failed.", result.message);
            _userManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public async Task RegisterPatient_ShouldSucceed_WhenRequestIsValid()
        {
            var dto = GetPatientDto();
            var patient = new Patient { PatientId = 1 };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);
            _patientRepo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());
            _mapper.Setup(x => x.Map<Patient>(dto)).Returns(patient);
            _patientRepo.Setup(x => x.Add(It.IsAny<Patient>())).ReturnsAsync(patient);
            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .Callback<ApplicationUser, string>((user, _) => user.Id = TestUserId)
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), PatientRole))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterPatient(dto);

            Assert.True(result.success);
            Assert.Equal("Patient registered successfully.", result.message);
            Assert.Equal(TestUserId, result.userId);
        }

        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenIdentityUserAlreadyExists()
        {
            var dto = GetDoctorDto();

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(new ApplicationUser());

            await Assert.ThrowsAsync<DuplicateEntityException>(() => _service.RegisterDoctor(dto));
        }

        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenDoctorEmailAlreadyExists()
        {
            var dto = GetDoctorDto();

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);
            _doctorRepo.Setup(x => x.ExistsByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<DuplicateEntityException>(() => _service.RegisterDoctor(dto));
        }

        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenDoctorNameIsEmpty()
        {
            var dto = GetDoctorDto(" ");
            var doctor = new Doctor { DoctorId = 5 };

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);
            _doctorRepo.Setup(x => x.ExistsByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(false);
            _mapper.Setup(x => x.Map<Doctor>(dto)).Returns(doctor);
            _doctorRepo.Setup(x => x.Add(It.IsAny<Doctor>())).ReturnsAsync(doctor);

            await Assert.ThrowsAsync<InvalidRequestException>(() => _service.RegisterDoctor(dto));
        }

        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenDoctorNameHasLessThanThreeCharacters()
        {
            var dto = GetDoctorDto("Jo");
            var doctor = new Doctor { DoctorId = 5 };

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);
            _doctorRepo.Setup(x => x.ExistsByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(false);
            _mapper.Setup(x => x.Map<Doctor>(dto)).Returns(doctor);
            _doctorRepo.Setup(x => x.Add(It.IsAny<Doctor>())).ReturnsAsync(doctor);

            await Assert.ThrowsAsync<InvalidRequestException>(() => _service.RegisterDoctor(dto));
        }

        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenCreateUserFails()
        {
            var dto = GetDoctorDto();
            var doctor = new Doctor { DoctorId = 5 };

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);
            _doctorRepo.Setup(x => x.ExistsByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(false);
            _mapper.Setup(x => x.Map<Doctor>(dto)).Returns(doctor);
            _doctorRepo.Setup(x => x.Add(It.IsAny<Doctor>())).ReturnsAsync(doctor);
            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(FailedIdentityResult("Create doctor failed."));

            var exception = await Assert.ThrowsAsync<BusinessRuleViolationException>(() => _service.RegisterDoctor(dto));

            Assert.Contains("Create doctor failed.", exception.Message);
        }

        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenRoleAssignmentFails()
        {
            var dto = GetDoctorDto();
            var doctor = new Doctor { DoctorId = 5 };

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);
            _doctorRepo.Setup(x => x.ExistsByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(false);
            _mapper.Setup(x => x.Map<Doctor>(dto)).Returns(doctor);
            _doctorRepo.Setup(x => x.Add(It.IsAny<Doctor>())).ReturnsAsync(doctor);
            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), DoctorRole))
                .ReturnsAsync(FailedIdentityResult("Doctor role failed."));

            var exception = await Assert.ThrowsAsync<BusinessRuleViolationException>(() => _service.RegisterDoctor(dto));

            Assert.Contains("Doctor role failed.", exception.Message);
        }

        [Fact]
        public async Task RegisterDoctor_ShouldSucceed_AndReturnTemporaryPassword()
        {
            var dto = GetDoctorDto("Alice Smith");
            var doctor = new Doctor { DoctorId = 11 };

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);
            _doctorRepo.Setup(x => x.ExistsByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(false);
            _mapper.Setup(x => x.Map<Doctor>(dto)).Returns(doctor);
            _doctorRepo.Setup(x => x.Add(It.IsAny<Doctor>())).ReturnsAsync(doctor);
            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Callback<ApplicationUser, string>((user, _) => user.Id = TestUserId)
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), DoctorRole))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterDoctor(dto);

            Assert.True(result.success);
            Assert.Equal("Doctor registered successfully.", result.message);
            Assert.Equal(TestUserId, result.userId);
            Assert.Equal($"Ali@{DateTime.Now.Year}", result.temporaryPassword);
        }

        [Fact]
        public async Task Login_ShouldFail_WhenUserNotFound()
        {
            _userManager.Setup(x => x.FindByEmailAsync(TestEmail))
                .ReturnsAsync((ApplicationUser?)null);

            var result = await _service.Login(new LoginDto { Email = TestEmail, Password = TestCredential });

            Assert.False(result.success);
            Assert.Equal("Invalid credentials.", result.message);
            Assert.Equal(string.Empty, result.token);
            Assert.Equal(0, result.expiresIn);
        }

        [Fact]
        public async Task Login_ShouldFail_WhenPasswordIsInvalid()
        {
            var user = new ApplicationUser { Id = TestUserId, Email = TestEmail };

            _userManager.Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);
            _userManager.Setup(x => x.CheckPasswordAsync(user, TestCredential))
                .ReturnsAsync(false);

            var result = await _service.Login(new LoginDto { Email = user.Email, Password = TestCredential });

            Assert.False(result.success);
            Assert.Equal("Invalid credentials.", result.message);
        }

        [Fact]
        public async Task Login_ShouldSucceed_AndGenerateToken_WithPatientDoctorEmailAndRoleClaims()
        {
            var user = new ApplicationUser
            {
                Id = TestUserId,
                Email = TestEmail,
                PatientId = 7,
                DoctorId = 8
            };

            _userManager.Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);
            _userManager.Setup(x => x.CheckPasswordAsync(user, TestCredential))
                .ReturnsAsync(true);
            _userManager.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { PatientRole, DoctorRole });

            var result = await _service.Login(new LoginDto { Email = user.Email, Password = TestCredential });
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(result.token);

            Assert.True(result.success);
            Assert.Equal("User logged in successfully.", result.message);
            Assert.Equal(60, result.expiresIn);
            Assert.False(string.IsNullOrWhiteSpace(result.token));
            Assert.Contains(jwt.Claims, claim => claim.Type == JwtRegisteredClaimNames.Sub && claim.Value == TestUserId);
            Assert.Contains(jwt.Claims, claim => claim.Type == JwtRegisteredClaimNames.Email && claim.Value == TestEmail);
            Assert.Contains(jwt.Claims, claim => claim.Type == "PatientId" && claim.Value == "7");
            Assert.Contains(jwt.Claims, claim => claim.Type == "DoctorId" && claim.Value == "8");
            Assert.Contains(jwt.Claims, claim => claim.Type == ClaimTypes.Role || claim.Type == "role");
        }

        [Fact]
        public async Task Login_ShouldSucceed_WhenUserEmailIsEmpty_WithoutEmailClaims()
        {
            var user = new ApplicationUser
            {
                Id = TestUserId,
                Email = string.Empty
            };

            _userManager.Setup(x => x.FindByEmailAsync(TestEmail))
                .ReturnsAsync(user);
            _userManager.Setup(x => x.CheckPasswordAsync(user, TestCredential))
                .ReturnsAsync(true);
            _userManager.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var result = await _service.Login(new LoginDto { Email = TestEmail, Password = TestCredential });
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(result.token);

            Assert.True(result.success);
            Assert.DoesNotContain(jwt.Claims, claim => claim.Type == JwtRegisteredClaimNames.Email);
            Assert.DoesNotContain(jwt.Claims, claim => claim.Type == ClaimTypes.Email);
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenUserIdIsEmpty()
        {
            await Assert.ThrowsAsync<UnauthorizedAccessAppException>(() =>
                _service.ChangePasswordAsync("", new ChangePasswordDto()));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.ChangePasswordAsync(TestUserId, null!));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenNewPasswordAndConfirmPasswordDoNotMatch()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = TestCredential,
                NewPassword = NewCredential,
                ConfirmNewPassword = "Different@123"
            };

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.ChangePasswordAsync(TestUserId, dto));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenNewPasswordIsSameAsCurrentPassword()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = TestCredential,
                NewPassword = TestCredential,
                ConfirmNewPassword = TestCredential
            };

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.ChangePasswordAsync(TestUserId, dto));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenUserNotFound()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = TestCredential,
                NewPassword = NewCredential,
                ConfirmNewPassword = NewCredential
            };

            _userManager.Setup(x => x.FindByIdAsync(TestUserId))
                .ReturnsAsync((ApplicationUser?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.ChangePasswordAsync(TestUserId, dto));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenIdentityChangePasswordFails()
        {
            var user = new ApplicationUser { Id = TestUserId };
            var dto = new ChangePasswordDto
            {
                CurrentPassword = TestCredential,
                NewPassword = NewCredential,
                ConfirmNewPassword = NewCredential
            };

            _userManager.Setup(x => x.FindByIdAsync(TestUserId))
                .ReturnsAsync(user);
            _userManager.Setup(x => x.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword))
                .ReturnsAsync(FailedIdentityResult("Change failed."));

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.ChangePasswordAsync(TestUserId, dto));

            Assert.Contains("Change failed.", exception.Message);
        }

        [Fact]
        public async Task ChangePassword_ShouldSucceed_WhenRequestIsValid()
        {
            var user = new ApplicationUser { Id = TestUserId };
            var dto = new ChangePasswordDto
            {
                CurrentPassword = TestCredential,
                NewPassword = NewCredential,
                ConfirmNewPassword = NewCredential
            };

            _userManager.Setup(x => x.FindByIdAsync(TestUserId))
                .ReturnsAsync(user);
            _userManager.Setup(x => x.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            await _service.ChangePasswordAsync(TestUserId, dto);

            _userManager.Verify(x => x.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword), Times.Once);
        }
    }
}
