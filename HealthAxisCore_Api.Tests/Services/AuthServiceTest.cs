using FluentAssertions;
using HealthAxisCore_Api.DTOs.User;
using HealthAxisCore_Api.Enums;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace HealthAxisCore_Api.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userManagerMock = MockUserManager();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _configuration = GetConfiguration();

            _authService = new AuthService(
                _userManagerMock.Object,
                _configuration,
                _patientRepositoryMock.Object
            );
        }

        private static Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();

            return new Mock<UserManager<ApplicationUser>>(
                store.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<ApplicationUser>>(),
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                Mock.Of<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<ApplicationUser>>>()
            );
        }

        private static IConfiguration GetConfiguration()
        {
            var settings = new Dictionary<string, string?>
            {
                { "Jwt:Key", "ThisIsASecretKeyForJWTAuthDontShare123!" },
                { "Jwt:Issuer", "HealthAxisApi" },
                { "Jwt:Audience", "HealthAxisUsers" },
                { "Jwt:DurationInMinutes", "60" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }

        [Fact]
        public async Task RegisterAsync_WhenPasswordsDoNotMatch_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var request = new RegisterDTO
            {
                Email = "patient@test.com",
                Password = "Password@123",
                ConfirmPassword = "WrongPassword@123",
                Role = "Patient",
                PatientName = "Ayushi",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = GenderType.Female,
                PhoneNumber = "9876543210"
            };

            // Act
            var act = async () => await _authService.RegisterAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Passwords do not match");
        }

        [Fact]
        public async Task RegisterAsync_WhenRoleIsNotPatient_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var request = new RegisterDTO
            {
                Email = "doctor@test.com",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                Role = "Doctor",
                PatientName = "Ayushi",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = GenderType.Female,
                PhoneNumber = "9876543210"
            };

            // Act
            var act = async () => await _authService.RegisterAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Only patients can register themselves");
        }

        [Fact]
        public async Task RegisterAsync_WhenUserAlreadyExists_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var request = new RegisterDTO
            {
                Email = "patient@test.com",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                Role = "Patient",
                PatientName = "Ayushi",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = GenderType.Female,
                PhoneNumber = "9876543210"
            };

            var existingUser = new ApplicationUser
            {
                Id = "existing-user-1",
                Email = request.Email,
                UserName = request.Email,
                Role = "Patient"
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(existingUser);

            // Act
            var act = async () => await _authService.RegisterAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("User already exists");

            _patientRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Patient>()),
                Times.Never
            );
        }

        [Fact]
        public async Task RegisterAsync_WhenIdentityCreateFails_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var request = new RegisterDTO
            {
                Email = "patient@test.com",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                Role = "Patient",
                PatientName = "Ayushi",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = GenderType.Female,
                PhoneNumber = "9876543210"
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _patientRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask)
                .Callback<Patient>(patient =>
                {
                    patient.PatientId = 1;
                });

            var identityError = new IdentityError
            {
                Description = "Password is too weak"
            };

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    request.Password))
                .ReturnsAsync(IdentityResult.Failed(identityError));

            // Act
            var act = async () => await _authService.RegisterAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Password is too weak");

            _userManagerMock.Verify(
                manager => manager.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                Times.Never
            );
        }

        [Fact]
        public async Task RegisterAsync_WhenValidPatient_ShouldRegisterPatientAndReturnAuthResponse()
        {
            // Arrange
            var request = new RegisterDTO
            {
                Email = "patient@test.com",
                Password = "Password@123",
                ConfirmPassword = "Password@123",
                Role = "Patient",
                PatientName = "Ayushi",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = GenderType.Female,
                PhoneNumber = "9876543210"
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _patientRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask)
                .Callback<Patient>(patient =>
                {
                    patient.PatientId = 1;
                });

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    request.Password))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<ApplicationUser, string>((user, password) =>
                {
                    user.Id = "patient-user-1";
                });

            _userManagerMock
                .Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(manager => manager.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "Patient" });

            // Act
            var result = await _authService.RegisterAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(request.Email);
            result.Role.Should().Be("Patient");
            result.ReferenceId.Should().Be(1);
            result.IsFirstLogin.Should().BeFalse();
            result.Token.Should().NotBeNullOrWhiteSpace();

            _patientRepositoryMock.Verify(
                repo => repo.AddAsync(It.Is<Patient>(patient =>
                    patient.PatientName == request.PatientName &&
                    patient.Email == request.Email &&
                    patient.PhoneNumber == request.PhoneNumber &&
                    patient.Gender == request.Gender
                )),
                Times.Once
            );

            _userManagerMock.Verify(
                manager => manager.CreateAsync(
                    It.Is<ApplicationUser>(user =>
                        user.Email == request.Email &&
                        user.UserName == request.Email &&
                        user.Role == "Patient" &&
                        user.ReferenceId == 1 &&
                        user.IsFirstLogin == false &&
                        user.TemporaryPassword == null
                    ),
                    request.Password),
                Times.Once
            );

            _userManagerMock.Verify(
                manager => manager.AddToRoleAsync(
                    It.Is<ApplicationUser>(user => user.Email == request.Email),
                    "Patient"),
                Times.Once
            );
        }

        [Fact]
        public async Task LoginAsync_WhenUserDoesNotExist_ShouldThrowUnauthorizedException()
        {
            // Arrange
            var request = new LoginDTO
            {
                Email = "missing@test.com",
                Password = "Password@123"
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var act = async () => await _authService.LoginAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid email or password");
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordIsInvalid_ShouldThrowUnauthorizedException()
        {
            // Arrange
            var request = new LoginDTO
            {
                Email = "patient@test.com",
                Password = "WrongPassword"
            };

            var user = new ApplicationUser
            {
                Id = "patient-user-1",
                Email = request.Email,
                UserName = request.Email,
                Role = "Patient",
                ReferenceId = 1,
                IsFirstLogin = false
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.CheckPasswordAsync(user, request.Password))
                .ReturnsAsync(false);

            // Act
            var act = async () => await _authService.LoginAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid email or password");
        }

        [Fact]
        public async Task LoginAsync_WhenCredentialsAreValid_ShouldReturnAuthResponse()
        {
            // Arrange
            var request = new LoginDTO
            {
                Email = "doctor@test.com",
                Password = "NewPass@123"
            };

            var user = new ApplicationUser
            {
                Id = "doctor-user-1",
                Email = request.Email,
                UserName = request.Email,
                Role = "Doctor",
                ReferenceId = 2,
                IsFirstLogin = false
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.CheckPasswordAsync(user, request.Password))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(manager => manager.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Doctor" });

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(request.Email);
            result.Role.Should().Be("Doctor");
            result.ReferenceId.Should().Be(2);
            result.IsFirstLogin.Should().BeFalse();
            result.Token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task LoginAsync_WhenDoctorFirstLogin_ShouldReturnIsFirstLoginTrue()
        {
            // Arrange
            var request = new LoginDTO
            {
                Email = "doctor@test.com",
                Password = "Temp@1234"
            };

            var user = new ApplicationUser
            {
                Id = "doctor-user-1",
                Email = request.Email,
                UserName = request.Email,
                Role = "Doctor",
                ReferenceId = 2,
                IsFirstLogin = true,
                TemporaryPassword = "Temp@1234"
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.CheckPasswordAsync(user, request.Password))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(manager => manager.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Doctor" });

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(request.Email);
            result.Role.Should().Be("Doctor");
            result.ReferenceId.Should().Be(2);
            result.IsFirstLogin.Should().BeTrue();
            result.Token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenUserDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            var request = new ChangePasswordDTO
            {
                Email = "missing@test.com",
                OldPassword = "OldPass@123",
                NewPassword = "NewPass@123"
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var act = async () => await _authService.ChangePasswordAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("User not found");
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenNewPasswordSameAsTemporaryPassword_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var request = new ChangePasswordDTO
            {
                Email = "doctor@test.com",
                OldPassword = "Temp@1234",
                NewPassword = "Temp@1234"
            };

            var user = new ApplicationUser
            {
                Id = "doctor-user-1",
                Email = request.Email,
                UserName = request.Email,
                Role = "Doctor",
                ReferenceId = 2,
                IsFirstLogin = true,
                TemporaryPassword = "Temp@1234"
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            // Act
            var act = async () => await _authService.ChangePasswordAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("New password cannot be same as temporary password");

            _userManagerMock.Verify(
                manager => manager.ChangePasswordAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenIdentityChangePasswordFails_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var request = new ChangePasswordDTO
            {
                Email = "doctor@test.com",
                OldPassword = "WrongOldPass@123",
                NewPassword = "NewPass@123"
            };

            var user = new ApplicationUser
            {
                Id = "doctor-user-1",
                Email = request.Email,
                UserName = request.Email,
                Role = "Doctor",
                ReferenceId = 2,
                IsFirstLogin = true,
                TemporaryPassword = "Temp@1234"
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            var error = new IdentityError
            {
                Description = "Incorrect password"
            };

            _userManagerMock
                .Setup(manager => manager.ChangePasswordAsync(
                    user,
                    request.OldPassword,
                    request.NewPassword))
                .ReturnsAsync(IdentityResult.Failed(error));

            // Act
            var act = async () => await _authService.ChangePasswordAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Incorrect password");

            _userManagerMock.Verify(
                manager => manager.UpdateAsync(It.IsAny<ApplicationUser>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenValidRequest_ShouldChangePasswordAndUpdateUser()
        {
            // Arrange
            var request = new ChangePasswordDTO
            {
                Email = "doctor@test.com",
                OldPassword = "Temp@1234",
                NewPassword = "NewPass@123"
            };

            var user = new ApplicationUser
            {
                Id = "doctor-user-1",
                Email = request.Email,
                UserName = request.Email,
                Role = "Doctor",
                ReferenceId = 2,
                IsFirstLogin = true,
                TemporaryPassword = "Temp@1234"
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(manager => manager.ChangePasswordAsync(
                    user,
                    request.OldPassword,
                    request.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(manager => manager.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _authService.ChangePasswordAsync(request);

            // Assert
            user.IsFirstLogin.Should().BeFalse();
            user.TemporaryPassword.Should().BeNull();

            _userManagerMock.Verify(
                manager => manager.UpdateAsync(user),
                Times.Once
            );
        }
    }
}
