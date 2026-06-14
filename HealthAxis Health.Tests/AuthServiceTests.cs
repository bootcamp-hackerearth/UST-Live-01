using BCrypt.Net;
using HealthAxis_Health.API.DTOs.AuthDtos;
using HealthAxisHealth.API.DTOs.AuthDtos;
using HealthAxisHealth.API.Enums;
using HealthAxisHealth.API.Exceptions;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Repositories.Interfaces;
using HealthAxisHealth.API.Services.Implementations;
using HealthAxisHealth.API.UnitOfWork;
using Moq;
using Xunit;

namespace HealthAxisHealth.API.Tests.Services
{
    public class AuthServiceTests
    {
        #region Fields

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;

        private readonly AuthService _service;

        #endregion

        #region Constructor

        public AuthServiceTests()
        {
            _unitOfWorkMock =
                new Mock<IUnitOfWork>();

            _userRepositoryMock =
                new Mock<IUserRepository>();

            _patientRepositoryMock =
                new Mock<IPatientRepository>();

            _jwtTokenGeneratorMock =
                new Mock<IJwtTokenGenerator>();

            _unitOfWorkMock
                .Setup(x => x.Users)
                .Returns(_userRepositoryMock.Object);

            _unitOfWorkMock
                .Setup(x => x.Patients)
                .Returns(_patientRepositoryMock.Object);

            _jwtTokenGeneratorMock
                .Setup(x => x.GenerateToken(It.IsAny<User>()))
                .Returns("access-token");

            _service =
                new AuthService(
                    _unitOfWorkMock.Object,
                    _jwtTokenGeneratorMock.Object);
        }

        #endregion

        #region RegisterAsync

        [Fact]
        public async Task RegisterAsync_ShouldThrow_WhenEmailAlreadyExists()
        {
            // Arrange
            RegisterDto dto = new RegisterDto
            {
                Email = "test@test.com",
                Password = "Password@123",
                FullName = "John Doe"
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync(new User());

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.RegisterAsync(dto));
        }

        [Fact]
        public async Task RegisterAsync_ShouldCreatePatientAndReturnTokens()
        {
            // Arrange
            RegisterDto dto = new RegisterDto
            {
                Email = "test@test.com",
                Password = "Password@123",
                FullName = "John Doe",
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);

            _userRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => u.UserId = 1)
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask);

            // Act
            RegisterResponseDto result =
                await _service.RegisterAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Tokens);

            Assert.Equal(
                "access-token",
                result.Data.Tokens.AccessToken);

            Assert.False(
                string.IsNullOrWhiteSpace(
                    result.Data.Tokens.RefreshToken));

            _userRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Once);

            _patientRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Patient>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Exactly(3));
        }

        #endregion

        #region LoginAsync

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenUserDoesNotExist()
        {
            // Arrange
            LoginDto dto = new LoginDto
            {
                Email = "unknown@test.com",
                Password = "Password@123"
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.LoginAsync(dto));
        }

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenPasswordIsInvalid()
        {
            // Arrange
            LoginDto dto = new LoginDto
            {
                Email = "test@test.com",
                Password = "WrongPassword"
            };

            User user = new User
            {
                Email = dto.Email,
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword("CorrectPassword"),
                IsActive = true
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.LoginAsync(dto));
        }
        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenUserIsInactive()
        {
            // Arrange
            LoginDto dto = new LoginDto
            {
                Email = "test@test.com",
                Password = "Password@123"
            };

            User user = new User
            {
                Email = dto.Email,
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive = false
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.LoginAsync(dto));
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid()
        {
            // Arrange
            LoginDto dto = new LoginDto
            {
                Email = "test@test.com",
                Password = "Password@123"
            };

            User user = new User
            {
                UserId = 1,
                Email = dto.Email,
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive = true,
                Role = UserRole.Patient
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            // Act
            LoginResponseDto result =
                await _service.LoginAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(
                "access-token",
                result.AccessToken);

            Assert.False(
                string.IsNullOrWhiteSpace(
                    result.RefreshToken));

            _userRepositoryMock.Verify(
                x => x.Update(It.IsAny<User>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        #endregion

        #region RefreshTokenAsync

        [Fact]
        public async Task RefreshTokenAsync_ShouldThrow_WhenUserNotFound()
        {
            // Arrange
            RefreshTokenDto dto =
                new RefreshTokenDto
                {
                    RefreshToken = "invalid-token"
                };

            _userRepositoryMock
                .Setup(x =>
                    x.GetByRefreshTokenAsync(
                        dto.RefreshToken))
                .ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.RefreshTokenAsync(dto));
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldThrow_WhenRefreshTokenExpired()
        {
            // Arrange
            RefreshTokenDto dto =
                new RefreshTokenDto
                {
                    RefreshToken = "expired-token"
                };

            User user = new User
            {
                RefreshToken = dto.RefreshToken,
                RefreshTokenExpiryDate =
                    DateTime.UtcNow.AddMinutes(-5)
            };

            _userRepositoryMock
                .Setup(x =>
                    x.GetByRefreshTokenAsync(
                        dto.RefreshToken))
                .ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.RefreshTokenAsync(dto));
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnNewTokens()
        {
            // Arrange
            RefreshTokenDto dto =
                new RefreshTokenDto
                {
                    RefreshToken = "valid-token"
                };

            User user = new User
            {
                UserId = 1,
                Email = "test@test.com",
                Role = UserRole.Patient,
                RefreshToken = dto.RefreshToken,
                RefreshTokenExpiryDate =
                    DateTime.UtcNow.AddDays(1)
            };

            _userRepositoryMock
                .Setup(x =>
                    x.GetByRefreshTokenAsync(
                        dto.RefreshToken))
                .ReturnsAsync(user);

            // Act
            LoginResponseDto result =
                await _service.RefreshTokenAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(
                "access-token",
                result.AccessToken);

            Assert.False(
                string.IsNullOrWhiteSpace(
                    result.RefreshToken));

            _userRepositoryMock.Verify(
                x => x.Update(It.IsAny<User>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        #endregion
        #region LogoutAsync

        [Fact]
        public async Task LogoutAsync_ShouldThrow_WhenRefreshTokenIsInvalid()
        {
            // Arrange
            string refreshToken = "invalid-token";

            _userRepositoryMock
                .Setup(x => x.GetByRefreshTokenAsync(refreshToken))
                .ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.LogoutAsync(refreshToken));
        }

        [Fact]
        public async Task LogoutAsync_ShouldRevokeRefreshToken()
        {
            // Arrange
            string refreshToken = "valid-token";

            User user = new User
            {
                UserId = 1,
                Email = "test@test.com",
                RefreshToken = refreshToken,
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            _userRepositoryMock
                .Setup(x => x.GetByRefreshTokenAsync(refreshToken))
                .ReturnsAsync(user);

            // Act
            await _service.LogoutAsync(refreshToken);

            // Assert
            Assert.Null(user.RefreshToken);
            Assert.Null(user.RefreshTokenExpiryDate);

            _userRepositoryMock.Verify(
                x => x.Update(It.Is<User>(u =>
                    u.RefreshToken == null &&
                    u.RefreshTokenExpiryDate == null)),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        #endregion
    }
}
