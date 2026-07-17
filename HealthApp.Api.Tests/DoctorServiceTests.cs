using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Impl;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using Moq;
using Xunit;

namespace HealthApp.Api.Tests.Services;

public class DoctorServiceTests
{
    private readonly Mock<IDoctorRepository> _doctorRepository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly DoctorService _service;

    public DoctorServiceTests()
    {
        _service = new DoctorService(
            _doctorRepository.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task GetAllDoctors_ShouldReturnMappedDoctors()
    {
        var doctors = new List<Doctor>
        {
            CreateDoctor(1),
            CreateDoctor(2)
        };
        var doctorDtos = new List<DoctorDto>
        {
            new() { DoctorId = 1 },
            new() { DoctorId = 2 }
        };

        _doctorRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(doctors);
        _mapper
            .Setup(mapper => mapper.Map<IEnumerable<DoctorDto>>(doctors))
            .Returns(doctorDtos);

        var result = await _service.GetAllDoctorsAsync();

        Assert.Equal(2, result.Count());
        Assert.Equal([1, 2], result.Select(doctor => doctor.DoctorId));
        _doctorRepository.Verify(
            repository => repository.GetAllAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetAllDoctors_WhenNoDoctorsExist_ShouldReturnEmpty()
    {
        var doctors = new List<Doctor>();
        _doctorRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(doctors);
        _mapper
            .Setup(mapper => mapper.Map<IEnumerable<DoctorDto>>(doctors))
            .Returns([]);

        var result = await _service.GetAllDoctorsAsync();

        Assert.Empty(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetDoctorById_WhenIdIsInvalid_ShouldThrow(int id)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.GetDoctorByIdAsync(id));

        _doctorRepository.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task GetDoctorById_WhenDoctorDoesNotExist_ShouldThrow()
    {
        _doctorRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.GetDoctorByIdAsync(1));
    }

    [Fact]
    public async Task GetDoctorById_WhenDoctorExists_ShouldReturnMappedDoctor()
    {
        var doctor = CreateDoctor(5);
        var dto = new DoctorDto { DoctorId = 5 };
        _doctorRepository
            .Setup(repository => repository.GetByIdAsync(5))
            .ReturnsAsync(doctor);
        _mapper
            .Setup(mapper => mapper.Map<DoctorDto>(doctor))
            .Returns(dto);

        var result = await _service.GetDoctorByIdAsync(5);

        Assert.Equal(5, result.DoctorId);
    }

    [Fact]
    public async Task AddDoctor_WhenDtoIsNull_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.AddDoctorAsync(null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AddDoctor_WhenNameIsMissing_ShouldThrow(string? value)
    {
        var dto = CreateDto();
        dto.FullName = value!;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.AddDoctorAsync(dto));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AddDoctor_WhenPhoneIsMissing_ShouldThrow(string? value)
    {
        var dto = CreateDto();
        dto.DoctorPhoneNo = value!;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.AddDoctorAsync(dto));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AddDoctor_WhenEmailIsMissing_ShouldThrow(string? value)
    {
        var dto = CreateDto();
        dto.DoctorEmail = value!;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.AddDoctorAsync(dto));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(61)]
    public async Task AddDoctor_WhenExperienceIsOutsideRange_ShouldThrow(
        int value)
    {
        var dto = CreateDto();
        dto.YearsOfExperience = value;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.AddDoctorAsync(dto));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(100001)]
    public async Task AddDoctor_WhenFeeIsOutsideRange_ShouldThrow(
        decimal value)
    {
        var dto = CreateDto();
        dto.ConsultationFee = value;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.AddDoctorAsync(dto));
    }

    [Fact]
    public async Task AddDoctor_WhenEmailExistsIgnoringCase_ShouldThrow()
    {
        var dto = CreateDto();
        _doctorRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync([
                new Doctor
                {
                    DoctorEmail = dto.DoctorEmail.ToUpperInvariant()
                }
            ]);

        await Assert.ThrowsAsync<DuplicateEntityException>(
            () => _service.AddDoctorAsync(dto));

        _doctorRepository.Verify(
            repository => repository.Add(It.IsAny<Doctor>()),
            Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task UpdateDoctor_WhenIdIsInvalid_ShouldThrow(int id)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.UpdateDoctorAsync(id, CreateDto()));
    }

    [Fact]
    public async Task UpdateDoctor_WhenDtoIsNull_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.UpdateDoctorAsync(1, null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpdateDoctor_WhenNameIsMissing_ShouldThrow(string? value)
    {
        var dto = CreateDto();
        dto.FullName = value!;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.UpdateDoctorAsync(1, dto));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpdateDoctor_WhenPhoneIsMissing_ShouldThrow(string? value)
    {
        var dto = CreateDto();
        dto.DoctorPhoneNo = value!;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.UpdateDoctorAsync(1, dto));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpdateDoctor_WhenEmailIsMissing_ShouldThrow(string? value)
    {
        var dto = CreateDto();
        dto.DoctorEmail = value!;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.UpdateDoctorAsync(1, dto));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(61)]
    public async Task UpdateDoctor_WhenExperienceIsOutsideRange_ShouldThrow(
        int value)
    {
        var dto = CreateDto();
        dto.YearsOfExperience = value;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.UpdateDoctorAsync(1, dto));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(100001)]
    public async Task UpdateDoctor_WhenFeeIsOutsideRange_ShouldThrow(
        decimal value)
    {
        var dto = CreateDto();
        dto.ConsultationFee = value;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.UpdateDoctorAsync(1, dto));
    }

