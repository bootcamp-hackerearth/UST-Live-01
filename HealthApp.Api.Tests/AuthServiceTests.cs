using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using HealthApp.Api.Services.Impl;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Models;
using HealthApp.Api.Exceptions;
using AutoMapper;
using System.Linq;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;

namespace HealthApp.Api.Tests.Services
{
    public class AuthServiceTests
    {
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

            var configDict = new Dictionary<string, string?>
        {
            { "Jwt:Key", "THIS_IS_A_SECRET_KEY_123456789" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" },
            { "Jwt:AccessTokenExpirationMinutes", "60" }
        };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(configDict)
                .Build();

            _service = new AuthService(
                _userManager.Object,
                _config,
                _mapper.Object,
                _patientRepo.Object,
                _doctorRepo.Object
            );
        }

        private static Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();

            var options = new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>();
            options.Setup(o => o.Value).Returns(new IdentityOptions());

            var passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
            var userValidators = new List<IUserValidator<ApplicationUser>>();
            var passwordValidators = new List<IPasswordValidator<ApplicationUser>>();
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errors = new IdentityErrorDescriber();
            var services = new Mock<IServiceProvider>();
            var logger = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<ApplicationUser>>>();

            return new Mock<UserManager<ApplicationUser>>(
                store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                keyNormalizer.Object,
                errors,
                services.Object,
                logger.Object
            );
        }


        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenPasswordsDoNotMatch()
        {
            var dto = new RegisterPatientDto
            {
                Email = "a@test.com",
                Password = "Password123!",
                ConfirmPassword = "Mismatch123!"
            };

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenEmailAlreadyExistsInIdentity()
        {
            var dto = new RegisterPatientDto
            {
                Email = "a@test.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(new ApplicationUser());

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenEmailExistsInPatientTable()
        {
            var dto = new RegisterPatientDto
            {
                Email = "a@test.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _patientRepo.Setup(x => x.GetPatientsAsync(null, dto.Email))
                .ReturnsAsync(new List<Patient>
                {
                new Patient { Email = dto.Email }
                });

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
        }

        [Fact]
        public async Task RegisterPatient_ShouldCreateUser_WhenValid()
        {
            var dto = new RegisterPatientDto
            {
                Email = "a@test.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _patientRepo.Setup(x => x.GetPatientsAsync(null, dto.Email))
                .ReturnsAsync(new List<Patient>());

            var patient = new Patient { PatientId = 1 };

            _mapper.Setup(m => m.Map<Patient>(dto))
                .Returns(patient);

            _patientRepo.Setup(x => x.Add(It.IsAny<Patient>()))
                .ReturnsAsync(patient);

            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterPatient(dto);

            Assert.True(result.success);
            Assert.Equal("Patient registered successfully.", result.message);
        }


        [Fact]
        public async Task Login_ShouldFail_WhenUserNotFound()
        {
            _userManager.Setup(x => x.FindByEmailAsync("test@test.com"))
                .ReturnsAsync((ApplicationUser?)null);

            var result = await _service.Login(new LoginDto
            {
                Email = "test@test.com",
                Password = "Password123!"
            });

            Assert.False(result.success);
        }

        [Fact]
        public async Task Login_ShouldFail_WhenPasswordInvalid()
        {
            var user = new ApplicationUser { Email = "test@test.com" };

            _userManager.Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManager.Setup(x => x.CheckPasswordAsync(user, "Password123!"))
                .ReturnsAsync(false);

            var result = await _service.Login(new LoginDto
            {
                Email = user.Email,
                Password = "Password123!"
            });

            Assert.False(result.success);
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenValid()
        {
            var user = new ApplicationUser
            {
                Id = "1",
                Email = "test@test.com"
            };

            _userManager.Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManager.Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>()))
                .ReturnsAsync(true);

            _userManager.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Patient" });

            var configDict = new Dictionary<string, string?>
    {
        { "Jwt:Key", "THIS_IS_A_SUPER_SECRET_KEY_1234567890123456" },
        { "Jwt:Issuer", "TestIssuer" },
        { "Jwt:Audience", "TestAudience" },
        { "Jwt:AccessTokenExpirationMinutes", "60" }
    };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(configDict)
                .Build();

            var service = new AuthService(
                _userManager.Object,
                config,
                _mapper.Object,
                _patientRepo.Object,
                _doctorRepo.Object);

            var result = await service.Login(new LoginDto
            {
                Email = user.Email,
                Password = "Password123!"
            });

            Assert.True(result.success);
            Assert.False(string.IsNullOrEmpty(result.token));
        }


        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenUserIdEmpty()
        {
            await Assert.ThrowsAsync<UnauthorizedAccessAppException>(() =>
                _service.ChangePasswordAsync("", new ChangePasswordDto()));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenPasswordsMismatch()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = "Old123!",
                NewPassword = "New123!",
                ConfirmNewPassword = "Mismatch"
            };

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.ChangePasswordAsync("1", dto));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenUserNotFound()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = "Old123!",
                NewPassword = "New123!",
                ConfirmNewPassword = "New123!"
            };

            _userManager.Setup(x => x.FindByIdAsync("1"))
                .ReturnsAsync((ApplicationUser?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.ChangePasswordAsync("1", dto));
        }

        [Fact]
        public async Task ChangePassword_ShouldUpdate_WhenValid()
        {
            var user = new ApplicationUser { Id = "1" };

            var dto = new ChangePasswordDto
            {
                CurrentPassword = "Old123!",
                NewPassword = "New123!",
                ConfirmNewPassword = "New123!"
            };

            _userManager.Setup(x => x.FindByIdAsync("1"))
                .ReturnsAsync(user);

            _userManager.Setup(x => x.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            await _service.ChangePasswordAsync("1", dto);

            _userManager.Verify(x => x.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword), Times.Once);
        }


        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenIdentityUserExists()
        {
            var dto = new DoctorCreateDto
            {
                DoctorEmail = "doc@test.com",
                FullName = "John"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(new ApplicationUser());

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _service.RegisterDoctor(dto));
        }

        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenDoctorAlreadyExists()
        {
            var dto = new DoctorCreateDto
            {
                DoctorEmail = "doc@test.com",
                FullName = "John"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);

            _doctorRepo.Setup(x => x.ExistsByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _service.RegisterDoctor(dto));
        }

        [Fact]
        public async Task RegisterDoctor_ShouldCreateDoctor_WhenValid()
        {
            var dto = new DoctorCreateDto
            {
                DoctorEmail = "doc@test.com",
                FullName = "John Doe",
                DoctorPhoneNo = "1234567890",
                Specialisation = SpecialisationType.GeneralPhysician,
                YearsOfExperience = 5,
                ConsultationFee = 500
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);

            _doctorRepo.Setup(x => x.ExistsByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(false);

            var doctor = new Doctor { DoctorId = 1 };

            _mapper.Setup(m => m.Map<Doctor>(dto))
                .Returns(doctor);

            _doctorRepo.Setup(x => x.Add(It.IsAny<Doctor>()))
                .ReturnsAsync(doctor);

            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterDoctor(dto);

            Assert.True(result.success);
            Assert.False(string.IsNullOrEmpty(result.temporaryPassword));
        }
        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenCreateUserFails()
        {
            var dto = new RegisterPatientDto
            {
                Email = "a@test.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _patientRepo.Setup(x => x.GetPatientsAsync(null, dto.Email))
                .ReturnsAsync(new List<Patient>());

            var patient = new Patient { PatientId = 1 };

            _mapper.Setup(x => x.Map<Patient>(dto)).Returns(patient);
            _patientRepo.Setup(x => x.Add(It.IsAny<Patient>())).ReturnsAsync(patient);

            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "fail" }));

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
        }
        [Fact]
        public async Task RegisterPatient_ShouldRollback_WhenRoleFails()
        {
            var dto = new RegisterPatientDto
            {
                Email = "a@test.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _patientRepo.Setup(x => x.GetPatientsAsync(null, dto.Email))
                .ReturnsAsync(new List<Patient>());

            var patient = new Patient { PatientId = 1 };

            _mapper.Setup(x => x.Map<Patient>(dto)).Returns(patient);
            _patientRepo.Setup(x => x.Add(It.IsAny<Patient>())).ReturnsAsync(patient);

            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Patient"))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
        }
        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenCreateFails()
        {
            var dto = new DoctorCreateDto { DoctorEmail = "doc@test.com", FullName = "John Doe" };

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);

            _doctorRepo.Setup(x => x.ExistsByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(false);

            _mapper.Setup(x => x.Map<Doctor>(dto)).Returns(new Doctor());
            _doctorRepo.Setup(x => x.Add(It.IsAny<Doctor>())).ReturnsAsync(new Doctor());

            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                _service.RegisterDoctor(dto));
        }
        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenRoleFails()
        {
            var dto = new DoctorCreateDto { DoctorEmail = "doc@test.com", FullName = "John Doe" };

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);

            _doctorRepo.Setup(x => x.ExistsByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(false);

            _mapper.Setup(x => x.Map<Doctor>(dto)).Returns(new Doctor());
            _doctorRepo.Setup(x => x.Add(It.IsAny<Doctor>())).ReturnsAsync(new Doctor());

            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Doctor"))
                .ReturnsAsync(IdentityResult.Failed());

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                _service.RegisterDoctor(dto));
        }
        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenRequestNull()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.ChangePasswordAsync("1", null!));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenChangeFails()
        {
            var user = new ApplicationUser { Id = "1" };

            var dto = new ChangePasswordDto
            {
                CurrentPassword = "Old",
                NewPassword = "New",
                ConfirmNewPassword = "New"
            };

            _userManager.Setup(x => x.FindByIdAsync("1"))
                .ReturnsAsync(user);

            _userManager.Setup(x => x.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "error" }));

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.ChangePasswordAsync("1", dto));
        }

        [Fact]
        public void GeneratePassword_ShouldThrow_WhenNameShort()
        {
            Assert.ThrowsAny<Exception>(() =>
                typeof(AuthService)
                .GetMethod("GenerateTemporaryDoctorPassword", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .Invoke(null, new object[] { "Jo" }));
        }
        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenSamePassword()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = "Same123!",
                NewPassword = "Same123!",
                ConfirmNewPassword = "Same123!"
            };

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.ChangePasswordAsync("1", dto));
        }

    }
}