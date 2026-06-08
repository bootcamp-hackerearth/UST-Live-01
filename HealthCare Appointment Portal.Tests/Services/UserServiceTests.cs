using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _service = new UserService(
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser()
        {
            var user = new User
            {
                UserId = 1,
                UserCode = "A001",
                Email = "admin@gmail.com"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Users.GetByIdAsync(1))
                .ReturnsAsync(user);

            var result =
                await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public async Task GetByIdAsync_WhenUserNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Users.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null!);

            await Assert.ThrowsAsync<UserNotFoundException>(
                () => _service.GetByIdAsync(1));
        }

        [Fact]
        public async Task GetByUserCodeAsync_ReturnsUser()
        {
            var user = new User
            {
                UserId = 1,
                UserCode = "A001"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Users.GetByUserCode("A001"))
                .Returns(user);

            var result =
                await _service.GetByUserCodeAsync(
                    "A001");

            Assert.NotNull(result);
            Assert.Equal("A001", result.UserCode);
        }

        [Fact]
        public async Task GetByUserCodeAsync_WhenUserNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Users.GetByUserCode(
                        It.IsAny<string>()))
                .Returns((User)null!);

            await Assert.ThrowsAsync<UserNotFoundException>(
                () => _service.GetByUserCodeAsync(
                    "A001"));
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsUser()
        {
            var user = new User
            {
                UserId = 1,
                Email = "admin@gmail.com"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Users.GetByEmail(
                        "admin@gmail.com"))
                .Returns(user);

            var result =
                await _service.GetByEmailAsync(
                    "admin@gmail.com");

            Assert.NotNull(result);
            Assert.Equal(
                "admin@gmail.com",
                result.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_WhenUserNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Users.GetByEmail(
                        It.IsAny<string>()))
                .Returns((User)null!);

            await Assert.ThrowsAsync<UserNotFoundException>(
                () => _service.GetByEmailAsync(
                    "admin@gmail.com"));
        }
    }
}