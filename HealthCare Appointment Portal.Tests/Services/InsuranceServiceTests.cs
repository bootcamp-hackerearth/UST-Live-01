using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.InsuranceDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

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
                new Insurance(),
                new Insurance()
            };

            var insuranceDtos = new List<InsuranceDto>
            {
                new InsuranceDto(),
                new InsuranceDto()
            };

            _unitOfWorkMock.Setup(x =>
                    x.Insurances.GetAllAsync())
                .ReturnsAsync(insurances);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<InsuranceDto>>(insurances))
                .Returns(insuranceDtos);

            var result =
                await _service.GetAllInsurancesAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetInsuranceByIdAsync_ReturnsInsurance()
        {
            var insurance = new Insurance();

            var dto = new InsuranceDto();

            _unitOfWorkMock.Setup(x =>
                    x.Insurances.GetByIdAsync(1))
                .ReturnsAsync(insurance);

            _mapperMock.Setup(x =>
                    x.Map<InsuranceDto>(insurance))
                .Returns(dto);

            var result =
                await _service.GetInsuranceByIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetInsuranceByIdAsync_WhenNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Insurances.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Insurance)null!);

            await Assert.ThrowsAsync<InsuranceNotFoundException>(
                () => _service.GetInsuranceByIdAsync(1));
        }

        [Fact]
        public async Task GetInsurancesByPatientAsync_ReturnsInsurances()
        {
            var insurances = new List<Insurance>
            {
                new Insurance()
            };

            var insuranceDtos = new List<InsuranceDto>
            {
                new InsuranceDto()
            };

            _unitOfWorkMock.Setup(x =>
                    x.Insurances.GetInsurancesByPatientAsync(1))
                .ReturnsAsync(insurances);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<InsuranceDto>>(insurances))
                .Returns(insuranceDtos);

            var result =
                await _service.GetInsurancesByPatientAsync(1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetInsurancesByStatusAsync_ReturnsInsurances()
        {
            var insurances = new List<Insurance>
            {
                new Insurance()
            };

            var insuranceDtos = new List<InsuranceDto>
            {
                new InsuranceDto()
            };

            _unitOfWorkMock.Setup(x =>
                    x.Insurances.GetInsurancesByStatusAsync(
                        InsuranceStatus.Active))
                .ReturnsAsync(insurances);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<InsuranceDto>>(insurances))
                .Returns(insuranceDtos);

            var result =
                await _service.GetInsurancesByStatusAsync(
                    InsuranceStatus.Active);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetExpiredInsurancesAsync_ReturnsInsurances()
        {
            var insurances = new List<Insurance>
            {
                new Insurance()
            };

            var insuranceDtos = new List<InsuranceDto>
            {
                new InsuranceDto()
            };

            _unitOfWorkMock.Setup(x =>
                    x.Insurances.GetExpiredInsurancesAsync())
                .ReturnsAsync(insurances);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<InsuranceDto>>(insurances))
                .Returns(insuranceDtos);

            var result =
                await _service.GetExpiredInsurancesAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetActiveInsurancesAsync_ReturnsInsurances()
        {
            var insurances = new List<Insurance>
            {
                new Insurance()
            };

            var insuranceDtos = new List<InsuranceDto>
            {
                new InsuranceDto()
            };

            _unitOfWorkMock.Setup(x =>
                    x.Insurances.GetActiveInsurancesAsync())
                .ReturnsAsync(insurances);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<InsuranceDto>>(insurances))
                .Returns(insuranceDtos);

            var result =
                await _service.GetActiveInsurancesAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task AddInsuranceAsync_WhenPolicyExists_ThrowsException()
        {
            var dto = new CreateInsuranceDto
            {
                PolicyNumber = "POL001"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Insurances
                        .GetInsuranceByPolicyNumberAsync(
                            dto.PolicyNumber))
                .ReturnsAsync(new Insurance());

            await Assert.ThrowsAsync<
                DuplicatePolicyNumberException>(
                () => _service.AddInsuranceAsync(dto));
        }

        [Fact]
        public async Task AddInsuranceAsync_ReturnsInsuranceId()
        {
            var dto = new CreateInsuranceDto
            {
                PolicyNumber = "POL001"
            };

            var insurance = new Insurance
            {
                InsuranceId = 1
            };

            _unitOfWorkMock.Setup(x =>
                    x.Insurances
                        .GetInsuranceByPolicyNumberAsync(
                            dto.PolicyNumber))
                .ReturnsAsync((Insurance)null!);

            _mapperMock.Setup(x =>
                    x.Map<Insurance>(dto))
                .Returns(insurance);

            var result =
                await _service.AddInsuranceAsync(dto);

            Assert.Equal(1, result);

            _unitOfWorkMock.Verify(
                x => x.Insurances.AddAsync(insurance),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateInsuranceAsync_WhenInsuranceNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Insurances.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Insurance)null!);

            await Assert.ThrowsAsync<
                InsuranceNotFoundException>(
                () => _service.UpdateInsuranceAsync(
                    1,
                    new UpdateInsuranceDto()));
        }

        [Fact]
        public async Task UpdateInsuranceAsync_WhenInsuranceExpired_ThrowsException()
        {
            var insurance = new Insurance
            {
                ExpiryDate = DateTime.Today.AddDays(-1)
            };

            _unitOfWorkMock.Setup(x =>
                    x.Insurances.GetByIdAsync(1))
                .ReturnsAsync(insurance);

            await Assert.ThrowsAsync<
                InsuranceExpiredException>(
                () => _service.UpdateInsuranceAsync(
                    1,
                    new UpdateInsuranceDto()));
        }

        [Fact]
        public async Task UpdateInsuranceAsync_UpdatesSuccessfully()
        {
            var insurance = new Insurance
            {
                ExpiryDate = DateTime.Today.AddDays(10)
            };

            var dto = new UpdateInsuranceDto();

            _unitOfWorkMock.Setup(x =>
                    x.Insurances.GetByIdAsync(1))
                .ReturnsAsync(insurance);

            await _service.UpdateInsuranceAsync(
                1,
                dto);

            _unitOfWorkMock.Verify(
                x => x.Insurances.UpdateAsync(insurance),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task DeleteInsuranceAsync_WhenInsuranceNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Insurances.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Insurance)null!);

            await Assert.ThrowsAsync<
                InsuranceNotFoundException>(
                () => _service.DeleteInsuranceAsync(1));
        }

        [Fact]
        public async Task DeleteInsuranceAsync_DeletesSuccessfully()
        {
            var insurance = new Insurance();

            _unitOfWorkMock.Setup(x =>
                    x.Insurances.GetByIdAsync(1))
                .ReturnsAsync(insurance);

            await _service.DeleteInsuranceAsync(1);

            _unitOfWorkMock.Verify(
                x => x.Insurances.DeleteAsync(1),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }
    }
}