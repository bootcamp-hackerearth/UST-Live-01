using AutoMapper;
using FluentAssertions;
using HealthAxis.Shared.DTOs.Doctor;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace HealthAxisCore_Api.Tests.Services
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
            _userManagerMock = MockUserManager();

            _service = new DoctorService(
                _doctorRepositoryMock.Object,
                _mapperMock.Object,
                _userManagerMock.Object
            );
        }

        // ------------------------------------------------------------
        // GetPagedAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetPagedAsync_WhenPageNumberLessThanOne_ShouldDefaultToOne()
        {
            var doctors = GetSampleDoctors();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                0,
                10,
                null,
                null,
                null
            );

            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalCount.Should().Be(doctors.Count);
        }

        [Fact]
        public async Task GetPagedAsync_WhenPageSizeLessThanOne_ShouldDefaultToTen()
        {
            var doctors = GetSampleDoctors();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                0,
                null,
                null,
                null
            );

            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GetPagedAsync_WhenPageSizeGreaterThanHundred_ShouldLimitToHundred()
        {
            var doctors = GetSampleDoctors();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                150,
                null,
                null,
                null
            );

            result.PageSize.Should().Be(100);
        }

        [Fact]
        public async Task GetPagedAsync_WithSearchByDoctorName_ShouldReturnMatchingDoctor()
        {
            var doctors = GetSampleDoctors();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                "Asha",
                null,
                null
            );

            result.TotalCount.Should().Be(1);
            result.Items.First().DoctorName.Should().Be("Asha Kumar");
        }

        [Fact]
        public async Task GetPagedAsync_WithSearchByEmail_ShouldReturnMatchingDoctor()
        {
            var doctors = GetSampleDoctors();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                "ravi",
                null,
                null
            );

            result.TotalCount.Should().Be(1);
            result.Items.First().Email.Should().Be("ravi@test.com");
        }

        [Fact]
        public async Task GetPagedAsync_WithSearchBySpecialisation_ShouldReturnMatchingDoctors()
        {
            var doctors = GetSampleDoctors();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                "Cardiologist",
                null,
                null
            );

            result.TotalCount.Should().Be(1);
            result.Items.First().Specialisation.Should().Be(SpecialisationType.Cardiologist);
        }

        [Fact]
        public async Task GetPagedAsync_WithSpecialisationFilter_ShouldReturnMatchingDoctors()
        {
            var doctors = GetSampleDoctors();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                "Neurologist",
                null
            );

            result.TotalCount.Should().Be(1);
            result.Items.First().Specialisation.Should().Be(SpecialisationType.Neurologist);
        }

        [Fact]
        public async Task GetPagedAsync_WithSpecialisationAll_ShouldNotFilterBySpecialisation()
        {
            var doctors = GetSampleDoctors();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                "All",
                null
            );

            result.TotalCount.Should().Be(doctors.Count);
        }

        [Fact]
        public async Task GetPagedAsync_WithActiveStatus_ShouldReturnOnlyActiveDoctors()
        {
            var doctors = GetSampleDoctors();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                null,
                "Active"
            );

            result.Items.Should().OnlyContain(x => x.IsActive);
        }

        [Fact]
        public async Task GetPagedAsync_WithInactiveStatus_ShouldReturnOnlyInactiveDoctors()
        {
            var doctors = GetSampleDoctors();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                null,
                "Inactive"
            );

            result.Items.Should().OnlyContain(x => !x.IsActive);
        }

        [Fact]
        public async Task GetPagedAsync_WithStatusAll_ShouldNotFilterByStatus()
        {
            var doctors = GetSampleDoctors();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                null,
                "All"
            );

            result.TotalCount.Should().Be(doctors.Count);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldApplyPagination()
        {
            var doctors = GetSampleDoctors();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                2,
                null,
                null,
                null
            );

            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(doctors.Count);
            result.TotalPages.Should().Be((int)Math.Ceiling(doctors.Count / 2.0));
        }

        [Fact]
        public async Task GetPagedAsync_ShouldOrderDoctorsByName()
        {
            var doctors = GetSampleDoctors();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                null,
                null
            );

            result.Items.First().DoctorName.Should().Be("Asha Kumar");
        }

        // ------------------------------------------------------------
        // GetAllAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDoctors()
        {
            var doctors = GetSampleDoctors();

            var mappedDoctors = doctors.Select(d => ToDoctorResponseDto(d)).ToList();

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<DoctorResponseDto>>(doctors))
                .Returns(mappedDoctors);

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(doctors.Count);

            _doctorRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        // ------------------------------------------------------------
        // GetByIdAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetByIdAsync_WhenDoctorExists_ShouldReturnMappedDoctor()
        {
            var doctor = new Doctor
            {
                DoctorId = 1,
                DoctorName = "Asha Kumar",
                Email = "asha@test.com",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };

            var mappedDoctor = ToDoctorResponseDto(doctor);

            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(mapper => mapper.Map<DoctorResponseDto>(doctor))
                .Returns(mappedDoctor);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.DoctorId.Should().Be(1);
            result.DoctorName.Should().Be("Asha Kumar");
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            var act = async () => await _service.GetByIdAsync(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Doctor not found");
        }

        // ------------------------------------------------------------
        // CreateAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task CreateAsync_WhenValidRequest_ShouldCreateDoctorAndIdentityUser()
        {
            var dto = new CreateDoctorDto
            {
                DoctorName = "Asha Kumar",
                Email = "asha@test.com",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };

            var doctorEntity = new Doctor
            {
                DoctorName = dto.DoctorName,
                Email = dto.Email,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = dto.IsActive
            };

            _mapperMock
                .Setup(mapper => mapper.Map<Doctor>(dto))
                .Returns(doctorEntity);

            _doctorRepositoryMock
                .Setup(repo => repo.AddAsync(doctorEntity))
                .Callback<Doctor>(doctor => doctor.DoctorId = 101)
                .Returns(Task.CompletedTask);

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _service.CreateAsync(dto);

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(101);
            result.DoctorName.Should().Be(dto.DoctorName);
            result.Email.Should().Be(dto.Email);
            result.Specialisation.Should().Be(dto.Specialisation);
            result.YearsOfExperience.Should().Be(dto.YearsOfExperience);
            result.ConsultationFee.Should().Be(dto.ConsultationFee);
            result.IsActive.Should().BeTrue();
            result.TemporaryPassword.Should().NotBeNullOrWhiteSpace();
            result.TemporaryPassword.Should().StartWith("Temp@");

            doctorEntity.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

            _doctorRepositoryMock.Verify(repo => repo.AddAsync(doctorEntity), Times.Once);

            _userManagerMock.Verify(manager => manager.CreateAsync(
                    It.Is<ApplicationUser>(user =>
                        user.Email == dto.Email &&
                        user.UserName == dto.Email &&
                        user.Role == "Doctor" &&
                        user.ReferenceId == 101 &&
                        user.IsFirstLogin &&
                        user.TemporaryPassword != null),
                    It.Is<string>(password => password.StartsWith("Temp@"))),
                Times.Once);

            _userManagerMock.Verify(manager => manager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    "Doctor"),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenIdentityCreateFails_ShouldThrowBusinessRuleException()
        {
            var dto = new CreateDoctorDto
            {
                DoctorName = "Ravi Menon",
                Email = "ravi@test.com",
                Specialisation = SpecialisationType.Neurologist,
                YearsOfExperience = 8,
                ConsultationFee = 700,
                IsActive = true
            };

            var doctorEntity = new Doctor
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
                .Setup(mapper => mapper.Map<Doctor>(dto))
                .Returns(doctorEntity);

            _doctorRepositoryMock
                .Setup(repo => repo.AddAsync(doctorEntity))
                .Returns(Task.CompletedTask);

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Email already exists"
                    },
                    new IdentityError
                    {
                        Description = "Password is too weak"
                    }
                ));

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Email already exists, Password is too weak");

            _doctorRepositoryMock.Verify(repo => repo.AddAsync(doctorEntity), Times.Once);

            _userManagerMock.Verify(manager => manager.AddToRoleAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        // ------------------------------------------------------------
        // UpdateAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task UpdateAsync_WhenDoctorExists_ShouldUpdateDoctorAndReturnTrue()
        {
            var doctor = new Doctor
            {
                DoctorId = 1,
                DoctorName = "Old Name",
                Email = "old@test.com",
                Specialisation = SpecialisationType.Pediatrician,
                YearsOfExperience = 5,
                ConsultationFee = 300,
                IsActive = true
            };

            var dto = new CreateDoctorDto
            {
                DoctorName = "New Name",
                Email = "new@test.com",
                Specialisation = SpecialisationType.Dermatologist,
                YearsOfExperience = 9,
                ConsultationFee = 800,
                IsActive = false
            };

            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(mapper => mapper.Map(dto, doctor))
                .Callback<CreateDoctorDto, Doctor>((source, destination) =>
                {
                    destination.DoctorName = source.DoctorName;
                    destination.Email = source.Email;
                    destination.Specialisation = source.Specialisation;
                    destination.YearsOfExperience = source.YearsOfExperience;
                    destination.ConsultationFee = source.ConsultationFee;
                    destination.IsActive = source.IsActive;
                })
                .Returns(doctor);

            _doctorRepositoryMock
                .Setup(repo => repo.UpdateAsync(doctor))
                .Returns(Task.CompletedTask);

            var result = await _service.UpdateAsync(1, dto);

            result.Should().BeTrue();
            doctor.DoctorName.Should().Be(dto.DoctorName);
            doctor.Email.Should().Be(dto.Email);
            doctor.Specialisation.Should().Be(dto.Specialisation);
            doctor.YearsOfExperience.Should().Be(dto.YearsOfExperience);
            doctor.ConsultationFee.Should().Be(dto.ConsultationFee);
            doctor.IsActive.Should().BeFalse();

            _doctorRepositoryMock.Verify(repo => repo.UpdateAsync(doctor), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            var dto = new CreateDoctorDto
            {
                DoctorName = "New Name",
                Email = "new@test.com",
                Specialisation = SpecialisationType.Dermatologist,
                YearsOfExperience = 9,
                ConsultationFee = 800,
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            var act = async () => await _service.UpdateAsync(1, dto);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Doctor not found");

            _doctorRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Doctor>()), Times.Never);
        }

        // ------------------------------------------------------------
        // DeleteAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task DeleteAsync_WhenDoctorExists_ShouldDeleteDoctorAndReturnTrue()
        {
            _doctorRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(true);

            _doctorRepositoryMock
                .Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(1);

            result.Should().BeTrue();

            _doctorRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(false);

            var act = async () => await _service.DeleteAsync(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Doctor not found");

            _doctorRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        // ------------------------------------------------------------
        // FilterAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task FilterAsync_ShouldReturnMappedDoctors()
        {
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    DoctorName = "Asha Kumar",
                    Email = "asha@test.com",
                    Specialisation = SpecialisationType.Cardiologist,
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                }
            };

            var mappedDoctors = doctors.Select(d => ToDoctorResponseDto(d)).ToList();

            _doctorRepositoryMock
                .Setup(repo => repo.GetDoctors(
                    "Asha",
                    SpecialisationType.Cardiologist,
                    true))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<DoctorResponseDto>>(doctors))
                .Returns(mappedDoctors);

            var result = await _service.FilterAsync(
                "Asha",
                SpecialisationType.Cardiologist,
                true
            );

            result.Should().HaveCount(1);
            result.First().DoctorName.Should().Be("Asha Kumar");
            result.First().Specialisation.Should().Be(SpecialisationType.Cardiologist);
            result.First().IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task FilterAsync_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyMappedList()
        {
            var doctors = new List<Doctor>();
            var mappedDoctors = new List<DoctorResponseDto>();

            _doctorRepositoryMock
                .Setup(repo => repo.GetDoctors(null, null, null))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<DoctorResponseDto>>(doctors))
                .Returns(mappedDoctors);

            var result = await _service.FilterAsync(null, null, null);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        // ------------------------------------------------------------
        // SetStatusAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task SetStatusAsync_WhenDoctorExists_ShouldSetStatusAndReturnTrue()
        {
            var doctor = new Doctor
            {
                DoctorId = 1,
                DoctorName = "Asha Kumar",
                Email = "asha@test.com",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(repo => repo.SetStatus(1, false))
                .Returns(Task.CompletedTask);

            var result = await _service.SetStatusAsync(1, false);

            result.Should().BeTrue();

            _doctorRepositoryMock.Verify(repo => repo.SetStatus(1, false), Times.Once);
        }

        [Fact]
        public async Task SetStatusAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            var act = async () => await _service.SetStatusAsync(1, false);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Doctor not found");

            _doctorRepositoryMock.Verify(repo => repo.SetStatus(It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
        }

        // ------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------

        private static List<Doctor> GetSampleDoctors()
        {
            return new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    DoctorName = "Ravi Menon",
                    Email = "ravi@test.com",
                    Specialisation = SpecialisationType.Neurologist,
                    YearsOfExperience = 8,
                    ConsultationFee = 700,
                    IsActive = true,
                    CreatedDate = DateTime.Today.AddDays(-10)
                },
                new Doctor
                {
                    DoctorId = 2,
                    DoctorName = "Asha Kumar",
                    Email = "asha@test.com",
                    Specialisation = SpecialisationType.Cardiologist,
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true,
                    CreatedDate = DateTime.Today.AddDays(-8)
                },
                new Doctor
                {
                    DoctorId = 3,
                    DoctorName = "Meera Nair",
                    Email = "meera@test.com",
                    Specialisation = SpecialisationType.Dermatologist,
                    YearsOfExperience = 6,
                    ConsultationFee = 600,
                    IsActive = false,
                    CreatedDate = DateTime.Today.AddDays(-5)
                },
                new Doctor
                {
                    DoctorId = 4,
                    DoctorName = "John Mathew",
                    Email = "john@test.com",
                    Specialisation = SpecialisationType.Pediatrician,
                    YearsOfExperience = 12,
                    ConsultationFee = 900,
                    IsActive = false,
                    CreatedDate = DateTime.Today.AddDays(-3)
                }
            };
        }

        private void SetupPagedMapper()
        {
            _mapperMock
                .Setup(mapper => mapper.Map<List<DoctorResponseDto>>(It.IsAny<List<Doctor>>()))
                .Returns((List<Doctor> source) =>
                    source.Select(ToDoctorResponseDto).ToList()
                );
        }

        private static DoctorResponseDto ToDoctorResponseDto(Doctor doctor)
        {
            return new DoctorResponseDto
            {
                DoctorId = doctor.DoctorId,
                DoctorName = doctor.DoctorName,
                Email = doctor.Email,
                Specialisation = doctor.Specialisation,
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive
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
                null!
            );
        }
    }
}
