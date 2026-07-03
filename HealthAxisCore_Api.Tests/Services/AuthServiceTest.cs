using FluentAssertions;
using HealthAxis.Shared.DTOs.Auth;
using HealthAxis.Shared.DTOs.User;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.DTOs.User;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace HealthAxisCore_Api.Tests.Services
{
    public class AuthServiceTests : IDisposable
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly IConfiguration _configuration;
        private readonly HealthAppDbContext _context;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _userManagerMock = MockUserManager();
            _patientRepositoryMock = new Mock<IPatientRepository>();

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Jwt:Key", "ThisIsASecretKeyForJwtTesting1234567890" },
                    { "Jwt:Issuer", "HealthAxisTestIssuer" },
                    { "Jwt:Audience", "HealthAxisTestAudience" },
                    { "Jwt:DurationInMinutes", "60" }
                })
                .Build();

            var options = new DbContextOptionsBuilder<HealthAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new HealthAppDbContext(options);

            _service = new AuthService(
                _userManagerMock.Object,
                _configuration,
                _patientRepositoryMock.Object,
                _context
            );
        }

        // ------------------------------------------------------------
        // RegisterAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task RegisterAsync_WhenPasswordsDoNotMatch_ShouldThrowBusinessRuleException()
        {
            var request = new RegisterDto
            {
                Email = "patient@test.com",
                Password = "Password123!",
                ConfirmPassword = "Different123!",
                Role = "Patient",
                PatientName = "Test Patient",
                PhoneNumber = "9999999999",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Gender = GenderType.Female
            };

            var act = async () => await _service.RegisterAsync(request);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Passwords do not match");
        }

        [Fact]
        public async Task RegisterAsync_WhenRoleIsNotPatient_ShouldThrowBusinessRuleException()
        {
            var request = new RegisterDto
            {
                Email = "doctor@test.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Role = "Doctor",
                PatientName = "Test Doctor",
                PhoneNumber = "9999999999",
                DateOfBirth = DateTime.Today.AddYears(-30),
                Gender = GenderType.Male
            };

            var act = async () => await _service.RegisterAsync(request);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Only patients can register themselves");
        }

        [Fact]
        public async Task RegisterAsync_WhenUserAlreadyExists_ShouldThrowBusinessRuleException()
        {
            var request = new RegisterDto
            {
                Email = "existing@test.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Role = "Patient",
                PatientName = "Existing Patient",
                PhoneNumber = "9999999999",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Gender = GenderType.Female
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = request.Email,
                    UserName = request.Email
                });

            var act = async () => await _service.RegisterAsync(request);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("User already exists");
        }

        [Fact]
        public async Task RegisterAsync_WhenCreateUserFails_ShouldThrowBusinessRuleException()
        {
            var request = new RegisterDto
            {
                Email = "newpatient@test.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Role = "Patient",
                PatientName = "New Patient",
                PhoneNumber = "9999999999",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Gender = GenderType.Female
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _patientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Patient>()))
                .Callback<Patient>(patient => patient.PatientId = 101)
                .Returns(Task.CompletedTask);

            _userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Password is too weak"
                    }
                ));

            var act = async () => await _service.RegisterAsync(request);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Password is too weak");
        }

        [Fact]
        public async Task RegisterAsync_WhenValidRequest_ShouldRegisterPatientAndReturnAuthResponse()
        {
            var request = new RegisterDto
            {
                Email = "patient@test.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Role = "Patient",
                PatientName = "Test Patient",
                PhoneNumber = "9999999999",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Gender = GenderType.Female
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _patientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Patient>()))
                .Callback<Patient>(patient => patient.PatientId = 123)
                .Returns(Task.CompletedTask);

            _userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "Patient" });

            var result = await _service.RegisterAsync(request);

            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrWhiteSpace();
            result.Email.Should().Be(request.Email);
            result.Role.Should().Be("Patient");
            result.ReferenceId.Should().Be(123);
            result.IsFirstLogin.Should().BeFalse();

            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Once);
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Once);
            _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Patient"), Times.Once);
        }

        // ------------------------------------------------------------
        // LoginAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task LoginAsync_WhenUserDoesNotExist_ShouldThrowUnauthorizedException()
        {
            var request = new LoginDto
            {
                Email = "missing@test.com",
                Password = "Password123!"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            var act = async () => await _service.LoginAsync(request);

            await act.Should()
                .ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid email or password");
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordInvalid_ShouldThrowUnauthorizedException()
        {
            var request = new LoginDto
            {
                Email = "user@test.com",
                Password = "WrongPassword"
            };

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = request.Email,
                Role = "Patient",
                ReferenceId = 10,
                IsFirstLogin = false
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, request.Password))
                .ReturnsAsync(false);

            var act = async () => await _service.LoginAsync(request);

            await act.Should()
                .ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid email or password");
        }

        [Fact]
        public async Task LoginAsync_WhenValidCredentials_ShouldReturnAuthResponseAndStoreRefreshToken()
        {
            var request = new LoginDto
            {
                Email = "user@test.com",
                Password = "Password123!"
            };

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = request.Email,
                Role = "Doctor",
                ReferenceId = 55,
                IsFirstLogin = true
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, request.Password))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Doctor" });

            var result = await _service.LoginAsync(request);

            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrWhiteSpace();
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();
            result.Email.Should().Be(request.Email);
            result.Role.Should().Be("Doctor");
            result.ReferenceId.Should().Be(55);
            result.IsFirstLogin.Should().BeTrue();

            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == result.RefreshToken);

            storedToken.Should().NotBeNull();
            storedToken!.UserId.Should().Be(user.Id);
            storedToken.IsRevoked.Should().BeFalse();
            storedToken.Expires.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public async Task LoginAsync_WhenUserHasNoRole_ShouldReturnDefaultUserRole()
        {
            var request = new LoginDto
            {
                Email = "norole@test.com",
                Password = "Password123!"
            };

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = request.Email,
                ReferenceId = 88,
                IsFirstLogin = false
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.CheckPasswordAsync(user, request.Password))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var result = await _service.LoginAsync(request);

            result.Role.Should().Be("User");
            result.Email.Should().Be(request.Email);
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        }

        // ------------------------------------------------------------
        // RefreshTokenAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task RefreshTokenAsync_WhenTokenDoesNotExist_ShouldThrowUnauthorizedException()
        {
            var act = async () => await _service.RefreshTokenAsync("missing-token");

            await act.Should()
                .ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid or expired refresh token");
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenTokenIsExpired_ShouldThrowUnauthorizedException()
        {
            var user = CreateUser("expired@test.com", "Patient", 12);

            var token = new RefreshToken
            {
                Token = "expired-token",
                UserId = user.Id,
                User = user,
                Expires = DateTime.UtcNow.AddDays(-1),
                IsRevoked = false
            };

            _context.Users.Add(user);
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            var act = async () => await _service.RefreshTokenAsync(token.Token);

            await act.Should()
                .ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid or expired refresh token");
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenTokenIsRevoked_ShouldThrowUnauthorizedException()
        {
            var user = CreateUser("revoked@test.com", "Patient", 13);

            var token = new RefreshToken
            {
                Token = "revoked-token",
                UserId = user.Id,
                User = user,
                Expires = DateTime.UtcNow.AddDays(7),
                IsRevoked = true,
                Revoked = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            var act = async () => await _service.RefreshTokenAsync(token.Token);

            await act.Should()
                .ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid or expired refresh token");
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenTokenIsValid_ShouldReturnNewAuthResponse()
        {
            var user = CreateUser("validrefresh@test.com", "Patient", 25);

            var token = new RefreshToken
            {
                Token = "valid-refresh-token",
                UserId = user.Id,
                User = user,
                Expires = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.Users.Add(user);
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            _userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Patient" });

            var result = await _service.RefreshTokenAsync(token.Token);

            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrWhiteSpace();
            result.RefreshToken.Should().Be(token.Token);
            result.Email.Should().Be(user.Email);
            result.Role.Should().Be("Patient");
            result.ReferenceId.Should().Be(user.ReferenceId);
            result.IsFirstLogin.Should().Be(user.IsFirstLogin);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenUserHasNoRole_ShouldReturnDefaultUserRole()
        {
            var user = CreateUser("norolerefresh@test.com", "User", 99);

            var token = new RefreshToken
            {
                Token = "valid-refresh-no-role",
                UserId = user.Id,
                User = user,
                Expires = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.Users.Add(user);
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            _userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
                .ReturnsAsync(new List<string>());

            var result = await _service.RefreshTokenAsync(token.Token);

            result.Role.Should().Be("User");
            result.RefreshToken.Should().Be(token.Token);
        }

        // ------------------------------------------------------------
        // RevokeRefreshTokenAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task RevokeRefreshTokenAsync_WhenTokenDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            var act = async () => await _service.RevokeRefreshTokenAsync("missing-token");

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Token not found");
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_WhenTokenExists_ShouldRevokeToken()
        {
            var user = CreateUser("logout@test.com", "Patient", 50);

            var token = new RefreshToken
            {
                Token = "logout-refresh-token",
                UserId = user.Id,
                User = user,
                Expires = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.Users.Add(user);
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            await _service.RevokeRefreshTokenAsync(token.Token);

            var updatedToken = await _context.RefreshTokens
                .FirstAsync(x => x.Token == token.Token);

            updatedToken.IsRevoked.Should().BeTrue();
            updatedToken.Revoked.Should().NotBeNull();
            updatedToken.Revoked.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        // ------------------------------------------------------------
        // ChangePasswordAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task ChangePasswordAsync_WhenUserDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            var request = new ChangePasswordDto
            {
                Email = "missing@test.com",
                OldPassword = "OldPassword123!",
                NewPassword = "NewPassword123!"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            var act = async () => await _service.ChangePasswordAsync(request);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("User not found");
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenNewPasswordSameAsTemporaryPassword_ShouldThrowBusinessRuleException()
        {
            var request = new ChangePasswordDto
            {
                Email = "firstlogin@test.com",
                OldPassword = "Temp123!",
                NewPassword = "Temp123!"
            };

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = request.Email,
                TemporaryPassword = "Temp123!",
                IsFirstLogin = true
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            var act = async () => await _service.ChangePasswordAsync(request);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("New password cannot be same as temporary password");
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenIdentityChangePasswordFails_ShouldThrowBusinessRuleException()
        {
            var request = new ChangePasswordDto
            {
                Email = "changepass@test.com",
                OldPassword = "WrongOldPassword!",
                NewPassword = "NewPassword123!"
            };

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = request.Email,
                TemporaryPassword = "Temp123!",
                IsFirstLogin = true
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.ChangePasswordAsync(user, request.OldPassword, request.NewPassword))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Incorrect password"
                    }
                ));

            var act = async () => await _service.ChangePasswordAsync(request);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Incorrect password");
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenValidRequest_ShouldChangePasswordAndUpdateUser()
        {
            var request = new ChangePasswordDto
            {
                Email = "changepasssuccess@test.com",
                OldPassword = "Temp123!",
                NewPassword = "NewPassword123!"
            };

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = request.Email,
                TemporaryPassword = "Temp123!",
                IsFirstLogin = true
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(x => x.ChangePasswordAsync(user, request.OldPassword, request.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            await _service.ChangePasswordAsync(request);

            user.IsFirstLogin.Should().BeFalse();
            user.TemporaryPassword.Should().BeNull();

            _userManagerMock.Verify(
                x => x.ChangePasswordAsync(user, request.OldPassword, request.NewPassword),
                Times.Once
            );

            _userManagerMock.Verify(
                x => x.UpdateAsync(user),
                Times.Once
            );
        }

        // ------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------

        private static Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();

            return new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!
            );
        }

        private static ApplicationUser CreateUser(string email, string role, int referenceId)
        {
            return new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                UserName = email,
                Role = role,
                ReferenceId = referenceId,
                IsFirstLogin = false,
                TemporaryPassword = null
            };
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}