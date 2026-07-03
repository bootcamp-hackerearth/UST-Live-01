using HealthApp.API.Exceptions;
using HealthApp.API.Identity;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace HealthApp.API.Tests;

public class AuthServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> userManagerMock;
    private readonly Mock<IConfiguration> configurationMock = new();
    private readonly Mock<IRefreshTokenRepository> refreshTokenRepositoryMock = new();
    private readonly Mock<IPatientRepository> patientRepositoryMock = new();
    private readonly Mock<IDoctorRepository> doctorRepositoryMock = new();

    private readonly AuthService authService;

    public AuthServiceTests()
    {
        userManagerMock = MockUserManager();

        SetupJwtConfiguration();

        authService = new AuthService(
            userManagerMock.Object,
            configurationMock.Object,
            refreshTokenRepositoryMock.Object,
            patientRepositoryMock.Object,
            doctorRepositoryMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowConflictException_WhenEmailAlreadyRegistered()
    {
        // Arrange
        var dto = CreateValidRegisterRequest();

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync(new ApplicationUser
            {
                Id = "existing-user-id",
                Email = dto.Email
            });

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() =>
            authService.RegisterAsync(dto));
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowBusinessRuleException_WhenUserCreationFails()
    {
        // Arrange
        var dto = CreateValidRegisterRequest();

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync((ApplicationUser?)null);

        userManagerMock
            .Setup(manager => manager.CreateAsync(
                It.IsAny<ApplicationUser>(),
                dto.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Description = "Password validation failed."
            }));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.RegisterAsync(dto));

        Assert.Contains("Password validation failed", exception.Message);
    }

    [Fact]
    public async Task RegisterAsync_ShouldCreatePatientUserAndReturnAuthResponse()
    {
        // Arrange
        var dto = CreateValidRegisterRequest();
        const string createdUserId = "patient-user-id";

        var createdPatient = new Patient
        {
            PatientId = 7,
            UserId = createdUserId,
            PatientName = dto.FullName,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth!.Value.Date,
            Gender = dto.Gender!.Value.ToString(),
            PhoneNumber = dto.PhoneNumber ?? string.Empty,
            InsuranceId = dto.InsuranceId
        };

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync((ApplicationUser?)null);

        userManagerMock
            .Setup(manager => manager.CreateAsync(
                It.IsAny<ApplicationUser>(),
                dto.Password))
            .Callback<ApplicationUser, string>((user, _) =>
            {
                user.Id = createdUserId;
            })
            .ReturnsAsync(IdentityResult.Success);

        userManagerMock
            .Setup(manager => manager.AddToRoleAsync(
                It.IsAny<ApplicationUser>(),
                Roles.Patient))
            .ReturnsAsync(IdentityResult.Success);

        userManagerMock
            .Setup(manager => manager.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { Roles.Patient });

        patientRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Patient>()))
            .ReturnsAsync(createdPatient);

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync(createdUserId))
            .ReturnsAsync(createdPatient);

        refreshTokenRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await authService.RegisterAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Email, result.Email);
        Assert.Equal(Roles.Patient, result.Role);
        Assert.False(result.MustChangePassword);
        Assert.Equal(7, result.PatientId);
        Assert.Null(result.DoctorId);
        Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));

        userManagerMock.Verify(manager =>
            manager.CreateAsync(
                It.Is<ApplicationUser>(user =>
                    user.Email == dto.Email &&
                    user.UserName == dto.Email &&
                    user.FullName == dto.FullName &&
                    user.EmailConfirmed &&
                    !user.MustChangePassword),
                dto.Password),
            Times.Once);

        userManagerMock.Verify(manager =>
            manager.AddToRoleAsync(
                It.IsAny<ApplicationUser>(),
                Roles.Patient),
            Times.Once);

        patientRepositoryMock.Verify(repository =>
            repository.AddAsync(It.Is<Patient>(patient =>
                patient.UserId == createdUserId &&
                patient.PatientName == dto.FullName &&
                patient.Email == dto.Email &&
                patient.PhoneNumber == dto.PhoneNumber)),
            Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowBusinessRuleException_WhenEmailInvalid()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Email = "invalid@gmail.com",
            Password = "Password@123"
        };

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync((ApplicationUser?)null);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.LoginAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowBusinessRuleException_WhenPasswordInvalid()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Email = "patient@gmail.com",
            Password = "WrongPassword"
        };

        var user = new ApplicationUser
        {
            Id = "patient-user-id",
            Email = dto.Email
        };

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(manager => manager.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.LoginAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnMustChangePasswordResponse_WhenDoctorMustChangePassword()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Email = "doctor@gmail.com",
            Password = "Temp@123"
        };

        var user = new ApplicationUser
        {
            Id = "doctor-user-id",
            Email = dto.Email,
            MustChangePassword = true
        };

        var doctor = new Doctor
        {
            DoctorId = 4,
            UserId = user.Id,
            DoctorName = "Dr Sneha Paul",
            IsActive = true
        };

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(manager => manager.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        userManagerMock
            .Setup(manager => manager.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { Roles.Doctor });

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync(user.Id))
            .ReturnsAsync(doctor);

        // Act
        var result = await authService.LoginAsync(dto);

        // Assert
        Assert.True(result.MustChangePassword);
        Assert.Equal(Roles.Doctor, result.Role);
        Assert.Equal(dto.Email, result.Email);
        Assert.Equal(4, result.DoctorId);
        Assert.Null(result.PatientId);
        Assert.Equal(string.Empty, result.AccessToken);
        Assert.Equal(string.Empty, result.RefreshToken);
        Assert.Equal(DateTime.MinValue, result.AccessTokenExpiresAt);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnAuthResponse_WhenPatientLoginIsSuccessful()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Email = "patient@gmail.com",
            Password = "Patient@123"
        };

        var user = new ApplicationUser
        {
            Id = "patient-user-id",
            Email = dto.Email,
            MustChangePassword = false
        };

        var patient = new Patient
        {
            PatientId = 9,
            UserId = user.Id,
            PatientName = "Kevin Baby",
            Email = dto.Email
        };

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(manager => manager.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        userManagerMock
            .Setup(manager => manager.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { Roles.Patient });

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync(user.Id))
            .ReturnsAsync(patient);

        refreshTokenRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await authService.LoginAsync(dto);

        // Assert
        Assert.False(result.MustChangePassword);
        Assert.Equal(Roles.Patient, result.Role);
        Assert.Equal(dto.Email, result.Email);
        Assert.Equal(9, result.PatientId);
        Assert.Null(result.DoctorId);
        Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnAuthResponse_WhenDoctorLoginIsSuccessful()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Email = "doctor@gmail.com",
            Password = "Doctor@123"
        };

        var user = new ApplicationUser
        {
            Id = "doctor-user-id",
            Email = dto.Email,
            MustChangePassword = false
        };

        var doctor = new Doctor
        {
            DoctorId = 11,
            UserId = user.Id,
            DoctorName = "Dr Sneha Paul",
            IsActive = true
        };

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(manager => manager.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        userManagerMock
            .Setup(manager => manager.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { Roles.Doctor });

        doctorRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync(user.Id))
            .ReturnsAsync(doctor);

        refreshTokenRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await authService.LoginAsync(dto);

        // Assert
        Assert.False(result.MustChangePassword);
        Assert.Equal(Roles.Doctor, result.Role);
        Assert.Equal(11, result.DoctorId);
        Assert.Null(result.PatientId);
        Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldThrowBusinessRuleException_WhenDtoIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.ChangePasswordAsync(null!));
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldThrowBusinessRuleException_WhenEmailMissing()
    {
        // Arrange
        var dto = CreateValidChangePasswordDto();
        dto.Email = "";

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.ChangePasswordAsync(dto));
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldThrowBusinessRuleException_WhenPasswordsDoNotMatch()
    {
        // Arrange
        var dto = CreateValidChangePasswordDto();
        dto.ConfirmNewPassword = "Different@123";

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.ChangePasswordAsync(dto));
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldThrowBusinessRuleException_WhenNewPasswordSameAsCurrentPassword()
    {
        // Arrange
        var dto = CreateValidChangePasswordDto();
        dto.NewPassword = dto.CurrentPassword;
        dto.ConfirmNewPassword = dto.CurrentPassword;

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.ChangePasswordAsync(dto));
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldThrowBusinessRuleException_WhenUserNotFound()
    {
        // Arrange
        var dto = CreateValidChangePasswordDto();

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync((ApplicationUser?)null);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.ChangePasswordAsync(dto));
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldThrowBusinessRuleException_WhenCurrentPasswordInvalid()
    {
        // Arrange
        var dto = CreateValidChangePasswordDto();

        var user = new ApplicationUser
        {
            Id = "doctor-user-id",
            Email = dto.Email
        };

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(manager => manager.CheckPasswordAsync(user, dto.CurrentPassword))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.ChangePasswordAsync(dto));
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldClearMustChangePassword_WhenPasswordChangeSuccessful()
    {
        // Arrange
        var dto = CreateValidChangePasswordDto();

        var user = new ApplicationUser
        {
            Id = "doctor-user-id",
            Email = dto.Email,
            MustChangePassword = true
        };

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(manager => manager.CheckPasswordAsync(user, dto.CurrentPassword))
            .ReturnsAsync(true);

        userManagerMock
            .Setup(manager => manager.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        userManagerMock
            .Setup(manager => manager.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await authService.ChangePasswordAsync(dto);

        // Assert
        Assert.False(user.MustChangePassword);

        userManagerMock.Verify(manager =>
            manager.UpdateAsync(It.Is<ApplicationUser>(updatedUser =>
                updatedUser.Id == user.Id &&
                !updatedUser.MustChangePassword)),
            Times.Once);
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldThrowBusinessRuleException_WhenIdentityChangePasswordFails()
    {
        // Arrange
        var dto = CreateValidChangePasswordDto();

        var user = new ApplicationUser
        {
            Id = "doctor-user-id",
            Email = dto.Email,
            MustChangePassword = true
        };

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(manager => manager.CheckPasswordAsync(user, dto.CurrentPassword))
            .ReturnsAsync(true);

        userManagerMock
            .Setup(manager => manager.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Description = "Password is too weak."
            }));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.ChangePasswordAsync(dto));

        Assert.Contains("Password is too weak", exception.Message);
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowBusinessRuleException_WhenRefreshTokenNotFound()
    {
        // Arrange
        var dto = new RefreshTokenRequestDto
        {
            RefreshToken = "invalid-refresh-token"
        };

        refreshTokenRepositoryMock
            .Setup(repository => repository.GetByTokenAsync(dto.RefreshToken))
            .ReturnsAsync((RefreshToken?)null);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.RefreshTokenAsync(dto));
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowBusinessRuleException_WhenRefreshTokenIsUsed()
    {
        // Arrange
        var dto = new RefreshTokenRequestDto
        {
            RefreshToken = "used-refresh-token"
        };

        var refreshToken = new RefreshToken
        {
            Token = dto.RefreshToken,
            IsUsed = true,
            IsRevoked = false,
            ExpiryDate = DateTime.UtcNow.AddDays(1),
            User = new ApplicationUser
            {
                Id = "patient-user-id",
                Email = "patient@gmail.com"
            }
        };

        refreshTokenRepositoryMock
            .Setup(repository => repository.GetByTokenAsync(dto.RefreshToken))
            .ReturnsAsync(refreshToken);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.RefreshTokenAsync(dto));
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowBusinessRuleException_WhenRefreshTokenExpired()
    {
        // Arrange
        var dto = new RefreshTokenRequestDto
        {
            RefreshToken = "expired-refresh-token"
        };

        var refreshToken = new RefreshToken
        {
            Token = dto.RefreshToken,
            IsUsed = false,
            IsRevoked = false,
            ExpiryDate = DateTime.UtcNow.AddMinutes(-1),
            User = new ApplicationUser
            {
                Id = "patient-user-id",
                Email = "patient@gmail.com"
            }
        };

        refreshTokenRepositoryMock
            .Setup(repository => repository.GetByTokenAsync(dto.RefreshToken))
            .ReturnsAsync(refreshToken);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.RefreshTokenAsync(dto));
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowPasswordChangeRequiredException_WhenUserMustChangePassword()
    {
        // Arrange
        var dto = new RefreshTokenRequestDto
        {
            RefreshToken = "refresh-token"
        };

        var refreshToken = new RefreshToken
        {
            Token = dto.RefreshToken,
            IsUsed = false,
            IsRevoked = false,
            ExpiryDate = DateTime.UtcNow.AddDays(1),
            User = new ApplicationUser
            {
                Id = "doctor-user-id",
                Email = "doctor@gmail.com",
                MustChangePassword = true
            }
        };

        refreshTokenRepositoryMock
            .Setup(repository => repository.GetByTokenAsync(dto.RefreshToken))
            .ReturnsAsync(refreshToken);

        // Act & Assert
        await Assert.ThrowsAsync<PasswordChangeRequiredException>(() =>
            authService.RefreshTokenAsync(dto));
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldReturnNewAuthResponse_WhenRefreshTokenValid()
    {
        // Arrange
        var dto = new RefreshTokenRequestDto
        {
            RefreshToken = "valid-refresh-token"
        };

        var user = new ApplicationUser
        {
            Id = "patient-user-id",
            Email = "patient@gmail.com",
            MustChangePassword = false
        };

        var patient = new Patient
        {
            PatientId = 3,
            UserId = user.Id,
            Email = user.Email,
            PatientName = "Kevin Baby"
        };

        var refreshToken = new RefreshToken
        {
            Token = dto.RefreshToken,
            IsUsed = false,
            IsRevoked = false,
            ExpiryDate = DateTime.UtcNow.AddDays(1),
            User = user
        };

        refreshTokenRepositoryMock
            .Setup(repository => repository.GetByTokenAsync(dto.RefreshToken))
            .ReturnsAsync(refreshToken);

        refreshTokenRepositoryMock
            .Setup(repository => repository.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        refreshTokenRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);

        userManagerMock
            .Setup(manager => manager.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { Roles.Patient });

        patientRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync(user.Id))
            .ReturnsAsync(patient);

        // Act
        var result = await authService.RefreshTokenAsync(dto);

        // Assert
        Assert.True(refreshToken.IsUsed);
        Assert.Equal(Roles.Patient, result.Role);
        Assert.Equal(3, result.PatientId);
        Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));

        refreshTokenRepositoryMock.Verify(repository =>
            repository.SaveChangesAsync(),
            Times.Once);
    }

    private void SetupJwtConfiguration()
    {
        configurationMock
            .Setup(configuration => configuration["Jwt:Key"])
            .Returns("this-is-a-super-secure-test-key-for-healthapp-jwt-token");

        configurationMock
            .Setup(configuration => configuration["Jwt:Issuer"])
            .Returns("HealthAppTestIssuer");

        configurationMock
            .Setup(configuration => configuration["Jwt:Audience"])
            .Returns("HealthAppTestAudience");

        configurationMock
            .Setup(configuration => configuration["Jwt:AccessTokenExpirationMinutes"])
            .Returns("15");

        configurationMock
            .Setup(configuration => configuration["Jwt:RefreshTokenExpirationDays"])
            .Returns("14");
    }

    private static RegisterRequestDto CreateValidRegisterRequest()
    {
        return new RegisterRequestDto
        {
            FullName = "Kevin Baby",
            Email = "kevin@gmail.com",
            Password = "Kevin@123",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderType.Male,
            PhoneNumber = "9876543210",
            InsuranceId = "INS123"
        };
    }

    private static ChangePasswordDto CreateValidChangePasswordDto()
    {
        return new ChangePasswordDto
        {
            Email = "doctor@gmail.com",
            CurrentPassword = "Temp@123",
            NewPassword = "Doctor@123",
            ConfirmNewPassword = "Doctor@123"
        };
    }

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
            null!);
    }
    [Fact]
    public async Task RegisterAsync_WhenDtoIsNull_ThrowsBusinessRuleException()
    {
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.RegisterAsync(null!));
    }

    [Fact]
    public async Task RegisterAsync_WhenFullNameMissing_ThrowsBusinessRuleException()
    {
        var dto = CreateValidRegisterRequest();
        dto.FullName = "";

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.RegisterAsync(dto));
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailMissing_ThrowsBusinessRuleException()
    {
        var dto = CreateValidRegisterRequest();
        dto.Email = "";

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.RegisterAsync(dto));
    }

    [Fact]
    public async Task RegisterAsync_WhenPasswordMissing_ThrowsBusinessRuleException()
    {
        var dto = CreateValidRegisterRequest();
        dto.Password = "";

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.RegisterAsync(dto));
    }

    [Fact]
    public async Task RegisterAsync_WhenDateOfBirthMissing_ThrowsBusinessRuleException()
    {
        var dto = CreateValidRegisterRequest();
        dto.DateOfBirth = null;

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.RegisterAsync(dto));
    }

    [Fact]
    public async Task RegisterAsync_WhenGenderMissing_ThrowsBusinessRuleException()
    {
        var dto = CreateValidRegisterRequest();
        dto.Gender = null;

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.RegisterAsync(dto));
    }

    [Fact]
    public async Task RegisterAsync_WhenPhoneNumberMissing_ThrowsBusinessRuleException()
    {
        var dto = CreateValidRegisterRequest();
        dto.PhoneNumber = "";

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.RegisterAsync(dto));
    }

    [Fact]
    public async Task ChangePasswordAsync_WhenNewPasswordMissing_ThrowsBusinessRuleException()
    {
        var dto = CreateValidChangePasswordDto();
        dto.NewPassword = "";

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            authService.ChangePasswordAsync(dto));
    }

    [Fact]
    public async Task ChangePasswordAsync_WhenMustChangePasswordFalse_DoesNotCallUpdateAsync()
    {
        var dto = CreateValidChangePasswordDto();

        var user = new ApplicationUser
        {
            Id = "patient-user-id",
            Email = dto.Email,
            MustChangePassword = false
        };

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(manager => manager.CheckPasswordAsync(user, dto.CurrentPassword))
            .ReturnsAsync(true);

        userManagerMock
            .Setup(manager => manager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        await authService.ChangePasswordAsync(dto);

        userManagerMock.Verify(manager =>
            manager.UpdateAsync(It.IsAny<ApplicationUser>()),
            Times.Never);
    }
}