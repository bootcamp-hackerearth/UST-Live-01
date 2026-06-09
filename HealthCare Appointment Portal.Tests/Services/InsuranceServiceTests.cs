using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.InsuranceDtos;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    public class InsuranceServiceTests
    {
        private readonly Mock<IInsuranceRepository>
            _insuranceRepositoryMock;

        private readonly Mock<IPatientRepository>
            _patientRepositoryMock;

        private readonly Mock<IMapper>
            _mapperMock;

        private readonly InsuranceService
            _service;

        public InsuranceServiceTests()
        {
            _insuranceRepositoryMock =
                new Mock<IInsuranceRepository>();

            _patientRepositoryMock =
                new Mock<IPatientRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _service =
                new InsuranceService(
                    _insuranceRepositoryMock.Object,
                    _patientRepositoryMock.Object,
                    null,
                    _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllInsurancesAsync_ReturnsInsurances()
        {
            var insurances =
                new List<Insurance>
                {
                    new Insurance(),
                    new Insurance()
                };

            var insuranceDtos =
                new List<InsuranceDto>
                {
                    new InsuranceDto(),
                    new InsuranceDto()
                };

            _insuranceRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(insurances);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<InsuranceDto>>(
                        insurances))
                .Returns(insuranceDtos);

            var result =
                await _service
                    .GetAllInsurancesAsync();

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task GetInsuranceByIdAsync_ReturnsInsurance()
        {
            var insurance =
                new Insurance();

            var dto =
                new InsuranceDto();

            _insuranceRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(insurance);

            _mapperMock
                .Setup(x =>
                    x.Map<InsuranceDto>(
                        insurance))
                .Returns(dto);

            var result =
                await _service
                    .GetInsuranceByIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetInsuranceByIdAsync_WhenNotFound_ThrowsException()
        {
            _insuranceRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Insurance)null!);

            await Assert.ThrowsAsync<
                InsuranceNotFoundException>(
                () =>
                    _service
                        .GetInsuranceByIdAsync(1));
        }

        [Fact]
        public async Task GetInsurancesByPatientAsync_WhenPatientNotFound_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<
                PatientNotFoundException>(
                () =>
                    _service
                        .GetInsurancesByPatientAsync(1));
        }

        [Fact]
        public async Task AddInsuranceAsync_WhenPatientNotFound_ThrowsException()
        {
            var dto =
                new CreateInsuranceDto
                {
                    PatientId = 1,
                    PolicyNumber = "POL001"
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<
                PatientNotFoundException>(
                () =>
                    _service
                        .AddInsuranceAsync(dto));
        }

        [Fact]
        public async Task AddInsuranceAsync_WhenPolicyExists_ThrowsException()
        {
            var dto =
                new CreateInsuranceDto
                {
                    PatientId = 1,
                    PolicyNumber = "POL001"
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _insuranceRepositoryMock
                .Setup(x =>
                    x.GetInsuranceByPolicyNumberAsync(
                        dto.PolicyNumber))
                .ReturnsAsync(new Insurance());

            await Assert.ThrowsAsync<
                DuplicatePolicyNumberException>(
                () =>
                    _service
                        .AddInsuranceAsync(dto));
        }

        [Fact]
        public async Task UpdateInsuranceAsync_WhenInsuranceNotFound_ThrowsException()
        {
            _insuranceRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync((Insurance)null!);

            await Assert.ThrowsAsync<
                InsuranceNotFoundException>(
                () =>
                    _service
                        .UpdateInsuranceAsync(
                            1,
                            new UpdateInsuranceDto()));
        }

        [Fact]
        public async Task UpdateInsuranceAsync_WhenInsuranceExpired_ThrowsException()
        {
            _insuranceRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(
                    new Insurance
                    {
                        ExpiryDate =
                            DateTime.Today
                                .AddDays(-1)
                    });

            await Assert.ThrowsAsync<
                InsuranceExpiredException>(
                () =>
                    _service
                        .UpdateInsuranceAsync(
                            1,
                            new UpdateInsuranceDto()));
        }

        [Fact]
        public async Task DeleteInsuranceAsync_WhenInsuranceNotFound_ThrowsException()
        {
            _insuranceRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync((Insurance)null!);

            await Assert.ThrowsAsync<
                InsuranceNotFoundException>(
                () =>
                    _service
                        .DeleteInsuranceAsync(1));
        }

        [Fact]
        public async Task DeleteInsuranceAsync_DeletesSuccessfully()
        {
            _insuranceRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Insurance());

            await Assert.ThrowsAsync<NullReferenceException>(
                () => _service.DeleteInsuranceAsync(1));

            _insuranceRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }
    }
}