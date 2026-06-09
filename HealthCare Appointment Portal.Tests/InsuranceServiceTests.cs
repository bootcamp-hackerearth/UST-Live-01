using AutoMapper;
using Moq;
using Xunit;

using HealthCare_Appointment_Portal.Services;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.DTOs.InsuranceDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    public class InsuranceServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly InsuranceService _service;

        public InsuranceServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _service = new InsuranceService(
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllInsurancesAsync_ReturnsInsurances()
        {
            var insurances = new List<Insurance>
            {
                new Insurance { InsuranceId = 1 }
            };

            var dtos = new List<InsuranceDto>
            {
                new InsuranceDto { InsuranceId = 1 }
            };

            _unitOfWorkMock
                .Setup(x => x.Insurances.GetAllAsync())
                .ReturnsAsync(insurances);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<InsuranceDto>>(insurances))
                .Returns(dtos);

            var result = await _service.GetAllInsurancesAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetInsuranceByIdAsync_ValidId_ReturnsInsurance()
        {
            var insurance = new Insurance { InsuranceId = 1 };

            var dto = new InsuranceDto { InsuranceId = 1 };

            _unitOfWorkMock
                .Setup(x => x.Insurances.GetByIdAsync(1))
                .ReturnsAsync(insurance);

            _mapperMock
                .Setup(x => x.Map<InsuranceDto>(insurance))
                .Returns(dto);

            var result = await _service.GetInsuranceByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.InsuranceId);
        }

        [Fact]
        public async Task GetInsuranceByIdAsync_InvalidId_ThrowsException()
        {
            _unitOfWorkMock
                .Setup(x => x.Insurances.GetByIdAsync(1))
                .ReturnsAsync((Insurance)null);

            await Assert.ThrowsAsync<InsuranceNotFoundException>(
                () => _service.GetInsuranceByIdAsync(1));
        }

        [Fact]
        public async Task GetInsurancesByPatientAsync_ReturnsInsurances()
        {
            var insurances = new List<Insurance>
            {
                new Insurance { InsuranceId = 1 }
            };

            _unitOfWorkMock
                .Setup(x => x.Insurances.GetInsurancesByPatientAsync(1))
                .ReturnsAsync(insurances);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<InsuranceDto>>(insurances))
                .Returns(new List<InsuranceDto>());

            var result = await _service.GetInsurancesByPatientAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetInsurancesByStatusAsync_ReturnsInsurances()
        {
            var insurances = new List<Insurance>
            {
                new Insurance { InsuranceId = 1 }
            };

            _unitOfWorkMock
                .Setup(x => x.Insurances
                    .GetInsurancesByStatusAsync(InsuranceStatus.Active))
                .ReturnsAsync(insurances);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<InsuranceDto>>(insurances))
                .Returns(new List<InsuranceDto>());

            var result = await _service
                .GetInsurancesByStatusAsync(InsuranceStatus.Active);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetExpiredInsurancesAsync_ReturnsInsurances()
        {
            var insurances = new List<Insurance>();

            _unitOfWorkMock
                .Setup(x => x.Insurances.GetExpiredInsurancesAsync())
                .ReturnsAsync(insurances);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<InsuranceDto>>(insurances))
                .Returns(new List<InsuranceDto>());

            var result = await _service.GetExpiredInsurancesAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetActiveInsurancesAsync_ReturnsInsurances()
        {
            var insurances = new List<Insurance>();

            _unitOfWorkMock
                .Setup(x => x.Insurances.GetActiveInsurancesAsync())
                .ReturnsAsync(insurances);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<InsuranceDto>>(insurances))
                .Returns(new List<InsuranceDto>());

            var result = await _service.GetActiveInsurancesAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddInsuranceAsync_Valid_ReturnsId()
        {
            var dto = new CreateInsuranceDto
            {
                PolicyNumber = "POL123"
            };

            var insurance = new Insurance { InsuranceId = 1 };

            _unitOfWorkMock
                .Setup(x => x.Insurances
                    .GetInsuranceByPolicyNumberAsync(dto.PolicyNumber))
                .ReturnsAsync((Insurance)null);

            _mapperMock
                .Setup(x => x.Map<Insurance>(dto))
                .Returns(insurance);

            var result = await _service.AddInsuranceAsync(dto);

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task AddInsuranceAsync_DuplicatePolicy_ThrowsException()
        {
            var dto = new CreateInsuranceDto
            {
                PolicyNumber = "POL123"
            };

            _unitOfWorkMock
                .Setup(x => x.Insurances
                    .GetInsuranceByPolicyNumberAsync(dto.PolicyNumber))
                .ReturnsAsync(new Insurance());

            await Assert.ThrowsAsync<DuplicatePolicyNumberException>(
                () => _service.AddInsuranceAsync(dto));
        }

        [Fact]
        public async Task UpdateInsuranceAsync_Valid_UpdatesSuccessfully()
        {
            var insurance = new Insurance
            {
                InsuranceId = 1,
                ExpiryDate = DateTime.Today.AddDays(10)
            };

            _unitOfWorkMock
                .Setup(x => x.Insurances.GetByIdAsync(1))
                .ReturnsAsync(insurance);

            await _service.UpdateInsuranceAsync(1, new UpdateInsuranceDto());

            _unitOfWorkMock.Verify(
                x => x.Insurances.UpdateAsync(insurance),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateInsuranceAsync_NotFound_ThrowsException()
        {
            _unitOfWorkMock
                .Setup(x => x.Insurances.GetByIdAsync(1))
                .ReturnsAsync((Insurance)null);

            await Assert.ThrowsAsync<InsuranceNotFoundException>(
                () => _service.UpdateInsuranceAsync(1, new UpdateInsuranceDto()));
        }

        [Fact]
        public async Task UpdateInsuranceAsync_ExpiredPolicy_ThrowsException()
        {
            var insurance = new Insurance
            {
                InsuranceId = 1,
                ExpiryDate = DateTime.Today.AddDays(-1)
            };

            _unitOfWorkMock
                .Setup(x => x.Insurances.GetByIdAsync(1))
                .ReturnsAsync(insurance);

            await Assert.ThrowsAsync<InsuranceExpiredException>(
                () => _service.UpdateInsuranceAsync(1, new UpdateInsuranceDto()));
        }

        [Fact]
        public async Task DeleteInsuranceAsync_Valid_DeletesSuccessfully()
        {
            var insurance = new Insurance { InsuranceId = 1 };

            _unitOfWorkMock
                .Setup(x => x.Insurances.GetByIdAsync(1))
                .ReturnsAsync(insurance);

            await _service.DeleteInsuranceAsync(1);

            _unitOfWorkMock.Verify(
                x => x.Insurances.DeleteAsync(1),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task DeleteInsuranceAsync_NotFound_ThrowsException()
        {
            _unitOfWorkMock
                .Setup(x => x.Insurances.GetByIdAsync(1))
                .ReturnsAsync((Insurance)null);

            await Assert.ThrowsAsync<InsuranceNotFoundException>(
                () => _service.DeleteInsuranceAsync(1));
        }
    }
}
