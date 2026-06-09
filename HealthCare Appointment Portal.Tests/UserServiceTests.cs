using Moq;
using Xunit;

using HealthCare_Appointment_Portal.Services;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;

using System.Threading.Tasks;

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
        public async Task GetByIdAsync_ValidId_ReturnsUser()
        {
            var user = new User
            {
                UserId = 1,
                Email = "aniket@gmail.com"
            };

            _unitOfWorkMock
                .Setup(x => x.Users.GetByIdAsync(1))
                .ReturnsAsync(user);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            _unitOfWorkMock
                .Setup(x => x.Users.GetByIdAsync(1))
                .ReturnsAsync((User)null);

            var result = await _service.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByUserCodeAsync_ValidCode_ReturnsUser()
        {
            var user = new User
            {
                UserId = 1,
                UserCode = "U123"
            };

            _unitOfWorkMock
                .Setup(x => x.Users.GetByUserCode("U123"))
                .Returns(user);

            var result = await _service.GetByUserCodeAsync("U123");

            Assert.NotNull(result);
            Assert.Equal("U123", result.UserCode);
        }

        [Fact]
        public async Task GetByUserCodeAsync_InvalidCode_ReturnsNull()
        {
            _unitOfWorkMock
                .Setup(x => x.Users.GetByUserCode("U123"))
                .Returns((User)null);

            var result = await _service.GetByUserCodeAsync("U123");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByEmailAsync_ValidEmail_ReturnsUser()
        {
            var user = new User
            {
                UserId = 1,
                Email = "aniket@gmail.com"
            };

            _unitOfWorkMock
                .Setup(x => x.Users.GetByEmail("aniket@gmail.com"))
                .Returns(user);

            var result = await _service.GetByEmailAsync("aniket@gmail.com");

            Assert.NotNull(result);
            Assert.Equal("aniket@gmail.com", result.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_InvalidEmail_ReturnsNull()
        {
            _unitOfWorkMock
                .Setup(x => x.Users.GetByEmail("aniket@gmail.com"))
                .Returns((User)null);

            var result = await _service.GetByEmailAsync("aniket@gmail.com");

            Assert.Null(result);
        }
    }
}
