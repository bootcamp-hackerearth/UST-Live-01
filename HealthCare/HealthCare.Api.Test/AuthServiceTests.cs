using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
using HealthCare.Shared.DTOs.Authentication;
using HealthCare.Shared.DTOs.Doctor;
using HealthCare.Shared.DTOs.Patient;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;



namespace HealthCare.Api.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPatientRepository> _patientRepoMock;
        private readonly Mock<IDoctorRepository> _doctorRepoMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly HealthCareDbContext _context;
        private readonly AuthService _service;
        private readonly Mock<ILogger<AuthService>> _loggerMock;

        public AuthServiceTests()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!
            );

            _mapperMock = new Mock<IMapper>();
            _patientRepoMock = new Mock<IPatientRepository>();
            _doctorRepoMock = new Mock<IDoctorRepository>();
            _jwtServiceMock = new Mock<IJwtService>();
            _loggerMock = new Mock<ILogger<AuthService>>();

            var options = new DbContextOptionsBuilder<HealthCareDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
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

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenRoleNotAssigned()
        {
            var user = new IdentityUser
            {
                Id = "1",
                Email = "test@test.com"
            };

            _userManagerMock.Setup(x =>
                x.FindByEmailAsync(user.Email!))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x =>
                x.CheckPasswordAsync(user, It.IsAny<string>()))
                .ReturnsAsync(true);

            _userManagerMock.Setup(x =>
                x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.LoginAsync(new LoginDto
                {
                    Email = user.Email!,
                    Password = "Password"
                }));
        }

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenPatientNotFound()
        {
            var user = new IdentityUser
            {
                Id = "1",
                Email = "patient@test.com"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email!))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>()))
                .ReturnsAsync(true);

            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Patient" });

            _patientRepoMock.Setup(x =>
                x.GetByUserIdAsync(user.Id))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.LoginAsync(new LoginDto
                {
                    Email = user.Email!,
                    Password = "Password"
                }));
        }

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenDoctorNotFound()
        {
            var user = new IdentityUser
            {
                Id = "1",
                Email = "doctor@test.com"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email!))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>()))
                .ReturnsAsync(true);

            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Doctor" });

            _doctorRepoMock.Setup(x =>
                x.GetByUserIdAsync(user.Id))
                .ReturnsAsync((Doctor)null!);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.LoginAsync(new LoginDto
                {
                    Email = user.Email!,
                    Password = "Password"
                }));
        }

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenRoleInvalid()
        {
            var user = new IdentityUser
            {
                Id = "1",
                Email = "user@test.com"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email!))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>()))
                .ReturnsAsync(true);

            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Manager" });

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.LoginAsync(new LoginDto
                {
                    Email = user.Email!,
                    Password = "Password"
                }));
        }



        [Fact]
        public async Task RegisterPatientAsync_ShouldCreateUserAndPatient()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                Email = "test@mail.com",
                Password = "Password123!"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser?)null);

            _userManagerMock.Setup(x =>
                x.CreateAsync(It.IsAny<IdentityUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(x =>
                x.AddToRoleAsync(It.IsAny<IdentityUser>(), "Patient"))
                .ReturnsAsync(IdentityResult.Success);

            _mapperMock.Setup(x => x.Map<Patient>(dto))
                .Returns(new Patient());

            // Act
            await _service.RegisterPatientAsync(dto);

            // Assert
            _patientRepoMock.Verify(
                x => x.AddAsync(It.IsAny<Patient>()),
                Times.Once);
        }

        [Fact]
        public async Task RegisterDoctorAsync_ShouldCreateDoctorAndSlots()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                Email = "doc@mail.com",
                Password = "Password123!",
                TimeSlots = new List<string>
        {
            "09:00-10:00"
        }
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser?)null);

            _userManagerMock.Setup(x =>
                x.CreateAsync(It.IsAny<IdentityUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(x =>
                x.AddToRoleAsync(It.IsAny<IdentityUser>(), "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            var doctor = new Doctor
            {
                DoctorId = 1
            };

            _mapperMock.Setup(x => x.Map<Doctor>(dto))
                .Returns(doctor);

            // Act
            await _service.RegisterDoctorAsync(dto);

            // Assert
            _doctorRepoMock.Verify(
                x => x.AddAsync(doctor),
                Times.Once);

            _doctorRepoMock.Verify(
                x => x.CreateSlots(
                    doctor.DoctorId,
                    dto.TimeSlots),
                Times.Once);
        }

        [Fact]
        public async Task RegisterPatientAsync_ShouldThrow_WhenEmailExists()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                Email = "test@mail.com",
                Password = "Password123!"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(new IdentityUser
                {
                    Id = "1",
                    Email = dto.Email,
                    UserName = dto.Email
                });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.RegisterPatientAsync(dto));
        }

        [Fact]
        public async Task RegisterDoctorAsync_ShouldThrow_WhenEmailExists()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                Email = "doc@mail.com",
                Password = "Password123!",
                TimeSlots = new List<string>()
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(new IdentityUser
                {
                    Id = "1",
                    Email = dto.Email,
                    UserName = dto.Email
                });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.RegisterDoctorAsync(dto));
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldThrow_WhenUserNotFound()
        {
            _userManagerMock.Setup(x => x.FindByIdAsync("1"))
                .ReturnsAsync((IdentityUser?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.ChangePasswordAsync("1",
                    new ChangePasswordDto
                    {
                        CurrentPassword = "old",
                        NewPassword = "new"
                    }));
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldThrow_WhenChangeFails()
        {
            var user = new IdentityUser
            {
                Id = "1"
            };

            _userManagerMock.Setup(x =>
                x.FindByIdAsync("1"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x =>
                x.ChangePasswordAsync(
                    user,
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        new IdentityError
                        {
                            Description = "Password error"
                        }));

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.ChangePasswordAsync("1",
                    new ChangePasswordDto
                    {
                        CurrentPassword = "Old",
                        NewPassword = "New"
                    }));
        }

        [Fact]
        public async Task UpdatePatientEmailAsync_ShouldThrow_WhenUserNotFound()
        {
            _userManagerMock.Setup(x =>
                x.FindByIdAsync("1"))
                .ReturnsAsync((IdentityUser)null!);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdatePatientEmailAsync(
                    "1",
                    "new@test.com"));
        }

        [Fact]
        public async Task UpdatePatientEmailAsync_ShouldThrow_WhenEmailExists()
        {
            var user = new IdentityUser
            {
                Id = "1"
            };

            var existing = new IdentityUser
            {
                Id = "2",
                Email = "new@test.com"
            };

            _userManagerMock.Setup(x =>
                x.FindByIdAsync("1"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x =>
                x.FindByEmailAsync("new@test.com"))
                .ReturnsAsync(existing);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdatePatientEmailAsync(
                    "1",
                    "new@test.com"));
        }

        [Fact]
        public async Task UpdatePatientEmailAsync_ShouldThrow_WhenUpdateFails()
        {
            var user = new IdentityUser
            {
                Id = "1"
            };

            _userManagerMock.Setup(x =>
                x.FindByIdAsync("1"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x =>
                x.FindByEmailAsync("new@test.com"))
                .ReturnsAsync((IdentityUser)null!);

            _userManagerMock.Setup(x =>
                x.UpdateAsync(user))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        new IdentityError
                        {
                            Description = "Update failed"
                        }));

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdatePatientEmailAsync(
                    "1",
                    "new@test.com"));
        }




        //  Login - patient
        [Fact]
        public async Task LoginAsync_ShouldReturnToken_ForPatient()
        {
            var user = new IdentityUser { Id = "1", Email = "test@mail.com" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "Password123!"))
                .ReturnsAsync(true);

            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Patient" });

            _patientRepoMock.Setup(r => r.GetByUserIdAsync(user.Id))
                .ReturnsAsync(new Patient { PatientId = 5 });

            _jwtServiceMock.Setup(j => j.GenerateToken(user, 5, null))
                .ReturnsAsync("token");

            var result = await _service.LoginAsync(new LoginDto
            {
                Email = user.Email,
                Password = "Password123!"
            });

            Assert.Equal("token", result.AccessToken);
            Assert.Equal("Patient", result.Role);
        }

        //  Login - doctor
        [Fact]
        public async Task LoginAsync_ShouldReturnToken_ForDoctor()
        {
            var user = new  IdentityUser { Id = "1", Email = "doc@mail.com" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "Password123!"))
                .ReturnsAsync(true);

            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Doctor" });

            _doctorRepoMock.Setup(r => r.GetByUserIdAsync(user.Id))
                .ReturnsAsync(new Doctor { DoctorId = 3 });

            _jwtServiceMock.Setup(j => j.GenerateToken(user, null, 3))
                .ReturnsAsync("token");

            var result = await _service.LoginAsync(new LoginDto
            {
                Email = user.Email,
                Password = "Password123!"
            });

            Assert.Equal("token", result.AccessToken);
            Assert.Equal("Doctor", result.Role);
        }

        //  Login - invalid password
        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenPasswordInvalid()
        {
            var user = new IdentityUser { Email = "test@mail.com" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "wrong"))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.LoginAsync(new LoginDto { Email = user.Email, Password = "wrong" }));
        }

        //  Login - user not found
        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenUserNotFound()
        {
            _userManagerMock.Setup(u => u.FindByEmailAsync("test@mail.com"))
                .ReturnsAsync((IdentityUser?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.LoginAsync(new LoginDto { Email = "test@mail.com", Password = "pass" }));
        }

        //  ChangePassword
        [Fact]
        public async Task ChangePasswordAsync_ShouldChangePassword()
        {
            var user = new IdentityUser { Id = "1" };

            _userManagerMock.Setup(u => u.FindByIdAsync("1"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.ChangePasswordAsync(user, "old", "new"))
                .ReturnsAsync(IdentityResult.Success);

            await _service.ChangePasswordAsync("1", new ChangePasswordDto
            {
                CurrentPassword = "old",
                NewPassword = "new"
            });


            _userManagerMock.Verify(u => u.FindByIdAsync("1"), Times.Once);

            _userManagerMock.Verify(u =>
                u.ChangePasswordAsync(user, "old", "new"), Times.Once);

        }

        //  ChangePassword - failure
        [Fact]
        public async Task ChangePasswordAsync_ShouldThrow_WhenFails()
        {
            var user = new IdentityUser { Id = "1" };

            _userManagerMock.Setup(u => u.FindByIdAsync("1"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.ChangePasswordAsync(user, "old", "new"))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError { Description = "Error" }
                ));

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.ChangePasswordAsync("1", new ChangePasswordDto
                {
                    CurrentPassword = "old",
                    NewPassword = "new"
                }));
        }
    }
}