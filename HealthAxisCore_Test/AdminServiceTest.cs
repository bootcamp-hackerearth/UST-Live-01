using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq.Expressions;

namespace HealthAxisCore_Api.Tests.Services
{
    public class AdminServiceTests
    {
        private static Mock<UserManager<ApplicationUser>> CreateUserManagerMock(
            IQueryable<ApplicationUser>? users = null)
        {
            var store = new Mock<IUserStore<ApplicationUser>>();

            var mock = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<ApplicationUser>>(),
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                Mock.Of<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<ApplicationUser>>>());

            mock.Setup(x => x.Users)
                .Returns(users ?? CreateAsyncQueryable(new List<ApplicationUser>()));

            return mock;
        }

        private static Mock<RoleManager<IdentityRole>> CreateRoleManagerMock()
        {
            var roleStore = new Mock<IRoleStore<IdentityRole>>();

            return new Mock<RoleManager<IdentityRole>>(
                roleStore.Object,
                Array.Empty<IRoleValidator<IdentityRole>>(),
                Mock.Of<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Mock.Of<ILogger<RoleManager<IdentityRole>>>());
        }

        private static AdminService CreateService(
            Mock<IDoctorRepository>? doctorRepositoryMock = null,
            Mock<IPatientRepository>? patientRepositoryMock = null,
            Mock<IAppointmentRepository>? appointmentRepositoryMock = null,
            Mock<UserManager<ApplicationUser>>? userManagerMock = null,
            Mock<RoleManager<IdentityRole>>? roleManagerMock = null,
            Mock<IMapper>? mapperMock = null)
        {
            return new AdminService(
                doctorRepositoryMock?.Object ?? new Mock<IDoctorRepository>().Object,
                patientRepositoryMock?.Object ?? new Mock<IPatientRepository>().Object,
                appointmentRepositoryMock?.Object ?? new Mock<IAppointmentRepository>().Object,
                userManagerMock?.Object ?? CreateUserManagerMock().Object,
                roleManagerMock?.Object ?? CreateRoleManagerMock().Object,
                mapperMock?.Object ?? new Mock<IMapper>().Object);
        }

