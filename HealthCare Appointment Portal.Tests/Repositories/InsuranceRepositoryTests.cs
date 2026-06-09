using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Repositories;
using HealthCare_Appointment_Portal.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Repositories
{
    public class InsuranceRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _contextMock;
        private readonly Mock<DbSet<Insurance>> _insuranceDbSetMock;
        private readonly InsuranceRepository _repository;

        public InsuranceRepositoryTests()
        {
            _contextMock =
                new Mock<ApplicationDbContext>();

            _insuranceDbSetMock =
                new Mock<DbSet<Insurance>>();

            _contextMock
                .Setup(c => c.Insurances)
                .Returns(_insuranceDbSetMock.Object);

            _repository =
                new InsuranceRepository(
                    _contextMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsInsurance()
        {
            var insurance =
                new Insurance
                {
                    InsuranceId = 1
                };

            _insuranceDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync(insurance);

            var result =
                await _repository.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.InsuranceId);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull()
        {
            _insuranceDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync((Insurance)null!);

            var result =
                await _repository.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllInsurances()
        {
            var data =
                new List<Insurance>
                {
                    new Insurance
                    {
                        InsuranceId = 1
                    },
                    new Insurance
                    {
                        InsuranceId = 2
                    }
                };

            var mockSet =
                DbSetMockHelper.CreateMockDbSet(
                    data);

            _contextMock
                .Setup(c => c.Insurances)
                .Returns(mockSet.Object);

            var repository =
                new InsuranceRepository(
                    _contextMock.Object);

            var result =
                await repository.GetAllAsync();

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsInsurance()
        {
            var insurance =
                new Insurance();

            await _repository.AddAsync(
                insurance);

            _insuranceDbSetMock.Verify(
                d => d.Add(insurance),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_RemovesInsurance()
        {
            var insurance =
                new Insurance
                {
                    InsuranceId = 1
                };

            _insuranceDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync(insurance);

            await _repository.DeleteAsync(1);

            _insuranceDbSetMock.Verify(
                d => d.Remove(insurance),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenNotFound_DoesNothing()
        {
            _insuranceDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync((Insurance)null!);

            await _repository.DeleteAsync(1);

            _insuranceDbSetMock.Verify(
                d => d.Remove(
                    It.IsAny<Insurance>()),
                Times.Never);
        }
    }
}