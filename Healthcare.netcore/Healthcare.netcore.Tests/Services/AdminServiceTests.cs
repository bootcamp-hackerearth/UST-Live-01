using AutoMapper;
using FluentAssertions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Impl;
using HealthAxis.Shared.DTOs.Doctor;
using HealthAxis.Shared.Enums;
using Moq;
using Xunit;

namespace Healthcare.netcore.Tests.Services
{
    public class AdminServiceTests
    {
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AdminService _service;

        public AdminServiceTests()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new AdminService(
                _doctorRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetDoctorsAsync_ReturnsDoctors()
        {
            var doctors = new List<Doctor>
            {
                new()
                {
                    DoctorId = 1,
                    FullName = "Doctor One"
                }
            };

            var doctorDtos = new List<DoctorDto>
            {
                new()
                {
                    DoctorId = 1,
                    FullName = "Doctor One"
                }
            };

            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result = await _service.GetDoctorsAsync();

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task CreateDoctorAsync_ReturnsDoctor()
        {
            var dto = new CreateDoctorDto
            {
                FullName = "Doctor One",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 500
            };

            var doctor = new Doctor
            {
                DoctorId = 1,
                FullName = dto.FullName
            };

            var doctorDto = new DoctorDto
            {
                DoctorId = 1,
                FullName = dto.FullName
            };

            _mapperMock
                .Setup(x => x.Map<Doctor>(dto))
                .Returns(doctor);

            _doctorRepositoryMock
                .Setup(x => x.AddAsync(doctor))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(doctorDto);

            var result = await _service.CreateDoctorAsync(dto);

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(1);
        }

        [Fact]
        public async Task UpdateDoctorAsync_ReturnsNull_WhenDoctorNotFound()
        {
            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync((Doctor?)null);

            var dto = new UpdateDoctorDto();

            var result =
                await _service.UpdateDoctorAsync(10, dto);

            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateDoctorAsync_ReturnsUpdatedDoctor()
        {
            var doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Old Name"
            };

            var dto = new UpdateDoctorDto
            {
                FullName = "New Name"
            };

            var doctorDto = new DoctorDto
            {
                DoctorId = 1,
                FullName = "New Name"
            };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(x => x.UpdateAsync(
                    1,
                    doctor,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(x => x.Map(dto, doctor));

            _mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(doctorDto);

            var result =
                await _service.UpdateDoctorAsync(1, dto);

            result.Should().NotBeNull();
            result!.FullName.Should().Be("New Name");
        }
    }
}