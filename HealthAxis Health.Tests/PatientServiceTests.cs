using AutoMapper;
using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.DTOs.HealthRecordDtos;
using HealthAxisHealth.API.DTOs.PatientDtos;
using HealthAxisHealth.API.Exceptions;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Repositories.Interfaces;
using HealthAxisHealth.API.Services.Implementations;
using HealthAxisHealth.API.UnitOfWork;
using Moq;
using Xunit;

namespace HealthAxisHealth.API.Tests.Services
{
    public class PatientServiceTests
    {
        #region Fields

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly PatientService _service;

        #endregion

        #region Constructor

        public PatientServiceTests()
        {
            _unitOfWorkMock =
                new Mock<IUnitOfWork>();

            _patientRepositoryMock =
                new Mock<IPatientRepository>();

            _userRepositoryMock =
                new Mock<IUserRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _unitOfWorkMock
                .Setup(x => x.Patients)
                .Returns(_patientRepositoryMock.Object);

            _unitOfWorkMock
                .Setup(x => x.Users)
                .Returns(_userRepositoryMock.Object);

            _service =
                new PatientService(
                    _unitOfWorkMock.Object,
                    _mapperMock.Object);
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenPatientNotFound()
        {
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetByIdAsync(1));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPatient()
        {
            var patient = new Patient
            {
                PatientId = 1
            };

            var dto = new PatientDto
            {
                PatientId = 1
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(x => x.Map<PatientDto>(patient))
                .Returns(dto);

            var result =
                await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result!.PatientId);
        }

        #endregion

        #region GetByUserIdAsync

        [Fact]
        public async Task GetByUserIdAsync_ShouldThrow_WhenPatientNotFound()
        {
            _patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync(10))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetByUserIdAsync(10));
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnPatient()
        {
            var patient = new Patient
            {
                PatientId = 1,
                UserId = 10
            };

            var dto = new PatientDto
            {
                PatientId = 1
            };

            _patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync(10))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(x => x.Map<PatientDto>(patient))
                .Returns(dto);

            var result =
                await _service.GetByUserIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal(1, result!.PatientId);
        }

        #endregion

        #region GetPagedAsync

