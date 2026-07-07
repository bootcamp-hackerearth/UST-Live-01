using FluentAssertions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services.Impl;
using HealthCareApp.Shared.Dtos.Auth;
using HealthCareApp.Shared.Dtos.Patients;
using HealthCareApp.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HealthCareApp.Testing.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<IdentityUser>> userManagerMock;

        private readonly Mock<IPatientRepository> patientRepositoryMock;

        private readonly Mock<IDoctorRepository> doctorRepositoryMock;

        private readonly IConfiguration configuration;

        private readonly AuthService authService;

        public AuthServiceTests()
        {
            userManagerMock = CreateUserManagerMock();

            patientRepositoryMock = new Mock<IPatientRepository>();

            doctorRepositoryMock = new Mock<IDoctorRepository>();

            configuration = CreateConfiguration();

            authService = new AuthService(
                userManagerMock.Object,
                patientRepositoryMock.Object,
                doctorRepositoryMock.Object,
                configuration);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenPasswordAndConfirmPasswordDoNotMatch_ShouldReturnFailure()
        {
            var request = GetValidPatientRegisterDto();

            request.ConfirmPassword = "Different@123";

            var result = await authService.RegisterPatientAsync(request);

            result.Success.Should().BeFalse();

            result.Message.Should().Be("Password and Confirm Password do not match.");

            result.PatientId.Should().Be(0);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDateOfBirthIsFutureDate_ShouldReturnFailure()
        {
            var request = GetValidPatientRegisterDto();

            request.DateOfBirth = DateTime.Today.AddDays(1);

            var result = await authService.RegisterPatientAsync(request);

            result.Success.Should().BeFalse();

            result.Message.Should().Be("Date of birth cannot be a future date.");

            result.PatientId.Should().Be(0);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenEmailAlreadyRegistered_ShouldReturnFailure()
        {
            var request = GetValidPatientRegisterDto();

            userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(new IdentityUser
                {
                    Email = request.Email,
                    UserName = request.Email
                });

            var result = await authService.RegisterPatientAsync(request);

            result.Success.Should().BeFalse();

            result.Message.Should().Be("Email is already registered.");

            result.PatientId.Should().Be(0);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenCreateUserFails_ShouldReturnFailureWithErrors()
        {
            var request = GetValidPatientRegisterDto();

            userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((IdentityUser?)null);

            userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    request.Password))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Password is weak."
                    }));

            var result = await authService.RegisterPatientAsync(request);

            result.Success.Should().BeFalse();

            result.Message.Should().Be("Password is weak.");

            result.PatientId.Should().Be(0);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenAddToRoleFails_ShouldDeleteUserAndReturnFailure()
        {
            var request = GetValidPatientRegisterDto();

            userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((IdentityUser?)null);

            userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    request.Password))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock
                .Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<IdentityUser>(),
                    "Patient"))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Role assignment failed."
                    }));

            userManagerMock
                .Setup(manager => manager.DeleteAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await authService.RegisterPatientAsync(request);

            result.Success.Should().BeFalse();

            result.Message.Should().Be("Role assignment failed.");

            result.PatientId.Should().Be(0);

            userManagerMock.Verify(
                manager => manager.DeleteAsync(It.IsAny<IdentityUser>()),
                Times.Once);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenValid_ShouldCreatePatientAndReturnSuccess()
        {
            var request = GetValidPatientRegisterDto();

            userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((IdentityUser?)null);

            userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    request.Password))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock
                .Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<IdentityUser>(),
                    "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            patientRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Patient>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient patient, CancellationToken cancellationToken) =>
                {
                    patient.PatientId = 101;

                    return patient;
                });

            var result = await authService.RegisterPatientAsync(request);

            result.Success.Should().BeTrue();

            result.Message.Should().Be("Patient registered successfully.");

            result.PatientId.Should().Be(101);

            patientRepositoryMock.Verify(
                repository => repository.CreateAsync(
                    It.Is<Patient>(patient =>
                        patient.PatientName == request.FullName &&
                        patient.DateOfBirth == request.DateOfBirth.Date &&
                        patient.Gender == request.Gender &&
                        patient.Email == request.Email &&
                        patient.PhoneNumber == request.PhoneNumber &&
                        patient.InsuranceID == request.InsuranceId &&
                        !string.IsNullOrWhiteSpace(patient.IdentityUserId) &&
                        patient.CreatedDate != default),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Login_WhenUserDoesNotExist_ShouldReturnInvalidCredentials()
        {
            var request = GetValidLoginDto();

            userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((IdentityUser?)null);

            var result = await authService.Login(request);

            result.Success.Should().BeFalse();

            result.Message.Should().Be("Invalid Credentials");

            result.Token.Should().BeEmpty();

            result.ExpiresIn.Should().Be(0);

            result.MustChangePassword.Should().BeFalse();
        }

        [Fact]
        public async Task Login_WhenPasswordIsInvalid_ShouldReturnInvalidCredentials()
        {
            var request = GetValidLoginDto();

            var user = new IdentityUser
            {
                Id = "user-1",
                Email = request.Email,
                UserName = request.Email
            };

            userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(manager => manager.CheckPasswordAsync(
                    user,
                    request.Password))
                .ReturnsAsync(false);

            var result = await authService.Login(request);

            result.Success.Should().BeFalse();

            result.Message.Should().Be("Invalid Credentials");

            result.Token.Should().BeEmpty();

            result.ExpiresIn.Should().Be(0);

            result.MustChangePassword.Should().BeFalse();
        }

        [Fact]
        public async Task Login_WhenValid_ShouldReturnJwtToken()
        {
            var request = GetValidLoginDto();

            var user = new IdentityUser
            {
                Id = "user-1",
                Email = request.Email,
                UserName = request.Email
            };

            userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(manager => manager.CheckPasswordAsync(
                    user,
                    request.Password))
                .ReturnsAsync(true);

            userManagerMock
                .Setup(manager => manager.GetRolesAsync(user))
                .ReturnsAsync(new List<string>
                {
                    "Admin"
                });

            var result = await authService.Login(request);

            result.Success.Should().BeTrue();

            result.Message.Should().Be("Login Successful");

            result.Token.Should().NotBeNullOrWhiteSpace();

            result.ExpiresIn.Should().Be(60);

            result.MustChangePassword.Should().BeFalse();

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.ReadJwtToken(result.Token);

            jwtToken.Claims.Should().Contain(claim =>
                claim.Type == JwtRegisteredClaimNames.Email &&
                claim.Value == request.Email);

            jwtToken.Claims.Should().Contain(claim =>
                claim.Type == ClaimTypes.Role &&
                claim.Value == "Admin");
        }

        [Fact]
        public async Task Login_WhenDoctorMustChangePassword_ShouldReturnMustChangePasswordTrue()
        {
            var request = GetValidLoginDto();

            var user = new IdentityUser
            {
                Id = "doctor-user-1",
                Email = request.Email,
                UserName = request.Email
            };

            var doctor = new Doctor
            {
                DoctorId = 10,
                DoctorName = "Test Doctor",
                Email = request.Email,
                Specialisation = SpecialisationType.GeneralPractitioner,
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IdentityUserId = user.Id,
                IsActive = true,
                MustChangePassword = true,
                CreatedDate = DateTime.Now
            };

            userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(manager => manager.CheckPasswordAsync(
                    user,
                    request.Password))
                .ReturnsAsync(true);

            userManagerMock
                .Setup(manager => manager.GetRolesAsync(user))
                .ReturnsAsync(new List<string>
                {
                    "Doctor"
                });

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            var result = await authService.Login(request);

            result.Success.Should().BeTrue();

            result.Message.Should().Be("Login Successful");

            result.Token.Should().NotBeNullOrWhiteSpace();

            result.ExpiresIn.Should().Be(60);

            result.MustChangePassword.Should().BeTrue();
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenNewPasswordSameAsCurrentPassword_ShouldReturnFailure()
        {
            var request = new ChangePasswordDto
            {
                CurrentPassword = "Same@123",
                NewPassword = "Same@123",
                ConfirmNewPassword = "Same@123"
            };

            var result = await authService.ChangePasswordAsync(
                "user-1",
                request);

            result.Success.Should().BeFalse();

            result.Message.Should().Be("New password cannot be the same as current password.");
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenNewPasswordAndConfirmPasswordDoNotMatch_ShouldReturnFailure()
        {
            var request = new ChangePasswordDto
            {
                CurrentPassword = "Old@123",
                NewPassword = "New@123",
                ConfirmNewPassword = "Different@123"
            };

            var result = await authService.ChangePasswordAsync(
                "user-1",
                request);

            result.Success.Should().BeFalse();

            result.Message.Should().Be("New Password and Confirm New Password do not match.");
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenUserDoesNotExist_ShouldReturnFailure()
        {
            var request = GetValidChangePasswordDto();

            userManagerMock
                .Setup(manager => manager.FindByIdAsync("user-1"))
                .ReturnsAsync((IdentityUser?)null);

            var result = await authService.ChangePasswordAsync(
                "user-1",
                request);

            result.Success.Should().BeFalse();

            result.Message.Should().Be("User not found.");
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenIdentityChangePasswordFails_ShouldReturnFailure()
        {
            var request = GetValidChangePasswordDto();

            var user = new IdentityUser
            {
                Id = "user-1",
                Email = "user@example.com",
                UserName = "user@example.com"
            };

            userManagerMock
                .Setup(manager => manager.FindByIdAsync("user-1"))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(manager => manager.ChangePasswordAsync(
                    user,
                    request.CurrentPassword,
                    request.NewPassword))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Current password is incorrect."
                    }));

            var result = await authService.ChangePasswordAsync(
                "user-1",
                request);

            result.Success.Should().BeFalse();

            result.Message.Should().Be("Current password is incorrect.");
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenValid_ShouldReturnSuccess()
        {
            var request = GetValidChangePasswordDto();

            var user = new IdentityUser
            {
                Id = "user-1",
                Email = "user@example.com",
                UserName = "user@example.com"
            };

            userManagerMock
                .Setup(manager => manager.FindByIdAsync("user-1"))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(manager => manager.ChangePasswordAsync(
                    user,
                    request.CurrentPassword,
                    request.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock
                .Setup(manager => manager.GetRolesAsync(user))
                .ReturnsAsync(new List<string>
                {
                    "Patient"
                });

            var result = await authService.ChangePasswordAsync(
                "user-1",
                request);

            result.Success.Should().BeTrue();

            result.Message.Should().Be("Password changed successfully.");
        }

        [Fact]
        public async Task ChangePasswordAsync_WhenDoctorMustChangePassword_ShouldSetMustChangePasswordFalse()
        {
            var request = GetValidChangePasswordDto();

            var user = new IdentityUser
            {
                Id = "doctor-user-1",
                Email = "doctor@example.com",
                UserName = "doctor@example.com"
            };

            var doctor = new Doctor
            {
                DoctorId = 11,
                DoctorName = "Test Doctor",
                Email = "doctor@example.com",
                Specialisation = SpecialisationType.GeneralPractitioner,
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IdentityUserId = user.Id,
                IsActive = true,
                MustChangePassword = true,
                CreatedDate = DateTime.Now
            };

            userManagerMock
                .Setup(manager => manager.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(manager => manager.ChangePasswordAsync(
                    user,
                    request.CurrentPassword,
                    request.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock
                .Setup(manager => manager.GetRolesAsync(user))
                .ReturnsAsync(new List<string>
                {
                    "Doctor"
                });

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            doctorRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    doctor.DoctorId,
                    It.IsAny<Doctor>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int doctorId, Doctor entity, CancellationToken cancellationToken) => entity);

            var result = await authService.ChangePasswordAsync(
                user.Id,
                request);

            result.Success.Should().BeTrue();

            result.Message.Should().Be("Password changed successfully.");

            doctor.MustChangePassword.Should().BeFalse();

            doctorRepositoryMock.Verify(
                repository => repository.UpdateAsync(
                    doctor.DoctorId,
                    It.Is<Doctor>(savedDoctor => savedDoctor.MustChangePassword == false),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private static PatientRegisterDto GetValidPatientRegisterDto()
        {
            return new PatientRegisterDto
            {
                FullName = "New Patient",
                DateOfBirth = new DateTime(2001, 2, 2),
                Gender = GenderType.Male,
                Email = "newpatient@example.com",
                PhoneNumber = "9876500000",
                InsuranceId = "INS999",
                Password = "Patient@123",
                ConfirmPassword = "Patient@123"
            };
        }

        private static LoginDto GetValidLoginDto()
        {
            return new LoginDto
            {
                Email = "admin@healthcare.com",
                Password = "Admin@123"
            };
        }

        private static ChangePasswordDto GetValidChangePasswordDto()
        {
            return new ChangePasswordDto
            {
                CurrentPassword = "Old@123",
                NewPassword = "New@123",
                ConfirmNewPassword = "New@123"
            };
        }

        private static IConfiguration CreateConfiguration()
        {
            var configurationValues = new Dictionary<string, string?>
            {
                {
                    "Jwt:Key",
                    "ThisIsASecretKeyForHealthAxisJwtTesting123456789"
                },
                {
                    "Jwt:Issuer",
                    "HealthAxisTestIssuer"
                },
                {
                    "Jwt:Audience",
                    "HealthAxisTestAudience"
                },
                {
                    "Jwt:AccessTokenExpirationMinutes",
                    "60"
                }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(configurationValues)
                .Build();
        }

        private static Mock<UserManager<IdentityUser>> CreateUserManagerMock()
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
    }
}