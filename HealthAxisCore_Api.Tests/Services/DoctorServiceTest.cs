using AutoMapper;
using FluentAssertions;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Doctor;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Implementations;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HealthAxisCore_Api.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILogger<DoctorService>> _loggerMock;

        private readonly DoctorService _doctorService;

        public DoctorServiceTests()
        {
            _repositoryMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();
            _cacheServiceMock = new Mock<ICacheService>();
            _loggerMock = new Mock<ILogger<DoctorService>>();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);

            _doctorService = new DoctorService(
                _repositoryMock.Object,
                _mapperMock.Object,
                _userManagerMock.Object,
                _cacheServiceMock.Object,
                _loggerMock.Object);
        }

        private static List<Doctor> GetSampleDoctors()
        {
            return new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    DoctorName = "John Smith",
                    Email = "john@test.com",
                    Specialisation = SpecialisationType.Cardiologist,
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new Doctor
                {
                    DoctorId = 2,
                    DoctorName = "David Miller",
                    Email = "david@test.com",
                    Specialisation = SpecialisationType.Neurologist,
                    YearsOfExperience = 8,
                    ConsultationFee = 700,
                    IsActive = false,
                    CreatedDate = DateTime.Now
                },
                new Doctor
                {
                    DoctorId = 3,
                    DoctorName = "Anna John",
                    Email = "anna@test.com",
                    Specialisation = SpecialisationType.Cardiologist,
                    YearsOfExperience = 5,
                    ConsultationFee = 450,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                }
            };
        }

        private static List<DoctorResponseDto> GetSampleDoctorResponseDtos()
        {
            return new List<DoctorResponseDto>
            {
                new DoctorResponseDto
                {
                    DoctorId = 1,
                    DoctorName = "John Smith",
                    Email = "john@test.com",
                    Specialisation = SpecialisationType.Cardiologist,
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                },
                new DoctorResponseDto
                {
                    DoctorId = 2,
                    DoctorName = "David Miller",
                    Email = "david@test.com",
                    Specialisation = SpecialisationType.Neurologist,
                    YearsOfExperience = 8,
                    ConsultationFee = 700,
                    IsActive = false
                }
            };
        }

        // -------------------------------------------------------
        // GetPagedAsync Tests
        // -------------------------------------------------------

        [Fact]
        public async Task GetPagedAsync_ShouldReturnPagedDoctors_WhenNoFiltersApplied()
        {
            // Arrange
            var doctors = GetSampleDoctors();

            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns((List<Doctor> source) =>
                    source.Select(d => new DoctorResponseDto
                    {
                        DoctorId = d.DoctorId,
                        DoctorName = d.DoctorName,
                        Email = d.Email,
                        Specialisation = d.Specialisation,
                        IsActive = d.IsActive
                    }).ToList());

            // Act
            var result = await _doctorService.GetPagedAsync(
                pageNumber: 1,
                pageSize: 2,
                search: null,
                specialisation: null,
                status: null);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(3);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(2);
            result.TotalPages.Should().Be(2);

            _repositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldDefaultPageNumberToOne_WhenPageNumberLessThanOne()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(GetSampleDoctors());

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<DoctorResponseDto>());

            // Act
            var result = await _doctorService.GetPagedAsync(
                pageNumber: 0,
                pageSize: 10,
                search: null,
                specialisation: null,
                status: null);

            // Assert
            result.PageNumber.Should().Be(1);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldDefaultPageSizeToTen_WhenPageSizeLessThanOne()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(GetSampleDoctors());

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<DoctorResponseDto>());

            // Act
            var result = await _doctorService.GetPagedAsync(
                pageNumber: 1,
                pageSize: 0,
                search: null,
                specialisation: null,
                status: null);

            // Assert
            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldLimitPageSizeToHundred_WhenPageSizeGreaterThanHundred()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(GetSampleDoctors());

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<DoctorResponseDto>());

            // Act
            var result = await _doctorService.GetPagedAsync(
                pageNumber: 1,
                pageSize: 150,
                search: null,
                specialisation: null,
                status: null);

            // Assert
            result.PageSize.Should().Be(100);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldFilterByDoctorName_WhenSearchProvided()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(GetSampleDoctors());

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns((List<Doctor> source) =>
                    source.Select(d => new DoctorResponseDto
                    {
                        DoctorId = d.DoctorId,
                        DoctorName = d.DoctorName
                    }).ToList());

            // Act
            var result = await _doctorService.GetPagedAsync(
                pageNumber: 1,
                pageSize: 10,
                search: "John",
                specialisation: null,
                status: null);

            // Assert
            result.TotalCount.Should().Be(2);
            result.Items.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldFilterByEmail_WhenSearchProvided()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(GetSampleDoctors());

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns((List<Doctor> source) =>
                    source.Select(d => new DoctorResponseDto
                    {
                        DoctorId = d.DoctorId,
                        Email = d.Email
                    }).ToList());

            // Act
            var result = await _doctorService.GetPagedAsync(
                pageNumber: 1,
                pageSize: 10,
                search: "david@test.com",
                specialisation: null,
                status: null);

            // Assert
            result.TotalCount.Should().Be(1);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldFilterBySpecialisationSearch_WhenSearchProvided()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(GetSampleDoctors());

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns((List<Doctor> source) =>
                    source.Select(d => new DoctorResponseDto
                    {
                        DoctorId = d.DoctorId,
                        Specialisation = d.Specialisation
                    }).ToList());

            // Act
            var result = await _doctorService.GetPagedAsync(
                pageNumber: 1,
                pageSize: 10,
                search: "Cardiology",
                specialisation: null,
                status: null);

            // Assert
            result.TotalCount.Should().Be(2);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldFilterBySpecialisation_WhenSpecialisationProvided()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(GetSampleDoctors());

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns((List<Doctor> source) =>
                    source.Select(d => new DoctorResponseDto
                    {
                        DoctorId = d.DoctorId,
                        Specialisation = d.Specialisation
                    }).ToList());

            // Act
            var result = await _doctorService.GetPagedAsync(
                pageNumber: 1,
                pageSize: 10,
                search: null,
                specialisation: "Neurology",
                status: null);

            // Assert
            result.TotalCount.Should().Be(1);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldNotFilterBySpecialisation_WhenSpecialisationIsAll()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(GetSampleDoctors());

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<DoctorResponseDto>());

            // Act
            var result = await _doctorService.GetPagedAsync(
                pageNumber: 1,
                pageSize: 10,
                search: null,
                specialisation: "All",
                status: null);

            // Assert
            result.TotalCount.Should().Be(3);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldFilterActiveDoctors_WhenStatusIsActive()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(GetSampleDoctors());

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns((List<Doctor> source) =>
                    source.Select(d => new DoctorResponseDto
                    {
                        DoctorId = d.DoctorId,
                        IsActive = d.IsActive
                    }).ToList());

            // Act
            var result = await _doctorService.GetPagedAsync(
                pageNumber: 1,
                pageSize: 10,
                search: null,
                specialisation: null,
                status: "Active");

            // Assert
            result.TotalCount.Should().Be(2);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldFilterInactiveDoctors_WhenStatusIsInactive()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(GetSampleDoctors());

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns((List<Doctor> source) =>
                    source.Select(d => new DoctorResponseDto
                    {
                        DoctorId = d.DoctorId,
                        IsActive = d.IsActive
                    }).ToList());

            // Act
            var result = await _doctorService.GetPagedAsync(
                pageNumber: 1,
                pageSize: 10,
                search: null,
                specialisation: null,
                status: "Inactive");

            // Assert
            result.TotalCount.Should().Be(1);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldNotFilterByStatus_WhenStatusIsAll()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(GetSampleDoctors());

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<DoctorResponseDto>());

            // Act
            var result = await _doctorService.GetPagedAsync(
                pageNumber: 1,
                pageSize: 10,
                search: null,
                specialisation: null,
                status: "All");

            // Assert
            result.TotalCount.Should().Be(3);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldReturnEmptyResult_WhenNoDoctorsMatch()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(GetSampleDoctors());

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<DoctorResponseDto>());

            // Act
            var result = await _doctorService.GetPagedAsync(
                pageNumber: 1,
                pageSize: 10,
                search: "Unknown",
                specialisation: null,
                status: null);

            // Assert
            result.TotalCount.Should().Be(0);
            result.TotalPages.Should().Be(0);
            result.Items.Should().BeEmpty();
        }

        // -------------------------------------------------------
        // GetAllAsync Tests
        // -------------------------------------------------------

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDoctors()
        {
            // Arrange
            var doctors = GetSampleDoctors();
            var doctorDtos = GetSampleDoctorResponseDtos();

            _repositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<DoctorResponseDto>>(doctors))
                .Returns(doctorDtos);

            // Act
            var result = await _doctorService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            _repositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
            _mapperMock.Verify(
                x => x.Map<IEnumerable<DoctorResponseDto>>(doctors),
                Times.Once);
        }

        // -------------------------------------------------------
        // GetByIdAsync Tests
        // -------------------------------------------------------

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDoctor_WhenDoctorExists()
        {
            // Arrange
            var doctor = GetSampleDoctors().First();

            var doctorDto = new DoctorResponseDto
            {
                DoctorId = doctor.DoctorId,
                DoctorName = doctor.DoctorName,
                Email = doctor.Email,
                Specialisation = doctor.Specialisation,
                IsActive = doctor.IsActive
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(x => x.Map<DoctorResponseDto>(doctor))
                .Returns(doctorDto);

            // Act
            var result = await _doctorService.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.DoctorId.Should().Be(1);
            result.DoctorName.Should().Be("John Smith");

            _repositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowEntityNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            // Act
            Func<Task> act = async () => await _doctorService.GetByIdAsync(99);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Doctor not found");
        }

        // -------------------------------------------------------
        // CreateAsync Tests
        // -------------------------------------------------------

        [Fact]
        public async Task CreateAsync_ShouldCreateDoctorAndUser_WhenValidDtoProvided()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                DoctorName = "Michael Brown",
                Email = "michael@test.com",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 12,
                ConsultationFee = 600,
                IsActive = true
            };

            var doctor = new Doctor
            {
                DoctorId = 10,
                DoctorName = dto.DoctorName,
                Email = dto.Email,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = dto.IsActive
            };

            _mapperMock
                .Setup(x => x.Map<Doctor>(dto))
                .Returns(doctor);

            _repositoryMock
                .Setup(x => x.AddAsync(doctor))
                .Returns(Task.CompletedTask);

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            _cacheServiceMock
                .Setup(x => x.RemoveAsync("available-doctors"))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _doctorService.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.DoctorId.Should().Be(10);
            result.DoctorName.Should().Be("Michael Brown");
            result.Email.Should().Be("michael@test.com");
            result.TemporaryPassword.Should().NotBeNullOrWhiteSpace();
            result.TemporaryPassword.Should().StartWith("Temp@");

            _repositoryMock.Verify(x => x.AddAsync(doctor), Times.Once);

            _userManagerMock.Verify(
                x => x.CreateAsync(
                    It.Is<ApplicationUser>(u =>
                        u.UserName == doctor.Email &&
                        u.Email == doctor.Email &&
                        u.Role == "Doctor" &&
                        u.ReferenceId == doctor.DoctorId &&
                        u.IsFirstLogin == true),
                    It.Is<string>(p => p.StartsWith("Temp@"))),
                Times.Once);

            _userManagerMock.Verify(
                x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"),
                Times.Once);

            _cacheServiceMock.Verify(
                x => x.RemoveAsync("available-doctors"),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowBusinessRuleException_WhenUserCreationFails()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                DoctorName = "Failed Doctor",
                Email = "failed@test.com"
            };

            var doctor = new Doctor
            {
                DoctorId = 20,
                DoctorName = dto.DoctorName,
                Email = dto.Email
            };

            _mapperMock
                .Setup(x => x.Map<Doctor>(dto))
                .Returns(doctor);

            _repositoryMock
                .Setup(x => x.AddAsync(doctor))
                .Returns(Task.CompletedTask);

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(
                    IdentityResult.Failed(
                        new IdentityError
                        {
                            Description = "Email already exists"
                        },
                        new IdentityError
                        {
                            Description = "Password is weak"
                        }));

            // Act
            Func<Task> act = async () => await _doctorService.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("*Email already exists*Password is weak*");

            _repositoryMock.Verify(x => x.AddAsync(doctor), Times.Once);

            _userManagerMock.Verify(
                x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()),
                Times.Never);

            _cacheServiceMock.Verify(
                x => x.RemoveAsync(It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldSetCreatedDate_WhenDoctorCreated()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                DoctorName = "Created Date Doctor",
                Email = "created@test.com"
            };

            var doctor = new Doctor
            {
                DoctorId = 5,
                DoctorName = dto.DoctorName,
                Email = dto.Email
            };

            _mapperMock
                .Setup(x => x.Map<Doctor>(dto))
                .Returns(doctor);

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _doctorService.CreateAsync(dto);

            // Assert
            doctor.CreatedDate.Should().NotBe(default);
            doctor.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        }

        // -------------------------------------------------------
        // UpdateAsync Tests
        // -------------------------------------------------------

        [Fact]
        public async Task UpdateAsync_ShouldUpdateDoctor_WhenDoctorExists()
        {
            // Arrange
            var doctor = GetSampleDoctors().First();

            var dto = new CreateDoctorDto
            {
                DoctorName = "Updated Doctor",
                Email = "updated@test.com",
                Specialisation = SpecialisationType.Neurologist,
                YearsOfExperience = 15,
                ConsultationFee = 900,
                IsActive = true
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _repositoryMock
                .Setup(x => x.UpdateAsync(doctor))
                .Returns(Task.CompletedTask);

            _cacheServiceMock
                .Setup(x => x.RemoveAsync("available-doctors"))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _doctorService.UpdateAsync(1, dto);

            // Assert
            result.Should().BeTrue();

            _mapperMock.Verify(
                x => x.Map(dto, doctor),
                Times.Once);

            _repositoryMock.Verify(
                x => x.UpdateAsync(doctor),
                Times.Once);

            _cacheServiceMock.Verify(
                x => x.RemoveAsync("available-doctors"),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowEntityNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            var dto = new CreateDoctorDto();

            // Act
            Func<Task> act = async () => await _doctorService.UpdateAsync(99, dto);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Doctor not found");

            _repositoryMock.Verify(
                x => x.UpdateAsync(It.IsAny<Doctor>()),
                Times.Never);

            _cacheServiceMock.Verify(
                x => x.RemoveAsync(It.IsAny<string>()),
                Times.Never);
        }

        // -------------------------------------------------------
        // DeleteAsync Tests
        // -------------------------------------------------------

        [Fact]
        public async Task DeleteAsync_ShouldDeleteDoctor_WhenDoctorExists()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.Exists(1))
                .ReturnsAsync(true);

            _repositoryMock
                .Setup(x => x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            _cacheServiceMock
                .Setup(x => x.RemoveAsync("available-doctors"))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _doctorService.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();

            _repositoryMock.Verify(x => x.Exists(1), Times.Once);
            _repositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);

            _cacheServiceMock.Verify(
                x => x.RemoveAsync("available-doctors"),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowEntityNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.Exists(99))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _doctorService.DeleteAsync(99);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Doctor not found");

            _repositoryMock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);

            _cacheServiceMock.Verify(
                x => x.RemoveAsync(It.IsAny<string>()),
                Times.Never);
        }

        // -------------------------------------------------------
        // FilterAsync Cache Tests
        // -------------------------------------------------------

        [Fact]
        public async Task FilterAsync_ShouldReturnCachedDoctors_WhenCacheHit()
        {
            // Arrange
            var cachedDoctors = new List<DoctorResponseDto>
            {
                new DoctorResponseDto
                {
                    DoctorId = 1,
                    DoctorName = "Cached Doctor",
                    Email = "cached@test.com",
                    Specialisation = SpecialisationType.Cardiologist,
                    IsActive = true
                }
            };

            _cacheServiceMock
                .Setup(x => x.GetAsync<List<DoctorResponseDto>>("available-doctors"))
                .ReturnsAsync(cachedDoctors);

            // Act
            var result = await _doctorService.FilterAsync(
                name: null,
                specialization: null,
                isActive: true);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().DoctorName.Should().Be("Cached Doctor");

            _cacheServiceMock.Verify(
                x => x.GetAsync<List<DoctorResponseDto>>("available-doctors"),
                Times.Once);

            _repositoryMock.Verify(
                x => x.GetDoctors(
                    It.IsAny<string?>(),
                    It.IsAny<SpecialisationType?>(),
                    It.IsAny<bool?>()),
                Times.Never);

            _cacheServiceMock.Verify(
                x => x.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<List<DoctorResponseDto>>(),
                    It.IsAny<TimeSpan>()),
                Times.Never);
        }

        [Fact]
        public async Task FilterAsync_ShouldLoadDoctorsFromRepositoryAndSetCache_WhenCacheMiss()
        {
            // Arrange
            var doctors = GetSampleDoctors()
                .Where(x => x.IsActive)
                .ToList();

            var mappedDoctors = doctors
                .Select(d => new DoctorResponseDto
                {
                    DoctorId = d.DoctorId,
                    DoctorName = d.DoctorName,
                    Email = d.Email,
                    Specialisation = d.Specialisation,
                    IsActive = d.IsActive
                })
                .ToList();

            _cacheServiceMock
                .Setup(x => x.GetAsync<List<DoctorResponseDto>>("available-doctors"))
                .ReturnsAsync((List<DoctorResponseDto>?)null);

            _repositoryMock
                .Setup(x => x.GetDoctors(null, null, true))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<List<DoctorResponseDto>>(doctors))
                .Returns(mappedDoctors);

            _cacheServiceMock
                .Setup(x => x.SetAsync(
                    "available-doctors",
                    mappedDoctors,
                    It.IsAny<TimeSpan>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _doctorService.FilterAsync(
                name: null,
                specialization: null,
                isActive: true);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            _cacheServiceMock.Verify(
                x => x.GetAsync<List<DoctorResponseDto>>("available-doctors"),
                Times.Once);

            _repositoryMock.Verify(
                x => x.GetDoctors(null, null, true),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<List<DoctorResponseDto>>(doctors),
                Times.Once);

            _cacheServiceMock.Verify(
                x => x.SetAsync(
                    "available-doctors",
                    mappedDoctors,
                    It.Is<TimeSpan>(t => t == TimeSpan.FromMinutes(5))),
                Times.Once);
        }

        [Fact]
        public async Task FilterAsync_ShouldBypassCache_WhenNameFilterProvided()
        {
            // Arrange
            var doctors = GetSampleDoctors()
                .Where(x => x.DoctorName.Contains("John"))
                .ToList();

            var doctorDtos = doctors
                .Select(d => new DoctorResponseDto
                {
                    DoctorId = d.DoctorId,
                    DoctorName = d.DoctorName
                });

            _repositoryMock
                .Setup(x => x.GetDoctors("John", null, true))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<DoctorResponseDto>>(doctors))
                .Returns(doctorDtos);

            // Act
            var result = await _doctorService.FilterAsync(
                name: "John",
                specialization: null,
                isActive: true);

            // Assert
            result.Should().HaveCount(2);

            _cacheServiceMock.Verify(
                x => x.GetAsync<List<DoctorResponseDto>>(It.IsAny<string>()),
                Times.Never);

            _cacheServiceMock.Verify(
                x => x.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<List<DoctorResponseDto>>(),
                    It.IsAny<TimeSpan>()),
                Times.Never);

            _repositoryMock.Verify(
                x => x.GetDoctors("John", null, true),
                Times.Once);
        }

        [Fact]
        public async Task FilterAsync_ShouldBypassCache_WhenSpecializationFilterProvided()
        {
            // Arrange
            var doctors = GetSampleDoctors()
                .Where(x => x.Specialisation == SpecialisationType.Cardiologist)
                .ToList();

            var doctorDtos = doctors
                .Select(d => new DoctorResponseDto
                {
                    DoctorId = d.DoctorId,
                    Specialisation = d.Specialisation
                });

            _repositoryMock
                .Setup(x => x.GetDoctors(null, SpecialisationType.Cardiologist, true))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<DoctorResponseDto>>(doctors))
                .Returns(doctorDtos);

            // Act
            var result = await _doctorService.FilterAsync(
                name: null,
                specialization: SpecialisationType.Cardiologist,
                isActive: true);

            // Assert
            result.Should().HaveCount(2);

            _cacheServiceMock.Verify(
                x => x.GetAsync<List<DoctorResponseDto>>(It.IsAny<string>()),
                Times.Never);

            _repositoryMock.Verify(
                x => x.GetDoctors(null, SpecialisationType.Cardiologist, true),
                Times.Once);
        }

        [Fact]
        public async Task FilterAsync_ShouldBypassCache_WhenIsActiveIsFalse()
        {
            // Arrange
            var doctors = GetSampleDoctors()
                .Where(x => !x.IsActive)
                .ToList();

            var doctorDtos = doctors
                .Select(d => new DoctorResponseDto
                {
                    DoctorId = d.DoctorId,
                    IsActive = d.IsActive
                });

            _repositoryMock
                .Setup(x => x.GetDoctors(null, null, false))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<DoctorResponseDto>>(doctors))
                .Returns(doctorDtos);

            // Act
            var result = await _doctorService.FilterAsync(
                name: null,
                specialization: null,
                isActive: false);

            // Assert
            result.Should().HaveCount(1);

            _cacheServiceMock.Verify(
                x => x.GetAsync<List<DoctorResponseDto>>(It.IsAny<string>()),
                Times.Never);

            _repositoryMock.Verify(
                x => x.GetDoctors(null, null, false),
                Times.Once);
        }

        [Fact]
        public async Task FilterAsync_ShouldBypassCache_WhenIsActiveIsNull()
        {
            // Arrange
            var doctors = GetSampleDoctors();

            var doctorDtos = doctors
                .Select(d => new DoctorResponseDto
                {
                    DoctorId = d.DoctorId
                });

            _repositoryMock
                .Setup(x => x.GetDoctors(null, null, null))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<DoctorResponseDto>>(doctors))
                .Returns(doctorDtos);

            // Act
            var result = await _doctorService.FilterAsync(
                name: null,
                specialization: null,
                isActive: null);

            // Assert
            result.Should().HaveCount(3);

            _cacheServiceMock.Verify(
                x => x.GetAsync<List<DoctorResponseDto>>(It.IsAny<string>()),
                Times.Never);

            _repositoryMock.Verify(
                x => x.GetDoctors(null, null, null),
                Times.Once);
        }

        // -------------------------------------------------------
        // SetStatusAsync Tests
        // -------------------------------------------------------

        [Fact]
        public async Task SetStatusAsync_ShouldSetDoctorStatus_WhenDoctorExists()
        {
            // Arrange
            var doctor = GetSampleDoctors().First();

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _repositoryMock
                .Setup(x => x.SetStatus(1, false))
                .Returns(Task.CompletedTask);

            _cacheServiceMock
                .Setup(x => x.RemoveAsync("available-doctors"))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _doctorService.SetStatusAsync(1, false);

            // Assert
            result.Should().BeTrue();

            _repositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
            _repositoryMock.Verify(x => x.SetStatus(1, false), Times.Once);

            _cacheServiceMock.Verify(
                x => x.RemoveAsync("available-doctors"),
                Times.Once);
        }

        [Fact]
        public async Task SetStatusAsync_ShouldThrowEntityNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            // Act
            Func<Task> act = async () => await _doctorService.SetStatusAsync(99, false);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Doctor not found");

            _repositoryMock.Verify(
                x => x.SetStatus(It.IsAny<int>(), It.IsAny<bool>()),
                Times.Never);

            _cacheServiceMock.Verify(
                x => x.RemoveAsync(It.IsAny<string>()),
                Times.Never);
        }
    }
}