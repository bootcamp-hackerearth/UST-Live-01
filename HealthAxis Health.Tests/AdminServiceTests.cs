using AutoMapper;
using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.DTOs.DoctorDtos;
using HealthAxisHealth.API.DTOs.UserDtos;
using HealthAxisHealth.API.Enums;
using HealthAxisHealth.API.Exceptions;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Services.Implementations;
using HealthAxisHealth.API.UnitOfWork;
using Moq;
using Xunit;

namespace HealthAxisHealth.Tests.Services
{
    public class AdminServiceTests
    {
        #region Fields

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly Mock<IMapper> _mapperMock;

        private readonly AdminService _service;

        #endregion

        #region Constructor

        public AdminServiceTests()
        {
            _unitOfWorkMock =
                new Mock<IUnitOfWork>();

            _mapperMock =
                new Mock<IMapper>();

            _service =
                new AdminService(
                    _unitOfWorkMock.Object,
                    _mapperMock.Object);
        }

        #endregion

        [Fact]
        public async Task GetDoctorsAsync_ShouldReturnPagedDoctors()
        {
            // Arrange
            var pagination = new PaginationParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            var doctors = new List<Doctor>
    {
        new Doctor
        {
            DoctorId = 1,
            FullName = "Dr. John"
        }
    };

            var doctorDtos = new List<DoctorDto>
    {
        new DoctorDto
        {
            DoctorId = 1,
            FullName = "Dr. John"
        }
    };

            var pagedResult = new PagedResultDto<Doctor>
            {
                Items = doctors,
                PageNumber = 1,
                PageSize = 10,
                TotalRecords = 1
            };

            _unitOfWorkMock
                .Setup(x => x.Doctors.GetPagedAsync(pagination))
                .ReturnsAsync(pagedResult);

            _mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            // Act
            var result = await _service.GetDoctorsAsync(pagination);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalRecords);
        }

