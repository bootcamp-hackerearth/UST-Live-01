using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Impl;
using HealthApp.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using OptionsHelper = Microsoft.Extensions.Options.Options;

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
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _userManager = GetUserManagerMock();
            _patientRepo = new Mock<IPatientRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();
            _mapper = new Mock<IMapper>();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] =
                        "THIS_IS_A_TEST_SIGNING_KEY_WITH_MORE_THAN_32_CHARS_1234567890",
                    ["Jwt:Issuer"] = "TestIssuer",
                    ["Jwt:Audience"] = "TestAudience",
                    ["Jwt:AccessTokenExpirationMinutes"] = "60"
                })
                .Build();

            _service = new AuthService(
                _userManager.Object,
                configuration,
                _mapper.Object,
                _patientRepo.Object,
                _doctorRepo.Object);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenPasswordsMismatch()
        {
            var dto = GetPatientDto();
            dto.ConfirmPassword = NewCredential;

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
            Assert.Equal("Passwords do not match.", result.message);
            Assert.Equal(string.Empty, result.userId);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenIdentityUserAlreadyExists()
        {
            var dto = GetPatientDto();
            _userManager
                .Setup(manager => manager.FindByEmailAsync(dto.Email))
                .ReturnsAsync(new ApplicationUser { Id = TestUserId });

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
            Assert.Equal("Email is already registered.", result.message);
            Assert.Equal(string.Empty, result.userId);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenPatientEmailAlreadyExistsIgnoringCase()
        {
            var dto = GetPatientDto();
            _userManager
                .Setup(manager => manager.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);
            _patientRepo
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Patient>
                {
                    new() { Email = "other@test.com" },
                    new() { Email = TestEmail.ToUpperInvariant() }
                });

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
            Assert.Equal(
                "Patient email is already registered.",
                result.message);
        }

        [Fact]
        public async Task RegisterPatient_ShouldSucceed_WhenExistingPatientEmailIsNull()
        {
            var dto = GetPatientDto();
            var patient = new Patient
            {
                PatientId = 10,
                Email = dto.Email
            };

            SetupSuccessfulPatientRegistration(
                dto,
                patient,
                [new Patient { Email = null }]);

            var result = await _service.RegisterPatient(dto);

            Assert.True(result.success);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenCreateUserFails()
        {
            var dto = GetPatientDto();
            var patient = new Patient { PatientId = 1 };

            SetupPatientRegistrationBeforeIdentityCreation(dto, patient);
            _userManager
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    dto.Password))
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

            SetupPatientRegistrationBeforeIdentityCreation(dto, patient);
            _userManager
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    dto.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManager
                .Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    PatientRole))
                .ReturnsAsync(FailedIdentityResult("Role failed."));
            _userManager
                .Setup(manager => manager.DeleteAsync(
                    It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
            Assert.Equal("Role failed.", result.message);
            _userManager.Verify(
                manager => manager.DeleteAsync(
                    It.IsAny<ApplicationUser>()),
                Times.Once);
        }

        [Fact]
        public async Task RegisterPatient_ShouldSucceed_WhenRequestIsValid()
        {
            var dto = GetPatientDto();
            var patient = new Patient { PatientId = 1 };

            SetupPatientRegistrationBeforeIdentityCreation(dto, patient);
            _userManager
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    dto.Password))
                .Callback<ApplicationUser, string>(
                    (user, _) => user.Id = TestUserId)
                .ReturnsAsync(IdentityResult.Success);
            _userManager
                .Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    PatientRole))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterPatient(dto);

            Assert.True(result.success);
            Assert.Equal(
                "Patient registered successfully.",
                result.message);
            Assert.Equal(TestUserId, result.userId);
        }

        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenIdentityUserAlreadyExists()
        {
            var dto = GetDoctorDto();
            _userManager
                .Setup(manager => manager.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(new ApplicationUser());

            await Assert.ThrowsAsync<DuplicateEntityException>(
                () => _service.RegisterDoctor(dto));
        }

        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenDoctorEmailAlreadyExists()
        {
            var dto = GetDoctorDto();
            _userManager
                .Setup(manager => manager.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);
            _doctorRepo
                .Setup(repository => repository.ExistsByEmailAsync(
                    dto.DoctorEmail))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<DuplicateEntityException>(
                () => _service.RegisterDoctor(dto));
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("Jo")]
        public async Task RegisterDoctor_ShouldThrow_WhenDoctorNameIsInvalid(
            string fullName)
        {
            var dto = GetDoctorDto(fullName);
            SetupDoctorBeforeNameValidation(dto);

            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.RegisterDoctor(dto));
        }

        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenCreateUserFails()
        {
            var dto = GetDoctorDto();
            SetupDoctorBeforeIdentityCreation(dto);
            _userManager
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(FailedIdentityResult(
                    "Create doctor failed."));

            var exception = await Assert.ThrowsAsync<
                BusinessRuleViolationException>(
                () => _service.RegisterDoctor(dto));

            Assert.Contains("Create doctor failed.", exception.Message);
        }

        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenRoleAssignmentFails()
        {
            var dto = GetDoctorDto();
            SetupDoctorBeforeIdentityCreation(dto);
            _userManager
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManager
                .Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    DoctorRole))
                .ReturnsAsync(FailedIdentityResult(
                    "Doctor role failed."));

            var exception = await Assert.ThrowsAsync<
                BusinessRuleViolationException>(
                () => _service.RegisterDoctor(dto));

            Assert.Contains("Doctor role failed.", exception.Message);
        }

        [Fact]
        public async Task RegisterDoctor_ShouldSucceed_AndReturnTemporaryPassword()
        {
            var dto = GetDoctorDto("Alice Smith");
            SetupDoctorBeforeIdentityCreation(
                dto,
                new Doctor { DoctorId = 11 });
            _userManager
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .Callback<ApplicationUser, string>(
                    (user, _) => user.Id = TestUserId)
                .ReturnsAsync(IdentityResult.Success);
            _userManager
                .Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    DoctorRole))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterDoctor(dto);

            Assert.True(result.success);
            Assert.Equal(
                "Doctor registered successfully.",
                result.message);
            Assert.Equal(TestUserId, result.userId);
            Assert.Equal(
                $"Ali@{DateTime.Now.Year}",
                result.temporaryPassword);
        }

        [Fact]
        public async Task Login_ShouldFail_WhenUserNotFound()
        {
            _userManager
                .Setup(manager => manager.FindByEmailAsync(TestEmail))
                .ReturnsAsync((ApplicationUser?)null);

            var result = await _service.Login(CreateLoginDto());

            Assert.False(result.success);
            Assert.Equal("Invalid credentials.", result.message);
            Assert.Equal(string.Empty, result.token);
            Assert.Equal(0, result.expiresIn);
        }

        [Fact]
        public async Task Login_ShouldFail_WhenPasswordIsInvalid()
        {
            var user = new ApplicationUser
            {
                Id = TestUserId,
                Email = TestEmail
            };
            _userManager
                .Setup(manager => manager.FindByEmailAsync(TestEmail))
                .ReturnsAsync(user);
            _userManager
                .Setup(manager => manager.CheckPasswordAsync(
                    user,
                    TestCredential))
                .ReturnsAsync(false);

            var result = await _service.Login(CreateLoginDto());

            Assert.False(result.success);
            Assert.Equal("Invalid credentials.", result.message);
        }

        [Fact]
        public async Task Login_ShouldSucceed_AndGenerateExpectedClaims()
        {
            var user = new ApplicationUser
            {
                Id = TestUserId,
                Email = TestEmail,
                PatientId = 7,
                DoctorId = 8
            };
            SetupSuccessfulLogin(
                user,
                [PatientRole, DoctorRole]);

            var result = await _service.Login(CreateLoginDto());
            var jwt = new JwtSecurityTokenHandler()
                .ReadJwtToken(result.token);

            Assert.True(result.success);
            Assert.Equal(
                "User logged in successfully.",
                result.message);
            Assert.Equal(60, result.expiresIn);
            Assert.False(string.IsNullOrWhiteSpace(result.token));
            Assert.Equal("TestIssuer", jwt.Issuer);
            Assert.Contains("TestAudience", jwt.Audiences);
            Assert.Contains(jwt.Claims, claim =>
                claim.Type == JwtRegisteredClaimNames.Sub &&
                claim.Value == TestUserId);
            Assert.Contains(jwt.Claims, claim =>
                claim.Type == JwtRegisteredClaimNames.Email &&
                claim.Value == TestEmail);
            Assert.Contains(jwt.Claims, claim =>
                claim.Type == "PatientId" && claim.Value == "7");
            Assert.Contains(jwt.Claims, claim =>
                claim.Type == "DoctorId" && claim.Value == "8");
            Assert.True(jwt.Claims.Count(claim =>
                claim.Type == ClaimTypes.Role ||
                claim.Type == "role") >= 2);
        }

        [Fact]
        public async Task Login_ShouldSucceed_WhenUserEmailIsEmpty_WithoutEmailClaim()
        {
            var user = new ApplicationUser
            {
                Id = TestUserId,
                Email = string.Empty
            };
            SetupSuccessfulLogin(user, []);

            var result = await _service.Login(CreateLoginDto());
            var jwt = new JwtSecurityTokenHandler()
                .ReadJwtToken(result.token);

            Assert.True(result.success);
            Assert.DoesNotContain(jwt.Claims, claim =>
                claim.Type == JwtRegisteredClaimNames.Email);
            Assert.DoesNotContain(jwt.Claims, claim =>
                claim.Type == ClaimTypes.Email);
        }

        [Fact]
        public async Task Login_ShouldSucceed_WithoutPatientOrDoctorClaims()
        {
            var user = new ApplicationUser
            {
                Id = TestUserId,
                Email = TestEmail,
                PatientId = null,
                DoctorId = null
            };
            SetupSuccessfulLogin(user, [PatientRole]);

            var result = await _service.Login(CreateLoginDto());
            var jwt = new JwtSecurityTokenHandler()
                .ReadJwtToken(result.token);

            Assert.DoesNotContain(jwt.Claims, claim =>
                claim.Type == "PatientId");
            Assert.DoesNotContain(jwt.Claims, claim =>
                claim.Type == "DoctorId");
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenUserIdIsEmpty()
        {
            await Assert.ThrowsAsync<UnauthorizedAccessAppException>(
                () => _service.ChangePasswordAsync(
                    string.Empty,
                    CreateChangePasswordDto()));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenRequestIsNull()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.ChangePasswordAsync(
                    TestUserId,
                    null!));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenPasswordsDoNotMatch()
        {
            var dto = CreateChangePasswordDto();
            dto.ConfirmNewPassword = "Different@123";

            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.ChangePasswordAsync(TestUserId, dto));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenNewPasswordEqualsCurrentPassword()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = TestCredential,
                NewPassword = TestCredential,
                ConfirmNewPassword = TestCredential
            };

            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.ChangePasswordAsync(TestUserId, dto));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenUserNotFound()
        {
            var dto = CreateChangePasswordDto();
            _userManager
                .Setup(manager => manager.FindByIdAsync(TestUserId))
                .ReturnsAsync((ApplicationUser?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.ChangePasswordAsync(TestUserId, dto));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenIdentityOperationFails()
        {
            var user = new ApplicationUser { Id = TestUserId };
            var dto = CreateChangePasswordDto();
            _userManager
                .Setup(manager => manager.FindByIdAsync(TestUserId))
                .ReturnsAsync(user);
            _userManager
                .Setup(manager => manager.ChangePasswordAsync(
                    user,
                    dto.CurrentPassword,
                    dto.NewPassword))
                .ReturnsAsync(FailedIdentityResult("Change failed."));

            var exception = await Assert.ThrowsAsync<
                InvalidRequestException>(
                () => _service.ChangePasswordAsync(TestUserId, dto));

            Assert.Contains("Change failed.", exception.Message);
        }

        [Fact]
        public async Task ChangePassword_ShouldSucceed_WhenRequestIsValid()
        {
            var user = new ApplicationUser { Id = TestUserId };
            var dto = CreateChangePasswordDto();
            _userManager
                .Setup(manager => manager.FindByIdAsync(TestUserId))
                .ReturnsAsync(user);
            _userManager
                .Setup(manager => manager.ChangePasswordAsync(
                    user,
                    dto.CurrentPassword,
                    dto.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            await _service.ChangePasswordAsync(TestUserId, dto);

            _userManager.Verify(
                manager => manager.ChangePasswordAsync(
                    user,
                    dto.CurrentPassword,
                    dto.NewPassword),
                Times.Once);
        }

        private static Mock<UserManager<ApplicationUser>>
            GetUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var options = OptionsHelper.Create(new IdentityOptions());
            var passwordHasher =
                new Mock<IPasswordHasher<ApplicationUser>>();
            var userValidators =
                new List<IUserValidator<ApplicationUser>>();
            var passwordValidators =
                new List<IPasswordValidator<ApplicationUser>>();
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errorDescriber = new IdentityErrorDescriber();
            var serviceProvider = new Mock<IServiceProvider>();
            var logger =
                new Mock<ILogger<UserManager<ApplicationUser>>>();

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

        private static RegisterPatientDto GetPatientDto() => new()
        {
            Email = TestEmail,
            Password = TestCredential,
            ConfirmPassword = TestCredential
        };

        private static DoctorCreateDto GetDoctorDto(
            string fullName = "John Doctor") => new()
            {
                DoctorEmail = "doctor@test.com",
                FullName = fullName
            };

        private static LoginDto CreateLoginDto() => new()
        {
            Email = TestEmail,
            Password = TestCredential
        };

        private static ChangePasswordDto CreateChangePasswordDto() => new()
        {
            CurrentPassword = TestCredential,
            NewPassword = NewCredential,
            ConfirmNewPassword = NewCredential
        };

        private static IdentityResult FailedIdentityResult(
            string description)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = description
            });
        }

        private void SetupPatientRegistrationBeforeIdentityCreation(
            RegisterPatientDto dto,
            Patient patient,
            IEnumerable<Patient>? existingPatients = null)
        {
            _userManager
                .Setup(manager => manager.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);
            _patientRepo
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(existingPatients?.ToList() ?? []);
            _mapper
                .Setup(mapper => mapper.Map<Patient>(dto))
                .Returns(patient);
            _patientRepo
                .Setup(repository => repository.Add(
                    It.IsAny<Patient>()))
                .ReturnsAsync(patient);
        }

        private void SetupSuccessfulPatientRegistration(
            RegisterPatientDto dto,
            Patient patient,
            IEnumerable<Patient>? existingPatients = null)
        {
            SetupPatientRegistrationBeforeIdentityCreation(
                dto,
                patient,
                existingPatients);
            _userManager
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    dto.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManager
                .Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    PatientRole))
                .ReturnsAsync(IdentityResult.Success);
        }

        private void SetupDoctorBeforeNameValidation(
            DoctorCreateDto dto)
        {
            _userManager
                .Setup(manager => manager.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);
            _doctorRepo
                .Setup(repository => repository.ExistsByEmailAsync(
                    dto.DoctorEmail))
                .ReturnsAsync(false);
            var doctor = new Doctor { DoctorId = 5 };
            _mapper
                .Setup(mapper => mapper.Map<Doctor>(dto))
                .Returns(doctor);
            _doctorRepo
                .Setup(repository => repository.Add(
                    It.IsAny<Doctor>()))
                .ReturnsAsync(doctor);
        }

        private void SetupDoctorBeforeIdentityCreation(
            DoctorCreateDto dto,
            Doctor? doctor = null)
        {
            _userManager
                .Setup(manager => manager.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);
            _doctorRepo
                .Setup(repository => repository.ExistsByEmailAsync(
                    dto.DoctorEmail))
                .ReturnsAsync(false);
            doctor ??= new Doctor { DoctorId = 5 };
            _mapper
                .Setup(mapper => mapper.Map<Doctor>(dto))
                .Returns(doctor);
            _doctorRepo
                .Setup(repository => repository.Add(
                    It.IsAny<Doctor>()))
                .ReturnsAsync(doctor);
        }

        private void SetupSuccessfulLogin(
            ApplicationUser user,
            IList<string> roles)
        {
            _userManager
                .Setup(manager => manager.FindByEmailAsync(TestEmail))
                .ReturnsAsync(user);
            _userManager
                .Setup(manager => manager.CheckPasswordAsync(
                    user,
                    TestCredential))
                .ReturnsAsync(true);
            _userManager
                .Setup(manager => manager.GetRolesAsync(user))
                .ReturnsAsync(roles);
        }
    }
}