using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Implementation;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace HealthAxisCore_Api.Tests.Services
{
    public class AdminHandoffServiceTests
    {
        private static AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private static IConfiguration CreateConfiguration()
        {
            var values = new Dictionary<string, string?>
            {
                ["Jwt:RefreshTokenExpiryDays"] = "7",
                ["Jwt:AccessTokenExpirationMinutes"] = "60"
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build();
        }

        private static Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
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
                Mock.Of<ILogger<UserManager<ApplicationUser>>>());
        }

        private static AdminHandoffService CreateService(
            AppDbContext context,
            Mock<UserManager<ApplicationUser>>? userManagerMock = null,
            Mock<IJwtService>? jwtServiceMock = null,
            IConfiguration? configuration = null)
        {
            return new AdminHandoffService(
                context,
                userManagerMock?.Object ?? CreateUserManagerMock().Object,
                jwtServiceMock?.Object ?? new Mock<IJwtService>().Object,
                configuration ?? CreateConfiguration());
        }

        private static ClaimsPrincipal CreateUserPrincipal(
            string? role = "Admin",
            string? userId = "admin-user-id")
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (!string.IsNullOrWhiteSpace(userId))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            }

            var identity = new ClaimsIdentity(claims, "TestAuth");

            return new ClaimsPrincipal(identity);
        }

        private static ApplicationUser CreateApplicationUser(
            string id = "admin-user-id",
            string email = "admin@test.com",
            bool isActive = true)
        {
            return new ApplicationUser
            {
                Id = id,
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                PhoneNumber = "9876543210",
                IsActive = isActive,
                RefreshTokens = new List<RefreshToken>()
            };
        }

        private static string HashCodeForTest(string rawCode)
        {
            var bytes = Encoding.UTF8.GetBytes(rawCode);

            var hashBytes = SHA256.HashData(bytes);

            return Convert.ToBase64String(hashBytes);
        }

        [Fact]
        public async Task CreateAsync_WhenUserIsNotAdmin_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var user = CreateUserPrincipal(
                role: "Doctor",
                userId: "doctor-user-id");

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.CreateAsync(user));

            Assert.Equal("Only admin can create handoff code", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenUserIdClaimMissing_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var user = CreateUserPrincipal(
                role: "Admin",
                userId: null);

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.CreateAsync(user));

            Assert.Equal("UserId claim missing", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenUserNotFound_ShouldThrowNotFoundException()
        {
            using var context = CreateDbContext();

            var user = CreateUserPrincipal(
                role: "Admin",
                userId: "missing-admin-id");

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(manager => manager.FindByIdAsync("missing-admin-id"))
                .ReturnsAsync((ApplicationUser?)null);

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.CreateAsync(user));

            Assert.Equal("User not found", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenUserIsInactive_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var appUser = CreateApplicationUser(
                id: "admin-user-id",
                isActive: false);

            var user = CreateUserPrincipal(
                role: "Admin",
                userId: appUser.Id);

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(manager => manager.FindByIdAsync(appUser.Id))
                .ReturnsAsync(appUser);

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.CreateAsync(user));

            Assert.Equal("User is inactive", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenUserDoesNotHaveAdminRoleInIdentity_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var appUser = CreateApplicationUser(
                id: "admin-user-id",
                isActive: true);

            var user = CreateUserPrincipal(
                role: "Admin",
                userId: appUser.Id);

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(manager => manager.FindByIdAsync(appUser.Id))
                .ReturnsAsync(appUser);

            userManagerMock
                .Setup(manager => manager.GetRolesAsync(appUser))
                .ReturnsAsync(new List<string> { "Doctor" });

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.CreateAsync(user));

            Assert.Equal("User is not admin", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenValidAdmin_ShouldCreateHandoffCodeAndReturnRawCode()
        {
            using var context = CreateDbContext();

            var appUser = CreateApplicationUser(
                id: "admin-user-id",
                isActive: true);

            var user = CreateUserPrincipal(
                role: "Admin",
                userId: appUser.Id);

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(manager => manager.FindByIdAsync(appUser.Id))
                .ReturnsAsync(appUser);

            userManagerMock
                .Setup(manager => manager.GetRolesAsync(appUser))
                .ReturnsAsync(new List<string> { "Admin" });

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var result = await service.CreateAsync(user);

            var savedCode = await context.AdminHandoffCodes.SingleAsync();

            Assert.False(string.IsNullOrWhiteSpace(result.Code));
            Assert.True(result.ExpiresAt > DateTime.UtcNow);
            Assert.Equal(appUser.Id, savedCode.UserId);
            Assert.False(savedCode.IsUsed);
            Assert.NotEqual(result.Code, savedCode.CodeHash);
            Assert.Equal(HashCodeForTest(result.Code), savedCode.CodeHash);

            userManagerMock.Verify(
                manager => manager.FindByIdAsync(appUser.Id),
                Times.Once);

            userManagerMock.Verify(
                manager => manager.GetRolesAsync(appUser),
                Times.Once);
        }

        [Fact]
        public async Task ExchangeAsync_WhenCodeDoesNotExist_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var request = new ExchangeAdminHandoffRequestDto
            {
                Code = "missing-code"
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.ExchangeAsync(request));

            Assert.Equal("Invalid or expired handoff code", exception.Message);
        }

        [Fact]
        public async Task ExchangeAsync_WhenCodeIsAlreadyUsed_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            const string rawCode = "used-code";

            var handoffCode = new AdminHandoffCode
            {
                UserId = "admin-user-id",
                CodeHash = HashCodeForTest(rawCode),
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = true
            };

            context.AdminHandoffCodes.Add(handoffCode);

            await context.SaveChangesAsync();

            var request = new ExchangeAdminHandoffRequestDto
            {
                Code = rawCode
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.ExchangeAsync(request));

            Assert.Equal("Invalid or expired handoff code", exception.Message);
        }

        [Fact]
        public async Task ExchangeAsync_WhenCodeIsExpired_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            const string rawCode = "expired-code";

            var handoffCode = new AdminHandoffCode
            {
                UserId = "admin-user-id",
                CodeHash = HashCodeForTest(rawCode),
                ExpiresAt = DateTime.UtcNow.AddSeconds(-10),
                IsUsed = false
            };

            context.AdminHandoffCodes.Add(handoffCode);

            await context.SaveChangesAsync();

            var request = new ExchangeAdminHandoffRequestDto
            {
                Code = rawCode
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.ExchangeAsync(request));

            Assert.Equal("Handoff code expired", exception.Message);
        }

        [Fact]
        public async Task ExchangeAsync_WhenUserNotFound_ShouldThrowNotFoundException()
        {
            using var context = CreateDbContext();

            const string rawCode = "valid-code-user-missing";

            var handoffCode = new AdminHandoffCode
            {
                UserId = "missing-user-id",
                CodeHash = HashCodeForTest(rawCode),
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            context.AdminHandoffCodes.Add(handoffCode);

            await context.SaveChangesAsync();

            var request = new ExchangeAdminHandoffRequestDto
            {
                Code = rawCode
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.ExchangeAsync(request));

            Assert.Equal("User not found", exception.Message);
            Assert.True(handoffCode.IsUsed);
        }

        [Fact]
        public async Task ExchangeAsync_WhenUserIsInactive_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            const string rawCode = "valid-code-inactive-user";

            var user = CreateApplicationUser(
                id: "admin-user-id",
                isActive: false);

            var handoffCode = new AdminHandoffCode
            {
                UserId = user.Id,
                CodeHash = HashCodeForTest(rawCode),
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            context.Users.Add(user);
            context.AdminHandoffCodes.Add(handoffCode);

            await context.SaveChangesAsync();

            var request = new ExchangeAdminHandoffRequestDto
            {
                Code = rawCode
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.ExchangeAsync(request));

            Assert.Equal("User is inactive", exception.Message);
            Assert.True(handoffCode.IsUsed);
        }

        [Fact]
        public async Task ExchangeAsync_WhenUserHasNoRole_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            const string rawCode = "valid-code-no-role";

            var user = CreateApplicationUser(
                id: "admin-user-id",
                isActive: true);

            var handoffCode = new AdminHandoffCode
            {
                UserId = user.Id,
                CodeHash = HashCodeForTest(rawCode),
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            context.Users.Add(user);
            context.AdminHandoffCodes.Add(handoffCode);

            await context.SaveChangesAsync();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(manager => manager.GetRolesAsync(
                    It.Is<ApplicationUser>(applicationUser => applicationUser.Id == user.Id)))
                .ReturnsAsync(new List<string>());

            var request = new ExchangeAdminHandoffRequestDto
            {
                Code = rawCode
            };

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.ExchangeAsync(request));

            Assert.Equal("User has no role", exception.Message);
            Assert.True(handoffCode.IsUsed);
        }

        [Fact]
        public async Task ExchangeAsync_WhenUserRoleIsNotAdmin_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            const string rawCode = "valid-code-doctor-role";

            var user = CreateApplicationUser(
                id: "admin-user-id",
                isActive: true);

            var handoffCode = new AdminHandoffCode
            {
                UserId = user.Id,
                CodeHash = HashCodeForTest(rawCode),
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            context.Users.Add(user);
            context.AdminHandoffCodes.Add(handoffCode);

            await context.SaveChangesAsync();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(manager => manager.GetRolesAsync(
                    It.Is<ApplicationUser>(applicationUser => applicationUser.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Doctor" });

            var request = new ExchangeAdminHandoffRequestDto
            {
                Code = rawCode
            };

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.ExchangeAsync(request));

            Assert.Equal("Only admin handoff is allowed", exception.Message);
            Assert.True(handoffCode.IsUsed);
        }

        [Fact]
        public async Task ExchangeAsync_WhenValidAdminCode_ShouldMarkCodeUsedAddRefreshTokenAndReturnAuthResponse()
        {
            using var context = CreateDbContext();

            const string rawCode = "valid-admin-code";

            var user = CreateApplicationUser(
                id: "admin-user-id",
                email: "admin@test.com",
                isActive: true);

            var handoffCode = new AdminHandoffCode
            {
                UserId = user.Id,
                CodeHash = HashCodeForTest(rawCode),
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            context.Users.Add(user);
            context.AdminHandoffCodes.Add(handoffCode);

            await context.SaveChangesAsync();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(manager => manager.GetRolesAsync(
                    It.Is<ApplicationUser>(applicationUser => applicationUser.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Admin" });

            var jwtServiceMock = new Mock<IJwtService>();

            jwtServiceMock
                .Setup(service => service.GenerateRefreshToken())
                .Returns("new-admin-refresh-token");

            jwtServiceMock
                .Setup(service => service.GenerateAccessTokenAsync(
                    It.Is<ApplicationUser>(applicationUser => applicationUser.Id == user.Id)))
                .ReturnsAsync("new-admin-access-token");

            var request = new ExchangeAdminHandoffRequestDto
            {
                Code = rawCode
            };

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                jwtServiceMock: jwtServiceMock);

            var result = await service.ExchangeAsync(request);

            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal("Admin", result.Role);
            Assert.Equal("new-admin-access-token", result.AccessToken);
            Assert.Equal("new-admin-refresh-token", result.RefreshToken);
            Assert.Equal(3600, result.ExpiresIn);
            Assert.True(handoffCode.IsUsed);

            var refreshToken = await context.RefreshTokens
                .SingleAsync(token => token.Token == "new-admin-refresh-token");

            Assert.Equal(user.Id, refreshToken.ApplicationUserId);
            Assert.False(refreshToken.IsRevoked);
            Assert.True(refreshToken.ExpiresAt > DateTime.UtcNow);

            jwtServiceMock.Verify(
                service => service.GenerateRefreshToken(),
                Times.Once);

            jwtServiceMock.Verify(
                service => service.GenerateAccessTokenAsync(
                    It.Is<ApplicationUser>(applicationUser => applicationUser.Id == user.Id)),
                Times.Once);
        }

        [Fact]
        public async Task ExchangeAsync_WhenValidAdminCodeAndUserNameIsNull_ShouldUseEmailAsFullName()
        {
            using var context = CreateDbContext();

            const string rawCode = "valid-admin-code-email-fallback";

            var user = CreateApplicationUser(
                id: "admin-user-id",
                email: "admin-email@test.com",
                isActive: true);

            user.UserName = null;

            var handoffCode = new AdminHandoffCode
            {
                UserId = user.Id,
                CodeHash = HashCodeForTest(rawCode),
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            context.Users.Add(user);
            context.AdminHandoffCodes.Add(handoffCode);

            await context.SaveChangesAsync();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(manager => manager.GetRolesAsync(
                    It.Is<ApplicationUser>(applicationUser => applicationUser.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Admin" });

            var jwtServiceMock = new Mock<IJwtService>();

            jwtServiceMock
                .Setup(service => service.GenerateRefreshToken())
                .Returns("refresh-token");

            jwtServiceMock
                .Setup(service => service.GenerateAccessTokenAsync(
                    It.Is<ApplicationUser>(applicationUser => applicationUser.Id == user.Id)))
                .ReturnsAsync("access-token");

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                jwtServiceMock: jwtServiceMock);

            var result = await service.ExchangeAsync(
                new ExchangeAdminHandoffRequestDto
                {
                    Code = rawCode
                });

            Assert.Equal("admin-email@test.com", result.FullName);
        }

        [Fact]
        public async Task ExchangeAsync_WhenValidAdminCodeAndUserNameAndEmailAreNull_ShouldUseSystemAdminAsFullName()
        {
            using var context = CreateDbContext();

            const string rawCode = "valid-admin-code-system-admin-fallback";

            var user = CreateApplicationUser(
                id: "admin-user-id",
                email: "admin@test.com",
                isActive: true);

            user.UserName = null;
            user.Email = null;

            var handoffCode = new AdminHandoffCode
            {
                UserId = user.Id,
                CodeHash = HashCodeForTest(rawCode),
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            context.Users.Add(user);
            context.AdminHandoffCodes.Add(handoffCode);

            await context.SaveChangesAsync();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(manager => manager.GetRolesAsync(
                    It.Is<ApplicationUser>(applicationUser => applicationUser.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Admin" });

            var jwtServiceMock = new Mock<IJwtService>();

            jwtServiceMock
                .Setup(service => service.GenerateRefreshToken())
                .Returns("refresh-token");

            jwtServiceMock
                .Setup(service => service.GenerateAccessTokenAsync(
                    It.Is<ApplicationUser>(applicationUser => applicationUser.Id == user.Id)))
                .ReturnsAsync("access-token");

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                jwtServiceMock: jwtServiceMock);

            var result = await service.ExchangeAsync(
                new ExchangeAdminHandoffRequestDto
                {
                    Code = rawCode
                });

            Assert.Equal("System Admin", result.FullName);
            Assert.Equal(string.Empty, result.Email);
        }
    }
}