        [Fact]
        public async Task GetPagedAsync_ShouldReturnPagedPatients()
        {
            var pagination =
                new PaginationParams();

            var patients =
                new List<Patient>
                {
                    new Patient
                    {
                        PatientId = 1
                    }
                };

            var patientDtos =
                new List<PatientDto>
                {
                    new PatientDto
                    {
                        PatientId = 1
                    }
                };

            var paged =
                new PagedResultDto<Patient>
                {
                    Items = patients,
                    PageNumber = 1,
                    PageSize = 10,
                    TotalRecords = 1
                };

            _patientRepositoryMock
                .Setup(x => x.GetPagedAsync(pagination))
                .ReturnsAsync(paged);

            _mapperMock
                .Setup(x => x.Map<List<PatientDto>>(patients))
                .Returns(patientDtos);

            var result =
                await _service.GetPagedAsync(
                    pagination);

            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalRecords);
        }

        #endregion
        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenPatientNotFound()
        {
            // Arrange
            var dto = new UpdatePatientDto
            {
                Email = "patient@test.com"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateAsync(1, dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenEmailAlreadyExists()
        {
            // Arrange
            var patient = new Patient
            {
                PatientId = 1,
                UserId = 10
            };

            var dto = new UpdatePatientDto
            {
                Email = "duplicate@test.com"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync(new User
                {
                    UserId = 99,
                    Email = dto.Email
                });

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _service.UpdateAsync(1, dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenLinkedUserNotFound()
        {
            // Arrange
            var patient = new Patient
            {
                PatientId = 1,
                UserId = 10
            };

            var dto = new UpdatePatientDto
            {
                Email = "new@test.com"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(patient.UserId))
                .ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateAsync(1, dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdatePatientSuccessfully()
        {
            // Arrange
            var patient = new Patient
            {
                PatientId = 1,
                UserId = 10
            };

            var dto = new UpdatePatientDto
            {
                Email = "updated@test.com"
            };

            var user = new User
            {
                UserId = 10,
                Email = "old@test.com"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(patient.UserId))
                .ReturnsAsync(user);

            // Act
            await _service.UpdateAsync(1, dto);

            // Assert
            Assert.Equal(dto.Email, user.Email);

            _patientRepositoryMock.Verify(
                x => x.Update(It.IsAny<Patient>()),
                Times.Once);

            _userRepositoryMock.Verify(
                x => x.Update(It.IsAny<User>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        #endregion

        #region UpdateByUserIdAsync

        [Fact]
        public async Task UpdateByUserIdAsync_ShouldThrow_WhenPatientNotFound()
        {
            var dto = new UpdatePatientDto
            {
                Email = "patient@test.com"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync(10))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateByUserIdAsync(10, dto));
        }

        [Fact]
        public async Task UpdateByUserIdAsync_ShouldUpdateSuccessfully()
        {
            var patient = new Patient
            {
                PatientId = 1,
                UserId = 10
            };

            var dto = new UpdatePatientDto
            {
                Email = "updated@test.com"
            };

            var user = new User
            {
                UserId = 10,
                Email = "old@test.com"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync(10))
                .ReturnsAsync(patient);

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(user);

            await _service.UpdateByUserIdAsync(10, dto);

            _patientRepositoryMock.Verify(
                x => x.Update(It.IsAny<Patient>()),
                Times.Once);

            _userRepositoryMock.Verify(
                x => x.Update(It.IsAny<User>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        #endregion
        #region GetHealthRecordsAsync

        [Fact]
        public async Task GetHealthRecordsAsync_ShouldThrow_WhenPatientNotFound()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetPatientWithHealthRecordsAsync(1))
                .ReturnsAsync((Patient?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetHealthRecordsAsync(1));
        }

        [Fact]
        public async Task GetHealthRecordsAsync_ShouldReturnHealthRecords()
        {
            // Arrange
            var patient = new Patient
            {
                PatientId = 1,
                HealthRecords = new List<HealthRecord>
                {
                    new HealthRecord
                    {
                        RecordId = 100
                    }
                }
            };

            var healthRecordDtos =
                new List<HealthRecordDto>
                {
                    new HealthRecordDto
                    {
                        RecordId = 100
                    }
                };

            _patientRepositoryMock
                .Setup(x => x.GetPatientWithHealthRecordsAsync(1))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(x => x.Map<List<HealthRecordDto>>(patient.HealthRecords))
                .Returns(healthRecordDtos);

            // Act
            var result =
                await _service.GetHealthRecordsAsync(1);

            // Assert
            Assert.Single(result);
        }

        #endregion

        #region GetHealthRecordsByUserIdAsync

        [Fact]
        public async Task GetHealthRecordsByUserIdAsync_ShouldThrow_WhenPatientByUserIdNotFound()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync(10))
                .ReturnsAsync((Patient?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetHealthRecordsByUserIdAsync(10));
        }

        [Fact]
        public async Task GetHealthRecordsByUserIdAsync_ShouldThrow_WhenPatientWithHealthRecordsNotFound()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync(10))
                .ReturnsAsync(new Patient
                {
                    PatientId = 1
                });

            _patientRepositoryMock
                .Setup(x => x.GetPatientWithHealthRecordsAsync(1))
                .ReturnsAsync((Patient?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetHealthRecordsByUserIdAsync(10));
        }

        [Fact]
        public async Task GetHealthRecordsByUserIdAsync_ShouldReturnHealthRecords()
        {
            // Arrange
            var patient = new Patient
            {
                PatientId = 1,
                UserId = 10,
                HealthRecords = new List<HealthRecord>
                {
                    new HealthRecord
                    {
                        RecordId = 500
                    }
                }
            };

            var healthRecordDtos =
                new List<HealthRecordDto>
                {
                    new HealthRecordDto
                    {
                        RecordId = 500
                    }
                };

            _patientRepositoryMock
                .Setup(x => x.GetByUserIdAsync(10))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.GetPatientWithHealthRecordsAsync(1))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(x => x.Map<List<HealthRecordDto>>(patient.HealthRecords))
                .Returns(healthRecordDtos);

            // Act
            var result =
                await _service.GetHealthRecordsByUserIdAsync(10);

            // Assert
            Assert.Single(result);
            Assert.Equal(
                500,
                result.First().RecordId);
        }

        #endregion
    }
}
