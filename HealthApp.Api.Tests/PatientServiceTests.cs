using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Impl;
using HealthApp.Shared.Dtos;
using Moq;
using Xunit;

namespace HealthApp.Api.Tests.Services;

public class PatientServiceTests
{
    private readonly Mock<IPatientRepository> _patientRepository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly PatientService _service;

    public PatientServiceTests()
    {
        _service = new PatientService(
            _patientRepository.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task GetAllPatientsAsync_WhenNoPatientsExist_ShouldReturnEmpty()
    {
        var patients = new List<Patient>();
        _patientRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(patients);
        _mapper
            .Setup(mapper =>
                mapper.Map<IEnumerable<PatientDto>>(patients))
            .Returns(new List<PatientDto>());

        var result = await _service.GetAllPatientsAsync();

        Assert.Empty(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetPatientByIdAsync_WhenIdIsInvalid_ShouldThrow(int id)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.GetPatientByIdAsync(id));

        _patientRepository.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task GetPatientByIdAsync_WhenPatientDoesNotExist_ShouldThrow()
    {
        _patientRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.GetPatientByIdAsync(1));
    }

    [Fact]
    public async Task GetPatientByIdAsync_WhenPatientExists_ShouldReturnMappedPatient()
    {
        var patient = CreatePatient(1);
        var patientDto = new PatientDto { PatientId = 1 };
        _patientRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(patient);
        _mapper
            .Setup(mapper => mapper.Map<PatientDto>(patient))
            .Returns(patientDto);

        var result = await _service.GetPatientByIdAsync(1);

        Assert.Equal(1, result.PatientId);
    }

    [Fact]
    public async Task RegisterPatientAsync_WhenDtoIsNull_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.RegisterPatientAsync(null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RegisterPatientAsync_WhenNameIsMissing_ShouldThrow(
        string? fullName)
    {
        var dto = CreateDto();
        dto.FullName = fullName!;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.RegisterPatientAsync(dto));
    }

    [Fact]
    public async Task RegisterPatientAsync_WhenDateOfBirthIsMissing_ShouldThrow()
    {
        var dto = CreateDto();
        dto.DateOfBirth = null;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.RegisterPatientAsync(dto));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RegisterPatientAsync_WhenGenderIsMissing_ShouldThrow(
        string? gender)
    {
        var dto = CreateDto();
        dto.Gender = gender!;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.RegisterPatientAsync(dto));
    }

    [Theory]
    [InlineData("Male")]
    [InlineData("Female")]
    [InlineData("Other")]
    public async Task RegisterPatientAsync_WhenGenderIsValid_ShouldAdd(
        string gender)
    {
        var dto = CreateDto();
        dto.Gender = gender;
        var mappedPatient = new Patient();
        SetupSuccessfulRegistration(dto, mappedPatient);

        await _service.RegisterPatientAsync(dto);

        Assert.Equal(gender, mappedPatient.Gender);
        _patientRepository.Verify(
            repository => repository.Add(mappedPatient),
            Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RegisterPatientAsync_WhenPhoneIsMissing_ShouldThrow(
        string? phoneNumber)
    {
        var dto = CreateDto();
        dto.PhoneNumber = phoneNumber!;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.RegisterPatientAsync(dto));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RegisterPatientAsync_WhenEmailIsMissing_ShouldThrow(
        string? email)
    {
        var dto = CreateDto();
        dto.Email = email!;

        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.RegisterPatientAsync(dto));
    }

    [Fact]
    public async Task RegisterPatientAsync_WhenDateOfBirthIsFuture_ShouldThrow()
    {
        var dto = CreateDto();
        dto.DateOfBirth = DateOnly.FromDateTime(
            DateTime.Today.AddDays(1));

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.RegisterPatientAsync(dto));
    }

    [Fact]
    public async Task RegisterPatientAsync_WhenDateOfBirthIsToday_ShouldSucceed()
    {
        var dto = CreateDto();
        dto.DateOfBirth = DateOnly.FromDateTime(DateTime.Today);
        var mappedPatient = new Patient();
        SetupSuccessfulRegistration(dto, mappedPatient);

        await _service.RegisterPatientAsync(dto);

        Assert.Equal(dto.DateOfBirth, mappedPatient.DateOfBirth);
    }

    [Fact]
    public async Task RegisterPatientAsync_WhenEmailExistsIgnoringCase_ShouldThrow()
    {
        var dto = CreateDto();
        _patientRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(new List<Patient>
            {
                new()
                {
                    Email = dto.Email.ToUpperInvariant()
                }
            });

        await Assert.ThrowsAsync<DuplicateEntityException>(
            () => _service.RegisterPatientAsync(dto));

        _patientRepository.Verify(
            repository => repository.IsDuplicatePatient(
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task RegisterPatientAsync_WhenExistingEmailIsNull_ShouldContinue()
    {
        var dto = CreateDto();
        var mappedPatient = new Patient();
        _patientRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(new List<Patient>
            {
                new() { Email = null }
            });
        _patientRepository
            .Setup(repository => repository.IsDuplicatePatient(
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>()))
            .ReturnsAsync(false);
        _mapper
            .Setup(mapper => mapper.Map<Patient>(dto))
            .Returns(mappedPatient);

        await _service.RegisterPatientAsync(dto);

        _patientRepository.Verify(
            repository => repository.Add(mappedPatient),
            Times.Once);
    }

    [Fact]
    public async Task RegisterPatientAsync_WhenDuplicatePatientExists_ShouldThrow()
    {
        var dto = CreateDto();
        var expectedDate = dto.DateOfBirth!.Value
            .ToDateTime(TimeOnly.MinValue);
        _patientRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(new List<Patient>());
        _patientRepository
            .Setup(repository => repository.IsDuplicatePatient(
                dto.FullName,
                expectedDate,
                dto.Email))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<DuplicateEntityException>(
            () => _service.RegisterPatientAsync(dto));
    }

    [Fact]
    public async Task RegisterPatientAsync_WhenValid_ShouldTrimAndPersistValues()
    {
        var beforeRegistration = DateTime.Now;
        var dto = CreateDto();
        dto.FullName = "  John Doe  ";
        dto.Gender = " Male ";
        dto.PhoneNumber = "  9876543210  ";
        dto.Email = "  patient@test.com  ";
        dto.InsuranceId = "  INS001  ";
        var mappedPatient = new Patient();
        SetupSuccessfulRegistration(dto, mappedPatient);

        await _service.RegisterPatientAsync(dto);

        var afterRegistration = DateTime.Now;
        Assert.Equal("John Doe", mappedPatient.FullName);
        Assert.Equal(dto.DateOfBirth, mappedPatient.DateOfBirth);
        Assert.Equal("Male", mappedPatient.Gender);
        Assert.Equal("9876543210", mappedPatient.PhoneNumber);
        Assert.Equal("patient@test.com", mappedPatient.Email);
        Assert.Equal("INS001", mappedPatient.InsuranceId);
        Assert.InRange(
            mappedPatient.CreatedDate,
            beforeRegistration,
            afterRegistration);
        _patientRepository.Verify(
            repository => repository.Add(mappedPatient),
            Times.Once);
    }

    [Fact]
    public async Task RegisterPatientAsync_WhenInsuranceIsNull_ShouldPersistNull()
    {
        var dto = CreateDto();
        dto.InsuranceId = null;
        var mappedPatient = new Patient();
        SetupSuccessfulRegistration(dto, mappedPatient);

        await _service.RegisterPatientAsync(dto);

        Assert.Null(mappedPatient.InsuranceId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task UpdatePatientAsync_WhenIdIsInvalid_ShouldThrow(int id)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.UpdatePatientAsync(id, CreateDto()));
    }

    [Fact]
    public async Task UpdatePatientAsync_WhenDtoIsNull_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.UpdatePatientAsync(1, null!));
    }

    [Fact]
    public async Task UpdatePatientAsync_WhenDateOfBirthIsFuture_ShouldThrow()
    {
        var dto = CreateDto();
        dto.DateOfBirth = DateOnly.FromDateTime(
            DateTime.Today.AddDays(1));

        await Assert.ThrowsAsync<BusinessRuleViolationException>(
            () => _service.UpdatePatientAsync(1, dto));

        _patientRepository.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdatePatientAsync_WhenPatientDoesNotExist_ShouldThrow()
    {
        _patientRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.UpdatePatientAsync(1, CreateDto()));
    }

    [Fact]
    public async Task UpdatePatientAsync_WhenEmailIsUsedByAnotherPatient_ShouldThrow()
    {
        var dto = CreateDto();
        _patientRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(CreatePatient(1));
        _patientRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(new List<Patient>
            {
                new()
                {
                    PatientId = 2,
                    Email = dto.Email.ToUpperInvariant()
                }
            });

        await Assert.ThrowsAsync<DuplicateEntityException>(
            () => _service.UpdatePatientAsync(1, dto));

        _patientRepository.Verify(
            repository => repository.Update(
                It.IsAny<int>(),
                It.IsAny<Patient>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdatePatientAsync_WhenEmailBelongsToSamePatient_ShouldSucceed()
    {
        var dto = CreateDto();
        var patient = CreatePatient(1);
        _patientRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(patient);
        _patientRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(new List<Patient>
            {
                patient,
                new()
                {
                    PatientId = 2,
                    Email = "other@test.com"
                }
            });

        await _service.UpdatePatientAsync(1, dto);

        _patientRepository.Verify(
            repository => repository.Update(1, patient),
            Times.Once);
    }

    [Fact]
    public async Task UpdatePatientAsync_WhenValid_ShouldTrimAndUpdateExistingEntity()
    {
        var dto = CreateDto();
        dto.FullName = "  Updated Name  ";
        dto.Gender = " Other ";
        dto.PhoneNumber = "  9999999999  ";
        dto.Email = "  updated@test.com  ";
        dto.InsuranceId = "  NEW001  ";
        var patient = CreatePatient(1);
        _patientRepository
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(patient);
        _patientRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(new List<Patient> { patient });

        await _service.UpdatePatientAsync(1, dto);

        Assert.Equal("Updated Name", patient.FullName);
        Assert.Equal(dto.DateOfBirth, patient.DateOfBirth);
        Assert.Equal("Other", patient.Gender);
        Assert.Equal("9999999999", patient.PhoneNumber);
        Assert.Equal("updated@test.com", patient.Email);
        Assert.Equal("NEW001", patient.InsuranceId);
        _patientRepository.Verify(
            repository => repository.Update(1, patient),
            Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task SearchPatientsAsync_WhenPageNumberIsInvalid_ShouldThrow(
        int pageNumber)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.SearchPatientsAsync(
                null,
                null,
                pageNumber,
                10));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public async Task SearchPatientsAsync_WhenPageSizeIsInvalid_ShouldThrow(
        int pageSize)
    {
        await Assert.ThrowsAsync<InvalidRequestException>(
            () => _service.SearchPatientsAsync(
                null,
                null,
                1,
                pageSize));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public async Task SearchPatientsAsync_WhenPageSizeIsAtBoundary_ShouldSucceed(
        int pageSize)
    {
        var patients = new List<Patient>();
        _patientRepository
            .Setup(repository => repository.GetPatientsAsync(
                null,
                null,
                1,
                pageSize))
            .ReturnsAsync((patients, 0));
        _mapper
            .Setup(mapper => mapper.Map<List<PatientDto>>(patients))
            .Returns(new List<PatientDto>());

        var result = await _service.SearchPatientsAsync(
            null,
            null,
            1,
            pageSize);

        Assert.Equal(pageSize, result.PageSize);
    }

    [Fact]
    public async Task SearchPatientsAsync_ShouldPassFiltersAndReturnPagedResult()
    {
        var patients = new List<Patient>
        {
            CreatePatient(1)
        };
        var dtos = new List<PatientDto>
        {
            new() { PatientId = 1 }
        };
        _patientRepository
            .Setup(repository => repository.GetPatientsAsync(
                "John",
                "patient@test.com",
                2,
                5))
            .ReturnsAsync((patients, 11));
        _mapper
            .Setup(mapper => mapper.Map<List<PatientDto>>(patients))
            .Returns(dtos);

        var result = await _service.SearchPatientsAsync(
            "John",
            "patient@test.com",
            2,
            5);

        Assert.Single(result.Items);
        Assert.Equal(2, result.PageNumber);
        Assert.Equal(5, result.PageSize);
        Assert.Equal(11, result.TotalCount);
    }

    [Fact]
    public async Task SearchPatientsAsync_WhenNoResults_ShouldReturnEmptyPage()
    {
        var patients = new List<Patient>();
        _patientRepository
            .Setup(repository => repository.GetPatientsAsync(
                "missing",
                null,
                1,
                10))
            .ReturnsAsync((patients, 0));
        _mapper
            .Setup(mapper => mapper.Map<List<PatientDto>>(patients))
            .Returns(new List<PatientDto>());

        var result = await _service.SearchPatientsAsync(
            "missing",
            null,
            1,
            10);

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    private void SetupSuccessfulRegistration(
        PatientCreateDto dto,
        Patient mappedPatient)
    {
        _patientRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(new List<Patient>());
        _patientRepository
            .Setup(repository => repository.IsDuplicatePatient(
                dto.FullName.Trim(),
                dto.DateOfBirth!.Value.ToDateTime(TimeOnly.MinValue),
                dto.Email.Trim()))
            .ReturnsAsync(false);
        _mapper
            .Setup(mapper => mapper.Map<Patient>(dto))
            .Returns(mappedPatient);
        _patientRepository
            .Setup(repository => repository.Add(mappedPatient))
            .ReturnsAsync(mappedPatient);
    }

    private static PatientCreateDto CreateDto() => new()
    {
        FullName = "John Doe",
        DateOfBirth = new DateOnly(1998, 1, 1),
        Gender = "Male",
        PhoneNumber = "9876543210",
        Email = "patient@test.com",
        InsuranceId = "INS001"
    };

    private static Patient CreatePatient(int id) => new()
    {
        PatientId = id,
        FullName = "John Doe",
        DateOfBirth = new DateOnly(1998, 1, 1),
        Gender = "Male",
        PhoneNumber = "9876543210",
        Email = "patient@test.com",
        InsuranceId = "INS001"
    };
}
