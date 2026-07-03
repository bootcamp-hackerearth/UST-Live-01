using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using S3_HealthAxis.Shared.DTOs.Auth;
using S3_HealthAxis.Shared.Enums;
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
            var request = new RegisterDto
            {
                Email = "admin@test.com",
                Password = "Password@123",
                ConfirmPassword = "DifferentPassword@123",
                Role = UserRole.Admin
            };

            var result = await _service.RegisterAsync(request);

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
            var request = new RegisterDto
            {
                Email = " ADMIN@Test.com ",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                Role = UserRole.Admin
            };

            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync("admin@test.com"))
                .ReturnsAsync(true);

            var result = await _service.RegisterAsync(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Email already exists.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.EmailExistsAsync("admin@test.com"), Times.Once);
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_ShouldRegisterUserSuccessfully_WhenRequestIsValid()
        {
            var request = new RegisterDto
            {
                Email = "  ADMIN@Test.com  ",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                Role = UserRole.Admin
            };

            User? capturedUser = null;

            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync("admin@test.com"))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .Callback<User>(u =>
                {
                    capturedUser = u;
                    u.UserId = 101;
                })
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await _service.RegisterAsync(request);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("User registered successfully.");
            result.Data.Should().NotBeNull();

            capturedUser.Should().NotBeNull();
            capturedUser!.Email.Should().Be("admin@test.com");
            capturedUser.Role.Should().Be(UserRole.Admin);
            capturedUser.PasswordHash.Should().Be(ComputeSha256Base64(request.Password));
            capturedUser.RefreshToken.Should().NotBeNullOrWhiteSpace();
            capturedUser.RefreshTokenExpiryTime.Should().NotBeNull();
            capturedUser.RefreshTokenExpiryTime.Should().BeAfter(DateTime.UtcNow);
            capturedUser.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
            capturedUser.MustChangePassword.Should().BeFalse();

            result.Data!.Email.Should().Be("admin@test.com");
            result.Data.Role.Should().Be("Admin");
            result.Data.RefreshToken.Should().Be(capturedUser.RefreshToken);
            result.Data.AccessToken.Should().NotBeNullOrWhiteSpace();
            result.Data.MustChangePassword.Should().BeFalse();

            AssertJwtContainsExpectedClaims(
                result.Data.AccessToken,
                expectedUserId: 101,
                expectedEmail: "admin@test.com",
                expectedRole: "Admin");

            _userRepositoryMock.Verify(x => x.EmailExistsAsync("admin@test.com"), Times.Once);
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterPatientAsync_ShouldReturnFailure_WhenPasswordsDoNotMatch()
        {
            var request = BuildRegisterPatientDto();
            request.ConfirmPassword = "Mismatch@123";

            var result = await _service.RegisterPatientAsync(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Passwords do not match.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.EmailExistsAsync(It.IsAny<string>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_ShouldReturnFailure_WhenEmailAlreadyExists()
        {
            var request = BuildRegisterPatientDto();
            request.Email = " RAHUL@Test.com ";

            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync("rahul@test.com"))
                .ReturnsAsync(true);

            var result = await _service.RegisterPatientAsync(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Email already exists.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.EmailExistsAsync("rahul@test.com"), Times.Once);
            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_ShouldRegisterPatientAndUserSuccessfully_WhenRequestIsValid()
        {
            var request = BuildRegisterPatientDto();
            request.FullName = "  Rahul Sharma  ";
            request.PhoneNumber = " 9999999999 ";
            request.Email = "  RAHUL@Test.com ";

            Patient? capturedPatient = null;
            User? capturedUser = null;

            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync("rahul@test.com"))
                .ReturnsAsync(false);

            _patientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Patient>()))
                .Callback<Patient>(p =>
                {
                    capturedPatient = p;
                    p.PatientId = 500;
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
                    u.UserId = 600;
                })
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await _service.RegisterPatientAsync(request);

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
            capturedUser.MustChangePassword.Should().BeFalse();

            result.Data!.Email.Should().Be("rahul@test.com");
            result.Data.Role.Should().Be("Patient");
            result.Data.ReferenceId.Should().Be(500);
            result.Data.AccessToken.Should().NotBeNullOrWhiteSpace();
            result.Data.RefreshToken.Should().Be(capturedUser.RefreshToken);
            result.Data.MustChangePassword.Should().BeFalse();

            AssertJwtContainsExpectedClaims(
                result.Data.AccessToken,
                expectedUserId: 600,
                expectedEmail: "rahul@test.com",
                expectedRole: "Patient");

            _userRepositoryMock.Verify(x => x.EmailExistsAsync("rahul@test.com"), Times.Once);
            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Once);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            var request = new LoginDto
            {
                Email = " missing@test.com ",
                Password = "Password@123"
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("missing@test.com"))
                .ReturnsAsync((User?)null);

            var result = await _service.LoginAsync(request);

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
            var request = new LoginDto
            {
                Email = "admin@test.com",
                Password = "WrongPassword"
            };

            var user = BuildUser(
                userId: 1,
                email: "admin@test.com",
                password: "CorrectPassword",
                role: UserRole.Admin,
                referenceId: null);

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("admin@test.com"))
                .ReturnsAsync(user);

            var result = await _service.LoginAsync(request);

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
            var request = new LoginDto
            {
                Email = "  ADMIN@Test.com ",
                Password = "Password@123"
            };

            var user = BuildUser(
                userId: 99,
                email: "admin@test.com",
                password: "Password@123",
                role: UserRole.Admin,
                referenceId: null);

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("admin@test.com"))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(x => x.UpdateAsync(user))
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await _service.LoginAsync(request);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Login successful.");
            result.Data.Should().NotBeNull();

            user.RefreshToken.Should().NotBeNullOrWhiteSpace();
            user.RefreshTokenExpiryTime.Should().NotBeNull();
            user.RefreshTokenExpiryTime.Should().BeAfter(DateTime.UtcNow);

            result.Data!.Email.Should().Be("admin@test.com");
            result.Data.Role.Should().Be("Admin");
            result.Data.AccessToken.Should().NotBeNullOrWhiteSpace();
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
        public async Task LoginAsync_ShouldReturnMustChangePassword_WhenDoctorRequiresPasswordChange()
        {
            var request = new LoginDto
            {
                Email = "doctor@test.com",
                Password = "Password@123"
            };

            var user = BuildUser(
                userId: 22,
                email: "doctor@test.com",
                password: "Password@123",
                role: UserRole.Doctor,
                referenceId: 10,
                mustChangePassword: true);

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("doctor@test.com"))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(x => x.UpdateAsync(user))
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await _service.LoginAsync(request);

            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Role.Should().Be("Doctor");
            result.Data.ReferenceId.Should().Be(10);
            result.Data.MustChangePassword.Should().BeTrue();

            AssertJwtContainsExpectedClaims(
                result.Data.AccessToken,
                expectedUserId: 22,
                expectedEmail: "doctor@test.com",
                expectedRole: "Doctor");
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnFailure_WhenRefreshTokenIsInvalid()
        {
            var request = new RefreshTokenDto
            {
                RefreshToken = "invalid-refresh-token"
            };

            _userRepositoryMock
                .Setup(x => x.GetByRefreshTokenAsync(request.RefreshToken))
                .ReturnsAsync((User?)null);

            var result = await _service.RefreshTokenAsync(request);

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
            var request = new RefreshTokenDto
            {
                RefreshToken = "expired-refresh-token"
            };

            var user = BuildUser(
                userId: 10,
                email: "user@test.com",
                password: "Password@123",
                role: UserRole.Patient,
                referenceId: 1);

            user.RefreshToken = "expired-refresh-token";
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(-1);

            _userRepositoryMock
                .Setup(x => x.GetByRefreshTokenAsync(request.RefreshToken))
                .ReturnsAsync(user);

            var result = await _service.RefreshTokenAsync(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Refresh token has expired.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnFailure_WhenRefreshTokenExpiryIsNull()
        {
            var request = new RefreshTokenDto
            {
                RefreshToken = "token-without-expiry"
            };

            var user = BuildUser(
                userId: 10,
                email: "user@test.com",
                password: "Password@123",
                role: UserRole.Patient,
                referenceId: 1);

            user.RefreshToken = "token-without-expiry";
            user.RefreshTokenExpiryTime = null;

            _userRepositoryMock
                .Setup(x => x.GetByRefreshTokenAsync(request.RefreshToken))
                .ReturnsAsync(user);

            var result = await _service.RefreshTokenAsync(request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Refresh token has expired.");
            result.Data.Should().BeNull();

            _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnSuccessAndRotateRefreshToken_WhenTokenIsValid()
        {
            var request = new RefreshTokenDto
            {
                RefreshToken = "valid-refresh-token"
            };

            var user = BuildUser(
                userId: 77,
                email: "doctor@test.com",
                password: "Password@123",
                role: UserRole.Doctor,
                referenceId: 44);

            user.RefreshToken = "valid-refresh-token";
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);

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

            var result = await _service.RefreshTokenAsync(request);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Token refreshed successfully.");
            result.Data.Should().NotBeNull();

            user.RefreshToken.Should().NotBeNullOrWhiteSpace();
            user.RefreshToken.Should().NotBe(oldRefreshToken);
            user.RefreshTokenExpiryTime.Should().NotBeNull();
            user.RefreshTokenExpiryTime.Should().BeAfter(DateTime.UtcNow);

            result.Data!.Email.Should().Be("doctor@test.com");
            result.Data.Role.Should().Be("Doctor");
            result.Data.ReferenceId.Should().Be(44);
            result.Data.AccessToken.Should().NotBeNullOrWhiteSpace();
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

        [Fact]
        public async Task ChangePasswordAsync_ShouldReturnFailure_WhenEmailIsMissing()
        {
            var result = await _service.ChangePasswordAsync("", BuildValidChangePasswordDto());

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid authenticated user.");
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldReturnFailure_WhenCurrentPasswordIsMissing()
        {
            var request = BuildValidChangePasswordDto();
            request.CurrentPassword = "";

            var result = await _service.ChangePasswordAsync("user@test.com", request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Current password is required.");
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldReturnFailure_WhenNewPasswordIsMissing()
        {
            var request = BuildValidChangePasswordDto();
            request.NewPassword = "";

            var result = await _service.ChangePasswordAsync("user@test.com", request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("New password is required.");
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldReturnFailure_WhenConfirmPasswordIsMissing()
        {
            var request = BuildValidChangePasswordDto();
            request.ConfirmNewPassword = "";

            var result = await _service.ChangePasswordAsync("user@test.com", request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Confirm password is required.");
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldReturnFailure_WhenNewAndConfirmPasswordDoNotMatch()
        {
            var request = BuildValidChangePasswordDto();
            request.ConfirmNewPassword = "Different@123";

            var result = await _service.ChangePasswordAsync("user@test.com", request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("New password and confirm password do not match.");
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldReturnFailure_WhenNewPasswordSameAsCurrentPassword()
        {
            var request = BuildValidChangePasswordDto();
            request.NewPassword = "OldPassword@123";
            request.ConfirmNewPassword = "OldPassword@123";

            var result = await _service.ChangePasswordAsync("user@test.com", request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("New password cannot be the same as current password.");
        }

        [Theory]
        [InlineData("Short1@", "Password must be at least 8 characters long.")]
        [InlineData("password@123", "Password must contain at least one uppercase letter.")]
        [InlineData("PASSWORD@123", "Password must contain at least one lowercase letter.")]
        [InlineData("Password@", "Password must contain at least one number.")]
        [InlineData("Password123", "Password must contain at least one special character.")]
        public async Task ChangePasswordAsync_ShouldReturnFailure_WhenPasswordStrengthInvalid(
            string newPassword,
            string expectedMessage)
        {
            var request = BuildValidChangePasswordDto();
            request.NewPassword = newPassword;
            request.ConfirmNewPassword = newPassword;

            var result = await _service.ChangePasswordAsync("user@test.com", request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be(expectedMessage);
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            var request = BuildValidChangePasswordDto();

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("user@test.com"))
                .ReturnsAsync((User?)null);

            var result = await _service.ChangePasswordAsync(" USER@Test.com ", request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("User account not found.");

            _userRepositoryMock.Verify(x => x.GetByEmailAsync("user@test.com"), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldReturnFailure_WhenCurrentPasswordIsIncorrect()
        {
            var request = BuildValidChangePasswordDto();

            var user = BuildUser(
                userId: 1,
                email: "user@test.com",
                password: "ActualOld@123",
                role: UserRole.Doctor,
                referenceId: 10,
                mustChangePassword: true);

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("user@test.com"))
                .ReturnsAsync(user);

            var result = await _service.ChangePasswordAsync("user@test.com", request);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Current password is incorrect.");

            _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldUpdatePassword_WhenRequestIsValid()
        {
            var request = BuildValidChangePasswordDto();

            var user = BuildUser(
                userId: 1,
                email: "user@test.com",
                password: "OldPassword@123",
                role: UserRole.Doctor,
                referenceId: 10,
                mustChangePassword: true);

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("user@test.com"))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(x => x.UpdateAsync(user))
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await _service.ChangePasswordAsync(" USER@Test.com ", request);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Password changed successfully.");

            user.PasswordHash.Should().Be(ComputeSha256Base64("NewPassword@123"));
            user.MustChangePassword.Should().BeFalse();

            _userRepositoryMock.Verify(x => x.GetByEmailAsync("user@test.com"), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        private static RegisterPatientDto BuildRegisterPatientDto()
        {
            return new RegisterPatientDto
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
        }

        private static ChangePasswordDto BuildValidChangePasswordDto()
        {
            return new ChangePasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmNewPassword = "NewPassword@123"
            };
        }

        private static User BuildUser(
            int userId,
            string email,
            string password,
            UserRole role,
            int? referenceId,
            bool mustChangePassword = false)
        {
            return new User
            {
                UserId = userId,
                Email = email,
                PasswordHash = ComputeSha256Base64(password),
                Role = role,
                ReferenceId = referenceId,
                CreatedDate = DateTime.UtcNow,
                MustChangePassword = mustChangePassword
            };
        }

        private static IConfiguration BuildConfiguration()
        {
            var settings = new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "this-is-a-very-secure-test-key-for-jwt-token-generation-123456789",
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
            jwt.Issuer.Should().Be("S3HealthAxisTestIssuer");
            jwt.Audiences.Should().Contain("S3HealthAxisTestAudience");

            jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value
                .Should().Be(expectedUserId.ToString());

            jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value
                .Should().Be(expectedEmail);

            jwt.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value
                .Should().Be(expectedUserId.ToString());

            jwt.Claims.First(x => x.Type == ClaimTypes.Role).Value
                .Should().Be(expectedRole);

            jwt.Claims.Should().Contain(x => x.Type == JwtRegisteredClaimNames.Jti);
            jwt.Claims.Should().Contain(x => x.Type == "ReferenceId");
            jwt.Claims.Should().Contain(x => x.Type == "MustChangePassword");
        }
    }
}
