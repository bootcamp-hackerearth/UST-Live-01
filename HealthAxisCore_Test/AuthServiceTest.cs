using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Implementation;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace HealthAxisCore_Api.Tests.Services
{
    public class AuthServiceTests
    {
        private static AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(warnings =>
                {
                    warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                })
                .Options;

            return new AppDbContext(options);
        }
        private static ClaimsPrincipal CreateClaimsPrincipal(
    string? userId = "user-1")
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            }

            var identity = new ClaimsIdentity(claims, "TestAuth");

            return new ClaimsPrincipal(identity);
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

        private static Mock<RoleManager<IdentityRole>> CreateRoleManagerMock()
        {
            var roleStore = new Mock<IRoleStore<IdentityRole>>();

            return new Mock<RoleManager<IdentityRole>>(
                roleStore.Object,
                Array.Empty<IRoleValidator<IdentityRole>>(),
                Mock.Of<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Mock.Of<ILogger<RoleManager<IdentityRole>>>());
        }

        private static Mock<SignInManager<ApplicationUser>> CreateSignInManagerMock(
            UserManager<ApplicationUser> userManager)
        {
            return new Mock<SignInManager<ApplicationUser>>(
                userManager,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<ILogger<SignInManager<ApplicationUser>>>(),
                Mock.Of<IAuthenticationSchemeProvider>(),
                Mock.Of<IUserConfirmation<ApplicationUser>>());
        }

        private static AuthService CreateService(
            AppDbContext context,
            Mock<UserManager<ApplicationUser>>? userManagerMock = null,
            Mock<RoleManager<IdentityRole>>? roleManagerMock = null,
            Mock<SignInManager<ApplicationUser>>? signInManagerMock = null,
            Mock<IJwtService>? jwtServiceMock = null,
            IConfiguration? configuration = null)
        {
            var userManager = userManagerMock ?? CreateUserManagerMock();

            return new AuthService(
                context,
                userManager.Object,
                roleManagerMock?.Object ?? CreateRoleManagerMock().Object,
                signInManagerMock?.Object ?? CreateSignInManagerMock(userManager.Object).Object,
                jwtServiceMock?.Object ?? new Mock<IJwtService>().Object,
                configuration ?? CreateConfiguration());
        }

        private static RegisterPatientDto CreateRegisterPatientDto(
            string email = "patient@test.com",
            string password = "Patient@123")
        {
            return new RegisterPatientDto
            {
                PatientName = "Patient One",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = "Male",
                Email = email,
                PhoneNumber = "9876543210",
                InsuranceID = "INS001",
                Password = password
            };
        }

        private static LoginDto CreateLoginDto(
            string email = "user@test.com",
            string password = "Password@123")
        {
            return new LoginDto
            {
                Email = email,
                Password = password
            };
        }

        private static Patient CreatePatient(
            int patientId = 1,
            bool isActive = true,
            string email = "patient@test.com")
        {
            return new Patient
            {
                PatientId = patientId,
                PatientName = "Patient One",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = "Male",
                Email = email,
                PhoneNumber = "9876543210",
                InsuranceID = "INS001",
                IsActive = isActive
            };
        }

        private static Doctor CreateDoctor(
            int doctorId = 1,
            bool isActive = true)
        {
            return new Doctor
            {
                DoctorId = doctorId,
                DoctorName = "Doctor One",
                Specialisation = "Cardiologist",
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = isActive
            };
        }

        private static ApplicationUser CreateApplicationUser(
            string id = "user-1",
            string email = "user@test.com",
            bool isActive = true)
        {
            return new ApplicationUser
            {
                Id = id,
                UserName = email,
                Email = email,
                PhoneNumber = "9876543210",
                EmailConfirmed = true,
                IsActive = isActive
            };
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenEmailAlreadyExists_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var request = CreateRegisterPatientDto();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(CreateApplicationUser(email: request.Email));

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.RegisterPatientAsync(request));

            Assert.Equal("Email already exists", exception.Message);

            userManagerMock.Verify(x => x.FindByEmailAsync(request.Email), Times.Once);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenPatientRoleDoesNotExist_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var request = CreateRegisterPatientDto();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            var roleManagerMock = CreateRoleManagerMock();

            roleManagerMock
                .Setup(x => x.RoleExistsAsync("Patient"))
                .ReturnsAsync(false);

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                roleManagerMock: roleManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.RegisterPatientAsync(request));

            Assert.Equal("Patient role does not exist", exception.Message);

            roleManagerMock.Verify(x => x.RoleExistsAsync("Patient"), Times.Once);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenCreateUserFails_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var request = CreateRegisterPatientDto(password: "weak");

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError { Description = "Password is invalid" }));

            var roleManagerMock = CreateRoleManagerMock();

            roleManagerMock
                .Setup(x => x.RoleExistsAsync("Patient"))
                .ReturnsAsync(true);

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                roleManagerMock: roleManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.RegisterPatientAsync(request));

            Assert.Contains("Password is invalid", exception.Message);

            userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Once);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenAddToRoleFails_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var request = CreateRegisterPatientDto();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .Callback<ApplicationUser, string>((user, _) =>
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                })
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Patient"))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError { Description = "Role assignment failed" }));

            var roleManagerMock = CreateRoleManagerMock();

            roleManagerMock
                .Setup(x => x.RoleExistsAsync("Patient"))
                .ReturnsAsync(true);

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                roleManagerMock: roleManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.RegisterPatientAsync(request));

            Assert.Contains("Role assignment failed", exception.Message);

            userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Patient"), Times.Once);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenValidRequest_ShouldCreatePatientUserRefreshTokenAndReturnResponse()
        {
            using var context = CreateDbContext();

            var request = CreateRegisterPatientDto();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .Callback<ApplicationUser, string>((user, _) =>
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                })
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            var roleManagerMock = CreateRoleManagerMock();

            roleManagerMock
                .Setup(x => x.RoleExistsAsync("Patient"))
                .ReturnsAsync(true);

            var jwtServiceMock = new Mock<IJwtService>();

            jwtServiceMock
                .Setup(x => x.GenerateRefreshToken())
                .Returns("refresh-token");

            jwtServiceMock
                .Setup(x => x.GenerateAccessTokenAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync("access-token");

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                roleManagerMock: roleManagerMock,
                jwtServiceMock: jwtServiceMock);

            var response = await service.RegisterPatientAsync(request);

            Assert.Equal("Patient", response.Role);
            Assert.Equal(request.Email, response.Email);
            Assert.Equal("access-token", response.AccessToken);
            Assert.Equal("refresh-token", response.RefreshToken);
            Assert.Equal(3600, response.ExpiresIn);

            Assert.Single(context.Patients);
            Assert.Single(context.RefreshTokens);

            userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Once);
            userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Patient"), Times.Once);
            jwtServiceMock.Verify(x => x.GenerateRefreshToken(), Times.Once);
            jwtServiceMock.Verify(x => x.GenerateAccessTokenAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WhenUserDoesNotExist_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var request = CreateLoginDto();

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.LoginAsync(request));

            Assert.Equal("Invalid email or password", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_WhenUserIsInactive_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                email: "inactive@test.com",
                isActive: false);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var request = CreateLoginDto(
                email: "inactive@test.com");

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.LoginAsync(request));

            Assert.Equal("User is inactive", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_WhenPatientIsInactive_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var patient = CreatePatient(
                patientId: 10,
                isActive: false,
                email: "patientinactive@test.com");

            var user = CreateApplicationUser(
                email: "patientinactive@test.com",
                isActive: true);

            user.PatientId = patient.PatientId;
            user.Patient = patient;

            context.Patients.Add(patient);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var request = CreateLoginDto(
                email: "patientinactive@test.com");

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.LoginAsync(request));

            Assert.Equal("Patient is inactive", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_WhenDoctorIsInactive_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var doctor = CreateDoctor(
                doctorId: 20,
                isActive: false);

            var user = CreateApplicationUser(
                email: "doctorinactive@test.com",
                isActive: true);

            user.DoctorId = doctor.DoctorId;
            user.Doctor = doctor;

            context.Doctors.Add(doctor);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var request = CreateLoginDto(
                email: "doctorinactive@test.com");

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.LoginAsync(request));

            Assert.Equal("Doctor is inactive", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordIsInvalid_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                email: "badpassword@test.com",
                isActive: true);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var request = CreateLoginDto(
                email: "badpassword@test.com",
                password: "WrongPassword@123");

            var userManagerMock = CreateUserManagerMock();

            var signInManagerMock = CreateSignInManagerMock(userManagerMock.Object);

            signInManagerMock
                .Setup(x => x.CheckPasswordSignInAsync(
                    user,
                    request.Password,
                    false))
                .ReturnsAsync(SignInResult.Failed);

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                signInManagerMock: signInManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.LoginAsync(request));

            Assert.Equal("Invalid email or password", exception.Message);

            signInManagerMock.Verify(x => x.CheckPasswordSignInAsync(user, request.Password, false), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WhenUserHasNoRole_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                email: "norole@test.com",
                isActive: true);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var request = CreateLoginDto(
                email: "norole@test.com");

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var signInManagerMock = CreateSignInManagerMock(userManagerMock.Object);

            signInManagerMock
                .Setup(x => x.CheckPasswordSignInAsync(user, request.Password, false))
                .ReturnsAsync(SignInResult.Success);

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                signInManagerMock: signInManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.LoginAsync(request));

            Assert.Equal("User has no role", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_WhenPatientLoginIsValid_ShouldReturnPatientResponse()
        {
            using var context = CreateDbContext();

            var patient = CreatePatient(
                patientId: 100,
                isActive: true,
                email: "patientlogin@test.com");

            var user = CreateApplicationUser(
                email: "patientlogin@test.com",
                isActive: true);

            user.PatientId = patient.PatientId;
            user.Patient = patient;

            context.Patients.Add(patient);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var request = CreateLoginDto(
                email: "patientlogin@test.com");

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Email == request.Email)))
                .ReturnsAsync(new List<string> { "Patient" });

            var signInManagerMock = CreateSignInManagerMock(userManagerMock.Object);

            signInManagerMock
                .Setup(x => x.CheckPasswordSignInAsync(
                    It.Is<ApplicationUser>(u => u.Email == request.Email),
                    request.Password,
                    false))
                .ReturnsAsync(SignInResult.Success);

            var jwtServiceMock = new Mock<IJwtService>();

            jwtServiceMock
                .Setup(x => x.GenerateRefreshToken())
                .Returns("patient-refresh-token");

            jwtServiceMock
                .Setup(x => x.GenerateAccessTokenAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync("patient-access-token");

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                signInManagerMock: signInManagerMock,
                jwtServiceMock: jwtServiceMock);

            var response = await service.LoginAsync(request);

            Assert.Equal("Patient", response.Role);
            Assert.Equal(patient.PatientName, response.FullName);
            Assert.Equal(patient.PatientId, response.PatientId);
            Assert.Equal("patient-access-token", response.AccessToken);
            Assert.Equal("patient-refresh-token", response.RefreshToken);

            Assert.Single(context.RefreshTokens);
        }

        [Fact]
        public async Task LoginAsync_WhenDoctorLoginIsValid_ShouldReturnDoctorResponse()
        {
            using var context = CreateDbContext();

            var doctor = CreateDoctor(
                doctorId: 200,
                isActive: true);

            var user = CreateApplicationUser(
                email: "doctorlogin@test.com",
                isActive: true);

            user.DoctorId = doctor.DoctorId;
            user.Doctor = doctor;

            context.Doctors.Add(doctor);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var request = CreateLoginDto(
                email: "doctorlogin@test.com");

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Email == request.Email)))
                .ReturnsAsync(new List<string> { "Doctor" });

            var signInManagerMock = CreateSignInManagerMock(userManagerMock.Object);

            signInManagerMock
                .Setup(x => x.CheckPasswordSignInAsync(
                    It.Is<ApplicationUser>(u => u.Email == request.Email),
                    request.Password,
                    false))
                .ReturnsAsync(SignInResult.Success);

            var jwtServiceMock = new Mock<IJwtService>();

            jwtServiceMock
                .Setup(x => x.GenerateRefreshToken())
                .Returns("doctor-refresh-token");

            jwtServiceMock
                .Setup(x => x.GenerateAccessTokenAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync("doctor-access-token");

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                signInManagerMock: signInManagerMock,
                jwtServiceMock: jwtServiceMock);

            var response = await service.LoginAsync(request);

            Assert.Equal("Doctor", response.Role);
            Assert.Equal(doctor.DoctorName, response.FullName);
            Assert.Equal(doctor.DoctorId, response.DoctorId);
            Assert.Equal("doctor-access-token", response.AccessToken);
            Assert.Equal("doctor-refresh-token", response.RefreshToken);

            Assert.Single(context.RefreshTokens);
        }

        [Fact]
        public async Task LoginAsync_WhenAdminLoginIsValid_ShouldReturnAdminResponse()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                email: "admin@test.com",
                isActive: true);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var request = CreateLoginDto(
                email: "admin@test.com");

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Email == request.Email)))
                .ReturnsAsync(new List<string> { "Admin" });

            var signInManagerMock = CreateSignInManagerMock(userManagerMock.Object);

            signInManagerMock
                .Setup(x => x.CheckPasswordSignInAsync(
                    It.Is<ApplicationUser>(u => u.Email == request.Email),
                    request.Password,
                    false))
                .ReturnsAsync(SignInResult.Success);

            var jwtServiceMock = new Mock<IJwtService>();

            jwtServiceMock
                .Setup(x => x.GenerateRefreshToken())
                .Returns("admin-refresh-token");

            jwtServiceMock
                .Setup(x => x.GenerateAccessTokenAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync("admin-access-token");

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                signInManagerMock: signInManagerMock,
                jwtServiceMock: jwtServiceMock);

            var response = await service.LoginAsync(request);

            Assert.Equal("Admin", response.Role);
            Assert.Equal("System Admin", response.FullName);
            Assert.Null(response.PatientId);
            Assert.Null(response.DoctorId);
            Assert.Equal("admin-access-token", response.AccessToken);
            Assert.Equal("admin-refresh-token", response.RefreshToken);

            Assert.Single(context.RefreshTokens);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenUserDoesNotExist_ShouldThrowNotFoundException()
        {
            using var context = CreateDbContext();

            var request = new RefreshTokenRequestDto
            {
                UserId = "missing-user",
                RefreshToken = "refresh-token"
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.RefreshTokenAsync(request));

            Assert.Equal("User not found", exception.Message);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenTokenIsMissing_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser();

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var request = new RefreshTokenRequestDto
            {
                UserId = user.Id,
                RefreshToken = "missing-token"
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.RefreshTokenAsync(request));

            Assert.Equal("Invalid refresh token", exception.Message);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenTokenIsRevoked_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser();

            var refreshToken = new RefreshToken
            {
                Token = "revoked-token",
                ApplicationUserId = user.Id,
                ApplicationUser = user,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = true
            };

            user.RefreshTokens.Add(refreshToken);

            context.Users.Add(user);
            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();

            var request = new RefreshTokenRequestDto
            {
                UserId = user.Id,
                RefreshToken = "revoked-token"
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.RefreshTokenAsync(request));

            Assert.Equal("Invalid refresh token", exception.Message);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenTokenIsExpired_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser();

            var refreshToken = new RefreshToken
            {
                Token = "expired-token",
                ApplicationUserId = user.Id,
                ApplicationUser = user,
                ExpiresAt = DateTime.UtcNow.AddDays(-1),
                IsRevoked = false
            };

            user.RefreshTokens.Add(refreshToken);

            context.Users.Add(user);
            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();

            var request = new RefreshTokenRequestDto
            {
                UserId = user.Id,
                RefreshToken = "expired-token"
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.RefreshTokenAsync(request));

            Assert.Equal("Invalid refresh token", exception.Message);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenUserHasNoRole_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser();

            var refreshToken = new RefreshToken
            {
                Token = "valid-token",
                ApplicationUserId = user.Id,
                ApplicationUser = user,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false
            };

            user.RefreshTokens.Add(refreshToken);

            context.Users.Add(user);
            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();

            var request = new RefreshTokenRequestDto
            {
                UserId = user.Id,
                RefreshToken = "valid-token"
            };

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
                .ReturnsAsync(new List<string>());

            var jwtServiceMock = new Mock<IJwtService>();
            jwtServiceMock
                .Setup(x => x.GenerateRefreshToken())
                .Returns("new-refresh-token");

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                jwtServiceMock: jwtServiceMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.RefreshTokenAsync(request));

            Assert.Equal("User has no role", exception.Message);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenValidToken_ShouldRevokeOldTokenAddNewTokenAndReturnResponse()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                id: "refresh-user",
                email: "refresh@test.com");

            var refreshToken = new RefreshToken
            {
                Token = "old-valid-token",
                ApplicationUserId = user.Id,
                ApplicationUser = user,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false
            };

            user.RefreshTokens.Add(refreshToken);

            context.Users.Add(user);
            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();

            var request = new RefreshTokenRequestDto
            {
                UserId = user.Id,
                RefreshToken = "old-valid-token"
            };

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Admin" });

            var jwtServiceMock = new Mock<IJwtService>();

            jwtServiceMock
                .Setup(x => x.GenerateRefreshToken())
                .Returns("new-refresh-token");

            jwtServiceMock
                .Setup(x => x.GenerateAccessTokenAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
                .ReturnsAsync("new-access-token");

            var service = CreateService(
                context,
                userManagerMock: userManagerMock,
                jwtServiceMock: jwtServiceMock);

            var response = await service.RefreshTokenAsync(request);

            Assert.Equal("Admin", response.Role);
            Assert.Equal("new-refresh-token", response.RefreshToken);
            Assert.Equal("new-access-token", response.AccessToken);

            Assert.True(refreshToken.IsRevoked);
            Assert.Contains(context.RefreshTokens, x => x.Token == "new-refresh-token");
            Assert.Equal(2, context.RefreshTokens.Count());

            jwtServiceMock.Verify(x => x.GenerateRefreshToken(), Times.Once);
            jwtServiceMock.Verify(x => x.GenerateAccessTokenAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)), Times.Once);
        }
        [Fact]
        public async Task RegisterPatientAsync_WhenEmailDomainIsHealthAxis_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var request = CreateRegisterPatientDto(
                email: "patient@healthaxis.com");

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.RegisterPatientAsync(request));

            Assert.Equal("Patients cannot register using HealthAxis email domain", exception.Message);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDateOfBirthIsFuture_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var request = CreateRegisterPatientDto();

            request.DateOfBirth = DateTime.UtcNow.Date.AddDays(1);

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.RegisterPatientAsync(request));

            Assert.Equal("Date of birth cannot be greater than today's date", exception.Message);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDateOfBirthIsTooOld_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var request = CreateRegisterPatientDto();

            request.DateOfBirth = DateTime.UtcNow.Date.AddYears(-121);

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.RegisterPatientAsync(request));

            Assert.Equal("Please enter a valid date of birth", exception.Message);
        }

        [Fact]
        public async Task ForgotPasswordAsync_WhenUserNotFound_ShouldThrowNotFoundException()
        {
            using var context = CreateDbContext();

            var request = new ForgotPasswordDto
            {
                Email = "missing@test.com"
            };

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.ForgotPasswordAsync(request));

            Assert.Equal("User not found", exception.Message);
        }

        [Fact]
        public async Task ForgotPasswordAsync_WhenUserExists_ShouldReturnResetToken()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                email: "user@test.com");

            var request = new ForgotPasswordDto
            {
                Email = user.Email!
            };

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync("reset-token");

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var result = await service.ForgotPasswordAsync(request);

            Assert.Equal("Password reset token generated successfully", result.Message);
            Assert.Equal("reset-token", result.ResetToken);
        }

        [Fact]
        public async Task ResetPasswordAsync_WhenUserNotFound_ShouldThrowNotFoundException()
        {
            using var context = CreateDbContext();

            var request = new ResetPasswordDto
            {
                Email = "missing@test.com",
                Token = "token",
                NewPassword = "NewPassword@123"
            };

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.ResetPasswordAsync(request));

            Assert.Equal("User not found", exception.Message);
        }

        [Fact]
        public async Task ResetPasswordAsync_WhenResetFails_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser();

            var request = new ResetPasswordDto
            {
                Email = user.Email!,
                Token = "bad-token",
                NewPassword = "NewPassword@123"
            };

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(x => x.ResetPasswordAsync(user, request.Token, request.NewPassword))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Invalid token"
                    }));

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.ResetPasswordAsync(request));

            Assert.Contains("Invalid token", exception.Message);
        }

        [Fact]
        public async Task ResetPasswordAsync_WhenValid_ShouldReturnSuccessMessage()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser();

            var request = new ResetPasswordDto
            {
                Email = user.Email!,
                Token = "valid-token",
                NewPassword = "NewPassword@123"
            };

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(x => x.ResetPasswordAsync(user, request.Token, request.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var result = await service.ResetPasswordAsync(request);

            Assert.Equal("Password reset successfully", result);
        }
        [Fact]
        public async Task ChangeFirstLoginPasswordAsync_WhenPasswordsDoNotMatch_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var request = new ChangeFirstLoginPasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "DifferentPassword@123"
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.ChangeFirstLoginPasswordAsync(
                    request,
                    CreateClaimsPrincipal()));

            Assert.Equal("New password and confirm password do not match", exception.Message);
        }

        [Fact]
        public async Task ChangeFirstLoginPasswordAsync_WhenUserIdClaimMissing_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var request = new ChangeFirstLoginPasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.ChangeFirstLoginPasswordAsync(
                    request,
                    CreateClaimsPrincipal(userId: null)));

            Assert.Equal("User ID claim missing", exception.Message);
        }

        [Fact]
        public async Task ChangeFirstLoginPasswordAsync_WhenUserNotFound_ShouldThrowNotFoundException()
        {
            using var context = CreateDbContext();

            var request = new ChangeFirstLoginPasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.ChangeFirstLoginPasswordAsync(
                    request,
                    CreateClaimsPrincipal("missing-user")));

            Assert.Equal("User not found", exception.Message);
        }

        [Fact]
        public async Task ChangeFirstLoginPasswordAsync_WhenUserIsInactive_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                id: "inactive-doctor",
                email: "inactive-doctor@test.com",
                isActive: false);

            user.FirstLogin = true;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var request = new ChangeFirstLoginPasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.ChangeFirstLoginPasswordAsync(
                    request,
                    CreateClaimsPrincipal(user.Id)));

            Assert.Equal("User is inactive", exception.Message);
        }

        [Fact]
        public async Task ChangeFirstLoginPasswordAsync_WhenUserIsNotDoctor_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                id: "patient-user",
                email: "patient-user@test.com",
                isActive: true);

            user.FirstLogin = true;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Patient" });

            var request = new ChangeFirstLoginPasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.ChangeFirstLoginPasswordAsync(
                    request,
                    CreateClaimsPrincipal(user.Id)));

            Assert.Equal("Only doctors can change first login password", exception.Message);
        }

        [Fact]
        public async Task ChangeFirstLoginPasswordAsync_WhenFirstLoginAlreadyFalse_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                id: "doctor-user",
                email: "doctor-user@test.com",
                isActive: true);

            user.FirstLogin = false;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Doctor" });

            var request = new ChangeFirstLoginPasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.ChangeFirstLoginPasswordAsync(
                    request,
                    CreateClaimsPrincipal(user.Id)));

            Assert.Equal("Password has already been changed", exception.Message);
        }

        [Fact]
        public async Task ChangeFirstLoginPasswordAsync_WhenChangePasswordFails_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                id: "doctor-change-fail",
                email: "doctor-change-fail@test.com",
                isActive: true);

            user.FirstLogin = true;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Doctor" });

            userManagerMock
                .Setup(x => x.ChangePasswordAsync(
                    It.Is<ApplicationUser>(u => u.Id == user.Id),
                    "OldPassword@123",
                    "NewPassword@123"))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Current password is incorrect"
                    }));

            var request = new ChangeFirstLoginPasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.ChangeFirstLoginPasswordAsync(
                    request,
                    CreateClaimsPrincipal(user.Id)));

            Assert.Contains("Current password is incorrect", exception.Message);
        }

        [Fact]
        public async Task ChangeFirstLoginPasswordAsync_WhenValidDoctorFirstLogin_ShouldChangePasswordAndDisableFirstLogin()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                id: "doctor-success",
                email: "doctor-success@test.com",
                isActive: true);

            user.FirstLogin = true;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Doctor" });

            userManagerMock
                .Setup(x => x.ChangePasswordAsync(
                    It.Is<ApplicationUser>(u => u.Id == user.Id),
                    "OldPassword@123",
                    "NewPassword@123"))
                .ReturnsAsync(IdentityResult.Success);

            var request = new ChangeFirstLoginPasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var result = await service.ChangeFirstLoginPasswordAsync(
                request,
                CreateClaimsPrincipal(user.Id));

            Assert.Equal("Password changed successfully", result);
            Assert.False(user.FirstLogin);

            userManagerMock.Verify(x => x.ChangePasswordAsync(
                It.Is<ApplicationUser>(u => u.Id == user.Id),
                "OldPassword@123",
                "NewPassword@123"), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenPasswordsDoNotMatch_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var request = new ChangePasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "DifferentPassword@123"
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.ChangePasswordAsync(
                    request,
                    CreateClaimsPrincipal()));

            Assert.Equal("New password and confirm password do not match", exception.Message);
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenUserIdClaimMissing_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var request = new ChangePasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.ChangePasswordAsync(
                    request,
                    CreateClaimsPrincipal(userId: null)));

            Assert.Equal("User ID claim missing", exception.Message);
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenUserNotFound_ShouldThrowNotFoundException()
        {
            using var context = CreateDbContext();

            var request = new ChangePasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.ChangePasswordAsync(
                    request,
                    CreateClaimsPrincipal("missing-user")));

            Assert.Equal("User not found", exception.Message);
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenUserIsInactive_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                id: "inactive-user",
                email: "inactive-user@test.com",
                isActive: false);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var request = new ChangePasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(context);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.ChangePasswordAsync(
                    request,
                    CreateClaimsPrincipal(user.Id)));

            Assert.Equal("User is inactive", exception.Message);
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenUserIsNotPatientOrDoctor_ShouldThrowUnauthorizedException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                id: "admin-user",
                email: "admin-user@test.com",
                isActive: true);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Admin" });

            var request = new ChangePasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.ChangePasswordAsync(
                    request,
                    CreateClaimsPrincipal(user.Id)));

            Assert.Equal("Only patients and doctors can change password here", exception.Message);
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenChangePasswordFails_ShouldThrowInvalidException()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                id: "patient-change-fail",
                email: "patient-change-fail@test.com",
                isActive: true);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Patient" });

            userManagerMock
                .Setup(x => x.ChangePasswordAsync(
                    It.Is<ApplicationUser>(u => u.Id == user.Id),
                    "OldPassword@123",
                    "NewPassword@123"))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Password change failed"
                    }));

            var request = new ChangePasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.ChangePasswordAsync(
                    request,
                    CreateClaimsPrincipal(user.Id)));

            Assert.Contains("Password change failed", exception.Message);
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenValidPatient_ShouldChangePassword()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                id: "patient-change-success",
                email: "patient-change-success@test.com",
                isActive: true);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Patient" });

            userManagerMock
                .Setup(x => x.ChangePasswordAsync(
                    It.Is<ApplicationUser>(u => u.Id == user.Id),
                    "OldPassword@123",
                    "NewPassword@123"))
                .ReturnsAsync(IdentityResult.Success);

            var request = new ChangePasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var result = await service.ChangePasswordAsync(
                request,
                CreateClaimsPrincipal(user.Id));

            Assert.Equal("Password changed successfully", result);

            userManagerMock.Verify(x => x.ChangePasswordAsync(
                It.Is<ApplicationUser>(u => u.Id == user.Id),
                "OldPassword@123",
                "NewPassword@123"), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenValidDoctor_ShouldChangePassword()
        {
            using var context = CreateDbContext();

            var user = CreateApplicationUser(
                id: "doctor-change-success",
                email: "doctor-change-success@test.com",
                isActive: true);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)))
                .ReturnsAsync(new List<string> { "Doctor" });

            userManagerMock
                .Setup(x => x.ChangePasswordAsync(
                    It.Is<ApplicationUser>(u => u.Id == user.Id),
                    "OldPassword@123",
                    "NewPassword@123"))
                .ReturnsAsync(IdentityResult.Success);

            var request = new ChangePasswordDto
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123",
                ConfirmPassword = "NewPassword@123"
            };

            var service = CreateService(
                context,
                userManagerMock: userManagerMock);

            var result = await service.ChangePasswordAsync(
                request,
                CreateClaimsPrincipal(user.Id));

            Assert.Equal("Password changed successfully", result);

            userManagerMock.Verify(x => x.ChangePasswordAsync(
                It.Is<ApplicationUser>(u => u.Id == user.Id),
                "OldPassword@123",
                "NewPassword@123"), Times.Once);
        }
    }
}