using Moq;
using S3_HealthAxis.Shared.DTOs.Admin;
using S3_HealthAxis.Shared.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Implementation;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace S3_HealthAxis.Tests.Services
{
    public class AdminServiceTests
    {
        private readonly Mock<IAdminRepository> _repoMock;
        private readonly AdminService _service;

        public AdminServiceTests()
        {
            _repoMock = new Mock<IAdminRepository>();
            _service = new AdminService(_repoMock.Object);
        }

        #region Dashboard Tests

        [Fact]
        public async Task GetDashboardAsync_ShouldReturnCorrectCounts()
        {
            // Arrange
            _repoMock.Setup(r => r.CountPatientsAsync()).ReturnsAsync(100);
            _repoMock.Setup(r => r.CountActivePatientsAsync()).ReturnsAsync(80);
            _repoMock.Setup(r => r.CountDoctorsAsync()).ReturnsAsync(20);
            _repoMock.Setup(r => r.CountActiveDoctorsAsync()).ReturnsAsync(15);
            _repoMock.Setup(r => r.CountTodayAppointmentsAsync()).ReturnsAsync(10);
            _repoMock.Setup(r => r.CountPendingAppointmentsAsync()).ReturnsAsync(5);
            _repoMock.Setup(r => r.CountCompletedAppointmentsAsync()).ReturnsAsync(7);

            // Act
            var result = await _service.GetDashboardAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.TotalPatients);
            Assert.Equal(80, result.ActivePatients);
            Assert.Equal(20, result.TotalDoctors);
            Assert.Equal(15, result.ActiveDoctors);
            Assert.Equal(10, result.TodayAppointments);
            Assert.Equal(5, result.PendingAppointments);
            Assert.Equal(7, result.CompletedAppointments);

            _repoMock.Verify(r => r.CountPatientsAsync(), Times.Once);
        }

        #endregion

        #region Statistics Tests

        [Fact]
        public async Task GetStatisticsAsync_ShouldReturnCorrectData()
        {
            // Arrange
            _repoMock.Setup(r => r.CountPatientsAsync()).ReturnsAsync(200);
            _repoMock.Setup(r => r.CountDoctorsAsync()).ReturnsAsync(25);
            _repoMock.Setup(r => r.CountTodayAppointmentsAsync()).ReturnsAsync(12);
            _repoMock.Setup(r => r.CountHealthRecordsAsync()).ReturnsAsync(50);

            // Act
            var result = await _service.GetStatisticsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Patients);
            Assert.Equal(25, result.Doctors);
            Assert.Equal(12, result.Appointments);
            Assert.Equal(50, result.HealthRecords);
        }

        #endregion

        #region GetUsers Tests

        [Fact]
        public async Task GetUsersAsync_ShouldReturnMappedUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, Email = "admin@test.com", Role = UserRole.Admin },
                new User { UserId = 2, Email = "doc@test.com", Role = UserRole.Doctor }
            };

            _repoMock.Setup(r => r.GetUsersAsync()).ReturnsAsync(users);

            _repoMock.Setup(r => r.ResolveUserActiveStatusAsync(It.IsAny<string>(), It.IsAny<string>()))
                     .ReturnsAsync(true);

            // Act
            var result = await _service.GetUsersAsync();
            var list = result.ToList();

            // Assert
            Assert.Equal(2, list.Count);

            Assert.Equal("admin@test.com", list[0].Email);
            Assert.Equal("Admin", list[0].Role);
            Assert.True(list[0].IsActive);

            Assert.Equal("doc@test.com", list[1].Email);
            Assert.Equal("Doctor", list[1].Role);
            Assert.True(list[1].IsActive);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldHandleEmptyList()
        {
            // Arrange
            _repoMock.Setup(r => r.GetUsersAsync())
                     .ReturnsAsync(new List<User>());

            // Act
            var result = await _service.GetUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetUserById Tests

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenFound()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Email = "user@test.com",
                Role = UserRole.Patient
            };

            _repoMock.Setup(r => r.GetUserByIdAsync(1))
                     .ReturnsAsync(user);

            _repoMock.Setup(r => r.ResolveUserActiveStatusAsync(user.Email, "Patient"))
                     .ReturnsAsync(true);

            // Act
            var result = await _service.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
            Assert.Equal("user@test.com", result.Email);
            Assert.Equal("Patient", result.Role);
            Assert.True(result.IsActive);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((User?)null);

            // Act
            var result = await _service.GetUserByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Edge Case Tests

        [Fact]
        public async Task GetUsersAsync_ShouldCallResolveStatusForEachUser()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, Email = "a@test.com", Role = UserRole.Admin },
                new User { UserId = 2, Email = "b@test.com", Role = UserRole.Patient }
            };

            _repoMock.Setup(r => r.GetUsersAsync()).ReturnsAsync(users);

            _repoMock.Setup(r => r.ResolveUserActiveStatusAsync(It.IsAny<string>(), It.IsAny<string>()))
                     .ReturnsAsync(true);

            // Act
            await _service.GetUsersAsync();

            // Assert
            _repoMock.Verify(r =>
                r.ResolveUserActiveStatusAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Exactly(users.Count));
        }

        #endregion
    }
}
