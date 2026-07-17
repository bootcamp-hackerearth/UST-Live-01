using FluentAssertions;
using HealthAxis.API.Data;
using HealthAxis.Shared.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Models.Auth;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Healthcare.netcore.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IRepository<Patient>> _patientRepositoryMock;
        private readonly HealthAxisDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);

            _patientRepositoryMock = new Mock<IRepository<Patient>>();

            var options = new DbContextOptionsBuilder<HealthAxisDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new HealthAxisDbContext(options);

            var configData = new Dictionary<string, string?>
            {
                { "Jwt:Issuer", "HealthAxis.API" },
                { "Jwt:Audience", "HealthAxis.API" },
                { "Jwt:Key", "ThisIsMySuperSecretKeyhealthcare2026" },
                { "Jwt:AccessTokenExpirationMinutes", "15" },
                { "Jwt:RefreshTokenExpirationDays", "7" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configData)
                .Build();

            _service = new AuthService(
                _userManagerMock.Object,
                _patientRepositoryMock.Object,
                _dbContext,
                _configuration);
        }

        private static RegisterDto GetRegisterDto()
        {
            return new RegisterDto
            {
                FullName = "Kiran",
                DateOfBirth = new DateTime(2003, 8, 21),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "kiran@gmail.com",
                Password = "Kiran@21",
                ConfirmPassword = "Kiran@21",
                InsuranceId = "INS1003"
            };
        }

        [Fact]
        public async Task Register_WhenPasswordsDoNotMatch_ReturnsFailure()
        {
            var dto = GetRegisterDto();
            dto.ConfirmPassword = "Wrong@21";

            var result = await _service.Register(dto);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Passwords do not match");
            result.UserId.Should().BeEmpty();
        }

        [Fact]
        public async Task Register_WhenIdentityCreateFails_ReturnsFailure()
        {
            var dto = GetRegisterDto();

            var identityErrors = new[]
            {
                new IdentityError { Description = "Email already exists" }
            };

            _userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Failed(identityErrors));

            var result = await _service.Register(dto);

            result.Success.Should().BeFalse();
            result.Message.Should().Contain("Email already exists");
        }

        [Fact]
        public async Task Register_WhenValidPatient_ReturnsSuccess()
        {
            var dto = GetRegisterDto();

            _userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .Callback<ApplicationUser, string>((user, _) =>
                {
                    user.Id = "user-1";
                })
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            _patientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Patient>()))
                .ReturnsAsync((Patient patient) => patient);

            var result = await _service.Register(dto);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Patient registered successfully");

            _userManagerMock.Verify(
                x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Patient"),
                Times.Once);

            _patientRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Patient>()),
                Times.Once);
        }

        [Fact]
        public async Task Login_WhenUserDoesNotExist_ReturnsFailure()
        {
            var dto = new LoginDto
            {
                Email = "unknown@gmail.com",
                Password = "Password@123"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            var result = await _service.Login(dto);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid credentials");
            result.AccessToken.Should().BeEmpty();
            result.RefreshToken.Should().BeEmpty();
            result.ExpiresIn.Should().Be(0);
            result.RequiresPasswordChange.Should().BeFalse();
        }

        [Fact]
        public async Task Login_WhenPasswordIsWrong_ReturnsFailure()
        {
            var dto = new LoginDto
            {
                Email = "kiran@gmail.com",
                Password = "Wrong@123"
            };

            var user = new ApplicationUser
            {
                Id = "user-1",
                Email = dto.Email,
                UserName = dto.Email
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, dto.Password))
                .ReturnsAsync(false);

            var result = await _service.Login(dto);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid credentials");
            result.AccessToken.Should().BeEmpty();
            result.RefreshToken.Should().BeEmpty();
        }

        [Fact]
        public async Task Login_WhenMustChangePasswordIsTrue_ReturnsPasswordChangeRequired()
        {
            var dto = new LoginDto
            {
                Email = "doctor@gmail.com",
                Password = "Temp@123"
            };

            var user = new ApplicationUser
            {
                Id = "doctor-1",
                Email = dto.Email,
                UserName = dto.Email,
                MustChangePassword = true
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, dto.Password))
                .ReturnsAsync(true);

            var result = await _service.Login(dto);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Password change required");
            result.AccessToken.Should().BeEmpty();
            result.RefreshToken.Should().BeEmpty();
            result.ExpiresIn.Should().Be(0);
            result.RequiresPasswordChange.Should().BeTrue();
        }

        [Fact]
        public async Task Login_WhenCredentialsAreValid_ReturnsAccessTokenAndRefreshToken()
        {
            var dto = new LoginDto
            {
                Email = "kiran@gmail.com",
                Password = "Kiran@21"
            };

            var user = new ApplicationUser
            {
                Id = "user-1",
                Email = dto.Email,
                UserName = dto.Email,
                MustChangePassword = false
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, dto.Password))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Patient" });

            var result = await _service.Login(dto);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Login successful");
            result.AccessToken.Should().NotBeNullOrWhiteSpace();
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();
            result.ExpiresIn.Should().Be(15);
            result.RequiresPasswordChange.Should().BeFalse();

            _dbContext.RefreshTokens.Count().Should().Be(1);
        }

        [Fact]
        public async Task RefreshToken_WhenTokenDoesNotExist_ReturnsFailure()
        {
            var request = new RefreshTokenRequestDto
            {
                RefreshToken = "invalid-token"
            };

            var result = await _service.RefreshToken(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid refresh token");
            result.AccessToken.Should().BeEmpty();
            result.RefreshToken.Should().BeEmpty();
            result.ExpiresIn.Should().Be(0);
        }

        [Fact]
        public async Task RefreshToken_WhenTokenIsRevoked_ReturnsFailure()
        {
            var token = new RefreshToken
            {
                UserId = "user-1",
                Token = "revoked-token",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = true
            };

            await _dbContext.RefreshTokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();

            var request = new RefreshTokenRequestDto
            {
                RefreshToken = "revoked-token"
            };

            var result = await _service.RefreshToken(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Refresh token is revoked");
            result.AccessToken.Should().BeEmpty();
            result.RefreshToken.Should().BeEmpty();
        }

        [Fact]
        public async Task RefreshToken_WhenTokenExpired_ReturnsFailure()
        {
            var token = new RefreshToken
            {
                UserId = "user-1",
                Token = "expired-token",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                ExpiresAt = DateTime.UtcNow.AddDays(-1),
                IsRevoked = false
            };

            await _dbContext.RefreshTokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();

            var request = new RefreshTokenRequestDto
            {
                RefreshToken = "expired-token"
            };

            var result = await _service.RefreshToken(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Refresh token expired");
            result.AccessToken.Should().BeEmpty();
            result.RefreshToken.Should().BeEmpty();
        }

        [Fact]
        public async Task RefreshToken_WhenUserDoesNotExist_ReturnsFailure()
        {
            var token = new RefreshToken
            {
                UserId = "missing-user",
                Token = "valid-token-user-missing",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _dbContext.RefreshTokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();

            _userManagerMock
                .Setup(x => x.FindByIdAsync("missing-user"))
                .ReturnsAsync((ApplicationUser?)null);

            var request = new RefreshTokenRequestDto
            {
                RefreshToken = "valid-token-user-missing"
            };

            var result = await _service.RefreshToken(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("User not found");
            result.AccessToken.Should().BeEmpty();
            result.RefreshToken.Should().BeEmpty();
        }

        [Fact]
        public async Task RefreshToken_WhenValid_ReturnsNewAccessTokenAndRefreshToken()
        {
            var oldRefreshToken = new RefreshToken
            {
                UserId = "user-1",
                Token = "valid-refresh-token",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _dbContext.RefreshTokens.AddAsync(oldRefreshToken);
            await _dbContext.SaveChangesAsync();

            var user = new ApplicationUser
            {
                Id = "user-1",
                Email = "kiran@gmail.com",
                UserName = "kiran@gmail.com",
                MustChangePassword = false
            };

            _userManagerMock
                .Setup(x => x.FindByIdAsync("user-1"))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Patient" });

            var request = new RefreshTokenRequestDto
            {
                RefreshToken = "valid-refresh-token"
            };

            var result = await _service.RefreshToken(request);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Token refreshed successfully");
            result.AccessToken.Should().NotBeNullOrWhiteSpace();
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();
            result.ExpiresIn.Should().Be(15);

            var revokedOldToken = _dbContext.RefreshTokens
                .First(x => x.Token == "valid-refresh-token");

            revokedOldToken.IsRevoked.Should().BeTrue();

            _dbContext.RefreshTokens.Count().Should().Be(2);
        }

        [Fact]
        public async Task ChangePassword_WhenConfirmPasswordDoesNotMatch_ReturnsFailure()
        {
            var request = new ChangePasswordDto
            {
                Email = "doctor@gmail.com",
                OldPassword = "Temp@123",
                NewPassword = "Doctor@123",
                ConfirmPassword = "Wrong@123"
            };

            var result = await _service.ChangePassword(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("New password and confirm password do not match");
        }

        [Fact]
        public async Task ChangePassword_WhenUserDoesNotExist_ReturnsFailure()
        {
            var request = new ChangePasswordDto
            {
                Email = "missing@gmail.com",
                OldPassword = "Temp@123",
                NewPassword = "Doctor@123",
                ConfirmPassword = "Doctor@123"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            var result = await _service.ChangePassword(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task ChangePassword_WhenValid_ReturnsSuccess()
        {
            var request = new ChangePasswordDto
            {
                Email = "doctor@gmail.com",
                OldPassword = "Temp@123",
                NewPassword = "Doctor@123",
                ConfirmPassword = "Doctor@123"
            };

            var user = new ApplicationUser
            {
                Id = "doctor-1",
                Email = request.Email,
                UserName = request.Email,
                MustChangePassword = true
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.ChangePasswordAsync(
                    user,
                    request.OldPassword,
                    request.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.ChangePassword(request);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Password changed successfully");
            user.MustChangePassword.Should().BeFalse();
        }
    }
}