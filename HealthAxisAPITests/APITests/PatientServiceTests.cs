using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using HealthAxisApplicn.Services.Impl;
using HealthAxisApplicn.Repositories;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Dto.Patients;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PatientServiceTests
{
    private readonly Mock<IPatientRepository> _repoMock;
    private readonly Mock<IHealthRecordRepository> _healthRecordRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly PatientService _service;

    public PatientServiceTests()
    {

        _repoMock = new Mock<IPatientRepository>();
        _healthRecordRepoMock = new Mock<IHealthRecordRepository>();
        _mapperMock = new Mock<IMapper>();

        _service = new PatientService(
            _repoMock.Object,
            _healthRecordRepoMock.Object,
            _mapperMock.Object
        );

    }

    [Fact]
    public async Task CreateAsync_Should_Create_Patient()
    {
        var dto = new CreatePatientDto { PatientName = "John" };
        var patient = new Patient { PatientName = "John" };
        var resultDto = new PatientDto { PatientName = "John" };

        _mapperMock.Setup(m => m.Map<Patient>(dto)).Returns(patient);
        _repoMock.Setup(r => r.CreateAsync(patient, default)).ReturnsAsync(patient);
        _mapperMock.Setup(m => m.Map<PatientDto>(patient)).Returns(resultDto);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.PatientName.Should().Be("John");
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_List()
    {
        var patients = new List<Patient> { new Patient { PatientId = 1 } };
        var patientDtos = new List<PatientDto> { new PatientDto { PatientId = 1 } };

        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(patients);
        _mapperMock.Setup(m => m.Map<List<PatientDto>>(patients)).Returns(patientDtos);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Patient()
    {
        var patient = new Patient { PatientId = 1 };
        var dto = new PatientDto { PatientId = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(patient);
        _mapperMock.Setup(m => m.Map<PatientDto>(patient)).Returns(dto);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.PatientId.Should().Be(1);
    }

    [Fact]
    public async Task DeactivatePatientAsync_Should_Deactivate_When_Active()
    {
        var patient = new Patient { PatientId = 1, IsActive = true };
        var updated = new Patient { PatientId = 1, IsActive = false };
        var dto = new PatientDto { PatientId = 1, IsActive = false };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(patient);
        _repoMock.Setup(r => r.UpdateAsync(1, patient, default)).ReturnsAsync(updated);
        _mapperMock.Setup(m => m.Map<PatientDto>(updated)).Returns(dto);

        var result = await _service.DeactivatePatientAsync(1);

        result.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeactivatePatientAsync_Should_Throw_When_NotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<System.Exception>(() => _service.DeactivatePatientAsync(1));
    }

    [Fact]
    public async Task ToggleActiveAsync_Should_Activate_When_Inactive()
    {
        var patient = new Patient
        {
            PatientId = 1,
            IsActive = false
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
                 .ReturnsAsync(patient);

        _repoMock.Setup(r => r.UpdateAsync(1, patient, default))
                 .ReturnsAsync(patient);

        _mapperMock.Setup(m => m.Map<PatientDto>(patient))
                   .Returns(new PatientDto { PatientId = 1, IsActive = true });

        var result = await _service.DeactivatePatientAsync(1);

        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_When_Found()
    {
        var existing = new Patient
        {
            PatientId = 1,
            PatientName = "Old",
            Email = "old@test.com"
        };

        var dto = new UpdatePatientDto
        {
            PatientName = "New",
            DateOfBirth = DateTime.Today.AddYears(-20),
            Gender = "Male",
            Email = "new@test.com",
            PhoneNo = "+919876543210"
        };

        var updated = new Patient
        {
            PatientId = 1,
            PatientName = "New",
            Email = "new@test.com"
        };

        var resultDto = new PatientDto
        {
            PatientId = 1,
            PatientName = "New",
            Email = "new@test.com"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
                 .ReturnsAsync(existing);

        _repoMock.Setup(r => r.SearchByEmailAsync(dto.Email, default))
                 .ReturnsAsync((Patient?)null); // ✅ no duplicate email

        _repoMock.Setup(r => r.UpdateAsync(1, existing, default))
                 .ReturnsAsync(updated);

        _mapperMock.Setup(m => m.Map<PatientDto>(updated))
                   .Returns(resultDto);

        var result = await _service.UpdateAsync(1, dto);

        result!.PatientName.Should().Be("New");
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_NotFound()
    {
        var dto = new UpdatePatientDto
        {
            PatientName = "New",
            DateOfBirth = DateTime.Today.AddYears(-20),
            Gender = "Male",
            Email = "new@test.com",
            PhoneNo = "+919876543210"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
                 .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, dto));
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Email_Already_Exists()
    {
        var existing = new Patient { PatientId = 1, Email = "old@test.com" };

        var dto = new UpdatePatientDto
        {
            PatientName = "New",
            DateOfBirth = DateTime.Today.AddYears(-20),
            Gender = "Male",
            Email = "duplicate@test.com",
            PhoneNo = "+919876543210"
        };

        var otherPatient = new Patient { PatientId = 2, Email = "duplicate@test.com" };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
                 .ReturnsAsync(existing);

        _repoMock.Setup(r => r.SearchByEmailAsync(dto.Email, default))
                 .ReturnsAsync(otherPatient); // ❌ duplicate found

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, dto));
    }

    [Fact]
    public async Task SearchByEmailAsync_Should_Return_Result()
    {
        var patient = new Patient { Email = "test@mail.com" };
        var dto = new PatientDto { Email = "test@mail.com" };

        _repoMock.Setup(r => r.SearchByEmailAsync("test@mail.com", default)).ReturnsAsync(patient);
        _mapperMock.Setup(m => m.Map<PatientDto>(patient)).Returns(dto);

        var result = await _service.SearchByEmailAsync("test@mail.com");

        result.Email.Should().Be("test@mail.com");
    }

    [Fact]
    public async Task SearchByPatientNameAsync_Should_Return_List()
    {
        var patients = new List<Patient> { new Patient { PatientName = "John" } };
        var dtos = new List<PatientDto> { new PatientDto { PatientName = "John" } };

        _repoMock.Setup(r => r.SearchByNameAsync("john", default)).ReturnsAsync(patients);
        _mapperMock.Setup(m => m.Map<List<PatientDto>>(patients)).Returns(dtos);

        var result = await _service.SearchByPatientNameAsync("john");

        result.Should().HaveCount(1);
    }
}