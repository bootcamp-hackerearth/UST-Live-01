using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthApp.Api.Services.Impl;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Exceptions;
using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Tests.Services
{
    public class AdminServiceTests
    {
        private readonly Mock<IAdminRepository> _adminRepo;
        private readonly AdminService _service;

        public AdminServiceTests()
        {
            _adminRepo = new Mock<IAdminRepository>();
            _service = new AdminService(_adminRepo.Object);
        }

        #region GetUsersAsync Tests

        [Fact]
        public async Task GetUsersAsync_ShouldReturnAllUsers_WhenRoleIsNull()
        {
            // Arrange
            var users = new List<AdminUserDto>
            {
                new AdminUserDto { Id = "1", Email = "user1@test.com" },
                new AdminUserDto { Id = "2", Email = "user2@test.com" }
            };

            _adminRepo.Setup(x => x.GetUsersAsync(null))
                .ReturnsAsync(users);

            // Act
            var result = await _service.GetUsersAsync(null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<AdminUserDto>)result).Count);
            _adminRepo.Verify(x => x.GetUsersAsync(null), Times.Once);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnFilteredUsers_WhenRoleProvided()
        {
            // Arrange
            var role = "Patient";
            var users = new List<AdminUserDto>
            {
                new AdminUserDto
                {
                    Id = "1",
                    Email = "a@test.com",
                    Roles = new List<string> { "Patient" }
                }
            };

            _adminRepo.Setup(x => x.GetUsersAsync(role))
                .ReturnsAsync(users);

            // Act
            var result = await _service.GetUsersAsync(role);

            // Assert
            Assert.Single(result);
            _adminRepo.Verify(x => x.GetUsersAsync(role), Times.Once);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnEmpty_WhenNoUsersFound()
        {
            // Arrange
            _adminRepo.Setup(x => x.GetUsersAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<AdminUserDto>());

            // Act
            var result = await _service.GetUsersAsync("Doctor");

            // Assert
            Assert.Empty(result);
            _adminRepo.Verify(x => x.GetUsersAsync("Doctor"), Times.Once);
        }

        [Theory]
        [InlineData("Doctor")]
        [InlineData("Patient")]
        [InlineData("Admin")]
        public async Task GetUsersAsync_ShouldCallRepository_WithGivenRole(string role)
        {
            // Arrange
            _adminRepo.Setup(x => x.GetUsersAsync(role))
                .ReturnsAsync(new List<AdminUserDto>());

            // Act
            await _service.GetUsersAsync(role);

            // Assert
            _adminRepo.Verify(x => x.GetUsersAsync(role), Times.Once);
        }

        #endregion

        #region GetAppointmentReportsAsync Tests

        [Fact]
        public async Task GetAppointmentReportsAsync_ShouldReturnReports()
        {
            // Arrange
            var reports = new List<AppointmentReportDto>
            {
                new AppointmentReportDto
                {
                    Date = DateOnly.FromDateTime(System.DateTime.Today),
                    Total = 5,
                    Confirmed = 2,
                    Pending = 1,
                    Completed = 1,
                    Cancelled = 1
                }
            };

            _adminRepo.Setup(x => x.GetAppointmentReportsAsync())
                .ReturnsAsync(reports);

            // Act
            var result = await _service.GetAppointmentReportsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _adminRepo.Verify(x => x.GetAppointmentReportsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAppointmentReportsAsync_ShouldReturnMultipleReports()
        {
            // Arrange
            var today = DateOnly.FromDateTime(System.DateTime.Today);
            var reports = new List<AppointmentReportDto>
            {
                new AppointmentReportDto
                {
                    Date = today,
                    Total = 5,
                    Confirmed = 2,
                    Pending = 1,
                    Completed = 1,
                    Cancelled = 1
                },
                new AppointmentReportDto
                {
                    Date = today.AddDays(-1),
                    Total = 3,
                    Confirmed = 1,
                    Pending = 1,
                    Completed = 1,
                    Cancelled = 0
                }
            };

            _adminRepo.Setup(x => x.GetAppointmentReportsAsync())
                .ReturnsAsync(reports);

            // Act
            var result = await _service.GetAppointmentReportsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<AppointmentReportDto>)result).Count);
            _adminRepo.Verify(x => x.GetAppointmentReportsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAppointmentReportsAsync_ShouldReturnEmpty_WhenNoData()
        {
            // Arrange
            _adminRepo.Setup(x => x.GetAppointmentReportsAsync())
                .ReturnsAsync(new List<AppointmentReportDto>());

            // Act
            var result = await _service.GetAppointmentReportsAsync();

            // Assert
            Assert.Empty(result);
            _adminRepo.Verify(x => x.GetAppointmentReportsAsync(), Times.Once);
        }

        #endregion

        #region GetDoctorLeavesAsync Tests

        [Fact]
        public async Task GetDoctorLeavesAsync_ShouldReturnLeaves_WithValidParameters()
        {
            // Arrange
            var leaves = new List<AdminDoctorLeaveDto>
            {
                new AdminDoctorLeaveDto
                {
                    DoctorLeaveId = 1,
                    DoctorName = "Dr. John",
                    StartDate = DateOnly.FromDateTime(System.DateTime.Today.AddDays(1)),
                    EndDate = DateOnly.FromDateTime(System.DateTime.Today.AddDays(5))
                }
            };

            _adminRepo.Setup(x => x.GetDoctorLeavesAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateOnly?>(),
                It.IsAny<DateOnly?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(leaves);

            // Act
            var result = await _service.GetDoctorLeavesAsync(
                null,
                null,
                null,
                null,
                1,
                10);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _adminRepo.Verify(x => x.GetDoctorLeavesAsync(
                null,
                null,
                null,
                null,
                1,
                10,
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetDoctorLeavesAsync_ShouldThrowException_WhenPageNumberIsZero()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.GetDoctorLeavesAsync(null, null, null, null, 0, 10));
            Assert.Contains("Page number must be greater than zero", exception.Message);
        }

        [Fact]
        public async Task GetDoctorLeavesAsync_ShouldThrowException_WhenPageNumberIsNegative()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.GetDoctorLeavesAsync(null, null, null, null, -1, 10));
            Assert.Contains("Page number must be greater than zero", exception.Message);
        }

        [Fact]
        public async Task GetDoctorLeavesAsync_ShouldThrowException_WhenPageSizeIsZero()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.GetDoctorLeavesAsync(null, null, null, null, 1, 0));
            Assert.Contains("Page size must be between 1 and 100", exception.Message);
        }

        [Fact]
        public async Task GetDoctorLeavesAsync_ShouldThrowException_WhenPageSizeExceedsMax()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.GetDoctorLeavesAsync(null, null, null, null, 1, 101));
            Assert.Contains("Page size must be between 1 and 100", exception.Message);
        }

        [Fact]
        public async Task GetDoctorLeavesAsync_ShouldThrowException_WhenToDateBeforeFromDate()
        {
            // Arrange
            var fromDate = DateOnly.FromDateTime(System.DateTime.Today.AddDays(10));
            var toDate = DateOnly.FromDateTime(System.DateTime.Today.AddDays(5));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.GetDoctorLeavesAsync(null, null, fromDate, toDate, 1, 10));
            Assert.Contains("To date cannot be before from date", exception.Message);
        }

        [Theory]
        [InlineData("Current")]
        [InlineData("Upcoming")]
        [InlineData("Past")]
        public async Task GetDoctorLeavesAsync_ShouldAcceptValidStatuses(string status)
        {
            // Arrange
            _adminRepo.Setup(x => x.GetDoctorLeavesAsync(
                It.IsAny<string>(),
                status,
                It.IsAny<DateOnly?>(),
                It.IsAny<DateOnly?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AdminDoctorLeaveDto>());

            // Act
            var result = await _service.GetDoctorLeavesAsync(null, status, null, null, 1, 10);

            // Assert
            Assert.NotNull(result);
            _adminRepo.Verify(x => x.GetDoctorLeavesAsync(
                null,
                status,
                null,
                null,
                1,
                10,
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetDoctorLeavesAsync_ShouldThrowException_WhenInvalidStatusProvided()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.GetDoctorLeavesAsync(null, "InvalidStatus", null, null, 1, 10));
            Assert.Contains("Leave status must be Current, Upcoming, or Past", exception.Message);
        }

        [Fact]
        public async Task GetDoctorLeavesAsync_ShouldTrimSearchString()
        {
            // Arrange
            var searchTerm = "  John  ";
            _adminRepo.Setup(x => x.GetDoctorLeavesAsync(
                "John",
                null,
                It.IsAny<DateOnly?>(),
                It.IsAny<DateOnly?>(),
                1,
                10,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AdminDoctorLeaveDto>());

            // Act
            await _service.GetDoctorLeavesAsync(searchTerm, null, null, null, 1, 10);

            // Assert
            _adminRepo.Verify(x => x.GetDoctorLeavesAsync(
                "John",
                null,
                null,
                null,
                1,
                10,
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetDoctorLeavesAsync_ShouldHandleNullStatus_AsEmptyString()
        {
            // Arrange
            _adminRepo.Setup(x => x.GetDoctorLeavesAsync(
                null,
                null,
                It.IsAny<DateOnly?>(),
                It.IsAny<DateOnly?>(),
                1,
                10,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AdminDoctorLeaveDto>());

            // Act
            await _service.GetDoctorLeavesAsync(null, null, null, null, 1, 10);

            // Assert
            _adminRepo.Verify(x => x.GetDoctorLeavesAsync(
                null,
                null,
                null,
                null,
                1,
                10,
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetDoctorLeavesAsync_ShouldHandleCaseInsensitiveStatus()
        {
            // Arrange
            _adminRepo.Setup(x => x.GetDoctorLeavesAsync(
                null,
                "Current",
                It.IsAny<DateOnly?>(),
                It.IsAny<DateOnly?>(),
                1,
                10,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AdminDoctorLeaveDto>());

            // Act
            await _service.GetDoctorLeavesAsync(null, "current", null, null, 1, 10);

            // Assert
            _adminRepo.Verify(x => x.GetDoctorLeavesAsync(
                null,
                "Current",
                null,
                null,
                1,
                10,
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetDoctorLeavesAsync_ShouldAcceptMaxPageSize()
        {
            // Arrange
            _adminRepo.Setup(x => x.GetDoctorLeavesAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateOnly?>(),
                It.IsAny<DateOnly?>(),
                1,
                100,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AdminDoctorLeaveDto>());

            // Act
            var result = await _service.GetDoctorLeavesAsync(null, null, null, null, 1, 100);

            // Assert
            Assert.NotNull(result);
            _adminRepo.Verify(x => x.GetDoctorLeavesAsync(
                null,
                null,
                null,
                null,
                1,
                100,
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetDoctorLeavesAsync_ShouldAcceptDateRange()
        {
            // Arrange
            var fromDate = DateOnly.FromDateTime(System.DateTime.Today);
            var toDate = DateOnly.FromDateTime(System.DateTime.Today.AddDays(10));

            _adminRepo.Setup(x => x.GetDoctorLeavesAsync(
                null,
                null,
                fromDate,
                toDate,
                1,
                10,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AdminDoctorLeaveDto>());

            // Act
            var result = await _service.GetDoctorLeavesAsync(null, null, fromDate, toDate, 1, 10);

            // Assert
            Assert.NotNull(result);
            _adminRepo.Verify(x => x.GetDoctorLeavesAsync(
                null,
                null,
                fromDate,
                toDate,
                1,
                10,
                It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
    }
}