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
using Xunit;

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
    }
}