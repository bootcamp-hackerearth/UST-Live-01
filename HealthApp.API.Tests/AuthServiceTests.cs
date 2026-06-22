using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using HealthApp.API.Models;
using HealthApp.API.Models.DTOs;
using HealthApp.API.Constants;
using HealthApp.API.Exceptions;
using HealthApp.API.Identity;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;

namespace HealthApp.API.Tests;

public class AuthServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManager;
    private readonly Mock<IRefreshTokenRepository> _refreshRepo;
    private readonly Mock<IPatientRepository> _patientRepo;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();

        _userManager = new Mock<UserManager<ApplicationUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _refreshRepo = new Mock<IRefreshTokenRepository>();
        _patientRepo = new Mock<IPatientRepository>();

        var config = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string>
    {
        {"Jwt:Key", "THIS_IS_A_SUPER_SECRET_KEY_1234567890123456"},
        { "Jwt:Issuer", "test"},
        { "Jwt:Audience", "test"},
        { "Jwt:AccessTokenExpirationMinutes", "10"},
        { "Jwt:RefreshTokenExpirationDays", "1"}
    })
    .Build();

        _service = new AuthService(
            _userManager.Object,
            config,
            _refreshRepo.Object,
            _patientRepo.Object);
    }

    // ✅ REGISTER

    [Fact]
    public async Task RegisterAsync_ShouldThrow_WhenEmailExists()
    {
        var dto = new RegisterRequestDto
        {
            Email = "test@test.com",
            Password = "123",
            Role = Roles.Patient
        };

        _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync(new ApplicationUser());

        await Assert.ThrowsAsync<ConflictException>(() =>
            _service.RegisterAsync(dto));
    }

    [Fact]
    public async Task RegisterAsync_ShouldWork_WhenValid()
    {
        var dto = new RegisterRequestDto
        {
            Email = "test@test.com",
            Password = "123",
            FullName = "Test User",
            Role = Roles.Patient,
            DateOfBirth = System.DateTime.Today.AddYears(-20),
            Gender = Enums.GenderType.Male,
            PhoneNumber = "9657255221"
        };

        _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync((ApplicationUser)null);

        _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), dto.Role))
            .ReturnsAsync(IdentityResult.Success);

        _userManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { dto.Role });

        _patientRepo.Setup(x => x.AddAsync(It.IsAny<Patient>()))
            .ReturnsAsync(new Patient { PatientId = 1 });

        _refreshRepo.Setup(x => x.AddAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.RegisterAsync(dto);

        Assert.NotNull(result);
        Assert.Equal(dto.Email, result.Email);
    }

    // ✅ LOGIN

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenUserNotFound()
    {
        _userManager.Setup(x => x.FindByEmailAsync("a@test.com"))
            .ReturnsAsync((ApplicationUser)null);

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            _service.LoginAsync(new LoginRequestDto
            {
                Email = "a@test.com",
                Password = "123"
            }));
    }

    [Fact]
    public async Task LoginAsync_ShouldWork_WhenValid()
    {
        var user = new ApplicationUser
        {
            Id = "1",
            Email = "a@test.com"
        };

        _userManager.Setup(x => x.FindByEmailAsync(user.Email))
            .ReturnsAsync(user);

        _userManager.Setup(x => x.CheckPasswordAsync(user, "123"))
            .ReturnsAsync(true);

        _userManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { Roles.Patient });

        _refreshRepo.Setup(x => x.AddAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.LoginAsync(new LoginRequestDto
        {
            Email = user.Email,
            Password = "123"
        });

        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
    }

    // ✅ CHANGE PASSWORD

    [Fact]
    public async Task ChangePasswordAsync_ShouldThrow_WhenMismatch()
    {
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            _service.ChangePasswordAsync(new ChangePasswordDto
            {
                Email = "a@test.com",
                CurrentPassword = "old",
                NewPassword = "new",
                ConfirmNewPassword = "wrong"
            }));
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldWork()
    {
        var user = new ApplicationUser
        {
            Email = "a@test.com",
            MustChangePassword = true
        };

        _userManager.Setup(x => x.FindByEmailAsync(user.Email))
            .ReturnsAsync(user);

        _userManager.Setup(x => x.CheckPasswordAsync(user, "old"))
            .ReturnsAsync(true);

        _userManager.Setup(x => x.ChangePasswordAsync(user, "old", "new"))
            .ReturnsAsync(IdentityResult.Success);

        _userManager.Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        await _service.ChangePasswordAsync(new ChangePasswordDto
        {
            Email = user.Email,
            CurrentPassword = "old",
            NewPassword = "new",
            ConfirmNewPassword = "new"
        });

        _userManager.Verify(x => x.ChangePasswordAsync(user, "old", "new"), Times.Once);
    }
}
