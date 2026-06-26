using FluentAssertions;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.DTOs.User;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;
using HealthAxis.Shared.DTOs.Auth;

namespace HealthAxisCore_Api.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly IConfiguration _configuration;
        private readonly HealthAppDbContext _context;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userManagerMock = MockUserManager();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _configuration = GetConfiguration();

            // ✅ InMemory DB (Aligned with your DbContext)
            var options = new DbContextOptionsBuilder<HealthAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new HealthAppDbContext(options);

            // ✅ Pass DbContext to AuthService (FIX)
            _authService = new AuthService(
                _userManagerMock.Object,
                _configuration,
                _patientRepositoryMock.Object,
                _context
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

        // ========================
        // ✅ LOGIN SUCCESS (UPDATED)
        // ========================
        [Fact]
        public async Task LoginAsync_WhenValidCredentials_ShouldReturnTokens()
        {
            var request = new LoginDTO
            {
                Email = "doctor@test.com",
                Password = "Pass@123"
            };

            var user = new ApplicationUser
            {
                Id = "user-1",
                Email = request.Email,
                UserName = request.Email,
                Role = "Doctor"
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

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrWhiteSpace();
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();

            // ✅ Ensure token saved in DB
            _context.RefreshTokens.Count().Should().Be(1);
        }

        // ========================
        // ✅ LOGIN FAILURE
        // ========================
        [Fact]
        public async Task LoginAsync_WhenUserNotFound_ShouldThrowUnauthorized()
        {
            var request = new LoginDTO
            {
                Email = "missing@test.com",
                Password = "Pass@123"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            var act = async () => await _authService.LoginAsync(request);

            await act.Should().ThrowAsync<UnauthorizedException>();
        }

        // ========================
        // ✅ REFRESH TOKEN TEST
        // ========================
        [Fact]
        public async Task RefreshTokenAsync_WhenValidToken_ShouldReturnNewAccessToken()
        {
            var user = new ApplicationUser
            {
                Id = "user-1",
                Email = "user@test.com",
                UserName = "user@test.com"
            };

            var token = new RefreshToken
            {
                Token = "valid-token",
                UserId = user.Id,
                User = user,
                Expires = DateTime.UtcNow.AddDays(1)
            };

            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            _userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            var result = await _authService.RefreshTokenAsync("valid-token");

            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrWhiteSpace();
        }

        // ========================
        // ✅ LOGOUT TEST
        // ========================
        [Fact]
        public async Task RevokeRefreshTokenAsync_ShouldMarkTokenAsRevoked()
        {
            var token = new RefreshToken
            {
                Token = "test-token",
                UserId = "user-1",
                Expires = DateTime.UtcNow.AddDays(1)
            };

            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            await _authService.RevokeRefreshTokenAsync("test-token");

            var dbToken = await _context.RefreshTokens.FirstAsync();

            dbToken.IsRevoked.Should().BeTrue();
            dbToken.Revoked.Should().NotBeNull();
        }

        // ========================
        // ✅ CHANGE PASSWORD
        // ========================
        [Fact]
        public async Task ChangePasswordAsync_WhenUserNotFound_ShouldThrow()
        {
            var request = new ChangePasswordDTO
            {
                Email = "missing@test.com"
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            var act = async () => await _authService.ChangePasswordAsync(request);

            await act.Should().ThrowAsync<EntityNotFoundException>();
        }
    }
}
