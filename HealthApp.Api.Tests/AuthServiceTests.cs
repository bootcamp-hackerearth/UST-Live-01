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
using System.Collections.Generic;
using System.Threading.Tasks;

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

            return new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null, null, null, null, null, null, null, null);
        }


        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenPasswordsMismatch()
        {
            var dto = new RegisterPatientDto
            {
                Email = "test@test.com",
                Password = "123",
                ConfirmPassword = "456"
            };

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenIdentityExists()
        {
            var dto = new RegisterPatientDto
            {
                Email = "test@test.com",
                Password = "123",
                ConfirmPassword = "123"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(new ApplicationUser());

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenPatientEmailExists()
        {
            var dto = new RegisterPatientDto
            {
                Email = "test@test.com",
                Password = "123",
                ConfirmPassword = "123"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _patientRepo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>
                {
                    new Patient { Email = dto.Email }
                });

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
            Assert.Equal("Patient email is already registered.", result.message);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenCreateUserFails()
        {
            var dto = new RegisterPatientDto
            {
                Email = "test@test.com",
                Password = "123",
                ConfirmPassword = "123"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _patientRepo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            var patient = new Patient { PatientId = 1 };

            _mapper.Setup(x => x.Map<Patient>(dto)).Returns(patient);
            _patientRepo.Setup(x => x.Add(It.IsAny<Patient>())).ReturnsAsync(patient);

            _userManager.Setup(x =>
                x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await _service.RegisterPatient(dto);

            Assert.False(result.success);
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenRoleFails()
        {
            var dto = new RegisterPatientDto
            {
                Email = "test@test.com",
                Password = "123",
                ConfirmPassword = "123"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _patientRepo.Setup(x => x.GetAllAsync())
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
        public async Task RegisterPatient_ShouldSucceed()
        {
            var dto = new RegisterPatientDto
            {
                Email = "test@test.com",
                Password = "123",
                ConfirmPassword = "123"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _patientRepo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            var patient = new Patient { PatientId = 1 };

            _mapper.Setup(x => x.Map<Patient>(dto)).Returns(patient);
            _patientRepo.Setup(x => x.Add(It.IsAny<Patient>())).ReturnsAsync(patient);

            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterPatient(dto);

            Assert.True(result.success);
            Assert.False(string.IsNullOrEmpty(result.userId));
        }


        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenIdentityExists()
        {
            var dto = new DoctorCreateDto { DoctorEmail = "doc@test.com", FullName = "John Doe" };

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(new ApplicationUser());

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _service.RegisterDoctor(dto));
        }

        [Fact]
        public async Task RegisterDoctor_ShouldThrow_WhenDoctorExists()
        {
            var dto = new DoctorCreateDto { DoctorEmail = "doc@test.com", FullName = "John Doe" };

            _userManager.Setup(x => x.FindByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync((ApplicationUser?)null);

            _doctorRepo.Setup(x => x.ExistsByEmailAsync(dto.DoctorEmail))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _service.RegisterDoctor(dto));
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
        public async Task Login_ShouldFail_WhenUserNotFound()
        {
            _userManager.Setup(x => x.FindByEmailAsync("a@test.com"))
                .ReturnsAsync((ApplicationUser?)null);

            var result = await _service.Login(new LoginDto { Email = "a@test.com", Password = "123" });

            Assert.False(result.success);
        }

        [Fact]
        public async Task Login_ShouldFail_WhenPasswordInvalid()
        {
            var user = new ApplicationUser { Email = "a@test.com" };

            _userManager.Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManager.Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _service.Login(new LoginDto { Email = user.Email, Password = "123" });

            Assert.False(result.success);
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenUserIdEmpty()
        {
            await Assert.ThrowsAsync<UnauthorizedAccessAppException>(() =>
                _service.ChangePasswordAsync("", new ChangePasswordDto()));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenSamePassword()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = "same",
                NewPassword = "same",
                ConfirmNewPassword = "same"
            };

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.ChangePasswordAsync("1", dto));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenUserNotFound()
        {
            _userManager.Setup(x => x.FindByIdAsync("1"))
                .ReturnsAsync((ApplicationUser?)null);

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.ChangePasswordAsync("1", new ChangePasswordDto()));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrow_WhenChangeFails()
        {
            var user = new ApplicationUser { Id = "1" };

            _userManager.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            _userManager.Setup(x => x.ChangePasswordAsync(user, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.ChangePasswordAsync("1", new ChangePasswordDto
                {
                    CurrentPassword = "old",
                    NewPassword = "new",
                    ConfirmNewPassword = "new"
                }));
        }
    }
}