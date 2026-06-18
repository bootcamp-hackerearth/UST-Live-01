using HealthAxis.API.DTOs.Auth;
using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using HealthAxis.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace HealthAxis.API.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userManagerMock = CreateUserManagerMock();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _configuration = CreateConfiguration();

            _authService = new AuthService(
                _userManagerMock.Object,
                _configuration,
                _patientRepositoryMock.Object);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenPasswordsDoNotMatch_ReturnsFailure()
        {
            // Arrange
            RegisterPatientDto request = CreateRegisterPatientDto();
            request.ConfirmPassword = "Different@123";

            // Act
            var result = await _authService.RegisterPatientAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Password and confirm password do not match.", result.Message);
            Assert.Equal(string.Empty, result.UserId);
            Assert.Equal(0, result.PatientId);

            _userManagerMock.Verify(
                manager => manager.FindByEmailAsync(It.IsAny<string>()),
                Times.Never);

            _patientRepositoryMock.Verify(
                repository => repository.CreateAsync(
                    It.IsAny<Patient>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenEmailAlreadyExists_ReturnsFailure()
        {
            // Arrange
            RegisterPatientDto request = CreateRegisterPatientDto();

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(new IdentityUser
                {
                    Id = "existing-user-id",
                    Email = request.Email,
                    UserName = request.Email
                });

            // Act
            var result = await _authService.RegisterPatientAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email already exists.", result.Message);
            Assert.Equal(string.Empty, result.UserId);
            Assert.Equal(0, result.PatientId);

            _patientRepositoryMock.Verify(
                repository => repository.CreateAsync(
                    It.IsAny<Patient>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenValidRequest_ReturnsSuccess()
        {
            // Arrange
            RegisterPatientDto request = CreateRegisterPatientDto();

            Patient createdPatient = new()
            {
                PatientId = 10,
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                CreatedDate = DateTime.Now
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((IdentityUser?)null);

            _patientRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Patient>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdPatient);

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    request.Password))
                .Callback<IdentityUser, string>((user, _) =>
                {
                    user.Id = "new-user-id";
                })
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(manager => manager.AddClaimAsync(
                    It.IsAny<IdentityUser>(),
                    It.Is<Claim>(claim =>
                        claim.Type == ClaimTypes.Role &&
                        claim.Value == "Patient")))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(manager => manager.AddClaimAsync(
                    It.IsAny<IdentityUser>(),
                    It.Is<Claim>(claim =>
                        claim.Type == "ReferenceId" &&
                        claim.Value == createdPatient.PatientId.ToString())))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.RegisterPatientAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Patient registered successfully.", result.Message);
            Assert.Equal("new-user-id", result.UserId);
            Assert.Equal(createdPatient.PatientId, result.PatientId);

            _patientRepositoryMock.Verify(
                repository => repository.CreateAsync(
                    It.IsAny<Patient>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _userManagerMock.Verify(
                manager => manager.AddClaimAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<Claim>()),
                Times.Exactly(2));
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenIdentityCreateFails_ReturnsFailure()
        {
            // Arrange
            RegisterPatientDto request = CreateRegisterPatientDto();

            Patient createdPatient = new()
            {
                PatientId = 20,
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                CreatedDate = DateTime.Now
            };

            IdentityError error = new()
            {
                Description = "Password is invalid."
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((IdentityUser?)null);

            _patientRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Patient>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdPatient);

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    request.Password))
                .ReturnsAsync(IdentityResult.Failed(error));

            // Act
            var result = await _authService.RegisterPatientAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Password is invalid.", result.Message);
            Assert.Equal(string.Empty, result.UserId);
            Assert.Equal(createdPatient.PatientId, result.PatientId);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenRoleClaimFails_ReturnsFailure()
        {
            // Arrange
            RegisterPatientDto request = CreateRegisterPatientDto();

            Patient createdPatient = new()
            {
                PatientId = 30,
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                CreatedDate = DateTime.Now
            };

            IdentityError error = new()
            {
                Description = "Role claim failed."
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((IdentityUser?)null);

            _patientRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Patient>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdPatient);

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    request.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(manager => manager.AddClaimAsync(
                    It.IsAny<IdentityUser>(),
                    It.Is<Claim>(claim =>
                        claim.Type == ClaimTypes.Role &&
                        claim.Value == "Patient")))
                .ReturnsAsync(IdentityResult.Failed(error));

            // Act
            var result = await _authService.RegisterPatientAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Role claim failed.", result.Message);
            Assert.Equal(string.Empty, result.UserId);
            Assert.Equal(createdPatient.PatientId, result.PatientId);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenReferenceIdClaimFails_ReturnsFailure()
        {
            // Arrange
            RegisterPatientDto request = CreateRegisterPatientDto();

            Patient createdPatient = new()
            {
                PatientId = 40,
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                CreatedDate = DateTime.Now
            };

            IdentityError error = new()
            {
                Description = "Reference claim failed."
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((IdentityUser?)null);

            _patientRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Patient>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdPatient);

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    request.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(manager => manager.AddClaimAsync(
                    It.IsAny<IdentityUser>(),
                    It.Is<Claim>(claim =>
                        claim.Type == ClaimTypes.Role &&
                        claim.Value == "Patient")))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(manager => manager.AddClaimAsync(
                    It.IsAny<IdentityUser>(),
                    It.Is<Claim>(claim =>
                        claim.Type == "ReferenceId" &&
                        claim.Value == createdPatient.PatientId.ToString())))
                .ReturnsAsync(IdentityResult.Failed(error));

            // Act
            var result = await _authService.RegisterPatientAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Reference claim failed.", result.Message);
            Assert.Equal(string.Empty, result.UserId);
            Assert.Equal(createdPatient.PatientId, result.PatientId);
        }

        [Fact]
        public async Task LoginAsync_WhenUserDoesNotExist_ReturnsFailure()
        {
            // Arrange
            LoginDto request = new()
            {
                Email = "missing@test.com",
                Password = "Password@123"
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((IdentityUser?)null);

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid email or password.", result.Message);
            Assert.Equal(string.Empty, result.AccessToken);
            Assert.Equal(string.Empty, result.RefreshToken);
            Assert.Equal(0, result.ExpiresIn);
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordIsInvalid_ReturnsFailure()
        {
            // Arrange
            LoginDto request = new()
            {
                Email = "patient@test.com",
                Password = "Wrong@123"
            };

            IdentityUser user = new()
            {
                Id = "patient-user-id",
                Email = request.Email,
                UserName = request.Email
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.CheckPasswordAsync(user, request.Password))
                .ReturnsAsync(false);

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid email or password.", result.Message);
            Assert.Equal(string.Empty, result.AccessToken);
            Assert.Equal(string.Empty, result.RefreshToken);
        }

        [Fact]
        public async Task LoginAsync_WhenCredentialsAreValid_ReturnsTokenAndUserDetails()
        {
            // Arrange
            LoginDto request = new()
            {
                Email = "patient@test.com",
                Password = "Password@123"
            };

            IdentityUser user = new()
            {
                Id = "patient-user-id",
                Email = request.Email,
                UserName = request.Email
            };

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Role, "Patient"),
                new Claim("ReferenceId", "15")
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.CheckPasswordAsync(user, request.Password))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(manager => manager.GetClaimsAsync(user))
                .ReturnsAsync(claims);

            _userManagerMock
                .Setup(manager => manager.SetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Login successful.", result.Message);
            Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));
            Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));
            Assert.Equal(1800, result.ExpiresIn);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal("Patient", result.Role);
            Assert.Equal(15, result.ReferenceId);

            _userManagerMock.Verify(
                manager => manager.SetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Exactly(2));
        }

        [Fact]
        public async Task LoginAsync_WhenReferenceIdIsInvalid_ReturnsReferenceIdAsZero()
        {
            // Arrange
            LoginDto request = new()
            {
                Email = "patient@test.com",
                Password = "Password@123"
            };

            IdentityUser user = new()
            {
                Id = "patient-user-id",
                Email = request.Email,
                UserName = request.Email
            };

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Role, "Patient"),
                new Claim("ReferenceId", "invalid-reference-id")
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.CheckPasswordAsync(user, request.Password))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(manager => manager.GetClaimsAsync(user))
                .ReturnsAsync(claims);

            _userManagerMock
                .Setup(manager => manager.SetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(0, result.ReferenceId);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenUserDoesNotExist_ReturnsFailure()
        {
            // Arrange
            RefreshTokenDto request = new()
            {
                UserId = "missing-user-id",
                RefreshToken = "refresh-token"
            };

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(request.UserId))
                .ReturnsAsync((IdentityUser?)null);

            // Act
            var result = await _authService.RefreshTokenAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid refresh token request.", result.Message);
            Assert.Equal(string.Empty, result.AccessToken);
            Assert.Equal(string.Empty, result.RefreshToken);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenRefreshTokenDoesNotMatch_ReturnsFailure()
        {
            // Arrange
            RefreshTokenDto request = new()
            {
                UserId = "user-id",
                RefreshToken = "request-token"
            };

            IdentityUser user = new()
            {
                Id = request.UserId,
                Email = "patient@test.com",
                UserName = "patient@test.com"
            };

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(request.UserId))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    "RefreshToken"))
                .ReturnsAsync("stored-token");

            // Act
            var result = await _authService.RefreshTokenAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid refresh token.", result.Message);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenRefreshTokenExpiryMissing_ReturnsFailure()
        {
            // Arrange
            RefreshTokenDto request = new()
            {
                UserId = "user-id",
                RefreshToken = "valid-token"
            };

            IdentityUser user = new()
            {
                Id = request.UserId,
                Email = "patient@test.com",
                UserName = "patient@test.com"
            };

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(request.UserId))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    "RefreshToken"))
                .ReturnsAsync(request.RefreshToken);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    "RefreshTokenExpiryTime"))
                .ReturnsAsync(string.Empty);

            // Act
            var result = await _authService.RefreshTokenAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Refresh token expired.", result.Message);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenRefreshTokenExpired_ReturnsFailure()
        {
            // Arrange
            RefreshTokenDto request = new()
            {
                UserId = "user-id",
                RefreshToken = "valid-token"
            };

            IdentityUser user = new()
            {
                Id = request.UserId,
                Email = "patient@test.com",
                UserName = "patient@test.com"
            };

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(request.UserId))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    "RefreshToken"))
                .ReturnsAsync(request.RefreshToken);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    "RefreshTokenExpiryTime"))
                .ReturnsAsync(DateTime.UtcNow.AddDays(-1).ToString("O"));

            // Act
            var result = await _authService.RefreshTokenAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Refresh token expired.", result.Message);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenRefreshTokenValid_ReturnsNewTokens()
        {
            // Arrange
            RefreshTokenDto request = new()
            {
                UserId = "user-id",
                RefreshToken = "valid-token"
            };

            IdentityUser user = new()
            {
                Id = request.UserId,
                Email = "patient@test.com",
                UserName = "patient@test.com"
            };

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Role, "Patient"),
                new Claim("ReferenceId", "7")
            };

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(request.UserId))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    "RefreshToken"))
                .ReturnsAsync(request.RefreshToken);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    "RefreshTokenExpiryTime"))
                .ReturnsAsync(DateTime.UtcNow.AddDays(1).ToString("O"));

            _userManagerMock
                .Setup(manager => manager.GetClaimsAsync(user))
                .ReturnsAsync(claims);

            _userManagerMock
                .Setup(manager => manager.SetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.RefreshTokenAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Token refreshed successfully.", result.Message);
            Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));
            Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));
            Assert.Equal(1800, result.ExpiresIn);

            _userManagerMock.Verify(
                manager => manager.SetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Exactly(2));
        }

        private static RegisterPatientDto CreateRegisterPatientDto()
        {
            return new RegisterPatientDto
            {
                FullName = "Patient Test",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "patient@test.com",
                Password = "Password@123",
                ConfirmPassword = "Password@123"
            };
        }

        private static IConfiguration CreateConfiguration()
        {
            Dictionary<string, string?> configurationValues = new()
            {
                { "Jwt:Issuer", "HealthAxis.API" },
                { "Jwt:Audience", "HealthAxis.Clients" },
                { "Jwt:Key", "THIS_IS_A_DEVELOPMENT_SECRET_KEY_CHANGE_LATER_123456789" },
                { "Jwt:AccessTokenExpirationMinutes", "30" },
                { "Jwt:RefreshTokenDays", "7" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(configurationValues)
                .Build();
        }

        private static Mock<UserManager<IdentityUser>> CreateUserManagerMock()
        {
            Mock<IUserStore<IdentityUser>> userStoreMock = new();

            return new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                Array.Empty<IUserValidator<IdentityUser>>(),
                Array.Empty<IPasswordValidator<IdentityUser>>(),
                Mock.Of<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());
        }
    }
}
