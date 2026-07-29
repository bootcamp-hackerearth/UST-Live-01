using FluentAssertions;
using HealthAxisApplicn.Data;
using HealthAxisApplicn.Dto.Auth;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Services.Impl;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

namespace HealthAxisAPITests.APITests
{
    

    public class AuthServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();

            _userManagerMock =
                new Mock<UserManager<ApplicationUser>>(
                    store.Object,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

            var dbOptions =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            _dbContext = new AppDbContext(dbOptions);

            var settings = new Dictionary<string, string>
        {
            { "Jwt:Key", "ThisIsATestKeyForJwtTokenGeneration123456789" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" },
            { "Jwt:AccessTokenExpirationMinutes", "60" }
        };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(settings!)
                .Build();

            _service = new AuthService(
                _userManagerMock.Object,
                _configuration,
                _dbContext);
        }

        [Fact]
        public async Task LoginAsync_Should_Return_InvalidCredentials_When_User_Not_Found()
        {
            _userManagerMock
                .Setup(x => x.FindByEmailAsync("test@test.com"))
                .ReturnsAsync((ApplicationUser?)null);

            var result = await _service.LoginAsync(
                new LoginDto
                {
                    Email = "test@test.com",
                    Password = "Password123"
                });

            result.Message.Should().Be("Invalid credentials");
            result.AccessToken.Should().BeEmpty();
        }

        [Fact]
        public async Task LoginAsync_Should_Return_InvalidPassword_When_Password_Wrong()
        {
            var user = new ApplicationUser
            {
                Email = "test@test.com"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, "wrong"))
                .ReturnsAsync(false);

            var result = await _service.LoginAsync(
                new LoginDto
                {
                    Email = user.Email,
                    Password = "wrong"
                });

            result.Message.Should().Be("Invalid password");
        }

        [Fact]
        public async Task RegisterAsync_Should_Return_Error_When_Passwords_Do_Not_Match()
        {
            var result = await _service.RegisterAsync(
                new RegisterDto
                {
                    Email = "test@test.com",
                    Password = "123",
                    ConfirmPassword = "456",
                    Role = "Patient"
                });

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Passwords do not match");
        }

        [Fact]
        public async Task RegisterAsync_Should_Return_Error_When_Role_Invalid()
        {
            var result = await _service.RegisterAsync(
                new RegisterDto
                {
                    Email = "test@test.com",
                    Password = "Password123!",
                    ConfirmPassword = "Password123!",
                    Role = "Manager"
                });

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Invalid role");
        }

        [Fact]
        public async Task RefreshAsync_Should_Return_Null_When_Token_Not_Found()
        {
            var result = await _service.RefreshAsync("invalid-token");

            result.Should().BeNull();
        }

        [Fact]
        public async Task RefreshAsync_Should_Return_Null_When_Token_Revoked()
        {
            _dbContext.RefreshTokens.Add(
                new RefreshToken
                {
                    Token = "token",
                    UserId = "user1",
                    IsRevoked = true,
                    Expires = DateTime.UtcNow.AddDays(1)
                });

            await _dbContext.SaveChangesAsync();

            var result = await _service.RefreshAsync("token");

            result.Should().BeNull();
        }

        [Fact]
        public async Task RefreshAsync_Should_Return_Null_When_Token_Expired()
        {
            _dbContext.RefreshTokens.Add(
                new RefreshToken
                {
                    Token = "token",
                    UserId = "user1",
                    IsRevoked = false,
                    Expires = DateTime.UtcNow.AddDays(-1)
                });

            await _dbContext.SaveChangesAsync();

            var result = await _service.RefreshAsync("token");

            result.Should().BeNull();
        }

