using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Repositories;
using HealthCare_Appointment_Portal.Tests.Helpers;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _contextMock;
        private readonly Mock<DbSet<User>> _userDbSetMock;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            _contextMock =
                new Mock<ApplicationDbContext>();

            _userDbSetMock =
                new Mock<DbSet<User>>();

            _contextMock
                .Setup(c => c.Users)
                .Returns(_userDbSetMock.Object);

            _repository =
                new UserRepository(
                    _contextMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser()
        {
            var user =
                new User
                {
                    UserId = 1,
                    Email = "test@test.com"
                };

            _userDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync(user);

            var result =
                await _repository.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull()
        {
            _userDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync((User)null!);

            var result =
                await _repository.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            var data =
                new List<User>
                {
                    new User
                    {
                        UserId = 1
                    },
                    new User
                    {
                        UserId = 2
                    }
                };

            var mockSet =
                DbSetMockHelper.CreateMockDbSet(
                    data);

            _contextMock
                .Setup(c => c.Users)
                .Returns(mockSet.Object);

            var repository =
                new UserRepository(
                    _contextMock.Object);

            var result =
                await repository.GetAllAsync();

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsUser()
        {
            var user =
                new User
                {
                    UserId = 1
                };

            await _repository.AddAsync(
                user);

            _userDbSetMock.Verify(
                d => d.Add(user),
                Times.Once);
        }


        [Fact]
        public async Task DeleteAsync_RemovesUser()
        {
            var user =
                new User
                {
                    UserId = 1
                };

            _userDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync(user);

            await _repository.DeleteAsync(1);

            _userDbSetMock.Verify(
                d => d.Remove(user),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenUserNotFound_DoesNothing()
        {
            _userDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync((User)null!);

            await _repository.DeleteAsync(1);

            _userDbSetMock.Verify(
                d => d.Remove(
                    It.IsAny<User>()),
                Times.Never);
        }

        [Fact]
        public void GetByEmail_ReturnsUser()
        {
            var data =
                new List<User>
                {
                    new User
                    {
                        UserId = 1,
                        Email = "john@test.com"
                    },
                    new User
                    {
                        UserId = 2,
                        Email = "jane@test.com"
                    }
                };

            var mockSet =
                DbSetMockHelper.CreateMockDbSet(
                    data);

            _contextMock
                .Setup(c => c.Users)
                .Returns(mockSet.Object);

            var repository =
                new UserRepository(
                    _contextMock.Object);

            var result =
                repository.GetByEmail(
                    "john@test.com");

            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public void GetByEmail_ReturnsNull()
        {
            var data =
                new List<User>
                {
                    new User
                    {
                        UserId = 1,
                        Email = "john@test.com"
                    }
                };

            var mockSet =
                DbSetMockHelper.CreateMockDbSet(
                    data);

            _contextMock
                .Setup(c => c.Users)
                .Returns(mockSet.Object);

            var repository =
                new UserRepository(
                    _contextMock.Object);

            var result =
                repository.GetByEmail(
                    "unknown@test.com");

            Assert.Null(result);
        }

        [Fact]
        public void GetByUserCode_ReturnsUser()
        {
            var data =
                new List<User>
                {
                    new User
                    {
                        UserId = 1,
                        UserCode = "USR001"
                    },
                    new User
                    {
                        UserId = 2,
                        UserCode = "USR002"
                    }
                };

            var mockSet =
                DbSetMockHelper.CreateMockDbSet(
                    data);

            _contextMock
                .Setup(c => c.Users)
                .Returns(mockSet.Object);

            var repository =
                new UserRepository(
                    _contextMock.Object);

            var result =
                repository.GetByUserCode(
                    "USR001");

            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public void GetByUserCode_ReturnsNull()
        {
            var data =
                new List<User>
                {
                    new User
                    {
                        UserId = 1,
                        UserCode = "USR001"
                    }
                };

            var mockSet =
                DbSetMockHelper.CreateMockDbSet(
                    data);

            _contextMock
                .Setup(c => c.Users)
                .Returns(mockSet.Object);

            var repository =
                new UserRepository(
                    _contextMock.Object);

            var result =
                repository.GetByUserCode(
                    "INVALID");

            Assert.Null(result);
        }
    }
}