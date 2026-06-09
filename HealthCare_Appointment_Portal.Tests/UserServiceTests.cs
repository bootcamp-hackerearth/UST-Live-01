using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;

        private Mock<IUserRepository> _userRepositoryMock;

        private UserService _service;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock =
                new Mock<IUnitOfWork>();

            _userRepositoryMock =
                new Mock<IUserRepository>();

            _unitOfWorkMock
                .Setup(x => x.Users)
                .Returns(_userRepositoryMock.Object);

            _service =
                new UserService(
                    _unitOfWorkMock.Object);
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingId_ShouldReturnUser()
        {
            // Arrange

            User user = new User
            {
                UserId = 1,
                Email = "anand@gmail.com"
            };

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(user);

            // Act

            var result =
                await _service.GetByIdAsync(1);

            // Assert

            Assert.IsNotNull(result);

            Assert.AreEqual(
                1,
                result.UserId);
        }

        [TestMethod]
        public async Task GetByUserCodeAsync_ValidCode_ShouldReturnUser()
        {
            // Arrange

            User user = new User
            {
                UserId = 1,
                UserCode = "P001"
            };

            _userRepositoryMock
                .Setup(x => x.GetByUserCode("P001"))
                .Returns(user);

            // Act

            var result =
                await _service.GetByUserCodeAsync(
                    "P001");

            // Assert

            Assert.IsNotNull(result);

            Assert.AreEqual(
                "P001",
                result.UserCode);
        }

        [TestMethod]
        public async Task GetByEmailAsync_ValidEmail_ShouldReturnUser()
        {
            // Arrange

            User user = new User
            {
                UserId = 1,
                Email = "anand@gmail.com"
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmail(
                    "anand@gmail.com"))
                .Returns(user);

            // Act

            var result =
                await _service.GetByEmailAsync(
                    "anand@gmail.com");

            // Assert

            Assert.IsNotNull(result);

            Assert.AreEqual(
                "anand@gmail.com",
                result.Email);
        }
    }
}