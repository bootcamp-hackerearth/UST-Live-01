using Xunit;
using Moq;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using HealthApp.Api.Service.Impl;
using HealthApp.Api.Repository.Interface;
using HealthApp.Shared.Dto;
using HealthApp.Api.Model;

namespace HealthApp.Test.Service_Testing
{
    public class AuthServiceTesting
    {
        private readonly Mock<UserManager<IdentityUser>> _userManager;
        private readonly Mock<IConfiguration> _config;
        private readonly Mock<IPatientRepository> _patientRepo;
        private readonly Mock<IDoctorRepository> _doctorRepo;

        private readonly AuthService _service;

        public AuthServiceTesting()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            _userManager = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            var settings = new Dictionary<string, string> {
                {"Jwt:key", "THIS_IS_SUPER_SECRET_KEY_12345"},
                {"Jwt:Issuer", "test"},
                {"Jwt:Audience", "test"},
                {"Jwt:AccessTokenExpirationMinutes", "60"}
            };

            _config = new Mock<IConfiguration>();
            _config.Setup(x => x.GetSection("Jwt")["key"]).Returns(settings["Jwt:key"]);
            _config.Setup(x => x.GetSection("Jwt")["Issuer"]).Returns(settings["Jwt:Issuer"]);
            _config.Setup(x => x.GetSection("Jwt")["Audience"]).Returns(settings["Jwt:Audience"]);
            _config.Setup(x => x.GetSection("Jwt")["AccessTokenExpirationMinutes"])
                   .Returns(settings["Jwt:AccessTokenExpirationMinutes"]);

            _patientRepo = new Mock<IPatientRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();

            _service = new AuthService(
                _userManager.Object,
                _config.Object,
                _patientRepo.Object,
                _doctorRepo.Object
            );
        }

        [Fact]
        public async Task Login_ShouldFail_WhenUserNotFound()
        {
            _userManager.Setup(x => x.FindByEmailAsync("test@mail.com"))
                .ReturnsAsync((IdentityUser)null);

            var result = await _service.Login(new LoginDto
            {
                Email = "test@mail.com",
                Password = "123"
            });

            result.success.Should().BeFalse();
        }

        [Fact]
        public async Task Login_ShouldFail_WhenPasswordInvalid()
        {
            var user = new IdentityUser { Email = "test@mail.com" };

            _userManager.Setup(x => x.FindByEmailAsync("test@mail.com"))
                .ReturnsAsync(user);

            _userManager.Setup(x => x.CheckPasswordAsync(user, "123"))
                .ReturnsAsync(false);

            var result = await _service.Login(new LoginDto
            {
                Email = "test@mail.com",
                Password = "123"
            });

            result.success.Should().BeFalse();
        }

        [Fact]
        public async Task Register_ShouldCreateAdmin()
        {
            var dto = new RegisterDto
            {
                Email = "admin@mail.com",
                Password = "123",
                ConfirmPassword = "123",
                Role = "Admin"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser)null);

            _userManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.Register(dto);

            result.success.Should().BeTrue();
        }

        [Fact]
        public async Task Register_ShouldFail_WhenPasswordsMismatch()
        {
            var dto = new RegisterDto
            {
                Password = "1",
                ConfirmPassword = "2"
            };

            var result = await _service.Register(dto);

            result.success.Should().BeFalse();
        }

        [Fact]
        public async Task RegisterPatient_ShouldCreatePatient()
        {
            var dto = new PatientRegisterDto
            {
                Email = "user@mail.com",
                Password = "123",
                ConfirmPassword = "123",
                DateOfBirth = DateTime.Now,
                FullName = "Test User"
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser)null);

            _userManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            _patientRepo.Setup(x => x.addAsync(It.IsAny<Patient>()))
                .ReturnsAsync(new Patient());

            var result = await _service.RegisterPatientAsync(dto);

            result.success.Should().BeTrue();
        }

        [Fact]
        public async Task RegisterPatient_ShouldFail_WhenDOBMissing()
        {
            var dto = new PatientRegisterDto();

            var result = await _service.RegisterPatientAsync(dto);

            result.success.Should().BeFalse();
        }
        [Fact]
        public async Task RegisterDoctor_ShouldCreateDoctor()
        {
            var dto = new DoctorRegisterDto
            {
                Email = "doc@mail.com",
                FullName = "Doc",
                Specialisation = "Cardio",
                PracticeStartDate = System.DateTime.Now
            };

            _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser)null);

            _doctorRepo.Setup(x => x.getallAsync())
                .ReturnsAsync(new List<Doctor>());

            _userManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            _doctorRepo.Setup(x => x.addAsync(It.IsAny<Doctor>()))
                .ReturnsAsync(new Doctor());

            var result = await _service.RegisterDoctorByAdminAsync(dto);

            result.success.Should().BeTrue();
        }
    }
}