        [Fact]
        public async Task RegisterAsync_Should_Return_CreateUser_Errors()
        {
            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        new IdentityError
                        {
                            Description = "Email already exists"
                        }));

            var result = await _service.RegisterAsync(
                new RegisterDto
                {
                    Email = "test@test.com",
                    Password = "Password123!",
                    ConfirmPassword = "Password123!",
                    Role = "Patient"
                });

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Email already exists");
        }

        [Fact]
        public async Task RefreshAsync_Should_Return_Null_When_User_Not_Found()
        {
            _dbContext.RefreshTokens.Add(
                new RefreshToken
                {
                    Token = "token",
                    UserId = "user1",
                    Expires = DateTime.UtcNow.AddDays(1),
                    IsRevoked = false
                });

            await _dbContext.SaveChangesAsync();

            _userManagerMock
                .Setup(x => x.FindByIdAsync("user1"))
                .ReturnsAsync((ApplicationUser?)null);

            var result = await _service.RefreshAsync("token");

            result.Should().BeNull();
        }

        [Fact]
        public async Task RegisterAsync_Should_Return_Error_When_Patient_Details_Missing()
        {
            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(
                    It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterAsync(
                new RegisterDto
                {
                    Email = "test@test.com",
                    Password = "Password123!",
                    ConfirmPassword = "Password123!",
                    Role = "Patient"
                });

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Missing patient details");
        }

        [Fact]
        public async Task RegisterAsync_Should_Create_Patient()
        {
            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(
                    It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterAsync(
                new RegisterDto
                {
                    Name = "John",
                    Email = "john@test.com",
                    Password = "Password123!",
                    ConfirmPassword = "Password123!",
                    DateOfBirth = DateTime.Today.AddYears(-20),
                    Gender = "Male",
                    PhoneNo = "9999999999",
                    Role = "Patient"
                });

            result.Success.Should().BeTrue();

            _dbContext.Patients.Count()
                .Should().Be(1);
        }

        [Fact]
        public async Task RegisterAsync_Should_Set_FirstLogin_False_For_Patient()
        {
            ApplicationUser? createdUser = null;

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .Callback<ApplicationUser, string>(
                    (u, _) => createdUser = u)
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(
                    It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            await _service.RegisterAsync(
                new RegisterDto
                {
                    Name = "John",
                    Email = "john@test.com",
                    Password = "Password123!",
                    ConfirmPassword = "Password123!",
                    DateOfBirth = DateTime.Today.AddYears(-20),
                    Gender = "Male",
                    PhoneNo = "9999999999",
                    Role = "Patient"
                });

            createdUser!.IsFirstLogin.Should().BeFalse();
        }

        [Fact]
        public async Task RegisterAsync_Should_Set_FirstLogin_True_For_Doctor()
        {
            ApplicationUser? updatedUser = null;

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(
                    It.IsAny<ApplicationUser>()))
                .Callback<ApplicationUser>(
                    u => updatedUser = u)
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterAsync(
                new RegisterDto
                {
                    Email = "doctor@test.com",
                    Password = "Password123!",
                    ConfirmPassword = "Password123!",
                    Role = "Doctor"
                });

            result.Success.Should().BeTrue();
            updatedUser!.IsFirstLogin.Should().BeTrue();
        }

        [Fact]
        public async Task LoginAsync_Should_Return_Success_Response()
        {
            var user = new ApplicationUser
            {
                Id = "user1",
                Email = "user@test.com",
                IsFirstLogin = false
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, "Password123!"))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Patient" });

            var result = await _service.LoginAsync(new LoginDto
            {
                Email = user.Email,
                Password = "Password123!"
            });

            result.Should().NotBeNull();
            result.Message.Should().Be("Login successful");
        }

        [Fact]
        public async Task RefreshAsync_Should_Return_New_Token()
        {
            var user = new ApplicationUser
            {
                Id = "user1",
                Email = "user@test.com"
            };

            _dbContext.RefreshTokens.Add(
                new RefreshToken
                {
                    Token = "refresh-token",
                    UserId = "user1",
                    IsRevoked = false,
                    Expires = DateTime.UtcNow.AddDays(1)
                });

            await _dbContext.SaveChangesAsync();

            _userManagerMock
                .Setup(x => x.FindByIdAsync("user1"))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Patient" });

            var result =
                await _service.RefreshAsync("refresh-token");

            result.Should().NotBeNull();
            result!.RefreshToken.Should().Be("refresh-token");
        }

        [Fact]
        public async Task RegisterAsync_Should_Create_Doctor()
        {
            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(
                    It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterAsync(
                new RegisterDto
                {
                    Email = "doctor@test.com",
                    Password = "Password123!",
                    ConfirmPassword = "Password123!",
                    Role = "Doctor"
                });

            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task RegisterAsync_Should_Add_User_To_Role()
        {
            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(
                    It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            await _service.RegisterAsync(
                new RegisterDto
                {
                    Email = "doctor@test.com",
                    Password = "Password123!",
                    ConfirmPassword = "Password123!",
                    Role = "Doctor"
                });

            _userManagerMock.Verify(
                x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"),
                Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_Should_Update_User_After_Setting_FirstLogin()
        {
            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(
                    It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            await _service.RegisterAsync(
                new RegisterDto
                {
                    Email = "doctor@test.com",
                    Password = "Password123!",
                    ConfirmPassword = "Password123!",
                    Role = "Doctor"
                });

            _userManagerMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<ApplicationUser>()),
                Times.Once);
        }

        [Fact]
        public async Task LoginAsync_Should_Return_IsFirstLogin_True()
        {
            var user = new ApplicationUser
            {
                Id = "user1",
                Email = "user@test.com",
                IsFirstLogin = true
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, "Password123!"))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Doctor" });

            var result = await _service.LoginAsync(new LoginDto
            {
                Email = user.Email,
                Password = "Password123!"
            });

            result.IsFirstLogin.Should().BeTrue();
        }

        [Fact]
        public async Task LoginAsync_Should_Save_RefreshToken()
        {
            var user = new ApplicationUser
            {
                Id = "user1",
                Email = "user@test.com"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, "Password123!"))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            await _service.LoginAsync(new LoginDto
            {
                Email = user.Email,
                Password = "Password123!"
            });

            _dbContext.RefreshTokens.Count()
                .Should().Be(1);
        }

        [Fact]
        public async Task RegisterAsync_Should_Create_Admin()
        {
            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(
                    It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RegisterAsync(
                new RegisterDto
                {
                    Email = "admin@test.com",
                    Password = "Password123!",
                    ConfirmPassword = "Password123!",
                    Role = "Admin"
                });

            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task RegisterAsync_Should_Not_Create_Patient_For_Doctor()
        {
            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(
                    It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            await _service.RegisterAsync(
                new RegisterDto
                {
                    Email = "doctor@test.com",
                    Password = "Password123!",
                    ConfirmPassword = "Password123!",
                    Role = "Doctor"
                });

            _dbContext.Patients.Should().BeEmpty();
        }

        [Fact]
        public async Task RefreshAsync_Should_Return_Configured_Expiry()
        {
            var user = new ApplicationUser
            {
                Id = "user1",
                Email = "user@test.com"
            };

            _dbContext.RefreshTokens.Add(
                new RefreshToken
                {
                    Token = "token",
                    UserId = "user1",
                    IsRevoked = false,
                    Expires = DateTime.UtcNow.AddDays(1)
                });

            await _dbContext.SaveChangesAsync();

            _userManagerMock
                .Setup(x => x.FindByIdAsync("user1"))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var result = await _service.RefreshAsync("token");

            result!.ExpiresIn.Should().Be(60);
        }

        [Fact]
        public async Task LoginAsync_Should_Return_Configured_Expiry()
        {
            var user = new ApplicationUser
            {
                Id = "user1",
                Email = "user@test.com"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, "Password123!"))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var result = await _service.LoginAsync(new LoginDto
            {
                Email = user.Email,
                Password = "Password123!"
            });

            result.ExpiresIn.Should().Be(60);
        }

        [Fact]
        public async Task RegisterAsync_Should_Copy_InsuranceId()
        {
            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(
                    It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            await _service.RegisterAsync(
                new RegisterDto
                {
                    Name = "John",
                    Email = "john@test.com",
                    Password = "Password123!",
                    ConfirmPassword = "Password123!",
                    DateOfBirth = DateTime.Today.AddYears(-20),
                    Gender = "Male",
                    PhoneNo = "9999999999",
                    InsuranceID = "INS-123",
                    Role = "Patient"
                });

            _dbContext.Patients.Single()
                .InsuranceID.Should().Be("INS-123");
        }

        [Fact]
        public async Task LoginAsync_Should_Add_PatientId_Claim()
        {
            var user = new ApplicationUser
            {
                Id = "user1",
                Email = "patient@test.com"
            };

            _dbContext.Patients.Add(new Patient
            {
                PatientId = 123,
                UserId = "user1",
                PatientName = "John",
                Email = "patient@test.com",
                PhoneNo = "9999999999",
                Gender = "Male",
                DateOfBirth = DateTime.Today.AddYears(-20)
            });

            await _dbContext.SaveChangesAsync();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, "Password123!"))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var result = await _service.LoginAsync(new LoginDto
            {
                Email = user.Email,
                Password = "Password123!"
            });

            var jwt = new JwtSecurityTokenHandler()
                .ReadJwtToken(result.AccessToken);

            jwt.Claims
                .FirstOrDefault(x => x.Type == "PatientId")
                ?.Value
                .Should()
                .Be("123");
        }

        [Fact]
        public async Task LoginAsync_Should_Add_DoctorId_Claim()
        {
            var user = new ApplicationUser
            {
                Id = "user1",
                Email = "doctor@test.com"
            };

            _dbContext.Doctors.Add(new Doctor
            {
                DoctorId = 456,
                UserId = "user1",
                DoctorName = "Doctor",
                Email = "doctor@test.com"
            });

            await _dbContext.SaveChangesAsync();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, "Password123!"))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var result = await _service.LoginAsync(new LoginDto
            {
                Email = user.Email,
                Password = "Password123!"
            });

            var jwt = new JwtSecurityTokenHandler()
                .ReadJwtToken(result.AccessToken);

            jwt.Claims
                .FirstOrDefault(x => x.Type == "DoctorId")
                ?.Value
                .Should()
                .Be("456");
        }

        [Fact]
        public async Task LoginAsync_Should_Add_Role_Claim()
        {
            var user = new ApplicationUser
            {
                Id = "user1",
                Email = "user@test.com"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, "Password123!"))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Doctor" });

            var result = await _service.LoginAsync(new LoginDto
            {
                Email = user.Email,
                Password = "Password123!"
            });

            var jwt = new JwtSecurityTokenHandler()
                .ReadJwtToken(result.AccessToken);

            jwt.Claims
                .Any(x => x.Type.Contains("role") &&
                          x.Value == "Doctor")
                .Should()
                .BeTrue();
        }


    }
}
