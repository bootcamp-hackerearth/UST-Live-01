using AutoMapper;
using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.DTOs.DoctorDtos;
using HealthAxisHealth.API.Enums;
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
    public class DoctorServiceTests
    {
        #region Fields

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly DoctorService _service;

        #endregion

        #region Constructor

        public DoctorServiceTests()
        {
            _unitOfWorkMock =
                new Mock<IUnitOfWork>();

            _doctorRepositoryMock =
                new Mock<IDoctorRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _unitOfWorkMock
                .Setup(x => x.Doctors)
                .Returns(_doctorRepositoryMock.Object);

            _service =
                new DoctorService(
                    _unitOfWorkMock.Object,
                    _mapperMock.Object);
        }

        #endregion

        #region GetPagedAsync

        [Fact]
        public async Task GetPagedAsync_ShouldReturnPagedDoctors()
        {
            // Arrange
            var pagination = new PaginationParams();

            var doctors =
                new List<Doctor>
                {
                    new Doctor
                    {
                        DoctorId = 1,
                        FullName = "Doctor One"
                    }
                };

            var doctorDtos =
                new List<DoctorDto>
                {
                    new DoctorDto
                    {
                        DoctorId = 1,
                        FullName = "Doctor One"
                    }
                };

            var pagedResult =
                new PagedResultDto<Doctor>
                {
                    Items = doctors,
                    PageNumber = 1,
                    PageSize = 10,
                    TotalRecords = 1
                };

            _doctorRepositoryMock
                .Setup(x => x.GetPagedAsync(pagination))
                .ReturnsAsync(pagedResult);

            _mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            // Act
            var result =
                await _service.GetPagedAsync(
                    pagination);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal(
                1,
                result.TotalRecords);
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenDoctorNotFound()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetByIdAsync(1));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDoctor()
        {
            // Arrange
            Doctor doctor =
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Doctor One"
                };

            DoctorDto dto =
                new DoctorDto
                {
                    DoctorId = 1,
                    FullName = "Doctor One"
                };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(dto);

            // Act
            var result =
                await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(
                1,
                result!.DoctorId);
        }

        #endregion

        #region GetByUserIdAsync

        [Fact]
        public async Task GetByUserIdAsync_ShouldThrow_WhenDoctorNotFound()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(x => x.GetByUserIdAsync(10))
                .ReturnsAsync((Doctor?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetByUserIdAsync(10));
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnDoctor()
        {
            // Arrange
            Doctor doctor =
                new Doctor
                {
                    DoctorId = 2,
                    UserId = 10
                };

            DoctorDto dto =
                new DoctorDto
                {
                    DoctorId = 2
                };

            _doctorRepositoryMock
                .Setup(x => x.GetByUserIdAsync(10))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(dto);

            // Act
            var result =
                await _service.GetByUserIdAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(
                2,
                result!.DoctorId);
        }

        #endregion
        #region GetBySpecialisationAsync

        [Fact]
        public async Task GetBySpecialisationAsync_ShouldReturnEmpty_WhenSpecialisationIsInvalid()
        {
            // Arrange
            string specialisation = "InvalidSpecialisation";

            // Act
            var result =
                await _service.GetBySpecialisationAsync(
                    specialisation);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetBySpecialisationAsync_ShouldReturnDoctors_WhenSpecialisationIsValid()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr. John",
                    Specialisation = Specialisation.Cardiology
                }
            };

            var doctorDtos = new List<DoctorDto>
            {
                new DoctorDto
                {
                    DoctorId = 1,
                    FullName = "Dr. John",
                    Specialisation = Specialisation.Cardiology
                }
            };

            _doctorRepositoryMock
                .Setup(x => x.GetBySpecialisationAsync(
                    Specialisation.Cardiology))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            // Act
            var result =
                await _service.GetBySpecialisationAsync(
                    "Cardiology");

            // Assert
            Assert.Single(result);
        }

        #endregion

        #region GetAvailabilityAsync

        [Fact]
        public async Task GetAvailabilityAsync_ShouldThrow_WhenDoctorNotFound()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(x => x.GetDoctorWithAppointmentsAsync(1))
                .ReturnsAsync((Doctor?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetAvailabilityAsync(1));
        }

        [Fact]
        public async Task GetAvailabilityAsync_ShouldReturnAvailability()
        {
            // Arrange
            Doctor doctor = new Doctor
            {
                DoctorId = 1,
                Appointments = new List<Appointment>
                {
                    new Appointment
                    {
                        ScheduledDate = new DateTime(2025, 6, 20),
                        TimeSlot = "09:00-09:30"
                    },
                    new Appointment
                    {
                        ScheduledDate = new DateTime(2025, 6, 20),
                        TimeSlot = "10:00-10:30"
                    }
                }
            };

            _doctorRepositoryMock
                .Setup(x => x.GetDoctorWithAppointmentsAsync(1))
                .ReturnsAsync(doctor);

            // Act
            var result =
                await _service.GetAvailabilityAsync(1);

            // Assert
            Assert.Equal(2, result.Count());

            Assert.Contains(result,
                x => x.TimeSlot == "09:00-09:30");

            Assert.Contains(result,
                x => x.TimeSlot == "10:00-10:30");
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenDoctorNotFound()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            var dto = new UpdateDoctorDto();

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateAsync(1, dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateDoctor()
        {
            // Arrange
            Doctor doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Old Name"
            };

            UpdateDoctorDto dto =
                new UpdateDoctorDto
                {
                    FullName = "New Name"
                };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            // Act
            await _service.UpdateAsync(1, dto);

            // Assert
            _mapperMock.Verify(
                x => x.Map(dto, doctor),
                Times.Once);

            _doctorRepositoryMock.Verify(
                x => x.Update(doctor),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        #endregion
    }
}
