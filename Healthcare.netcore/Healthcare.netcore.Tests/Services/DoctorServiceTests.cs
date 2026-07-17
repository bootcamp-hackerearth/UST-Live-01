using AutoMapper;
using FluentAssertions;
using HealthAxis.Shared.DTOs.Doctor;
using HealthAxis.Shared.Enums;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Models.Auth;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Moq;
using ValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace Healthcare.netcore.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();

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

            _service = new DoctorService(
                _doctorRepositoryMock.Object,
                _userManagerMock.Object,
                _mapperMock.Object);
        }

        private static Doctor GetDoctor(bool isActive = true)
        {
            return new Doctor
            {
                DoctorId = 6,
                FullName = "Dr Nevin",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 800,
                IsActive = isActive
            };
        }

        private static DoctorDto GetDoctorDto()
        {
            return new DoctorDto
            {
                DoctorId = 6,
                FullName = "Dr Nevin",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 800,
                IsActive = true
            };
        }

        private static CreateDoctorDto GetCreateDoctorDto()
        {
            return new CreateDoctorDto
            {
                FullName = "Dr Nevin",
                Email = "nevin@gmail.com",
                TemporaryPassword = "Temp@123",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 800
            };
        }

        private static UpdateDoctorDto GetUpdateDoctorDto()
        {
            return new UpdateDoctorDto
            {
                FullName = "Dr Nevin Updated",
                Specialisation = Specialisation.Neurology,
                YearsOfExperience = 6,
                ConsultationFee = 900,
                IsActive = true
            };
        }

        [Fact]
        public async Task GetAllAsync_WhenDoctorsExist_ReturnsDoctorList()
        {
            var doctors = new List<Doctor>
            {
                GetDoctor(),
                new Doctor
                {
                    DoctorId = 7,
                    FullName = "Dr Kiran",
                    Specialisation = Specialisation.Dermatology,
                    YearsOfExperience = 8,
                    ConsultationFee = 700,
                    IsActive = true
                }
            };

            var doctorDtos = new List<DoctorDto>
            {
                GetDoctorDto(),
                new DoctorDto
                {
                    DoctorId = 7,
                    FullName = "Dr Kiran",
                    Specialisation = Specialisation.Dermatology,
                    YearsOfExperience = 8,
                    ConsultationFee = 700,
                    IsActive = true
                }
            };

            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().DoctorId.Should().Be(6);
            result.First().FullName.Should().Be("Dr Nevin");
        }

        [Fact]
        public async Task GetAllAsync_WhenNoDoctorsExist_ReturnsEmptyList()
        {
            var doctors = new List<Doctor>();
            var doctorDtos = new List<DoctorDto>();

            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryFails_ThrowsException()
        {
            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.GetAllAsync();

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorExists_ReturnsDoctorDto()
        {
            var doctor = GetDoctor();
            var doctorDto = GetDoctorDto();

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(6))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(doctorDto);

            var result = await _service.GetByIdAsync(6);

            result.Should().NotBeNull();
            result!.DoctorId.Should().Be(6);
            result.FullName.Should().Be("Dr Nevin");
            result.Specialisation.Should().Be(Specialisation.Cardiology);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () => await _service.GetByIdAsync(99);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Doctor not found");
        }

        [Fact]
        public async Task GetByIdAsync_WhenRepositoryFails_ThrowsException()
        {
            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(6))
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.GetByIdAsync(6);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorExists_ReturnsAvailability()
        {
            var doctor = GetDoctor(true);

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(6))
                .ReturnsAsync(doctor);

            var result = await _service.GetAvailabilityAsync(6, CancellationToken.None);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(100))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () => await _service.GetAvailabilityAsync(100, CancellationToken.None);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Doctor not found");
        }

        [Fact]
        public async Task AddAsync_WhenDoctorEmailAlreadyExists_ThrowsBusinessRuleException()
        {
            var dto = GetCreateDoctorDto();

            var existingUser = new ApplicationUser
            {
                Id = "doctor-user-1",
                Email = dto.Email,
                UserName = dto.Email
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(existingUser);

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Doctor login already exists with this email.");
        }

        [Fact]
        public async Task AddAsync_WhenIdentityCreateFails_ThrowsValidationException()
        {
            var dto = GetCreateDoctorDto();

            var identityErrors = new[]
            {
                new IdentityError
                {
                    Description = "Password is too weak"
                }
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    dto.TemporaryPassword))
                .ReturnsAsync(IdentityResult.Failed(identityErrors));

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Password is too weak");
        }

        [Fact]
        public async Task AddAsync_WhenAddToRoleFails_ThrowsValidationException()
        {
            var dto = GetCreateDoctorDto();

            var roleErrors = new[]
            {
                new IdentityError
                {
                    Description = "Role does not exist"
                }
            };

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    dto.TemporaryPassword))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Failed(roleErrors));

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Role does not exist");
        }

        [Fact]
        public async Task AddAsync_WhenValidDoctor_ReturnsDoctorDto()
        {
            var dto = GetCreateDoctorDto();

            var doctor = GetDoctor();

            var doctorDto = GetDoctorDto();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    dto.TemporaryPassword))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            _doctorRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Doctor>()))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(x => x.Map<DoctorDto>(It.IsAny<Doctor>()))
                .Returns(doctorDto);

            var result = await _service.AddAsync(dto);

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(6);
            result.FullName.Should().Be("Dr Nevin");

            _userManagerMock.Verify(
                x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    dto.TemporaryPassword),
                Times.Once);

            _userManagerMock.Verify(
                x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"),
                Times.Once);

            _doctorRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Doctor>()),
                Times.Once);
        }

        [Fact]
        public async Task AddAsync_WhenValidDoctor_SetsMustChangePasswordTrue()
        {
            var dto = GetCreateDoctorDto();

            ApplicationUser? capturedUser = null;

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    dto.TemporaryPassword))
                .Callback<ApplicationUser, string>((user, _) =>
                {
                    capturedUser = user;
                })
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            _doctorRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Doctor>()))
                .ReturnsAsync(GetDoctor());

            _mapperMock
                .Setup(x => x.Map<DoctorDto>(It.IsAny<Doctor>()))
                .Returns(GetDoctorDto());

            await _service.AddAsync(dto);

            capturedUser.Should().NotBeNull();
            capturedUser!.Email.Should().Be(dto.Email);
            capturedUser.UserName.Should().Be(dto.Email);
            capturedUser.MustChangePassword.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            var dto = GetUpdateDoctorDto();

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () => await _service.UpdateAsync(99, dto);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Doctor not found");
        }

        [Fact]
        public async Task UpdateAsync_WhenDoctorExists_ReturnsUpdatedDoctorDto()
        {
            var doctor = GetDoctor();

            var dto = GetUpdateDoctorDto();

            var updatedDoctorDto = new DoctorDto
            {
                DoctorId = 6,
                FullName = "Dr Nevin Updated",
                Specialisation = Specialisation.Neurology,
                YearsOfExperience = 6,
                ConsultationFee = 900,
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(6))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(x => x.Map(dto, doctor));

            _doctorRepositoryMock
                .Setup(x => x.UpdateAsync(6, doctor, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(updatedDoctorDto);

            var result = await _service.UpdateAsync(6, dto);

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(6);
            result.FullName.Should().Be("Dr Nevin Updated");
            result.Specialisation.Should().Be(Specialisation.Neurology);

            _doctorRepositoryMock.Verify(
                x => x.UpdateAsync(6, doctor, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenRepositoryFails_ThrowsException()
        {
            var doctor = GetDoctor();

            var dto = GetUpdateDoctorDto();

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(6))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(x => x.Map(dto, doctor));

            _doctorRepositoryMock
                .Setup(x => x.UpdateAsync(6, doctor, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.UpdateAsync(6, dto);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }
        [Fact]
        public async Task GetByUserIdAsync_WhenDoctorExists_ReturnsDoctor()
        {
            var doctor = GetDoctor();
            doctor.UserId = "user1";

            var dto = GetDoctorDto();

            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Doctor> { doctor });

            _mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(dto);

            var result = await _service.GetByUserIdAsync("user1");

            result.Should().NotBeNull();
            result!.DoctorId.Should().Be(6);
        }
        [Fact]
        public async Task GetByUserIdAsync_WhenDoctorDoesNotExist_ThrowsException()
        {
            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            Func<Task> act = async () =>
                await _service.GetByUserIdAsync("xyz");

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Doctor profile not found.");
        }
        [Fact]
        public async Task GetAllAsync_WithPagination_ReturnsPagedDoctors()
        {
            var doctors = Enumerable.Range(1, 20)
                .Select(i => new Doctor
                {
                    DoctorId = i,
                    FullName = "Doctor" + i
                }).ToList();

            var dtos = doctors.Select(d => new DoctorDto
            {
                DoctorId = d.DoctorId,
                FullName = d.FullName
            });

            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(dtos);

            var result = await _service.GetAllAsync(
                new PaginationParams
                {
                    PageNumber = 2,
                    PageSize = 5
                });

            result.Should().NotBeNull();
            result.Items.Count.Should().Be(5);
            result.PageNumber.Should().Be(2);
        }
        [Fact]
        public async Task GetAllAsync_WithZeroPagination_UsesDefaults()
        {
            var doctors = new List<Doctor>
    {
        GetDoctor()
    };

            var dtos = new List<DoctorDto>
    {
        GetDoctorDto()
    };

            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(dtos);

            var result = await _service.GetAllAsync(
                new PaginationParams
                {
                    PageNumber = 0,
                    PageSize = 0
                });

            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
        }
        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorInactive_ReturnsNoSlots()
        {
            var doctor = GetDoctor(false);

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(6))
                .ReturnsAsync(doctor);

            var result = await _service.GetAvailabilityAsync(
                6,
                DateTime.Today);

            result.Should().NotBeNull();
            result.AvailableSlots.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAvailabilityAsync_WithFutureDate_ReturnsAvailability()
        {
            var doctor = GetDoctor();

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(6))
                .ReturnsAsync(doctor);

            var result = await _service.GetAvailabilityAsync(
                6,
                DateTime.Today.AddDays(5));

            result.Should().NotBeNull();
            result.Date.Date.Should().Be(DateTime.Today.AddDays(5).Date);
        }
        [Fact]
        public async Task GetAvailabilityAsync_WhenRepositoryThrows_ExceptionIsThrown()
        {
            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(6))
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> act = async () =>
                await _service.GetAvailabilityAsync(
                    6,
                    DateTime.Today);

            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }
        [Fact]
        public async Task AddAsync_ShouldCallRepositoryOnce()
        {
            var dto = GetCreateDoctorDto();

            _userManagerMock
                .Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    dto.TemporaryPassword))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(x => x.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            _doctorRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Doctor>()))
                .ReturnsAsync(GetDoctor());

            _mapperMock
                .Setup(x => x.Map<DoctorDto>(It.IsAny<Doctor>()))
                .Returns(GetDoctorDto());

            await _service.AddAsync(dto);

            _doctorRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Doctor>()),
                Times.Once);
        }
        [Fact]
        public async Task GetAllAsync_ShouldCallRepositoryOnce()
        {
            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            _mapperMock
                .Setup(x => x.Map<IEnumerable<DoctorDto>>(It.IsAny<IEnumerable<Doctor>>()))
                .Returns(new List<DoctorDto>());

            await _service.GetAllAsync();

            _doctorRepositoryMock.Verify(
                x => x.GetAllAsync(),
                Times.Once);
        }


    }
}