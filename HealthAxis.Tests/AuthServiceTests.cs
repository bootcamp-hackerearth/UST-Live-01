using FluentAssertions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementation;
using HealthAxis.Shared.DTO.AuthDtos;
using HealthAxis.Shared.DTO.DoctorDtos;
using HealthAxis.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Timers;

namespace HealthAxis.API.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<IRepository<Patient>> _patientRepositoryMock;
        private readonly IConfiguration _configuration;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _userManagerMock = MockUserManager();
            _patientRepositoryMock = new Mock<IRepository<Patient>>();

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Jwt:Key", "ThisIsASecretKeyForHealthAxisTesting12345" },
                    { "Jwt:Issuer", "HealthAxisIssuer" },
                    { "Jwt:Audience", "HealthAxisAudience" },
                    { "Jwt:AccessTokenExpirationMinutes", "60" },
                    { "Jwt:RefreshTokenExpirationDays", "7" }
                })
                .Build();

            _service = new AuthService(
                _userManagerMock.Object,
                _configuration,
                _patientRepositoryMock.Object);
        }

        [Fact]
        public async Task Login_WhenUserNotFound_ReturnsInvalidCredentials()
        {
            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync("test@gmail.com"))
                .ReturnsAsync((IdentityUser?)null);

            var result = await _service.Login(new LoginDto
            {
                Email = "test@gmail.com",
                Password = "Password@123"
            });

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid credentials");
            result.AccessToken.Should().BeEmpty();
            result.RefreshToken.Should().BeEmpty();
        }

        [Fact]
        public async Task Login_WhenPasswordInvalid_ReturnsInvalidCredentials()
        {
            var user = CreateIdentityUser();

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(user.Email!))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.CheckPasswordAsync(user, "wrong"))
                .ReturnsAsync(false);

            var result = await _service.Login(new LoginDto
            {
                Email = user.Email!,
                Password = "wrong"
            });

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid credentials");
        }

        [Fact]
        public async Task Login_WhenValid_ReturnsAccessAndRefreshToken()
        {
            var user = CreateIdentityUser();

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(user.Email!))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.CheckPasswordAsync(user, "Password@123"))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(manager => manager.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Patient" });

            _userManagerMock
                .Setup(manager => manager.SetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.Login(new LoginDto
            {
                Email = user.Email!,
                Password = "Password@123"
            });

            result.Success.Should().BeTrue();
            result.Message.Should().Be("User logged in successfully");
            result.AccessToken.Should().NotBeNullOrWhiteSpace();
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();
            result.ExpiresIn.Should().Be(60);
        }

        [Fact]
        public async Task Register_WhenEmailAlreadyExists_ReturnsBadRequest()
        {
            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Patient>
                {
                    new()
                    {
                        Email = "test@gmail.com",
                        PhoneNumber = "9876543210"
                    }
                });

            var result = await _service.Register(CreateRegisterDto(email: "TEST@gmail.com"));

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Email already registered");
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Register_WhenPhoneAlreadyExists_ReturnsBadRequest()
        {
            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Patient>
                {
                    new()
                    {
                        Email = "other@gmail.com",
                        PhoneNumber = "9876543210"
                    }
                });

            var result = await _service.Register(CreateRegisterDto(phone: "9876543210"));

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Phone number already registered");
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Register_WhenPasswordMismatch_ReturnsBadRequest()
        {
            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            var dto = CreateRegisterDto();
            dto.ConfirmPassword = "Different@123";

            var result = await _service.Register(dto);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Password and Confirm Password do not match");
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Register_WhenIdentityCreateFails_ReturnsErrors()
        {
            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError { Description = "Password too weak" }));

            var result = await _service.Register(CreateRegisterDto());

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Password too weak");
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Register_WhenValid_CreatesUserAndPatient()
        {
            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    "Password@123"))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<IdentityUser, string>((user, password) =>
                {
                    user.Id = "identity-user-1";
                });

            _userManagerMock
                .Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<IdentityUser>(),
                    "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            _patientRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient patient, CancellationToken _) => patient);

            var result = await _service.Register(CreateRegisterDto());

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Patient registered successfully");
            result.StatusCode.Should().Be(200);
            result.UserId.Should().Be("identity-user-1");

            _patientRepositoryMock.Verify(repo => repo.AddAsync(
                It.Is<Patient>(patient =>
                    patient.Email == "test@gmail.com" &&
                    patient.UserId == "identity-user-1")), Times.Once);
        }

        [Fact]
        public async Task ChangePassword_WhenNewPasswordMismatch_ReturnsBadRequest()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = "Old@123",
                NewPassword = "New@123",
                ConfirmNewPassword = "Different@123"
            };

            var result = await _service.ChangePassword("user-1", dto);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("New password and confirm password do not match");
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task ChangePassword_WhenNewPasswordSameAsCurrent_ReturnsBadRequest()
        {
            var dto = new ChangePasswordDto
            {
                CurrentPassword = "Same@123",
                NewPassword = "Same@123",
                ConfirmNewPassword = "Same@123"
            };

            var result = await _service.ChangePassword("user-1", dto);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("New password cannot be same as current password");
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task ChangePassword_WhenUserNotFound_ReturnsNotFound()
        {
            _userManagerMock
                .Setup(manager => manager.FindByIdAsync("missing"))
                .ReturnsAsync((IdentityUser?)null);

            var result = await _service.ChangePassword("missing", CreateChangePasswordDto());

            result.Success.Should().BeFalse();
            result.Message.Should().Be("User not found");
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task ChangePassword_WhenIdentityFails_ReturnsBadRequest()
        {
            var user = CreateIdentityUser();

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.ChangePasswordAsync(
                    user,
                    "Old@123",
                    "New@123"))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError { Description = "Old password incorrect" }));

            var result = await _service.ChangePassword(user.Id, CreateChangePasswordDto());

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Old password incorrect");
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task ChangePassword_WhenValid_ReturnsSuccess()
        {
            var user = CreateIdentityUser();

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.ChangePasswordAsync(
                    user,
                    "Old@123",
                    "New@123"))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.ChangePassword(user.Id, CreateChangePasswordDto());

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Password changed successfully");
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task RefreshToken_WhenAccessTokenInvalid_ReturnsUnauthorized()
        {
            var result = await _service.RefreshToken(new RefreshTokenDto
            {
                AccessToken = "invalid-token",
                RefreshToken = "refresh-token"
            });

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid access token");
            result.StatusCode.Should().Be(401);
        }

        [Fact]
        public async Task RefreshToken_WhenUserNotFound_ReturnsNotFound()
        {
            var token = GenerateExpiredAccessToken("missing-user");

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync("missing-user"))
                .ReturnsAsync((IdentityUser?)null);

            var result = await _service.RefreshToken(new RefreshTokenDto
            {
                AccessToken = token,
                RefreshToken = "refresh-token"
            });

            result.Success.Should().BeFalse();
            result.Message.Should().Be("User not found");
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task RefreshToken_WhenRefreshTokenDoesNotMatch_ReturnsUnauthorized()
        {
            var user = CreateIdentityUser();
            var token = GenerateExpiredAccessToken(user.Id);

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(
                    user,
                    "HealthAxisAPI",
                    "RefreshToken"))
                .ReturnsAsync("saved-token");

            var result = await _service.RefreshToken(new RefreshTokenDto
            {
                AccessToken = token,
                RefreshToken = "different-token"
            });

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid refresh token");
            result.StatusCode.Should().Be(401);
        }

        [Fact]
        public async Task RefreshToken_WhenExpiryMissing_ReturnsUnauthorized()
        {
            var user = CreateIdentityUser();
            var token = GenerateExpiredAccessToken(user.Id);

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(user, "HealthAxisAPI", "RefreshToken"))
                .ReturnsAsync("refresh-token");

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(user, "HealthAxisAPI", "RefreshTokenExpiry"))
                .ReturnsAsync((string?)null);

            var result = await _service.RefreshToken(new RefreshTokenDto
            {
                AccessToken = token,
                RefreshToken = "refresh-token"
            });

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Refresh token expiry not found");
            result.StatusCode.Should().Be(401);
        }

        [Fact]
        public async Task RefreshToken_WhenExpiryInvalid_ReturnsUnauthorized()
        {
            var user = CreateIdentityUser();
            var token = GenerateExpiredAccessToken(user.Id);

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(user, "HealthAxisAPI", "RefreshToken"))
                .ReturnsAsync("refresh-token");

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(user, "HealthAxisAPI", "RefreshTokenExpiry"))
                .ReturnsAsync("invalid-date");

            var result = await _service.RefreshToken(new RefreshTokenDto
            {
                AccessToken = token,
                RefreshToken = "refresh-token"
            });

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Refresh token expired. Please login again");
            result.StatusCode.Should().Be(401);
        }
        [Fact]
        public async Task AdminResetPassword_WhenUserIdAndEmailMissing_ReturnsBadRequest()
        {
            var result = await _service.AdminResetPassword(
                new AdminResetPasswordDto
                {
                    NewPassword = "Pass@123",
                    ConfirmPassword = "Pass@123"
                });

            result.Success.Should().BeFalse();
            result.Message.Should().Be("User id or email is required.");
            result.StatusCode.Should().Be(400);
        }
        [Fact]
        public async Task AdminResetPassword_WhenPasswordMismatch_ReturnsBadRequest()
        {
            var result = await _service.AdminResetPassword(
                new AdminResetPasswordDto
                {
                    UserId = "user-1",
                    NewPassword = "Pass@123",
                    ConfirmPassword = "Other@123"
                });

            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(400);
        }
        [Fact]
        public async Task AdminResetPassword_WhenUserNotFound_Returns404()
        {
            _userManagerMock
                .Setup(x => x.FindByIdAsync("user-1"))
                .ReturnsAsync((IdentityUser?)null);

            var result = await _service.AdminResetPassword(
                new AdminResetPasswordDto
                {
                    UserId = "user-1",
                    NewPassword = "Pass@123",
                    ConfirmPassword = "Pass@123"
                });

            result.StatusCode.Should().Be(404);
        }
        [Fact]
        public async Task AdminResetPassword_WhenRoleNotAllowed_ReturnsForbidden()
        {
            var user = CreateIdentityUser();

            _userManagerMock
                .Setup(x => x.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Admin" });

            var result = await _service.AdminResetPassword(
                new AdminResetPasswordDto
                {
                    UserId = user.Id,
                    NewPassword = "Pass@123",
                    ConfirmPassword = "Pass@123"
                });

            result.StatusCode.Should().Be(403);
        }
        [Fact]
        public async Task AdminResetPassword_WhenIdentityFails_ReturnsBadRequest()
        {
            var user = CreateIdentityUser();

            _userManagerMock
                .Setup(x => x.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Patient" });

            _userManagerMock
                .Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync("token");

            _userManagerMock
                .Setup(x => x.ResetPasswordAsync(
                    user,
                    "token",
                    "Pass@123"))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        new IdentityError
                        {
                            Description = "Reset failed"
                        }));

            var result = await _service.AdminResetPassword(
                new AdminResetPasswordDto
                {
                    UserId = user.Id,
                    NewPassword = "Pass@123",
                    ConfirmPassword = "Pass@123"
                });

            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task RefreshToken_WhenExpired_ReturnsUnauthorized()
        {
            var user = CreateIdentityUser();
            var token = GenerateExpiredAccessToken(user.Id);

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(user, "HealthAxisAPI", "RefreshToken"))
                .ReturnsAsync("refresh-token");

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(user, "HealthAxisAPI", "RefreshTokenExpiry"))
                .ReturnsAsync(DateTime.UtcNow.AddDays(-1).ToString("O"));

            var result = await _service.RefreshToken(new RefreshTokenDto
            {
                AccessToken = token,
                RefreshToken = "refresh-token"
            });

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Refresh token expired. Please login again");
            result.StatusCode.Should().Be(401);
        }

        [Fact]
        public async Task RefreshToken_WhenValid_ReturnsNewTokens()
        {
            var user = CreateIdentityUser();
            var token = GenerateExpiredAccessToken(user.Id);

            _userManagerMock
                .Setup(manager => manager.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(user, "HealthAxisAPI", "RefreshToken"))
                .ReturnsAsync("refresh-token");

            _userManagerMock
                .Setup(manager => manager.GetAuthenticationTokenAsync(user, "HealthAxisAPI", "RefreshTokenExpiry"))
                .ReturnsAsync(DateTime.UtcNow.AddDays(1).ToString("O"));

            _userManagerMock
                .Setup(manager => manager.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Patient" });

            _userManagerMock
                .Setup(manager => manager.SetAuthenticationTokenAsync(
                    user,
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.RefreshToken(new RefreshTokenDto
            {
                AccessToken = token,
                RefreshToken = "refresh-token"
            });

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Token refreshed successfully");
            result.AccessToken.Should().NotBeNullOrWhiteSpace();
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();
            result.StatusCode.Should().Be(200);
        }

        private static Mock<UserManager<IdentityUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();

            return new Mock<UserManager<IdentityUser>>(
                store.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);
        }

        private static IdentityUser CreateIdentityUser()
        {
            return new IdentityUser
            {
                Id = "user-1",
                UserName = "test@gmail.com",
                Email = "test@gmail.com"
            };
        }

        private static RegisterDto CreateRegisterDto(
            string email = "test@gmail.com",
            string phone = "9876543210")
        {
            return new RegisterDto
            {
                FullName = "Mona",
                DateOfBirth = new DateTime(2004, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = phone,
                Email = email,
                Password = "Password@123",
                ConfirmPassword = "Password@123"
            };
        }

        private static ChangePasswordDto CreateChangePasswordDto()
        {
            return new ChangePasswordDto
            {
                CurrentPassword = "Old@123",
                NewPassword = "New@123",
                ConfirmNewPassword = "New@123"
            };
        }

        private string GenerateExpiredAccessToken(string userId)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, "test@gmail.com"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, "Patient")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(-5),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}