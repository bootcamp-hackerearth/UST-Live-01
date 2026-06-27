using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthAxisCore_Api.Tests.Services
{
    public class JwtServiceTests
    {
        private static IConfiguration CreateConfiguration(
            string key = "491314c1ca342d46b2a38cd9b8140529a411d9ccbf9ca51b16d70fba7def2cd2",
            string issuer = "HealthAxisCore",
            string audience = "HealthAxisCore",
            string accessTokenExpirationMinutes = "60")
        {
            var values = new Dictionary<string, string?>
            {
                ["Jwt:Key"] = key,
                ["Jwt:Issuer"] = issuer,
                ["Jwt:Audience"] = audience,
                ["Jwt:AccessTokenExpirationMinutes"] = accessTokenExpirationMinutes
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

        private static ApplicationUser CreateUser(
            string id = "user-1",
            string email = "user@test.com",
            int? patientId = null,
            int? doctorId = null)
        {
            return new ApplicationUser
            {
                Id = id,
                UserName = email,
                Email = email,
                PatientId = patientId,
                DoctorId = doctorId,
                IsActive = true,
                EmailConfirmed = true
            };
        }

        private static JwtSecurityToken ReadJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            return handler.ReadJwtToken(token);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_WhenUserHasNoRolesAndNoPatientOrDoctor_ShouldCreateTokenWithBasicClaims()
        {
            var configuration = CreateConfiguration();

            var user = CreateUser(
                id: "user-basic",
                email: "basic@test.com");

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var service = new JwtService(
                configuration,
                userManagerMock.Object);

            var token = await service.GenerateAccessTokenAsync(user);

            var jwt = ReadJwtToken(token);

            Assert.False(string.IsNullOrWhiteSpace(token));
            Assert.Equal("HealthAxisCore", jwt.Issuer);
            Assert.Contains("HealthAxisCore", jwt.Audiences);

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == "UserId" && claim.Value == "user-basic");

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == ClaimTypes.NameIdentifier && claim.Value == "user-basic");

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == ClaimTypes.Email && claim.Value == "basic@test.com");

            Assert.DoesNotContain(
                jwt.Claims,
                claim => claim.Type == ClaimTypes.Role);

            Assert.DoesNotContain(
                jwt.Claims,
                claim => claim.Type == "Role");

            Assert.DoesNotContain(
                jwt.Claims,
                claim => claim.Type == "PatientId");

            Assert.DoesNotContain(
                jwt.Claims,
                claim => claim.Type == "DoctorId");

            userManagerMock.Verify(x => x.GetRolesAsync(user), Times.Once);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_WhenUserHasAdminRole_ShouldCreateTokenWithRoleClaims()
        {
            var configuration = CreateConfiguration();

            var user = CreateUser(
                id: "admin-user",
                email: "admin@test.com");

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Admin" });

            var service = new JwtService(
                configuration,
                userManagerMock.Object);

            var token = await service.GenerateAccessTokenAsync(user);

            var jwt = ReadJwtToken(token);

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == ClaimTypes.Role && claim.Value == "Admin");

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == "Role" && claim.Value == "Admin");

            userManagerMock.Verify(x => x.GetRolesAsync(user), Times.Once);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_WhenUserHasMultipleRoles_ShouldCreateAllRoleClaims()
        {
            var configuration = CreateConfiguration();

            var user = CreateUser(
                id: "multi-role-user",
                email: "multi@test.com");

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Admin", "Doctor" });

            var service = new JwtService(
                configuration,
                userManagerMock.Object);

            var token = await service.GenerateAccessTokenAsync(user);

            var jwt = ReadJwtToken(token);

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == ClaimTypes.Role && claim.Value == "Admin");

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == ClaimTypes.Role && claim.Value == "Doctor");

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == "Role" && claim.Value == "Admin");

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == "Role" && claim.Value == "Doctor");
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_WhenUserHasPatientId_ShouldCreatePatientIdClaim()
        {
            var configuration = CreateConfiguration();

            var user = CreateUser(
                id: "patient-user",
                email: "patient@test.com",
                patientId: 11);

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Patient" });

            var service = new JwtService(
                configuration,
                userManagerMock.Object);

            var token = await service.GenerateAccessTokenAsync(user);

            var jwt = ReadJwtToken(token);

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == "PatientId" && claim.Value == "11");

            Assert.DoesNotContain(
                jwt.Claims,
                claim => claim.Type == "DoctorId");
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_WhenUserHasDoctorId_ShouldCreateDoctorIdClaim()
        {
            var configuration = CreateConfiguration();

            var user = CreateUser(
                id: "doctor-user",
                email: "doctor@test.com",
                doctorId: 21);

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Doctor" });

            var service = new JwtService(
                configuration,
                userManagerMock.Object);

            var token = await service.GenerateAccessTokenAsync(user);

            var jwt = ReadJwtToken(token);

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == "DoctorId" && claim.Value == "21");

            Assert.DoesNotContain(
                jwt.Claims,
                claim => claim.Type == "PatientId");
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_WhenUserHasPatientIdAndDoctorId_ShouldCreateBothClaims()
        {
            var configuration = CreateConfiguration();

            var user = CreateUser(
                id: "mixed-user",
                email: "mixed@test.com",
                patientId: 15,
                doctorId: 25);

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Admin" });

            var service = new JwtService(
                configuration,
                userManagerMock.Object);

            var token = await service.GenerateAccessTokenAsync(user);

            var jwt = ReadJwtToken(token);

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == "PatientId" && claim.Value == "15");

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == "DoctorId" && claim.Value == "25");
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_WhenEmailIsNull_ShouldCreateEmailClaimWithEmptyValue()
        {
            var configuration = CreateConfiguration();

            var user = new ApplicationUser
            {
                Id = "no-email-user",
                UserName = "no-email-user",
                Email = null,
                IsActive = true,
                EmailConfirmed = true
            };

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var service = new JwtService(
                configuration,
                userManagerMock.Object);

            var token = await service.GenerateAccessTokenAsync(user);

            var jwt = ReadJwtToken(token);

            Assert.Contains(
                jwt.Claims,
                claim => claim.Type == ClaimTypes.Email && claim.Value == string.Empty);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_ShouldCreateTokenWithConfiguredIssuerAndAudience()
        {
            var configuration = CreateConfiguration(
                issuer: "CustomIssuer",
                audience: "CustomAudience");

            var user = CreateUser();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var service = new JwtService(
                configuration,
                userManagerMock.Object);

            var token = await service.GenerateAccessTokenAsync(user);

            var jwt = ReadJwtToken(token);

            Assert.Equal("CustomIssuer", jwt.Issuer);
            Assert.Contains("CustomAudience", jwt.Audiences);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_ShouldCreateTokenWithFutureExpiry()
        {
            var configuration = CreateConfiguration(
                accessTokenExpirationMinutes: "60");

            var user = CreateUser();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var beforeGeneration = DateTime.UtcNow;

            var service = new JwtService(
                configuration,
                userManagerMock.Object);

            var token = await service.GenerateAccessTokenAsync(user);

            var afterGeneration = DateTime.UtcNow;

            var jwt = ReadJwtToken(token);

            Assert.True(jwt.ValidTo > beforeGeneration.AddMinutes(59));
            Assert.True(jwt.ValidTo <= afterGeneration.AddMinutes(61));
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_ShouldCreateSignedJwtToken()
        {
            var key = "491314c1ca342d46b2a38cd9b8140529a411d9ccbf9ca51b16d70fba7def2cd2";

            var configuration = CreateConfiguration(key: key);

            var user = CreateUser();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Admin" });

            var service = new JwtService(
                configuration,
                userManagerMock.Object);

            var token = await service.GenerateAccessTokenAsync(user);

            var handler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "HealthAxisCore",
                ValidateAudience = true,
                ValidAudience = "HealthAxisCore",
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key)),
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.NameIdentifier
            };

            var principal = handler.ValidateToken(
                token,
                validationParameters,
                out var validatedToken);

            Assert.NotNull(principal);
            Assert.NotNull(validatedToken);
            Assert.True(principal.IsInRole("Admin"));
            Assert.Equal("user-1", principal.FindFirst("UserId")?.Value);
        }

        [Fact]
        public void GenerateRefreshToken_ShouldReturnNonEmptyString()
        {
            var configuration = CreateConfiguration();

            var userManagerMock = CreateUserManagerMock();

            var service = new JwtService(
                configuration,
                userManagerMock.Object);

            var token = service.GenerateRefreshToken();

            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public void GenerateRefreshToken_ShouldReturnValidBase64String()
        {
            var configuration = CreateConfiguration();

            var userManagerMock = CreateUserManagerMock();

            var service = new JwtService(
                configuration,
                userManagerMock.Object);

            var token = service.GenerateRefreshToken();

            var bytes = Convert.FromBase64String(token);

            Assert.NotNull(bytes);
            Assert.Equal(64, bytes.Length);
        }

        [Fact]
        public void GenerateRefreshToken_WhenCalledMultipleTimes_ShouldReturnDifferentValues()
        {
            var configuration = CreateConfiguration();

            var userManagerMock = CreateUserManagerMock();

            var service = new JwtService(
                configuration,
                userManagerMock.Object);

            var firstToken = service.GenerateRefreshToken();

            var secondToken = service.GenerateRefreshToken();

            Assert.NotEqual(firstToken, secondToken);
        }
    }
}