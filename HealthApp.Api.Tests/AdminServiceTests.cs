using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthApp.Api.Services.Impl;
using HealthApp.Api.Repositories.Interfaces;
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


        [Fact]
        public async Task GetUsersAsync_ShouldReturnAllUsers_WhenRoleIsNull()
        {
            var users = new List<AdminUserDto>
        {
            new AdminUserDto { Id = "1", Email = "user1@test.com" },
            new AdminUserDto { Id = "2", Email = "user2@test.com" }
        };

            _adminRepo.Setup(x => x.GetUsersAsync(null))
                .ReturnsAsync(users);

            var result = await _service.GetUsersAsync(null);

            Assert.NotNull(result);
            Assert.Equal(2, ((List<AdminUserDto>)result).Count);

            _adminRepo.Verify(x => x.GetUsersAsync(null), Times.Once);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnFilteredUsers_WhenRoleProvided()
        {
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

            var result = await _service.GetUsersAsync(role);

            Assert.Single(result);
            _adminRepo.Verify(x => x.GetUsersAsync(role), Times.Once);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnEmpty_WhenNoUsersFound()
        {
            _adminRepo.Setup(x => x.GetUsersAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<AdminUserDto>());

            var result = await _service.GetUsersAsync("Doctor");

            Assert.Empty(result);
            _adminRepo.Verify(x => x.GetUsersAsync("Doctor"), Times.Once);
        }


        [Fact]
        public async Task GetAppointmentReportsAsync_ShouldReturnReports()
        {
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

            var result = await _service.GetAppointmentReportsAsync();

            Assert.NotNull(result);
            Assert.Single(result);

            _adminRepo.Verify(x => x.GetAppointmentReportsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAppointmentReportsAsync_ShouldReturnEmpty_WhenNoData()
        {
            _adminRepo.Setup(x => x.GetAppointmentReportsAsync())
                .ReturnsAsync(new List<AppointmentReportDto>());

            var result = await _service.GetAppointmentReportsAsync();

            Assert.Empty(result);
            _adminRepo.Verify(x => x.GetAppointmentReportsAsync(), Times.Once);
        }
    }
}