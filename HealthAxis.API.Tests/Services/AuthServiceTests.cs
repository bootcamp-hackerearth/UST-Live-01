using HealthAxis.API.DTOs.Auth;
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
        public async Task RegisterPatientAsync_WhenPasswordAndConfirmPasswordDoNotMatch_ReturnsFailure()
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
                    UserName = request.Email,
                    Email = request.Email
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
        public async Task RegisterPatientAsync_WhenIdentityUserCreationFails_ReturnsFailureWithCreatedPatientId()
        {
            // Arrange
            RegisterPatientDto request = CreateRegisterPatientDto();

            Patient createdPatient = new()
            {
                PatientId = 10,
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender
            };

            IdentityError identityError = new()
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
                .ReturnsAsync(IdentityResult.Failed(identityError));

            // Act
            var result = await _authService.RegisterPatientAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Password is invalid.", result.Message);
            Assert.Equal(string.Empty, result.UserId);
            Assert.Equal(createdPatient.PatientId, result.PatientId);

            _patientRepositoryMock.Verify(
                repository => repository.CreateAsync(
                    It.Is<Patient>(patient =>
                        patient.FullName == request.FullName &&
                        patient.Email == request.Email &&
                        patient.PhoneNumber == request.PhoneNumber),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenRoleClaimFails_ReturnsFailureWithCreatedPatientId()
        {
            // Arrange
            RegisterPatientDto request = CreateRegisterPatientDto();

            Patient createdPatient = new()
            {
                PatientId = 11,
                FullName = request.FullName,
                Email = request.Email
            };

            IdentityError identityError = new()
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
                .ReturnsAsync(IdentityResult.Failed(identityError));

            // Act
            var result = await _authService.RegisterPatientAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Role claim failed.", result.Message);
            Assert.Equal(string.Empty, result.UserId);
            Assert.Equal(createdPatient.PatientId, result.PatientId);

            _userManagerMock.Verify(
                manager => manager.AddClaimAsync(
                    It.IsAny<IdentityUser>(),
                    It.Is<Claim>(claim =>
                        claim.Type == "ReferenceId")),
                Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenReferenceClaimFails_ReturnsFailureWithCreatedPatientId()
        {
            // Arrange
            RegisterPatientDto request = CreateRegisterPatientDto();

            Patient createdPatient = new()
            {
                PatientId = 12,
                FullName = request.FullName,
                Email = request.Email
            };

            IdentityError identityError = new()
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
                .ReturnsAsync(IdentityResult.Failed(identityError));

            // Act
            var result = await _authService.RegisterPatientAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Reference claim failed.", result.Message);
            Assert.Equal(string.Empty, result.UserId);
            Assert.Equal(createdPatient.PatientId, result.PatientId);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenValidRequest_ReturnsSuccess()
        {
            // Arrange
            RegisterPatientDto request = CreateRegisterPatientDto();

            Patient createdPatient = new()
            {
                PatientId = 100,
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
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
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.RegisterPatientAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Patient registered successfully.", result.Message);
            Assert.NotNull(result.UserId);
            Assert.Equal(createdPatient.PatientId, result.PatientId);

            _patientRepositoryMock.Verify(
                repository => repository.CreateAsync(
                    It.Is<Patient>(patient =>
                        patient.FullName == request.FullName &&
                        patient.Email == request.Email),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _userManagerMock.Verify(
                manager => manager.CreateAsync(
                    It.Is<IdentityUser>(user =>
                        user.UserName == request.Email &&
                        user.Email == request.Email),
                    request.Password),
                Times.Once);

            _userManagerMock.Verify(
                manager => manager.AddClaimAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<Claim>()),
                Times.Exactly(2));
        }

        [Fact]
        public async Task LoginAsync_WhenUserDoesNotExist_ReturnsFailure()
        {
            // Arrange
            LoginDto request = CreateLoginDto();

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
            Assert.Equal(string.Empty, result.UserId);
            Assert.Equal(string.Empty, result.Email);
            Assert.Equal(string.Empty, result.Role);
            Assert.Equal(0, result.ReferenceId);
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordIsInvalid_ReturnsFailure()
        {
            // Arrange
            LoginDto request = CreateLoginDto();

            IdentityUser user = new()
            {
                Id = "user-1",
                UserName = request.Email,
                Email = request.Email
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

            _userManagerMock.Verify(
                manager => manager.GetClaimsAsync(It.IsAny<IdentityUser>()),
                Times.Never);
        }

        [Fact]
        public async Task LoginAsync_WhenValidCredentials_ReturnsTokensAndUserDetails()
        {
            // Arrange
            LoginDto request = CreateLoginDto();

            IdentityUser user = new()
            {
                Id = "user-1",
                UserName = request.Email,
                Email = request.Email
            };

            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Patient"),
                new Claim("ReferenceId", "101")
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
                    "HealthAxis",
                    "RefreshToken",
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(manager => manager.SetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
                    "RefreshTokenExpiryTime",
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Login successful.", result.Message);
            Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));
            Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));
            Assert.Equal(30 * 60, result.ExpiresIn);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal("Patient", result.Role);
            Assert.Equal(101, result.ReferenceId);

            _userManagerMock.Verify(
                manager => manager.SetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
                    "RefreshToken",
                    It.IsAny<string>()),
                Times.Once);

            _userManagerMock.Verify(
                manager => manager.SetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
                    "RefreshTokenExpiryTime",
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WhenReferenceIdClaimIsInvalid_ReturnsReferenceIdAsZero()
        {
            // Arrange
            LoginDto request = CreateLoginDto();

            IdentityUser user = new()
            {
                Id = "user-2",
                UserName = request.Email,
                Email = request.Email
            };

            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Doctor"),
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
            Assert.Equal("Doctor", result.Role);
            Assert.Equal(0, result.ReferenceId);
        }

        [Fact]
        public async Task LoginAsync_WhenRoleAndReferenceClaimsAreMissing_ReturnsEmptyRoleAndZeroReferenceId()
        {
            // Arrange
            LoginDto request = CreateLoginDto();

            IdentityUser user = new()
            {
                Id = "user-3",
                UserName = request.Email,
                Email = request.Email
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.CheckPasswordAsync(user, request.Password))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(manager => manager.GetClaimsAsync(user))
                .ReturnsAsync(new List<Claim>());

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
            Assert.Equal(string.Empty, result.Role);
            Assert.Equal(0, result.ReferenceId);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenUserDoesNotExist_ReturnsFailure()
        {
            // Arrange
            RefreshTokenDto request = new()
            {
                UserId = "invalid-user-id",
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
            Assert.Equal(0, result.ExpiresIn);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenStoredRefreshTokenDoesNotMatch_ReturnsFailure()
        {
            // Arrange
            RefreshTokenDto request = new()
            {
                UserId = "user-1",
                RefreshToken = "request-refresh-token"
            };

            IdentityUser user = new()
            {
                Id = request.UserId,
                Email = "patient@test.com"
            };

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(request.UserId))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
                    "RefreshToken"))
                .ReturnsAsync("different-refresh-token");

            // Act
            var result = await _authService.RefreshTokenAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid refresh token.", result.Message);
            Assert.Equal(string.Empty, result.AccessToken);
            Assert.Equal(string.Empty, result.RefreshToken);
            Assert.Equal(0, result.ExpiresIn);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenExpiryValueIsMissing_ReturnsExpiredFailure()
        {
            // Arrange
            RefreshTokenDto request = new()
            {
                UserId = "user-1",
                RefreshToken = "valid-refresh-token"
            };

            IdentityUser user = new()
            {
                Id = request.UserId,
                Email = "patient@test.com"
            };

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(request.UserId))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
                    "RefreshToken"))
                .ReturnsAsync(request.RefreshToken);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
                    "RefreshTokenExpiryTime"))
                .ReturnsAsync((string?)null);

            // Act
            var result = await _authService.RefreshTokenAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Refresh token expired.", result.Message);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenExpiryValueIsInvalid_ReturnsExpiredFailure()
        {
            // Arrange
            RefreshTokenDto request = new()
            {
                UserId = "user-1",
                RefreshToken = "valid-refresh-token"
            };

            IdentityUser user = new()
            {
                Id = request.UserId,
                Email = "patient@test.com"
            };

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(request.UserId))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
                    "RefreshToken"))
                .ReturnsAsync(request.RefreshToken);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
                    "RefreshTokenExpiryTime"))
                .ReturnsAsync("invalid-date");

            // Act
            var result = await _authService.RefreshTokenAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Refresh token expired.", result.Message);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenRefreshTokenIsExpired_ReturnsExpiredFailure()
        {
            // Arrange
            RefreshTokenDto request = new()
            {
                UserId = "user-1",
                RefreshToken = "valid-refresh-token"
            };

            IdentityUser user = new()
            {
                Id = request.UserId,
                Email = "patient@test.com"
            };

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(request.UserId))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
                    "RefreshToken"))
                .ReturnsAsync(request.RefreshToken);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
                    "RefreshTokenExpiryTime"))
                .ReturnsAsync(DateTime.UtcNow.AddDays(-1).ToString("O"));

            // Act
            var result = await _authService.RefreshTokenAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Refresh token expired.", result.Message);
            Assert.Equal(string.Empty, result.AccessToken);
            Assert.Equal(string.Empty, result.RefreshToken);
            Assert.Equal(0, result.ExpiresIn);

            _userManagerMock.Verify(
                manager => manager.GetClaimsAsync(It.IsAny<IdentityUser>()),
                Times.Never);

            _userManagerMock.Verify(
                manager => manager.SetAuthenticationTokenAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenValidRefreshToken_ReturnsNewTokens()
        {
            // Arrange
            RefreshTokenDto request = new()
            {
                UserId = "user-1",
                RefreshToken = "valid-refresh-token"
            };

            IdentityUser user = new()
            {
                Id = request.UserId,
                UserName = "patient@test.com",
                Email = "patient@test.com"
            };

            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Patient"),
                new Claim("ReferenceId", "100")
            };

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(request.UserId))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
                    "RefreshToken"))
                .ReturnsAsync(request.RefreshToken);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
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
            Assert.Equal(30 * 60, result.ExpiresIn);
            Assert.NotEqual(request.RefreshToken, result.RefreshToken);

            _userManagerMock.Verify(
                manager => manager.SetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
                    "RefreshToken",
                    It.IsAny<string>()),
                Times.Once);

            _userManagerMock.Verify(
                manager => manager.SetAuthenticationTokenAsync(
                    user,
                    "HealthAxis",
                    "RefreshTokenExpiryTime",
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenUserDoesNotExist_ReturnsFailure()
        {
            // Arrange
            string userId = "invalid-user-id";
            ChangePasswordDto request = CreateChangePasswordDto();

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(userId))
                .ReturnsAsync((IdentityUser?)null);

            // Act
            var result = await _authService.ChangePasswordAsync(userId, request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User not found.", result.Message);

            _userManagerMock.Verify(
                manager => manager.ChangePasswordAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenNewPasswordAndConfirmPasswordDoNotMatch_ReturnsFailure()
        {
            // Arrange
            string userId = "user-1";

            IdentityUser user = new()
            {
                Id = userId,
                Email = "patient@test.com"
            };

            ChangePasswordDto request = CreateChangePasswordDto();
            request.ConfirmPassword = "Different@123";

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _authService.ChangePasswordAsync(userId, request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("New password and confirm password do not match.", result.Message);

            _userManagerMock.Verify(
                manager => manager.ChangePasswordAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenIdentityChangePasswordFails_ReturnsFailureWithErrorMessage()
        {
            // Arrange
            string userId = "user-1";

            IdentityUser user = new()
            {
                Id = userId,
                Email = "patient@test.com"
            };

            ChangePasswordDto request = CreateChangePasswordDto();

            IdentityError identityError = new()
            {
                Description = "Current password is incorrect."
            };

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.ChangePasswordAsync(
                    user,
                    request.CurrentPassword,
                    request.NewPassword))
                .ReturnsAsync(IdentityResult.Failed(identityError));

            // Act
            var result = await _authService.ChangePasswordAsync(userId, request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Current password is incorrect.", result.Message);
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenValidRequest_ReturnsSuccess()
        {
            // Arrange
            string userId = "user-1";

            IdentityUser user = new()
            {
                Id = userId,
                Email = "patient@test.com"
            };

            ChangePasswordDto request = CreateChangePasswordDto();

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.ChangePasswordAsync(
                    user,
                    request.CurrentPassword,
                    request.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.ChangePasswordAsync(userId, request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Password changed successfully.", result.Message);

            _userManagerMock.Verify(
                manager => manager.ChangePasswordAsync(
                    user,
                    request.CurrentPassword,
                    request.NewPassword),
                Times.Once);
        }

        private static RegisterPatientDto CreateRegisterPatientDto()
        {
            return new RegisterPatientDto
            {
                FullName = "Patient Test",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = default,
                PhoneNumber = "9999999999",
                Email = "patient@test.com",
                Password = "Patient@123",
                ConfirmPassword = "Patient@123"
            };
        }

        private static LoginDto CreateLoginDto()
        {
            return new LoginDto
            {
                Email = "patient@test.com",
                Password = "Patient@123"
            };
        }

        private static ChangePasswordDto CreateChangePasswordDto()
        {
            return new ChangePasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };
        }

        private static IConfiguration CreateConfiguration()
        {
            Dictionary<string, string?> settings = new()
            {
                { "Jwt:Key", "ThisIsASecretKeyForJwtTokenTesting1234567890" },
                { "Jwt:Issuer", "HealthAxisTestIssuer" },
                { "Jwt:Audience", "HealthAxisTestAudience" },
                { "Jwt:AccessTokenExpirationMinutes", "30" },
                { "Jwt:RefreshTokenDays", "7" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }

        private static Mock<UserManager<IdentityUser>> CreateUserManagerMock()
        {
            Mock<IUserStore<IdentityUser>> storeMock = new();

            return new Mock<UserManager<IdentityUser>>(
                storeMock.Object,
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
