using FluentAssertions;
using HealthAppWebApi.Models;
using HealthAppWebApi.Repositories.Interface;
using HealthAppWebApi.Services.Impl;
using Moq;
using SharedDto.DoctorDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthAppWebApi.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _repoMock;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _repoMock = new Mock<IDoctorRepository>();
            _service = new DoctorService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllDoctorsAsync_ShouldReturnDoctorDtos()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr John Smith",
                    Specialisation = 1,
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true,
                    DoctorPhoneNo = "9876543210",
                    DoctorEmail = "john.smith@example.com"
                },
                new Doctor
                {
                    DoctorId = 2,
                    FullName = "Dr Jane Doe",
                    Specialisation = 2,
                    YearsOfExperience = 8,
                    ConsultationFee = 700,
                    IsActive = true,
                    DoctorPhoneNo = "9876543211",
                    DoctorEmail = "jane.doe@example.com"
                }
            };

            _repoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(doctors);

            // Act
            var result = await _service.GetAllDoctorsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            result[0].DoctorId.Should().Be(1);
            result[0].FullName.Should().Be("Dr John Smith");
            result[0].ConsultationFee.Should().Be(500);
            result[0].IsActive.Should().BeTrue();
            result[0].DoctorPhoneNo.Should().Be("9876543210");
            result[0].DoctorEmail.Should().Be("john.smith@example.com");

            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_WhenDoctorExists_ShouldReturnDoctorDto()
        {
            // Arrange
            var doctor = CreateDoctor();

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            // Act
            var result = await _service.GetDoctorByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.DoctorId.Should().Be(1);
            result.FullName.Should().Be("Dr John Smith");
            result.ConsultationFee.Should().Be(500);
            result.IsActive.Should().BeTrue();
            result.DoctorPhoneNo.Should().Be("9876543210");
            result.DoctorEmail.Should().Be("john.smith@example.com");

            _repoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_WhenDoctorDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Doctor)null);

            // Act
            var result = await _service.GetDoctorByIdAsync(99);

            // Assert
            result.Should().BeNull();

            _repoMock.Verify(r => r.GetByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task AddDoctorAsync_WhenSpecialisationIsInvalid_ShouldThrowException()
        {
            // Arrange
            var dto = CreateDoctorDto();
            dto.Specialisation = "InvalidSpecialisation";

            // Act
            Func<Task> act = async () =>
                await _service.AddDoctorAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Invalid Specialisation.");

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Doctor>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public async Task AddDoctorAsync_WhenConsultationFeeIsLessThanOrEqualToZero_ShouldThrowException(decimal consultationFee)
        {
            // Arrange
            var dto = CreateDoctorDto();
            dto.ConsultationFee = consultationFee;

            // Act
            Func<Task> act = async () =>
                await _service.AddDoctorAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Consultation fee must be greater than zero.");

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public async Task AddDoctorAsync_WhenYearsOfExperienceIsNegative_ShouldThrowException()
        {
            // Arrange
            var dto = CreateDoctorDto();
            dto.YearsOfExperience = -1;

            // Act
            Func<Task> act = async () =>
                await _service.AddDoctorAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Years of experience cannot be negative.");

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public async Task AddDoctorAsync_WhenValidData_ShouldAddDoctor()
        {
            // Arrange
            var dto = CreateDoctorDto();
            var expectedSpecialisation =
                Enum.Parse<SpecialisationType>(dto.Specialisation, true);

            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<Doctor>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddDoctorAsync(dto);

            // Assert
            _repoMock.Verify(r => r.AddAsync(It.Is<Doctor>(d =>
                d.FullName == dto.FullName &&
                d.Specialisation == (int)expectedSpecialisation &&
                d.YearsOfExperience == dto.YearsOfExperience &&
                d.ConsultationFee == dto.ConsultationFee &&
                d.DoctorEmail == dto.DoctorEmail &&
                d.DoctorPhoneNo == dto.DoctorPhoneNo &&
                d.IsActive == true
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenDoctorDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var dto = CreateDoctorDto();

            _repoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Doctor)null);

            // Act
            Func<Task> act = async () =>
                await _service.UpdateDoctorAsync(99, dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Doctor not found.");

            _repoMock.Verify(r => r.GetByIdAsync(99), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenSpecialisationIsInvalid_ShouldThrowException()
        {
            // Arrange
            var existingDoctor = CreateDoctor();

            var dto = CreateDoctorDto();
            dto.Specialisation = "InvalidSpecialisation";

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingDoctor);

            // Act
            Func<Task> act = async () =>
                await _service.UpdateDoctorAsync(1, dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Invalid Specialisation.");

            _repoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Doctor>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public async Task UpdateDoctorAsync_WhenConsultationFeeIsInvalid_ShouldThrowException(decimal consultationFee)
        {
            // Arrange
            var existingDoctor = CreateDoctor();

            var dto = CreateDoctorDto();
            dto.ConsultationFee = consultationFee;

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingDoctor);

            // Act
            Func<Task> act = async () =>
                await _service.UpdateDoctorAsync(1, dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Consultation fee must be greater than zero.");

            _repoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenYearsOfExperienceIsNegative_ShouldThrowException()
        {
            // Arrange
            var existingDoctor = CreateDoctor();

            var dto = CreateDoctorDto();
            dto.YearsOfExperience = -2;

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingDoctor);

            // Act
            Func<Task> act = async () =>
                await _service.UpdateDoctorAsync(1, dto);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Years of experience cannot be negative.");

            _repoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenValidData_ShouldUpdateDoctor()
        {
            // Arrange
            var existingDoctor = CreateDoctor();

            var dto = CreateDoctorDto();
            dto.FullName = "Dr Updated Name";
            dto.ConsultationFee = 900;
            dto.YearsOfExperience = 12;
            dto.DoctorEmail = "updated.doctor@example.com";
            dto.DoctorPhoneNo = "9999999999";

            var expectedSpecialisation =
                Enum.Parse<SpecialisationType>(dto.Specialisation, true);

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingDoctor);

            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Doctor>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateDoctorAsync(1, dto);

            // Assert
            _repoMock.Verify(r => r.UpdateAsync(It.Is<Doctor>(d =>
                d.DoctorId == 1 &&
                d.FullName == dto.FullName &&
                d.Specialisation == (int)expectedSpecialisation &&
                d.YearsOfExperience == dto.YearsOfExperience &&
                d.ConsultationFee == dto.ConsultationFee &&
                d.DoctorEmail == dto.DoctorEmail &&
                d.DoctorPhoneNo == dto.DoctorPhoneNo
            )), Times.Once);
        }

        [Fact]
        public async Task ChangeStatusAsync_WhenDoctorDoesNotExist_ShouldThrowException()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Doctor)null);

            // Act
            Func<Task> act = async () =>
                await _service.ChangeStatusAsync(99, false);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Doctor not found.");

            _repoMock.Verify(r => r.GetByIdAsync(99), Times.Once);
            _repoMock.Verify(r => r.ChangeStatusAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ChangeStatusAsync_WhenDoctorExists_ShouldChangeStatus(bool isActive)
        {
            // Arrange
            var doctor = CreateDoctor();

            _repoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _repoMock
                .Setup(r => r.ChangeStatusAsync(1, isActive))
                .Returns(Task.CompletedTask);

            // Act
            await _service.ChangeStatusAsync(1, isActive);

            // Assert
            _repoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
            _repoMock.Verify(r => r.ChangeStatusAsync(1, isActive), Times.Once);
        }

        [Fact]
        public async Task GetDoctorsBySpecialisationAsync_WhenSpecialisationIsInvalid_ShouldThrowException()
        {
            // Arrange
            string invalidSpecialisation = "InvalidSpecialisation";

            // Act
            Func<Task> act = async () =>
                await _service.GetDoctorsBySpecialisationAsync(invalidSpecialisation);

            // Assert
            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Invalid Specialisation.");

            _repoMock.Verify(
                r => r.GetBySpecialisationAsync(It.IsAny<SpecialisationType>()),
                Times.Never);
        }

        [Fact]
        public async Task GetDoctorsBySpecialisationAsync_WhenValid_ShouldReturnDoctorDtos()
        {
            // Arrange
            string specialisation = GetValidSpecialisationName();
            var parsedSpecialisation =
                Enum.Parse<SpecialisationType>(specialisation, true);

            var doctors = new List<Doctor>
            {
                CreateDoctor()
            };

            _repoMock
                .Setup(r => r.GetBySpecialisationAsync(parsedSpecialisation))
                .ReturnsAsync(doctors);

            // Act
            var result = await _service.GetDoctorsBySpecialisationAsync(specialisation);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].DoctorId.Should().Be(1);
            result[0].FullName.Should().Be("Dr John Smith");
            result[0].ConsultationFee.Should().Be(500);
            result[0].IsActive.Should().BeTrue();

            _repoMock.Verify(r => r.GetBySpecialisationAsync(parsedSpecialisation), Times.Once);
        }

        [Fact]
        public async Task SearchByNameAsync_ShouldReturnMatchingDoctors()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                CreateDoctor()
            };

            _repoMock
                .Setup(r => r.SearchByNameAsync("John"))
                .ReturnsAsync(doctors);

            // Act
            var result = await _service.SearchByNameAsync("John");

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            result[0].DoctorId.Should().Be(1);
            result[0].FullName.Should().Be("Dr John Smith");
            result[0].DoctorEmail.Should().Be("john.smith@example.com");

            _repoMock.Verify(r => r.SearchByNameAsync("John"), Times.Once);
        }

        private static Doctor CreateDoctor()
        {
            return new Doctor
            {
                DoctorId = 1,
                FullName = "Dr John Smith",
                Specialisation = 1,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true,
                DoctorPhoneNo = "9876543210",
                DoctorEmail = "john.smith@example.com"
            };
        }

        private static CreateDoctorDto CreateDoctorDto()
        {
            return new CreateDoctorDto
            {
                FullName = "Dr John Smith",
                Specialisation = GetValidSpecialisationName(),
                YearsOfExperience = 10,
                ConsultationFee = 500,
                DoctorEmail = "john.smith@example.com",
                DoctorPhoneNo = "9876543210"
            };
        }

        private static string GetValidSpecialisationName()
        {
            return Enum.GetNames(typeof(SpecialisationType)).First();
        }
    }
}