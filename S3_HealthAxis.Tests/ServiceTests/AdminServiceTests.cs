using FluentAssertions;
using Moq;
using S3_HealthAxisApi.DTOs.Admin;
using S3_HealthAxisApi.Enums;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Implementation;
using Xunit;

// Change this namespace if your User entity is in another folder/namespace
using S3_HealthAxisApi.Models;

namespace S3_HealthAxis.Tests.Services
{
    public class AdminServiceTests
    {
        private readonly Mock<IAdminRepository> _repositoryMock;
        private readonly AdminService _service;

        public AdminServiceTests()
        {
            _repositoryMock = new Mock<IAdminRepository>();
            _service = new AdminService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetDashboardAsync_ShouldReturnCorrectDashboardCounts()
        {
            // Arrange
            _repositoryMock.Setup(x => x.CountPatientsAsync()).ReturnsAsync(100);
            _repositoryMock.Setup(x => x.CountActivePatientsAsync()).ReturnsAsync(80);
            _repositoryMock.Setup(x => x.CountDoctorsAsync()).ReturnsAsync(25);
            _repositoryMock.Setup(x => x.CountActiveDoctorsAsync()).ReturnsAsync(20);
            _repositoryMock.Setup(x => x.CountTodayAppointmentsAsync()).ReturnsAsync(15);
            _repositoryMock.Setup(x => x.CountPendingAppointmentsAsync()).ReturnsAsync(7);
            _repositoryMock.Setup(x => x.CountCompletedAppointmentsAsync()).ReturnsAsync(5);

            // Act
            AdminDashboardDto result = await _service.GetDashboardAsync();

            // Assert
            result.Should().NotBeNull();
            result.TotalPatients.Should().Be(100);
            result.ActivePatients.Should().Be(80);
            result.TotalDoctors.Should().Be(25);
            result.ActiveDoctors.Should().Be(20);
            result.TodayAppointments.Should().Be(15);
            result.PendingAppointments.Should().Be(7);
            result.CompletedAppointments.Should().Be(5);

            _repositoryMock.Verify(x => x.CountPatientsAsync(), Times.Once);
            _repositoryMock.Verify(x => x.CountActivePatientsAsync(), Times.Once);
            _repositoryMock.Verify(x => x.CountDoctorsAsync(), Times.Once);
            _repositoryMock.Verify(x => x.CountActiveDoctorsAsync(), Times.Once);
            _repositoryMock.Verify(x => x.CountTodayAppointmentsAsync(), Times.Once);
            _repositoryMock.Verify(x => x.CountPendingAppointmentsAsync(), Times.Once);
            _repositoryMock.Verify(x => x.CountCompletedAppointmentsAsync(), Times.Once);

            _repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetStatisticsAsync_ShouldReturnCorrectStatistics()
        {
            // Arrange
            _repositoryMock.Setup(x => x.CountPatientsAsync()).ReturnsAsync(120);
            _repositoryMock.Setup(x => x.CountDoctorsAsync()).ReturnsAsync(30);
            _repositoryMock.Setup(x => x.CountTodayAppointmentsAsync()).ReturnsAsync(18);
            _repositoryMock.Setup(x => x.CountHealthRecordsAsync()).ReturnsAsync(92);

            // Act
            AdminStatisticsDto result = await _service.GetStatisticsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Patients.Should().Be(120);
            result.Doctors.Should().Be(30);
            result.Appointments.Should().Be(18);
            result.HealthRecords.Should().Be(92);

            _repositoryMock.Verify(x => x.CountPatientsAsync(), Times.Once);
            _repositoryMock.Verify(x => x.CountDoctorsAsync(), Times.Once);
            _repositoryMock.Verify(x => x.CountTodayAppointmentsAsync(), Times.Once);
            _repositoryMock.Verify(x => x.CountHealthRecordsAsync(), Times.Once);

            _repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetUsersAsync_ShouldMapAllUsersCorrectly()
        {
            // Arrange
            var users = new List<User>
            {
                CreateUser(1, "admin@test.com", UserRole.Admin, 101),
                CreateUser(2, "doctor@test.com", UserRole.Doctor, 202),
                CreateUser(3, "patient@test.com", UserRole.Patient, 303)
            };

            _repositoryMock
                .Setup(x => x.GetUsersAsync())
                .ReturnsAsync(users);

            // Act
            IEnumerable<UserManagementDto> result = await _service.GetUsersAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);

            var resultList = result.ToList();

            resultList[0].UserId.Should().Be(1);
            resultList[0].Email.Should().Be("admin@test.com");
            resultList[0].Role.Should().Be("Admin");
            resultList[0].ReferenceId.Should().Be(101);

            resultList[1].UserId.Should().Be(2);
            resultList[1].Email.Should().Be("doctor@test.com");
            resultList[1].Role.Should().Be("Doctor");
            resultList[1].ReferenceId.Should().Be(202);

            resultList[2].UserId.Should().Be(3);
            resultList[2].Email.Should().Be("patient@test.com");
            resultList[2].Role.Should().Be("Patient");
            resultList[2].ReferenceId.Should().Be(303);

            _repositoryMock.Verify(x => x.GetUsersAsync(), Times.Once);
            _repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnEmptyCollection_WhenNoUsersExist()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetUsersAsync())
                .ReturnsAsync(new List<User>());

            // Act
            IEnumerable<UserManagementDto> result = await _service.GetUsersAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _repositoryMock.Verify(x => x.GetUsersAsync(), Times.Once);
            _repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnMappedUser_WhenUserExists()
        {
            // Arrange
            var user = CreateUser(10, "singleuser@test.com", UserRole.Doctor, 999);

            _repositoryMock
                .Setup(x => x.GetUserByIdAsync(10))
                .ReturnsAsync(user);

            // Act
            UserManagementDto? result = await _service.GetUserByIdAsync(10);

            // Assert
            result.Should().NotBeNull();
            result!.UserId.Should().Be(10);
            result.Email.Should().Be("singleuser@test.com");
            result.Role.Should().Be("Doctor");
            result.ReferenceId.Should().Be(999);

            _repositoryMock.Verify(x => x.GetUserByIdAsync(10), Times.Once);
            _repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetUserByIdAsync(404))
                .ReturnsAsync((User?)null);

            // Act
            UserManagementDto? result = await _service.GetUserByIdAsync(404);

            // Assert
            result.Should().BeNull();

            _repositoryMock.Verify(x => x.GetUserByIdAsync(404), Times.Once);
            _repositoryMock.VerifyNoOtherCalls();
        }

        private static User CreateUser(int userId, string email, UserRole role, int? referenceId)
        {
            return new User
            {
                UserId = userId,
                Email = email,
                Role = role,
                ReferenceId = referenceId
            };
        }
    }
}