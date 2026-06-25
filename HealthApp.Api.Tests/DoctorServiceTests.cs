using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Impl;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
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

        // ✅ ADD DOCTOR

        [Fact]
        public async Task AddDoctor_ShouldThrow_WhenEmailExists()
        {
            var dto = GetValidDoctorCreateDto();

            _doctorRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Doctor>
                {
                    new Doctor { DoctorEmail = dto.DoctorEmail }
                });

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _doctorService.AddDoctorAsync(dto));
        }

        [Fact]
        public async Task AddDoctor_ShouldAdd_WhenValid()
        {
            var dto = GetValidDoctorCreateDto();

            _doctorRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            _mapperMock.Setup(x => x.Map<Doctor>(dto))
                .Returns(new Doctor());

            await _doctorService.AddDoctorAsync(dto);

            _doctorRepositoryMock.Verify(x =>
                x.Add(It.IsAny<Doctor>()), Times.Once);
        }

        // ✅ UPDATE

        [Fact]
        public async Task UpdateDoctor_ShouldThrow_WhenNotFound()
        {
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _doctorService.UpdateDoctorAsync(1, GetValidDoctorCreateDto()));
        }

        [Fact]
        public async Task UpdateDoctor_ShouldThrow_WhenEmailConflict()
        {
            var dto = GetValidDoctorCreateDto();

            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Doctor { DoctorId = 1 });

            _doctorRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Doctor>
                {
                    new Doctor { DoctorId = 2, DoctorEmail = dto.DoctorEmail }
                });

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _doctorService.UpdateDoctorAsync(1, dto));
        }

        [Fact]
        public async Task UpdateDoctor_ShouldUpdate_WhenValid()
        {
            var dto = GetValidDoctorCreateDto();
            var doctor = new Doctor { DoctorId = 1 };

            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            await _doctorService.UpdateDoctorAsync(1, dto);

            _doctorRepositoryMock.Verify(x =>
                x.Update(1, It.IsAny<Doctor>()), Times.Once);
        }

        // ✅ CHANGE STATUS

        [Fact]
        public async Task ChangeStatus_ShouldThrow_WhenNotFound()
        {
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _doctorService.ChangeStatusAsync(1, true));
        }

        [Fact]
        public async Task ChangeStatus_ShouldThrow_WhenUpdateFails()
        {
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Doctor());

            _doctorRepositoryMock.Setup(x => x.ChangeStatusAsync(1, true))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                _doctorService.ChangeStatusAsync(1, true));
        }

        [Fact]
        public async Task ChangeStatus_ShouldUpdate_WhenValid()
        {
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Doctor());

            _doctorRepositoryMock.Setup(x => x.ChangeStatusAsync(1, true))
                .ReturnsAsync(true);

            await _doctorService.ChangeStatusAsync(1, true);

            _doctorRepositoryMock.Verify(x =>
                x.ChangeStatusAsync(1, true), Times.Once);
        }

        // ✅ GET BY SPECIALISATION

        [Fact]
        public async Task GetBySpecialisation_ShouldReturnDoctors()
        {
            var doctors = new List<Doctor>
            {
                new Doctor { Specialisation = SpecialisationType.Cardiologist, IsActive = true }
            };

            _doctorRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock.Setup(x => x.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(new List<DoctorDto> { new DoctorDto() });

            var result = await _doctorService.GetDoctorsBySpecialisationAsync(SpecialisationType.Cardiologist);

            Assert.Single(result);
        }

        // ✅ SEARCH (PAGED)

        [Fact]
        public async Task SearchDoctors_ShouldReturnPagedResult()
        {
            var doctors = new List<Doctor>
            {
                new Doctor()
            };

            _doctorRepositoryMock.Setup(x =>
                x.SearchDoctorsAsync("test", null, null, 1, 10))
                .ReturnsAsync((doctors, 1));

            _mapperMock.Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(new List<DoctorDto> { new DoctorDto() });

            var result = await _doctorService.SearchDoctorsAsync("test", null, null, 1, 10);

            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalCount);
        }
    }
}