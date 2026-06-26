using AutoMapper;
using FluentAssertions;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Implementations;
using HealthAxisCore_Api.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using HealthAxis.Shared.DTOs.Doctor;

namespace HealthAxisCore_Api.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly IMapper _mapper;
        private readonly DoctorService _doctorService;

        public DoctorServiceTests()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _userManagerMock = MockUserManager();
            _mapper = MapperHelper.GetMapper();

            _doctorService = new DoctorService(
                _doctorRepositoryMock.Object,
                _mapper,
                _userManagerMock.Object
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

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDoctors()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    DoctorName = "Rahul",
                    Email = "rahul@test.com",
                    Specialisation = SpecialisationType.Cardiologist,
                    YearsOfExperience = 5,
                    ConsultationFee = 500,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = 2,
                    DoctorName = "Mily",
                    Email = "mily@test.com",
                    Specialisation = SpecialisationType.Dermatologist,
                    YearsOfExperience = 6,
                    ConsultationFee = 1000,
                    IsActive = true
                }
            };

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            // Act
            var result = await _doctorService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().DoctorName.Should().Be("Rahul");
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorExists_ShouldReturnDoctor()
        {
            // Arrange
            var doctor = new Doctor
            {
                DoctorId = 1,
                DoctorName = "Rahul",
                Email = "rahul@test.com",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            // Act
            var result = await _doctorService.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.DoctorId.Should().Be(1);
            result.DoctorName.Should().Be("Rahul");
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            // Act
            var act = async () => await _doctorService.GetByIdAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Doctor not found");
        }

        [Fact]
        public async Task CreateAsync_WhenValidDoctor_ShouldCreateDoctorAndApplicationUser()
        {
            // Arrange
            var dto = new CreateDoctorDTO
            {
                DoctorName = "Rahul",
                Email = "rahul@test.com",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Doctor>()))
                .Returns(Task.CompletedTask)
                .Callback<Doctor>(doctor =>
                {
                    doctor.DoctorId = 1;
                });

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.Is<string>(password => password.StartsWith("Temp@"))))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _doctorService.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.DoctorId.Should().Be(1);
            result.DoctorName.Should().Be(dto.DoctorName);

            _doctorRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Doctor>()),
                Times.Once
            );

            _userManagerMock.Verify(
                manager => manager.CreateAsync(
                    It.Is<ApplicationUser>(user =>
                        user.Email == dto.Email &&
                        user.UserName == dto.Email &&
                        user.Role == "Doctor" &&
                        user.ReferenceId == 1 &&
                        user.IsFirstLogin == true &&
                        user.TemporaryPassword != null &&
                        user.TemporaryPassword.StartsWith("Temp@")
                    ),
                    It.Is<string>(password => password.StartsWith("Temp@"))
                ),
                Times.Once
            );

            _userManagerMock.Verify(
                manager => manager.AddToRoleAsync(
                    It.Is<ApplicationUser>(user => user.Email == dto.Email),
                    "Doctor"
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateAsync_WhenUserCreationFails_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var dto = new CreateDoctorDTO
            {
                DoctorName = "Rahul",
                Email = "rahul@test.com",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Doctor>()))
                .Returns(Task.CompletedTask)
                .Callback<Doctor>(doctor =>
                {
                    doctor.DoctorId = 1;
                });

            var identityError = new IdentityError
            {
                Description = "Password is too weak"
            };

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(identityError));

            // Act
            var act = async () => await _doctorService.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Password is too weak");

            _userManagerMock.Verify(
                manager => manager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()),
                Times.Never
            );
        }

        [Fact]
        public async Task UpdateAsync_WhenDoctorExists_ShouldUpdateDoctorAndReturnTrue()
        {
            // Arrange
            var existingDoctor = new Doctor
            {
                DoctorId = 1,
                DoctorName = "OldName",
                Email = "old@test.com",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };

            var updateDto = new CreateDoctorDTO
            {
                DoctorName = "NewName",
                Email = "new@test.com",
                Specialisation = SpecialisationType.Dermatologist,
                YearsOfExperience = 7,
                ConsultationFee = 800,
                IsActive = false
            };

            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingDoctor);

            _doctorRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Doctor>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _doctorService.UpdateAsync(1, updateDto);

            // Assert
            result.Should().BeTrue();

            existingDoctor.DoctorName.Should().Be("NewName");
            existingDoctor.Email.Should().Be("new@test.com");
            existingDoctor.Specialisation.Should().Be(SpecialisationType.Dermatologist);
            existingDoctor.YearsOfExperience.Should().Be(7);
            existingDoctor.ConsultationFee.Should().Be(800);
            existingDoctor.IsActive.Should().BeFalse();

            _doctorRepositoryMock.Verify(
                repo => repo.UpdateAsync(existingDoctor),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            var updateDto = new CreateDoctorDTO
            {
                DoctorName = "NewName",
                Email = "new@test.com",
                Specialisation = SpecialisationType.Dermatologist,
                YearsOfExperience = 7,
                ConsultationFee = 800,
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            // Act
            var act = async () => await _doctorService.UpdateAsync(1, updateDto);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Doctor not found");

            _doctorRepositoryMock.Verify(
                repo => repo.UpdateAsync(It.IsAny<Doctor>()),
                Times.Never
            );
        }

        [Fact]
        public async Task DeleteAsync_WhenDoctorExists_ShouldDeleteDoctorAndReturnTrue()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(true);

            _doctorRepositoryMock
                .Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _doctorService.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();

            _doctorRepositoryMock.Verify(
                repo => repo.DeleteAsync(1),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(false);

            // Act
            var act = async () => await _doctorService.DeleteAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Doctor not found");

            _doctorRepositoryMock.Verify(
                repo => repo.DeleteAsync(It.IsAny<int>()),
                Times.Never
            );
        }

        [Fact]
        public async Task FilterAsync_ShouldReturnFilteredDoctors()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    DoctorName = "Rahul",
                    Email = "rahul@test.com",
                    Specialisation = SpecialisationType.Cardiologist,
                    YearsOfExperience = 5,
                    ConsultationFee = 500,
                    IsActive = true
                }
            };

            _doctorRepositoryMock
                .Setup(repo => repo.GetDoctors(
                    "Rahul",
                    SpecialisationType.Cardiologist,
                    true))
                .ReturnsAsync(doctors);

            // Act
            var result = await _doctorService.FilterAsync(
                "Rahul",
                SpecialisationType.Cardiologist,
                true
            );

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().DoctorName.Should().Be("Rahul");

            _doctorRepositoryMock.Verify(
                repo => repo.GetDoctors(
                    "Rahul",
                    SpecialisationType.Cardiologist,
                    true),
                Times.Once
            );
        }

        [Fact]
        public async Task SetStatusAsync_WhenDoctorExists_ShouldSetStatusAndReturnTrue()
        {
            // Arrange
            var doctor = new Doctor
            {
                DoctorId = 1,
                DoctorName = "Rahul",
                Email = "rahul@test.com",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(repo => repo.SetStatus(1, false))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _doctorService.SetStatusAsync(1, false);

            // Assert
            result.Should().BeTrue();

            _doctorRepositoryMock.Verify(
                repo => repo.SetStatus(1, false),
                Times.Once
            );
        }

        [Fact]
        public async Task SetStatusAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            // Act
            var act = async () => await _doctorService.SetStatusAsync(1, false);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Doctor not found");

            _doctorRepositoryMock.Verify(
                repo => repo.SetStatus(It.IsAny<int>(), It.IsAny<bool>()),
                Times.Never
            );
        }
    }
}