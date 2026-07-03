using Moq;
using S3_HealthAxis.Shared.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Implementation;
using Xunit;

namespace S3_HealthAxis.Tests.ServiceTests
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
        public async Task GetDashboardAsync_ShouldReturnDashboardCounts()
        {
            // Arrange
            _repositoryMock.Setup(r => r.CountPatientsAsync()).ReturnsAsync(100);
            _repositoryMock.Setup(r => r.CountActivePatientsAsync()).ReturnsAsync(80);
            _repositoryMock.Setup(r => r.CountDoctorsAsync()).ReturnsAsync(20);
            _repositoryMock.Setup(r => r.CountActiveDoctorsAsync()).ReturnsAsync(15);
            _repositoryMock.Setup(r => r.CountTodayAppointmentsAsync()).ReturnsAsync(12);
            _repositoryMock.Setup(r => r.CountPendingAppointmentsAsync()).ReturnsAsync(5);
            _repositoryMock.Setup(r => r.CountCompletedAppointmentsAsync()).ReturnsAsync(7);

            // Act
            var result = await _service.GetDashboardAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.TotalPatients);
            Assert.Equal(80, result.ActivePatients);
            Assert.Equal(20, result.TotalDoctors);
            Assert.Equal(15, result.ActiveDoctors);
            Assert.Equal(12, result.TodayAppointments);
            Assert.Equal(5, result.PendingAppointments);
            Assert.Equal(7, result.CompletedAppointments);

            _repositoryMock.Verify(r => r.CountPatientsAsync(), Times.Once);
            _repositoryMock.Verify(r => r.CountActivePatientsAsync(), Times.Once);
            _repositoryMock.Verify(r => r.CountDoctorsAsync(), Times.Once);
            _repositoryMock.Verify(r => r.CountActiveDoctorsAsync(), Times.Once);
            _repositoryMock.Verify(r => r.CountTodayAppointmentsAsync(), Times.Once);
            _repositoryMock.Verify(r => r.CountPendingAppointmentsAsync(), Times.Once);
            _repositoryMock.Verify(r => r.CountCompletedAppointmentsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetStatisticsAsync_ShouldReturnStatisticsCounts()
        {
            // Arrange
            _repositoryMock.Setup(r => r.CountPatientsAsync()).ReturnsAsync(45);
            _repositoryMock.Setup(r => r.CountDoctorsAsync()).ReturnsAsync(10);
            _repositoryMock.Setup(r => r.CountTodayAppointmentsAsync()).ReturnsAsync(8);
            _repositoryMock.Setup(r => r.CountHealthRecordsAsync()).ReturnsAsync(30);

            // Act
            var result = await _service.GetStatisticsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(45, result.Patients);
            Assert.Equal(10, result.Doctors);
            Assert.Equal(8, result.Appointments);
            Assert.Equal(30, result.HealthRecords);

            _repositoryMock.Verify(r => r.CountPatientsAsync(), Times.Once);
            _repositoryMock.Verify(r => r.CountDoctorsAsync(), Times.Once);
            _repositoryMock.Verify(r => r.CountTodayAppointmentsAsync(), Times.Once);
            _repositoryMock.Verify(r => r.CountHealthRecordsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnMappedUsersWithActiveStatus()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    Email = "admin@test.com",
                    Role = UserRole.Admin
                },
                new User
                {
                    UserId = 2,
                    Email = "doctor@test.com",
                    Role = UserRole.Doctor
                },
                new User
                {
                    UserId = 3,
                    Email = "patient@test.com",
                    Role = UserRole.Patient
                }
            };

            _repositoryMock.Setup(r => r.GetUsersAsync())
                .ReturnsAsync(users);

            _repositoryMock.Setup(r => r.ResolveUserActiveStatusAsync("admin@test.com", "Admin"))
                .ReturnsAsync(true);

            _repositoryMock.Setup(r => r.ResolveUserActiveStatusAsync("doctor@test.com", "Doctor"))
                .ReturnsAsync(false);

            _repositoryMock.Setup(r => r.ResolveUserActiveStatusAsync("patient@test.com", "Patient"))
                .ReturnsAsync(true);

            // Act
            var result = (await _service.GetUsersAsync()).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].UserId);
            Assert.Equal("admin@test.com", result[0].Email);
            Assert.Equal("Admin", result[0].Role);
            Assert.True(result[0].IsActive);

            Assert.Equal(2, result[1].UserId);
            Assert.Equal("doctor@test.com", result[1].Email);
            Assert.Equal("Doctor", result[1].Role);
            Assert.False(result[1].IsActive);

            Assert.Equal(3, result[2].UserId);
            Assert.Equal("patient@test.com", result[2].Email);
            Assert.Equal("Patient", result[2].Role);
            Assert.True(result[2].IsActive);

            _repositoryMock.Verify(r => r.GetUsersAsync(), Times.Once);
            _repositoryMock.Verify(r => r.ResolveUserActiveStatusAsync("admin@test.com", "Admin"), Times.Once);
            _repositoryMock.Verify(r => r.ResolveUserActiveStatusAsync("doctor@test.com", "Doctor"), Times.Once);
            _repositoryMock.Verify(r => r.ResolveUserActiveStatusAsync("patient@test.com", "Patient"), Times.Once);
        }

        [Fact]
        public async Task GetUsersAsync_WhenNoUsers_ShouldReturnEmptyList()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUsersAsync())
                .ReturnsAsync(new List<User>());

            // Act
            var result = (await _service.GetUsersAsync()).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _repositoryMock.Verify(r => r.GetUsersAsync(), Times.Once);
            _repositoryMock.Verify(
                r => r.ResolveUserActiveStatusAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserExists_ShouldReturnMappedUser()
        {
            // Arrange
            var user = new User
            {
                UserId = 10,
                Email = "doctor@test.com",
                Role = UserRole.Doctor
            };

            _repositoryMock.Setup(r => r.GetUserByIdAsync(10))
                .ReturnsAsync(user);

            _repositoryMock.Setup(r => r.ResolveUserActiveStatusAsync("doctor@test.com", "Doctor"))
                .ReturnsAsync(true);

            // Act
            var result = await _service.GetUserByIdAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.UserId);
            Assert.Equal("doctor@test.com", result.Email);
            Assert.Equal("Doctor", result.Role);
            Assert.True(result.IsActive);

            _repositoryMock.Verify(r => r.GetUserByIdAsync(10), Times.Once);
            _repositoryMock.Verify(r => r.ResolveUserActiveStatusAsync("doctor@test.com", "Doctor"), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(99))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _service.GetUserByIdAsync(99);

            // Assert
            Assert.Null(result);

            _repositoryMock.Verify(r => r.GetUserByIdAsync(99), Times.Once);
            _repositoryMock.Verify(
                r => r.ResolveUserActiveStatusAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenPatientUserExists_ShouldReturnPatientRoleAndInactiveStatus()
        {
            // Arrange
            var user = new User
            {
                UserId = 20,
                Email = "patient@test.com",
                Role = UserRole.Patient
            };

            _repositoryMock.Setup(r => r.GetUserByIdAsync(20))
                .ReturnsAsync(user);

            _repositoryMock.Setup(r => r.ResolveUserActiveStatusAsync("patient@test.com", "Patient"))
                .ReturnsAsync(false);

            // Act
            var result = await _service.GetUserByIdAsync(20);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(20, result.UserId);
            Assert.Equal("patient@test.com", result.Email);
            Assert.Equal("Patient", result.Role);
            Assert.False(result.IsActive);

            _repositoryMock.Verify(r => r.GetUserByIdAsync(20), Times.Once);
            _repositoryMock.Verify(r => r.ResolveUserActiveStatusAsync("patient@test.com", "Patient"), Times.Once);
        }
    }
}