        [Fact]
        public async Task GetDoctorsAsync_WhenDoctorsExist_ShouldReturnPagedMappedDoctorDtos()
        {
            var ct = CancellationToken.None;

            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 3,
                    DoctorName = "Doctor Three",
                    Specialisation = "Neurologist",
                    YearsOfExperience = 8,
                    ConsultationFee = 900,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = 1,
                    DoctorName = "Doctor One",
                    Specialisation = "Cardiologist",
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = 2,
                    DoctorName = "Doctor Two",
                    Specialisation = "Dermatologist",
                    YearsOfExperience = 6,
                    ConsultationFee = 700,
                    IsActive = false
                }
            };

            var mappedDtos = new List<DoctorDto>
            {
                new DoctorDto
                {
                    DoctorId = 1,
                    DoctorName = "Doctor One",
                    Specialisation = "Cardiologist",
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                },
                new DoctorDto
                {
                    DoctorId = 2,
                    DoctorName = "Doctor Two",
                    Specialisation = "Dermatologist",
                    YearsOfExperience = 6,
                    ConsultationFee = 700,
                    IsActive = false
                }
            };

            var query = new PaginationQueryDto
            {
                PageNumber = 1,
                PageSize = 2
            };

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetAllAsync(ct))
                .ReturnsAsync(doctors);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(
                    It.Is<List<Doctor>>(d =>
                        d.Count == 2 &&
                        d[0].DoctorId == 1 &&
                        d[1].DoctorId == 2)))
                .Returns(mappedDtos);

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetDoctorsAsync(query, ct);

            Assert.Equal(2, result.Items.Count);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(2, result.TotalPages);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(2, result.PageSize);
            Assert.Equal(1, result.Items[0].DoctorId);
            Assert.Equal(2, result.Items[1].DoctorId);

            doctorRepositoryMock.Verify(x => x.GetAllAsync(ct), Times.Once);
            mapperMock.Verify(x => x.Map<List<DoctorDto>>(
                It.Is<List<Doctor>>(d =>
                    d.Count == 2 &&
                    d[0].DoctorId == 1 &&
                    d[1].DoctorId == 2)), Times.Once);
        }

        [Fact]
        public async Task GetDoctorsAsync_WhenSecondPageRequested_ShouldReturnSecondPage()
        {
            var ct = CancellationToken.None;

            var doctors = new List<Doctor>
            {
                new Doctor { DoctorId = 1, DoctorName = "Doctor One" },
                new Doctor { DoctorId = 2, DoctorName = "Doctor Two" },
                new Doctor { DoctorId = 3, DoctorName = "Doctor Three" }
            };

            var mappedDtos = new List<DoctorDto>
            {
                new DoctorDto
                {
                    DoctorId = 3,
                    DoctorName = "Doctor Three"
                }
            };

            var query = new PaginationQueryDto
            {
                PageNumber = 2,
                PageSize = 2
            };

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetAllAsync(ct))
                .ReturnsAsync(doctors);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(
                    It.Is<List<Doctor>>(d =>
                        d.Count == 1 &&
                        d[0].DoctorId == 3)))
                .Returns(mappedDtos);

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetDoctorsAsync(query, ct);

            Assert.Single(result.Items);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(2, result.TotalPages);
            Assert.Equal(2, result.PageNumber);
            Assert.Equal(2, result.PageSize);
            Assert.Equal(3, result.Items[0].DoctorId);
        }

        [Fact]
        public async Task GetDoctorsAsync_WhenDoctorsDoNotExist_ShouldReturnEmptyPagedResult()
        {
            var ct = CancellationToken.None;

            var query = new PaginationQueryDto
            {
                PageNumber = 1,
                PageSize = 5
            };

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetAllAsync(ct))
                .ReturnsAsync(new List<Doctor>());

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<DoctorDto>());

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetDoctorsAsync(query, ct);

            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
            Assert.Equal(0, result.TotalPages);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(5, result.PageSize);
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenEmailAlreadyExists_ShouldThrowInvalidException()
        {
            var request = new CreateDoctorDto
            {
                DoctorName = "Doctor Existing",
                Specialisation = "Cardiologist",
                YearsOfExperience = 8,
                ConsultationFee = 700,
                Email = "doctor@test.com",
                PhoneNumber = "9876543210",
                Password = "Doctor@123"
            };

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.Email
                });

            var service = CreateService(userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.CreateDoctorAsync(request));

            Assert.Equal("Email already exists", exception.Message);

            userManagerMock.Verify(x => x.FindByEmailAsync(request.Email), Times.Once);
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenUserCreationFails_ShouldThrowInvalidException()
        {
            var ct = CancellationToken.None;

            var request = new CreateDoctorDto
            {
                DoctorName = "Doctor UserFail",
                Specialisation = "Neurologist",
                YearsOfExperience = 5,
                ConsultationFee = 600,
                Email = "doctorfail@test.com",
                PhoneNumber = "9876543211",
                Password = "weak"
            };

            var doctor = new Doctor
            {
                DoctorName = request.DoctorName,
                Specialisation = request.Specialisation,
                YearsOfExperience = request.YearsOfExperience,
                ConsultationFee = request.ConsultationFee,
                IsActive = true
            };

            var savedDoctor = new Doctor
            {
                DoctorId = 5,
                DoctorName = request.DoctorName,
                Specialisation = request.Specialisation,
                YearsOfExperience = request.YearsOfExperience,
                ConsultationFee = request.ConsultationFee,
                IsActive = true
            };

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            userManagerMock
                .Setup(x => x.CreateAsync(
                    It.IsAny<ApplicationUser>(),
                    request.Password))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError { Description = "Password is invalid" }));

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Doctor>(), ct))
                .ReturnsAsync(savedDoctor);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<Doctor>(request))
                .Returns(doctor);

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                userManagerMock: userManagerMock,
                mapperMock: mapperMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.CreateDoctorAsync(request, ct));

            Assert.Contains("Password is invalid", exception.Message);

            doctorRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Doctor>(), ct), Times.Once);
            userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Once);
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenDoctorRoleDoesNotExist_ShouldThrowInvalidException()
        {
            var ct = CancellationToken.None;

            var request = new CreateDoctorDto
            {
                DoctorName = "Doctor RoleMissing",
                Specialisation = "Dermatologist",
                YearsOfExperience = 6,
                ConsultationFee = 800,
                Email = "doctorrole@test.com",
                PhoneNumber = "9876543212",
                Password = "Doctor@123"
            };

            var doctor = new Doctor
            {
                DoctorName = request.DoctorName,
                Specialisation = request.Specialisation,
                YearsOfExperience = request.YearsOfExperience,
                ConsultationFee = request.ConsultationFee,
                IsActive = true
            };

            var savedDoctor = new Doctor
            {
                DoctorId = 10,
                DoctorName = request.DoctorName,
                Specialisation = request.Specialisation,
                YearsOfExperience = request.YearsOfExperience,
                ConsultationFee = request.ConsultationFee,
                IsActive = true
            };

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);

            var roleManagerMock = CreateRoleManagerMock();

            roleManagerMock
                .Setup(x => x.RoleExistsAsync("Doctor"))
                .ReturnsAsync(false);

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Doctor>(), ct))
                .ReturnsAsync(savedDoctor);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<Doctor>(request))
                .Returns(doctor);

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                userManagerMock: userManagerMock,
                roleManagerMock: roleManagerMock,
                mapperMock: mapperMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.CreateDoctorAsync(request, ct));

            Assert.Equal("Doctor role does not exist", exception.Message);

            roleManagerMock.Verify(x => x.RoleExistsAsync("Doctor"), Times.Once);
            userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Doctor"), Times.Never);
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenAddToRoleFails_ShouldThrowInvalidException()
        {
            var ct = CancellationToken.None;

            var request = new CreateDoctorDto
            {
                DoctorName = "Doctor RoleFail",
                Specialisation = "Psychiatrist",
                YearsOfExperience = 9,
                ConsultationFee = 900,
                Email = "doctoraddrole@test.com",
                PhoneNumber = "9876543213",
                Password = "Doctor@123"
            };

            var doctor = new Doctor
            {
                DoctorName = request.DoctorName,
                Specialisation = request.Specialisation,
                YearsOfExperience = request.YearsOfExperience,
                ConsultationFee = request.ConsultationFee,
                IsActive = true
            };

            var savedDoctor = new Doctor
            {
                DoctorId = 20,
                DoctorName = request.DoctorName,
                Specialisation = request.Specialisation,
                YearsOfExperience = request.YearsOfExperience,
                ConsultationFee = request.ConsultationFee,
                IsActive = true
            };

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Doctor"))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError { Description = "Failed to add role" }));

            var roleManagerMock = CreateRoleManagerMock();

            roleManagerMock
                .Setup(x => x.RoleExistsAsync("Doctor"))
                .ReturnsAsync(true);

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Doctor>(), ct))
                .ReturnsAsync(savedDoctor);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<Doctor>(request))
                .Returns(doctor);

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                userManagerMock: userManagerMock,
                roleManagerMock: roleManagerMock,
                mapperMock: mapperMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.CreateDoctorAsync(request, ct));

            Assert.Contains("Failed to add role", exception.Message);

            userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Doctor"), Times.Once);
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenValidRequest_ShouldCreateDoctorUserRoleAndReturnDoctorDto()
        {
            var ct = CancellationToken.None;

            var request = new CreateDoctorDto
            {
                DoctorName = "Doctor Success",
                Specialisation = "Cardiologist",
                YearsOfExperience = 11,
                ConsultationFee = 1000,
                Email = "doctorsuccess@test.com",
                PhoneNumber = "9876543214",
                Password = "Doctor@123"
            };

            var doctor = new Doctor
            {
                DoctorName = request.DoctorName,
                Specialisation = request.Specialisation,
                YearsOfExperience = request.YearsOfExperience,
                ConsultationFee = request.ConsultationFee,
                IsActive = false
            };

            var savedDoctor = new Doctor
            {
                DoctorId = 30,
                DoctorName = request.DoctorName,
                Specialisation = request.Specialisation,
                YearsOfExperience = request.YearsOfExperience,
                ConsultationFee = request.ConsultationFee,
                IsActive = true
            };

            var expectedDto = new DoctorDto
            {
                DoctorId = 30,
                DoctorName = request.DoctorName,
                Specialisation = request.Specialisation,
                YearsOfExperience = request.YearsOfExperience,
                ConsultationFee = request.ConsultationFee,
                IsActive = true
            };

            ApplicationUser? capturedUser = null;

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .Callback<ApplicationUser, string>((user, _) => capturedUser = user)
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            var roleManagerMock = CreateRoleManagerMock();

            roleManagerMock
                .Setup(x => x.RoleExistsAsync("Doctor"))
                .ReturnsAsync(true);

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Doctor>(), ct))
                .ReturnsAsync(savedDoctor);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<Doctor>(request))
                .Returns(doctor);

            mapperMock
                .Setup(x => x.Map<DoctorDto>(savedDoctor))
                .Returns(expectedDto);

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                userManagerMock: userManagerMock,
                roleManagerMock: roleManagerMock,
                mapperMock: mapperMock);

            var result = await service.CreateDoctorAsync(request, ct);

            Assert.Equal(expectedDto.DoctorId, result.DoctorId);
            Assert.Equal(expectedDto.DoctorName, result.DoctorName);
            Assert.True(result.IsActive);

            Assert.NotNull(capturedUser);
            Assert.Equal(request.Email, capturedUser!.Email);
            Assert.Equal(request.Email, capturedUser.UserName);
            Assert.Equal(request.PhoneNumber, capturedUser.PhoneNumber);
            Assert.Equal(savedDoctor.DoctorId, capturedUser.DoctorId);
            Assert.True(capturedUser.IsActive);
            Assert.True(capturedUser.EmailConfirmed);

            doctorRepositoryMock.Verify(x => x.CreateAsync(It.Is<Doctor>(d => d.IsActive), ct), Times.Once);
            userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Once);
            roleManagerMock.Verify(x => x.RoleExistsAsync("Doctor"), Times.Once);
            userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Doctor"), Times.Once);
            mapperMock.Verify(x => x.Map<DoctorDto>(savedDoctor), Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenDoctorDoesNotExist_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var request = new UpdateDoctorDto
            {
                DoctorName = "Doctor Missing",
                Specialisation = "Cardiologist",
                YearsOfExperience = 3,
                ConsultationFee = 300,
                IsActive = true
            };

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(999, ct))
                .ReturnsAsync((Doctor?)null);

            var service = CreateService(doctorRepositoryMock: doctorRepositoryMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.UpdateDoctorAsync(999, request, ct));

            Assert.Equal("Doctor not found", exception.Message);

            doctorRepositoryMock.Verify(x => x.GetByIdAsync(999, ct), Times.Once);
            doctorRepositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<int>(),
                It.IsAny<Doctor>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenUpdateReturnsNull_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var request = new UpdateDoctorDto
            {
                DoctorName = "Doctor Update Null",
                Specialisation = "Neurologist",
                YearsOfExperience = 7,
                ConsultationFee = 700,
                IsActive = true
            };

            var existingDoctor = new Doctor
            {
                DoctorId = 1,
                DoctorName = "Old Doctor",
                Specialisation = "Cardiologist",
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1, ct))
                .ReturnsAsync(existingDoctor);

            doctorRepositoryMock
                .Setup(x => x.UpdateAsync(1, existingDoctor, ct))
                .ReturnsAsync((Doctor?)null);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map(request, existingDoctor))
                .Callback<UpdateDoctorDto, Doctor>((source, destination) =>
                {
                    destination.DoctorName = source.DoctorName;
                    destination.Specialisation = source.Specialisation;
                    destination.YearsOfExperience = source.YearsOfExperience;
                    destination.ConsultationFee = source.ConsultationFee;
                    destination.IsActive = source.IsActive;
                })
                .Returns(existingDoctor);

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                mapperMock: mapperMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.UpdateDoctorAsync(1, request, ct));

            Assert.Equal("Doctor not found", exception.Message);

            doctorRepositoryMock.Verify(x => x.GetByIdAsync(1, ct), Times.Once);
            doctorRepositoryMock.Verify(x => x.UpdateAsync(1, existingDoctor, ct), Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenValidRequest_ShouldUpdateAndReturnDoctorDto()
        {
            var ct = CancellationToken.None;

            var request = new UpdateDoctorDto
            {
                DoctorName = "Doctor Updated",
                Specialisation = "Dermatologist",
                YearsOfExperience = 12,
                ConsultationFee = 1200,
                IsActive = false
            };

            var existingDoctor = new Doctor
            {
                DoctorId = 2,
                DoctorName = "Doctor Old",
                Specialisation = "Cardiologist",
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };

            var updatedDoctor = new Doctor
            {
                DoctorId = 2,
                DoctorName = request.DoctorName,
                Specialisation = request.Specialisation,
                YearsOfExperience = request.YearsOfExperience,
                ConsultationFee = request.ConsultationFee,
                IsActive = request.IsActive
            };

            var expectedDto = new DoctorDto
            {
                DoctorId = 2,
                DoctorName = request.DoctorName,
                Specialisation = request.Specialisation,
                YearsOfExperience = request.YearsOfExperience,
                ConsultationFee = request.ConsultationFee,
                IsActive = request.IsActive
            };

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(2, ct))
                .ReturnsAsync(existingDoctor);

            doctorRepositoryMock
                .Setup(x => x.UpdateAsync(2, existingDoctor, ct))
                .ReturnsAsync(updatedDoctor);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map(request, existingDoctor))
                .Callback<UpdateDoctorDto, Doctor>((source, destination) =>
                {
                    destination.DoctorName = source.DoctorName;
                    destination.Specialisation = source.Specialisation;
                    destination.YearsOfExperience = source.YearsOfExperience;
                    destination.ConsultationFee = source.ConsultationFee;
                    destination.IsActive = source.IsActive;
                })
                .Returns(existingDoctor);

            mapperMock
                .Setup(x => x.Map<DoctorDto>(updatedDoctor))
                .Returns(expectedDto);

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.UpdateDoctorAsync(2, request, ct);

            Assert.Equal(expectedDto.DoctorId, result.DoctorId);
            Assert.Equal(expectedDto.DoctorName, result.DoctorName);
            Assert.Equal(expectedDto.Specialisation, result.Specialisation);
            Assert.Equal(expectedDto.YearsOfExperience, result.YearsOfExperience);
            Assert.Equal(expectedDto.ConsultationFee, result.ConsultationFee);
            Assert.Equal(expectedDto.IsActive, result.IsActive);

            doctorRepositoryMock.Verify(x => x.GetByIdAsync(2, ct), Times.Once);
            doctorRepositoryMock.Verify(x => x.UpdateAsync(2, existingDoctor, ct), Times.Once);
            mapperMock.Verify(x => x.Map(request, existingDoctor), Times.Once);
            mapperMock.Verify(x => x.Map<DoctorDto>(updatedDoctor), Times.Once);
        }

        [Fact]
        public async Task GetUsersAsync_WhenRoleFilterIsNull_ShouldReturnPagedAllUsersWithFullNames()
        {
            var ct = CancellationToken.None;

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "user-2",
                    Email = "patient@test.com",
                    UserName = "patient@test.com",
                    PatientId = 10,
                    IsActive = false
                },
                new ApplicationUser
                {
                    Id = "user-1",
                    Email = "admin@test.com",
                    UserName = "admin@test.com",
                    IsActive = true
                },
                new ApplicationUser
                {
                    Id = "user-3",
                    Email = "doctor@test.com",
                    UserName = "doctor@test.com",
                    DoctorId = 20,
                    IsActive = true
                }
            };

            var userManagerMock = CreateUserManagerMock(CreateAsyncQueryable(users));

            userManagerMock
                .Setup(x => x.GetRolesAsync(users[1]))
                .ReturnsAsync(new List<string> { "Admin" });

            userManagerMock
                .Setup(x => x.GetRolesAsync(users[0]))
                .ReturnsAsync(new List<string> { "Patient" });

            userManagerMock
                .Setup(x => x.GetRolesAsync(users[2]))
                .ReturnsAsync(new List<string> { "Doctor" });

            var patientRepositoryMock = new Mock<IPatientRepository>();

            patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10, ct))
                .ReturnsAsync(new Patient
                {
                    PatientId = 10,
                    PatientName = "Patient One"
                });

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(20, ct))
                .ReturnsAsync(new Doctor
                {
                    DoctorId = 20,
                    DoctorName = "Doctor One"
                });

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                patientRepositoryMock: patientRepositoryMock,
                userManagerMock: userManagerMock);

            var query = new PaginationQueryDto
            {
                PageNumber = 1,
                PageSize = 2
            };

            var result = await service.GetUsersAsync(null, query, ct);

            Assert.Equal(2, result.Items.Count);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(2, result.TotalPages);

            Assert.Contains(result.Items, x => x.Id == "user-1" && x.Role == "Admin" && x.FullName == "admin@test.com");
            Assert.Contains(result.Items, x => x.Id == "user-3" && x.Role == "Doctor" && x.FullName == "Doctor One");
        }

        [Fact]
        public async Task GetUsersAsync_WhenRoleFilterIsProvided_ShouldReturnPagedUsersInRole()
        {
            var ct = CancellationToken.None;

            var usersInRole = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "patient-2",
                    Email = "bpatient@test.com",
                    UserName = "bpatient@test.com",
                    PatientId = 2,
                    IsActive = true
                },
                new ApplicationUser
                {
                    Id = "patient-1",
                    Email = "apatient@test.com",
                    UserName = "apatient@test.com",
                    PatientId = 1,
                    IsActive = true
                }
            };

            var userManagerMock = CreateUserManagerMock();

            userManagerMock
                .Setup(x => x.GetUsersInRoleAsync("Patient"))
                .ReturnsAsync(usersInRole);

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "Patient" });

            var patientRepositoryMock = new Mock<IPatientRepository>();

            patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1, ct))
                .ReturnsAsync(new Patient
                {
                    PatientId = 1,
                    PatientName = "Patient One"
                });

            var service = CreateService(
                patientRepositoryMock: patientRepositoryMock,
                userManagerMock: userManagerMock);

            var query = new PaginationQueryDto
            {
                PageNumber = 1,
                PageSize = 1
            };

            var result = await service.GetUsersAsync("Patient", query, ct);

            Assert.Single(result.Items);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.TotalPages);
            Assert.Equal("patient-1", result.Items[0].Id);
            Assert.Equal("Patient", result.Items[0].Role);
            Assert.Equal("Patient One", result.Items[0].FullName);

            userManagerMock.Verify(x => x.GetUsersInRoleAsync("Patient"), Times.Once);
        }

        [Fact]
        public async Task GetUsersAsync_WhenPatientOrDoctorEntityMissing_ShouldUseUserNameFallback()
        {
            var ct = CancellationToken.None;

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "user-1",
                    Email = "missingpatient@test.com",
                    UserName = "missing-patient-user",
                    PatientId = 99,
                    IsActive = true
                },
                new ApplicationUser
                {
                    Id = "user-2",
                    Email = "missingdoctor@test.com",
                    UserName = "missing-doctor-user",
                    DoctorId = 88,
                    IsActive = true
                }
            };

            var userManagerMock = CreateUserManagerMock(CreateAsyncQueryable(users));

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string>());

            var patientRepositoryMock = new Mock<IPatientRepository>();

            patientRepositoryMock
                .Setup(x => x.GetByIdAsync(99, ct))
                .ReturnsAsync((Patient?)null);

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(88, ct))
                .ReturnsAsync((Doctor?)null);

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                patientRepositoryMock: patientRepositoryMock,
                userManagerMock: userManagerMock);

            var query = new PaginationQueryDto
            {
                PageNumber = 1,
                PageSize = 10
            };

            var result = await service.GetUsersAsync(null, query, ct);

            Assert.Equal(2, result.Items.Count);
            Assert.Contains(result.Items, x => x.FullName == "missing-patient-user");
            Assert.Contains(result.Items, x => x.FullName == "missing-doctor-user");
        }

        [Fact]
        public async Task GetAppointmentReportAsync_ShouldReturnRepositoryReport()
        {
            var ct = CancellationToken.None;

            var reports = new List<AppointmentReportDto>
            {
                new AppointmentReportDto
                {
                    Date = new DateTime(2026, 06, 18),
                    Confirmed = 3,
                    Cancelled = 1,
                    Completed = 2
                }
            };

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.GetAppointmentReportAsync(ct))
                .ReturnsAsync(reports);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock);

            var result = await service.GetAppointmentReportAsync(ct);

            Assert.Single(result);
            Assert.Equal(3, result[0].Confirmed);
            Assert.Equal(1, result[0].Cancelled);
            Assert.Equal(2, result[0].Completed);

            appointmentRepositoryMock.Verify(x => x.GetAppointmentReportAsync(ct), Times.Once);
        }

        [Fact]
        public async Task UpdatePatientStatusAsync_WhenPatientDoesNotExist_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var patientRepositoryMock = new Mock<IPatientRepository>();

            patientRepositoryMock
                .Setup(x => x.GetByIdAsync(99, ct))
                .ReturnsAsync((Patient?)null);

            var service = CreateService(
                patientRepositoryMock: patientRepositoryMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.UpdatePatientStatusAsync(99, true, ct));

            Assert.Equal("Patient not found", exception.Message);

            patientRepositoryMock.Verify(x => x.GetByIdAsync(99, ct), Times.Once);
            patientRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePatientStatusAsync_WhenPatientExistsAndUserDoesNotExist_ShouldUpdatePatientOnlyAndSaveChanges()
        {
            var ct = CancellationToken.None;

            var patient = new Patient
            {
                PatientId = 10,
                PatientName = "Patient One",
                DateOfBirth = new DateTime(2000, 01, 01),
                Gender = "Male",
                Email = "patient@test.com",
                PhoneNumber = "9876543210",
                InsuranceID = "INS001",
                IsActive = true
            };

            var patientRepositoryMock = new Mock<IPatientRepository>();

            patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10, ct))
                .ReturnsAsync(patient);

            patientRepositoryMock
                .Setup(x => x.SaveChangesAsync(ct))
                .ReturnsAsync(1);

            var userManagerMock = CreateUserManagerMock(CreateAsyncQueryable(new List<ApplicationUser>()));

            var service = CreateService(
                patientRepositoryMock: patientRepositoryMock,
                userManagerMock: userManagerMock);

            await service.UpdatePatientStatusAsync(10, false, ct);

            Assert.False(patient.IsActive);

            userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            patientRepositoryMock.Verify(x => x.SaveChangesAsync(ct), Times.Once);
        }

        [Fact]
        public async Task UpdatePatientStatusAsync_WhenPatientExistsAndUserExists_ShouldUpdatePatientUserAndSaveChanges()
        {
            var ct = CancellationToken.None;

            var patient = new Patient
            {
                PatientId = 10,
                PatientName = "Patient One",
                IsActive = true
            };

            var user = new ApplicationUser
            {
                Id = "user-1",
                PatientId = 10,
                IsActive = true
            };

            var patientRepositoryMock = new Mock<IPatientRepository>();

            patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10, ct))
                .ReturnsAsync(patient);

            patientRepositoryMock
                .Setup(x => x.SaveChangesAsync(ct))
                .ReturnsAsync(1);

            var userManagerMock = CreateUserManagerMock(CreateAsyncQueryable(new List<ApplicationUser> { user }));

            userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            var service = CreateService(
                patientRepositoryMock: patientRepositoryMock,
                userManagerMock: userManagerMock);

            await service.UpdatePatientStatusAsync(10, false, ct);

            Assert.False(patient.IsActive);
            Assert.False(user.IsActive);

            userManagerMock.Verify(x => x.UpdateAsync(user), Times.Once);
            patientRepositoryMock.Verify(x => x.SaveChangesAsync(ct), Times.Once);
        }

        [Fact]
        public async Task UpdatePatientStatusAsync_WhenUserUpdateFails_ShouldThrowInvalidException()
        {
            var ct = CancellationToken.None;

            var patient = new Patient
            {
                PatientId = 10,
                PatientName = "Patient One",
                IsActive = true
            };

            var user = new ApplicationUser
            {
                Id = "user-1",
                PatientId = 10,
                IsActive = true
            };

            var patientRepositoryMock = new Mock<IPatientRepository>();

            patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10, ct))
                .ReturnsAsync(patient);

            var userManagerMock = CreateUserManagerMock(CreateAsyncQueryable(new List<ApplicationUser> { user }));

            userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError { Description = "Patient user update failed" }));

            var service = CreateService(
                patientRepositoryMock: patientRepositoryMock,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.UpdatePatientStatusAsync(10, false, ct));

            Assert.Contains("Patient user update failed", exception.Message);

            patientRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDoctorStatusAsync_WhenDoctorDoesNotExist_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(99, ct))
                .ReturnsAsync((Doctor?)null);

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.UpdateDoctorStatusAsync(99, true, ct));

            Assert.Equal("Doctor not found", exception.Message);

            doctorRepositoryMock.Verify(x => x.GetByIdAsync(99, ct), Times.Once);
            doctorRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDoctorStatusAsync_WhenDoctorExistsAndUserDoesNotExist_ShouldUpdateDoctorOnlyAndSaveChanges()
        {
            var ct = CancellationToken.None;

            var doctor = new Doctor
            {
                DoctorId = 10,
                DoctorName = "Doctor One",
                IsActive = true
            };

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(10, ct))
                .ReturnsAsync(doctor);

            doctorRepositoryMock
                .Setup(x => x.SaveChangesAsync(ct))
                .ReturnsAsync(1);

            var userManagerMock = CreateUserManagerMock(CreateAsyncQueryable(new List<ApplicationUser>()));

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                userManagerMock: userManagerMock);

            await service.UpdateDoctorStatusAsync(10, false, ct);

            Assert.False(doctor.IsActive);

            userManagerMock.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            doctorRepositoryMock.Verify(x => x.SaveChangesAsync(ct), Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorStatusAsync_WhenDoctorExistsAndUserExists_ShouldUpdateDoctorUserAndSaveChanges()
        {
            var ct = CancellationToken.None;

            var doctor = new Doctor
            {
                DoctorId = 10,
                DoctorName = "Doctor One",
                IsActive = true
            };

            var user = new ApplicationUser
            {
                Id = "user-1",
                DoctorId = 10,
                IsActive = true
            };

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(10, ct))
                .ReturnsAsync(doctor);

            doctorRepositoryMock
                .Setup(x => x.SaveChangesAsync(ct))
                .ReturnsAsync(1);

            var userManagerMock = CreateUserManagerMock(CreateAsyncQueryable(new List<ApplicationUser> { user }));

            userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                userManagerMock: userManagerMock);

            await service.UpdateDoctorStatusAsync(10, false, ct);

            Assert.False(doctor.IsActive);
            Assert.False(user.IsActive);

            userManagerMock.Verify(x => x.UpdateAsync(user), Times.Once);
            doctorRepositoryMock.Verify(x => x.SaveChangesAsync(ct), Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorStatusAsync_WhenUserUpdateFails_ShouldThrowInvalidException()
        {
            var ct = CancellationToken.None;

            var doctor = new Doctor
            {
                DoctorId = 10,
                DoctorName = "Doctor One",
                IsActive = true
            };

            var user = new ApplicationUser
            {
                Id = "user-1",
                DoctorId = 10,
                IsActive = true
            };

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(10, ct))
                .ReturnsAsync(doctor);

            var userManagerMock = CreateUserManagerMock(CreateAsyncQueryable(new List<ApplicationUser> { user }));

            userManagerMock
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError { Description = "Doctor user update failed" }));

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                userManagerMock: userManagerMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.UpdateDoctorStatusAsync(10, false, ct));

            Assert.Contains("Doctor user update failed", exception.Message);

            doctorRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        // ============================================================
        // Async IQueryable Helpers
        // Needed because AdminService uses CountAsync and ToListAsync
        // on userManager.Users.
        // ============================================================

        private static IQueryable<T> CreateAsyncQueryable<T>(IEnumerable<T> source)
        {
            return new TestAsyncEnumerable<T>(source);
        }

        private sealed class TestAsyncEnumerable<T> :
            EnumerableQuery<T>,
            IAsyncEnumerable<T>,
            IQueryable<T>
        {
            public TestAsyncEnumerable(IEnumerable<T> enumerable)
                : base(enumerable)
            {
            }

            public TestAsyncEnumerable(Expression expression)
                : base(expression)
            {
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(
                CancellationToken cancellationToken = default)
            {
                return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }

            IQueryProvider IQueryable.Provider =>
                new TestAsyncQueryProvider<T>(this);
        }

        private sealed class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public TestAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner;
            }

            public T Current => _inner.Current;

            public ValueTask DisposeAsync()
            {
                _inner.Dispose();

                return ValueTask.CompletedTask;
            }

            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(_inner.MoveNext());
            }
        }

        private sealed class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;

            public TestAsyncQueryProvider(IQueryProvider inner)
            {
                _inner = inner;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new TestAsyncEnumerable<TEntity>(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new TestAsyncEnumerable<TElement>(expression);
            }

            public object? Execute(Expression expression)
            {
                return _inner.Execute(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _inner.Execute<TResult>(expression);
            }

            public TResult ExecuteAsync<TResult>(
                Expression expression,
                CancellationToken cancellationToken = default)
            {
                var expectedResultType = typeof(TResult).GetGenericArguments()[0];

                var executionResult = typeof(IQueryProvider)
                    .GetMethod(
                        name: nameof(IQueryProvider.Execute),
                        genericParameterCount: 1,
                        types: new[] { typeof(Expression) })!
                    .MakeGenericMethod(expectedResultType)
                    .Invoke(_inner, new object[] { expression });

                return (TResult)typeof(Task)
                    .GetMethod(nameof(Task.FromResult))!
                    .MakeGenericMethod(expectedResultType)
                    .Invoke(null, new[] { executionResult })!;
            }
        }
    }
}