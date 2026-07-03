using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Impl;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private static Doctor GetDoctor(int id = 1)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = "Dr Test",
                Specialisation = SpecialisationType.Cardiologist,
                DoctorPhoneNo = "9876543210",
                DoctorEmail = "doctor@test.com",
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };
        }

        [Fact]
        public async Task GetAllDoctors_ShouldReturnMappedDoctors()
        {
            var doctors = new List<Doctor>
            {
                GetDoctor(1),
                GetDoctor(2)
            };

            var doctorDtos = new List<DoctorDto>
            {
                new DoctorDto { DoctorId = 1 },
                new DoctorDto { DoctorId = 2 }
            };

            _doctorRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock.Setup(x => x.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result = await _doctorService.GetAllDoctorsAsync();

            Assert.Equal(2, result.Count());
            _doctorRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetDoctorById_ShouldThrow_WhenIdIsInvalid(int id)
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _doctorService.GetDoctorByIdAsync(id));
        }

        [Fact]
        public async Task GetDoctorById_ShouldThrow_WhenDoctorNotFound()
        {
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _doctorService.GetDoctorByIdAsync(1));
        }

        [Fact]
        public async Task GetDoctorById_ShouldReturnMappedDoctor_WhenDoctorExists()
        {
            var doctor = GetDoctor(1);
            var doctorDto = new DoctorDto { DoctorId = 1 };

            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _mapperMock.Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(doctorDto);

            var result = await _doctorService.GetDoctorByIdAsync(1);

            Assert.Equal(1, result.DoctorId);
        }

        [Fact]
        public async Task AddDoctor_ShouldThrow_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _doctorService.AddDoctorAsync(null!));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task AddDoctor_ShouldThrow_WhenFullNameIsMissing(string? fullName)
        {
            var dto = GetValidDoctorCreateDto();
            dto.FullName = fullName!;

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _doctorService.AddDoctorAsync(dto));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task AddDoctor_ShouldThrow_WhenPhoneNumberIsMissing(string? phoneNumber)
        {
            var dto = GetValidDoctorCreateDto();
            dto.DoctorPhoneNo = phoneNumber!;

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _doctorService.AddDoctorAsync(dto));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task AddDoctor_ShouldThrow_WhenEmailIsMissing(string? email)
        {
            var dto = GetValidDoctorCreateDto();
            dto.DoctorEmail = email!;

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _doctorService.AddDoctorAsync(dto));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(61)]
        public async Task AddDoctor_ShouldThrow_WhenYearsOfExperienceIsOutOfRange(int yearsOfExperience)
        {
            var dto = GetValidDoctorCreateDto();
            dto.YearsOfExperience = yearsOfExperience;

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _doctorService.AddDoctorAsync(dto));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(100001)]
        public async Task AddDoctor_ShouldThrow_WhenConsultationFeeIsOutOfRange(decimal consultationFee)
        {
            var dto = GetValidDoctorCreateDto();
            dto.ConsultationFee = consultationFee;

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _doctorService.AddDoctorAsync(dto));
        }

        [Fact]
        public async Task AddDoctor_ShouldThrow_WhenEmailExistsIgnoringCase()
        {
            var dto = GetValidDoctorCreateDto();

            _doctorRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Doctor>
                {
                    new Doctor { DoctorEmail = dto.DoctorEmail.ToUpperInvariant() }
                });

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _doctorService.AddDoctorAsync(dto));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task UpdateDoctor_ShouldThrow_WhenIdIsInvalid(int id)
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _doctorService.UpdateDoctorAsync(id, GetValidDoctorCreateDto()));
        }

        [Fact]
        public async Task UpdateDoctor_ShouldThrow_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _doctorService.UpdateDoctorAsync(1, null!));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task UpdateDoctor_ShouldThrow_WhenFullNameIsMissing(string? fullName)
        {
            var dto = GetValidDoctorCreateDto();
            dto.FullName = fullName!;

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _doctorService.UpdateDoctorAsync(1, dto));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(61)]
        public async Task UpdateDoctor_ShouldThrow_WhenYearsOfExperienceIsOutOfRange(int yearsOfExperience)
        {
            var dto = GetValidDoctorCreateDto();
            dto.YearsOfExperience = yearsOfExperience;

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _doctorService.UpdateDoctorAsync(1, dto));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(100001)]
        public async Task UpdateDoctor_ShouldThrow_WhenConsultationFeeIsOutOfRange(decimal consultationFee)
        {
            var dto = GetValidDoctorCreateDto();
            dto.ConsultationFee = consultationFee;

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _doctorService.UpdateDoctorAsync(1, dto));
        }

        [Fact]
        public async Task UpdateDoctor_ShouldThrow_WhenDoctorNotFound()
        {
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _doctorService.UpdateDoctorAsync(1, GetValidDoctorCreateDto()));
        }

        [Fact]
        public async Task UpdateDoctor_ShouldThrow_WhenEmailIsUsedByAnotherDoctorIgnoringCase()
        {
            var dto = GetValidDoctorCreateDto();

            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(GetDoctor(1));

            _doctorRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Doctor>
                {
                    new Doctor { DoctorId = 2, DoctorEmail = dto.DoctorEmail.ToUpperInvariant() }
                });

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _doctorService.UpdateDoctorAsync(1, dto));
        }        

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task ChangeStatus_ShouldThrow_WhenIdIsInvalid(int id)
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _doctorService.ChangeStatusAsync(id, true));
        }

        [Fact]
        public async Task ChangeStatus_ShouldThrow_WhenDoctorNotFound()
        {
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _doctorService.ChangeStatusAsync(1, true));
        }

        [Fact]
        public async Task ChangeStatus_ShouldThrow_WhenRepositoryUpdateFails()
        {
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(GetDoctor(1));

            _doctorRepositoryMock.Setup(x => x.ChangeStatusAsync(1, true))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                _doctorService.ChangeStatusAsync(1, true));
        }

        [Fact]
        public async Task ChangeStatus_ShouldUpdate_WhenValid()
        {
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(GetDoctor(1));

            _doctorRepositoryMock.Setup(x => x.ChangeStatusAsync(1, true))
                .ReturnsAsync(true);

            await _doctorService.ChangeStatusAsync(1, true);

            _doctorRepositoryMock.Verify(x => x.ChangeStatusAsync(1, true), Times.Once);
        }

        [Fact]
        public async Task GetDoctorsBySpecialisation_ShouldReturnOnlyActiveDoctorsWithMatchingSpecialisation()
        {
            var doctors = new List<Doctor>
            {
                new Doctor { DoctorId = 1, Specialisation = SpecialisationType.Cardiologist, IsActive = true },
                new Doctor { DoctorId = 2, Specialisation = SpecialisationType.Cardiologist, IsActive = false },
                new Doctor { DoctorId = 3, Specialisation = SpecialisationType.Dermatologist, IsActive = true }
            };

            _doctorRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock.Setup(x => x.Map<IEnumerable<DoctorDto>>(It.IsAny<IEnumerable<Doctor>>()))
                .Returns((IEnumerable<Doctor> source) =>
                    source.Select(d => new DoctorDto { DoctorId = d.DoctorId }).ToList());

            var result = await _doctorService.GetDoctorsBySpecialisationAsync(SpecialisationType.Cardiologist);

            var doctorDto = Assert.Single(result);
            Assert.Equal(1, doctorDto.DoctorId);
        }

        [Fact]
        public async Task SearchDoctors_ShouldReturnPagedResult()
        {
            var doctors = new List<Doctor>
            {
                GetDoctor(1)
            };

            var doctorDtos = new List<DoctorDto>
            {
                new DoctorDto { DoctorId = 1 }
            };

            _doctorRepositoryMock.Setup(x =>
                    x.SearchDoctorsAsync("test", null, null, 1, 10))
                .ReturnsAsync((doctors, 1));

            _mapperMock.Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result = await _doctorService.SearchDoctorsAsync("test", null, null, 1, 10);

            Assert.Single(result.Items);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(1, result.TotalCount);
        }
    }
}