        [Fact]
        public async Task CreateDoctorAsync_ShouldThrowBadRequest_WhenEmailExists()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                Email = "doctor@test.com",
                Password = "Password@123"
            };

            _unitOfWorkMock
                .Setup(x => x.Users.GetByEmailAsync(dto.Email))
                .ReturnsAsync(new User());

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.CreateDoctorAsync(dto));
        }

        [Fact]
        public async Task CreateDoctorAsync_ShouldCreateDoctorSuccessfully()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                Email = "doctor@test.com",
                Password = "Password@123",
                FullName = "Doctor",
                ConsultationFee = 500,
                YearsOfExperience = 5,
                Specialisation = Specialisation.Cardiology
            };

            _unitOfWorkMock
                .Setup(x => x.Users.GetByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);

            _unitOfWorkMock
                .Setup(x => x.Users.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(x => x.Doctors.AddAsync(It.IsAny<Doctor>()))
                .Returns(Task.CompletedTask);

            // CommitAsync returns Task<int>
            _unitOfWorkMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            // Act
            await _service.CreateDoctorAsync(dto);

            // Assert
            _unitOfWorkMock.Verify(
                x => x.Users.AddAsync(It.IsAny<User>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.Doctors.AddAsync(It.IsAny<Doctor>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Exactly(2));
        }

        [Fact]
        public async Task UpdateDoctorAsync_ShouldThrowNotFound_WhenDoctorNotFound()
        {
            // Arrange
            _unitOfWorkMock
                .Setup(x => x.Doctors.GetDoctorWithAppointmentsAsync(1))
                .ReturnsAsync((Doctor?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateDoctorAsync(
                    1,
                    new UpdateDoctorDto()));
        }

        [Fact]
        public async Task UpdateDoctorAsync_ShouldThrowBadRequest_WhenDoctorHasUpcomingAppointments()
        {
            // Arrange
            var doctor = new Doctor
            {
                DoctorId = 1,
                UserId = 10,
                Appointments = new List<Appointment>
        {
            new Appointment
            {
                ScheduledDate = DateTime.Today.AddDays(1),
                Status = AppointmentStatus.Pending
            }
        }
            };

            _unitOfWorkMock
                .Setup(x => x.Doctors.GetDoctorWithAppointmentsAsync(1))
                .ReturnsAsync(doctor);

            var dto = new UpdateDoctorDto
            {
                IsActive = false
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.UpdateDoctorAsync(1, dto));
        }
        [Fact]
        public async Task UpdateDoctorAsync_ShouldThrowNotFound_WhenUserNotFound()
        {
            // Arrange
            var doctor = new Doctor
            {
                DoctorId = 1,
                UserId = 100,
                Appointments = new List<Appointment>()
            };

            var dto = new UpdateDoctorDto
            {
                FullName = "Updated Doctor",
                IsActive = true
            };

            _unitOfWorkMock
                .Setup(x => x.Doctors.GetDoctorWithAppointmentsAsync(1))
                .ReturnsAsync(doctor);

            _unitOfWorkMock
                .Setup(x => x.Users.GetByIdAsync(100))
                .ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateDoctorAsync(1, dto));
        }

        [Fact]
        public async Task UpdateDoctorAsync_ShouldUpdateDoctorSuccessfully()
        {
            // Arrange
            var doctor = new Doctor
            {
                DoctorId = 1,
                UserId = 10,
                Appointments = new List<Appointment>()
            };

            var user = new User
            {
                UserId = 10,
                IsActive = true
            };

            var dto = new UpdateDoctorDto
            {
                FullName = "Updated Doctor",
                IsActive = false
            };

            _unitOfWorkMock
                .Setup(x => x.Doctors.GetDoctorWithAppointmentsAsync(1))
                .ReturnsAsync(doctor);

            _unitOfWorkMock
                .Setup(x => x.Users.GetByIdAsync(10))
                .ReturnsAsync(user);

            _unitOfWorkMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            // Act
            await _service.UpdateDoctorAsync(1, dto);

            // Assert
            _mapperMock.Verify(
                x => x.Map(dto, doctor),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.Doctors.Update(doctor),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.Users.Update(user),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);

            Assert.False(user.IsActive);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnPagedUsers()
        {
            // Arrange
            var pagination = new PaginationParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            var users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    Email = "admin@test.com",
                    Role = UserRole.Admin
                }
            };

            var userDtos = new List<UserDto>
            {
                new UserDto
                {
                    UserId = 1,
                    Email = "admin@test.com",
                    Role = UserRole.Admin
                }
            };

            var pagedUsers = new PagedResultDto<User>
            {
                Items = users,
                PageNumber = 1,
                PageSize = 10,
                TotalRecords = 1
            };

            _unitOfWorkMock
                .Setup(x => x.Users.GetPagedAsync(
                    pagination,
                    UserRole.Admin))
                .ReturnsAsync(pagedUsers);

            _mapperMock
                .Setup(x => x.Map<List<UserDto>>(users))
                .Returns(userDtos);

            // Act
            var result =
                await _service.GetUsersAsync(
                    pagination,
                    "Admin");

            // Assert
            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalRecords);
        }

        [Fact]
        public async Task GetAppointmentReportAsync_ShouldReturnReport()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    ScheduledDate = DateTime.Today,
                    Status = AppointmentStatus.Confirmed
                },
                new Appointment
                {
                    ScheduledDate = DateTime.Today,
                    Status = AppointmentStatus.Completed
                },
                new Appointment
                {
                    ScheduledDate = DateTime.Today,
                    Status = AppointmentStatus.Cancelled
                }
            };

            _unitOfWorkMock
                .Setup(x => x.Appointments.GetAllAsync())
                .ReturnsAsync(appointments);

            // Act
            var result =
                await _service.GetAppointmentReportAsync();

            // Assert
            var report = Assert.Single(result);

            Assert.Equal(1, report.ConfirmedCount);
            Assert.Equal(1, report.CompletedCount);
            Assert.Equal(1, report.CancelledCount);
        }
        [Fact]
        public async Task GetUsersAsync_ShouldReturnUsers_WhenRoleIsNull()
        {
            // Arrange
            var pagination = new PaginationParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            var users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    Email = "user@test.com",
                    Role = UserRole.Patient
                }
            };

            var pagedResult = new PagedResultDto<User>
            {
                Items = users,
                PageNumber = 1,
                PageSize = 10,
                TotalRecords = 1
            };

            var userDtos = new List<UserDto>
            {
                 new UserDto
                {
                    UserId = 1,
                    Email = "user@test.com",
                    Role = UserRole.Patient
                }
           };

            _unitOfWorkMock
                .Setup(x => x.Users.GetPagedAsync(
                    pagination,
                    null))
                .ReturnsAsync(pagedResult);

            _mapperMock
                .Setup(x => x.Map<List<UserDto>>(users))
                .Returns(userDtos);

            // Act
            var result =
                await _service.GetUsersAsync(
                    pagination,
                    null);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalRecords);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnUsers_WhenRoleIsInvalid()
        {
            // Arrange
            var pagination = new PaginationParams
            {
                PageNumber = 1,
                PageSize = 10
            };

            var users = new List<User>();

            var pagedResult = new PagedResultDto<User>
            {
                Items = users,
                PageNumber = 1,
                PageSize = 10,
                TotalRecords = 0
            };

            _unitOfWorkMock
                .Setup(x => x.Users.GetPagedAsync(
                    pagination,
                    null))
                .ReturnsAsync(pagedResult);

            _mapperMock
                .Setup(x => x.Map<List<UserDto>>(users))
                .Returns(new List<UserDto>());

            // Act
            var result =
                await _service.GetUsersAsync(
                    pagination,
                    "InvalidRole");

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalRecords);
        }

        [Fact]
        public async Task CreateDoctorAsync_ShouldReturnDoctorId()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                Email = "doctor@test.com",
                Password = "Password@123",
                FullName = "Doctor",
                YearsOfExperience = 10,
                ConsultationFee = 750,
                Specialisation = Specialisation.Cardiology
            };

            _unitOfWorkMock
                .Setup(x => x.Users.GetByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);

            _unitOfWorkMock
                .Setup(x => x.Users.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => u.UserId = 100)
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(x => x.Doctors.AddAsync(It.IsAny<Doctor>()))
                .Callback<Doctor>(d => d.DoctorId = 200)
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            // Act
            var result =
                await _service.CreateDoctorAsync(dto);

            // Assert
            Assert.Equal(200, result);
        }

        [Fact]
        public async Task GetAppointmentReportAsync_ShouldReturnEmptyCollection_WhenNoAppointmentsExist()
        {
            // Arrange
            _unitOfWorkMock
                .Setup(x => x.Appointments.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            // Act
            var result =
                await _service.GetAppointmentReportAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAppointmentReportAsync_ShouldGroupAppointmentsByDate()
        {
            // Arrange
            var appointments = new List<Appointment>
        {
        new Appointment
            {
                ScheduledDate = DateTime.Today,
                Status = AppointmentStatus.Confirmed
            },
        new Appointment
            {
                ScheduledDate = DateTime.Today.AddDays(1),
                Status = AppointmentStatus.Completed
            }
         };

            _unitOfWorkMock
                .Setup(x => x.Appointments.GetAllAsync())
                .ReturnsAsync(appointments);

            // Act
            var result =
                (await _service.GetAppointmentReportAsync())
                .ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(
                result,
                r => r.Date == DateTime.Today);

            Assert.Contains(
                result,
                r => r.Date == DateTime.Today.AddDays(1));
        }
    }
}