    [Fact]
    public async Task UpdateDoctor_WhenDoctorDoesNotExist_ShouldThrow()
    {
        _doctorRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.UpdateDoctorAsync(1, CreateDto()));
    }

    [Fact]
    public async Task UpdateDoctor_WhenEmailIsUsedByAnotherDoctor_ShouldThrow()
    {
        var dto = CreateDto();
        _doctorRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(CreateDoctor(1));
        _doctorRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync([
                new Doctor
                {
                    DoctorId = 2,
                    DoctorEmail = dto.DoctorEmail.ToUpperInvariant()
                }
            ]);

        await Assert.ThrowsAsync<DuplicateEntityException>(
            () => _service.UpdateDoctorAsync(1, dto));

        _doctorRepository.Verify(
            repository => repository.Update(
                It.IsAny<int>(),
                It.IsAny<Doctor>()),
            Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task ChangeStatus_WhenIdIsInvalid_ShouldThrow(int id)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.ChangeStatusAsync(id, true));
    }

    [Fact]
    public async Task ChangeStatus_WhenDoctorDoesNotExist_ShouldThrow()
    {
        _doctorRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.ChangeStatusAsync(1, true));
    }

    [Fact]
    public async Task ChangeStatus_WhenRepositoryUpdateFails_ShouldThrow()
    {
        _doctorRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(CreateDoctor(1));
        _doctorRepository
            .Setup(repository => repository.ChangeStatusAsync(1, true))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.ChangeStatusAsync(1, true));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ChangeStatus_WhenValid_ShouldPassStatusToRepository(
        bool isActive)
    {
        _doctorRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(CreateDoctor(1));
        _doctorRepository
            .Setup(repository => repository.ChangeStatusAsync(1, isActive))
            .ReturnsAsync(true);

        await _service.ChangeStatusAsync(1, isActive);

        _doctorRepository.Verify(
            repository => repository.ChangeStatusAsync(1, isActive),
            Times.Once);
    }

    [Fact]
    public async Task GetDoctorsBySpecialisation_ShouldReturnOnlyActiveMatches()
    {
        var doctors = new List<Doctor>
        {
            new()
            {
                DoctorId = 1,
                Specialisation = SpecialisationType.Cardiologist,
                IsActive = true
            },
            new()
            {
                DoctorId = 2,
                Specialisation = SpecialisationType.Cardiologist,
                IsActive = false
            },
            new()
            {
                DoctorId = 3,
                Specialisation = SpecialisationType.Dermatologist,
                IsActive = true
            }
        };
        _doctorRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(doctors);
        _mapper
            .Setup(mapper => mapper.Map<IEnumerable<DoctorDto>>(
                It.IsAny<IEnumerable<Doctor>>()))
            .Returns((IEnumerable<Doctor> source) =>
                source.Select(doctor => new DoctorDto
                {
                    DoctorId = doctor.DoctorId
                }).ToList());

        var result = await _service.GetDoctorsBySpecialisationAsync(
            SpecialisationType.Cardiologist);

        var doctor = Assert.Single(result);
        Assert.Equal(1, doctor.DoctorId);
    }

    [Fact]
    public async Task GetDoctorsBySpecialisation_WhenNoMatches_ShouldReturnEmpty()
    {
        _doctorRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync([
                new Doctor
                {
                    DoctorId = 1,
                    Specialisation = SpecialisationType.Cardiologist,
                    IsActive = false
                },
                new Doctor
                {
                    DoctorId = 2,
                    Specialisation = SpecialisationType.Dermatologist,
                    IsActive = true
                }
            ]);
        _mapper
            .Setup(mapper => mapper.Map<IEnumerable<DoctorDto>>(
                It.IsAny<IEnumerable<Doctor>>()))
            .Returns([]);

        var result = await _service.GetDoctorsBySpecialisationAsync(
            SpecialisationType.Cardiologist);

        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchDoctors_ShouldReturnRepositoryPageAndMappedItems()
    {
        var doctors = new List<Doctor> { CreateDoctor(1) };
        var dtos = new List<DoctorDto>
        {
            new() { DoctorId = 1 }
        };
        _doctorRepository
            .Setup(repository => repository.SearchDoctorsAsync(
                "test",
                SpecialisationType.Cardiologist,
                true,
                2,
                5))
            .ReturnsAsync((doctors, 11));
        _mapper
            .Setup(mapper => mapper.Map<List<DoctorDto>>(doctors))
            .Returns(dtos);

        var result = await _service.SearchDoctorsAsync(
            "test",
            SpecialisationType.Cardiologist,
            true,
            2,
            5);

        Assert.Single(result.Items);
        Assert.Equal(2, result.PageNumber);
        Assert.Equal(5, result.PageSize);
        Assert.Equal(11, result.TotalCount);
    }

    [Fact]
    public async Task SearchDoctors_WhenNoResults_ShouldReturnEmptyPage()
    {
        var doctors = new List<Doctor>();
        _doctorRepository
            .Setup(repository => repository.SearchDoctorsAsync(
                "missing",
                null,
                null,
                1,
                10))
            .ReturnsAsync((doctors, 0));
        _mapper
            .Setup(mapper => mapper.Map<List<DoctorDto>>(doctors))
            .Returns([]);

        var result = await _service.SearchDoctorsAsync(
            "missing",
            null,
            null,
            1,
            10);

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    private static DoctorCreateDto CreateDto() => new()
    {
        FullName = "Dr Test",
        Specialisation = SpecialisationType.Cardiologist,
        DoctorPhoneNo = "9876543210",
        DoctorEmail = "doctor@test.com",
        YearsOfExperience = 5,
        ConsultationFee = 500
    };

    private static Doctor CreateDoctor(int id) => new()
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
