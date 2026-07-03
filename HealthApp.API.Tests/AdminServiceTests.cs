using AutoMapper;
using HealthApp.API.Exceptions;
using HealthApp.API.Identity;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Collections;
using System.Linq.Expressions;
using Xunit;

namespace HealthApp.API.Tests;

public class AdminServiceTests
{
    private readonly Mock<IDoctorRepository> doctorRepositoryMock = new();
    private readonly Mock<IPatientRepository> patientRepositoryMock = new();
    private readonly Mock<IAppointmentRepository> appointmentRepositoryMock = new();
    private readonly Mock<IMapper> mapperMock = new();
    private readonly Mock<UserManager<ApplicationUser>> userManagerMock;

    private readonly AdminService adminService;

    public AdminServiceTests()
    {
        userManagerMock = MockUserManager();

        adminService = new AdminService(
            doctorRepositoryMock.Object,
            patientRepositoryMock.Object,
            appointmentRepositoryMock.Object,
            userManagerMock.Object,
            mapperMock.Object);
    }

    [Fact]
    public async Task GetDoctorsAsync_ShouldReturnPagedDoctorDtos()
    {
        var pagination = new PaginationQueryDto
        {
            PageNumber = 1,
            PageSize = 5
        };

        var doctors = new List<Doctor>
    {
        new()
        {
            DoctorId = 1,
            DoctorName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist.ToString(),
            YearsOfExperience = 5,
            ConsultationFee = 800,
            IsActive = true
        }
    };

        var doctorDtos = new List<DoctorDto>
    {
        new()
        {
            DoctorId = 1,
            FullName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist,
            YearsOfExperience = 5,
            ConsultationFee = 800,
            IsActive = true
        }
    };

        doctorRepositoryMock
            .Setup(repository => repository.GetPagedAsync(1, 5))
            .ReturnsAsync((doctors, 1));

        mapperMock
            .Setup(mapper => mapper.Map<List<DoctorDto>>(doctors))
            .Returns(doctorDtos);

        var result = await adminService.GetDoctorsAsync(pagination);

        Assert.Single(result.Items);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Dr Sneha Paul", result.Items[0].FullName);
    }

