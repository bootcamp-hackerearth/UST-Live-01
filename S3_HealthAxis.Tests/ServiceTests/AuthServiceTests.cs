using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using S3_HealthAxisApi.DTOs.Auth;
using S3_HealthAxisApi.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Implementation;
using Xunit;

namespace S3_HealthAxis.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly IConfiguration _configuration;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();

            _configuration = BuildConfiguration();

            _service = new AuthService(
                _userRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _configuration);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFailure_WhenPasswordsDoNotMatch()
        {
            // Arrange
            var request = new RegisterDto
            {
                Email = "admin@test.com",
                Password = "Password@123",
                ConfirmPassword = "DifferentPassword@123",
                Role = UserRole.Admin
            };

            // Act
            var result = await _service.RegisterAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Passwords do not match.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.EmailExistsAsync(It.IsAny<string>()), Times.Never);
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFailure_WhenEmailAlreadyExists()
        {
            // Arrange
            var request = new RegisterDto
            {
                Email = "admin@test.com",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                Role = UserRole.Admin
            };

            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync(request.Email))
                .ReturnsAsync(true);

            // Act
            var result = await _service.RegisterAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Email already exists.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.EmailExistsAsync(request.Email), Times.Once);
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_ShouldRegisterUserSuccessfully_WhenRequestIsValid()
        {
            // Arrange
            var request = new RegisterDto
            {
                Email = "  ADMIN@Test.com  ",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                Role = UserRole.Admin
            };

            User? capturedUser = null;

            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync(request.Email))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .Callback<User>(u =>
                {
                    capturedUser = u;
                    u.UserId = 101; // simulate DB-generated identity
                })
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.RegisterAsync(request);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("User registered successfully.");
            result.Data.Should().NotBeNull();

            capturedUser.Should().NotBeNull();
            capturedUser!.Email.Should().Be("admin@test.com");
            capturedUser.Role.Should().Be(UserRole.Admin);
            capturedUser.PasswordHash.Should().Be(ComputeSha256Base64(request.Password));
            capturedUser.RefreshToken.Should().NotBeNullOrWhiteSpace();
            capturedUser.RefreshTokenExpiryTime.Should().NotBeNull();
            capturedUser.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));

            result.Data!.Email.Should().Be("admin@test.com");
            result.Data.Role.Should().Be("Admin");
            result.Data.RefreshToken.Should().NotBeNullOrWhiteSpace();
            result.Data.AccessToken.Should().NotBeNullOrWhiteSpace();

            result.Data.RefreshToken.Should().Be(capturedUser.RefreshToken);

            AssertJwtContainsExpectedClaims(
                result.Data.AccessToken,
                expectedUserId: 101,
                expectedEmail: "admin@test.com",
                expectedRole: "Admin");

            _userRepositoryMock.Verify(x => x.EmailExistsAsync(request.Email), Times.Once);
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterPatientAsync_ShouldReturnFailure_WhenPasswordsDoNotMatch()
        {
            // Arrange
            var request = new RegisterPatientDto
            {
                FullName = "Rahul Sharma",
                DateOfBirth = new DateOnly(1995, 5, 20),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "rahul@test.com",
                InsuranceNumber = "INS-001",
                Password = "Password@123",
                ConfirmPassword = "Mismatch@123"
            };

            // Act
            var result = await _service.RegisterPatientAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Passwords do not match.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.EmailExistsAsync(It.IsAny<string>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_ShouldReturnFailure_WhenEmailAlreadyExists()
        {
            // Arrange
            var request = new RegisterPatientDto
            {
                FullName = "Rahul Sharma",
                DateOfBirth = new DateOnly(1995, 5, 20),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "rahul@test.com",
                InsuranceNumber = "INS-001",
                Password = "Password@123",
                ConfirmPassword = "Password@123"
            };

            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync(request.Email))
                .ReturnsAsync(true);

            // Act
            var result = await _service.RegisterPatientAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Email already exists.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.EmailExistsAsync(request.Email), Times.Once);
            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_ShouldRegisterPatientAndUserSuccessfully_WhenRequestIsValid()
        {
            // Arrange
            var request = new RegisterPatientDto
            {
                FullName = "  Rahul Sharma  ",
                DateOfBirth = new DateOnly(1995, 5, 20),
                Gender = Gender.Male,
                PhoneNumber = " 9999999999 ",
                Email = "  RAHUL@Test.com ",
                InsuranceNumber = "INS-001",
                Password = "Password@123",
                ConfirmPassword = "Password@123"
            };

            Patient? capturedPatient = null;
            User? capturedUser = null;

            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync(request.Email))
                .ReturnsAsync(false);

            _patientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Patient>()))
                .Callback<Patient>(p =>
                {
                    capturedPatient = p;
                    p.PatientId = 500; // simulate DB-generated identity
                })
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .Callback<User>(u =>
                {
                    capturedUser = u;
                    u.UserId = 600; // simulate DB-generated identity
                })
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.RegisterPatientAsync(request);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Patient registered successfully.");
            result.Data.Should().NotBeNull();

            capturedPatient.Should().NotBeNull();
            capturedPatient!.FullName.Should().Be("Rahul Sharma");
            capturedPatient.DateOfBirth.Should().Be(new DateOnly(1995, 5, 20));
            capturedPatient.Gender.Should().Be(Gender.Male);
            capturedPatient.PhoneNumber.Should().Be("9999999999");
            capturedPatient.Email.Should().Be("rahul@test.com");
            capturedPatient.InsuranceNumber.Should().Be("INS-001");
            capturedPatient.IsActive.Should().BeTrue();

            capturedUser.Should().NotBeNull();
            capturedUser!.Email.Should().Be("rahul@test.com");
            capturedUser.PasswordHash.Should().Be(ComputeSha256Base64(request.Password));
            capturedUser.Role.Should().Be(UserRole.Patient);
            capturedUser.ReferenceId.Should().Be(500);
            capturedUser.RefreshToken.Should().NotBeNullOrWhiteSpace();
            capturedUser.RefreshTokenExpiryTime.Should().NotBeNull();

            result.Data!.Email.Should().Be("rahul@test.com");
            result.Data.Role.Should().Be("Patient");
            result.Data.AccessToken.Should().NotBeNullOrWhiteSpace();
            result.Data.RefreshToken.Should().NotBeNullOrWhiteSpace();

            result.Data.RefreshToken.Should().Be(capturedUser.RefreshToken);

            AssertJwtContainsExpectedClaims(
                result.Data.AccessToken,
                expectedUserId: 600,
                expectedEmail: "rahul@test.com",
                expectedRole: "Patient");

            _userRepositoryMock.Verify(x => x.EmailExistsAsync(request.Email), Times.Once);
            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Once);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);

            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);

            // one save after adding user + one save after refresh token assignment
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Exactly(2));
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new LoginDto
            {
                Email = " missing@test.com ",
                Password = "Password@123"
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("missing@test.com"))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _service.LoginAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid email or password.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.GetByEmailAsync("missing@test.com"), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFailure_WhenPasswordIsIncorrect()
        {
            // Arrange
            var request = new LoginDto
            {
                Email = "admin@test.com",
                Password = "WrongPassword"
            };

            var user = new User
            {
                UserId = 1,
                Email = "admin@test.com",
                PasswordHash = ComputeSha256Base64("CorrectPassword"),
                Role = UserRole.Admin
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("admin@test.com"))
                .ReturnsAsync(user);

            // Act
            var result = await _service.LoginAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid email or password.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.GetByEmailAsync("admin@test.com"), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnSuccessAndUpdateRefreshToken_WhenCredentialsAreValid()
        {
            // Arrange
            var request = new LoginDto
            {
                Email = "  ADMIN@Test.com ",
                Password = "Password@123"
            };

            var user = new User
            {
                UserId = 99,
                Email = "admin@test.com",
                PasswordHash = ComputeSha256Base64("Password@123"),
                Role = UserRole.Admin
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("admin@test.com"))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(x => x.UpdateAsync(user))
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.LoginAsync(request);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Login successful.");
            result.Data.Should().NotBeNull();

            user.RefreshToken.Should().NotBeNullOrWhiteSpace();
            user.RefreshTokenExpiryTime.Should().NotBeNull();

            result.Data!.Email.Should().Be("admin@test.com");
            result.Data.Role.Should().Be("Admin");
            result.Data.AccessToken.Should().NotBeNullOrWhiteSpace();
            result.Data.RefreshToken.Should().NotBeNullOrWhiteSpace();

            result.Data.RefreshToken.Should().Be(user.RefreshToken);

            AssertJwtContainsExpectedClaims(
                result.Data.AccessToken,
                expectedUserId: 99,
                expectedEmail: "admin@test.com",
                expectedRole: "Admin");

            _userRepositoryMock.Verify(x => x.GetByEmailAsync("admin@test.com"), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnFailure_WhenRefreshTokenIsInvalid()
        {
            // Arrange
            var request = new RefreshTokenDto
            {
                RefreshToken = "invalid-refresh-token"
            };

            _userRepositoryMock
                .Setup(x => x.GetByRefreshTokenAsync(request.RefreshToken))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _service.RefreshTokenAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid refresh token.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.GetByRefreshTokenAsync(request.RefreshToken), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnFailure_WhenRefreshTokenHasExpired()
        {
            // Arrange
            var request = new RefreshTokenDto
            {
                RefreshToken = "expired-refresh-token"
            };

            var user = new User
            {
                UserId = 10,
                Email = "user@test.com",
                Role = UserRole.Patient,
                RefreshToken = "expired-refresh-token",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(-1)
            };

            _userRepositoryMock
                .Setup(x => x.GetByRefreshTokenAsync(request.RefreshToken))
                .ReturnsAsync(user);

            // Act
            var result = await _service.RefreshTokenAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Refresh token has expired.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.GetByRefreshTokenAsync(request.RefreshToken), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnFailure_WhenRefreshTokenExpiryIsNull()
        {
            // Arrange
            var request = new RefreshTokenDto
            {
                RefreshToken = "token-without-expiry"
            };

            var user = new User
            {
                UserId = 10,
                Email = "user@test.com",
                Role = UserRole.Patient,
                RefreshToken = "token-without-expiry",
                RefreshTokenExpiryTime = null
            };

            _userRepositoryMock
                .Setup(x => x.GetByRefreshTokenAsync(request.RefreshToken))
                .ReturnsAsync(user);

            // Act
            var result = await _service.RefreshTokenAsync(request);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Refresh token has expired.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.GetByRefreshTokenAsync(request.RefreshToken), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnSuccessAndRotateRefreshToken_WhenTokenIsValid()
        {
            // Arrange
            var request = new RefreshTokenDto
            {
                RefreshToken = "valid-refresh-token"
            };

            var user = new User
            {
                UserId = 77,
                Email = "doctor@test.com",
                Role = UserRole.Doctor,
                RefreshToken = "valid-refresh-token",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2)
            };

            var oldRefreshToken = user.RefreshToken;

            _userRepositoryMock
                .Setup(x => x.GetByRefreshTokenAsync(request.RefreshToken))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(x => x.UpdateAsync(user))
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.RefreshTokenAsync(request);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Token refreshed successfully.");
            result.Data.Should().NotBeNull();

            user.RefreshToken.Should().NotBeNullOrWhiteSpace();
            user.RefreshToken.Should().NotBe(oldRefreshToken);
            user.RefreshTokenExpiryTime.Should().NotBeNull();

            result.Data!.Email.Should().Be("doctor@test.com");
            result.Data.Role.Should().Be("Doctor");
            result.Data.AccessToken.Should().NotBeNullOrWhiteSpace();
            result.Data.RefreshToken.Should().NotBeNullOrWhiteSpace();
            result.Data.RefreshToken.Should().Be(user.RefreshToken);

            AssertJwtContainsExpectedClaims(
                result.Data.AccessToken,
                expectedUserId: 77,
                expectedEmail: "doctor@test.com",
                expectedRole: "Doctor");

            _userRepositoryMock.Verify(x => x.GetByRefreshTokenAsync(request.RefreshToken), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        private static IConfiguration BuildConfiguration()
        {
            var settings = new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "this-is-a-very-secure-test-key-for-jwt-12345",
                ["Jwt:Issuer"] = "S3HealthAxisTestIssuer",
                ["Jwt:Audience"] = "S3HealthAxisTestAudience",
                ["Jwt:AccessTokenExpirationMinutes"] = "60"
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }

        private static string ComputeSha256Base64(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private static void AssertJwtContainsExpectedClaims(
            string token,
            int expectedUserId,
            string expectedEmail,
            string expectedRole)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            jwt.Should().NotBeNull();

            jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value
                .Should().Be(expectedUserId.ToString());

            jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value
                .Should().Be(expectedEmail);

            jwt.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value
                .Should().Be(expectedUserId.ToString());

            jwt.Claims.First(x => x.Type == ClaimTypes.Role).Value
                .Should().Be(expectedRole);

            jwt.Claims.Should().Contain(x => x.Type == JwtRegisteredClaimNames.Jti);
        }
    }
}