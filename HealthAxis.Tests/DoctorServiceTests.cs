using AutoMapper;
using FluentAssertions;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementation;
using HealthAxis.Shared.DTO.DoctorDtos;
using HealthAxis.Shared.Enums;
using Moq;

namespace HealthAxis.API.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();

            _mapperMock
                .Setup(mapper => mapper.Map<DoctorDto>(It.IsAny<Doctor>()))
                .Returns((Doctor doctor) => MapDoctorDto(doctor));

            _mapperMock
                .Setup(mapper => mapper.Map<List<DoctorDto>>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    var doctors = source as IEnumerable<Doctor> ?? new List<Doctor>();
                    return doctors.Select(MapDoctorDto).ToList();
                });

            _service = new DoctorService(
                _doctorRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_WhenDoctorsExist_ReturnsDoctorDtos()
        {
            var doctors = new List<Doctor>
            {
                CreateDoctor(id: 1, fullName: "Dr John"),
                CreateDoctor(id: 2, fullName: "Dr Smith")
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result[0].DoctorId.Should().Be(1);
            result[0].FullName.Should().Be("Dr John");
            result[1].DoctorId.Should().Be(2);
            result[1].FullName.Should().Be("Dr Smith");

            _doctorRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoDoctors_ReturnsEmptyList()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _doctorRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorExists_ReturnsDoctorDto()
        {
            var doctor = CreateDoctor(id: 1, fullName: "Dr John");

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(1);
            result.FullName.Should().Be("Dr John");

            _doctorRepositoryMock.Verify(repository => repository.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            Func<Task> act = async () => await _service.GetByIdAsync(99);

            await act.Should().ThrowAsync<NotFoundException>();

            _doctorRepositoryMock.Verify(repository => repository.GetByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenDoctorProfileExists_ReturnsDoctorDto()
        {
            var doctors = new List<Doctor>
            {
                CreateDoctor(id: 1, fullName: "Dr John", userId: "doctor-user-1"),
                CreateDoctor(id: 2, fullName: "Dr Smith", userId: "doctor-user-2")
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var result = await _service.GetByUserIdAsync("doctor-user-2");

            result.Should().NotBeNull();
            result!.DoctorId.Should().Be(2);
            result.FullName.Should().Be("Dr Smith");

            _doctorRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenDoctorsListIsEmpty_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            Func<Task> act = async () => await _service.GetByUserIdAsync("missing-user");

            await act.Should().ThrowAsync<NotFoundException>();

            _doctorRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenMatchingDoctorDoesNotExist_ThrowsNotFoundException()
        {
            var doctors = new List<Doctor>
            {
                CreateDoctor(id: 1, fullName: "Dr John", userId: "doctor-user-1")
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            Func<Task> act = async () => await _service.GetByUserIdAsync("wrong-user");

            await act.Should().ThrowAsync<NotFoundException>();

            _doctorRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorIsActive_ReturnsDoctorDto()
        {
            var doctor = CreateDoctor(id: 1, fullName: "Dr John", isActive: true);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            var result = await _service.GetAvailabilityAsync(1);

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(1);
            result.IsActive.Should().BeTrue();

            _doctorRepositoryMock.Verify(repository => repository.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorIsInactive_ReturnsDoctorDto()
        {
            var doctor = CreateDoctor(id: 1, fullName: "Dr John", isActive: false);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            var result = await _service.GetAvailabilityAsync(1);

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(1);
            result.IsActive.Should().BeFalse();

            _doctorRepositoryMock.Verify(repository => repository.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(100))
                .ReturnsAsync((Doctor?)null);

            Func<Task> act = async () => await _service.GetAvailabilityAsync(100);

            await act.Should().ThrowAsync<NotFoundException>();

            _doctorRepositoryMock.Verify(repository => repository.GetByIdAsync(100), Times.Once);
        }

        private static Doctor CreateDoctor(
            int id = 1,
            string fullName = "Dr John",
            string userId = "doctor-user-1",
            bool isActive = true)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = fullName,
                UserId = userId,
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = isActive
            };
        }

        private static DoctorDto MapDoctorDto(Doctor doctor)
        {
            return new DoctorDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                Specialisation = doctor.Specialisation,
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive
            };
        }
    }
}