    [Fact]
    public async Task CreateDoctorAsync_ShouldThrowBusinessRuleException_WhenDtoIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            adminService.CreateDoctorAsync(null!));
    }

    [Fact]
    public async Task CreateDoctorAsync_ShouldThrowConflictException_WhenEmailAlreadyExists()
    {
        // Arrange
        var dto = CreateValidCreateDoctorDto();

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync(new ApplicationUser
            {
                Id = "existing-user-id",
                Email = dto.Email
            });

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() =>
            adminService.CreateDoctorAsync(dto));
    }

    [Fact]
    public async Task CreateDoctorAsync_ShouldCreateDoctorUserAndDoctorRecord()
    {
        var dto = CreateValidCreateDoctorDto();

        var doctor = new Doctor
        {
            DoctorId = 10,
            UserId = "doctor-user-id",
            DoctorName = dto.FullName,
            Specialisation = dto.Specialisation.ToString(),
            YearsOfExperience = 5,
            ConsultationFee = dto.ConsultationFee,
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        var savedDoctor = new Doctor
        {
            DoctorId = 10,
            UserId = "doctor-user-id",
            DoctorName = dto.FullName,
            Specialisation = dto.Specialisation.ToString(),
            YearsOfExperience = 5,
            ConsultationFee = dto.ConsultationFee,
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        var doctorDto = new DoctorDto
        {
            DoctorId = 10,
            FullName = dto.FullName,
            Specialisation = dto.Specialisation,
            YearsOfExperience = 5,
            ConsultationFee = dto.ConsultationFee,
            IsActive = true
        };

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync((ApplicationUser?)null);

        userManagerMock
            .Setup(manager => manager.CreateAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>()))
            .Callback<ApplicationUser, string>((user, _) =>
            {
                user.Id = "doctor-user-id";
            })
            .ReturnsAsync(IdentityResult.Success);

        userManagerMock
            .Setup(manager => manager.AddToRoleAsync(
                It.IsAny<ApplicationUser>(),
                Roles.Doctor))
            .ReturnsAsync(IdentityResult.Success);

        mapperMock
            .Setup(mapper => mapper.Map<Doctor>(dto))
            .Returns(doctor);

        doctorRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Doctor>()))
            .ReturnsAsync(savedDoctor);

        mapperMock
            .Setup(mapper => mapper.Map<DoctorDto>(savedDoctor))
            .Returns(doctorDto);

        var result = await adminService.CreateDoctorAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("Doctor registered successfully. Share the temporary password securely.", result.Message);
        Assert.False(string.IsNullOrWhiteSpace(result.TemporaryPassword));
        Assert.Equal(10, result.Doctor.DoctorId);
        Assert.Equal(dto.FullName, result.Doctor.FullName);

        userManagerMock.Verify(
            manager => manager.CreateAsync(
                It.Is<ApplicationUser>(user =>
                    user.Email == dto.Email &&
                    user.UserName == dto.Email &&
                    user.FullName == dto.FullName &&
                    user.MustChangePassword),
                It.IsAny<string>()),
            Times.Once);

        userManagerMock.Verify(
            manager => manager.AddToRoleAsync(
                It.IsAny<ApplicationUser>(),
                Roles.Doctor),
            Times.Once);

        doctorRepositoryMock.Verify(
            repository => repository.AddAsync(It.Is<Doctor>(doctorEntity =>
                doctorEntity.UserId == "doctor-user-id" &&
                doctorEntity.IsActive &&
                doctorEntity.DoctorName == dto.FullName &&
                doctorEntity.Specialisation == dto.Specialisation.ToString())),
            Times.Once);
    }

    [Fact]
    public async Task CreateDoctorAsync_ShouldDeleteCreatedUser_WhenRoleAssignmentFails()
    {
        // Arrange
        var dto = CreateValidCreateDoctorDto();

        userManagerMock
            .Setup(manager => manager.FindByEmailAsync(dto.Email))
            .ReturnsAsync((ApplicationUser?)null);

        userManagerMock
            .Setup(manager => manager.CreateAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        userManagerMock
            .Setup(manager => manager.AddToRoleAsync(
                It.IsAny<ApplicationUser>(),
                Roles.Doctor))
            .ReturnsAsync(IdentityResult.Failed(
                new IdentityError
                {
                    Description = "Role assignment failed."
                }));

        userManagerMock
            .Setup(manager => manager.DeleteAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            adminService.CreateDoctorAsync(dto));

        userManagerMock.Verify(
            manager => manager.DeleteAsync(It.IsAny<ApplicationUser>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateDoctorAsync_ShouldThrowBusinessRuleException_WhenDtoIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            adminService.UpdateDoctorAsync(1, null!));
    }

    [Fact]
    public async Task UpdateDoctorAsync_ShouldThrowBusinessRuleException_WhenDoctorIdInvalid()
    {
        // Arrange
        var dto = CreateValidUpdateDoctorDto();

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            adminService.UpdateDoctorAsync(0, dto));
    }

    [Fact]
    public async Task UpdateDoctorAsync_ShouldThrowEntityNotFoundException_WhenDoctorNotFound()
    {
        // Arrange
        var dto = CreateValidUpdateDoctorDto();

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync((Doctor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            adminService.UpdateDoctorAsync(1, dto));
    }

    [Fact]
    public async Task UpdateDoctorAsync_ShouldUpdateDoctor()
    {
        var dto = CreateValidUpdateDoctorDto();

        var existingDoctor = new Doctor
        {
            DoctorId = 1,
            UserId = "doctor-user-id",
            CreatedDate = new DateTime(2024, 1, 1),
            DoctorName = "Old Name",
            Specialisation = SpecialisationType.Cardiologist.ToString(),
            YearsOfExperience = 4,
            ConsultationFee = 500,
            IsActive = true
        };

        var mappedDoctor = new Doctor
        {
            DoctorName = dto.FullName,
            Specialisation = dto.Specialisation.ToString(),
            YearsOfExperience = 6,
            ConsultationFee = dto.ConsultationFee,
            IsActive = true
        };

        var updatedDoctor = new Doctor
        {
            DoctorId = 1,
            UserId = existingDoctor.UserId,
            CreatedDate = existingDoctor.CreatedDate,
            DoctorName = dto.FullName,
            Specialisation = dto.Specialisation.ToString(),
            YearsOfExperience = 6,
            ConsultationFee = dto.ConsultationFee,
            IsActive = true
        };

        var doctorDto = new DoctorDto
        {
            DoctorId = 1,
            FullName = dto.FullName,
            Specialisation = dto.Specialisation,
            YearsOfExperience = 6,
            ConsultationFee = dto.ConsultationFee,
            IsActive = true
        };

        doctorRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(existingDoctor);

        mapperMock
            .Setup(mapper => mapper.Map<Doctor>(dto))
            .Returns(mappedDoctor);

        doctorRepositoryMock
            .Setup(repository => repository.UpdateAsync(1, It.IsAny<Doctor>()))
            .ReturnsAsync(updatedDoctor);

        mapperMock
            .Setup(mapper => mapper.Map<DoctorDto>(updatedDoctor))
            .Returns(doctorDto);

        var result = await adminService.UpdateDoctorAsync(1, dto);

        Assert.Equal(1, result.DoctorId);
        Assert.Equal(dto.FullName, result.FullName);

        doctorRepositoryMock.Verify(repository =>
            repository.UpdateAsync(1, It.Is<Doctor>(doctorEntity =>
                doctorEntity.DoctorId == 1 &&
                doctorEntity.UserId == existingDoctor.UserId &&
                doctorEntity.CreatedDate == existingDoctor.CreatedDate &&
                doctorEntity.DoctorName == dto.FullName &&
                doctorEntity.Specialisation == dto.Specialisation.ToString())),
            Times.Once);
    }

    [Fact]
    public async Task GetAppointmentReportsAsync_ShouldReturnGroupedPagedReports()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            new()
            {
                AppointmentId = 1,
                ScheduledDate = new DateTime(2026, 7, 1),
                Status = AppointmentStatus.Pending.ToString()
            },
            new()
            {
                AppointmentId = 2,
                ScheduledDate = new DateTime(2026, 7, 1),
                Status = AppointmentStatus.Completed.ToString()
            },
            new()
            {
                AppointmentId = 3,
                ScheduledDate = new DateTime(2026, 7, 2),
                Status = AppointmentStatus.Cancelled.ToString()
            }
        };

        appointmentRepositoryMock
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(appointments);

        var pagination = new PaginationQueryDto
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await adminService.GetAppointmentReportsAsync(pagination);

        // Assert
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Count);

        var latestReport = result.Items.First();

        Assert.Equal(new DateTime(2026, 7, 2), latestReport.Date.Date);
        Assert.Equal(1, latestReport.Cancelled);
    }

    [Fact]
    public async Task GetUsersAsync_ShouldThrowBusinessRuleException_WhenRoleInvalid()
    {
        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            adminService.GetUsersAsync("InvalidRole"));
    }

    [Fact]
    public async Task GetUsersAsync_ShouldReturnUsersByRole_WhenRoleProvided()
    {
        // Arrange
        var users = new List<ApplicationUser>
        {
            new()
            {
                Id = "user-1",
                Email = "admin@healthapp.com",
                FullName = "Admin User"
            }
        };

        userManagerMock
            .Setup(manager => manager.GetUsersInRoleAsync(Roles.Admin))
            .ReturnsAsync(users);

        // Act
        var result = await adminService.GetUsersAsync(Roles.Admin);

        // Assert
        Assert.Single(result);
        Assert.Equal("admin@healthapp.com", result[0].Email);
        Assert.Equal(Roles.Admin, result[0].Role);
    }

    [Fact]
    public async Task GetPatientsAsync_ShouldThrowBusinessRuleException_WhenGenderInvalid()
    {
        // Arrange
        var invalidGender = (GenderType)999;

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            adminService.GetPatientsAsync(gender: invalidGender));
    }

    [Fact]
    public async Task GetPatientsAsync_ShouldReturnPagedPatients()
    {
        // Arrange
        var pagination = new PaginationQueryDto
        {
            PageNumber = 1,
            PageSize = 5
        };

        var patients = new List<Patient>
        {
            new()
            {
                PatientId = 1,
                PatientName = "Kevin Baby",
                Email = "kevin@gmail.com",
                Gender = GenderType.Male.ToString()
            }
        };

        var patientDtos = new List<PatientDto>
        {
            new()
            {
                PatientId = 1,
                FullName = "Kevin Baby",
                Email = "kevin@gmail.com",
                Gender = GenderType.Male
            }
        };

        patientRepositoryMock
            .Setup(repository => repository.GetFilteredPagedAsync(
                null,
                null,
                null,
                1,
                5))
            .ReturnsAsync((patients, 1));

        mapperMock
            .Setup(mapper => mapper.Map<List<PatientDto>>(patients))
            .Returns(patientDtos);

        // Act
        var result = await adminService.GetPatientsAsync(
            pagination: pagination);

        // Assert
        Assert.Single(result.Items);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Kevin Baby", result.Items[0].FullName);
    }

    private static CreateDoctorDto CreateValidCreateDoctorDto()
    {
        return new CreateDoctorDto
        {
            FullName = "Dr Sneha Paul",
            Email = "sneha.paul@healthapp.com",
            Specialisation = SpecialisationType.Cardiologist,
            PracticeStartDate = DateTime.Today.AddYears(-5),
            ConsultationFee = 800
        };
    }

    private static UpdateDoctorDto CreateValidUpdateDoctorDto()
    {
        return new UpdateDoctorDto
        {
            FullName = "Dr Sneha Paul",
            Specialisation = SpecialisationType.Cardiologist,
            PracticeStartDate = DateTime.Today.AddYears(-6),
            ConsultationFee = 900
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
}