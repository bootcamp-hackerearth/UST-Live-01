using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs.Auth;
using HealthCare.Api.DTOs.Doctor;
using HealthCare.Api.DTOs.Patient;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace HealthCare.Api.Tests;

public class AuthServiceTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPatientRepository> _patientRepoMock;
    private readonly Mock<IDoctorRepository> _doctorRepoMock;
    private readonly Mock<IJwtService> _jwtServiceMock;

    private readonly HealthCareDbContext _context;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _userManagerMock = CreateUserManagerMock();
        _mapperMock = new Mock<IMapper>();
        _patientRepoMock = new Mock<IPatientRepository>();
        _doctorRepoMock = new Mock<IDoctorRepository>();
        _jwtServiceMock = new Mock<IJwtService>();

        var options = new DbContextOptionsBuilder<HealthCareDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new HealthCareDbContext(options);

        _service = new AuthService(
            _userManagerMock.Object,
            _mapperMock.Object,
            _patientRepoMock.Object,
            _doctorRepoMock.Object,
            _jwtServiceMock.Object,
            _context
        );
    }

    private static Mock<UserManager<User>> CreateUserManagerMock()
    {
        var store = new Mock<IUserStore<User>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        var passwordHasher = new Mock<IPasswordHasher<User>>();
        var userValidators = new List<IUserValidator<User>>();
        var passwordValidators = new List<IPasswordValidator<User>>();
        var keyNormalizer = new Mock<ILookupNormalizer>();
        var errors = new IdentityErrorDescriber();
        var services = new Mock<IServiceProvider>();
        var logger = new Mock<ILogger<UserManager<User>>>();

        return new Mock<UserManager<User>>(
            store.Object,
            options.Object,
            passwordHasher.Object,
            userValidators,
            passwordValidators,
            keyNormalizer.Object,
            errors,
            services.Object,
            logger.Object
        );
    }

    [Fact]
    public async Task RegisterPatientAsync_CreatesUserAndPatient_WhenValid()
    {
        // Arrange
        var dto = new CreatePatientDto
        {
            Email = "patient@test.com",
            Password = "Password@123"
        };

        var createdUser = new User
        {
            Id = "user-1",
            Email = dto.Email,
            UserName = dto.Email
        };

        var patient = new Patient
        {
            PatientId = 1,
            FullName = "Test Patient"
        };

        _userManagerMock
            .Setup(u => u.FindByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);

        _userManagerMock
            .Setup(u => u.CreateAsync(It.IsAny<User>(), dto.Password))
            .Callback<User, string>((user, password) =>
            {
                user.Id = createdUser.Id;
            })
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock
            .Setup(u => u.AddToRoleAsync(It.IsAny<User>(), "Patient"))
            .ReturnsAsync(IdentityResult.Success);

        _mapperMock
            .Setup(m => m.Map<Patient>(dto))
            .Returns(patient);

        _patientRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Patient>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.RegisterPatientAsync(dto);

        // Assert
        Assert.Equal("user-1", patient.UserId);

        _userManagerMock.Verify(
            u => u.FindByEmailAsync(dto.Email),
            Times.Once
        );

        _userManagerMock.Verify(
            u => u.CreateAsync(It.IsAny<User>(), dto.Password),
            Times.Once
        );

        _userManagerMock.Verify(
            u => u.AddToRoleAsync(It.IsAny<User>(), "Patient"),
            Times.Once
        );

        _patientRepoMock.Verify(
            r => r.AddAsync(patient),
            Times.Once
        );
    }

    [Fact]
    public async Task RegisterPatientAsync_Throws_WhenEmailAlreadyExists()
    {
        // Arrange
        var dto = new CreatePatientDto
        {
            Email = "patient@test.com",
            Password = "Password@123"
        };

        _userManagerMock
            .Setup(u => u.FindByEmailAsync(dto.Email))
            .ReturnsAsync(new User
            {
                Id = "existing-user",
                Email = dto.Email
            });

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.RegisterPatientAsync(dto));

        // Assert
        Assert.Equal("Email already in use", exception.Message);

        _userManagerMock.Verify(
            u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()),
            Times.Never
        );

        _patientRepoMock.Verify(
            r => r.AddAsync(It.IsAny<Patient>()),
            Times.Never
        );
    }

    [Fact]
    public async Task RegisterPatientAsync_Throws_WhenUserCreationFails()
    {
        // Arrange
        var dto = new CreatePatientDto
        {
            Email = "patient@test.com",
            Password = "weak"
        };

        _userManagerMock
            .Setup(u => u.FindByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);

        _userManagerMock
            .Setup(u => u.CreateAsync(It.IsAny<User>(), dto.Password))
            .ReturnsAsync(IdentityResult.Failed(
                new IdentityError
                {
                    Description = "Password is too weak"
                }
            ));

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.RegisterPatientAsync(dto));

        // Assert
        Assert.Contains("Password is too weak", exception.Message);

        _userManagerMock.Verify(
            u => u.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()),
            Times.Never
        );

        _patientRepoMock.Verify(
            r => r.AddAsync(It.IsAny<Patient>()),
            Times.Never
        );
    }

    
    [Fact]
    public async Task LoginAsync_ReturnsPatientToken_WhenPatientLoginIsValid()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "patient@test.com",
            Password = "Password@123"
        };

        var user = new User
        {
            Id = "patient-user-1",
            Email = dto.Email,
            UserName = dto.Email
        };

        var patient = new Patient
        {
            PatientId = 5,
            UserId = user.Id
        };

        _userManagerMock
            .Setup(u => u.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(u => u.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Patient" });

        _patientRepoMock
            .Setup(r => r.GetByUserIdAsync(user.Id))
            .ReturnsAsync(patient);

        _jwtServiceMock
            .Setup(j => j.GenerateToken(user, patient.PatientId, null))
            .ReturnsAsync("patient-token");

        // Act
        var result = await _service.LoginAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("patient-token", result.AccessToken);
        Assert.Equal("Patient", result.Role);
    }

    [Fact]
    public async Task LoginAsync_ReturnsDoctorToken_WhenDoctorLoginIsValid()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "doctor@test.com",
            Password = "Password@123"
        };

        var user = new User
        {
            Id = "doctor-user-1",
            Email = dto.Email,
            UserName = dto.Email
        };

        var doctor = new Doctor
        {
            DoctorId = 10,
            UserId = user.Id
        };

        _userManagerMock
            .Setup(u => u.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(u => u.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Doctor" });

        _doctorRepoMock
            .Setup(r => r.GetByUserIdAsync(user.Id))
            .ReturnsAsync(doctor);

        _jwtServiceMock
            .Setup(j => j.GenerateToken(user, null, doctor.DoctorId))
            .ReturnsAsync("doctor-token");

        // Act
        var result = await _service.LoginAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("doctor-token", result.AccessToken);
        Assert.Equal("Doctor", result.Role);
    }

    [Fact]
    public async Task LoginAsync_ReturnsAdminToken_WhenAdminLoginIsValid()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "admin@test.com",
            Password = "Password@123"
        };

        var user = new User
        {
            Id = "admin-user-1",
            Email = dto.Email,
            UserName = dto.Email
        };

        _userManagerMock
            .Setup(u => u.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(u => u.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Admin" });

        _jwtServiceMock
            .Setup(j => j.GenerateToken(user, null, null))
            .ReturnsAsync("admin-token");

        // Act
        var result = await _service.LoginAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("admin-token", result.AccessToken);
        Assert.Equal("Admin", result.Role);
    }

    [Fact]
    public async Task LoginAsync_ThrowsUnauthorizedAccessException_WhenUserNotFound()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "missing@test.com",
            Password = "Password@123"
        };

        _userManagerMock
            .Setup(u => u.FindByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _service.LoginAsync(dto));

        // Assert
        Assert.Equal("Invalid email or password", exception.Message);
    }

    [Fact]
    public async Task LoginAsync_ThrowsUnauthorizedAccessException_WhenPasswordIsInvalid()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "patient@test.com",
            Password = "WrongPassword"
        };

        var user = new User
        {
            Id = "user-1",
            Email = dto.Email,
            UserName = dto.Email
        };

        _userManagerMock
            .Setup(u => u.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(u => u.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(false);

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _service.LoginAsync(dto));

        // Assert
        Assert.Equal("Invalid email or password", exception.Message);
    }

    [Fact]
    public async Task LoginAsync_ThrowsInvalidOperationException_WhenRoleIsNotAssigned()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "user@test.com",
            Password = "Password@123"
        };

        var user = new User
        {
            Id = "user-1",
            Email = dto.Email,
            UserName = dto.Email
        };

        _userManagerMock
            .Setup(u => u.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(u => u.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string>());

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.LoginAsync(dto));

        // Assert
        Assert.Equal("Role is not assigned.", exception.Message);
    }

    [Fact]
    public async Task LoginAsync_ThrowsInvalidOperationException_WhenPatientRecordNotFound()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "patient@test.com",
            Password = "Password@123"
        };

        var user = new User
        {
            Id = "patient-user-1",
            Email = dto.Email,
            UserName = dto.Email
        };

        _userManagerMock
            .Setup(u => u.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(u => u.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Patient" });

        _patientRepoMock
            .Setup(r => r.GetByUserIdAsync(user.Id))
            .ReturnsAsync((Patient?)null);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.LoginAsync(dto));

        // Assert
        Assert.Equal("Patient record not found.", exception.Message);
    }

    [Fact]
    public async Task LoginAsync_ThrowsInvalidOperationException_WhenDoctorRecordNotFound()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "doctor@test.com",
            Password = "Password@123"
        };

        var user = new User
        {
            Id = "doctor-user-1",
            Email = dto.Email,
            UserName = dto.Email
        };

        _userManagerMock
            .Setup(u => u.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(u => u.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Doctor" });

        _doctorRepoMock
            .Setup(r => r.GetByUserIdAsync(user.Id))
            .ReturnsAsync((Doctor?)null);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.LoginAsync(dto));

        // Assert
        Assert.Equal("Doctor record not found.", exception.Message);
    }

    [Fact]
    public async Task LoginAsync_ThrowsInvalidOperationException_WhenRoleIsInvalid()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "user@test.com",
            Password = "Password@123"
        };

        var user = new User
        {
            Id = "user-1",
            Email = dto.Email,
            UserName = dto.Email
        };

        _userManagerMock
            .Setup(u => u.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(u => u.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Receptionist" });

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.LoginAsync(dto));

        // Assert
        Assert.Equal("Invalid role.", exception.Message);
    }

    [Fact]
    public async Task ChangePasswordAsync_ChangesPassword_WhenValid()
    {
        // Arrange
        var userId = "user-1";

        var dto = new ChangePasswordDto
        {
            CurrentPassword = "OldPassword@123",
            NewPassword = "NewPassword@123"
        };

        var user = new User
        {
            Id = userId,
            Email = "user@test.com"
        };

        _userManagerMock
            .Setup(u => u.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(u => u.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _service.ChangePasswordAsync(userId, dto);

        // Assert
        _userManagerMock.Verify(
            u => u.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword),
            Times.Once
        );
    }

    [Fact]
    public async Task ChangePasswordAsync_Throws_WhenUserNotFound()
    {
        // Arrange
        var dto = new ChangePasswordDto
        {
            CurrentPassword = "OldPassword@123",
            NewPassword = "NewPassword@123"
        };

        _userManagerMock
            .Setup(u => u.FindByIdAsync("missing-user"))
            .ReturnsAsync((User?)null);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.ChangePasswordAsync("missing-user", dto));

        // Assert
        Assert.Equal("User not found", exception.Message);
    }

    [Fact]
    public async Task ChangePasswordAsync_Throws_WhenChangePasswordFails()
    {
        // Arrange
        var userId = "user-1";

        var dto = new ChangePasswordDto
        {
            CurrentPassword = "WrongOldPassword",
            NewPassword = "NewPassword@123"
        };

        var user = new User
        {
            Id = userId,
            Email = "user@test.com"
        };

        _userManagerMock
            .Setup(u => u.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(u => u.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword))
            .ReturnsAsync(IdentityResult.Failed(
                new IdentityError
                {
                    Description = "Incorrect password"
                }
            ));

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.ChangePasswordAsync(userId, dto));

        // Assert
        Assert.Contains("Incorrect password", exception.Message);
    }
}