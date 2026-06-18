using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

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
                .Returns(users ?? new List<ApplicationUser>().AsQueryable());

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
        public async Task GetDoctorsAsync_WhenDoctorsExist_ShouldReturnMappedDoctorDtos()
        {
            var ct = CancellationToken.None;

            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    DoctorName = "Doctor One",
                    Specialisation = "Cardiologist",
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                }
            };

            var doctorDtos = new List<DoctorDto>
            {
                new DoctorDto
                {
                    DoctorId = 1,
                    DoctorName = "Doctor One",
                    Specialisation = "Cardiologist",
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                }
            };

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetAllAsync(ct))
                .ReturnsAsync(doctors);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetDoctorsAsync(ct);

            Assert.Single(result);
            Assert.Equal(1, result[0].DoctorId);
            Assert.Equal("Doctor One", result[0].DoctorName);

            doctorRepositoryMock.Verify(x => x.GetAllAsync(ct), Times.Once);
            mapperMock.Verify(x => x.Map<List<DoctorDto>>(doctors), Times.Once);
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
            doctorRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<Doctor>(), It.IsAny<CancellationToken>()), Times.Never);
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
        public async Task GetUsersAsync_WhenRoleFilterIsNull_ShouldReturnAllUsers()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "user-1",
                    Email = "admin@test.com",
                    IsActive = true
                },
                new ApplicationUser
                {
                    Id = "user-2",
                    Email = "patient@test.com",
                    IsActive = false
                },
                new ApplicationUser
                {
                    Id = "user-3",
                    Email = "norole@test.com",
                    IsActive = true
                }
            };

            var userManagerMock = CreateUserManagerMock(users.AsQueryable());

            userManagerMock
                .Setup(x => x.GetRolesAsync(users[0]))
                .ReturnsAsync(new List<string> { "Admin" });

            userManagerMock
                .Setup(x => x.GetRolesAsync(users[1]))
                .ReturnsAsync(new List<string> { "Patient" });

            userManagerMock
                .Setup(x => x.GetRolesAsync(users[2]))
                .ReturnsAsync(new List<string>());

            var service = CreateService(userManagerMock: userManagerMock);

            var result = await service.GetUsersAsync(null);

            Assert.Equal(3, result.Count);

            Assert.Contains(result, x => x.Id == "user-1" && x.Role == "Admin");
            Assert.Contains(result, x => x.Id == "user-2" && x.Role == "Patient");
            Assert.Contains(result, x => x.Id == "user-3" && x.Role == string.Empty);

            userManagerMock.Verify(x => x.GetRolesAsync(users[0]), Times.Once);
            userManagerMock.Verify(x => x.GetRolesAsync(users[1]), Times.Once);
            userManagerMock.Verify(x => x.GetRolesAsync(users[2]), Times.Once);
        }

        [Fact]
        public async Task GetUsersAsync_WhenRoleFilterIsProvided_ShouldReturnOnlyMatchingUsers()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "user-1",
                    Email = "admin@test.com",
                    IsActive = true
                },
                new ApplicationUser
                {
                    Id = "user-2",
                    Email = "patient@test.com",
                    IsActive = true
                }
            };

            var userManagerMock = CreateUserManagerMock(users.AsQueryable());

            userManagerMock
                .Setup(x => x.GetRolesAsync(users[0]))
                .ReturnsAsync(new List<string> { "Admin" });

            userManagerMock
                .Setup(x => x.GetRolesAsync(users[1]))
                .ReturnsAsync(new List<string> { "Patient" });

            var service = CreateService(userManagerMock: userManagerMock);

            var result = await service.GetUsersAsync("Admin");

            Assert.Single(result);
            Assert.Equal("user-1", result[0].Id);
            Assert.Equal("admin@test.com", result[0].Email);
            Assert.Equal("Admin", result[0].Role);
            Assert.True(result[0].IsActive);
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
        public async Task UpdatePatientStatusAsync_WhenPatientExists_ShouldUpdateStatusAndSaveChanges()
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

            var service = CreateService(
                patientRepositoryMock: patientRepositoryMock);

            await service.UpdatePatientStatusAsync(10, false, ct);

            Assert.False(patient.IsActive);

            patientRepositoryMock.Verify(x => x.GetByIdAsync(10, ct), Times.Once);
            patientRepositoryMock.Verify(x => x.SaveChangesAsync(ct), Times.Once);
        }
    }
}
