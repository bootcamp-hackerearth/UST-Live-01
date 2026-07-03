using AutoMapper;
using Healthcare.Shared.DTOs.Authentication;
using Healthcare.Shared.DTOs.Patient;
using HealthCare.Api.Data;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;


namespace HealthCare.Api.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPatientRepository> _patientRepoMock;
        private readonly Mock<IDoctorRepository> _doctorRepoMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly HealthCareDbContext _context;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _userManagerMock = GetUserManagerMock();
            _mapperMock = new Mock<IMapper>();
            _patientRepoMock = new Mock<IPatientRepository>();
            _doctorRepoMock = new Mock<IDoctorRepository>();

            var sectionMock = new Mock<IConfigurationSection>();

            sectionMock.Setup(s => s["Key"])
                .Returns("THIS_IS_A_SECRET_KEY_1234567891234");

            sectionMock.Setup(s => s["Issuer"])
                .Returns("TestIssuer");

            sectionMock.Setup(s => s["Audience"])
                .Returns("TestAudience");

            sectionMock.Setup(s => s["AccessTokenExpirationMinutes"])
                .Returns("60");

            _configMock = new Mock<IConfiguration>();

            _configMock.Setup(c => c.GetSection("Jwt"))
                .Returns(sectionMock.Object);

            var options = new DbContextOptionsBuilder<HealthCareDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new HealthCareDbContext(options);

            _service = new AuthService(
                _userManagerMock.Object,
                _mapperMock.Object,
                _patientRepoMock.Object,
                _doctorRepoMock.Object,
                _configMock.Object,
                _context
            );
        }
        private Mock<UserManager<IdentityUser>> GetUserManagerMock()
        {
            var store = new Mock<IUserStore<IdentityUser>>();

            return new Mock<UserManager<IdentityUser>>(
                store.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                new List<IUserValidator<IdentityUser>>(),
                new List<IPasswordValidator<IdentityUser>>(),
                Mock.Of<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>()
            );
        }

        //  Login Success (Patient)
        [Fact]
        public async Task LoginAsync_ShouldReturnToken_ForPatient()
        {
            var user = new IdentityUser { Id = "1", Email = "test@test.com" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "123"))
                .ReturnsAsync(true);

            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Patient" });

            _patientRepoMock.Setup(p => p.GetByUserIdAsync(user.Id))
                .ReturnsAsync(new Patient { PatientId = 10 });

            var result = await _service.LoginAsync(new LoginDto
            {
                Email = user.Email,
                Password = "123"
            });

            Assert.NotNull(result);
            Assert.Equal("Patient", result.Role);
            Assert.False(string.IsNullOrEmpty(result.AccessToken));
        }

        //  Login Invalid Password
        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenPasswordInvalid()
        {
            var user = new IdentityUser { Email = "test@test.com" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "123"))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.LoginAsync(new LoginDto
                {
                    Email = user.Email,
                    Password = "123"
                }));
        }

        //  Register Patient Success
        [Fact]
        public async Task RegisterPatientAsync_ShouldCreatePatient()
        {
            var dto = new CreatePatientDto
            {
                Email = "test@test.com",
                Password = "123",
                ConfirmPassword = "123"
            };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser?)null);

            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<IdentityUser>(), "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            var patient = new Patient();

            _mapperMock.Setup(m => m.Map<Patient>(dto)).Returns(patient);

            await _service.RegisterPatientAsync(dto);

            _patientRepoMock.Verify(r => r.AddAsync(patient), Times.Once);
        }

        //  Register Password Mismatch
        [Fact]
        public async Task RegisterPatientAsync_ShouldThrow_WhenPasswordMismatch()
        {
            var dto = new CreatePatientDto
            {
                Email = "test@test.com",
                Password = "123",
                ConfirmPassword = "456"
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _service.RegisterPatientAsync(dto));
        }

        //  Register Doctor
        [Fact]
        public async Task RegisterDoctorAsync_ShouldCreateDoctor()
        {
            var dto = new DoctorRegisterDto
            {
                Email = "doc@test.com",
                Password = "123",
                ConfirmPassword = "123",
                TimeSlots = new List<string> { "10" }
            };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser?)null);

            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<IdentityUser>(), "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            var doctor = new Doctor { DoctorId = 1 };

            _mapperMock.Setup(m => m.Map<Doctor>(dto)).Returns(doctor);

            await _service.RegisterDoctorAsync(dto);

            _doctorRepoMock.Verify(r => r.AddAsync(doctor), Times.Once);
            _doctorRepoMock.Verify(r => r.CreateSlots(doctor.DoctorId, dto.TimeSlots), Times.Once);
        }

        //User Mismatch
        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenUserNotFound()
        {
            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser?)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.LoginAsync(new LoginDto
                {
                    Email = "invalid@test.com",
                    Password = "123"
                }));
        }

        //No Roles Assigned

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenNoRoles()
        {
            var user = new IdentityUser { Id = "1", Email = "test@test.com" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "123"))
                .ReturnsAsync(true);

            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                .ReturnsAsync(new List<string>()); // no roles

            await Assert.ThrowsAsync<Exception>(() =>
                _service.LoginAsync(new LoginDto
                {
                    Email = user.Email,
                    Password = "123"
                }));
        }

        //Patient Not Found

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenPatientNotFound()
        {
            var user = new IdentityUser { Id = "1", Email = "test@test.com" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "123"))
                .ReturnsAsync(true);

            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Patient" });

            _patientRepoMock.Setup(p => p.GetByUserIdAsync(user.Id))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.LoginAsync(new LoginDto
                {
                    Email = user.Email,
                    Password = "123"
                }));
        }

        //Doctor Role-Sucess
        [Fact]
        public async Task LoginAsync_ShouldReturnToken_ForDoctor()
        {
            var user = new IdentityUser { Id = "2", Email = "doc@test.com" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "123"))
                .ReturnsAsync(true);

            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Doctor" });

            _doctorRepoMock.Setup(d => d.GetByUserIdAsync(user.Id))
                .ReturnsAsync(new Doctor { DoctorId = 20 });

            var result = await _service.LoginAsync(new LoginDto
            {
                Email = user.Email,
                Password = "123"
            });

            Assert.Equal("Doctor", result.Role);
            Assert.False(string.IsNullOrEmpty(result.AccessToken));
        }

        //Doctor Role Not found
        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenDoctorNotFound()
        {
            var user = new IdentityUser { Id = "2", Email = "doc@test.com" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "123"))
                .ReturnsAsync(true);

            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Doctor" });

            _doctorRepoMock.Setup(d => d.GetByUserIdAsync(user.Id))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.LoginAsync(new LoginDto
                {
                    Email = user.Email,
                    Password = "123"
                }));
        }

        //Admin Role
        [Fact]
        public async Task LoginAsync_ShouldReturnToken_ForAdmin()
        {
            var user = new IdentityUser { Id = "3", Email = "admin@test.com" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "123"))
                .ReturnsAsync(true);

            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Admin" });

            var result = await _service.LoginAsync(new LoginDto
            {
                Email = user.Email,
                Password = "123"
            });

            Assert.Equal("Admin", result.Role);
            Assert.False(string.IsNullOrEmpty(result.AccessToken));
        }
        //Unknown roles
        [Fact]
        public async Task LoginAsync_ShouldThrow_ForUnknownRole()
        {
            var user = new IdentityUser { Id = "4", Email = "unknown@test.com" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "123"))
                .ReturnsAsync(true);

            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Other" });

            await Assert.ThrowsAsync<Exception>(() =>
                _service.LoginAsync(new LoginDto
                {
                    Email = user.Email,
                    Password = "123"
                }));
        }

    }
}