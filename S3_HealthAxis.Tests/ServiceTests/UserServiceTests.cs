using FluentAssertions;
using Moq;
using S3_HealthAxis.Shared.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Implementation;
using Xunit;

namespace S3_HealthAxis.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _service = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = BuildUser(
                userId: 1,
                email: "admin@test.com",
                role: UserRole.Admin,
                referenceId: null);

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("admin@test.com"))
                .ReturnsAsync(user);

            // Act
            var result = await _service.GetByEmailAsync("admin@test.com");

            // Assert
            result.Should().NotBeNull();
            result!.UserId.Should().Be(1);
            result.Email.Should().Be("admin@test.com");
            result.Role.Should().Be(UserRole.Admin);
            result.ReferenceId.Should().BeNull();

            _userRepositoryMock.Verify(x => x.GetByEmailAsync("admin@test.com"), Times.Once);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("missing@test.com"))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _service.GetByEmailAsync("missing@test.com");

            // Assert
            result.Should().BeNull();

            _userRepositoryMock.Verify(x => x.GetByEmailAsync("missing@test.com"), Times.Once);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldPassEmailToRepositoryExactly()
        {
            // Arrange
            var email = " USER@Test.COM ";

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(email))
                .ReturnsAsync((User?)null);

            // Act
            await _service.GetByEmailAsync(email);

            // Assert
            _userRepositoryMock.Verify(x => x.GetByEmailAsync(email), Times.Once);
        }

        [Fact]
        public async Task GetByRefreshTokenAsync_ShouldReturnUser_WhenTokenExists()
        {
            // Arrange
            var user = BuildUser(
                userId: 2,
                email: "doctor@test.com",
                role: UserRole.Doctor,
                referenceId: 20);

            user.RefreshToken = "valid-refresh-token";
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            _userRepositoryMock
                .Setup(x => x.GetByRefreshTokenAsync("valid-refresh-token"))
                .ReturnsAsync(user);

            // Act
            var result = await _service.GetByRefreshTokenAsync("valid-refresh-token");

            // Assert
            result.Should().NotBeNull();
            result!.UserId.Should().Be(2);
            result.Email.Should().Be("doctor@test.com");
            result.Role.Should().Be(UserRole.Doctor);
            result.ReferenceId.Should().Be(20);
            result.RefreshToken.Should().Be("valid-refresh-token");
            result.RefreshTokenExpiryTime.Should().NotBeNull();

            _userRepositoryMock.Verify(x => x.GetByRefreshTokenAsync("valid-refresh-token"), Times.Once);
        }

        [Fact]
        public async Task GetByRefreshTokenAsync_ShouldReturnNull_WhenTokenDoesNotExist()
        {
            // Arrange
            _userRepositoryMock
                .Setup(x => x.GetByRefreshTokenAsync("invalid-token"))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _service.GetByRefreshTokenAsync("invalid-token");

            // Assert
            result.Should().BeNull();

            _userRepositoryMock.Verify(x => x.GetByRefreshTokenAsync("invalid-token"), Times.Once);
        }

        [Theory]
        [InlineData("admin@test.com", true)]
        [InlineData("missing@test.com", false)]
        public async Task EmailExistsAsync_ShouldReturnRepositoryResult(
            string email,
            bool repositoryResult)
        {
            // Arrange
            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync(email))
                .ReturnsAsync(repositoryResult);

            // Act
            var result = await _service.EmailExistsAsync(email);

            // Assert
            result.Should().Be(repositoryResult);

            _userRepositoryMock.Verify(x => x.EmailExistsAsync(email), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallRepositoryAddAsync()
        {
            // Arrange
            var user = BuildUser(
                userId: 0,
                email: "patient@test.com",
                role: UserRole.Patient,
                referenceId: 100);

            _userRepositoryMock
                .Setup(x => x.AddAsync(user))
                .Returns(Task.CompletedTask);

            // Act
            await _service.CreateAsync(user);

            // Assert
            _userRepositoryMock.Verify(x => x.AddAsync(user), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldPassSameUserInstanceToRepository()
        {
            // Arrange
            var user = BuildUser(
                userId: 0,
                email: "doctor@test.com",
                role: UserRole.Doctor,
                referenceId: 25);

            User? capturedUser = null;

            _userRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => capturedUser = u)
                .Returns(Task.CompletedTask);

            // Act
            await _service.CreateAsync(user);

            // Assert
            capturedUser.Should().BeSameAs(user);
            capturedUser!.Email.Should().Be("doctor@test.com");
            capturedUser.Role.Should().Be(UserRole.Doctor);
            capturedUser.ReferenceId.Should().Be(25);

            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryUpdateAsync()
        {
            // Arrange
            var user = BuildUser(
                userId: 5,
                email: "admin@test.com",
                role: UserRole.Admin,
                referenceId: null);

            user.RefreshToken = "new-refresh-token";
            user.MustChangePassword = false;

            _userRepositoryMock
                .Setup(x => x.UpdateAsync(user))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(user);

            // Assert
            _userRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldPassSameUserInstanceToRepository()
        {
            // Arrange
            var user = BuildUser(
                userId: 10,
                email: "patient@test.com",
                role: UserRole.Patient,
                referenceId: 200);

            User? capturedUser = null;

            _userRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .Callback<User>(u => capturedUser = u)
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(user);

            // Assert
            capturedUser.Should().BeSameAs(user);
            capturedUser!.UserId.Should().Be(10);
            capturedUser.Email.Should().Be("patient@test.com");
            capturedUser.Role.Should().Be(UserRole.Patient);
            capturedUser.ReferenceId.Should().Be(200);

            _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldCallRepositorySaveChangesAsync()
        {
            // Arrange
            _userRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.SaveChangesAsync();

            // Assert
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotCallUpdateOrSaveChanges()
        {
            // Arrange
            var user = BuildUser(
                userId: 0,
                email: "user@test.com",
                role: UserRole.Patient,
                referenceId: 1);

            _userRepositoryMock
                .Setup(x => x.AddAsync(user))
                .Returns(Task.CompletedTask);

            // Act
            await _service.CreateAsync(user);

            // Assert
            _userRepositoryMock.Verify(x => x.AddAsync(user), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotCallAddOrSaveChanges()
        {
            // Arrange
            var user = BuildUser(
                userId: 10,
                email: "user@test.com",
                role: UserRole.Doctor,
                referenceId: 99);

            _userRepositoryMock
                .Setup(x => x.UpdateAsync(user))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(user);

            // Assert
            _userRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task GetByEmailAsync_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync("error@test.com"))
                .ThrowsAsync(new InvalidOperationException("Repository failure."));

            // Act
            var act = async () => await _service.GetByEmailAsync("error@test.com");

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Repository failure.");
        }

        [Fact]
        public async Task EmailExistsAsync_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync("error@test.com"))
                .ThrowsAsync(new InvalidOperationException("Repository failure."));

            // Act
            var act = async () => await _service.EmailExistsAsync("error@test.com");

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Repository failure.");
        }

        [Fact]
        public async Task CreateAsync_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            var user = BuildUser(
                userId: 0,
                email: "user@test.com",
                role: UserRole.Patient,
                referenceId: 1);

            _userRepositoryMock
                .Setup(x => x.AddAsync(user))
                .ThrowsAsync(new InvalidOperationException("Create failed."));

            // Act
            var act = async () => await _service.CreateAsync(user);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Create failed.");
        }

        [Fact]
        public async Task UpdateAsync_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            var user = BuildUser(
                userId: 1,
                email: "user@test.com",
                role: UserRole.Doctor,
                referenceId: 10);

            _userRepositoryMock
                .Setup(x => x.UpdateAsync(user))
                .ThrowsAsync(new InvalidOperationException("Update failed."));

            // Act
            var act = async () => await _service.UpdateAsync(user);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Update failed.");
        }

        [Fact]
        public async Task SaveChangesAsync_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            _userRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .ThrowsAsync(new InvalidOperationException("Save failed."));

            // Act
            var act = async () => await _service.SaveChangesAsync();

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Save failed.");
        }

        private static User BuildUser(
            int userId,
            string email,
            UserRole role,
            int? referenceId)
        {
            return new User
            {
                UserId = userId,
                Email = email,
                PasswordHash = "hashed-password",
                Role = role,
                ReferenceId = referenceId,
                CreatedDate = DateTime.UtcNow,
                RefreshToken = null,
                RefreshTokenExpiryTime = null,
                MustChangePassword = false
            };
        }
    }
}
