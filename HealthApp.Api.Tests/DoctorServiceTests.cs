using AutoMapper;
using HealthApp.Api.Dtos;
using HealthApp.Api.Enums;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Impl;
using Moq;
using Xunit;

namespace HealthApp.Api.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DoctorService _doctorService;

        public DoctorServiceTests()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();

            _doctorService = new DoctorService(
                _doctorRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllDoctorsAsync_ShouldReturnDoctorDtos()
        {
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr John",
                    Specialisation = SpecialisationType.Cardiologist,
                    DoctorPhoneNo = "9876543210",
                    DoctorEmail = "john@test.com",
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                }
            };

            var doctorDtos = new List<DoctorDto>
            {
                new DoctorDto
                {
                    DoctorId = 1,
                    FullName = "Dr John",
                    Specialisation = "Cardiologist",
                    DoctorPhoneNo = "9876543210",
                    DoctorEmail = "john@test.com",
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                }
            };

            _doctorRepositoryMock
                .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result = await _doctorService.GetAllDoctorsAsync();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Dr John", result.First().FullName);

            _doctorRepositoryMock.Verify(
                repo => repo.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_WhenIdIsInvalid_ShouldThrowInvalidRequestException()
        {
            var act = async () => await _doctorService.GetDoctorByIdAsync(0);

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(act);
            Assert.Equal("Valid doctor id is required.", exception.Message);

            _doctorRepositoryMock.Verify(
                repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            int doctorId = 10;

            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(doctorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            var act = async () => await _doctorService.GetDoctorByIdAsync(doctorId);

            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(act);
            Assert.Equal("Doctor with id '10' was not found.", exception.Message);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_WhenDoctorExists_ShouldReturnDoctorDto()
        {
            int doctorId = 1;

            var doctor = new Doctor
            {
                DoctorId = doctorId,
                FullName = "Dr Alice",
                Specialisation = SpecialisationType.Dermatologist,
                DoctorPhoneNo = "9876543210",
                DoctorEmail = "alice@test.com",
                YearsOfExperience = 8,
                ConsultationFee = 700,
                IsActive = true
            };

            var doctorDto = new DoctorDto
            {
                DoctorId = doctorId,
                FullName = "Dr Alice",
                Specialisation = "Dermatologist",
                DoctorPhoneNo = "9876543210",
                DoctorEmail = "alice@test.com",
                YearsOfExperience = 8,
                ConsultationFee = 700,
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(doctorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(mapper => mapper.Map<DoctorDto>(doctor))
                .Returns(doctorDto);

            var result = await _doctorService.GetDoctorByIdAsync(doctorId);

            Assert.NotNull(result);
            Assert.Equal(doctorId, result.DoctorId);
            Assert.Equal("Dr Alice", result.FullName);
            Assert.Equal("Dermatologist", result.Specialisation);
        }

        [Fact]
        public async Task AddDoctorAsync_WhenDtoIsNull_ShouldThrowInvalidRequestException()
        {
            var act = async () => await _doctorService.AddDoctorAsync(null!);

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(act);
            Assert.Equal("Doctor data is required.", exception.Message);
        }

        [Fact]
        public async Task AddDoctorAsync_WhenDoctorNameIsEmpty_ShouldThrowInvalidRequestException()
        {
            var dto = GetValidDoctorCreateDto();
            dto.FullName = "";

            var act = async () => await _doctorService.AddDoctorAsync(dto);

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(act);
            Assert.Equal("Doctor name is required.", exception.Message);
        }

        [Fact]
        public async Task AddDoctorAsync_WhenEmailAlreadyExists_ShouldThrowDuplicateEntityException()
        {
            var dto = GetValidDoctorCreateDto();

            var existingDoctors = new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr Existing",
                    DoctorEmail = "doctor@test.com",
                    DoctorPhoneNo = "9876543210",
                    Specialisation = SpecialisationType.Cardiologist,
                    IsActive = true
                }
            };

            _doctorRepositoryMock
                .Setup(repo => repo.SearchDoctorsAsync(
                    null,
                    null,
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingDoctors);

            var act = async () => await _doctorService.AddDoctorAsync(dto);

            var exception = await Assert.ThrowsAsync<DuplicateEntityException>(act);
            Assert.Equal("A doctor with this email already exists.", exception.Message);

            _doctorRepositoryMock.Verify(
                repo => repo.Add(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task AddDoctorAsync_WhenValid_ShouldAddDoctor()
        {
            var dto = GetValidDoctorCreateDto();

            var mappedDoctor = new Doctor
            {
                FullName = dto.FullName,
                DoctorEmail = dto.DoctorEmail,
                DoctorPhoneNo = dto.DoctorPhoneNo,
                Specialisation = dto.Specialisation!.Value,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee
            };

            _doctorRepositoryMock
                .Setup(repo => repo.SearchDoctorsAsync(
                    null, null, null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Doctor>());

            _mapperMock
                .Setup(mapper => mapper.Map<Doctor>(dto))
                .Returns(mappedDoctor);

            _doctorRepositoryMock
                .Setup(repo => repo.Add(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor doctor, CancellationToken _) => doctor);

            await _doctorService.AddDoctorAsync(dto);

            _doctorRepositoryMock.Verify(
                repo => repo.Add(It.Is<Doctor>(doctor =>
                    doctor.FullName == "Dr Test" &&
                    doctor.DoctorEmail == "doctor@test.com" &&
                    doctor.DoctorPhoneNo == "9876543210" &&
                    doctor.Specialisation == SpecialisationType.Cardiologist &&
                    doctor.IsActive == true),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorAsync_ShouldThrow_WhenNotFound()
        {
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _doctorService.UpdateDoctorAsync(1, GetValidDoctorCreateDto()));
        }

        [Fact]
        public async Task UpdateDoctorAsync_ShouldUpdate_WhenValid()
        {
            var doctor = new Doctor { DoctorId = 1 };

            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock.Setup(x => x.SearchDoctorsAsync(null, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Doctor>());

            await _doctorService.UpdateDoctorAsync(1, GetValidDoctorCreateDto());

            _doctorRepositoryMock.Verify(x =>
                x.Update(1, It.IsAny<Doctor>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ChangeStatus_ShouldThrow_WhenDoctorNotFound()
        {
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _doctorService.ChangeStatusAsync(1, false));
        }

        [Fact]
        public async Task ChangeStatus_ShouldUpdate_WhenValid()
        {
            var doc = new Doctor { DoctorId = 1 };

            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doc);

            _doctorRepositoryMock.Setup(x => x.ChangeStatusAsync(1, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await _doctorService.ChangeStatusAsync(1, false);

            _doctorRepositoryMock.Verify(x =>
                x.ChangeStatusAsync(1, false, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task SearchDoctors_ShouldReturnResults()
        {
            _doctorRepositoryMock.Setup(x =>
                x.SearchDoctorsAsync("test", null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Doctor> { new Doctor() });

            _mapperMock.Setup(x => x.Map<IEnumerable<DoctorDto>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<DoctorDto> { new DoctorDto() });

            var result = await _doctorService.SearchDoctorsAsync("test", null, null);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetBySpecialisation_ShouldReturnDoctors()
        {
            _doctorRepositoryMock.Setup(x =>
                x.SearchDoctorsAsync(null, SpecialisationType.Cardiologist, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Doctor> { new Doctor() });

            _mapperMock.Setup(x =>
                x.Map<IEnumerable<DoctorDto>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<DoctorDto> { new DoctorDto() });

            var result = await _doctorService.GetDoctorsBySpecialisationAsync(SpecialisationType.Cardiologist);

            Assert.Single(result);
        }
        

        private static DoctorCreateDto GetValidDoctorCreateDto()
        {
            return new DoctorCreateDto
            {
                FullName = "Dr Test",
                Specialisation = SpecialisationType.Cardiologist,
                DoctorPhoneNo = "9876543210",
                DoctorEmail = "doctor@test.com",
                YearsOfExperience = 5,
                ConsultationFee = 500
            };
        }
    }
}