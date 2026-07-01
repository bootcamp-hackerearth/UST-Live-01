using AutoMapper;
using FluentAssertions;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services;
using HealthCareApp.Shared.Dtos.Doctors;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareApp.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;

        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _repositoryMock = new Mock<IDoctorRepository>();

            _mapperMock = new Mock<IMapper>();

            _userManagerMock = MockUserManager();

            _roleManagerMock = MockRoleManager();

            _service = new DoctorService(
                _repositoryMock.Object,
                _mapperMock.Object,
                _userManagerMock.Object,
                _roleManagerMock.Object);
        }

        private Mock<UserManager<IdentityUser>> MockUserManager()
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

        private Mock<RoleManager<IdentityRole>> MockRoleManager()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();

            return new Mock<RoleManager<IdentityRole>>(
                store.Object,
                null!,
                null!,
                null!,
                null!);
        }

        [Fact]
        public async Task GetAllDoctorsAsync_ShouldReturnDoctorDtos()
        {
            var doctors = DoctorTestData.Doctors;
            var dtos = DoctorTestData.DoctorDtos;

            _repositoryMock
                .Setup(r => r.GetAllAsync(default))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(m => m.Map<List<DoctorDto>>(doctors))
                .Returns(dtos);

            var result = await _service.GetAllDoctorsAsync();

            result.Should().HaveCount(2);

            result.Should().BeEquivalentTo(dtos);

            _repositoryMock.Verify(x => x.GetAllAsync(default), Times.Once);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_ShouldReturnDoctor()
        {
            var doctor = DoctorTestData.Doctor;

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, default))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(m => m.Map<DoctorDto>(doctor))
                .Returns(DoctorTestData.DoctorDto);

            var result = await _service.GetDoctorByIdAsync(1);

            result.DoctorId.Should().Be(1);

            result.FullName.Should().Be("John Smith");
        }

        [Fact]
        public async Task GetDoctorByIdAsync_InvalidId_ShouldThrow()
        {
            Func<Task> action = async () =>
                await _service.GetDoctorByIdAsync(0);

            await action.Should()
                .ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task GetDoctorByIdAsync_NotFound_ShouldThrow()
        {
            _repositoryMock
                .Setup(r => r.GetByIdAsync(10, default))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await _service.GetDoctorByIdAsync(10);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetAllActiveDoctors_ShouldReturnOnlyActiveDoctors()
        {
            var doctors = DoctorTestData.Doctors.Where(x => x.IsActive).ToList();

            var dtos = DoctorTestData.DoctorDtos.Where(x => x.IsActive).ToList();

            _repositoryMock
                .Setup(r => r.GetAllActiveAsync(default))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(m => m.Map<List<DoctorDto>>(doctors))
                .Returns(dtos);

            var result = await _service.GetAllActiveDoctorsAsync();

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetDoctorsBySpecialisation_ShouldReturnDoctors()
        {
            var doctors = new List<Doctor>
        {
            DoctorTestData.Doctor
        };

            var dtos = new List<DoctorDto>
        {
            DoctorTestData.DoctorDto
        };

            _repositoryMock
                .Setup(r => r.GetBySpecialisationAsync(
                    DoctorTestData.Doctor.Specialisation,
                    default))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(m => m.Map<List<DoctorDto>>(doctors))
                .Returns(dtos);

            var result = await _service.GetDoctorsBySpecialisationAsync(
                DoctorTestData.Doctor.Specialisation);

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetDoctorAvailability_ShouldReturnTimeSlots()
        {
            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, default))
                .ReturnsAsync(DoctorTestData.Doctor);

            var result = await _service.GetDoctorAvailabilityAsync(1);

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetDoctorAvailability_InactiveDoctor_ShouldThrow()
        {
            var doctor = DoctorTestData.Doctors[1];

            _repositoryMock
                .Setup(r => r.GetByIdAsync(2, default))
                .ReturnsAsync(doctor);

            Func<Task> action = async () =>
                await _service.GetDoctorAvailabilityAsync(2);

            await action.Should()
                .ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task GetMyProfile_ShouldReturnDoctor()
        {
            _repositoryMock
                .Setup(r => r.GetByIdentityUserIdAsync("doctor1", default))
                .ReturnsAsync(DoctorTestData.Doctor);

            _mapperMock
                .Setup(m => m.Map<DoctorDto>(DoctorTestData.Doctor))
                .Returns(DoctorTestData.DoctorDto);

            var result = await _service.GetMyProfileAsync("doctor1");

            result.Should().NotBeNull();

            result.DoctorId.Should().Be(1);
        }

        [Fact]
        public async Task UpdateDoctorAsync_ShouldReturnUpdatedDoctor()
        {
            var existingDoctor = DoctorTestData.Doctor;

            var updateDto = DoctorTestData.UpdateDoctorDto;

            var mappedDoctor = new Doctor
            {
                DoctorId = 1,
                DoctorName = updateDto.FullName,
                ConsultationFee = (int)updateDto.ConsultationFee,
                Specialisation = updateDto.Specialisation,
                YearsOfExperience = 10
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, default))
                .ReturnsAsync(existingDoctor);

            _mapperMock
                .Setup(m => m.Map<Doctor>(updateDto))
                .Returns(mappedDoctor);

            _repositoryMock
                .Setup(r => r.UpdateAsync(1, It.IsAny<Doctor>(), default))
                .ReturnsAsync(mappedDoctor);

            _mapperMock
                .Setup(m => m.Map<DoctorDto>(mappedDoctor))
                .Returns(new DoctorDto
                {
                    DoctorId = 1,
                    FullName = updateDto.FullName
                });

            var result = await _service.UpdateDoctorAsync(1, updateDto);

            result.FullName.Should().Be(updateDto.FullName);

            _repositoryMock.Verify(r =>
                r.UpdateAsync(1, It.IsAny<Doctor>(), default),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorAsync_DoctorNotFound_ShouldThrow()
        {
            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, default))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await _service.UpdateDoctorAsync(1, DoctorTestData.UpdateDoctorDto);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdateDoctorAsync_InvalidId_ShouldThrow()
        {
            Func<Task> action = async () =>
                await _service.UpdateDoctorAsync(0,
                    DoctorTestData.UpdateDoctorDto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task GetAllDoctorsPagedAsync_ShouldReturnPagedDoctors()
        {
            var doctors = DoctorTestData.Doctors;

            _repositoryMock
                .Setup(r => r.GetAllAsync(default))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(m => m.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
                .Returns(DoctorTestData.DoctorDtos);

            var query = new DoctorPaginationQueryDto
            {
                PageNumber = 1,
                PageSize = 10
            };

            var result = await _service.GetAllDoctorsPagedAsync(query);

            result.Should().NotBeNull();

            result.Items.Should().HaveCount(2);

            result.TotalRecords.Should().Be(2);

            result.PageNumber.Should().Be(1);
        }

        [Fact]
        public async Task GetAllDoctorsPagedAsync_SearchFilter_ShouldReturnSingleDoctor()
        {
            _repositoryMock
                .Setup(r => r.GetAllAsync(default))
                .ReturnsAsync(DoctorTestData.Doctors);

            _mapperMock
                .Setup(m => m.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<DoctorDto>
                {
            DoctorTestData.DoctorDto
                });

            var query = new DoctorPaginationQueryDto
            {
                SearchTerm = "John"
            };

            var result = await _service.GetAllDoctorsPagedAsync(query);

            result.TotalRecords.Should().Be(1);

            result.Items.Should().HaveCount(1);
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_ShouldCreateDoctor()
        {
            var dto = DoctorTestData.CreateDoctorDto;

            _repositoryMock
                .Setup(r => r.ExistsByEmailAsync(dto.Email.ToLower(), default))
                .ReturnsAsync(false);

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(dto.Email.ToLower()))
                .ReturnsAsync((IdentityUser?)null);

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _roleManagerMock
                .Setup(x => x.RoleExistsAsync("Doctor"))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<IdentityUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            _mapperMock
                .Setup(x => x.Map<Doctor>(dto))
                .Returns(new Doctor());

            _repositoryMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<Doctor>(),
                    default))
                .ReturnsAsync(new Doctor
                {
                    DoctorId = 5,
                    DoctorName = dto.FullName,
                    Email = dto.Email
                });

            var result = await _service.CreateDoctorByAdminAsync(dto);

            result.DoctorId.Should().Be(5);

            result.DoctorName.Should().Be(dto.FullName);
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_EmailExists_ShouldThrow()
        {
            _repositoryMock
                .Setup(r => r.ExistsByEmailAsync(
                    It.IsAny<string>(),
                    default))
                .ReturnsAsync(true);

            Func<Task> action = async () =>
                await _service.CreateDoctorByAdminAsync(
                    DoctorTestData.CreateDoctorDto);

            await action.Should()
                .ThrowAsync<ConflictException>();
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_IdentityUserExists_ShouldThrow()
        {
            var dto = DoctorTestData.CreateDoctorDto;

            _repositoryMock
                .Setup(r => r.ExistsByEmailAsync(dto.Email.ToLower(), default))
                .ReturnsAsync(false);

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(dto.Email.ToLower()))
                .ReturnsAsync(new IdentityUser());

            Func<Task> action = async () =>
                await _service.CreateDoctorByAdminAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>();
        }
    }
}

